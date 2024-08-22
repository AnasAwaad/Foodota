using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Foodota.Core.ViewModels;

public class MenuItemFormViewModel
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;

	[Display(Name="Selling Price")]
	public int SellingPrice { get; set; }

	[Display(Name = "Main Price")]
	public int MainPrice { get; set; }
	public string Description { get; set; } = null!;
	public IFormFile? Image { get; set; } = null!;
	public string? ImagePath { get; set; } = null!;
	public int RestaurantId { get; set; }
	public int CategoryId { get; set; }
	public IList<SelectListItem>? Categories { get; set; } = new List<SelectListItem>();
	public IList<SelectListItem>? Restaurants { get; set; } = new List<SelectListItem>();

}
