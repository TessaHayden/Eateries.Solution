namespace Eateries.Models
{
  public class RestaurantTag
  {
    public int RestaurantTagId { get; set; }
    public int RestaurantId { get; set; }
    public Restaurant Restaurants { get; set; }
    public int TagId { get; set; }
    public Tag Tag { get; set; }
  }
}