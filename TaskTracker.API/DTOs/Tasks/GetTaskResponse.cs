using TaskTracker.API.DTOs.Users;
using TaskTracker.Domain.Entities;

namespace TaskTracker.API.DTOs.Tasks
{
    public class GetTaskResponse
    {
        public int Id { get; set; }
        public bool IsMarked { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public int CounterpartyId { get; set; }
        public string? CounterpartyName { get; set; }
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public int? BasisTaskId { get; set; }
        public string? BasisTaskTitle { get; set; }
        public IEnumerable<User>? Performers { get; set; }
    }
}
