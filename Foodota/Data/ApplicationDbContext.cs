using Foodota.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Foodota.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
	public DbSet<OpeningHour> OpeningHours { get; set; }
	public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<WeekDay> WeekDays { get; set; }
}
