namespace W_M_S_Project.DTOs
{
    public class UpdateProfileDto
    {
        public string? Name { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ProfilePhotoUrl { get; set; }
    }
}
