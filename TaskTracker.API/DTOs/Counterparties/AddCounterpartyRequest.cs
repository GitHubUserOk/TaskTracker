using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.DTOs.Counterparties;
public class AddCounterpartyRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
}