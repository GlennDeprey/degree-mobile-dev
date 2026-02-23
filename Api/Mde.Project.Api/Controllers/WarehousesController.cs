using Mde.Project.Api.Dtos.Products;
using Mde.Project.Api.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Mde.Project.Api.Dtos.Warehouses;
using Mde.Project.Api.Core.Entities.Warehouses;

namespace Mde.Project.Api.Controllers
{
    [Route("api/warehouses")]
    [ApiController]
    [Authorize]
    public class WarehousesController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IWarehouseService _warehouseService;
        private readonly int _itemsPage;
        public WarehousesController(IWarehouseService warehouseService, IProductService productService)
        {
            _warehouseService = warehouseService;
            _productService = productService;
            _itemsPage = 10;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<WarehouseListDto>> GetWarehousesAsync([FromQuery] string name, [FromQuery] bool withPagination = false, [FromQuery] int pageNumber = 1)
        {
            var warehouseList = new WarehouseListDto();
            var filterExpression = string.IsNullOrEmpty(name) ? null : (Expression<Func<Warehouse, bool>>)(c => c.Name.Contains(name));

            if (withPagination)
            {
                var pagedWarehouses = await _warehouseService.GetAllAsync(filterExpression, pageNumber, _itemsPage);
                if (pagedWarehouses.Success)
                {
                    warehouseList.Items = pagedWarehouses.Data.Select(p => new WarehouseDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ShortName = p.ShortName,
                        Phone = p.Phone,
                        Earnings = p.Earnings
                    });
                    warehouseList.TotalItems = pagedWarehouses.TotalItems;
                    warehouseList.TotalPages = pagedWarehouses.TotalPages;
                    return Ok(warehouseList);
                }
                return NotFound(pagedWarehouses.Errors);
            }

            var warehouses = await _warehouseService.GetAllAsync(filterExpression);
            if (warehouses.Success)
            {
                warehouseList.Items = warehouses.Data.Select(p => new WarehouseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ShortName = p.ShortName,
                    Phone = p.Phone,
                    Earnings = p.Earnings
                });
                return Ok(warehouseList);
            }
            return NotFound(warehouses.Errors);
        }

        [HttpGet("{warehouseId:Guid}", Name = "GetWarehouseById")]
        public async Task<ActionResult<WarehouseDetailDto>> GetWarehouseByIdAsync(Guid warehouseId)
        {
            var warehouse = await _warehouseService.GetByIdAsync(warehouseId, includeProperties: "LocationInfo,WarehouseItems,WarehouseItems.Product,GoogleInfo,GoogleInfo.GooglePhotoUris");
            if (warehouse.Success)
            {
                var warehouseDetailDto = new WarehouseDetailDto
                {
                    Id = warehouse.Data.Id,
                    Name = warehouse.Data.Name,
                    ShortName = warehouse.Data.ShortName,
                    Phone = warehouse.Data.Phone,
                    Location = new WarehouseLocationDto
                    {
                        Id = warehouse.Data.LocationInfo.Id,
                        Address = warehouse.Data.LocationInfo.Address,
                        City = warehouse.Data.LocationInfo.City,
                        State = warehouse.Data.LocationInfo.State,
                        Country = warehouse.Data.LocationInfo.Country,
                        PostalCode = warehouse.Data.LocationInfo.PostalCode,
                        Longitude = warehouse.Data.LocationInfo.Longitude,
                        Latitude = warehouse.Data.LocationInfo.Latitude,
                    },
                    GoogleInfo = new WarehouseGoogleDto
                    {
                        GoogleAddress = warehouse.Data.GoogleInfo?.GoogleAddress ?? "",
                        GoogleAddressId = warehouse.Data.GoogleInfo?.GoogleAddressId ?? "",
                        GooglePhotoUris = warehouse.Data.GoogleInfo?.GooglePhotoUris ?? new List<WarehousePhoto>(),
                    },
                    Stock = new WarehouseStockDto
                    {
                        TotalItems = warehouse.Data.WarehouseItems.Sum(item => item.Quantity),
                        LowestItemPrice = warehouse.Data.WarehouseItems.Select(item => item.Product.SalesPrice).DefaultIfEmpty(0).Min(),
                        HighestItemPrice = warehouse.Data.WarehouseItems.Select(item => item.Product.SalesPrice).DefaultIfEmpty(0).Max(),
                        AverageItemPrice = warehouse.Data.WarehouseItems.Select(item => item.Product.SalesPrice).DefaultIfEmpty(0).Average()
                    },
                    Earnings = warehouse.Data.Earnings
                };

                return Ok(warehouseDetailDto);
            }

            return NotFound(warehouse.Errors);
        }

        [HttpPost]
        public async Task<ActionResult<BrandDto>> CreateWarehouseAsync(WarehouseCreationDto warehouseCreationDto)
        {
            var warehouse = new Warehouse()
            {
                Name = warehouseCreationDto.Name,
                ShortName = warehouseCreationDto.ShortName,
                Phone = warehouseCreationDto.Phone,
                LocationInfo = new WarehouseLocationInfo
                {
                    Address = warehouseCreationDto.Location.Address,
                    City = warehouseCreationDto.Location.City,
                    State = warehouseCreationDto.Location.State,
                    Country = warehouseCreationDto.Location.Country,
                    PostalCode = warehouseCreationDto.Location.PostalCode,
                    Longitude = warehouseCreationDto.Location.Longitude,
                    Latitude = warehouseCreationDto.Location.Latitude,
                },
                GoogleInfo = new WarehouseGoogleInfo()
                {
                    GoogleAddress = warehouseCreationDto.GoogleInfo.GoogleAddress ?? "",
                    GoogleAddressId = warehouseCreationDto.GoogleInfo.GoogleAddressId ?? "",
                    GooglePhotoUris = warehouseCreationDto.GoogleInfo.GooglePhotoUris ?? new List<WarehousePhoto>(),
                },
            };

            var createdWarehouse = await _warehouseService.AddAsync(warehouse);
            if (createdWarehouse.Success)
            {
                var warehouseDto = new WarehouseDto()
                {
                    Id = warehouse.Id,
                    Name = warehouse.Name,
                    ShortName = warehouse.ShortName,
                    Phone = warehouse.Phone,
                    Location = new WarehouseLocationDto
                    {
                        Id = warehouse.LocationInfo.Id,
                        Address = warehouse.LocationInfo.Address,
                        City = warehouse.LocationInfo.City,
                        State = warehouse.LocationInfo.State,
                        Country = warehouse.LocationInfo.Country,
                        PostalCode = warehouse.LocationInfo.PostalCode,
                        Longitude = warehouse.LocationInfo.Longitude,
                        Latitude = warehouse.LocationInfo.Latitude,
                    },
                    GoogleInfo = new WarehouseGoogleDto()
                    {
                        GoogleAddress = warehouseCreationDto.GoogleInfo.GoogleAddress ?? "",
                        GoogleAddressId = warehouseCreationDto.GoogleInfo.GoogleAddressId ?? "",
                        GooglePhotoUris = warehouseCreationDto.GoogleInfo.GooglePhotoUris ?? new List<WarehousePhoto>(),
                    },
                    Earnings = warehouse.Earnings
                };

                return CreatedAtRoute("GetWarehouseById", new { warehouseId = warehouseDto.Id }, warehouseDto);
            }

            return BadRequest(createdWarehouse.Errors);
        }

        [HttpPut("{warehouseId:Guid}")]
        public async Task<ActionResult> UpdateWarehouseAsync(Guid warehouseId, WarehouseUpdateDto warehouseUpdateDto)
        {
            var warehouse = await _warehouseService.GetByIdAsync(warehouseId, includeProperties: "LocationInfo,WarehouseItems");
            if (!warehouse.Success)
            {
                return NotFound(warehouse.Errors);
            }

            warehouse.Data.Name = warehouseUpdateDto.Name;
            warehouse.Data.ShortName = warehouseUpdateDto.ShortName;
            warehouse.Data.Phone = warehouseUpdateDto.Phone;

            warehouse.Data.LocationInfo.Address = warehouseUpdateDto.Location.Address;
            warehouse.Data.LocationInfo.City = warehouseUpdateDto.Location.City;
            warehouse.Data.LocationInfo.State = warehouseUpdateDto.Location.State;
            warehouse.Data.LocationInfo.Country = warehouseUpdateDto.Location.Country;
            warehouse.Data.LocationInfo.PostalCode = warehouseUpdateDto.Location.PostalCode;
            warehouse.Data.LocationInfo.Longitude = warehouseUpdateDto.Location.Longitude;
            warehouse.Data.LocationInfo.Latitude = warehouseUpdateDto.Location.Latitude;

            if (warehouse.Data.GoogleInfo == null)
            {
                warehouse.Data.GoogleInfo = new WarehouseGoogleInfo();
            }

            warehouse.Data.GoogleInfo.GoogleAddress = warehouseUpdateDto.GoogleInfo.GoogleAddress ?? "";
            warehouse.Data.GoogleInfo.GoogleAddressId = warehouseUpdateDto.GoogleInfo.GoogleAddressId ?? "";
            warehouse.Data.GoogleInfo.GooglePhotoUris = warehouseUpdateDto.GoogleInfo.GooglePhotoUris ?? new List<WarehousePhoto>();

            var updatedWarehouse = await _warehouseService.UpdateAsync(warehouse.Data);
            if (updatedWarehouse.Success)
            {
                return NoContent();
            }
            return BadRequest(updatedWarehouse.Errors);
        }

        [HttpDelete("{warehouseId:Guid}")]
        public async Task<ActionResult> DeleteWarehouseAsync(Guid warehouseId)
        {
            var warehouse = await _warehouseService.GetByIdAsync(warehouseId);
            if (!warehouse.Success)
            {
                return NotFound(warehouse.Errors);
            }

            var deletedWarehouse = await _warehouseService.RemoveAsync(warehouseId);
            if (deletedWarehouse.Success)
            {
                return NoContent();
            }
            return BadRequest(deletedWarehouse.Errors);
        }
    }
}
