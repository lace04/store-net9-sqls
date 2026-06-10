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

    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id)
    {
      // TODO: Implement edit logic
      return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
      // TODO: Implement delete logic
      return RedirectToAction(nameof(Index));
    }
  }
}
