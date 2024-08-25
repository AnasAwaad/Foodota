using Foodota.Areas.Admin.Data;
using Foodota.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Foodota.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

	public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IMapper mapper)
	{
		_logger = logger;
		_context = context;
		_mapper = mapper;
	}

	public IActionResult Index()
    {
        var categories=_context.Categories
            .Include(c => c.RestaurantCategories)
			.Where(c => c.RestaurantCategories.Any())
            .Take(6)
			.ToList();
		
		var restaurants = _context.Restaurants
            .Include(r=>r.OpeningHours)
                .ThenInclude(o=>o.WeekDay)
            .Include(r=>r.RestaurantCategories)
                .ThenInclude(rc=>rc.Category)
            .Take(9)
            .ToList();

        //var viewModel = _mapper.Map<IEnumerable<RestaurantOpeningHourViewModel>>(restaurants);

        HomeViewModel model = new HomeViewModel
        {
            Categories = categories,
            Restaurants = restaurants
        };
        return View(model);
        //return Ok();
    }
}
