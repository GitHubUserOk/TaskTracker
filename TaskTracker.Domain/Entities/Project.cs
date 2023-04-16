using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Domain.Entities;
public class Project
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public Counterparty Counterparty { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
}

