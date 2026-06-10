using Microsoft.EntityFrameworkCore;
using Store.Repositories;
using Store.Entitites;
using Store.Models;

namespace Store.Services
{
  public class CategoryService(GenericRepository<Category> _categoryRepository)
  {
    public async Task<IEnumerable<CategoryVM>> GetAllAsync()
    {
      var categories = await _categoryRepository.GetAllAsync(c => c.Products);
      return categories.Select(item =>
      new CategoryVM
      {
        CategoryId = item.CategoryId,
        Name = item.Name,
        Description = item.Description,
        ProductCount = item.Products.Count
      }
      ).ToList();
    }

    public async Task<CategoryVM?> GetByIdAsync(int id)
    {
      var category = await _categoryRepository.GetByIdAsync(id);
      if (category == null)
      {
        return null;
      }
      return new CategoryVM
      {
        CategoryId = category.CategoryId,
        Name = category.Name,
        Description = category.Description
      };
    }

    public async Task AddAsync(CategoryVM categoryVM)
    {
      if (categoryVM.CategoryId is null or 0)
      {
        var category = new Category
        {
          Name = categoryVM.Name,
          Description = categoryVM.Description
        };
        await _categoryRepository.AddAsync(category);
      }
      else
      {
        var category = await _categoryRepository.GetByIdAsync(categoryVM.CategoryId.Value);
        if (category != null)
        {
          category.Name = categoryVM.Name;
          category.Description = categoryVM.Description;
          await _categoryRepository.UpdateAsync(category);
        }
      }
    }

    public async Task DeleteAsync(int id)
    {
      var category = await _categoryRepository.GetByIdAsync(id);
      if (category != null)
      {
        await _categoryRepository.DeleteAsync(category);
      }
    }
  }
}
