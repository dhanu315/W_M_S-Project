using System.Collections.Generic;
using System.Threading.Tasks;
using W_M_S_Project.DTOs;
using W_M_S_Project.Helpers;

namespace W_M_S_Project.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<PaginatedResult<UserResponseDto>> GetPaginatedUsersAsync(int pageNumber, int pageSize);
        Task<UserResponseDto?> GetUserByIdAsync(int id);
        Task<UserResponseDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(int id);
    }
}
