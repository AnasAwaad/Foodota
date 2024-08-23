namespace Foodota.Areas.Admin.Models;

public class RestaurantCategory
{
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public int RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }
}
