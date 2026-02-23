using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Products.RequestModels;
using Mde.Project.Mobile.Core.Products.Service.Interfaces;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Products;
using Mde.Project.Mobile.Models.Products;
using Mde.Project.Mobile.ViewModels.Base;

namespace Mde.Project.Mobile.ViewModels.App.Products
{
    public partial class ProductUpsertViewModel : UserViewModel
    {
        [ObservableProperty] 
        private ProductDetailModel _product;

        [ObservableProperty] private string _title;

        [ObservableProperty]
        private ObservableCollection<BrandItemModel> _brands;

        [ObservableProperty]
        private ObservableCollection<CategoryItemModel> _categories;

        [ObservableProperty]
        private ObservableCollection<TaxItemModel> _taxes;

        [ObservableProperty]
        private CategoryItemModel _selectedCategory;

        [ObservableProperty]
        private BrandItemModel _selectedBrand;

        [ObservableProperty]
        private TaxItemModel _selectedTax;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private ImageSource _thumbnailImage;

        private FileResult _selectedFile;
        private string _fileName;
        private Stream _resizedImageStream;

        private readonly IAuthenticationService _authenticationService;
        private readonly IProductService _productService;
        public ProductUpsertViewModel(IAuthenticationService authenticationService, IProductService productService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _productService = productService;
            Product = new ProductDetailModel();
            Product.Image = Constants.NoImageUri;
            Title = "Add Product";

            WeakReferenceMessenger.Default.Register<SendProductDetailMessage>(this, (r, m) =>
            {
                Product = m.ProductDetail;
                Title = "Edit Product";
            });
        }

        [RelayCommand]
        public async Task InitializeProductAsync()
        {
            IsBusy = true;
            await LoadCategoriesAsync();
            await LoadBrandsAsync();
            await LoadTaxRatesAsync();

            if (Product.ProductId != null)
            {
                SelectedBrand = Brands.FirstOrDefault(b => b.Id == Product.Brand.Id);
                SelectedCategory = Categories.FirstOrDefault(c => c.Id == Product.Category.Id);
                SelectedTax = Taxes.FirstOrDefault(t => t.Id == Product.SalesTax.Id);
            }

            IsBusy = false;
        }

        [RelayCommand]
        public async Task PickImageAsync()
        {
            try
            {
                var file = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Product Image",
                    FileTypes = FilePickerFileType.Images
                });

                if (file != null)
                {
                    _selectedFile = file;
                    _fileName = file.FileName;

                    using var originalStream = await file.OpenReadAsync();
                    var thumbnailStream = MauiServiceHelper.ResizeImage(originalStream, 250, 250);
                    thumbnailStream.Position = 0;

                    ThumbnailImage = ImageSource.FromStream(() =>
                    {
                        var copy = new MemoryStream();
                        thumbnailStream.CopyTo(copy);
                        copy.Position = 0;
                        thumbnailStream.Position = 0;
                        return copy;
                    });

                    WeakReferenceMessenger.Default.Send(new SendProductPictureMessage(ThumbnailImage));
                    _resizedImageStream = thumbnailStream;
                }
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Failed to pick image.", typeof(ProductUpsertViewModel)));
            }
        }

        [RelayCommand]
        public async Task UpsertProductAsync()
        {
            IsBusy = true;
            var productUpsertRequestModel = new ProductUpsertRequestModel
            {
                Name = Product.Name,
                Description = Product.Description,
                SalesPrice = Product.SalesPrice,
                Image = Product.Image,
                BrandId = SelectedBrand.Id.Value,
                CategoryId = SelectedCategory.Id.Value,
                SalesTaxId = SelectedTax.Id.Value
            };

            if (Product.ProductId != null && Product.ProductId != Guid.Empty)
            {
                productUpsertRequestModel.Id = Product.ProductId.Value;
                productUpsertRequestModel.Barcode = Product.Barcode;
            }

            var result = await _productService.TryUpsertProductAsync(productUpsertRequestModel, _resizedImageStream, _fileName);
            if (!result.IsSuccess)
            {
                var messageAction = productUpsertRequestModel.Id != null && productUpsertRequestModel.Id != Guid.Empty ? "updating" : "adding";
                WeakReferenceMessenger.Default.Send(new SendToastrMessage($"There was a issue with {messageAction} the product.", typeof(ProductUpsertViewModel)));
                IsBusy = false;
                return;
            }
            await Shell.Current.GoToAsync("..");
            WeakReferenceMessenger.Default.Send(new SendProductsChangedMessage());
            if (Product.ProductId != null && Product.ProductId != Guid.Empty)
            {
                WeakReferenceMessenger.Default.Send(new SendProductIdentifierMessage(Product.ProductId.Value));
            }
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _productService.TryGetCategoriesOptionsAsync();
            if (!categories.IsSuccess || !categories.Items.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(categories.Message, typeof(ProductUpsertViewModel)));
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
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(brands.Message, typeof(ProductUpsertViewModel)));
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

        private async Task LoadTaxRatesAsync()
        {
            var taxes = await _productService.TryGetTaxRateOptionsAsync();
            if (!taxes.IsSuccess || !taxes.Items.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(taxes.Message, typeof(ProductUpsertViewModel)));
                Taxes = new ObservableCollection<TaxItemModel>();
                return;
            }

            Taxes = new ObservableCollection<TaxItemModel>(
                taxes.Items.Select(c => new TaxItemModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    TaxRate = c.Rate,
                })
            );
        }
    }
}
