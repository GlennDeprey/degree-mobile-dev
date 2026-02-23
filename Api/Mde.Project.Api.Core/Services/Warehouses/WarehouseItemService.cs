using Mde.Project.Api.Core.Data;
using Mde.Project.Api.Core.Entities.Warehouses;
using Mde.Project.Api.Core.Services.Base;
using Mde.Project.Api.Core.Services.Interfaces.Warehouses;
using Mde.Project.Api.Core.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Mde.Project.Api.Core.Services.Warehouses
{
    public class WarehouseItemService : CrudService<WarehouseItem>, IWarehouseItemService
    {
        private readonly ApplicationDbContext _context;
        public WarehouseItemService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BaseResult> TryTransferStockAsync(Guid senderWarehouseId, Guid destinationWarehouseId, Guid productId, int quantity)
        {

            var result = new BaseResult();

            if (senderWarehouseId == Guid.Empty || destinationWarehouseId == Guid.Empty || productId == Guid.Empty)
            {
                result.Errors.Add("Invalid parameters.");
                return result;
            }

            if (senderWarehouseId == destinationWarehouseId)
            {
                result.Errors.Add("Sender and destination warehouses cannot be the same.");
                return result;
            }

            var senderWarehouse = await _context.Warehouses
                .FirstOrDefaultAsync(w => w.Id == senderWarehouseId);

            if (senderWarehouse == null)
            {
                result.Errors.Add("Sender warehouse not found.");
                return result;
            }

            var destinationWarehouse = await _context.Warehouses
                .FirstOrDefaultAsync(w => w.Id == destinationWarehouseId);

            if (destinationWarehouse == null)
            {
                result.Errors.Add("Destination warehouse not found.");
                return result;
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                result.Errors.Add("Product not found.");
                return result;
            }

            var senderStock = await _context.WarehouseItems
            .FirstOrDefaultAsync(ws => ws.WarehouseId == senderWarehouseId && ws.ProductId == productId);

            if (senderStock == null || senderStock.Quantity < quantity)
            {
                result.Errors.Add("Insufficient stock in sender warehouse.");
                return result;
            }

            var destinationStock = await _context.WarehouseItems
                .FirstOrDefaultAsync(ws => ws.WarehouseId == destinationWarehouseId && ws.ProductId == productId);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                senderStock.Quantity -= quantity;

                if (destinationStock is null)
                {
                    destinationStock = new WarehouseItem
                    {
                        WarehouseId = destinationWarehouseId,
                        ProductId = productId,
                        Quantity = quantity
                    };
                    _context.WarehouseItems.Add(destinationStock);
                }
                else
                {
                    destinationStock.Quantity += quantity;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Errors.Add($"Error during transfer: {ex.Message}");
            }

            return result;
        }
    }
}
