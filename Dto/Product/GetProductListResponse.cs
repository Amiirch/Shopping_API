namespace Shopping_API.Dto.Product;

public class GetProductListResponse
{
    public GetProductListResponse(int categoryId,string categoryName,string categoryDescription,List<ProductResponse> products)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
        CategoryDescription = categoryDescription;
        Products = products;

    }
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }
        
    public string CategoryDescription { get; set; }

    public List<ProductResponse> Products { get; set; }
}
public class ProductResponse
{
    public ProductResponse(int id, string name, string description, decimal price, int stockQuantity)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
    }
    public int Id { get; set;}
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }
    
    public int StockQuantity { get; set; }
    
}