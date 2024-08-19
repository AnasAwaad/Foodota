using AutoMapper;
using Foodota.Core.Models;
using Foodota.Core.ViewModels;
using Foodota.Data;
using Foodota.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Foodota.Controllers;
public class RestaurantController : Controller
{
	private readonly IImageService _imageService;
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;

	public RestaurantController(IImageService imageService, ApplicationDbContext context, IMapper mapper)
	{
		_imageService = imageService;
		_context = context;
		_mapper = mapper;
	}

	public IActionResult Index()
	{
		return View();
	}

	[HttpGet]
	public IActionResult Create()
	{
		return View("Form");
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Create(RestaurantFormViewModel viewModel)
	{
		if (!ModelState.IsValid) return View("Form",viewModel);

		var imageName = $"{Guid.NewGuid()}{Path.GetExtension(viewModel.ImageUrl.FileName)}";
		var logoName = $"{Guid.NewGuid()}{Path.GetExtension(viewModel.Logo.FileName)}";

		var logoRes = _imageService.UploadImage(viewModel.Logo, logoName, "images/restaurant/logo", false);

		if (!logoRes.isUploaded)
		{
			ModelState.AddModelError("Logo", logoRes.errorMessage!);
			return View("Form", viewModel);
		}

		var result=_imageService.UploadImage(viewModel.ImageUrl, imageName, "images/restaurant/banner", false);

		if(!result.isUploaded)
		{
			ModelState.AddModelError("ImageUrl", result.errorMessage!);
			return View("Form", viewModel) ;
		}
		var model = _mapper.Map<Restaurant>(viewModel);
		model.ImageUrl = imageName;
		model.Logo = logoName;

		_context.Restaurants.Add(model);
		_context.SaveChanges();

		return RedirectToAction(nameof(Index));
	}
}
