using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Items.Interfaces;
using Mde.Project.Mobile.Core.Models.Warehouses;
using Mde.Project.Mobile.Core.Products.Service.Interfaces;
using Mde.Project.Mobile.Core.Warehouses.Interface;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Operations;
using Mde.Project.Mobile.Models.Products;
using Mde.Project.Mobile.Models.Warehouse;
using Mde.Project.Mobile.Services.Interface;
using Mde.Project.Mobile.ViewModels.Base;

namespace Mde.Project.Mobile.ViewModels.App.Operations
{
    public partial class StockConfigurationViewModel : UserViewModel
    {
        [ObservableProperty] private ObservableCollection<WarehouseStockItemModel> _warehouseProducts;

        [ObservableProperty]
        private ObservableCollection<BrandItemModel> _brands;

        [ObservableProperty]
        private ObservableCollection<CategoryItemModel> _categories;

        [ObservableProperty]
        private ObservableCollection<WarehouseItemModel> _warehouses;

        [ObservableProperty]
        private CategoryItemModel _selectedCategory;

        [ObservableProperty]
        private BrandItemModel _selectedBrand;

        [ObservableProperty]
        private WarehouseItemModel _selectedWarehouse;

        [ObservableProperty]
        private string _filter;

        [ObservableProperty]
        private int _currentPage;

        [ObservableProperty]
        private int _totalPages;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private int _totalItems;

        private readonly IWarehouseService _warehouseService;
        private readonly IProductService _productService;
        private readonly IWarehouseHubService _warehouseHubService;
        private readonly IWarehouseItemsService _warehouseItemsService;

        public StockConfigurationViewModel(IWarehouseService warehouseService, IProductService productService, IAuthenticationService authenticationService, IWarehouseHubService warehouseHubService, IWarehouseItemsService warehouseItemsService) : base(authenticationService)
        {
            _warehouseService = warehouseService;
            _productService = productService;
            _warehouseHubService = warehouseHubService;
            _warehouseItemsService = warehouseItemsService;
            CurrentPage = 1;

            _warehouseHubService.RegisterErrorHandler((error) => 
            {
                OnHubErrorHandling(error);
                return Task.CompletedTask;
            });

            _warehouseHubService.RegisterProductUpdateHandler((warehouseId, productId, quantity) =>
            {
                UpdateWarehouseProduct(warehouseId, productId, quantity);
                return Task.CompletedTask;
            });

            _warehouseHubService.RegisterProductAddHandler((warehouseId, item) =>
            {
                AddWarehouseProduct(warehouseId, item);
                return Task.CompletedTask;
            });

            WeakReferenceMessenger.Default.Register<SendItemInfoChangedMessage>(this, async (r, m) =>
            {
                await FilterProductsAsync();
            });
        }

        [RelayCommand]
        public async Task WarehouseChangedAsync()
        {
            IsBusy = true;
            await _warehouseHubService.DisconnectAsync();

            SelectedBrand = null;
            SelectedCategory = null;
            Filter = string.Empty;
            CurrentPage = 1;


            if (SelectedWarehouse != null)
            {
                await _warehouseHubService.TryConnectAsync(SelectedWarehouse.Id.Value);
                await FilterProductsAsync();
            }
            IsBusy = false;
        }

