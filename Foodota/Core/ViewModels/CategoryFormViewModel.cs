using Foodota.Web.Core.Consts;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Foodota.Core.ViewModels;

public class CategoryFormViewModel
{
	public int? Id { get; set; }
	public string Name { get; set; } = null!;

	[Display(Name = "Image")]
	public IFormFile? Image { get; set; } = null!;
	public string? ImagePath { get; set; }
}
