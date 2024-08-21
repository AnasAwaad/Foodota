using System.ComponentModel.DataAnnotations;

namespace Foodota.Core.ViewModels;

public class CategoryViewModel
{
	public int Id { get; set; }

	[StringLength(100)]
	public string Name { get; set; } = null!;

	[StringLength(500)]
	public string ImagePath { get; set; } = null!;
	public bool IsActive { get; set; }
	public DateTime CreatedOn { get; set; }
}
