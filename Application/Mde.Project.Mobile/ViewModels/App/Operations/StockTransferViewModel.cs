using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Items.Interfaces;
using Mde.Project.Mobile.Core.Products.Service.Interfaces;
using Mde.Project.Mobile.Core.Warehouses.Interface;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Models.Products;
using Mde.Project.Mobile.Models.Warehouse;
using Mde.Project.Mobile.Services.Interface;
using Mde.Project.Mobile.ViewModels.Base;
using System.Collections.ObjectModel;

namespace Mde.Project.Mobile.ViewModels.App.Operations
{
    public partial class StockTransferViewModel : UserViewModel
    {
        [ObservableProperty]
        private ObservableCollection<WarehouseItemModel> _warehouses;

        [ObservableProperty]
        private ObservableCollection<WarehouseItemModel> _filteredSenderWarehouses;

        [ObservableProperty]
        private ObservableCollection<ProductItemDisplayModel> _filteredProducts;

        [ObservableProperty]
        private ObservableCollection<string> _filteredProductsNames;

        [ObservableProperty]
        private WarehouseItemModel _senderWarehouse;

        [ObservableProperty]
        private WarehouseItemModel _destinationWarehouse;

        [ObservableProperty]
        private ProductItemDisplayModel _selectedProduct;

        [ObservableProperty]
        private string _selectedProductName;

        [ObservableProperty]
        private int _quantityToSend;

        [ObservableProperty]
        private bool _isSearchResultVisible;

        [ObservableProperty]
        private bool _isBusy;

        private readonly IAuthenticationService _authenticationService;
        private readonly IWarehouseService _warehouseService;
        private readonly IProductService _productService;
        private readonly IWarehouseHubService _warehouseHubService;
        private readonly IWarehouseItemsService _warehouseItemsService;

        public StockTransferViewModel(IWarehouseService warehouseService, IProductService productService, IAuthenticationService authenticationService, IWarehouseHubService warehouseHubService, IWarehouseItemsService warehouseItemsService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _warehouseService = warehouseService;
            _productService = productService;
            _warehouseHubService = warehouseHubService;
            _warehouseItemsService = warehouseItemsService;

            _filteredProducts = new ObservableCollection<ProductItemDisplayModel>();
            _warehouseHubService.RegisterErrorHandler((error) =>
            {
                OnHubErrorHandling(error);
                return Task.CompletedTask;
            });
        }

        [RelayCommand]
        public void SelectSenderWarehouse(WarehouseItemModel warehouse)
        {
            SenderWarehouse = warehouse;
        }

        [RelayCommand]
        public void ResetSearchState()
        {
            FilteredSenderWarehouses = new ObservableCollection<WarehouseItemModel>();
            SelectedProductName = string.Empty;
            SelectedProduct = null;
            DestinationWarehouse = null;
            QuantityToSend = 0;
            IsSearchResultVisible = false;
        }

        [RelayCommand]
        public async Task InitializeAsync()
        {

            await _warehouseHubService.DisconnectAsync();

            IsBusy = true;
            await InitializeAccountAsync();
            await LoadWarehousesAsync();
            if (!Warehouses.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("No warehouses found.", typeof(StockTransferViewModel)));
                return;
            }

            await _warehouseHubService.TryConnectAsync();
            IsBusy = false;         
        }

        [RelayCommand]
        private async Task FindWarehousesWithStockAsync()
        {
            IsBusy = true;
            IsSearchResultVisible = false;

            try
            {
                SelectedProduct = FilteredProducts.FirstOrDefault(p => p.Name == SelectedProductName);

                if (SelectedProduct is null || DestinationWarehouse is null || QuantityToSend <= 0)
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage("Please select a destination warehouse, a product and enter a valid quantity.", typeof(StockTransferViewModel)));
                    return;
                }

                var result = await _warehouseItemsService.TryGetWarehousesWithProductStockAsync(
                    SelectedProduct.Id,
                    DestinationWarehouse.Id,
                    QuantityToSend);

                if (!result.IsSuccess || result.Data is null || !result.Data.Any())
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage("No warehouses found with sufficient stock.", typeof(StockTransferViewModel)));
                    FilteredSenderWarehouses = new ObservableCollection<WarehouseItemModel>();
                    return;
                }

                FilteredSenderWarehouses = new ObservableCollection<WarehouseItemModel>(
                    result.Data.Select(item => new WarehouseItemModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Quantity = item.Quantity
                    }));

                IsSearchResultVisible = true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SendStockTransferAsync()
        {
            IsBusy = true;
            if (SenderWarehouse is null || DestinationWarehouse is null || SelectedProduct is null || QuantityToSend <= 0)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("All fields must be completed before sending.", typeof(StockTransferViewModel)));
                return;
            }

            var result = await _warehouseHubService.SendStockTransferAsync(
                SenderWarehouse.Id.Value,
                DestinationWarehouse.Id.Value,
                SelectedProduct.Id,
                QuantityToSend);

            if (result)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Stock tranfer has been send.", typeof(StockTransferViewModel)));
                ResetSearchState();
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Failed to transfer the product. Please try again.", typeof(StockTransferViewModel)));
            }

            IsBusy = false;
        }

        [RelayCommand]
        public async Task SearchProductsAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                FilteredProducts.Clear();
                return;
            }

            var products = await _productService.TryGetProductListAsync(searchText, 1, null, null);
            if (products?.IsSuccess == true && products.Items.Any())
            {
                FilteredProducts = new ObservableCollection<ProductItemDisplayModel>(
                    products.Items.Select(p => new ProductItemDisplayModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Brand = new BrandItemModel
                        {
                            Id = p.Brand.Id,
                            Name = p.Brand.Name
                        },
                        ImageSource = p.Image,
                        Category = new CategoryItemModel
                        {
                            Id = p.Category.Id,
                            Name = p.Category.Name
                        },
                    }));

                FilteredProductsNames = new ObservableCollection<string>(FilteredProducts.Select(p => p.Name));
            }
            else
            {
                FilteredProducts.Clear();
            }
        }
        private void OnHubErrorHandling(string error)
        {
            WeakReferenceMessenger.Default.Send(new SendToastrMessage(error));
        }
        private async Task LoadWarehousesAsync()
        {
            var warehouses = await _warehouseService.TryGetWarehouseOptionsAsync();
            if (!warehouses.IsSuccess || !warehouses.Items.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(warehouses.Message));
                Warehouses = new ObservableCollection<WarehouseItemModel>();
                return;
            }

            Warehouses = new ObservableCollection<WarehouseItemModel>(
                warehouses.Items.Select(c => new WarehouseItemModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
            );
        }
    }
}
