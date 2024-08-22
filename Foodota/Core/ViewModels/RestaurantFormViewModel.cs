using Foodota.Core.Models;
using Foodota.Web.Core.Consts;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Foodota.Core.ViewModels;

public class RestaurantFormViewModel
{
	public int? Id { get; set; }
	public string Name { get; set; } = null!;
	public string Description { get; set; } = null!;
	[Display(Name="Image")]

	[RequiredIf("Id==null",ErrorMessage =Errors.RequiredField)]
	public IFormFile? Image { get; set; } = null!;
	public string? ImagePath { get; set; }

	[RequiredIf("Id==null", ErrorMessage = Errors.RequiredField)]
	public IFormFile? Logo { get; set; } = null!;
	public string? LogoPath { get; set; } = null!;
	public string Address { get; set; } = null!;

	[ValidateNever]
	public IList<WeekDay> weekDays { get; set; }=new List<WeekDay>();
	public IList<OpeningHour>? OpeningHours { get; set; }=new List<OpeningHour>();
	public IList<SelectListItem>? Categories { get; set; }=new List<SelectListItem>();

	[Display(Name ="Categories")]
	public IList<int> SelectedCategories { get; set; }=new List<int>();
}
