using Foodota.Areas.Admin.Models;
using System.ComponentModel.DataAnnotations;

namespace Foodota.Core.ViewModels;

public class RestaurantDetailsViewModel
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	public string Description { get; set; } = null!;
	public string ImagePath { get; set; } = null!;
	public string LogoPath { get; set; } = null!;
	public string Address { get; set; } = null!;
	public IList<OpeningHour> OpeningHours { get; set; } = new List<OpeningHour>();
	public ICollection<RestaurantCategory> RestaurantCategories { get; set; } = new List<RestaurantCategory>();
	public IList<string> WeekDays { get; set; } = new List<string>();

}
