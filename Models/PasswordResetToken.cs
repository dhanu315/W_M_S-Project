using System;

namespace W_M_S_Project.Models
{
    public class PasswordResetToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; } // Mark as used once the password is reset

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
