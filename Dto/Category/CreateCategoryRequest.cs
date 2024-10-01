using System.ComponentModel.DataAnnotations;

namespace Shopping_API.Dto.Category;

public class CreateCategoryRequest
{
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(50, ErrorMessage = "Category name must be between 1 and 50 characters", MinimumLength = 1)]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
    public string Description { get; set; }
    
    public int ParentCategoryId { get; set; }
    
    public List<int> ChildCategoriesId {get; set;}
    
    public List<int> ProductsId { get; set; }
}