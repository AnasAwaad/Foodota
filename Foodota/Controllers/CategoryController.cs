using Microsoft.EntityFrameworkCore;

namespace Foodota.Controllers;
public class CategoryController : Controller
{
	private readonly IImageService _imageService;
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;

	public CategoryController(IImageService imageService, ApplicationDbContext context, IMapper mapper)
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
		return PartialView("_Form");
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Create(CategoryFormViewModel viewModel)
	{
		if (!ModelState.IsValid) return BadRequest();

		var imageName = $"{Guid.NewGuid()}{Path.GetExtension(viewModel.Image!.FileName)}";

		var result = _imageService.UploadImage(viewModel.Image, imageName, "images/category", false);

		if (!result.isUploaded)
		{
			ModelState.AddModelError("Image", result.errorMessage!);
			return View("Form", viewModel);
		}
		var model = _mapper.Map<Category>(viewModel);
		model.ImagePath = "/images/category/" + imageName;
		_context.Categories.Add(model);
		_context.SaveChanges();

		return Ok();
	}

	[HttpGet]
	public IActionResult Update(int id)
	{
		var category = _context.Categories.Find(id);
		if (category == null)
			return NotFound();

		var viewModel = _mapper.Map<CategoryFormViewModel>(category);

		return PartialView("_Form", viewModel);
	}


	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Update(CategoryFormViewModel viewModel)
	{
		if (!ModelState.IsValid) return BadRequest();

		var category = _context.Categories.Find(viewModel.Id);
		if (category is null)
			return NotFound();

		if (viewModel.Image is not null)
		{
			var imageName = $"{Guid.NewGuid()}{Path.GetExtension(viewModel.Image.FileName)}";


			_imageService.DeleteImage(category.ImagePath!);
			var res = _imageService.UploadImage(viewModel.Image, imageName, "images/category", false);
			if (!res.isUploaded)
			{
				ModelState.AddModelError("Image", res.errorMessage!);
				return View("Update", viewModel);
			}
			viewModel.ImagePath = "/images/category/" + imageName;
		}
		else
			viewModel.ImagePath = category.ImagePath;

		category = _mapper.Map(viewModel, category);

		_context.Categories.Update(category);
		_context.SaveChanges();
		return Ok();

	}



	[HttpPost]
	public IActionResult GetCategories()
	{
		var skip = Convert.ToInt32(Request.Form["start"]);
		var pageSize = Convert.ToInt32(Request.Form["length"]);
		var orderColumnIndex = Convert.ToInt32(Request.Form["order[0][column]"]);
		var orderColumnName = Request.Form[$"columns[{orderColumnIndex}][name]"];
		var orderColumnDirection = Request.Form["order[0][dir]"];
		var searchValue = Request.Form["search[value]"];


		IQueryable<Category> categories = _context.Categories;

		if (!string.IsNullOrEmpty(searchValue))
			categories = categories.Where(b => b.Name.Contains(searchValue!));

		categories = categories.OrderBy($"{orderColumnName} {orderColumnDirection}");  //orderBy from system.Linq.Dynamic lib

		var data = categories.Skip(skip).Take(pageSize).ToList();
		var categoryVM = _mapper.Map<IEnumerable<CategoryViewModel>>(data);
		var recordsTotal = categories.Count();

		return Json(new { recordsFiltered = recordsTotal, recordsTotal, data = categoryVM });
	}
}
