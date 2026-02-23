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
    public partial class BrandDetailViewModel : UserViewModel
    {
        [ObservableProperty]
        private BrandDisplayModel _brand;

        [ObservableProperty]
        private bool _isBusy;

        private IAuthenticationService _authenticationService;
        private IProductService _productService;
        public BrandDetailViewModel(IAuthenticationService authenticationService, IProductService productService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _productService = productService;
            Brand = new BrandDisplayModel();
            WeakReferenceMessenger.Default.Register<SendBrandIdentifierMessage>(this, async (r, m) =>
            {
                await InitializeBrandCommand.ExecuteAsync(m.Id);
            });
        }

        [RelayCommand]
        public async Task InitializeBrandAsync(Guid id)
        {
            IsBusy = true;

            if (id == Guid.Empty)
            {
                await Shell.Current.GoToAsync("..");
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Brand did not have a correct Id.", typeof(BrandDetailViewModel)));
            }

            var brand = await _productService.TryGetBrandByIdAsync(id);
            if (!brand.IsSuccess)
            {
                await Shell.Current.GoToAsync("..");
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Cannot find a brand with given id.", typeof(BrandDetailViewModel)));
            }

            Brand = new BrandDisplayModel
            {
                Id = brand.Data.Id,
                Name = brand.Data.Name,
                TotalProducts = brand.Data.ProductCount
            };

            IsBusy = false;
        }

        [RelayCommand]
        public async Task BrandEditNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.BrandUpsert);
            WeakReferenceMessenger.Default.Send(new SendBrandDetailMessage(Brand.Id.Value, Brand.Name));
        }

        [RelayCommand]
        public async Task BrandDeleteNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.BrandDelete);
            WeakReferenceMessenger.Default.Send(new SendBrandDetailMessage(Brand.Id.Value, Brand.Name));
        }
    }
}
