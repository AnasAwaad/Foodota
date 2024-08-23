using Foodota.Areas.Admin.Data;
using Foodota.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Foodota.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]

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

        return View("Form", viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(MenuItemFormViewModel viewModel)
    {
        if (viewModel.Image is null)
        {
            ModelState.AddModelError("Image", "Image field is required!");
        }

        if (!ModelState.IsValid)
        {
            // If ModelState is invalid, repopulate the dropdowns and return the form
            viewModel.Categories = _context.Categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
            viewModel.Restaurants = _context.Restaurants.Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Id.ToString()
            }).ToList();

            return View("Form", viewModel);
        }

        // Image is not null, proceed with image processing
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

    [HttpGet]
    public IActionResult Update(int id)
    {
        var menuItem = _context.MenuItems.Find(id);
        if (menuItem == null)
            return NotFound();

        var viewModel = _mapper.Map<MenuItemFormViewModel>(menuItem);

        viewModel.Categories = _context.Categories.Select(c => new SelectListItem
        {
            Text = c.Name,
            Value = Convert.ToString(c.Id)
        }).ToList();

        viewModel.Restaurants = _context.Restaurants.Select(c => new SelectListItem
        {
            Text = c.Name,
            Value = Convert.ToString(c.Id)
        }).ToList();

        return View("Form", viewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(MenuItemFormViewModel viewModel)
    {
        if (!ModelState.IsValid) return BadRequest();

        var menuItem = _context.MenuItems.Find(viewModel.Id);
        if (menuItem is null)
            return NotFound();

        if (viewModel.Image is not null)
        {
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(viewModel.Image.FileName)}";

            _imageService.DeleteImage(menuItem.ImagePath!);
            var res = _imageService.UploadImage(viewModel.Image, imageName, "images/menuItem", false);
            if (!res.isUploaded)
            {
                ModelState.AddModelError("Image", res.errorMessage!);
                return View("Update", viewModel);
            }
            viewModel.ImagePath = "/images/menuItem/" + imageName;
        }
        else
            viewModel.ImagePath = menuItem.ImagePath;

        menuItem = _mapper.Map(viewModel, menuItem);

        _context.MenuItems.Update(menuItem);
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
