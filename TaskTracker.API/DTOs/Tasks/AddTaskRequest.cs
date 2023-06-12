using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.DTOs.Tasks
{
    public class AddTaskRequest
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        public int AuthorId { get; set; }
        [Required]
        public int CounterpartyId { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public int? BasisTaskId { get; set; }
    }
}
