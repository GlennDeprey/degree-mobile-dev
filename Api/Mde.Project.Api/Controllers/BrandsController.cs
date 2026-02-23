using Mde.Project.Api.Dtos.Products;
using Mde.Project.Api.Core.Entities.Products;
using Mde.Project.Api.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Mde.Project.Api.Controllers
{
    [Route("api/brands")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private IProductService _productService;
        private int _itemsPage;

        public BrandsController(IProductService productService)
        {
            _productService = productService;
            _itemsPage = 10;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<BrandListDto>> GetBrandsAsync([FromQuery] string name, [FromQuery] bool withProductCount = false, [FromQuery] bool withPagination = false, [FromQuery] int pageNumber = 1)
        {
            var brandList = new BrandListDto();
            var includeProperties = withProductCount ? "Products" : string.Empty;
            var filterExpression = string.IsNullOrEmpty(name) ? null : (Expression<Func<Brand, bool>>)(c => c.Name.Contains(name));

            if (withPagination)
            {
                var pagedBrands = await _productService.Brands.GetAllAsync(filterExpression, pageNumber, _itemsPage, includeProperties);
                if (pagedBrands.Success)
                {
                    brandList.Items = pagedBrands.Data.Select(p => new BrandDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ProductCount = withProductCount ? p.Products?.Count ?? 0 : 0,
                    });
                    brandList.TotalItems = pagedBrands.TotalItems;
                    brandList.TotalPages = pagedBrands.TotalPages;
                    return Ok(brandList);
                }
                return NotFound(pagedBrands.Errors);
            }

            var brands = await _productService.Brands.GetAllAsync(filterExpression, includeProperties);
            if (brands.Success)
            {
                brandList.Items = brands.Data.Select(p => new BrandDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ProductCount = withProductCount ? p.Products?.Count ?? 0 : 0,
                });

                return Ok(brandList);
            }
            return NotFound(brands.Errors);

        }

        [HttpGet("{brandId:Guid}", Name = "GetBrandById")]
        public async Task<ActionResult<BrandDto>> GetBrandByIdAsync(Guid brandId)
        {
            var brand = await _productService.Brands.GetByIdAsync(brandId, includeProperties: "Products");
            if (brand.Success)
            {
                var brandDto = new BrandDto
                {
                    Id = brand.Data.Id,
                    Name = brand.Data.Name,
                    ProductCount = brand.Data.Products?.Count ?? 0,
                };

                return Ok(brandDto);
            }

            return NotFound(brand.Errors);
        }

        [HttpPost]
        public async Task<ActionResult<BrandDto>> CreateBrandAsync(BrandCreationDto brandCreationDto)
        {
            var brand = new Brand
            {
                Name = brandCreationDto.Name,
            };

            var createdBrand = await _productService.Brands.AddAsync(brand);
            if (createdBrand.Success)
            {
                var brandDto = new BrandDto()
                {
                    Id = createdBrand.Data.Id,
                    Name = createdBrand.Data.Name,
                };

                return CreatedAtRoute("GetBrandById", new { brandId = brandDto.Id }, brandDto);
            }

            return BadRequest(createdBrand.Errors);
        }

        [HttpPut("{brandId:Guid}")]
        public async Task<ActionResult> UpdateBrandAsync(Guid brandId, BrandUpdateDto brandUpdateDto)
        {
            var brand = await _productService.Brands.GetByIdAsync(brandId);
            if (!brand.Success)
            {
                return NotFound(brand.Errors);
            }

            brand.Data.Name = brandUpdateDto.Name;

            var updatedBrand = await _productService.Brands.UpdateAsync(brand.Data);
            if (updatedBrand.Success)
            {
                return NoContent();
            }
            return BadRequest(updatedBrand.Errors);
        }

        [HttpDelete("{brandId:Guid}")]
        public async Task<ActionResult> DeleteBrandAsync(Guid brandId)
        {
            var brand = await _productService.Brands.GetByIdAsync(brandId);
            if (!brand.Success)
            {
                return NotFound(brand.Errors);
            }

            var deletedBrand = await _productService.Brands.RemoveAsync(brandId);
            if (deletedBrand.Success)
            {
                return NoContent();
            }
            return BadRequest(deletedBrand.Errors);
        }
    }
}
