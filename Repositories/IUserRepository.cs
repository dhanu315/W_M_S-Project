using System.Collections.Generic;
using System.Threading.Tasks;
using W_M_S_Project.Models;

namespace W_M_S_Project.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<(IEnumerable<User> Items, int TotalCount)> GetPaginatedUsersAsync(int pageNumber, int pageSize);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(string email);

        // Password Reset related methods
        Task SavePasswordResetTokenAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetPasswordResetTokenAsync(string token);
        Task UpdatePasswordResetTokenAsync(PasswordResetToken token);
    }
}
