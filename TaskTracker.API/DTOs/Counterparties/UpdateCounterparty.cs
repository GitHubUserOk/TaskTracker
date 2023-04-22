using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.DTOs.Counterparties
{
    public class UpdateCounterpartyRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool IsMarked { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
