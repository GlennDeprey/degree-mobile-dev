using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Models.Dashboard;
using Mde.Project.Mobile.ViewModels.Base;
using SkiaSharp.Extended.UI.Controls;

namespace Mde.Project.Mobile.ViewModels.App
{
    public partial class DashboardViewModel : UserViewModel
    {
        [ObservableProperty]
        private ObservableCollection<CarouselItemModel> _carouselItems;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private int _selectedPosition;

        public DashboardViewModel(IAuthenticationService authenticationService) : base(authenticationService)
        {
            CarouselItems = new ObservableCollection<CarouselItemModel>();
        }

        [RelayCommand]
        public async Task InitializeDashboardAsync() 
        {
            IsBusy = true;
            await InitializeAccountAsync();
            await IsAdministatorAsync();
            LoadCarouselItems();
            IsBusy = false;
        }
        private void LoadCarouselItems()
        {
            if (CarouselItems == null || !CarouselItems.Any())
            {
                CarouselItems = new ObservableCollection<CarouselItemModel>
                {
                    new CarouselItemModel { AnimationSource = SKLottieImageSource.FromFile("dashboard_hello.json"), Margin= new Thickness(0,0,0,-10), Scale = 1, Title = "Welcome,", Content = User?.FullName ?? "Unknown"},
                    new CarouselItemModel { AnimationSource = SKLottieImageSource.FromFile("dashboard_location.json"), Margin=new Thickness(0,0,30,-40), Scale = 1, Title = "Google Places", Content = "We make use of google api for places and maps."},
                    new CarouselItemModel { AnimationSource = SKLottieImageSource.FromFile("dashboard_scanning.json"), Margin=new Thickness(0,0,30,0), Scale = 1, Title = "Camera", Content = "We make use of native camera for product scanning."}
                };
            }

            SelectedPosition = 1;
        }

        [RelayCommand]
        public void ScannerNavigation()
        {
            Application.Current.MainPage = new ScannerShell();
        }

        [RelayCommand]
        public void ProductsNavigation()
        {
            Application.Current.MainPage = new ProductsShell();
        }

        [RelayCommand]
        public void WarehousesNavigation()
        {
            Application.Current.MainPage = new WarehousesShell();
        }

        [RelayCommand]
        public void OperationsNavigation()
        {
            Application.Current.MainPage = new OperationsShell();
        }

        [RelayCommand]
        public void ReportsNavigation()
        {
            Application.Current.MainPage = new ReportsShell();
        }

        [RelayCommand]
        public void StatisticsNavigation()
        {
            Application.Current.MainPage = new StatisticsShell();
        }
    }
}
