using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Items.Interfaces;
using Mde.Project.Mobile.Core.Items.RequestModels;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Operations;
using Mde.Project.Mobile.Models.Warehouse;
using Mde.Project.Mobile.ViewModels.Base;

namespace Mde.Project.Mobile.ViewModels.App.Operations
{
    public partial class StockSettingsViewModel : UserViewModel
    {
        [ObservableProperty]
        private WarehouseStockItemModel _item;

        [ObservableProperty]
        private bool _isBusy;

        private readonly IWarehouseItemsService _warehouseItemsService;
        public StockSettingsViewModel(IAuthenticationService authenticationService, IWarehouseItemsService warehouseItemsService) : base(authenticationService)
        {
            _warehouseItemsService = warehouseItemsService;
            Item = new WarehouseStockItemModel();
            WeakReferenceMessenger.Default.Register<SendItemInfoMessage>(this, async (r, m) =>
            {
                Item = m.Item;
            });
        }

        [RelayCommand]
        public async Task UpdateItemSettingsAsync()
        {
            if (Item.MinimumQuantity < 0 || Item.RefillQuantity < 0)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Quantity can not be below 0", typeof(StockSettingsViewModel)));
                return;
            }

            IsBusy = true;
            var requestModel = new UpdateItemRequestModel
            {
                IsChangeSettings = true,
                MinimumQuantity = Item.MinimumQuantity,
                RefillQuantity = Item.RefillQuantity,
                HasAutoRefill = Item.HasAutoRefill
            };

            var result = await _warehouseItemsService.TryUpdateItemAsync(Item.WarehouseId, Item.Product.Id, requestModel);
            if (result.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Settings updated successfully.", typeof(StockSettingsViewModel)));
                await Shell.Current.GoToAsync("..");
                WeakReferenceMessenger.Default.Send(new SendItemInfoChangedMessage());
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Failed to update settings.", typeof(StockSettingsViewModel)));
            }

            IsBusy = false;
        }
    }
}
