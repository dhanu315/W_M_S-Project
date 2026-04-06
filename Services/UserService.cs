using System.Threading.Tasks;
using W_M_S_Project.DTOs;
using W_M_S_Project.Models;
using W_M_S_Project.Repositories;
using W_M_S_Project.Helpers;

namespace W_M_S_Project.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<bool> UpdateProfileAsync(int userId, UpdateProfileDto dto);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, PasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<bool> UpdateProfileAsync(int userId, UpdateProfileDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return false;

            if (!string.IsNullOrEmpty(dto.Name))
            {
                user.Name = dto.Name;
            }

            if (!string.IsNullOrEmpty(dto.ProfilePhotoUrl))
            {
                user.ProfilePhotoUrl = dto.ProfilePhotoUrl;
            }

            if (!string.IsNullOrEmpty(dto.OldPassword) && !string.IsNullOrEmpty(dto.NewPassword))
            {
                if (!_passwordHasher.VerifyPassword(dto.OldPassword, user.PasswordHash))
                {
                    return false; // Old password doesn't match
                }
                user.PasswordHash = _passwordHasher.HashPassword(dto.NewPassword);
            }

            await _userRepository.UpdateUserAsync(user);
            return true;
        }
    }
}
