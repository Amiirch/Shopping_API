using System.ComponentModel.DataAnnotations;

namespace Shopping_API.Extensions.Dto.Product;

public class UpdateProductRequest
{
    [Required(ErrorMessage = "product id is required")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "product name is required")]
    [StringLength(50, ErrorMessage = "product name must be between 1 and 50 characters", MinimumLength = 5)]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "product description is required")]
    [StringLength(50, ErrorMessage = "product description must be between 1 and 50 characters", MinimumLength = 10)]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "product price is required")]
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    
}