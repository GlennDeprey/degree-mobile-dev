using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.Authentication;

namespace Mde.Project.Mobile.Pages.Authentication;

public partial class SignInPage : SfContentPage
{
    private SignInViewModel _signInViewModel;
    protected override Type ViewModelType { get; set; } = typeof(SignInViewModel);
    public SignInPage(SignInViewModel signInViewModel)
	{
        _signInViewModel = signInViewModel;
        BindingContext = signInViewModel;
        InitializeComponent();
    }
}