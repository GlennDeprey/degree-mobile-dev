using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.Authentication;

namespace Mde.Project.Mobile.Pages.Authentication;

public partial class SignUpPage : SfContentPage
{
    private readonly SignUpViewModel _signUpViewModel;
    protected override Type ViewModelType { get; set; } = typeof(SignUpViewModel);
    public SignUpPage(SignUpViewModel signUpViewModel)
	{
        _signUpViewModel = signUpViewModel;
        BindingContext = signUpViewModel;
        InitializeComponent();      
    }
}