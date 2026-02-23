using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Authentication;
using Mde.Project.Mobile.Models.Accounts;
using Mde.Project.Mobile.ViewModels.Authentication;
using UserProfileModel = Mde.Project.Mobile.Models.Accounts.UserProfileModel;

namespace Mde.Project.Mobile.ViewModels.Base
{
    public abstract partial class UserViewModel : ObservableObject
    {
        [ObservableProperty]
        protected UserProfileModel _user;

        [ObservableProperty] 
        protected bool _isAdmin;

        private readonly IAuthenticationService _authenticationService;
        public UserViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [RelayCommand]
        public async Task InitializeAccountAsync()
        {
            await ReloadLoadUserProfileAsync();
            if (User != null)
            {
                UpdateAdminState();
            }
        }

        [RelayCommand]
        public void UpdateAdminState()
        {
            IsAdmin = User.Roles.Contains("Admin");
        }

        [RelayCommand]
        public async Task IsAdministatorAsync()
        {
            IsAdmin = await _authenticationService.IsInRole("Admin");
            if (!IsAdmin)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new ScannerShell();
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage("You are not authorized to see this content", typeof(UserViewModel)));
                });
            }
        }

        [RelayCommand]
        public void HomeNavigation()
        {
            Application.Current.MainPage = new AppShell();
        }

        [RelayCommand]
        public async Task ProfileNavigation()
        {
            await Shell.Current.GoToAsync(MauiRoutes.Profile);
        }

        protected async Task ReloadLoadUserProfileAsync() 
        {
            var userModel = await _authenticationService.TryGetProfileAsync();
            if (!userModel.IsSuccess)
            {
                _authenticationService.SignOut();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new AuthShell();
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage("There was no user found with those credentials, please reconnect", typeof(SignInViewModel)));
                });
                return;
            }

            User = new UserProfileModel
            {
                Id = userModel.Data.Id,
                Email = userModel.Data.Email,
                ImageUrl = string.IsNullOrEmpty(userModel.Data.ImageUrl) ? Constants.NoImageUri : $"{Constants.DefaultUploadRoot}{userModel.Data.ImageUrl}",
                FirstName = userModel.Data.FirstName,
                LastName = userModel.Data.LastName,
                Roles = userModel.Data.Roles.ToList(),
                ExternalAccounts = userModel.Data.ExternalAccounts
                .Select(ea => new ExternalLoginModel
                {
                    LoginProvider = ea.LoginProvider,
                    ProviderDisplayName = ea.ProviderDisplayName,
                    ProviderKey = ea.ProviderKey,
                }).ToList(),
            };

            if (string.IsNullOrWhiteSpace(User.ImageUrl))
            {
                User.ImageUrl = Constants.NoImageUri + $"{User.FirstName[0]}";
            }
        }
    }
}
