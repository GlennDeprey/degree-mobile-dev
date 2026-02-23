using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Messages;

namespace Mde.Project.Mobile.ViewModels.Authentication
{
    public partial class SignUpViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _firstName;

        [ObservableProperty]
        private string _lastName;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _repeatPassword;

        // Services
        private readonly IAuthenticationService _authenticationService;
        public SignUpViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // Commands
        public bool CanSignUp => !SignUpCommand.IsRunning;

        [RelayCommand(CanExecute = nameof(CanSignUp))]
        public async Task SignUpAsync()
        {
            if (string.Compare(Password, RepeatPassword) != 0)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Passwords do not match.", typeof(SignUpViewModel)));
                return;
            }

            try
            {
                var result = await _authenticationService.TrySignUpAsync(Email, FirstName, LastName, Password);
                if (!result)
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage("There was a problem with the registration.", typeof(SignUpViewModel)));
                    return;
                }

                await Shell.Current.GoToAsync(MauiRoutes.AuthSignIn);
            }
            catch (Exception)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There is a issue connecting with the server.", typeof(SignUpViewModel)));
            }
        }

        [RelayCommand]
        public async Task SignInNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.AuthSignIn);
        }
    }
}
