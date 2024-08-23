using System.ComponentModel.DataAnnotations;

namespace Foodota.Areas.Admin.Models;

public class MenuItem : BaseModel
{
    public int Id { get; set; }
    [StringLength(100)]
    public string Name { get; set; } = null!;
    public int SellingPrice { get; set; }
    public int MainPrice { get; set; }
    public string Description { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
    public int RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
