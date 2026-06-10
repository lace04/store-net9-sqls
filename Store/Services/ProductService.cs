using Microsoft.EntityFrameworkCore;
using Store.Entitites;
using Store.Models;
using Store.Repositories;

namespace Store.Services
{
  public class ProductService(GenericRepository<Product> _productRepository)
  {
    public async Task<IEnumerable<ProductVM>> GetAllAsync(int? categoryId = null)
    {
      var products = await _productRepository.GetAllAsync(p => p.Category);
      var query = products.AsQueryable();

      if (categoryId.HasValue)
      {
        query = query.Where(p => p.CategoryId == categoryId.Value);
      }

      return query.AsEnumerable().Select(item => new ProductVM
      {
        ProductId = item.ProductId,
        CategoryId = item.CategoryId,
        CategoryName = item.Category?.Name,
        Name = item.Name,
        Description = item.Description,
        Price = item.Price,
        Stock = item.Stock,
        ImageName = item.ImageName
      }).ToList();
    }

    public async Task<ProductVM?> GetByIdAsync(int id)
    {
      var product = await _productRepository.GetByIdAsync(id);
      if (product == null) return null;

      return new ProductVM
      {
        ProductId = product.ProductId,
        CategoryId = product.CategoryId,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        Stock = product.Stock,
        ImageName = product.ImageName
      };
    }

    public async Task AddAsync(ProductVM productVM)
    {
      if (productVM.ProductId == 0)
      {
        var product = new Product
        {
          CategoryId = productVM.CategoryId,
          Name = productVM.Name,
          Description = productVM.Description,
          Price = productVM.Price,
          Stock = productVM.Stock,
          ImageName = productVM.ImageName
        };
        await _productRepository.AddAsync(product);
      }
      else
      {
        var product = await _productRepository.GetByIdAsync(productVM.ProductId);
        if (product != null)
        {
          product.CategoryId = productVM.CategoryId;
          product.Name = productVM.Name;
          product.Description = productVM.Description;
          product.Price = productVM.Price;
          product.Stock = productVM.Stock;
          product.ImageName = productVM.ImageName;
          await _productRepository.UpdateAsync(product);
        }
      }
    }

    public async Task DeleteAsync(int id)
    {
      var product = await _productRepository.GetByIdAsync(id);
      if (product != null)
      {
        await _productRepository.DeleteAsync(product);
      }
    }
  }
}
