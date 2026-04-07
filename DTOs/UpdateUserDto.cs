using System.ComponentModel.DataAnnotations;

namespace W_M_S_Project.DTOs
{
    public class UpdateUserDto
    {
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Role { get; set; }

        public string? Status { get; set; }
    }
}
