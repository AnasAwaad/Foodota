using Foodota.Areas.Admin.Models;

namespace Foodota.Core.ViewModels;

public class ShoppingCartViewModel
{
	public int Id { get; set; }
	public int Count { get; set; }
    public int TotalPrice { get; set; }
    public MenuItem MenuItem { get; set; }


}
