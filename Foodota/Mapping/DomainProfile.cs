﻿using AutoMapper;
using Foodota.Core.Models;
using Foodota.Core.ViewModels;

namespace Foodota.Mapping;

public class DomainProfile : Profile
{
    public DomainProfile()
    {
        CreateMap<RestaurantFormViewModel, Restaurant>();
        CreateMap<Restaurant, RestaurantViewModel>().ReverseMap();
    }
}
