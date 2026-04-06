using System.Collections.Generic;

namespace W_M_S_Project.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., Admin, User

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
