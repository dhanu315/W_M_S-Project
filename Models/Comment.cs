using System;
using System.ComponentModel.DataAnnotations;

namespace W_M_S_Project.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TaskId { get; set; }
        public TaskEntity Task { get; set; } = null!;

        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
