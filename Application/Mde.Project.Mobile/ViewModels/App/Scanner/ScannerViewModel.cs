using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Scanner;
using Mde.Project.Mobile.Core.Items.Interfaces;
using Mde.Project.Mobile.Services.Interface;
using Mde.Project.Mobile.Models.Scanner;
using Mde.Project.Mobile.Core;

namespace Mde.Project.Mobile.ViewModels.App.Scanner
{
    public partial class ScannerViewModel : UserViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IWarehouseItemsService _warehouseItemsService;
        private readonly IWarehouseHubService _warehouseHubService;
        private Guid _selectedWarehouseId;

        [ObservableProperty]
        private decimal _totalCost;

        [ObservableProperty]
        private ObservableCollection<ScannerItemViewModel> _products;

        [ObservableProperty]
        private bool _hasProducts;

        [ObservableProperty] private string _barcodeText;

        [ObservableProperty]
        private bool _isBusy;

        public ScannerViewModel(IAuthenticationService authenticationService, IWarehouseItemsService warehouseItemsService, IWarehouseHubService warehouseHubService) : base(authenticationService)
        {
            _warehouseItemsService = warehouseItemsService;
            _authenticationService = authenticationService;
            _warehouseHubService = warehouseHubService;
            Products = new();
            BarcodeText = "";

            _warehouseHubService.RegisterErrorHandler((error) =>
            {
                OnHubErrorHandling(error);
                return Task.CompletedTask;
            });

            _warehouseHubService.RegisterUpdatedCartHandler((products) =>
            {
                OnHubProductUpdate(products);
                return Task.CompletedTask;
            });

            WeakReferenceMessenger.Default.Register<SendSelectedWarehouseMessage>(this, async (r, m) =>
            {
                _selectedWarehouseId = m.WarehouseId;
            });

            WeakReferenceMessenger.Default.Register<SendReturnBasketMessage>(this, async (r, m) =>
            {
                await BasketNavigationAsync();
            });
        }

        private void OnHubErrorHandling(string error)
        {
            WeakReferenceMessenger.Default.Send(new SendToastrMessage(error, typeof(ScannerViewModel)));
        }

        private void OnHubProductUpdate(IEnumerable<ScannerProductModel> products)
        {
            IsBusy = true;
            if (products == null || !products.Any())
            {
                return;
            }

            foreach (var product in products)
            {
                var existingProduct = Products.FirstOrDefault(p => p.Id == product.ProductId);
                if (existingProduct != null)
                {
                    existingProduct.Quantity = product.Quantity;
                }
            }
            IsBusy = false;
        }

        [RelayCommand]
        public async Task InitializeAsync()
        {
            await _warehouseHubService.TryConnectAsync();
        }

