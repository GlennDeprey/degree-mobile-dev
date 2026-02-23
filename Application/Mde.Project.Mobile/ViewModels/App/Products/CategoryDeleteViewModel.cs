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
    public partial class CategoryDeleteViewModel : UserViewModel
    {
        [ObservableProperty]
        private CategoryDisplayModel _category;

        [ObservableProperty]
        private bool _isBusy;

        private readonly IAuthenticationService _authenticationService;
        private readonly IProductService _productService;
        public CategoryDeleteViewModel(IAuthenticationService authenticationService, IProductService productService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _productService = productService;
            Category = new CategoryDisplayModel();

            WeakReferenceMessenger.Default.Register(this, (MessageHandler<object, SendCategoryDetailMessage>)((r, m) =>
            {
                Category = new CategoryDisplayModel { Id = m.Id, Name = m.Name};
            }));
        }

        [RelayCommand]
        public async Task DeleteCategoryAsync()
        {
            IsBusy = true;
            var result = await _productService.TryRemoveCategoryAsync(Category.Id.Value);
            if (!result.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(CategoryDeleteViewModel)));
                IsBusy = false;
                return;
            }

            await Shell.Current.GoToAsync("../../");
            WeakReferenceMessenger.Default.Send(new SendCategoriesChangedMessage());
        }
    }
}
