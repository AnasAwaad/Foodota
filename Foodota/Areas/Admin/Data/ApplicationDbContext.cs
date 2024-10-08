﻿using Foodota.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Foodota.Areas.Admin.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<RestaurantCategory>()
            .HasKey(e => new { e.RestaurantId, e.CategoryId });
    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<OpeningHour> OpeningHours { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<RestaurantCategory> RestaurantCategories { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<WeekDay> WeekDays { get; set; }
}
