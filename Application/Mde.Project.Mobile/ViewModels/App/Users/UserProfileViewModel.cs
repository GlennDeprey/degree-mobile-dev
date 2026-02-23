using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Core;
using Mde.Project.Mobile.Core.Authentication.Services.Interfaces;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.Messages.Users;
using Mde.Project.Mobile.Services.Interface;
using Mde.Project.Mobile.Services.Types;
using Mde.Project.Mobile.ViewModels.Authentication;
using Mde.Project.Mobile.ViewModels.Base;

namespace Mde.Project.Mobile.ViewModels.App.Users
{
    public partial class UserProfileViewModel : UserViewModel
    {

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private bool _hasMicrosoftAccount;

        [ObservableProperty]
        private bool _hasGoogleAccount;

        [ObservableProperty]
        private string _googleAccountId;

        [ObservableProperty]
        private ImageSource _thumbnailImage;

        [ObservableProperty]
        private string _fileName;

        private FileResult _selectedFile;
        
        private Stream _resizedImageStream;

        private readonly IAuthenticationService _authenticationService;
        private readonly IExternalLoginService _externalLoginService;
        public UserProfileViewModel(IAuthenticationService authenticationService, IExternalLoginService externalLoginService) : base(authenticationService)
        {
            _authenticationService = authenticationService;
            _externalLoginService = externalLoginService;
        }

        [RelayCommand]
        public async Task InitializeProfileAsync()
        {
            await LoadUserProfileAsync();
        }

        private async Task LoadUserProfileAsync()
        {
            IsBusy = true;
            await InitializeAccountAsync();
            CheckExternalAccounts();
            IsBusy = false;
        }

        private void CheckExternalAccounts()
        {
            if (User.ExternalAccounts != null)
            {
                HasMicrosoftAccount = User.ExternalAccounts.Any(x => x.LoginProvider == "Microsoft");
                HasGoogleAccount = User.ExternalAccounts.Any(x => x.LoginProvider == "Google");

                if (HasGoogleAccount)
                {
                    var googleAccount = User.ExternalAccounts.FirstOrDefault(x => x.LoginProvider == "Google");
                    if (googleAccount != null)
                    {
                        GoogleAccountId = googleAccount.ProviderKey;
                    }
                }
            }
        }

        [RelayCommand]
        public async Task PickImageAsync()
        {
            try
            {
                var file = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Profile Image",
                    FileTypes = FilePickerFileType.Images
                });

                if (file != null)
                {
                    _selectedFile = file;
                    FileName = file.FileName;

                    using var originalStream = await file.OpenReadAsync();
                    var thumbnailStream = MauiServiceHelper.ResizeImage(originalStream, 250, 250);
                    thumbnailStream.Position = 0;

                    ThumbnailImage = ImageSource.FromStream(() =>
                    {
                        var copy = new MemoryStream();
                        thumbnailStream.CopyTo(copy);
                        copy.Position = 0;
                        thumbnailStream.Position = 0;
                        return copy;
                    });

                    WeakReferenceMessenger.Default.Send(new SendProfilePictureMessage(ThumbnailImage));
                    _resizedImageStream = thumbnailStream;
                }
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("Failed to pick image.", typeof(UserProfileViewModel)));
            }
        }

        [RelayCommand]
        public void RemoveProfileImage()
        {
            if (ThumbnailImage != null)
            {
                ThumbnailImage = ImageSource.FromUri(new Uri(Constants.NoImageUri));
                WeakReferenceMessenger.Default.Send(new SendProfilePictureMessage(ThumbnailImage));
                _selectedFile = null;
                FileName = null;
                _resizedImageStream = null;
            }
        }

        [RelayCommand]
        public async Task UploadImageAsync()
        {
            if (_resizedImageStream != null && !string.IsNullOrWhiteSpace(FileName))
            {
                IsBusy = true;
                var result = await _authenticationService.TryUploadProfilePictureAsync(_resizedImageStream, FileName);
                if (result.IsSuccess)
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage("Profile picture updated successfully.", typeof(UserProfileViewModel)));
                    ResetImage();
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(UserProfileViewModel)));
                }
                IsBusy = false;
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage("No image selected.", typeof(UserProfileViewModel)));
            }
        }

        [RelayCommand]
        public async Task MicrosoftLinkAsync()
        {
            try
            {
                var result = await _externalLoginService.LinkAsync(ExternalLoginType.Microsoft);
                if (result.IsSuccess)
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage("Successfully linked Microsoft Account.", typeof(UserProfileViewModel)));
                    await LoadUserProfileAsync();
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(UserProfileViewModel)));
                }
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage($"Link failed: {ex.Message}", typeof(UserProfileViewModel)));
            }
        }

        [RelayCommand]
        public async Task RemoveMicrosoftLinkAsync()
        {
            try
            {
                var result = await _externalLoginService.UnlinkAsync(ExternalLoginType.Microsoft);
                if (result.IsSuccess)
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage("Successfully unlinked Microsoft Account.", typeof(UserProfileViewModel)));
                    await LoadUserProfileAsync();
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(UserProfileViewModel)));
                }
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage($"Link failed: {ex.Message}", typeof(UserProfileViewModel)));
            }
        }

        [RelayCommand]
        public async Task GoogleLinkAsync()
        {
            try
            {
                var result = await _externalLoginService.LinkAsync(ExternalLoginType.Google);
                if (result.IsSuccess)
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage("Successfully linked Google Account.", typeof(UserProfileViewModel)));
                    await LoadUserProfileAsync();
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(UserProfileViewModel)));
                }
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage($"Unlink failed: {ex.Message}", typeof(UserProfileViewModel)));
            }
        }

        [RelayCommand]
        public async Task RemoveGoogleLinkAsync()
        {
            try
            {
                var result = await _externalLoginService.UnlinkAsync(ExternalLoginType.Google);
                if (result.IsSuccess)
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage("Successfully unlinked Google Account.", typeof(UserProfileViewModel)));
                    await LoadUserProfileAsync();
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new SendToastrMessage(result.Message, typeof(UserProfileViewModel)));
                }
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new SendToastrMessage($"Unlink failed: {ex.Message}", typeof(UserProfileViewModel)));
            }
        }

        [RelayCommand]
        public void Logout()
        {
            IsBusy = true;
            _authenticationService.SignOut();
            Application.Current.MainPage = new AuthShell();
            WeakReferenceMessenger.Default.Send(new SendToastrMessage("You have been logged out.", typeof(SignInViewModel)));
            IsBusy = false;
        }

        private void ResetImage()
        {
            ThumbnailImage = null;
            FileName = null;
            _selectedFile = null;
            _resizedImageStream = null;
        }

    }
}
