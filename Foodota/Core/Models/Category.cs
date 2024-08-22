using System.ComponentModel.DataAnnotations;

namespace Foodota.Core.Models;

public class Category : BaseModel
{
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

	[StringLength(500)]
	public string ImagePath { get; set; } = null!;
    public ICollection<RestaurantCategory> RestaurantCategories { get; set; } = new List<RestaurantCategory>();
}
