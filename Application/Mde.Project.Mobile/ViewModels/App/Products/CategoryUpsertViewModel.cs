using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Core.Products.RequestModels;
using Mde.Project.Mobile.Core.Products.Service.Interfaces;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Products;
using Mde.Project.Mobile.Models.Products;
using Mde.Project.Mobile.ViewModels.Base;

namespace Mde.Project.Mobile.ViewModels.App.Products
{
    public partial class CategoryUpsertViewModel : UserViewModel
    {
        [ObservableProperty]
        private CategoryDisplayModel _category;

        [ObservableProperty] private string _title;

        [ObservableProperty]
        private bool _isBusy;

        private readonly IAuthenticationService _authenticationService;
        private readonly IProductService _productService;
        public CategoryUpsertViewModel(IAuthenticationService authenticationService, IProductService productService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _productService = productService;
            Category = new CategoryDisplayModel();
            Title = "Add Category";

            WeakReferenceMessenger.Default.Register<SendCategoryDetailMessage>(this, (r, m) =>
            {
                Category = new CategoryDisplayModel
                {
                    Id = m.Id,
                    Name = m.Name
                };
                Title = "Edit Category";
            });
        }

        [RelayCommand]
        public async Task UpsertCategoryAsync()
        {
            IsBusy = true;
            var categoryUpsertRequestModel = new CategoryUpsertRequestModel();
            categoryUpsertRequestModel.Name = Category.Name;

            if (Category.Id != null && Category.Id != Guid.Empty)
            {
                categoryUpsertRequestModel.Id = Category.Id.Value;
            }

            var result = await _productService.TryUpsertCategoryAsync(categoryUpsertRequestModel);
            if (!result.IsSuccess)
            {
                var messageAction = categoryUpsertRequestModel.Id != Guid.Empty ? "updating" : "adding";
                WeakReferenceMessenger.Default.Send(new SendToastrMessage($"There was a issue with {messageAction} the category.", typeof(CategoryUpsertViewModel)));
                IsBusy = false;
                return;
            }
            await Shell.Current.GoToAsync("..");
            WeakReferenceMessenger.Default.Send(new SendCategoriesChangedMessage());
            if (Category.Id != null && Category.Id != Guid.Empty)
            {
                WeakReferenceMessenger.Default.Send(new SendCategoryIdentifierMessage(Category.Id.Value));
            }
        }
    }
}
