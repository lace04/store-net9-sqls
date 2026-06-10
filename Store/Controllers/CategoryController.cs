using Microsoft.AspNetCore.Mvc;
using Store.Context;

namespace Store.Controllers
{
  public class CategoryController(AppDbContext _dbContext) : Controller
  {
    public IActionResult Index()
    {

      // Get list of categories from database
      var categories = _dbContext.Categories.ToList();

      return View(categories);
    }
  }
}
