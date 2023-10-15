using Microsoft.EntityFrameworkCore;

namespace Eateries.Models
{
  public class EateriesContext : DbContext
  {
    public DbSet<Cuisine> Cuisine { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<RestaurantTag> RestaurantTags { get; set; }

    public EateriesContext(DbContextOptions options) : base(options) { }
  }
}