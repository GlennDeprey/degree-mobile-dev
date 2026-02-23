using Mde.Project.Api.Core.Entities.Warehouses;
using Mde.Project.Api.Core.Services.Interfaces;
using Mde.Project.Api.Core.Services.RequestModel;
using Mde.Project.Api.Dtos.Products;
using Mde.Project.Api.Dtos.Reports;
using Mde.Project.Api.Dtos.Scanners;
using Mde.Project.Api.Dtos.Warehouses;
using Microsoft.AspNetCore.SignalR;

namespace Mde.Project.Api.Hubs
{
    public class WarehouseHub : Hub
    {
        private readonly IHubContext<WarehouseHub> _hubContext;
        private readonly IWarehouseService _warehouseService;
        private readonly IStatisticsService _statisticsService;
        private readonly IProductService _productService;
        private readonly IReportsService _reportsService;
        public WarehouseHub(IHubContext<WarehouseHub> hubContext, IWarehouseService warehouseService, IProductService productService, IReportsService reportsService, IStatisticsService statisticsService)
        {
            _hubContext = hubContext;
            _warehouseService = warehouseService;
            _productService = productService;
            _reportsService = reportsService;
            _statisticsService = statisticsService;
        }

        public override async Task OnConnectedAsync()
        {
            // Retrieve the warehouseId from the query string
            var httpContext = Context.GetHttpContext();
            var warehouseId = httpContext?.Request.Query["warehouseId"].ToString();

            if (!string.IsNullOrEmpty(warehouseId))
            {
                // Add the client to the group for the specified warehouse
                await Groups.AddToGroupAsync(Context.ConnectionId, warehouseId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Retrieve the warehouseId from the query string
            var httpContext = Context.GetHttpContext();
            var warehouseId = httpContext?.Request.Query["warehouseId"].ToString();

            if (!string.IsNullOrEmpty(warehouseId))
            {
                // Remove the client from the group for the specified warehouse
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, warehouseId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendOrderRequestAsync(Guid warehouseId, IEnumerable<ScannerProductDto> scannedProducts)
        {
            if (!await ValidateWarehouseExists(warehouseId, $"Warehouse with ID {warehouseId} not found."))
                return;

            if (scannedProducts == null || !scannedProducts.Any())
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, "No products scanned.");
                return;
            }

            var productList = scannedProducts.ToList();
            var allProductsAvailable = true;

            foreach (var scannedProduct in productList)
            {
                if (scannedProduct.Quantity <= 0)
                {
                    await Clients.Caller.SendAsync(HubConstants.ReceiveError, $"Invalid quantity for product {scannedProduct.ProductId}.");
                    return;
                }

                var warehouseItem = await _warehouseService.Items.GetByExpressionAsync(p => p.ProductId == scannedProduct.ProductId && p.WarehouseId == warehouseId);
                if (!warehouseItem.Success || warehouseItem.Data == null)
                {
                    await Clients.Caller.SendAsync(HubConstants.ReceiveError, $"Product with ID {scannedProduct.ProductId} not found in the warehouse.");
                    return;
                }

                if (warehouseItem.Data.Quantity < scannedProduct.Quantity)
                {
                    allProductsAvailable = false;
                    scannedProduct.Quantity = warehouseItem.Data.Quantity;
                }
            }

            if (!allProductsAvailable)
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, $"Insufficient stock for some product, the max amount of the product quantity has been updated.");
                await Clients.Caller.SendAsync(HubConstants.ReceiveUpdatedCart, scannedProducts);
                return;
            }

            try
            {
                var updateTasks = productList
                    .Select(p => UpdateWarehouseProductAsync(warehouseId, p.ProductId, -p.Quantity, "Order", true));
                await Task.WhenAll(updateTasks);
            }
            catch
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, "An error occurred while updating products.");
            }
        }

        public async Task AddWarehouseProductAsync(Guid warehouseId, WarehouseItemCreationDto warehouseItemCreation, string tag)
        {
            if (!await ValidateWarehouseExists(warehouseId, $"Warehouse with ID {warehouseId} not found."))
                return;

            if (!await ValidateProductExists(warehouseItemCreation.ProductId, $"Product with ID {warehouseItemCreation.ProductId} not found."))
                return;

            var existingWarehouseItem = await _warehouseService.Items.GetByExpressionAsync(wi => wi.WarehouseId == warehouseId && wi.ProductId == warehouseItemCreation.ProductId);
            if (existingWarehouseItem.Data != null)
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, "Item already exists in the warehouse.");
                return;
            }

            var warehouseItem = new WarehouseItem
            {
                ProductId = warehouseItemCreation.ProductId,
                WarehouseId = warehouseId,
                Quantity = warehouseItemCreation.Quantity
            };

            var createdWarehouseItem = await _warehouseService.Items.AddAsync(warehouseItem);
            if (!createdWarehouseItem.Success)
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, "Failed to add the item.");
                return;
            }

            await NotifyWarehouseProductAdd(warehouseId, warehouseItemCreation.ProductId, "Manual");
        }
        public async Task UpdateWarehouseProductAsync(Guid warehouseId, Guid productId, int delta, string tag, bool soldProduct = false)
        {
            if (delta == 0)
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, "Delta cannot be zero.");
                return;
            }

            if (!await ValidateWarehouseExists(warehouseId, $"Warehouse with ID {warehouseId} not found."))
                return;

            var warehouseItem = await _warehouseService.Items.GetByExpressionAsync(p => p.ProductId == productId && p.WarehouseId == warehouseId);
            if (!warehouseItem.Success || warehouseItem.Data == null)
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, $"Product with ID {productId} not found in the warehouse.");
                return;
            }

            if (warehouseItem.Data.Quantity + delta < 0)
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, "Insufficient stock to complete the operation.");
                return;
            }

            warehouseItem.Data.Quantity += delta;
            var updatedWarehouseItem = await _warehouseService.Items.UpdateAsync(warehouseItem.Data);
            if (!updatedWarehouseItem.Success)
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, "Failed to update the product quantity.");
                return;
            }

            var newReportModel = new ReportCreationDto
            {
                WarehouseId = warehouseId,
                ProductId = productId,
                QuantityChange = delta,
                Tag = tag
            };
            await AddNewReportAsync(newReportModel);

            if (delta > 0)
            {
                await UpsertWarehouseStatsAsync(
                    warehouseId,
                    saleDelta: soldProduct ? delta : 0,
                    restockDelta: soldProduct ? 0 : delta
                );
            }

            await NotifyWarehouseProductUpdate(warehouseId, productId);
        }

        public async Task TransferWarehouseProductAsync(Guid sourceWarehouseId, Guid destinationWarehouseId, Guid productId, int quantity)
        {
            if (quantity <= 0)
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, "Quantity must be greater than zero.");
                return;
            }

            if (sourceWarehouseId == Guid.Empty || destinationWarehouseId == Guid.Empty || productId == Guid.Empty)
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, "Invalid warehouse or product ID.");
                return;
            }

            if (sourceWarehouseId == destinationWarehouseId)
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, "Source and destination warehouses cannot be the same.");
                return;
            }

            if (!await ValidateWarehouseExists(sourceWarehouseId, "Source warehouse not found.") ||
                !await ValidateWarehouseExists(destinationWarehouseId, "Destination warehouse not found."))
            {
                return;
            }

            bool destinationHasProduct = false;
            var destinationWarehouseItem = await _warehouseService.Items.GetByExpressionAsync(p => p.ProductId == productId && p.WarehouseId == destinationWarehouseId);
            if (destinationWarehouseItem.Success)
            {
                destinationHasProduct = true;
            }

            var transferResult = await _warehouseService.Items.TryTransferStockAsync(sourceWarehouseId, destinationWarehouseId, productId, quantity);
            if (!transferResult.Success)
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, "Failed to transfer stock.");
                return;
            }

            var newReportModel = new ReportCreationDto
            {
                WarehouseId = sourceWarehouseId,
                DestinationWarehouseId = destinationWarehouseId,
                ProductId = productId,
                QuantityChange = -quantity,
                Tag = "Transfer"
            };
            await AddNewReportAsync(newReportModel);

            await NotifyWarehouseProductUpdate(sourceWarehouseId, productId);

            if (destinationHasProduct)
            {
                var updateReportModel = new ReportCreationDto
                {
                    WarehouseId = destinationWarehouseId,
                    DestinationWarehouseId = sourceWarehouseId,
                    ProductId = productId,
                    QuantityChange = quantity,
                    Tag = "Transfer"
                };
                await AddNewReportAsync(newReportModel);

                await NotifyWarehouseProductUpdate(destinationWarehouseId, productId);
            }
            else
            {
                await NotifyWarehouseProductAdd(destinationWarehouseId, productId, "Transfer", sourceWarehouseId);
            }

            await Clients.Caller.SendAsync(HubConstants.ReceiveSuccess, "Product transfer completed successfully.");
        }

        public async Task AddNewReportAsync(ReportCreationDto reportCreationDto)
        {
            var reportCreation = new ReportCreationModel
            {
                WarehouseId = reportCreationDto.WarehouseId,
                ProductId = reportCreationDto.ProductId,
                DestinationWarehouseId = reportCreationDto.DestinationWarehouseId,
                Tag = reportCreationDto.Tag,
                QuantityChange = reportCreationDto.QuantityChange
            };

            var result = await _reportsService.CreateReportAsync(reportCreation);
            if (result.Success)
            {
                var reportDto = new ReportDto
                {
                    Id = result.Data.Id,
                    WarehouseId = result.Data.WarehouseId.Value,
                    ProductId = result.Data.ProductId.Value,
                    QuantityChange = result.Data.QuantityChange,
                    Description = result.Data.Description,
                    CreatedOn = result.Data.CreatedOn,
                };

                await _hubContext.Clients.Group(reportCreationDto.WarehouseId.ToString())
                    .SendAsync(HubConstants.ReportReceiveNewReport, reportDto);
                await Clients.Caller.SendAsync(HubConstants.ReceiveSuccess, "Report added successfully.");
            }
            else
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, "Failed to add a new report.");
            }
        }

        private async Task UpsertWarehouseStatsAsync(Guid warehouseId, int saleDelta = 0, int restockDelta = 0)
        {
            if (!await ValidateWarehouseExists(warehouseId, "Warehouse not found."))
            {
                return;
            }

            if (saleDelta == 0 && restockDelta == 0)
            {
                return;
            }

            var warehouseStats = await _statisticsService.GetByExpressionAsync(s => s.WarehouseId == warehouseId && s.CreatedOn.Date == DateTime.Now.Date);
            if (warehouseStats.Data == null)
            {
                var createStatsModel = new WarehouseStatsCreationModel
                {
                    WarehouseId = warehouseId,
                    TotalSales = saleDelta,
                    TotalRestock = restockDelta
                };

                var result = await _statisticsService.CreateWarehouseStatsAsync(createStatsModel);
                if (!result.Success)
                {
                    await Clients.Caller.SendAsync(HubConstants.ReceiveError, "Failed to add warehouse statistics.");
                }
                await Clients.Group(warehouseId.ToString())
                        .SendAsync(HubConstants.StatisticsReceiveWarehouseStats, result.Data);
                return;
            }
            
            var updateStatsModel = new WarehouseStatsUpdateModel
            {
                Id = warehouseStats.Data.Id,
                WarehouseId = warehouseId,
                SalesDelta = saleDelta,
                RestockDelta = restockDelta
            };

            var updateResult = await _statisticsService.UpdateWarehouseStatsAsync(updateStatsModel);
            if (!updateResult.Success)
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, "Failed to update warehouse statistics.");
                return;
            }

            await Clients.Group(warehouseId.ToString())
                        .SendAsync(HubConstants.StatisticsReceiveWarehouseStats, updateResult.Data);
        }

        private async Task<WarehouseItem> AutoRefillProductAsync(WarehouseItem item)
        {
            if (item != null)
            {
                item.Quantity += item.RefillQuantity;
                var updatedWarehouseItem = await _warehouseService.Items.UpdateAsync(item);
                if (!updatedWarehouseItem.Success)
                {
                    await Clients.Caller.SendAsync(HubConstants.ReceiveError, "Failed to autofill update the product quantity.");
                    item.Quantity -= item.RefillQuantity;
                }

                var newReportModel = new ReportCreationDto
                {
                    WarehouseId = item.WarehouseId,
                    ProductId = item.ProductId,
                    QuantityChange = item.RefillQuantity,
                    Tag = "Autofill"
                };
                await AddNewReportAsync(newReportModel);

                await UpsertWarehouseStatsAsync(item.WarehouseId, restockDelta: item.RefillQuantity);

                return item;
            }

            return null;
        }

        private async Task<bool> ValidateWarehouseExists(Guid warehouseId, string errorMessage)
        {
            var warehouseResult = await _warehouseService.ExistsAsync(w => w.Id == warehouseId);
            if (!warehouseResult.Success || warehouseResult.Data == false)
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, errorMessage);
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateProductExists(Guid productId, string errorMessage)
        {
            var productResult = await _productService.GetByIdAsync(productId);
            if (!productResult.Success)
            {
                await Clients.Caller.SendAsync(HubConstants.ReceiveError, errorMessage);
                return false;
            }
            return true;
        }

        private async Task NotifyWarehouseProductUpdate(Guid warehouseId, Guid productId)
        {
            var warehouseItem = await _warehouseService.Items.GetByExpressionAsync(p => p.ProductId == productId && p.WarehouseId == warehouseId);
            if (warehouseItem.Success && warehouseItem.Data != null)
            {
                var newQuantity = warehouseItem.Data.Quantity;
                if (warehouseItem.Data.HasAutoRefill == true && warehouseItem.Data.Quantity < warehouseItem.Data.MinimumQuantity)
                {
                    var refilledItem = await AutoRefillProductAsync(warehouseItem.Data);
                    if (refilledItem != null)
                    {
                        newQuantity = refilledItem.Quantity;
                    }
                }

                await _hubContext.Clients.Group(warehouseId.ToString())
                    .SendAsync(HubConstants.StockReceiveProductUpdate, warehouseId, productId, newQuantity);
            }
        }

        private async Task NotifyWarehouseProductAdd(Guid warehouseId, Guid productId, string tag, Guid? destinationWarehouseId = null)
        {
            var warehouseItem = await _warehouseService.Items.GetByExpressionAsync(p => p.ProductId == productId && p.WarehouseId == warehouseId, includeProperties: "Product,Product.Brand,Product.Category");
            if (warehouseItem.Success && warehouseItem.Data != null)
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

                await AddNewReportAsync(new ReportCreationDto
                {
                    WarehouseId = warehouseId,
                    DestinationWarehouseId = destinationWarehouseId,
                    ProductId = productId,
                    QuantityChange = warehouseItem.Data.Quantity,
                    Tag = tag
                });

                await UpsertWarehouseStatsAsync(warehouseId, restockDelta: warehouseItem.Data.Quantity);

                await _hubContext.Clients.Group(warehouseId.ToString())
                    .SendAsync(HubConstants.StockReceiveProductAdd, warehouseId, warehouseItemDto);
            }
        }
    }
}
