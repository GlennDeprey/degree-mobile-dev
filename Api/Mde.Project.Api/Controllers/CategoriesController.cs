using Mde.Project.Api.Dtos.Products;
using Mde.Project.Api.Core.Entities.Products;
using Mde.Project.Api.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Mde.Project.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly int _itemsPage;
        public CategoriesController(IProductService productService)
        {
            _productService = productService;
            _itemsPage = 10;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<CategoryListDto>> GetCategoriesAsync([FromQuery] string name, [FromQuery] bool withProductCount = false, [FromQuery] bool withPagination = false, [FromQuery] int pageNumber = 1)
        {
            var categoryList = new CategoryListDto();
            var includeProperties = withProductCount ? "Products" : string.Empty;
            var filterExpression = string.IsNullOrEmpty(name) ? null : (Expression<Func<ProductCategory, bool>>)(c => c.Name.Contains(name));

            if (withPagination)
            {
                var pagedCategories = await _productService.Categories.GetAllAsync(filterExpression, pageNumber, _itemsPage, includeProperties);
                if (pagedCategories.Success)
                {
                    categoryList.Items = pagedCategories.Data.Select(p => new CategoryDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ProductCount = withProductCount ? p.Products?.Count ?? 0 : 0,
                    });
                    categoryList.TotalItems = pagedCategories.TotalItems;
                    categoryList.TotalPages = pagedCategories.TotalPages;
                    return Ok(categoryList);
                }
                return NotFound(pagedCategories.Errors);
            }

            var categories = await _productService.Categories.GetAllAsync(filterExpression, includeProperties);
            if (categories.Success)
            {
                categoryList.Items = categories.Data.Select(p => new CategoryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductCount = withProductCount ? p.Products?.Count ?? 0 : 0,
                });

                return Ok(categoryList);
            }
            return NotFound(categories.Errors);

        }

        [HttpGet("{categoryId:Guid}", Name = "GetCategoryById")]
        public async Task<ActionResult<BrandDto>> GetCategoryByIdAsync(Guid categoryId)
        {
            var category = await _productService.Categories.GetByIdAsync(categoryId, includeProperties: "Products");
            if (category.Success)
            {
                var categoryDto = new CategoryDto()
                {
                    Id = category.Data.Id,
                    Name = category.Data.Name,
                    ProductCount = category.Data.Products?.Count ?? 0,
                };

                return Ok(categoryDto);
            }

            return NotFound(category.Errors);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateProductAsync(CategoryCreationDto categoryCreationDto)
        {
            var category = new ProductCategory
            {
                Name = categoryCreationDto.Name,
            };

            var createdCategory = await _productService.Categories.AddAsync(category);
            if (createdCategory.Success)
            {
                var categoryDto = new CategoryDto()
                {
                    Id = createdCategory.Data.Id,
                    Name = createdCategory.Data.Name,
                };

                return CreatedAtRoute("GetCategoryById", new { categoryId = categoryDto.Id }, categoryDto);
            }

            return BadRequest(createdCategory.Errors);
        }

        [HttpPut("{categoryId:Guid}")]
        public async Task<ActionResult> UpdateCategoryAsync(Guid categoryId, CategoryUpdateDto categoryUpdateDto)
        {
            var category = await _productService.Categories.GetByIdAsync(categoryId);
            if (!category.Success)
            {
                return NotFound(category.Errors);
            }

            category.Data.Name = categoryUpdateDto.Name;

            var updatedCategory = await _productService.Categories.UpdateAsync(category.Data);
            if (updatedCategory.Success)
            {
                return NoContent();
            }
            return BadRequest(updatedCategory.Errors);
        }

        [HttpDelete("{categoryId:Guid}")]
        public async Task<ActionResult> DeleteCategoryAsync(Guid categoryId)
        {
            var category = await _productService.Categories.GetByIdAsync(categoryId);
            if (!category.Success)
            {
                return NotFound(category.Errors);
            }

            var deletedCategory = await _productService.Categories.RemoveAsync(categoryId);
            if (deletedCategory.Success)
            {
                return NoContent();
            }
            return BadRequest(deletedCategory.Errors);
        }
    }
}
