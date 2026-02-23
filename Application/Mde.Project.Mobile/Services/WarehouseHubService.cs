using Mde.Project.Mobile.Core;
using Mde.Project.Mobile.Core.Items.RequestModels;
using Mde.Project.Mobile.Core.Models.Reports;
using Mde.Project.Mobile.Core.Models.Statistics;
using Mde.Project.Mobile.Core.Models.Warehouses;
using Mde.Project.Mobile.Models.Scanner;
using Mde.Project.Mobile.Services.Interface;
using Microsoft.AspNetCore.SignalR.Client;

namespace Mde.Project.Mobile.Services
{
    public class WarehouseHubService : IWarehouseHubService
    {
        private HubConnection _hubConnection;
        private Func<Guid, Guid, int, Task> _onProductUpdate;
        private Func<Guid, WarehouseItem, Task> _onProductAdd;
        private Func<Report, Task> _onNewReport;
        private Func<WarehouseStats, Task> _onWarehouseStatsUpdate;
        private Func<string, Task> _onErrorHandling;
        private Func<IEnumerable<ScannerProductModel>, Task> _onUpdatedCart;

        private bool _isConnecting;

        public async Task TryConnectAsync(Guid warehouseId)
        {
            if (warehouseId == Guid.Empty)
                throw new ArgumentException("Warehouse ID cannot be empty.", nameof(warehouseId));

            if (_isConnecting)
                return;

            _isConnecting = true;

            try
            {
                if (_hubConnection != null)
                {
                    await DisconnectAsync();
                }

                _hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{Constants.ProjectApiUrl}/warehouseHub?warehouseId={warehouseId}")
                    .WithAutomaticReconnect()
                    .Build();

                AttachHandlers();

                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to connect for receiving updates.", ex);
            }
            finally
            {
                _isConnecting = false;
            }
        }

        public async Task TryConnectAsync()
        {
            if (_isConnecting)
                return;

            _isConnecting = true;

            try
            {
                if (_hubConnection != null)
                {
                    await DisconnectAsync();
                }

                _hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{Constants.ProjectApiUrl}/warehouseHub")
                    .WithAutomaticReconnect()
                    .Build();

                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to connect for sending updates.", ex);
            }
            finally
            {
                _isConnecting = false;
            }
        }

        public async Task DisconnectAsync()
        {
            if (_hubConnection != null)
            {
                try
                {
                    if (_hubConnection.State == HubConnectionState.Connected)
                    {
                        await _hubConnection.StopAsync();
                    }
                }
                finally
                {
                    _hubConnection = null;
                }
            }
        }

        public void RegisterProductAddHandler(Func<Guid, WarehouseItem, Task> onProductAdd)
        {
            _onProductAdd = onProductAdd;

            if (_hubConnection != null)
            {
                _hubConnection.Remove("ReceiveProductAdd");
                _hubConnection.On<Guid, WarehouseItem>("ReceiveProductAdd", async (warehouseId, warehouseItem) =>
                {
                    if (_onProductAdd != null)
                    {
                        await _onProductAdd(warehouseId, warehouseItem);
                    }
                });
            }
        }

        public void RegisterProductUpdateHandler(Func<Guid, Guid, int, Task> onProductUpdate)
        {
            _onProductUpdate = onProductUpdate;

            if (_hubConnection != null)
            {
                _hubConnection.Remove("ReceiveProductUpdate");
                _hubConnection.On<Guid, Guid, int>("ReceiveProductUpdate", async (warehouseId, productId, quantity) =>
                {
                    if (_onProductUpdate != null)
                    {
                        await _onProductUpdate(warehouseId, productId, quantity);
                    }
                });
            }
        }

        public void RegisterNewReportHandler(Func<Report, Task> onNewReport)
        {
            _onNewReport = onNewReport;
            if (_hubConnection != null)
            {
                _hubConnection.Remove("ReceiveNewReport");
                _hubConnection.On<Report>("ReceiveNewReport", async (report) =>
                {
                    if (_onNewReport != null)
                    {
                        await _onNewReport(report);
                    }
                });
            }
        }

