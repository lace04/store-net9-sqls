using Microsoft.AspNetCore.Mvc;
using Store.Services;

namespace Store.Controllers
{
  public class CategoryController(CategoryService _categoryService) : Controller
  {
    public IActionResult Index()
    {
      // Get list of categories from database
      var categories = _categoryService.GetAllAsync().Result;
      return View(categories);
    }
  }
}
