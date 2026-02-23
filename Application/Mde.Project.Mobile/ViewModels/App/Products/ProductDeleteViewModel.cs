using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Products.Service.Interfaces;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Products;
using Mde.Project.Mobile.Models.Products;
using Mde.Project.Mobile.ViewModels.Base;

namespace Mde.Project.Mobile.ViewModels.App.Products
{
    public partial class ProductDeleteViewModel : UserViewModel
    {
        [ObservableProperty]
        private ProductItemDisplayModel _product;

        [ObservableProperty]
        private bool _isBusy;

        private readonly IAuthenticationService _authenticationService;
        private readonly IProductService _productService;
        public ProductDeleteViewModel(IAuthenticationService authenticationService, IProductService productService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _productService = productService;
            WeakReferenceMessenger.Default.Register<SendProductDisplayMessage>(this, (r, m) =>
            {
                Product = new ProductItemDisplayModel { Id = m.Id, Name = m.Name, ImageSource = m.ImageSource };
            });
        }

        [RelayCommand]
        public async Task DeleteProductAsync()
        {
            IsBusy = true;
            var result = await _productService.TryRemoveProductAsync(Product.Id);
            if (!result.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(ProductDeleteViewModel)));
                IsBusy = false;
                return;
            }

            await Shell.Current.GoToAsync("../../");
            WeakReferenceMessenger.Default.Send(new SendProductsChangedMessage());
        }
    }
}
