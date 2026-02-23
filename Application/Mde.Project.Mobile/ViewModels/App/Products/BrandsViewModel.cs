using System.Collections.ObjectModel;
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
    public partial class BrandsViewModel : UserViewModel
    {
        [ObservableProperty]
        private ObservableCollection<BrandDisplayModel> _brands;

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

        private readonly IProductService _productService;
        private readonly IAuthenticationService _authenticationService;
        public BrandsViewModel(IAuthenticationService authenticationService, IProductService productService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _productService = productService;
            CurrentPage = 1;

            WeakReferenceMessenger.Default.Register<SendBrandsChangedMessage>(this, async (r, m) =>
            {
                await InitializeBrandsCommand.ExecuteAsync(null);
            });
        }

        [RelayCommand]
        public async Task InitializeBrandsAsync()
        {
            IsBusy = true;
            await InitializeAccountAsync();
            await LoadBrandsAsync(CurrentPage, Filter);
            IsBusy = false;
        }

        [RelayCommand]
        public async Task ChangeBrandPageAsync(int page)
        {
            IsBusy = true;

            if (page >= 1 && page <= TotalPages)
            {
                CurrentPage = page;
            }

            await LoadBrandsAsync(CurrentPage, Filter);

            IsBusy = false;
        }

        [RelayCommand]
        public async Task BrandDetailNavigationAsync(Guid id)
        {
            await Shell.Current.GoToAsync(MauiRoutes.BrandDetail);
            WeakReferenceMessenger.Default.Send(new SendBrandIdentifierMessage(id));
        }

        [RelayCommand]
        public async Task BrandAddNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.BrandUpsert);
        }

        private async Task LoadBrandsAsync(int currentPage, string filter)
        {
            var brands = await _productService.TryGetBrandListAsync(currentPage, filter);
            if (!brands.IsSuccess || !brands.Items.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(brands.Message, typeof(BrandsViewModel)));
                Brands = new ObservableCollection<BrandDisplayModel>();
                return;
            }

            Brands = new ObservableCollection<BrandDisplayModel>(
                brands.Items.Select(c => new BrandDisplayModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    TotalProducts = c.ProductCount

                })
            );
            TotalPages = brands.TotalPages;
            TotalItems = brands.TotalItems;
        }
    }
}
