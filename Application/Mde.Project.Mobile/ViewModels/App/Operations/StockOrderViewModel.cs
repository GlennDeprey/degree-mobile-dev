using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Items.Interfaces;
using Mde.Project.Mobile.Core.Items.RequestModels;
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
    public partial class StockOrderViewModel : UserViewModel
    {

        [ObservableProperty]
        private ObservableCollection<WarehouseItemModel> _warehouses;

        [ObservableProperty]
        private ObservableCollection<ProductItemDisplayModel> _filteredProducts;

        [ObservableProperty]
        private ObservableCollection<string> _filteredProductsNames;

        [ObservableProperty]
        private WarehouseItemModel _selectedWarehouse;

        [ObservableProperty]
        private ProductItemDisplayModel _selectedProduct;

        [ObservableProperty]
        private string _selectedProductName;

        [ObservableProperty]
        private int _quantityToAdd;

        [ObservableProperty]
        private bool _isBusy;

        private readonly IWarehouseService _warehouseService;
        private readonly IProductService _productService;
        private readonly IWarehouseHubService _warehouseHubService;
        private readonly IWarehouseItemsService _warehouseItemsService;

        public StockOrderViewModel(IWarehouseService warehouseService, IProductService productService, IAuthenticationService authenticationService, IWarehouseHubService warehouseHubService, IWarehouseItemsService warehouseItemsService) : base(authenticationService)
        {
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
        public async Task InitializeWarehousesAsync()
        {
            IsBusy = true;
            await InitializeAccountAsync();
            await _warehouseHubService.DisconnectAsync();
            await LoadWarehousesAsync();
            if (!Warehouses.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("No warehouses found.", typeof(StockOrderViewModel)));
                return;
            }

            await _warehouseHubService.TryConnectAsync();

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
                        ImageSource = string.IsNullOrEmpty(p.Image) ? Constants.NoImageUri : $"{Constants.DefaultUploadRoot}{p.Image}",
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

        [RelayCommand]
        public async Task UpsertProductToWarehouseAsync()
        {
            SelectedProduct = FilteredProducts.FirstOrDefault(p => p.Name == SelectedProductName);

            if (SelectedWarehouse == null || SelectedProduct == null || QuantityToAdd <= 0)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Please select a warehouse, product, and valid quantity.", typeof(StockOrderViewModel)));
                return;
            }

            IsBusy = true;

            var existingProduct = await _warehouseItemsService.TryGetItemByIdAsync(SelectedWarehouse.Id.Value, SelectedProduct.Id);
            try
            {
                if (existingProduct.Data == null)
                {
                    var requestModel = new CreateItemRequestModel
                    {
                        ProductId = SelectedProduct.Id,
                        Quantity = QuantityToAdd
                    };

                    var result = await _warehouseHubService.SendProductAddAsync(SelectedWarehouse.Id.Value, requestModel, "Manual");
                    if (result)
                    {
                        WeakReferenceMessenger.Default.Send(new SendToastrMessage("Product added successfully.", typeof(StockOrderViewModel)));
                        FilteredProducts.Clear();
                        SelectedProduct = null;
                        QuantityToAdd = 0;
                    }
                    else
                    {
                        WeakReferenceMessenger.Default.Send(new SendToastrMessage("Failed to add the product. Please try again.", typeof(StockOrderViewModel)));
                    }
                }
                else
                {
                    var result = await _warehouseHubService.SendProductUpdateAsync(SelectedWarehouse.Id.Value, existingProduct.Data.Product.Id, QuantityToAdd, "Manual");
                    if (result)
                    {
                        WeakReferenceMessenger.Default.Send(new SendToastrMessage("Product updated successfully.", typeof(StockOrderViewModel)));
                        FilteredProducts.Clear();
                        SelectedProduct = null;
                        SelectedProductName = null;
                        SelectedWarehouse = null;
                        QuantityToAdd = 0;
                    }
                    else
                    {
                        WeakReferenceMessenger.Default.Send(new SendToastrMessage("Failed to update the product. Please try again.", typeof(StockOrderViewModel)));
                    }
                }               
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("An error occurred while adding the product. Please try again later.", typeof(StockOrderViewModel)));
            }
            finally
            {
                IsBusy = false;
            }
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

        private void OnHubErrorHandling(string error)
        {
            WeakReferenceMessenger.Default.Send(new SendToastrMessage(error));
        }
    }
}
