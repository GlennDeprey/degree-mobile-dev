using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.Messages.Users;
using Mde.Project.Mobile.ViewModels.App.Users;

namespace Mde.Project.Mobile.Pages.App.Users;

public partial class UserProfilePage : SfContentPage
{
    private readonly UserProfileViewModel _profileViewModel;
    protected override Type ViewModelType { get; set; } = typeof(UserProfileViewModel);

    public UserProfilePage(UserProfileViewModel profileViewModel)
	{
        _profileViewModel = profileViewModel;
        BindingContext = _profileViewModel;
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<SendProfilePictureMessage>(this, (r, m) =>
        {
            UpdateProfilePicture(m.NewImage);
        });
    }

    private void UpdateProfilePicture(ImageSource image)
    {
        if (image != null)
        {
            ProfilePicture.Source = image;
        }
    }
}