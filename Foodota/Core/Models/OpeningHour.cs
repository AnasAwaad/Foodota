namespace Foodota.Core.Models;

public class OpeningHour
{
    public int Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public int WeekDayId { get; set; }
    public WeekDay WeekDay { get; set; } = null!;
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
}
