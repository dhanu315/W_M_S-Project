using System;
using System.Threading.Tasks;
using W_M_S_Project.DTOs;
using W_M_S_Project.Models;
using W_M_S_Project.Repositories;
using W_M_S_Project.Helpers;

namespace W_M_S_Project.Services
{
    public interface IAuthService
    {
        Task<string?> RegisterAsync(RegisterDto dto);
        Task<string?> LoginAsync(LoginDto dto);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtHelper _jwtHelper;
        private readonly PasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;

        public AuthService(IUserRepository userRepository, JwtHelper jwtHelper, PasswordHasher passwordHasher, IEmailService emailService)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task<string?> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser != null)
                return null; // User exists

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                RoleId = 2 // Default Role (User)
            };

            await _userRepository.CreateUserAsync(user);

            // Fetch created user with Role included
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
                UserId = user.Id,
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddHours(1),
                IsUsed = false
            };

            await _userRepository.SavePasswordResetTokenAsync(resetToken);

            var resetLink = $"http://localhost:5000/api/password/reset?token={token}";
            await _emailService.SendEmailAsync(user.Email, "Reset Password", $"Click this link to reset your password: {resetLink}");

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var resetToken = await _userRepository.GetPasswordResetTokenAsync(dto.Token);
            if (resetToken == null || resetToken.IsUsed || resetToken.ExpirationDate < DateTime.UtcNow)
                return false;

            resetToken.User.PasswordHash = _passwordHasher.HashPassword(dto.NewPassword);
            resetToken.IsUsed = true;

            await _userRepository.UpdateUserAsync(resetToken.User);
            await _userRepository.UpdatePasswordResetTokenAsync(resetToken);

            return true;
        }
    }
}
