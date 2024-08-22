using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Foodota.Controllers;
public class MenuItemController : Controller
{
	private readonly IImageService _imageService;
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;

	public MenuItemController(IImageService imageService, ApplicationDbContext context, IMapper mapper)
	{
		_imageService = imageService;
		_context = context;
		_mapper = mapper;
	}


	public IActionResult Index()
	{
		return View();
	}

	public IActionResult Create()
	{
		MenuItemFormViewModel viewModel = new()
		{
			Categories = _context.Categories.Select(c => new SelectListItem()
			{
				Text = c.Name,
				Value = c.Id.ToString()
			}).ToList(),
			Restaurants = _context.Restaurants.Select(r => new SelectListItem()
			{
				Text = r.Name,
				Value = r.Id.ToString()
			}).ToList()
		};

		return View(viewModel);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Create(MenuItemFormViewModel viewModel)
	{
		if (!ModelState.IsValid) return View(viewModel);

		var imageName = $"{Guid.NewGuid()}{Path.GetExtension(viewModel.Image!.FileName)}";

		var result = _imageService.UploadImage(viewModel.Image, imageName, "images/menuItem", false);

		if (!result.isUploaded)
		{
			ModelState.AddModelError("Image", result.errorMessage!);
			return View("Form", viewModel);
		}

		var model = _mapper.Map<MenuItem>(viewModel);
		model.ImagePath = "/images/menuItem/" + imageName;
		model.IsActive = true;
		model.CreatedOn = DateTime.Now;

		_context.MenuItems.Add(model);
		_context.SaveChanges();

		return RedirectToAction(nameof(Index));
	}


	[HttpPost]
	public IActionResult GetItems()
	{
		var skip = Convert.ToInt32(Request.Form["start"]);
		var pageSize = Convert.ToInt32(Request.Form["length"]);
		var orderColumnIndex = Convert.ToInt32(Request.Form["order[0][column]"]);
		var orderColumnName = Request.Form[$"columns[{orderColumnIndex}][name]"];
		var orderColumnDirection = Request.Form["order[0][dir]"];
		var searchValue = Request.Form["search[value]"];


		IQueryable<MenuItem> menuItems = _context.MenuItems;

		if (!string.IsNullOrEmpty(searchValue))
			menuItems = menuItems.Where(b => b.Name.Contains(searchValue!) || b.Description.Contains(searchValue!));

		menuItems = menuItems.OrderBy($"{orderColumnName} {orderColumnDirection}");  //orderBy from system.Linq.Dynamic lib

		var data = menuItems.Skip(skip).Take(pageSize).ToList();

		var menuItemVM = _mapper.Map<IEnumerable<MenuItemViewModel>>(data);
		var recordsTotal = menuItems.Count();

		return Json(new { recordsFiltered = recordsTotal, recordsTotal, data = menuItemVM });
	}
}
