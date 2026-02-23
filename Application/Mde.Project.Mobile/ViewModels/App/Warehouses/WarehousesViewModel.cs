using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Warehouses.Interface;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Products;
using Mde.Project.Mobile.Messages.Warehouses;
using Mde.Project.Mobile.Models.Warehouse;
using Mde.Project.Mobile.ViewModels.Base;

namespace Mde.Project.Mobile.ViewModels.App.Warehouses
{
    public partial class WarehousesViewModel : UserViewModel
    {
        [ObservableProperty] private ObservableCollection<WarehouseDisplayModel> _warehouses;

        [ObservableProperty]
        private int _currentPage;

        [ObservableProperty]
        private int _totalPages;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private int _totalItems;

        [ObservableProperty]
        private string _filter;

        private readonly IAuthenticationService _authenticationService;
        private readonly IWarehouseService _warehouseService;
        public WarehousesViewModel(IAuthenticationService authenticationService, IWarehouseService warehouseService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _warehouseService = warehouseService;
            _currentPage = 1;

            WeakReferenceMessenger.Default.Register<SendWarehousesChangedMessage>(this, async (r, m) =>
            {
                Warehouses = new();
                await InitializeWarehousesCommand.ExecuteAsync(null);
            });
        }

        [RelayCommand]
        public async Task InitializeWarehousesAsync()
        {
            IsBusy = true;
            await InitializeAccountAsync();
            await LoadWarehousesAsync(CurrentPage, Filter);
            IsBusy = false;
        }

        [RelayCommand]
        public async Task ChangeWarehousePageAsync(int page)
        {
            IsBusy = true;

            if (page >= 1 && page <= TotalPages)
            {
                CurrentPage = page;
            }

            await LoadWarehousesAsync(CurrentPage, Filter);

            IsBusy = false;
        }

        [RelayCommand]
        public async Task WarehouseDetailNavigationAsync(Guid id)
        {
            await Shell.Current.GoToAsync(MauiRoutes.WarehouseDetail);
            WeakReferenceMessenger.Default.Send(new SendWarehouseIdentifierMessage(id));
        }

        [RelayCommand]
        public async Task WarehouseAddNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.WarehouseUpsert);
        }

        private async Task LoadWarehousesAsync(int page, string filter)
        {
            var warehouses = await _warehouseService.TryGetWarehouseListAsync(filter, page);
            if (!warehouses.IsSuccess || !warehouses.Items.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(warehouses.Message, typeof(WarehousesViewModel)));
                Warehouses = new ObservableCollection<WarehouseDisplayModel>();
                return;
            }

            Warehouses = new ObservableCollection<WarehouseDisplayModel>(
                warehouses.Items.Select(c => new WarehouseDisplayModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ShortName = c.ShortName,
                    Phone = c.Phone,
                    Earnings = c.Earnings
                })
            );
            TotalPages = warehouses.TotalPages;
            TotalItems = warehouses.TotalItems;
        }
    }
}
