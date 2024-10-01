using System.ComponentModel.DataAnnotations;

namespace Shopping_API.Dto.Category;

public class UpdateCategoryRequest
{
    [Required(ErrorMessage = "category id is required")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required")]
    [StringLength(50, ErrorMessage = "Category name must be between 10 and 50 characters", MinimumLength = 10)]
    public string Name { get; set; }

    [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
    public string Description { get; set; }
    
    public int? ParentCategoryId { get; set; }
}