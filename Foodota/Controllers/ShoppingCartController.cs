using Foodota.Areas.Admin.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Foodota.Controllers;
[Authorize]
public class ShoppingCartController : Controller
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	public ShoppingCartController(ApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public IActionResult Index()
	{
		var shoppingCart = _context.ShoppingCarts
			.Include(s => s.MenuItem)
			.ThenInclude(s => s.Category)
			.Where(s => s.ApplicationUserId == User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
			.ToList();



		return View(_mapper.Map<IEnumerable<ShoppingCartViewModel>>(shoppingCart));
	}

	public IActionResult GetAllItemsInCart()
	{
		var shoppingCart = _context.ShoppingCarts
			.Include(s => s.MenuItem)
			.Where(s => s.ApplicationUserId == User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
			.ToList();

		var cart = _mapper.Map<IEnumerable<ShoppingCartViewModel>>(shoppingCart);
		return Json(new { cart });
	}

	public IActionResult AddToCart([FromBody] AddMenuItemToCartRequest request)
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;


		var menuItem = _context.ShoppingCarts.FirstOrDefault(cart => cart.ApplicationUserId == userId && cart.MenuItemId == request.menuItemId);
		if (menuItem != null)
			menuItem.Count += request.menuItemCount;
		else
		{
			ShoppingCart cart = new()
			{
				ApplicationUserId = userId,
				Count = request.menuItemCount,
				MenuItemId = request.menuItemId,
			};
			_context.ShoppingCarts.Add(cart);
		}
		_context.SaveChanges();
		return Ok("Item added successfully");
	}

	public IActionResult RemoveItem(int id)
	{
		var menuItem=_context.ShoppingCarts.Find(id);
		if (menuItem is null)
			return NotFound();
		_context.ShoppingCarts.Remove(menuItem);
		_context.SaveChanges();
		return Ok();
	}

	public IActionResult IncreaseItem(int id)
	{
		var cartItem = _context.ShoppingCarts
			.Include(s => s.MenuItem)
			.ThenInclude(s => s.Category)
			.Where(s => s.Id == id)
			.FirstOrDefault();

		if (cartItem is null)
		{
			return NotFound();
		}
		cartItem.Count++;
		_context.SaveChanges();

		return PartialView("_ShoppingCartItemRow", _mapper.Map<ShoppingCartViewModel>(cartItem));
	}

	public IActionResult DecreaseItem(int id)
	{
		var cartItem = _context.ShoppingCarts
			.Include(s => s.MenuItem)
			.ThenInclude(s => s.Category)
			.Where(s => s.Id == id)
			.FirstOrDefault();

		if (cartItem is null)
		{
			return NotFound();
		}

		if (cartItem.Count <= 1)
			_context.ShoppingCarts.Remove(cartItem);
		else
			cartItem.Count--;
		_context.SaveChanges();


		return PartialView("_ShoppingCartItemRow", _mapper.Map<ShoppingCartViewModel>(cartItem));
	}

	public IActionResult GetTotalItemsInCart()
	{
		var count=_context.ShoppingCarts.Where(s=>s.ApplicationUserId==User.FindFirst(ClaimTypes.NameIdentifier)!.Value).Count();
		return Json(new { count });
	}



}
