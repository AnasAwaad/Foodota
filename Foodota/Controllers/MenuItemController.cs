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

		return Ok(model.Id);
	}

}
