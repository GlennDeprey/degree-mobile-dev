using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Services.Interface;
using Mde.Project.Mobile.Services.Types;

namespace Mde.Project.Mobile.ViewModels.Authentication
{
    public partial class SignInViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        // Services
        private readonly IAuthenticationService _authenticationService;
        private readonly IExternalLoginService _externalLoginService;

        public SignInViewModel(IAuthenticationService authenticationService, IExternalLoginService externalLoginService)
        {
            _authenticationService = authenticationService;
            _externalLoginService = externalLoginService;
#if DEBUG
            Email = "glenn.deprey@student.howest.be";
            Password = "Test123?";
#endif
        }

        // Commands
        public bool CanExecuteAction => !SignInCommand.IsRunning && 
                                       !GoogleSignInCommand.IsRunning && 
                                       !MicrosoftSignInCommand.IsRunning;

        [RelayCommand]
        public async Task ForgotPasswordNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.AuthForgotPassword);
        }

        [RelayCommand]
        public async Task SignUpNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.AuthSignUp);
        }

        [RelayCommand(CanExecute = nameof(CanExecuteAction))]
        public async Task SignInAsync()
        {
            try
            {
                var result = await _authenticationService.TrySignInAsync(Email, Password);
                if (!result)
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage("There was no user found with those credentials.", typeof(SignInViewModel)));
                    return;
                }

                Application.Current.MainPage = new AppShell();
            }
            catch (Exception)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There is a issue connecting with the server.", typeof(SignInViewModel)));
            }
        }

        [RelayCommand(CanExecute = nameof(CanExecuteAction))]
        public async Task MicrosoftSignInAsync()
        {
            try
            {
                var result = await _externalLoginService.LoginAsync(ExternalLoginType.Microsoft);
                if (result.IsSuccess)
                {
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(SignInViewModel)));
                }
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage($"Authentication failed: {ex.Message}", typeof(SignInViewModel)));
            }
        }

        [RelayCommand(CanExecute = nameof(CanExecuteAction))]
        public async Task GoogleSignInAsync()
        {
            try
            {
                var result = await _externalLoginService.LoginAsync(ExternalLoginType.Google);
                if (result.IsSuccess)
                {
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(SignInViewModel)));
                }
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage($"Authentication failed: {ex.Message}", typeof(SignInViewModel)));
            }
        }
    }
}
