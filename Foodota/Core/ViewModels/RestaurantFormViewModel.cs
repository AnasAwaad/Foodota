using Foodota.Core.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Foodota.Core.ViewModels;

public class RestaurantFormViewModel
{
	public int? Id { get; set; }
	public string Name { get; set; } = null!;
	public string Description { get; set; } = null!;
	[Display(Name="Image")]
	public IFormFile ImageUrl { get; set; } = null!;
	public IFormFile Logo { get; set; } = null!;
	public string Address { get; set; } = null!;

	[ValidateNever]
	public IList<WeekDay> weekDays { get; set; }=new List<WeekDay>();
	public ICollection<OpeningHour>? OpeningHours { get; set; }=new List<OpeningHour>();
}
