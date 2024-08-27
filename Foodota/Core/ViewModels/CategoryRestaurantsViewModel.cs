using Foodota.Areas.Admin.Models;

namespace Foodota.Core.ViewModels;

public class CategoryRestaurantsViewModel
{
    public int SelectedCategory { get; set; }
    public ICollection<Category> AllCategories { get; set; }=new List<Category>();
    public ICollection<Restaurant> CategoryRestaurants { get; set; }=new List<Restaurant>();
}
