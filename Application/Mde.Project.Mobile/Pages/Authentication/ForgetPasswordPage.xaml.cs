using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.Authentication;

namespace Mde.Project.Mobile.Pages.Authentication;

public partial class ForgetPasswordPage : SfContentPage
{
    private readonly ForgetPasswordViewModel _forgotPasswordViewModel;
    protected override Type ViewModelType { get; set; } = typeof(ForgetPasswordViewModel);
    public ForgetPasswordPage(ForgetPasswordViewModel forgotPasswordViewModel)
	{
        _forgotPasswordViewModel = forgotPasswordViewModel;
        BindingContext = forgotPasswordViewModel;
        InitializeComponent();
	}
}