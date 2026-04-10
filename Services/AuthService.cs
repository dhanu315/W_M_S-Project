using System;
using System.Threading.Tasks;
using W_M_S_Project.DTOs;
using W_M_S_Project.Helpers;
using W_M_S_Project.Models;
using W_M_S_Project.Repositories;

namespace W_M_S_Project.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtHelper _jwtHelper;
        private readonly PasswordHasher _passwordHasher;
        private readonly IEmailService _emailService; // ? Fixed

        public AuthService(
            IUserRepository userRepository,
            JwtHelper jwtHelper,
            PasswordHasher passwordHasher,
            IEmailService emailService) // ? Fixed
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task<string?> RegisterAsync(RegisterDto dto)
        {
            if (await _userRepository.UserExistsAsync(dto.Email))
                return null;

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                RoleId = 2
            };

            await _userRepository.AddUserAsync(user);

            var createdUser = await _userRepository.GetUserByEmailAsync(user.Email);
            return _jwtHelper.GenerateToken(createdUser!);
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null || !_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
                return null;

            return _jwtHelper.GenerateToken(user);
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return false;

            var token = Guid.NewGuid().ToString();
            var resetToken = new PasswordResetToken
            {
                Email = email,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            await _userRepository.SavePasswordResetTokenAsync(resetToken);
            await _emailService.SendEmailAsync(email, "Password Reset", $"Your reset token is: {token}");
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var resetToken = await _userRepository.GetPasswordResetTokenAsync(dto.Token);
            if (resetToken == null || resetToken.IsUsed || resetToken.ExpiresAt < DateTime.UtcNow)
                return false;

            var user = await _userRepository.GetUserByEmailAsync(resetToken.Email);
            if (user == null) return false;

            user.PasswordHash = _passwordHasher.HashPassword(dto.NewPassword);
            await _userRepository.UpdateUserAsync(user);

            resetToken.IsUsed = true;
            await _userRepository.UpdatePasswordResetTokenAsync(resetToken);
            return true;
        }
    }
}