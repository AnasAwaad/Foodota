using System.ComponentModel.DataAnnotations;

namespace Foodota.Core.ViewModels;

public class RestaurantViewModel
{
	public int? Id { get; set; }
	public string Name { get; set; } = null!;
	public string Description { get; set; } = null!;
	public string Logo { get; set; } = null!;
	public string Address { get; set; } = null!;
	public bool IsActive { get; set; }
	public DateTime CreatedOn { get; set; }
}
