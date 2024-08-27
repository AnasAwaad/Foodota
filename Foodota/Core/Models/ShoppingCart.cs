using Foodota.Areas.Admin.Models;
using System.ComponentModel.DataAnnotations;

namespace Foodota.Core.Models;

public class ShoppingCart
{
    public int Id { get; set; }
    [Range(1,100)]
    public int Count { get; set; }
    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; }
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}
