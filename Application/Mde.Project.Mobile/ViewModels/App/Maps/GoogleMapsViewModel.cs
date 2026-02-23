using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.ViewModels.Base;

namespace Mde.Project.Mobile.ViewModels.App.Maps
{
    public partial class GoogleMapsViewModel : UserViewModel
    {
        [ObservableProperty] private bool _isBusy;
        public GoogleMapsViewModel(IAuthenticationService authenticationService) : base(authenticationService)
        {
            IsBusy = true;
        }

        [RelayCommand]
        public void LoadedComplete()
        {
            IsBusy = false;
        }
    }
}
