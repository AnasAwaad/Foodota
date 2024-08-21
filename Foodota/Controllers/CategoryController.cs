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
