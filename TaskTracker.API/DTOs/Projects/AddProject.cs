using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.DTOs.Projects;
public class AddProjectRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    [Required]
    public int CounterpartyId { get; set; }
}
