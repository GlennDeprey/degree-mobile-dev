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
    public partial class CategoriesViewModel : UserViewModel
    {
        [ObservableProperty]
        private ObservableCollection<CategoryDisplayModel> _categories;

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
        private readonly IProductService _productService;
        public CategoriesViewModel(IAuthenticationService authenticationService, IProductService productService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _productService = productService;
            CurrentPage = 1;

            WeakReferenceMessenger.Default.Register<SendCategoriesChangedMessage>(this, async (r, m) =>
            {
                await InitializeCategoriesCommand.ExecuteAsync(null);
            });
        }

        [RelayCommand]
        public async Task InitializeCategoriesAsync()
        {
            IsBusy = true;
            await InitializeAccountAsync();
            await LoadCategoriesAsync(CurrentPage, Filter);
            IsBusy = false;
        }

        [RelayCommand]
        public async Task CategoryDetailNavigationAsync(Guid id)
        {
            await Shell.Current.GoToAsync(MauiRoutes.CategoryDetail);
            WeakReferenceMessenger.Default.Send(new SendCategoryIdentifierMessage(id));
        }

        [RelayCommand]
        public async Task CategoryAddNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.CategoryUpsert);
        }

        [RelayCommand]
        public async Task ChangeCategoryPageAsync(int page)
        {
            IsBusy = true;
            if (page >= 1 && page <= TotalPages)
            {
                CurrentPage = page;
            }
            await LoadCategoriesAsync(CurrentPage, Filter);
            IsBusy = false;
        }

        private async Task LoadCategoriesAsync(int currentPage, string filter)
        {
            var categories = await _productService.TryGetCategoryListAsync(currentPage, filter);
            if (!categories.IsSuccess || !categories.Items.Any())
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(categories.Message, typeof(CategoriesViewModel)));
                Categories = new ObservableCollection<CategoryDisplayModel>();
                return;
            }

            Categories = new ObservableCollection<CategoryDisplayModel>(
                categories.Items.Select(c => new CategoryDisplayModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    TotalProducts = c.ProductCount

                })
            );
            TotalPages = categories.TotalPages;
            TotalItems = categories.TotalItems;
        }
    }
}
