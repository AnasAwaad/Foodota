using Foodota.Areas.Admin.Models;

namespace Foodota.Core.ViewModels;

public class HomeViewModel
{
    public ICollection<Category>? Categories { get; set; }
}
