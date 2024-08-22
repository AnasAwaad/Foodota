using AutoMapper;
using Foodota.Core.Models;
using Foodota.Core.ViewModels;

namespace Foodota.Mapping;

public class DomainProfile : Profile
{
    public DomainProfile()
    {
        // Restaurant
        CreateMap<RestaurantFormViewModel, Restaurant>().ReverseMap();
        CreateMap<Restaurant, RestaurantViewModel>().ReverseMap();

		// Category
		CreateMap<CategoryFormViewModel, Category>().ReverseMap();
		CreateMap<Category, CategoryViewModel>().ReverseMap();

		// MenuItem
		CreateMap<MenuItemFormViewModel, MenuItem>().ReverseMap();
		CreateMap<MenuItem, MenuItemViewModel>().ReverseMap();

	}
}
