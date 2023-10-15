using System.Collections.Generic;

namespace Eateries.Models
{
  public class Cuisine
  {
    public int CuisineId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public List<Restaurant> Restaurants { get; set; }
  }
}