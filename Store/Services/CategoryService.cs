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
      var categories = await _categoryRepository.GetAllAsync();
      return categories.Select(item =>
      new CategoryVM
      {
        CategoryId = item.CategoryId,
        Name = item.Name,
        Description = item.Description
      }
      ).ToList();
    }
  }
}
