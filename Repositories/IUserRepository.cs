using System.Threading.Tasks;
using W_M_S_Project.Models;

namespace W_M_S_Project.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task SavePasswordResetTokenAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetPasswordResetTokenAsync(string token);
        Task UpdatePasswordResetTokenAsync(PasswordResetToken token);
    }
}
