using System.ComponentModel.DataAnnotations;

namespace Store.Entitites
{
  public class Category
  {
    public int CategoryId { get; set; }
    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    public ICollection<Product> Products { get; set; }

  }
}
