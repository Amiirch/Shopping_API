using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping_API.Dto.Category;
using Shopping_API.Extensions.Dto.User;
using Shopping_API.Services.IServices;
using Shopping_API.Extensions.Exceptions;


namespace Shopping_API.Controllers;

    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        [HttpGet("{categoryId}")]
        public async Task<ActionResult> GetByIdAsync([FromRoute] int categoryId)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(categoryId);
                return Ok(category);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
            }
        }

        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            try
            {
                await _categoryService.CreateAsync(createCategoryRequest);
                return Ok("Created Category Successfully");
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
            }
        }
        
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            try
            {
                var categories = await _categoryService.GetAllAsync();
                var categoriesResponse = categories.Select(category => new GetCategoryListResponse(
                    category.Id, 
                    category.Name, 
                    category.Description));
                return Ok(categoriesResponse);
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            try
            {
                await _categoryService.UpdateAsync(updateCategoryRequest);
                return Ok("category updated successfully");
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
            }
        }
    
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        [HttpDelete("{categoryId:int}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int categoryId)
        {
            try
            {
                await _categoryService.DeleteAsync(categoryId);
                return Ok("category deleted successfully");
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
            }
        }
        
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.User}")]
        [HttpGet("products/{categoryId:int}")]
        public async Task<ActionResult> GetListByCategoryId([FromRoute] int categoryId)
        {
            try
            {
                var products = await _categoryService.getProductsByCategoryId(categoryId);
                return Ok(products);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.ErrorCode, new { errormessage = ex.Message });
            }
        }
    }


