using System;
using System.Collections.Generic;

namespace W_M_S_Project.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? ProfilePhotoUrl { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        
        public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
    }
}
