using Microsoft.AspNetCore.Mvc;
using Store.Services;

namespace Store.Controllers
{
  public class CategoryController(CategoryService _categoryService) : Controller
  {
    public async Task<IActionResult> Index()
    {
      var categories = await _categoryService.GetAllAsync();
      return View(categories);
    }
  }
}
