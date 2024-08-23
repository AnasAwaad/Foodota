using System.ComponentModel.DataAnnotations;

namespace Foodota.Areas.Admin.Models;

public class WeekDay
{
    public int Id { get; set; }
    [StringLength(25)]
    public string Name { get; set; } = null!;
    public ICollection<OpeningHour> OpeningHours { get; set; } = new List<OpeningHour>();
}
