using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.Authentication;


namespace Mde.Project.Mobile.Pages.Authentication;

public partial class ResetPasswordPage : SfContentPage
{
	private readonly ResetPasswordViewModel _resetPasswordViewModel;
    protected override Type ViewModelType { get; set; } = typeof(ResetPasswordViewModel);
    public ResetPasswordPage(ResetPasswordViewModel resetPasswordViewModel)
	{
        _resetPasswordViewModel = resetPasswordViewModel;
        BindingContext = resetPasswordViewModel;
        InitializeComponent();
	}
}