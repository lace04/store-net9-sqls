using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Entitites;
using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
  public class ProductVM
  {
    public int ProductId { get; set; }
    public CategoryVM Category { get; set; } = new CategoryVM();
    public List<SelectListItem>? Categories { get; set; }
    public string? CategoryName { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? ImageName { get; set; }
    public IFormFile? ImageFile { get; set; }
  }
}
