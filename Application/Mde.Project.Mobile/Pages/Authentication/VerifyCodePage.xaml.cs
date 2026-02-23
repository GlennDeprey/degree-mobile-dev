using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.Authentication;

namespace Mde.Project.Mobile.Pages.Authentication;

public partial class VerifyCodePage : SfContentPage
{
    private readonly VerifyCodeViewModel _verifyCodeViewModel;
    protected override Type ViewModelType { get; set; } = typeof(VerifyCodeViewModel);
    public VerifyCodePage(VerifyCodeViewModel verifyCodeViewModel)
	{
        _verifyCodeViewModel = verifyCodeViewModel;
        BindingContext = verifyCodeViewModel;
        InitializeComponent();
    }

    private void OnCodeEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (entry.Text.Length == 1)
        {
            switch (entry)
            {
                case var _ when entry == CodeEntry1:
                    CodeEntry2.Focus();
                    break;
                case var _ when entry == CodeEntry2:
                    CodeEntry3.Focus();
                    break;
                case var _ when entry == CodeEntry3:
                    CodeEntry4.Focus();
                    break;
                case var _ when entry == CodeEntry4:
                    CodeEntry5.Focus();
                    break;
                case var _ when entry == CodeEntry5:
                    CodeEntry6.Focus();
                    break;
            }
        }
        else if (entry.Text.Length == 0)
        {
            switch (entry)
            {
                case var _ when entry == CodeEntry2:
                    CodeEntry1.Focus();
                    break;
                case var _ when entry == CodeEntry3:
                    CodeEntry2.Focus();
                    break;
                case var _ when entry == CodeEntry4:
                    CodeEntry3.Focus();
                    break;
                case var _ when entry == CodeEntry5:
                    CodeEntry4.Focus();
                    break;
                case var _ when entry == CodeEntry6:
                    CodeEntry5.Focus();
                    break;
            }
        }
    }
}