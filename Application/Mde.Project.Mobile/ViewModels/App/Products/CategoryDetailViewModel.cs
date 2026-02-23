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
    public partial class CategoryDetailViewModel : UserViewModel
    {
        [ObservableProperty]
        private CategoryDisplayModel _category;

        [ObservableProperty]
        private bool _isBusy;

        private IAuthenticationService _authenticationService;
        private IProductService _productService;
        public CategoryDetailViewModel(IAuthenticationService authenticationService, IProductService productService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _productService = productService;
            Category = new CategoryDisplayModel();
            WeakReferenceMessenger.Default.Register<SendCategoryIdentifierMessage>(this, async (r, m) =>
            {
                await InitializeCategoryCommand.ExecuteAsync(m.Id);
            });
        }

        [RelayCommand]
        public async Task InitializeCategoryAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There was no correct Category id.", typeof(CategoryDetailViewModel)));
                await Shell.Current.GoToAsync("..");
            }

            IsBusy = true;
            var category = await _productService.TryGetCategoryByIdAsync(id);
            if (!category.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("The given category was not found.", typeof(CategoryDetailViewModel)));
                await Shell.Current.GoToAsync("..");
            }

            Category = new CategoryDisplayModel
            {
                Id = category.Data.Id,
                Name = category.Data.Name,
                TotalProducts = category.Data.ProductCount
            };
            IsBusy = false;
        }

        [RelayCommand]
        public async Task CategoryEditNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.CategoryUpsert);
            WeakReferenceMessenger.Default.Send(new SendCategoryDetailMessage(Category.Id.Value, Category.Name));
        }

        [RelayCommand]
        public async Task CategoryDeleteNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.CategoryDelete);
            WeakReferenceMessenger.Default.Send(new SendCategoryDetailMessage(Category.Id.Value, Category.Name));
        }
    }
}
