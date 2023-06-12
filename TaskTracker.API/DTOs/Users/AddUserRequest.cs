using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.DTOs.Users
{
    public class AddUserRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
