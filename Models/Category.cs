using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shopping_API.Models;


public class Category
{
    [Key] 
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required] 
    public string Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public List<Category>? ChildCategories { get; set; }
    public List<Product>? Products { get; set; }
    
}