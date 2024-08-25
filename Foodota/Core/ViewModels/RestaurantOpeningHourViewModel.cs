using Foodota.Areas.Admin.Models;

namespace Foodota.Core.ViewModels;

public class RestaurantOpeningHourViewModel
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	public string Description { get; set; } = null!;
	public string ImagePath { get; set; } = null!;
	public string LogoPath { get; set; } = null!;
	public string Address { get; set; } = null!;
	public string From { get; set; }
	public string To { get; set; }

	public ICollection<RestaurantCategory> RestaurantCategories { get; set; } = new List<RestaurantCategory>();
}
