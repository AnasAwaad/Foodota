using Foodota.Areas.Admin.Data;
using Foodota.Areas.Admin.Models;
using Foodota.Core.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Foodota.Areas.Admin.Controllers;

[Authorize(Roles = AppRoles.Admin)]
[Area("Admin")]

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
            weekDays = _context.WeekDays.ToList(),
            Categories = _context.Categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = Convert.ToString(c.Id)
            }).ToList()
        };
        return View("Form", viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(RestaurantFormViewModel viewModel)
    {
        if (!ModelState.IsValid) return View("Form", viewModel);

        var imageName = $"{Guid.NewGuid()}{Path.GetExtension(viewModel.Image!.FileName)}";
        var logoName = $"{Guid.NewGuid()}{Path.GetExtension(viewModel.Logo!.FileName)}";

        var logoRes = _imageService.UploadImage(viewModel.Logo, logoName, "images/restaurant/logo", false);

        if (!logoRes.isUploaded)
        {
            ModelState.AddModelError("Logo", logoRes.errorMessage!);
            return View("Form", viewModel);
        }

        var result = _imageService.UploadImage(viewModel.Image, imageName, "images/restaurant/banner", false);

        if (!result.isUploaded)
        {
            ModelState.AddModelError("ImageUrl", result.errorMessage!);
            return View("Form", viewModel);
        }
        var model = _mapper.Map<Restaurant>(viewModel);
        model.ImagePath = "/images/restaurant/banner/" + imageName;
        model.LogoPath = "/images/restaurant/logo/" + logoName;
        model.IsActive = true;
        model.CreatedOn = DateTime.Now;

        foreach (var item in viewModel.SelectedCategories)
            model.RestaurantCategories.Add(new RestaurantCategory { CategoryId = item, });

        _context.Restaurants.Add(model);
        _context.SaveChanges();

        return Ok(model.Id);
    }



    [HttpGet]
    public IActionResult Update(int id)
    {
        var restaurant = _context.Restaurants.Include(r => r.OpeningHours).Include(r => r.RestaurantCategories).SingleOrDefault(r => r.Id == id);
        if (restaurant == null)
            return NotFound();

        var viewModel = _mapper.Map<RestaurantFormViewModel>(restaurant);
        viewModel.weekDays = _context.WeekDays.ToList();
        viewModel.SelectedCategories = restaurant.RestaurantCategories.Select(r => r.CategoryId).ToList();
        viewModel.Categories = _context.Categories.Select(c => new SelectListItem
        {
            Text = c.Name,
            Value = Convert.ToString(c.Id)
        }).ToList();


        return View("Form", viewModel);
    }



    public IActionResult Update(RestaurantFormViewModel viewModel)
    {
        if (!ModelState.IsValid) return BadRequest();

        var restaurant = _context.Restaurants.Find(viewModel.Id);
        if (restaurant is null)
            return NotFound();

        if (viewModel.Image is not null)
        {
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(viewModel.Image.FileName)}";


            _imageService.DeleteImage(restaurant.ImagePath!);
            var res = _imageService.UploadImage(viewModel.Image, imageName, "images/restaurant/banner", false);
            if (!res.isUploaded)
            {
                ModelState.AddModelError("Image", res.errorMessage!);
                return View("Update", viewModel);
            }
            viewModel.ImagePath = "/images/restaurant/banner/" + imageName;
        }
        else
            viewModel.ImagePath = restaurant.ImagePath;

        if (viewModel.Logo is not null)
        {
            var logoName = $"{Guid.NewGuid()}{Path.GetExtension(viewModel.Logo.FileName)}";

            _imageService.DeleteImage(restaurant.LogoPath!);
            var res = _imageService.UploadImage(viewModel.Logo, logoName, "images/restaurant/logo", false);
            if (!res.isUploaded)
            {
                ModelState.AddModelError("Logo", res.errorMessage!);
                return View("Update", viewModel);
            }
            viewModel.LogoPath = "/images/restaurant/logo/" + logoName;
        }
        else
            viewModel.LogoPath = restaurant.LogoPath;


        restaurant = _mapper.Map(viewModel, restaurant);


        // update categories for restaurant
        var categories = _context.RestaurantCategories.Where(r => r.RestaurantId == viewModel.Id).ToList();
        _context.RestaurantCategories.RemoveRange(categories);
        foreach (var item in viewModel.SelectedCategories)
            restaurant.RestaurantCategories.Add(new RestaurantCategory { CategoryId = item });


        _context.Restaurants.Update(restaurant);
        _context.SaveChanges();
        return Ok();

    }

    #region Ajax Requests

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

    [HttpPost]
    public IActionResult AddOpeningHours([FromBody] OpeningHoursRequest request)
    {
        if (request.OpeningHours is null)
        {
            return BadRequest();
        }
        _context.OpeningHours.AddRange(request.OpeningHours);
        _context.SaveChanges();
        return Ok(Json("added successfully"));
    }

    public IActionResult UpdateOpeningHours([FromBody] OpeningHoursRequest request)
    {
        if (request.OpeningHours is null)
        {
            return BadRequest();
        }
        var openingHours = _context.OpeningHours.Where(o => o.RestaurantId == request.OpeningHours.First().RestaurantId).ToList();

        _context.OpeningHours.RemoveRange(openingHours);
        _context.OpeningHours.AddRange(request.OpeningHours);
        _context.SaveChanges();
        return Ok(Json("added successfully"));
    }

    public IActionResult GetOpeningHours(int id)
    {
        var openingHours = _context.OpeningHours.Include(o => o.WeekDay).Where(o => o.RestaurantId == id).Select(o => new
        {
            o.From,
            o.To,
            Day = o.WeekDay.Name
        }).ToList();
        return Json(new { OpeningHours = openingHours });
    }
    #endregion

}
