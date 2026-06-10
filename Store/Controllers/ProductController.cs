using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Store.Models;
using Store.Services;

namespace Store.Controllers
{
  public class ProductController(ProductService _productService, CategoryService _categoryService, ILogger<ProductController> _logger) : Controller
  {
    public async Task<IActionResult> Index(int? categoryId)
    {
      ViewBag.Categories = (await _categoryService.GetAllAsync()).Select(c =>
        new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name }
      );

      var products = await _productService.GetAllAsync(categoryId);
      return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> AddEdit(int? id)
    {
      var categories = (await _categoryService.GetAllAsync()).Select(c =>
        new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name }
      ).ToList();

      if (id.HasValue)
      {
        var product = await _productService.GetByIdAsync(id.Value);
        if (product == null) return NotFound();
        product.Categories = categories;
        return View(product);
      }
      return View(new ProductVM { Categories = categories });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddEdit(ProductVM productVM)
    {
      if (productVM.Category?.CategoryId is null)
      {
        ModelState.AddModelError("Category.CategoryId", "Please select a category.");
      }

      if (!ModelState.IsValid)
      {
        foreach (var kv in ModelState)
        {
          foreach (var err in kv.Value.Errors)
          {
            _logger.LogWarning("ModelState error: {Key} = {ErrorMessage}", kv.Key, err.ErrorMessage);
          }
        }
        productVM.Categories = (await _categoryService.GetAllAsync()).Select(c =>
          new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name }
        ).ToList();
        return View(productVM);
      }

      try
      {
        bool isNew = productVM.ProductId == 0;
        await _productService.AddAsync(productVM);
        TempData["Success"] = isNew
          ? "Product created successfully."
          : "Product updated successfully.";
        return RedirectToAction(nameof(Index));
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error creating product");
        ModelState.AddModelError("", $"An error occurred: {ex.Message}");
        productVM.Categories = (await _categoryService.GetAllAsync()).Select(c =>
          new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name }
        ).ToList();
        return View(productVM);
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        await _productService.DeleteAsync(id);
        TempData["Success"] = "Product deleted successfully.";
      }
      catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("FK") == true || ex.InnerException?.Message.Contains("REFERENCE") == true)
      {
        var product = await _productService.GetByIdAsync(id);
        TempData["Error"] = $"Cannot delete \"{product?.Name ?? "product"}\": it is referenced in existing orders. Remove the order references first.";
      }
      catch
      {
        TempData["Error"] = "An error occurred while deleting the product.";
      }
      return RedirectToAction(nameof(Index));
    }
  }
}
