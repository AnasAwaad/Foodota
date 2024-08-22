using System.ComponentModel.DataAnnotations;

namespace Foodota.Core.Models;

public class Restaurant : BaseModel
{
    public int Id { get; set; }
    [StringLength(100)]
    public string Name { get; set; } = null!;
	[StringLength(200)]
	public string Description { get; set; } = null!;
    [StringLength(500)]
    public string ImagePath { get; set; } = null!;
	[StringLength(500)]
	public string LogoPath { get; set; }=null!;
	[StringLength(100)]
	public string Address { get; set; }=null!;
    public ICollection<OpeningHour> OpeningHours { get; set; } = new List<OpeningHour>();
    public ICollection<RestaurantCategory> RestaurantCategories { get; set; } = new List<RestaurantCategory>();
    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
