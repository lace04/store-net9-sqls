using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Models;
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

    [HttpGet]
    public async Task<IActionResult> AddEdit(int? id)
    {
      if (id.HasValue)
      {
        var category = await _categoryService.GetByIdAsync(id.Value);
        if (category == null)
        {
          return NotFound();
        }
        return View(category);
      }
      return View(new CategoryVM());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddEdit(CategoryVM categoryVM)
    {
      if (string.IsNullOrWhiteSpace(categoryVM.Name))
        ModelState.AddModelError("Name", "Name is required.");
      if (string.IsNullOrWhiteSpace(categoryVM.Description))
        ModelState.AddModelError("Description", "Description is required.");

      if (!ModelState.IsValid)
      {
        return View(categoryVM);
      }

      try
      {
        bool isNew = categoryVM.CategoryId is null or 0;
        await _categoryService.AddAsync(categoryVM);
        TempData["Success"] = isNew
          ? "Category created successfully."
          : "Category updated successfully.";
        return RedirectToAction(nameof(Index));
      }
      catch (Exception ex)
      {
        ModelState.AddModelError("", $"An error occurred: {ex.Message}");
        return View(categoryVM);
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        await _categoryService.DeleteAsync(id);
        TempData["Success"] = "Category deleted successfully.";
      }
      catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("FK") == true || ex.InnerException?.Message.Contains("REFERENCE") == true)
      {
        var category = await _categoryService.GetByIdAsync(id);
        TempData["Error"] = $"Cannot delete \"{category?.Name ?? "category"}\": one or more products are still assigned to it. Reassign or delete those products first.";
      }
      catch (Exception)
      {
        TempData["Error"] = "An error occurred while deleting the category.";
      }
      return RedirectToAction(nameof(Index));
    }
  }
}
