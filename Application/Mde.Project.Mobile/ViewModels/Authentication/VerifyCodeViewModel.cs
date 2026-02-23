using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Authentication;

namespace Mde.Project.Mobile.ViewModels.Authentication
{
    public partial class VerifyCodeViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanVerifyCode))]
        private string _firstDigit;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanVerifyCode))]
        private string _secondDigit;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanVerifyCode))]
        private string _thirdDigit;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanVerifyCode))]
        private string _fourthDigit;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanVerifyCode))]
        private string _fifthDigit;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanVerifyCode))]
        private string _sixthDigit;

        [ObservableProperty] private string _email;


        // Services
        private readonly IAuthenticationService _authenticationService;
        public VerifyCodeViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;

            WeakReferenceMessenger.Default.Register<SendEmailMessage>(this, (r, m) =>
            {
                Email = m.Email;
            });
        }

        // Commands
        public bool CanVerifyCode => !string.IsNullOrWhiteSpace(FirstDigit) &&
                                     !string.IsNullOrWhiteSpace(SecondDigit) &&
                                     !string.IsNullOrWhiteSpace(ThirdDigit) &&
                                     !string.IsNullOrWhiteSpace(FourthDigit) &&
                                     !string.IsNullOrWhiteSpace(FifthDigit) &&
                                     !string.IsNullOrWhiteSpace(SixthDigit);

        [RelayCommand(CanExecute = nameof(CanVerifyCode))]
        public async Task VerifyCodeAsync()
        {
            try
            {
                var inputCode = FirstDigit + SecondDigit + ThirdDigit + FourthDigit + FifthDigit + SixthDigit;
                var code = await _authenticationService.TryVerifyCodeAsync(Email, inputCode);
                if (code == null)
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage("The code is incorrect or has expired after 15 minutes.", typeof(VerifyCodeViewModel)));
                    return;
                }

                WeakReferenceMessenger.Default.Send(new SendVerificationMessage(Email, code));
                await Shell.Current.GoToAsync(MauiRoutes.AuthResetPassword);
            }
            catch (Exception)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("There is a issue connecting with the server.", typeof(VerifyCodeViewModel)));
            }
        }

        [RelayCommand]
        public async Task BackToForgotPasswordNavigationAsync()
        {
            await Shell.Current.GoToAsync(MauiRoutes.AuthForgotPassword);
        }
    }
}
