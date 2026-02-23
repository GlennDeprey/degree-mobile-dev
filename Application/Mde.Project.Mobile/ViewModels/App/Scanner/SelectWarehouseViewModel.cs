using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.ViewModels.Base;
using System.Collections.ObjectModel;
using Mde.Project.Mobile.Core.Warehouses.Interface;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Models.Warehouse;
using Mde.Project.Mobile.Messages.Scanner;

namespace Mde.Project.Mobile.ViewModels.App.Scanner
{
    public partial class SelectWarehouseViewModel : UserViewModel
    {
        [ObservableProperty]
        private ObservableCollection<WarehouseItemModel> _warehouses;

        [ObservableProperty]
        private WarehouseItemModel _selectedWarehouse;


        private readonly IAuthenticationService _authenticationService;
        private readonly IWarehouseService _warehouseService;
        public SelectWarehouseViewModel(IAuthenticationService authenticationService, IWarehouseService warehouseService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _warehouseService = warehouseService;
        }

        [RelayCommand]
        public async Task ScannerNavigation()
        {
            await Shell.Current.GoToAsync(MauiRoutes.ScannerScan);
            WeakReferenceMessenger.Default.Send(new SendSelectedWarehouseMessage(SelectedWarehouse.Id.Value));
        }

        [RelayCommand]
        public void DashboardNavigation()
        {
            if (!IsAdmin) return;
            Application.Current.MainPage = new AppShell();
        }

        [RelayCommand]
        public async Task InitializeWarehousesAsync()
        {
            await InitializeAccountAsync();

            Warehouses = new();
            var warehouses = await _warehouseService.TryGetWarehouseOptionsAsync();
            if (!warehouses.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(warehouses.Message, typeof(SelectWarehouseViewModel)));
                return;
            }

            foreach (var warehouse in warehouses.Items)
            {
                Warehouses.Add(new WarehouseItemModel
                {
                    Id = warehouse.Id,
                    Name = warehouse.Name
                });
            }
        }
    }
}
