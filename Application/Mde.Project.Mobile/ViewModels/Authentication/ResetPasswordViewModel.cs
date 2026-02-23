using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Authentication;

namespace Mde.Project.Mobile.ViewModels.Authentication
{
    public partial class ResetPasswordViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanResetPassword))]
        private string _password;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanResetPassword))]
        private string _repeatPassword;

        [ObservableProperty] private string _email;
        [ObservableProperty] private string _code;

        // Services
        private readonly IAuthenticationService _authenticationService;

        // Commands
        public bool CanResetPassword => !ResetPasswordCommand.IsRunning && 
                                        !string.IsNullOrWhiteSpace(Password) && 
            !string.IsNullOrWhiteSpace(RepeatPassword);

        [RelayCommand(CanExecute = nameof(CanResetPassword))]
        public async Task ResetPasswordAsync()
        {
            if (string.Compare(Password, RepeatPassword) != 0)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Passwords do not match.", typeof(ResetPasswordViewModel)));
                return;
            }

            try
            {
                var result = await _authenticationService.TryResetPasswordAsync(Email, Password, Code);
                if (!result)
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage("Invalid password change request.", typeof(ResetPasswordViewModel)));
                    return;
                }
                await Shell.Current.GoToAsync(MauiRoutes.AuthSignIn);
            }
            catch (Exception)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There is a issue connecting with the server.", typeof(ResetPasswordViewModel)));
            }
        }

        public ResetPasswordViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;

            WeakReferenceMessenger.Default.Register<SendVerificationMessage>(this, (r, m) =>
            {
                Email = m.Email;
                Code = m.Code;
            });
        }
    }
}
