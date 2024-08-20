using AutoMapper;
using Foodota.Core.Models;
using Foodota.Core.ViewModels;
using Foodota.Data;
using Foodota.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

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
		var viewModel = new RestaurantFormViewModel()
		{
			weekDays = _context.WeekDays.ToList()
		};
		return View("Form",viewModel);
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
		model.ImageUrl = "/images/restaurant/banner/"+imageName;
		model.Logo = "/images/restaurant/logo/" + logoName;

		_context.Restaurants.Add(model);
		_context.SaveChanges();

		return Ok(model.Id);
	}

	[HttpPost]
	public IActionResult AddOpeningHours([FromBody]OpeningHoursRequest request)
	{
		if(request.OpeningHours is null){
			return BadRequest();
		}
		_context.OpeningHours.AddRange(request.OpeningHours);
		_context.SaveChanges();
		return Ok(Json("added successfully"));
	}

	[HttpPost]
	public IActionResult GetRestaurants()
	{
		var skip = Convert.ToInt32(Request.Form["start"]);
		var pageSize = Convert.ToInt32(Request.Form["length"]);
		var orderColumnIndex = Convert.ToInt32(Request.Form["order[0][column]"]);
		var orderColumnName = Request.Form[$"columns[{orderColumnIndex}][name]"];
		var orderColumnDirection = Request.Form["order[0][dir]"];
		var searchValue = Request.Form["search[value]"];


		IQueryable<Restaurant> restaurants = _context.Restaurants;

		if (!string.IsNullOrEmpty(searchValue))
			restaurants = restaurants.Where(b => b.Name.Contains(searchValue!) || b.Description.Contains(searchValue!));

		restaurants = restaurants.OrderBy($"{orderColumnName} {orderColumnDirection}");  //orderBy from system.Linq.Dynamic lib

		var data = restaurants.Skip(skip).Take(pageSize).ToList();
		var restaurantVM = _mapper.Map<IEnumerable<RestaurantViewModel>>(data);
		var recordsTotal = restaurants.Count();

		return Json(new { recordsFiltered = recordsTotal, recordsTotal, data = restaurantVM });
	}
}
