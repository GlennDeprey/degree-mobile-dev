using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Authentication;

namespace Mde.Project.Mobile.ViewModels.Authentication
{
    public partial class ForgetPasswordViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _email;

        // Services
        private readonly IAuthenticationService _authenticationService;
        public ForgetPasswordViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // Commands
        public bool CanSendCode => !SendCodeCommand.IsRunning;

        [RelayCommand(CanExecute = nameof(CanSendCode))]
        public async Task SendCodeAsync()
        {
            try
            {
                var result = await _authenticationService.TryForgotPasswordAsync(Email);
                if (!result)
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage("The email is not found.", typeof(ForgetPasswordViewModel)));
                    return;
                }

                WeakReferenceMessenger.Default.Send(new SendEmailMessage(Email));
                await Shell.Current.GoToAsync(MauiRoutes.AuthVerifyCode);
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There is a issue connecting with the server.", typeof(ForgetPasswordViewModel)));
            }
        }

        [RelayCommand]
        public async Task SignInNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.AuthSignIn);
        }
    }
}
