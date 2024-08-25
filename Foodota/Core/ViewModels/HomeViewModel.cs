using Foodota.Areas.Admin.Models;

namespace Foodota.Core.ViewModels;

public class HomeViewModel
{
    public IEnumerable<Category>? Categories { get; set; }
    public IEnumerable<Restaurant>? Restaurants { get; set; }
}
