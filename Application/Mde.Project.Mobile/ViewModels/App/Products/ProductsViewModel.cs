using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Products.Service.Interfaces;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Products;
using Mde.Project.Mobile.Models.Products;
using Mde.Project.Mobile.ViewModels.Base;

namespace Mde.Project.Mobile.ViewModels.App.Products
{
    public partial class ProductsViewModel : UserViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ProductItemDisplayModel> _products;

        [ObservableProperty]
        private ObservableCollection<BrandItemModel> _brands;

        [ObservableProperty]
        private ObservableCollection<CategoryItemModel> _categories;

        [ObservableProperty]
        private CategoryItemModel _selectedCategory;

        [ObservableProperty]
        private BrandItemModel _selectedBrand;

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

        private readonly IAuthenticationService _authenticationService;
        private readonly IProductService _productService;
        public ProductsViewModel(IAuthenticationService authenticationService, IProductService productService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _productService = productService;
            CurrentPage = 1;

            WeakReferenceMessenger.Default.Register<SendProductsChangedMessage>(this, async (r, m) =>
            {
                await FilterProductsCommand.ExecuteAsync(null);
            });
        }

        [RelayCommand]
        public async Task FilterProductsAsync()
        {
            IsBusy = true;
            await InitializeAccountAsync();
            await LoadProductsAsync(Filter, CurrentPage, SelectedBrand?.Id, SelectedCategory?.Id);
            IsBusy = false;
        }

        [RelayCommand]
        public async Task ProductAddNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.ProductUpsert);
        }

        [RelayCommand]
        public async Task ProductDetailNavigationAsync(Guid id)
        {
            await Shell.Current.GoToAsync(MauiRoutes.ProductDetail);
            WeakReferenceMessenger.Default.Send(new SendProductIdentifierMessage(id));
        }

        [RelayCommand]
        public async Task ChangeProductPageAsync(int page)
        {
            if (page >= 1 && page <= TotalPages)
            {
                CurrentPage = page;
                IsBusy = true;
                await LoadProductsAsync(Filter, CurrentPage, SelectedBrand?.Id, SelectedCategory?.Id);
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task InitializeProductsAsync()
        {
            IsBusy = true;
            await LoadCategoriesAsync();
            await LoadBrandsAsync();
            await LoadProductsAsync(Filter, CurrentPage, SelectedBrand?.Id, SelectedCategory?.Id);
            IsBusy = false;
        }

        private async Task LoadProductsAsync(string filter, int currentPage, Guid? categoryId, Guid? brandId)
        {
            var products = await _productService.TryGetProductListAsync(filter, currentPage, categoryId, brandId);
            if (products == null || !products.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(products?.Message ?? "No products found.", typeof(ProductsViewModel)));
                Products = new ObservableCollection<ProductItemDisplayModel>();
                return;
            }

            if (!products.Items.Any())
            {
                Products = new ObservableCollection<ProductItemDisplayModel>();
                return;
            }

            Products = new ObservableCollection<ProductItemDisplayModel>(
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
                })
            );
            TotalPages = products.TotalPages;
            TotalItems = products.TotalItems;

        }
        private async Task LoadCategoriesAsync()
        {
            var categories = await _productService.TryGetCategoriesOptionsAsync();
            if (!categories.IsSuccess || !categories.Items.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(categories.Message, typeof(ProductsViewModel)));
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
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(brands.Message, typeof(ProductsViewModel)));
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
    }
}
