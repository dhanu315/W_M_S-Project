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
        Task<(System.Collections.Generic.IEnumerable<UserResponseDto> Users, int TotalCount)> GetUsersAsync(int page, int limit, string? search);
        Task<UserResponseDto?> GetUserResponseByIdAsync(int id);
        Task<UserResponseDto?> CreateUserAsync(CreateUserDto dto);
        Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto dto);
        Task<bool> DeleteUserAsync(int id);
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

        private UserResponseDto MapToResponse(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role?.Name ?? string.Empty,
                Status = user.Status,
                ProfilePhotoUrl = user.ProfilePhotoUrl,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        public async Task<(System.Collections.Generic.IEnumerable<UserResponseDto> Users, int TotalCount)> GetUsersAsync(int page, int limit, string? search)
        {
            var (users, totalCount) = await _userRepository.GetUsersAsync(page, limit, search);
            var dtos = System.Linq.Enumerable.Select(users, MapToResponse);
            return (dtos, totalCount);
        }

        public async Task<UserResponseDto?> GetUserResponseByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;
            return MapToResponse(user);
        }

        public async Task<UserResponseDto?> CreateUserAsync(CreateUserDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser != null) return null;

            var role = await _userRepository.GetRoleByNameAsync(dto.Role);
            if (role == null) return null;

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                RoleId = role.Id,
                Status = dto.Status,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow
            };

            await _userRepository.CreateUserAsync(user);
            
            var createdUser = await _userRepository.GetUserByIdAsync(user.Id);
            return createdUser != null ? MapToResponse(createdUser) : null;
        }

        public async Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;

            if (!string.IsNullOrEmpty(dto.Name)) user.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email) 
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
                if (existingUser != null && existingUser.Id != id) return null; // Email taken
                user.Email = dto.Email;
            }
            if (!string.IsNullOrEmpty(dto.Role))
            {
                var role = await _userRepository.GetRoleByNameAsync(dto.Role);
                if (role == null) return null;
                user.RoleId = role.Id;
            }
            if (!string.IsNullOrEmpty(dto.Status)) user.Status = dto.Status;

            user.UpdatedAt = System.DateTime.UtcNow;

            await _userRepository.UpdateUserAsync(user);

            var updatedUser = await _userRepository.GetUserByIdAsync(user.Id);
            return updatedUser != null ? MapToResponse(updatedUser) : null;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteUserAsync(user);
            return true;
        }
    }
}
