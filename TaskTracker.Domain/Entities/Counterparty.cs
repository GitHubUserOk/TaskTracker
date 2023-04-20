using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Domain.Entities;

public class Counterparty
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public bool IsMarked { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public List<string> AvailableFields()
    {
        var list = new List<string>();

        Type type = typeof(Counterparty);

        foreach (var prop in type.GetProperties())
        {
            if (prop.Name == "Id") continue;

            list.Add(prop.Name);
        }

        return list;
    }

    public void FillPropertyValues(Counterparty counterparty)
    {
        var propNames = AvailableFields();

        foreach (var name in propNames)
        {
            var field = this.GetType().GetProperty(name);
            var value = counterparty.GetType().GetProperty(name).GetValue(counterparty);
            field.SetValue(this, value);
        }
    }
}
