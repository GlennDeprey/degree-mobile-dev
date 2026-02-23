using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Warehouses.Interface;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Warehouses;
using Mde.Project.Mobile.Models.Warehouse;
using Mde.Project.Mobile.ViewModels.Base;

namespace Mde.Project.Mobile.ViewModels.App.Warehouses
{
    public partial class WarehouseDeleteViewModel : UserViewModel
    {
        [ObservableProperty]
        private WarehouseItemModel _warehouse;

        [ObservableProperty]
        private bool _isBusy;

        private readonly IAuthenticationService _authenticationService;
        private readonly IWarehouseService _warehouseService;
        public WarehouseDeleteViewModel(IAuthenticationService authenticationService, IWarehouseService warehouseService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _warehouseService = warehouseService;
            WeakReferenceMessenger.Default.Register<SendWarehouseDeleteMessage>(this, (r, m) =>
            {
                Warehouse = new WarehouseItemModel
                {
                    Id = m.Id,
                    Name = m.Name
                };
            });
        }

        [RelayCommand]
        public async Task DeleteWarehouseAsync()
        {
            IsBusy = true;
            var result = await _warehouseService.TryRemoveWarehouseAsync(Warehouse.Id.Value);
            if (!result.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(WarehouseDeleteViewModel)));
                IsBusy = false;
                return;
            }

            await Shell.Current.GoToAsync("../../");
            WeakReferenceMessenger.Default.Send(new SendWarehousesChangedMessage());
        }
    }
}
