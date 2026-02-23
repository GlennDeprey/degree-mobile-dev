using Mde.Project.Api.Dtos.Products;
using Mde.Project.Api.Core.Entities.Products;
using Mde.Project.Api.Core.Extensions;
using Mde.Project.Api.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Mde.Project.Api.Core.Services.Files;
using Mde.Project.Api.Core;

namespace Mde.Project.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IFilesService _filesService;
        private readonly int _itemsPage;
        public ProductsController(IProductService productService, IFilesService filesService)
        {
            _productService = productService;
            _filesService = filesService;
            _itemsPage = 10;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<ProductListDto>> GetProductsAsync([FromQuery] string name, [FromQuery] int pageNumber, [FromQuery] Guid? brandId, [FromQuery] Guid? categoryId)
        {
            if (pageNumber <= 0)
            {
                return BadRequest("Page number must be a positive integer.");
            }

            Expression<Func<Product, bool>> filter = p => true;

            if (brandId.HasValue)
            {
                filter = filter.AndAlso(p => p.BrandId == brandId.Value);
            }

            if (categoryId.HasValue)
            {
                filter = filter.AndAlso(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                filter = filter.AndAlso(p => p.Name.Contains(name));
            }

            var products = await _productService.GetAllAsync(filter, pageNumber: pageNumber, pageSize: _itemsPage, includeProperties: "Category,SalesTax,Brand");
            if (products.Success)
            {
                var productListDto = new ProductListDto
                {
                    Items = products.Data.Select(p => new ProductListItemDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Brand = new BrandDto
                        {
                            Id = p.Brand.Id,
                            Name = p.Brand.Name
                        },
                        Image = p.Image,
                        Category = new CategoryDto
                        {
                            Id = p.Category.Id,
                            Name = p.Category.Name
                        }
                    }),
                    TotalItems = products.TotalItems,
                    TotalPages = products.TotalPages
                };

                return Ok(productListDto);
            }
            return NotFound(products.Errors);
        }

        [HttpGet("{productId:Guid}", Name = "GetProductById")]
        public async Task<ActionResult<ProductDto>> GetProductByIdAsync(Guid productId)
        {
            var product = await _productService.GetByIdAsync(productId, includeProperties: "Category,SalesTax,Brand");
            if (product.Success)
            {
                var productDto = new ProductDto
                {
                    Id = product.Data.Id,
                    Name = product.Data.Name,
                    Brand = new BrandDto
                    {
                        Id = product.Data.Brand.Id,
                        Name = product.Data.Brand.Name
                    },
                    Image = product.Data.Image,
                    Description = product.Data.Description,
                    Price = product.Data.SalesPrice,
                    SalesTax = new TaxDto
                    {
                        Id = product.Data.SalesTax.Id,
                        Name = product.Data.SalesTax.Name,
                        TaxRate = product.Data.SalesTax.TaxRate
                    },
                    Category = new CategoryDto
                    {
                        Id = product.Data.Category.Id,
                        Name = product.Data.Category.Name
                    },
                    Barcode = product.Data.Barcode
                };

                return Ok(productDto);
            }

            return NotFound(product.Errors);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProductAsync([FromForm] ProductCreationDto productCreationDto)
        {
            var product = new Product
            {
                Name = productCreationDto.Name,
                BrandId = productCreationDto.BrandId,
                Image = ApiConstants.DefaultImage,
                Description = productCreationDto.Description ?? "",
                SalesPrice = productCreationDto.Price,
                SalesTaxId = productCreationDto.SalesTaxId,
                CategoryId = productCreationDto.CategoryId,
                Barcode = await _productService.GenerateBarcodeAsync()
            };

            if (productCreationDto.ImageFile != null)
            {
                if (!_filesService.IsValidImageFile(productCreationDto.ImageFile.FileName, productCreationDto.ImageFile.ContentType))
                {
                    return BadRequest("Invalid image file type.");
                }

                var savedFilePath = "";

                using (var stream = productCreationDto.ImageFile.OpenReadStream())
                {
                    savedFilePath = await _filesService.SaveFileAsync(stream, productCreationDto.ImageFile.FileName);
                }

                if (!string.IsNullOrEmpty(savedFilePath))
                {
                    product.Image = Path.GetFileName(savedFilePath);
                }
            }

            var createdProduct = await _productService.AddAsync(product);
            if (createdProduct.Success)
            {
                var productDto = new ProductDto
                {
                    Id = createdProduct.Data.Id,
                    Name = createdProduct.Data.Name,
                    Brand = new BrandDto
                    {
                        Id = createdProduct.Data.Brand.Id,
                        Name = createdProduct.Data.Brand.Name
                    },
                    Image = createdProduct.Data.Image,
                    Description = createdProduct.Data.Description,
                    Price = createdProduct.Data.SalesPrice,
                    SalesTax = new TaxDto
                    {
                        Id = createdProduct.Data.SalesTax.Id,
                        Name = createdProduct.Data.SalesTax.Name,
                        TaxRate = createdProduct.Data.SalesTax.TaxRate
                    },
                    Category = new CategoryDto
                    {
                        Id = createdProduct.Data.Category.Id,
                        Name = createdProduct.Data.Category.Name
                    },
                    Barcode = createdProduct.Data.Barcode
                };

                return CreatedAtRoute("GetProductById", new { productId = productDto.Id }, productDto);
            }

            return BadRequest(createdProduct.Errors);
        }

        [HttpPut("{productId:Guid}")]
        public async Task<ActionResult> UpdateProductAsync(Guid productId, [FromForm] ProductUpdateDto productUpdateDto)
        {
            var product = await _productService.GetByIdAsync(productId);
            if (!product.Success)
            {
                return NotFound(product.Errors);
            }

            if (productUpdateDto.ImageFile != null)
            {
                if (!_filesService.IsValidImageFile(productUpdateDto.ImageFile.FileName, productUpdateDto.ImageFile.ContentType))
                {
                    return BadRequest("Invalid image file type.");
                }

                var savedFilePath = "";

                using (var stream = productUpdateDto.ImageFile.OpenReadStream())
                {
                    savedFilePath = await _filesService.SaveFileAsync(stream, productUpdateDto.ImageFile.FileName);
                }

                if (!string.IsNullOrEmpty(savedFilePath))
                {
                    if (product.Data.Image != null && product.Data.Image != ApiConstants.DefaultImage)
                    {
                        await _filesService.RemoveFileIfExistsAsync(product.Data.Image);
                    }

                    productUpdateDto.Image = Path.GetFileName(savedFilePath);
                }
            }

            if (!string.IsNullOrWhiteSpace(productUpdateDto.Image))
            {
                product.Data.Image = productUpdateDto.Image;
            }

            product.Data.Name = productUpdateDto.Name;
            product.Data.BrandId = productUpdateDto.BrandId;
            product.Data.Description = productUpdateDto.Description ?? "";
            product.Data.SalesPrice = productUpdateDto.Price;
            product.Data.SalesTaxId = productUpdateDto.SalesTaxId;
            product.Data.CategoryId = productUpdateDto.CategoryId;

            var updatedProduct = await _productService.UpdateAsync(product.Data);
            if (updatedProduct.Success)
            {
                return NoContent();
            }
            return BadRequest(updatedProduct.Errors);
        }

        [HttpDelete("{productId:Guid}")]
        public async Task<ActionResult> DeleteProduct(Guid productId)
        {
            var product = await _productService.GetByIdAsync(productId);
            if (!product.Success)
            {
                return NotFound(product.Errors);
            }

            var deletedProduct = await _productService.RemoveAsync(productId);
            if (deletedProduct.Success)
            {
                return NoContent();
            }
            return BadRequest(deletedProduct.Errors);
        }

        [HttpGet("{productId:Guid}/pdf")]
        public async Task<ActionResult> GetProductPdfAsync(Guid productId)
        {
            var product = await _productService.GetByIdAsync(productId, includeProperties: "SalesTax");
            if (!product.Success)
            {
                return NotFound(product.Errors);
            }
            var pdfBytes = await _filesService.GenerateProductPdfAsync(product.Data);
            if (pdfBytes == null || pdfBytes.Length == 0)
            {
                return NotFound("PDF generation failed.");
            }
            return File(pdfBytes, "application/pdf", $"{product.Data.Name}.pdf");
        }
    }
}
