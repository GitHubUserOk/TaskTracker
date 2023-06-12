using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.DTOs.Tasks
{
    public class UpdateTaskRequest
    {
        [Required]
        public bool IsMarked { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
    }
}
