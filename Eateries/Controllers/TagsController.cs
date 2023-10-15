using Microsoft.AspNetCore.Mvc;
using Eateries.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eateries.Controllers
{
  public class TagsController : Controller
  {
    private readonly EateriesContext _db;

    public TagsController(EateriesContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Tags.ToList());
    }

    public ActionResult Details(int id)
    {
      Tag thisTag = _db.Tags
          .Include(tag => tag.JoinEntities)
          .ThenInclude(join => join.Restaurants)
          .FirstOrDefault(tag => tag.TagId == id);
      return View(thisTag);
    }
    public ActionResult Create()
    {
      return View();
    }
    [HttpPost]
    public ActionResult Create(Tag tag)
    {
      _db.Tags.Add(tag);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult AddRestaurant(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tag => tag.TagId == id);
      ViewBag.RestaurantId = new SelectList(_db.Restaurants, "RestaurantId", "Name");
      return View(thisTag);
    }
    [HttpPost]
    public ActionResult AddRestaurant(Tag tag, int restaurantId)
    {
      #nullable enable
      RestaurantTag? joinEntity = _db.RestaurantTags.FirstOrDefault(join => (join.RestaurantId == restaurantId && join.TagId == tag.TagId));
      #nullable disable
      if (joinEntity == null && restaurantId != 0)
      {
        _db.RestaurantTags.Add(new RestaurantTag() { RestaurantId = restaurantId, TagId = tag.TagId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = tag.TagId });
    }
    public ActionResult Edit(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tag => tag.TagId == id);
      return View(thisTag);
    }
    [HttpPost]
    public ActionResult Edit (Tag tag)
    {
      _db.Tags.Update(tag);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Delete(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tag => tag.TagId == id);
      return View(thisTag);
    }
    [HttpPost, ActionName("Delete")]
     public ActionResult DeleteConfirmed(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
      _db.Tags.Remove(thisTag);
      _db.SaveChanges();
      return RedirectToAction("Index");
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