        [RelayCommand]
        public async Task TextScanProductAsync()
        {
            if (string.IsNullOrWhiteSpace(BarcodeText) && BarcodeText.Length < 13)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("The barcode is not in a valid format.", typeof(ScannerViewModel)));
                return;
            }

            await ScanProductAsync(BarcodeText);
        }

        [RelayCommand]
        public async Task ScanProductNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.ScanProduct);
        }

        [RelayCommand]
        public async Task BasketNavigationAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task ScanProductAsync(string productBarcode)
        {
            Products.CollectionChanged += OnProductsCollectionChanged;

            if (_selectedWarehouseId == Guid.Empty)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There is no valid warehouse found.", typeof(ScannerViewModel)));
                return;
            }

            try
            {
                var productCount = 0;
                var existingProduct = Products.FirstOrDefault(p => p.Barcode == productBarcode);
                if (existingProduct != null)
                {
                    productCount = existingProduct.Quantity;
                }

                var productCheck = await _warehouseItemsService.TryGetItemByBarcodeAsync(_selectedWarehouseId, productBarcode, productCount);
                if (!productCheck.IsSuccess)
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage(productCheck.Message, typeof(ScannerViewModel)));
                    return;
                }

                if (existingProduct != null)
                {
                    if (!await CanAddProduct(_selectedWarehouseId, productCheck.Data.Product.Id, productCount))
                    {
                        WeakReferenceMessenger.Default.Send(new SendToastrMessage("Warehouse does not have more of this product.", typeof(ScannerViewModel)));
                        return;
                    }

                    existingProduct.Quantity++;
                }
                else
                {
                    var newProduct = new ScannerItemViewModel
                    {
                        Id = productCheck.Data.Product.Id,
                        Brand = productCheck.Data.Product.Brand.Name,
                        Name = productCheck.Data.Product.Name,
                        Quantity = 1,
                        ImageSource = string.IsNullOrEmpty(productCheck.Data.Product.Image) ? Constants.NoImageUri : $"{Constants.DefaultUploadRoot}{productCheck.Data.Product.Image}",
                        Barcode = productBarcode
                    };

                    newProduct.Price = productCheck.Data.Product.Price;
                    if (productCheck.Data.Product.SalesTax.Rate > 0)
                    {
                        var taxPrice = (newProduct.Price * (decimal)productCheck.Data.Product.SalesTax.Rate);
                        newProduct.Price += taxPrice;
                    }

                    Products.Add(newProduct);
                }
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(ex.Message, typeof(ScannerViewModel)));
            }
        }

        [RelayCommand]
        public async Task CheckoutOrderAsync()
        {
            if (Products.Count == 0)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("No products to checkout.", typeof(ScannerViewModel)));
                return;
            }
            if (_selectedWarehouseId == Guid.Empty)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There is no valid warehouse found.", typeof(ScannerViewModel)));
                return;
            }

            IsBusy = true;

            var productsToCheckout = Products.Select(p => new ScannerProductModel
            {
                ProductId = p.Id,
                Quantity = p.Quantity
            }).ToList();

            var result = await _warehouseHubService.SendOrderRequestAsync(_selectedWarehouseId, productsToCheckout);
            if (!result)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There was an error processing the order.", typeof(ScannerViewModel)));
            }
            else
            {
                Products.Clear();
                TotalCost = 0;
                HasProducts = false;
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Order processed successfully.", typeof(ScannerViewModel)));
            }
        }

        [RelayCommand]
        private async Task IncrementQuantity(ScannerItemViewModel product)
        {
            if (!await CanAddProduct(_selectedWarehouseId, product.Id, product.Quantity))
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Warehouse does not have more of this product.", typeof(ScannerViewModel)));
                return;
            }

            product.Quantity++;
        }

        [RelayCommand]
        private void DecrementQuantity(ScannerItemViewModel product)
        {
            if (product.Quantity > 1)
            {
                product.Quantity--;
            }
        }

        [RelayCommand]
        private void RemoveProduct(ScannerItemViewModel product)
        {
            Products.Remove(product);
        }

        private async Task<bool> CanAddProduct(Guid warehouseId, Guid productId, int currentCount)
        {
            var result = await _warehouseItemsService.TryGetItemByIdAsync(warehouseId, productId);
            if (!result.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(ScannerViewModel)));
                return false;
            }
            return result.Data.Quantity >= currentCount + 1;
        }

        private void OnProductsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ScannerItemViewModel product in e.NewItems)
                {
                    product.PropertyChanged += OnProductPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (ScannerItemViewModel product in e.OldItems)
                {
                    product.PropertyChanged -= OnProductPropertyChanged;
                }
            }

            UpdateTotalCost();
            HasProducts = Products.Any();
        }

        private void OnProductPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ScannerItemViewModel.Quantity) || e.PropertyName == nameof(ScannerItemViewModel.Price))
            {
                UpdateTotalCost();
            }
        }

        private void UpdateTotalCost()
        {
            TotalCost = Products.Sum(p => p.Quantity * p.Price);
        }
    }
}
