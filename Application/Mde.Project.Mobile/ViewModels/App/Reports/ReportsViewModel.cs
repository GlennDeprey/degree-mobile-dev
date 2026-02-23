using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Models.Reports;
using Mde.Project.Mobile.Core.Reports.Service.Interfaces;
using Mde.Project.Mobile.Core.Warehouses.Interface;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Models.Reports;
using Mde.Project.Mobile.Models.Warehouse;
using Mde.Project.Mobile.Services.Interface;
using Mde.Project.Mobile.ViewModels.Base;
using System.Collections.ObjectModel;

namespace Mde.Project.Mobile.ViewModels.App.Reports
{
    public partial class ReportsViewModel : UserViewModel
    {
        [ObservableProperty] 
        private ObservableCollection<ReportModel> _warehouseReports;

        [ObservableProperty]
        private ObservableCollection<WarehouseItemModel> _warehouses;

        [ObservableProperty]
        private WarehouseItemModel _selectedWarehouse;

        [ObservableProperty]
        private int _currentPage;

        [ObservableProperty]
        private int _totalPages;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private int _totalItems;

        private int _pageSize = 10;

        private readonly IAuthenticationService _authenticationService;
        private readonly IWarehouseService _warehouseService;
        private readonly IWarehouseHubService _warehouseHubService;
        private readonly IReportsService _reportsService;
        public ReportsViewModel(IAuthenticationService authenticationService, IWarehouseService warehouseService, IWarehouseHubService warehouseHubService, IReportsService reportsService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _warehouseService = warehouseService;
            _warehouseHubService = warehouseHubService;
            _reportsService = reportsService;

            WarehouseReports = new ObservableCollection<ReportModel>();
            CurrentPage = 1;

            _warehouseHubService.RegisterErrorHandler((error) =>
            {
                OnHubErrorHandling(error);
                return Task.CompletedTask;
            });

            _warehouseHubService.RegisterNewReportHandler((newReport) =>
            {
                AddNewReport(newReport);
                return Task.CompletedTask;
            });
        }

        [RelayCommand]
        public async Task InitializeReportsAsync()
        {
            IsBusy = true;
            await InitializeAccountAsync();
            await LoadWarehousesAsync();

            SelectedWarehouse = Warehouses.FirstOrDefault();

            if (SelectedWarehouse?.Id != null)
            {
                await _warehouseHubService.TryConnectAsync(SelectedWarehouse.Id.Value);
                await FilterReportsAsync();
            }
            IsBusy = false;
        }

        [RelayCommand]
        public async Task WarehouseChangedAsync()
        {
            IsBusy = true;
            await _warehouseHubService.DisconnectAsync();
            CurrentPage = 1;

            if (SelectedWarehouse != null)
            {
                await _warehouseHubService.TryConnectAsync(SelectedWarehouse.Id.Value);
                await FilterReportsAsync();
            }
            IsBusy = false;
        }

        [RelayCommand]
        public async Task FilterReportsAsync()
        {
            if (SelectedWarehouse?.Id != null)
            {
                IsBusy = true;
                await LoadReportsAsync();
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task ChangeReportPageAsync(int page)
        {
            if (page >= 1 && page <= TotalPages)
            {
                CurrentPage = page;
                IsBusy = true;

                if (SelectedWarehouse?.Id != null)
                {
                    await FilterReportsAsync();
                }

                IsBusy = false;
            }
        }

        private void OnHubErrorHandling(string error)
        {
            WeakReferenceMessenger.Default.Send(new SendToastrMessage(error, typeof(ReportsViewModel)));
        }

        private async Task LoadWarehousesAsync()
        {
            var warehouses = await _warehouseService.TryGetWarehouseOptionsAsync();
            if (!warehouses.IsSuccess || !warehouses.Items.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(warehouses.Message, typeof(ReportsViewModel)));
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

        private async Task LoadReportsAsync()
        {
            TotalItems = 0;
            TotalPages = 1;

            var reports = await _reportsService.TryGetReportListAsync(SelectedWarehouse.Id.Value, CurrentPage, _pageSize);
            if (reports == null || !reports.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(reports.Message, typeof(ReportsViewModel)));
                WarehouseReports = new ObservableCollection<ReportModel>();
                return;
            }

            if (reports.Items == null || !reports.Items.Any())
            {
                WarehouseReports = new ObservableCollection<ReportModel>();
                return;
            }

            WarehouseReports = new ObservableCollection<ReportModel>(
                reports.Items.Select(c => new ReportModel
                {
                    Id = c.Id,
                    WarehouseId = c.WarehouseId,
                    ProductId = c.ProductId,
                    QuantityChange = c.QuantityChange,
                    Tag = c.Tag,
                    Description = c.Description,
                    CreatedOn = c.CreatedOn,
                })
            );

            TotalItems = reports.TotalItems;
            TotalPages = reports.TotalPages;
        }

        private void AddNewReport(Report newReport)
        {
            if (newReport.WarehouseId != SelectedWarehouse?.Id)
            {
                return;
            }

            if (newReport == null)
            {
                return;
            }

            if (WarehouseReports == null)
            {
                WarehouseReports = new ObservableCollection<ReportModel>();
            }

            WarehouseReports.Insert(0, new ReportModel
            {
                Id = newReport.Id,
                WarehouseId = newReport.WarehouseId,
                ProductId = newReport.ProductId,
                QuantityChange = newReport.QuantityChange,
                Tag = newReport.Tag,
                Description = newReport.Description,
                CreatedOn = newReport.CreatedOn
            });

            if (WarehouseReports.Count > _pageSize)
            {
                WarehouseReports.RemoveAt(WarehouseReports.Count - 1);
            }
        }
    }
}
