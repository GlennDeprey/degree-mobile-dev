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
    public partial class BrandDeleteViewModel : UserViewModel
    {
        [ObservableProperty]
        private BrandDisplayModel _brand;

        [ObservableProperty]
        private bool _isBusy;

        private readonly IAuthenticationService _authenticationService;
        private readonly IProductService _productService;
        public BrandDeleteViewModel(IAuthenticationService authenticationService, IProductService productService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _productService = productService;
            Brand = new BrandDisplayModel();
            WeakReferenceMessenger.Default.Register<SendBrandDetailMessage>(this, (r, m) =>
            {
                Brand = new BrandDisplayModel
                {
                    Id = m.Id,
                    Name = m.Name
                };
            });
        }

        [RelayCommand]
        public async Task DeleteBrandAsync()
        {
            IsBusy = true;
            var result = await _productService.TryRemoveBrandAsync(Brand.Id.Value);
            if (!result.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(BrandDeleteViewModel)));
                IsBusy = false;
                return;
            }

            await Shell.Current.GoToAsync("../../");
            WeakReferenceMessenger.Default.Send(new SendBrandsChangedMessage());
        }
    }
}
