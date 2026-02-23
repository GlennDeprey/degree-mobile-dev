using Mde.Project.Api.Core.Entities.Warehouses;
using Mde.Project.Api.Core.Services.Interfaces;
using Mde.Project.Api.Dtos.Products;
using Mde.Project.Api.Dtos.Warehouses;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Mde.Project.Api.Core.Extensions;
using Mde.Project.Api.Core.Services.Files;

namespace Mde.Project.Api.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        private readonly IProductService _productService;
        private readonly IFilesService _filesService;
        private readonly int _itemsPage;

        public ItemsController(IWarehouseService warehouseService, IProductService productService, IFilesService filesService)
        {
            _warehouseService = warehouseService;
            _productService = productService;
            _filesService = filesService;
            _itemsPage = 10;
        }


        [HttpGet("{warehouseId:Guid}/products")]
        public async Task<ActionResult<WarehouseProductListDto>> GetWarehouseProductsByIdAsync([FromQuery] string name, [FromQuery] int pageNumber, Guid warehouseId, [FromQuery] Guid? brandId, [FromQuery] Guid? categoryId)
        {
            if (pageNumber <= 0)
            {
                return BadRequest("Page number must be a positive integer.");
            }

            Expression<Func<WarehouseItem, bool>> filter = p => p.WarehouseId == warehouseId;

            if (brandId.HasValue)
            {
                filter = filter.AndAlso(p => p.Product.BrandId == brandId.Value);
            }

            if (categoryId.HasValue)
            {
                filter = filter.AndAlso(p => p.Product.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                filter = filter.AndAlso(p => p.Product.Name.Contains(name));
            }

            var warehouseItems = await _warehouseService.Items.GetAllAsync(filter, pageNumber, _itemsPage, includeProperties: "Product,Product.Brand,Product.Category");
            if (warehouseItems.Success)
            {
                var warehouseProductListDto = new WarehouseProductListDto
                {
                    Items = warehouseItems.Data.Select(i => new WarehouseItemDto
                    {
                        Id = i.Id,
                        WarehouseId = i.WarehouseId,
                        Product = new WarehouseProductDto
                        {
                            Id = i.Product.Id,
                            Name = i.Product.Name,
                            Brand = new BrandDto
                            {
                                Id = i.Product.Brand.Id,
                                Name = i.Product.Brand.Name
                            },
                            Image = i.Product.Image,
                            Price = i.Product.SalesPrice,
                            Category = new CategoryDto
                            {
                                Id = i.Product.Category.Id,
                                Name = i.Product.Category.Name
                            }
                        },
                        Quantity = i.Quantity,
                        MinimumQuantity = i.MinimumQuantity,
                        RefillQuantity = i.RefillQuantity,
                        HasAutoRefill = i.HasAutoRefill
                    }),
                    TotalItems = warehouseItems.TotalItems,
                    TotalPages = warehouseItems.TotalPages
                };

                return Ok(warehouseProductListDto);
            }

            return NotFound(warehouseItems.Errors);
        }

        [HttpGet("{warehouseId:Guid}/products/{productId:Guid}", Name = "GetWarehouseProductById")]
        public async Task<ActionResult<WarehouseItemDto>> GetWarehouseProductByIdAsync(Guid warehouseId, Guid productId)
        {
            var warehouseItem = await _warehouseService.Items.GetByExpressionAsync(p => p.ProductId == productId, includeProperties: "Product,Product.Brand,Product.Category");
            if (warehouseItem.Success)
            {
                var warehouseItemDto = new WarehouseItemDto
                {
                    Id = warehouseItem.Data.Id,
                    Product = new WarehouseProductDto
                    {
                        Id = warehouseItem.Data.Product.Id,
                        Name = warehouseItem.Data.Product.Name,
                        Brand = new BrandDto
                        {
                            Id = warehouseItem.Data.Product.Brand.Id,
                            Name = warehouseItem.Data.Product.Brand.Name
                        },
                        Image = warehouseItem.Data.Product.Image,
                        Price = warehouseItem.Data.Product.SalesPrice,
                        Category = new CategoryDto
                        {
                            Id = warehouseItem.Data.Product.Category.Id,
                            Name = warehouseItem.Data.Product.Category.Name
                        }
                    },
                    Quantity = warehouseItem.Data.Quantity,
                    MinimumQuantity = warehouseItem.Data.MinimumQuantity,
                    RefillQuantity = warehouseItem.Data.RefillQuantity,
                    HasAutoRefill = warehouseItem.Data.HasAutoRefill
                };
                return Ok(warehouseItemDto);
            }
            return NotFound(warehouseItem.Errors);
        }

        [HttpGet("{warehouseId:Guid}/products/{barcode}")]
        public async Task<ActionResult<WarehouseItemDto>> GetWarehouseProductByBarcodeAsync(Guid warehouseId, string barcode, [FromQuery] int? currentCount = null)
        {
            Expression<Func<WarehouseItem, bool>> expression = wi => wi.WarehouseId == warehouseId && wi.Product.Barcode == barcode;

            if (currentCount.HasValue && currentCount.Value > 0)
            {
                expression = expression.AndAlso(wi => wi.Quantity >= currentCount + 1);
            }

            var warehouseItem = await _warehouseService.Items.GetByExpressionAsync(expression, includeProperties: "Product,Product.Brand,Product.Category,Product.SalesTax");
            if (warehouseItem.Success)
            {
                var warehouseItemDto = new WarehouseItemDto
                {
                    Id = warehouseItem.Data.WarehouseId,
                    Product = new WarehouseProductDto
                    {
                        Id = warehouseItem.Data.ProductId,
                        Name = warehouseItem.Data.Product.Name,
                        Brand = new BrandDto
                        {
                            Id = warehouseItem.Data.Product.Brand.Id,
                            Name = warehouseItem.Data.Product.Brand.Name
                        },
                        Image = warehouseItem.Data.Product.Image,
                        Price = warehouseItem.Data.Product.SalesPrice,
                        Category = new CategoryDto
                        {
                            Id = warehouseItem.Data.Product.Category.Id,
                            Name = warehouseItem.Data.Product.Category.Name
                        },
                        SalesTax = new TaxDto
                        {
                            Id = warehouseItem.Data.Product.SalesTax.Id,
                            Name = warehouseItem.Data.Product.SalesTax.Name,
                            TaxRate = warehouseItem.Data.Product.SalesTax.TaxRate
                        }
                    },
                    Quantity = warehouseItem.Data.Quantity,
                };
                return Ok(warehouseItemDto);
            }
            return NotFound("Warehouse does not have any more stock of the scanned item.");
        }

        [HttpGet("{productId:Guid}/warehouses")]
        public async Task<ActionResult<IEnumerable<WarehouseItemStockDto>>> GetWarehousesWithStock(Guid productId, [FromQuery] Guid? excludeWarehouseId = null, [FromQuery] int minQuantity = 1)
        {

            Expression<Func<WarehouseItem, bool>> filter = wi => wi.ProductId == productId && wi.Quantity >= minQuantity;

            if (excludeWarehouseId.HasValue)
            {
                filter = filter.AndAlso(wi => wi.WarehouseId != excludeWarehouseId.Value);
            }

            var result = await _warehouseService.Items.GetAllAsync(
                filter, includeProperties: "Warehouse");
            if (!result.Success)
            {
                return NotFound("There was a issue contacting the server.");
            }

            var warehouses = result.Data
                .Select(wi => new WarehouseItemStockDto
                {
                    Id = wi.Warehouse.Id,
                    Name = wi.Warehouse.Name,
                    Quantity = wi.Quantity
                })
                .DistinctBy(w => w.Id);

            return Ok(warehouses);
        }

        [HttpPost("{warehouseId:Guid}/products")]
        public async Task<ActionResult<WarehouseItemDto>> AddWarehouseProductAsync(Guid warehouseId, WarehouseItemCreationDto warehouseItemCreationDto)
        {
            var warehouse = await _warehouseService.GetByIdAsync(warehouseId);
            if (!warehouse.Success)
            {
                return NotFound(warehouse.Errors);
            }
            var product = await _productService.GetByIdAsync(warehouseItemCreationDto.ProductId);
            if (!product.Success)
            {
                return NotFound(product.Errors);
            }

            var existingWarehouseItem = await _warehouseService.Items.GetByExpressionAsync(wi => wi.WarehouseId == warehouseId && wi.ProductId == warehouseItemCreationDto.ProductId);
            if (existingWarehouseItem != null)
            {
                return BadRequest("Item already exists in the warehouse.");
            }

            var warehouseItem = new WarehouseItem
            {
                ProductId = warehouseItemCreationDto.ProductId,
                WarehouseId = warehouseId,
                Quantity = warehouseItemCreationDto.Quantity,
                MinimumQuantity = 0,
                RefillQuantity = 0,
                HasAutoRefill = false
            };
            var createdWarehouseItem = await _warehouseService.Items.AddAsync(warehouseItem);
            if (createdWarehouseItem.Success)
            {
                var loadedWarehouseItem = await _warehouseService.Items.GetByIdAsync(
                    createdWarehouseItem.Data.Id,
                    includeProperties: "Product,Product.Brand,Product.Category"
                );

                if (!loadedWarehouseItem.Success)
                {
                    return BadRequest("Failed to load the created item with navigation properties.");
                }

                var warehouseItemDto = new WarehouseItemDto
                {
                    Id = loadedWarehouseItem.Data.Id,
                    Product = new WarehouseProductDto
                    {
                        Id = loadedWarehouseItem.Data.Product.Id,
                        Name = loadedWarehouseItem.Data.Product.Name,
                        Brand = new BrandDto
                        {
                            Id = loadedWarehouseItem.Data.Product.Brand.Id,
                            Name = loadedWarehouseItem.Data.Product.Brand.Name
                        },
                        Image = loadedWarehouseItem.Data.Product.Image,
                        Price = loadedWarehouseItem.Data.Product.SalesPrice,
                        Category = new CategoryDto
                        {
                            Id = loadedWarehouseItem.Data.Product.Category.Id,
                            Name = loadedWarehouseItem.Data.Product.Category.Name
                        }
                    },
                    Quantity = loadedWarehouseItem.Data.Quantity,
                };

                return CreatedAtAction("GetWarehouseProductById", new { warehouseId, productId = createdWarehouseItem.Data.Id }, warehouseItemDto);
            }
            return BadRequest(createdWarehouseItem.Errors);
        }

        [HttpPut("{warehouseId:Guid}/products/{productId:Guid}")]
        public async Task<ActionResult> UpdateWarehouseProductAsync(Guid warehouseId, Guid productId, WarehouseItemUpdateDto warehouseItemUpdateDto)
        {

            var warehouse = await _warehouseService.GetByIdAsync(warehouseId);
            if (!warehouse.Success)
            {
                return NotFound(warehouse.Errors);
            }

            if (!warehouseItemUpdateDto.IsChangeSettings && warehouseItemUpdateDto.Delta == 0)
            {
                return BadRequest("Delta must be a non-zero value.");
            }

            var warehouseItem = await _warehouseService.Items.GetByExpressionAsync(p => p.ProductId == productId, includeProperties: "Product,Product.Brand,Product.Category");
            if (!warehouseItem.Success)
            {
                return NotFound(warehouseItem.Errors);
            }

            if (warehouseItemUpdateDto.IsChangeSettings)
            {
                warehouseItem.Data.MinimumQuantity = warehouseItemUpdateDto.MinimumQuantity;
                warehouseItem.Data.RefillQuantity = warehouseItemUpdateDto.RefillQuantity;
                warehouseItem.Data.HasAutoRefill = warehouseItemUpdateDto.HasAutoRefill;
            }
            else
            {
                if (warehouseItem.Data.Quantity + warehouseItemUpdateDto.Delta < 0)
                {
                    return BadRequest("Quantity cannot be negative.");
                }

                warehouseItem.Data.Quantity += warehouseItemUpdateDto.Delta;
            }
       
            var updatedWarehouseItem = await _warehouseService.Items.UpdateAsync(warehouseItem.Data);
            if (updatedWarehouseItem.Success)
            {
                return NoContent();
            }
            return BadRequest(updatedWarehouseItem.Errors);
        }

        [HttpDelete("{warehouseId:Guid}/products/{productId:Guid}")]
        public async Task<ActionResult> DeleteWarehouseProductAsync(Guid warehouseId, Guid productId)
        {
            var warehouse = await _warehouseService.GetByIdAsync(warehouseId);
            if (!warehouse.Success)
            {
                return NotFound(warehouse.Errors);
            }
            var warehouseItem = await _warehouseService.Items.GetByIdAsync(productId);
            if (!warehouseItem.Success)
            {
                return NotFound(warehouseItem.Errors);
            }
            var deletedWarehouseItem = await _warehouseService.Items.RemoveAsync(productId);
            if (deletedWarehouseItem.Success)
            {
                return NoContent();
            }
            return BadRequest(deletedWarehouseItem.Errors);
        }

        [HttpGet("{warehouseId:Guid}/pdf")]
        public async Task<ActionResult> GetWarehouseProductsPdfAsync(Guid warehouseId)
        {
            var warehouse = await _warehouseService.GetByIdAsync(warehouseId);
            if (!warehouse.Success)
            {
                return BadRequest(warehouse.Errors);
            }

            var warehouseItems = await _warehouseService.Items.GetAllAsync(i => i.WarehouseId == warehouseId, includeProperties: "Product,Product.SalesTax");
            if (!warehouseItems.Success)
            {
                return BadRequest("There was a issue getting items for warehouse.");
            }

            var products = warehouseItems.Data.Select(i => i.Product).Distinct().ToList();
            if (!products.Any())
            {
                return NotFound("The warehouse does not contain any items.");
            }

            var pdfBytes = await _filesService.GenerateWarehouseProductsPdfAsync(warehouse.Data.Name, products);
            if (pdfBytes == null || pdfBytes.Length == 0)
            {
                return BadRequest("PDF generation failed.");
            }
            return File(pdfBytes, "application/pdf", $"{warehouse.Data.Name}.pdf");
        }
    }
}
