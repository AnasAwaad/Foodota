using Foodota.Areas.Admin.Data;


namespace Foodota.Controllers;
public class RestaurantController : Controller
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;

	public RestaurantController(ApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}
	public IActionResult Index(int id)
	{
		var allCategories = _context.Categories
			.Include(c => c.RestaurantCategories)
			.Where(c=>c.RestaurantCategories.Any())
			.ToList();

		var categoryRestaurants = _context.Restaurants
			.Include(r => r.RestaurantCategories) 
				.ThenInclude(rc => rc.Category) 
			.Include(r => r.OpeningHours) 
				.ThenInclude(o=>o.WeekDay)
			.Where(r => r.RestaurantCategories.Any(rc => rc.CategoryId == id))
			.ToList();


		CategoryRestaurantsViewModel model = new()
		{
			SelectedCategory=id,
			AllCategories = allCategories,
			CategoryRestaurants = categoryRestaurants
		};

		return View(model);
	}

	public IActionResult Details(int id)
	{
		var restaurant = _context.Restaurants
			.Include(r=>r.MenuItems)
			.Include(r => r.OpeningHours)
				.ThenInclude(o => o.WeekDay)
			.Where(r => r.Id == id)
			.FirstOrDefault();

		if (restaurant is null)
			return NotFound();
		var model = _mapper.Map<RestaurantDetailsViewModel>(restaurant);

		model.Categories = _context.Categories
			.Include(c => c.RestaurantCategories)
				.ThenInclude(r => r.Restaurant)
			.Where(c => c.RestaurantCategories.Any(rc => rc.RestaurantId == id ))
			.ToList();

		model.WeekDays = _context.WeekDays.Select(w=>w.Name).ToList();
		model.MenuItems = restaurant.MenuItems;
		return View(model);
	}

	public IActionResult GetFilteredData(List<int> CategoryIds)
	{
		var restautants = _context.Restaurants
			.Include(r => r.RestaurantCategories)
				.ThenInclude(rc => rc.Category)
			.Include(r => r.OpeningHours)
				.ThenInclude(o=>o.WeekDay)
			.Where(r => r.RestaurantCategories.Any(rc => CategoryIds.Contains(rc.CategoryId)))
			.ToList();
		return PartialView("_Restaurants", restautants);
	}

	[HttpPost]
	public IActionResult GetCategoryItems([FromBody] RestaurantMenuItemsRequest model)
	{
		// When categoryId is 0, retrieve all items for the given restaurant
		var items = model.categoryId == 0
			? _context.MenuItems.Include(m => m.Category).Where(m => m.RestaurantId == model.restaurantId).ToList()
			: _context.MenuItems.Include(m => m.Category).Where(i => i.CategoryId == model.categoryId && i.RestaurantId == model.restaurantId).ToList();

		return PartialView("_MenuItemsList", items);
	}

}
