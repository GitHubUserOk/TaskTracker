using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Domain.Entities;

public class Task
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public bool IsMarked { get; set; }
    [MaxLength(100)]
    public string Title { get; set; } = null!;
    [MaxLength]
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public int AuthorId { get; set; }
    public int CounterpartyId { get; set; }
    public int ProjectId { get; set; }
    public int BasisTaskId { get; set; }
}
