namespace Foodota.Core.ViewModels;

public class MenuItemViewModel
{
	public int? Id { get; set; }
	public string Name { get; set; } = null!;
	public string? ImagePath { get; set; } = null!;
	public string Description { get; set; } = null!;
	public int SellingPrice { get; set; }
	public bool IsActive { get; set; }
	public DateTime CreatedOn { get; set; }
}
