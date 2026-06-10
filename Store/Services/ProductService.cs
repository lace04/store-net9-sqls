using Microsoft.EntityFrameworkCore;
using Store.Entitites;
using Store.Models;
using Store.Repositories;

namespace Store.Services
{
  public class ProductService(
    GenericRepository<Product> _productRepository,
    IWebHostEnvironment _env)
  {
    private static string ImagesPath => "images/products";

    private async Task<string?> SaveImageAsync(IFormFile? imageFile)
    {
      if (imageFile == null || imageFile.Length == 0) return null;

      var uploads = Path.Combine(_env.WebRootPath, ImagesPath);
      Directory.CreateDirectory(uploads);

      var ext = Path.GetExtension(imageFile.FileName);
      var fileName = $"{Guid.NewGuid()}{ext}";
      var filePath = Path.Combine(uploads, fileName);

      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await imageFile.CopyToAsync(stream);
      }

      return fileName;
    }

    private void DeleteImage(string? imageName)
    {
      if (string.IsNullOrEmpty(imageName)) return;
      var filePath = Path.Combine(_env.WebRootPath, ImagesPath, imageName);
      if (File.Exists(filePath))
        File.Delete(filePath);
    }

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
        Category = new CategoryVM { CategoryId = item.CategoryId, Name = item.Category?.Name },
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
        Category = new CategoryVM { CategoryId = product.CategoryId, Name = product.Category?.Name },
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
          CategoryId = productVM.Category?.CategoryId ?? 0,
          Name = productVM.Name,
          Description = productVM.Description,
          Price = productVM.Price,
          Stock = productVM.Stock
        };

        if (productVM.ImageFile != null)
          product.ImageName = await SaveImageAsync(productVM.ImageFile);
        else
          product.ImageName = productVM.ImageName;

        await _productRepository.AddAsync(product);
      }
      else
      {
        var product = await _productRepository.GetByIdAsync(productVM.ProductId);
        if (product != null)
        {
          product.CategoryId = productVM.Category?.CategoryId ?? 0;
          product.Name = productVM.Name;
          product.Description = productVM.Description;
          product.Price = productVM.Price;
          product.Stock = productVM.Stock;

          if (productVM.ImageFile != null)
          {
            DeleteImage(product.ImageName);
            product.ImageName = await SaveImageAsync(productVM.ImageFile);
          }

          await _productRepository.UpdateAsync(product);
        }
      }
    }

    public async Task DeleteAsync(int id)
    {
      var product = await _productRepository.GetByIdAsync(id);
      if (product != null)
      {
        DeleteImage(product.ImageName);
        await _productRepository.DeleteAsync(product);
      }
    }
  }
}
