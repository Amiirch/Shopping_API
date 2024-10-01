using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping_API.Extensions.Dto.User;
using Shopping_API.Services.IServices;
using Shopping_API.Extensions.Dto.Product;
using Shopping_API.Extensions.Exceptions;


namespace Shopping_API.Controllers;



[Route("api/[Controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    
    [Authorize(Roles = $"{UserRoles.User}")]
    [HttpGet("{productId:int}")]
    public async Task<ActionResult> GetByIdAsync([FromRoute] int productId)
    {
        try
        {
            var product = await _productService.GetById(productId);
            return Ok(product);
        }
        catch (CustomException ex)
        {
            return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
        }
    }

    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] CreateProductRequest createProductRequest)
    {
        try
        {
            await _productService.CreateAsync(createProductRequest);
            return Ok("Products created successfully");
        }
        catch (CustomException ex)
        {
            return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
        }
    }
    
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
    [HttpPut]
    public async Task<ActionResult> UpdateAsync([FromBody] UpdateProductRequest updateProductRequest)
    {
        try
        {
            await _productService.UpdateAsync(updateProductRequest);
            return Ok("product updated successfully");
        }
        catch (CustomException ex)
        {
            return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
        }
    }
    
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
    [HttpDelete("{productId:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int productId)
    {
        try
        {
            await _productService.DeleteAsync(productId);
            return Ok("product deleted successfully");
        }
        catch (CustomException ex)
        {
            return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
        }
    }
}  