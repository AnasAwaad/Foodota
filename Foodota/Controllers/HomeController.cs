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

	public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
	{
		_logger = logger;
		_context = context;
	}

	public IActionResult Index()
    {
        var categories=_context.Categories.Include(c=>c.RestaurantCategories).ToList();
        var restaurants = _context.Restaurants
            .Include(r=>r.OpeningHours)
            .Include(r=>r.RestaurantCategories)
            .ThenInclude(rc=>rc.Category)
            .Take(9)
            .ToList();
        HomeViewModel model = new HomeViewModel
        {
            Categories = categories,
            Restaurants=restaurants
        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