        [RelayCommand]
        public async Task FilterProductsAsync()
        {
            if (SelectedWarehouse?.Id != null)
            {
                IsBusy = true;
                await LoadProductsAsync(Filter, SelectedWarehouse.Id.Value, CurrentPage, SelectedBrand?.Id, SelectedCategory?.Id);
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task InitializeProductsAsync()
        {
            IsBusy = true;
            await InitializeAccountAsync();
            await LoadCategoriesAsync();
            await LoadBrandsAsync();
            await LoadWarehousesAsync();
            if (!Warehouses.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("No warehouses found.", typeof(StockConfigurationViewModel)));
                return;
            }

            SelectedWarehouse = Warehouses.FirstOrDefault();

            if (SelectedWarehouse?.Id != null)
            {
                await _warehouseHubService.TryConnectAsync(SelectedWarehouse.Id.Value);
                await LoadProductsAsync(Filter, SelectedWarehouse.Id.Value, CurrentPage, SelectedBrand?.Id, SelectedCategory?.Id);
            }

            IsBusy = false;
        }


        [RelayCommand]
        public async Task ChangeProductPageAsync(int page)
        {
            if (page >= 1 && page <= TotalPages)
            {
                CurrentPage = page;
                IsBusy = true;

                if (SelectedWarehouse?.Id != null)
                {
                    await LoadProductsAsync(Filter, SelectedWarehouse.Id.Value, CurrentPage, SelectedBrand?.Id,
                        SelectedCategory?.Id);
                }

                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task ItemSettingsNavigationAsync(WarehouseStockItemModel item)
        {
            await Shell.Current.GoToAsync(MauiRoutes.StockSettings);
            WeakReferenceMessenger.Default.Send(new SendItemInfoMessage(item));
        }

        private async Task LoadProductsAsync(string filter, Guid warehouseId, int currentPage, Guid? categoryId, Guid? brandId)
        {
            var products = await _warehouseItemsService.TryGetItemsAsync(filter, warehouseId, currentPage, categoryId, brandId);
            if (!products.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(products.Message, typeof(StockConfigurationViewModel)));
                WarehouseProducts = new ObservableCollection<WarehouseStockItemModel>();
                return;
            }

            if (products.Items == null || !products.Items.Any())
            {
                WarehouseProducts = new ObservableCollection<WarehouseStockItemModel>();
                return;
            }

            WarehouseProducts = new ObservableCollection<WarehouseStockItemModel>(products.Items.Select(p =>
                new WarehouseStockItemModel
                {
                    Id = p.Id,
                    WarehouseId = p.WarehouseId,
                    Product = new ProductItemDisplayModel
                    {
                        Id = p.Product.Id,
                        Name = p.Product.Name,
                        Brand = new BrandItemModel
                        {
                            Id = p.Product.Brand.Id,
                            Name = p.Product.Brand.Name,
                        },
                        Category = new CategoryItemModel
                        {
                            Id = p.Product.Category.Id,
                            Name = p.Product.Category.Name,
                        },
                        ImageSource = string.IsNullOrEmpty(p.Product.Image) ? Constants.NoImageUri : $"{Constants.DefaultUploadRoot}{p.Product.Image}",
                    },
                    Quantity = p.Quantity,
                    MinimumQuantity = p.MinimumQuantity,
                    RefillQuantity = p.RefillQuantity,
                    HasAutoRefill = p.HasAutoRefill,
                }));
            TotalPages = products.TotalPages;
            TotalItems = products.TotalItems;

        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _productService.TryGetCategoriesOptionsAsync();
            if (!categories.IsSuccess || !categories.Items.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(categories.Message, typeof(StockConfigurationViewModel)));
                Categories = new ObservableCollection<CategoryItemModel>();
                return;
            }

            Categories = new ObservableCollection<CategoryItemModel>(
                categories.Items.Select(c => new CategoryItemModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
            );
        }
        private async Task LoadBrandsAsync()
        {
            var brands = await _productService.TryGetBrandsOptionsAsync();
            if (!brands.IsSuccess || !brands.Items.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(brands.Message, typeof(StockConfigurationViewModel)));
                Brands = new ObservableCollection<BrandItemModel>();
                return;
            }

            Brands = new ObservableCollection<BrandItemModel>(
                brands.Items.Select(c => new BrandItemModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
            );
        }

        private async Task LoadWarehousesAsync()
        {
            var warehouses = await _warehouseService.TryGetWarehouseOptionsAsync();
            if (!warehouses.IsSuccess || !warehouses.Items.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(warehouses.Message, typeof(StockConfigurationViewModel)));
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
            WeakReferenceMessenger.Default.Send(new SendToastrMessage(error, typeof(StockConfigurationViewModel)));
        }

        // This method is called when a product is updated in the warehouse hub
        private void UpdateWarehouseProduct(Guid warehouseId, Guid productId, int quantity)
        {
            if (SelectedWarehouse.Id != warehouseId)
            {
                return;
            }

            var productToUpdate = WarehouseProducts.FirstOrDefault(p => p.Product.Id == productId);
            if (productToUpdate != null)
            {
                productToUpdate.Quantity = quantity;
            }
        }

        private void AddWarehouseProduct(Guid warehouseId, WarehouseItem item)
        {
            if (SelectedWarehouse.Id != warehouseId)
            {
                return;
            }

            if (SelectedBrand != null && SelectedBrand.Id != item.Product.Brand.Id)
            {
                return;
            }

            if (SelectedCategory != null && SelectedCategory.Id != item.Product.Category.Id)
            {
                return;
            }

            var existingProduct = WarehouseProducts.FirstOrDefault(p => p.Product.Id == item.Product.Id);
            if (existingProduct == null)
            {
                WarehouseProducts.Add(new WarehouseStockItemModel
                {
                    Id = item.Id,
                    Product = new ProductItemDisplayModel
                    {
                        Id = item.Product.Id,
                        Name = item.Product.Name,
                        Brand = new BrandItemModel
                        {
                            Id = item.Product.Brand.Id,
                            Name = item.Product.Brand.Name,
                        },
                        Category = new CategoryItemModel
                        {
                            Id = item.Product.Category.Id,
                            Name = item.Product.Category.Name,
                        },
                        ImageSource = string.IsNullOrEmpty(item.Product.Image) ? Constants.NoImageUri : $"{Constants.DefaultUploadRoot}{item.Product.Image}",
                    },
                    Quantity = item.Quantity,
                });
            }
        }
    }
}