        public void RegisterWarehouseStatsUpdateHandler(Func<WarehouseStats, Task> onWarehouseStatsUpdate)
        {
            _onWarehouseStatsUpdate = onWarehouseStatsUpdate;
            if (_hubConnection != null)
            {
                _hubConnection.Remove("ReceiveWarehouseStats");
                _hubConnection.On<WarehouseStats>("ReceiveWarehouseStats", async (stats) =>
                {
                    if (_onWarehouseStatsUpdate != null)
                    {
                        await _onWarehouseStatsUpdate(stats);
                    }
                });
            }
        }

        public void RegisterUpdatedCartHandler(Func<IEnumerable<ScannerProductModel>, Task> onUpdatedCart)
        {
            _onUpdatedCart = onUpdatedCart;
            if (_hubConnection != null)
            {
                _hubConnection.Remove("ReceiveUpdatedCart");
                _hubConnection.On<IEnumerable<ScannerProductModel>>("ReceiveUpdatedCart", async (products) =>
                {
                    if (_onUpdatedCart != null)
                    {
                        await _onUpdatedCart(products);
                    }
                });
            }
        }

        public void RegisterErrorHandler(Func<string, Task> onErrorHandling)
        {
            if (_hubConnection != null)
            {
                _hubConnection.Remove("ReceiveError");
                _hubConnection.On<string>("ReceiveError", async (error) =>
                {
                    if (_onErrorHandling != null)
                    {
                        await _onErrorHandling(error);
                    }
                });
            }
        }

        private void AttachHandlers()
        {
            if (_onErrorHandling != null)
            {
                _hubConnection.On<string>("ReceiveError", async (error) =>
                {
                    await _onErrorHandling(error);
                });
            }

            if (_onProductUpdate != null)
            {
                _hubConnection.On<Guid, Guid, int>("ReceiveProductUpdate", async (warehouseId, productId, quantity) =>
                {
                    await _onProductUpdate(warehouseId, productId, quantity);
                });
            }

            if (_onProductAdd != null)
            {
                _hubConnection.On<Guid, Guid, WarehouseItem>("ReceiveProductAdd", async (warehouseId, productId, warehouseItem) =>
                {
                    await _onProductAdd(warehouseId, warehouseItem);
                });
            }

            if (_onNewReport != null)
            {
                _hubConnection.On<Report>("ReceiveNewReport", async (report) =>
                {
                    await _onNewReport(report);
                });
            }

            if (_onWarehouseStatsUpdate != null)
            {
                _hubConnection.On<WarehouseStats>("ReceiveWarehouseStats", async (stats) =>
                {
                    await _onWarehouseStatsUpdate(stats);
                });
            }

            if (_onUpdatedCart != null)
            {
                _hubConnection.On<IEnumerable<ScannerProductModel>>("ReceiveUpdatedCart", async (products) =>
                {
                    await _onUpdatedCart(products);
                });
            }
        }

        public async Task<bool> SendProductAddAsync(Guid warehouseId, CreateItemRequestModel createItemRequest, string tag)
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("AddWarehouseProductAsync", warehouseId, createItemRequest, tag);
                return true;
            }

            return false;
        }

        public async Task<bool> SendProductUpdateAsync(Guid warehouseId, Guid productId, int delta, string tag)
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("UpdateWarehouseProductAsync", warehouseId, productId, delta, tag, false);
                return true;
            }

            return false;
        }

        public async Task<bool> SendStockTransferAsync(Guid sourceWarehouseId, Guid destinationWarehouseId, Guid productId, int quantity)
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("TransferWarehouseProductAsync", sourceWarehouseId, destinationWarehouseId, productId, quantity);
                return true;
            }
            return false;
        }

        // OK
        public async Task<bool> SendOrderRequestAsync(Guid warehouseId, IEnumerable<ScannerProductModel> products)
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("SendOrderRequestAsync", warehouseId, products);
                return true;
            }
            return false;
        }
    }
}
