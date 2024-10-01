

using Shopping_API.Dto.Category;

namespace Shopping_API.Extensions.Dto.Product;

public class GetProductResponse
{
    public GetProductResponse(int id,string name,string description,decimal price ,int stockQuantity,List<GetProductCategoriesResponse> categories)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        Categories = categories;
    }
    public int Id { get; set;}
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public List<GetProductCategoriesResponse> Categories { get; set; }
    
}