using Foodota.Areas.Admin.Models;

namespace Foodota.Core.ViewModels;

public class HomeRestaurantViewModel
{
	public ICollection<Category>? Categories { get; set; }
	public ICollection<Restaurant>? Restaurants { get; set; }
}
