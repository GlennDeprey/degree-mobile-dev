using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.ViewModels.Base;
using Mde.Project.Mobile.Services.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView.Maui;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Models.Statistics;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Messages;
using System.Collections.ObjectModel;
using Mde.Project.Mobile.Models.Warehouse;
using Mde.Project.Mobile.Core.Warehouses.Interface;
using Mde.Project.Mobile.Core.Statistics.Interface;
using Mde.Project.Mobile.Core.Models.Statistics;
using System.Runtime.InteropServices;

namespace Mde.Project.Mobile.ViewModels.App.Statistics
{
    public partial class StatisticsViewModel : UserViewModel
    {
        [ObservableProperty]
        private CartesianChart _latestSalesInfo;

        [ObservableProperty]
        private ObservableCollection<WarehouseItemModel> _warehouses;

        [ObservableProperty]
        private ObservableCollection<LatestSaleInfoModel> _latestSalesInfoCollection;

        [ObservableProperty]
        private WarehouseItemModel _selectedWarehouse;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private bool _isWarehouseStatsLoaded;

        private readonly IWarehouseService _warehouseService;
        private readonly IWarehouseHubService _warehouseHubService;
        private readonly IChartsService _chartsService;
        private readonly IStatisticsService _statisticsService;

        private bool isDark = Application.Current.RequestedTheme == AppTheme.Dark;

        public StatisticsViewModel(IAuthenticationService authenticationService, IChartsService chartsService, IWarehouseHubService warehouseHubService, IWarehouseService warehouseService, IStatisticsService statisticsService) : base(authenticationService)
        {
            _chartsService = chartsService;
            _warehouseHubService = warehouseHubService;
            _warehouseService = warehouseService;
            _statisticsService = statisticsService;

            LatestSalesInfoCollection = new ObservableCollection<LatestSaleInfoModel>();

            _warehouseHubService.RegisterErrorHandler((error) =>
            {
                OnHubErrorHandling(error);
                return Task.CompletedTask;
            });

            _warehouseHubService.RegisterWarehouseStatsUpdateHandler((updatedSaleInfo) =>
            {
                UpdateLatestSalesInfo(updatedSaleInfo);
                return Task.CompletedTask;
            });
        }

        [RelayCommand]
        public async Task InitializeAsync()
        {
            IsBusy = true;
            await InitializeAccountAsync();
            await LoadWarehousesAsync();

            SelectedWarehouse = Warehouses.FirstOrDefault();

            if (SelectedWarehouse?.Id != null)
            {
                await _warehouseHubService.TryConnectAsync(SelectedWarehouse.Id.Value);
                await LoadLatestSalesInfo(SelectedWarehouse.Id.Value);
            }
            IsBusy = false;
        }

        [RelayCommand]
        public async Task WarehouseChangedAsync()
        {
            IsBusy = true;
            IsWarehouseStatsLoaded = false;
            await _warehouseHubService.DisconnectAsync();

            if (SelectedWarehouse != null)
            {
                await _warehouseHubService.TryConnectAsync(SelectedWarehouse.Id.Value);
                await LoadLatestSalesInfo(SelectedWarehouse.Id.Value);
            }
            IsBusy = false;
        }

        private async Task LoadWarehousesAsync()
        {
            var warehouses = await _warehouseService.TryGetWarehouseOptionsAsync();
            if (!warehouses.IsSuccess || !warehouses.Items.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(warehouses.Message, typeof(StatisticsViewModel)));
                Warehouses = new ObservableCollection<WarehouseItemModel>();
                return;
            }

            Warehouses = new ObservableCollection<WarehouseItemModel>(
                warehouses.Items.Select(c => new WarehouseItemModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
            );
        }

        private void OnHubErrorHandling(string error)
        {
            WeakReferenceMessenger.Default.Send(new SendToastrMessage(error, typeof(StatisticsViewModel)));
        }

        private async Task LoadLatestSalesInfo(Guid warehouseId)
        {

            var warehouseStats = await _statisticsService.TryGetWarehouseStatsAsync(warehouseId);
            if (!warehouseStats.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(warehouseStats.Message, typeof(StatisticsViewModel)));
                LatestSalesInfoCollection = new ObservableCollection<LatestSaleInfoModel>();
                return;
            }

            if (warehouseStats.Items != null)
            {
                LatestSalesInfoCollection = new ObservableCollection<LatestSaleInfoModel>(
                    warehouseStats.Items.Select(c => new LatestSaleInfoModel
                    {
                        Id = c.Id,
                        Date = c.CreatedOn,
                        Sales = c.TotalSales,
                        Restock = c.TotalRestock
                    })
                );
            }

            var result = _chartsService.GenerateLatestSaleInfo(LatestSalesInfoCollection, isDark);
            if (!result.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(StatisticsViewModel)));
                return;
            }

            LatestSalesInfo = result.Data;
            IsWarehouseStatsLoaded = true;
        }

        private void UpdateLatestSalesInfo(WarehouseStats latestSaleInfoUpdate)
        {
            if (latestSaleInfoUpdate == null || latestSaleInfoUpdate.WarehouseId == Guid.Empty)
            {
                return;
            }

            if (SelectedWarehouse.Id.HasValue || SelectedWarehouse.Id.Value != latestSaleInfoUpdate.WarehouseId)
            {
                return;
            }

            var latestSaleInfo = new LatestSaleInfoModel
            {
                Id = latestSaleInfoUpdate.Id,
                Date = latestSaleInfoUpdate.CreatedOn,
                Sales = latestSaleInfoUpdate.TotalSales,
                Restock = latestSaleInfoUpdate.TotalRestock
            };

            var result = _chartsService.UpdateLatestSaleInfo(LatestSalesInfo, latestSaleInfo);
            if (!result.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(StatisticsViewModel)));
                return;
            }
        }
    }
}
