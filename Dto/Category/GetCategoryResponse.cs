using Shopping_API.Dto.Product;
using Shopping_API.Extensions.Dto.Product;

namespace Shopping_API.Dto.Category;

public class GetCategoryResponse
{
    public GetCategoryResponse(int id,string? name,string? description,List<ProductResponse>? products,GetParentCategoryResponse? parentCategory,List<GetCategoryListResponse>? childCategories)
    {
        Id = id;
        Name = name;
        Description = description;
        ParentCategory = parentCategory;
        ChildCategories = childCategories;
        Products = products;
    }

    public GetCategoryResponse(int id,string? name,string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    
    public int Id { get; set;}
    public string? Name { get; set; }
    public string? Description { get; set; }
    public GetParentCategoryResponse? ParentCategory { get; set; }
    public List<GetCategoryListResponse>? ChildCategories { get; set; }
    public List<ProductResponse>? Products{ get; set; }
}