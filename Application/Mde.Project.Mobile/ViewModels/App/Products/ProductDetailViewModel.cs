using CommunityToolkit.Maui.Storage;
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
    public partial class ProductDetailViewModel : UserViewModel
    {
        [ObservableProperty]
        private ProductDetailModel _product;

        [ObservableProperty]
        private bool _isBusy;

        private IAuthenticationService _authenticationService;
        private IProductService _productService;
        public ProductDetailViewModel(IAuthenticationService authenticationService, IProductService productService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _productService = productService;

            WeakReferenceMessenger.Default.Register<SendProductIdentifierMessage>(this, async (r, m) =>
            {
                await InitializeProductCommand.ExecuteAsync(m.Id);
            });
        }

        [RelayCommand]
        public async Task InitializeProductAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There is no correct Product id.", typeof(ProductDetailViewModel)));
                await Shell.Current.GoToAsync("..");
            }

            IsBusy = true;
            var product = await _productService.TryGetProductByIdAsync(id);
            if (!product.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("The given Product id was not found.", typeof(ProductDetailViewModel)));
                await Shell.Current.GoToAsync("..");
            }

            Product = new ProductDetailModel
            {
                ProductId = product.Data.Id,
                Name = product.Data.Name,
                Description = string.IsNullOrWhiteSpace(product.Data.Description) ? "No description available." : product.Data.Description,
                SalesPrice = product.Data.Price,
                Barcode = product.Data.Barcode,
                Image = string.IsNullOrEmpty(product.Data.Image) ? Constants.NoImageUri : $"{Constants.DefaultUploadRoot}{product.Data.Image}",
                SalesTax = product.Data.SalesTax != null ? new TaxItemModel
                {
                    Id = product.Data.SalesTax.Id,
                    Name = product.Data.SalesTax.Name,
                    TaxRate = product.Data.SalesTax.Rate,
                } : null,
                Brand = product.Data.Brand != null ? new BrandItemModel
                {
                    Id = product.Data.Brand.Id,
                    Name = product.Data.Brand.Name,
                } : null,
                Category = product.Data.Category != null ? new CategoryItemModel
                {
                    Id = product.Data.Category.Id,
                    Name = product.Data.Category.Name,
                } : null,
            };

            IsBusy = false;
        }

        [RelayCommand]
        public async Task ProductEditNavigationAsync()
        {
            if (Product.ProductId == null)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There is no correct Product id.", typeof(ProductDetailViewModel)));
                return;
            }

            await Shell.Current.GoToAsync(MauiRoutes.ProductUpsert);
            WeakReferenceMessenger.Default.Send(new SendProductDetailMessage(Product));
        }

        [RelayCommand]
        public async Task ProductDeleteNavigationAsync()
        {
            if (Product.ProductId == null)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There is no correct Product id.", typeof(ProductDetailViewModel)));
                return;
            }

            var productDisplayModel = new SendProductDisplayMessage
            {
                Id = Product.ProductId ??= Guid.NewGuid(),
                Name = Product.Name,
                ImageSource = Product.Image,
            };

            await Shell.Current.GoToAsync(MauiRoutes.ProductDelete);
            WeakReferenceMessenger.Default.Send(productDisplayModel);
        }

        [RelayCommand]
        public async Task ProductPdfDownloadAsync()
        {
            if (!Product.ProductId.HasValue)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There is no correct Product id.", typeof(ProductDetailViewModel)));
                return;
            }

            var result = await _productService.TryGetProductPdf(Product.ProductId.Value);
            if (!result.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(ProductDetailViewModel)));
                return;
            }

            var fileSaver = FileSaver.Default;
            string safeName = string.Join("_", Product.Name.Split(Path.GetInvalidFileNameChars()));
            var fileName = $"{safeName}.pdf";

            var stream = new MemoryStream(result.Data);
            var response = await fileSaver.SaveAsync(fileName, stream, new CancellationToken());

            if (response.IsSuccessful)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Product barcode successfully saved.", typeof(ProductDetailViewModel)));

                // This does not work on android due to permissions, so i had to set it to windows only.
#if WINDOWS
                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(response.FilePath)
                });
#endif
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Could not save PDF", typeof(ProductDetailViewModel)));
            }
        }
    }
}
