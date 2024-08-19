using System.ComponentModel.DataAnnotations;

namespace Foodota.Core.Models;

public class WeekDay
{
    public int Id { get; set; }
    [StringLength(25)]
    public int Name { get; set; }
    public ICollection<OpeningHour> OpeningHours { get; set; } = new List<OpeningHour>();
}
