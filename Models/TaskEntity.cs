using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace W_M_S_Project.Models
{
    public class TaskEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public string Status { get; set; } = "Todo"; // Todo, InProgress, Done

        public int AssignedToUserId { get; set; }
        public User AssignedToUser { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
