using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Eateries.Models;
using System.Collections.Generic;
using System.Linq;

namespace Eateries.Controllers
{
  public class RestaurantsController : Controller
  {
    private readonly EateriesContext _db;

    public RestaurantsController(EateriesContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Restaurant> model = _db.Restaurants
                            .Include(restaurant => restaurant.Cuisine)
                            .ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.CuisineId = new SelectList(_db.Cuisine, "CuisineId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Restaurant restaurant)
    {
      if (restaurant.CuisineId == 0)
      {
        return RedirectToAction("Create");
      }
      _db.Restaurants.Add(restaurant);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Restaurant thisRestaurant = _db.Restaurants
              .Include(restaurant => restaurant.Cuisine)
              .Include(restaurant => restaurant.JoinEntities)
              .ThenInclude(join => join.Tag)
              .FirstOrDefault(restaurant => restaurant.RestaurantId == id);
      return View(thisRestaurant);
    }

    public ActionResult Edit(int id)
    {
      Restaurant thisRestaurant = _db.Restaurants.FirstOrDefault(restaurant => restaurant.RestaurantId == id);
      ViewBag.CuisineId = new SelectList(_db.Cuisine, "CuisineId", "Name");
      return View(thisRestaurant);
    }

    [HttpPost]
    public ActionResult Edit(Restaurant restaurant)
    {
      _db.Restaurants.Update(restaurant);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Restaurant thisRestaurant = _db.Restaurants.FirstOrDefault(restaurant => restaurant.RestaurantId == id);
      return View(thisRestaurant);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Restaurant thisRestaurant = _db.Restaurants.FirstOrDefault(restaurant => restaurant.RestaurantId == id);
      _db.Restaurants.Remove(thisRestaurant);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult AddTag(int id)
    {
      Restaurant thisRestaurant = _db.Restaurants.FirstOrDefault(restaurants => restaurants.RestaurantId == id);
      ViewBag.TagId = new SelectList(_db.Tags, "TagId", "Title");
      return View(thisRestaurant);
    }
    [HttpPost]
    public ActionResult AddTag(Restaurant restaurant, int tagId)
    {
      #nullable enable
      RestaurantTag? joinEntity = _db.RestaurantTags.FirstOrDefault(join => (join.TagId == tagId && join.RestaurantId == restaurant.RestaurantId));
      #nullable disable
      if (joinEntity == null && tagId != 0)
      {
        _db.RestaurantTags.Add(new RestaurantTag() { TagId = tagId, RestaurantId = restaurant.RestaurantId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = restaurant.RestaurantId });
    }
    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      RestaurantTag joinEntry = _db.RestaurantTags.FirstOrDefault(entry => entry.RestaurantTagId == joinId);
      _db.RestaurantTags.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}