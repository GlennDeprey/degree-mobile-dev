using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Products.RequestModels;
using Mde.Project.Mobile.Core.Products.Service.Interfaces;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Products;
using Mde.Project.Mobile.Models.Products;
using Mde.Project.Mobile.ViewModels.Base;

namespace Mde.Project.Mobile.ViewModels.App.Products
{
    public partial class BrandUpsertViewModel : UserViewModel
    {
        [ObservableProperty]
        private BrandDisplayModel _brand;

        [ObservableProperty] private string _title;

        [ObservableProperty]
        private bool _isBusy;

        private readonly IAuthenticationService _authenticationService;
        private readonly IProductService _productService;
        public BrandUpsertViewModel(IAuthenticationService authenticationService, IProductService productService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _productService = productService;
            Brand = new BrandDisplayModel();
            Title = "Add Brand";

            WeakReferenceMessenger.Default.Register(this, (MessageHandler<object, SendBrandDetailMessage>)((r, m) =>
            {
                Brand = new BrandDisplayModel
                {
                    Id = m.Id,
                    Name = m.Name
                };
                Title = "Edit Brand";
            }));
        }

        [RelayCommand]
        public async Task UpsertBrandAsync()
        {
            IsBusy = true;
            var brandUpsertRequestModel = new BrandUpsertRequestModel();
            brandUpsertRequestModel.Name = Brand.Name;

            if (Brand.Id != null && Brand.Id != Guid.Empty)
            {
                brandUpsertRequestModel.Id = Brand.Id.Value;
            }

            var result = await _productService.TryUpsertBrandAsync(brandUpsertRequestModel);
            if (!result.IsSuccess)
            {
                var messageAction = brandUpsertRequestModel.Id != Guid.Empty ? "updating" : "adding";
                WeakReferenceMessenger.Default.Send(new SendToastrMessage($"There was a issue with {messageAction} the brand.", typeof(BrandUpsertViewModel)));
                IsBusy = false;
                return;
            }
            await Shell.Current.GoToAsync("..");
            WeakReferenceMessenger.Default.Send(new SendBrandsChangedMessage());
            if (Brand.Id != null && Brand.Id != Guid.Empty)
            {
                WeakReferenceMessenger.Default.Send(new SendBrandIdentifierMessage(Brand.Id.Value));
            }
        }
    }
}
