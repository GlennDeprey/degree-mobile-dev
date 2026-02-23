using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.Messages.Scanner;
using Mde.Project.Mobile.ViewModels.App.Scanner;

namespace Mde.Project.Mobile.Pages.App.Scanner;

public partial class ScannerPage : SfContentPage
{
    private readonly ScannerViewModel _scannerViewModel;
    protected override Type ViewModelType { get; set; } = typeof(ScannerViewModel);
    public ScannerPage(ScannerViewModel scannerViewModel)
    {
        _scannerViewModel = scannerViewModel;
        BindingContext = _scannerViewModel;
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<SendBarcodeMessage>(this, async (r, m) =>
        {
            await _scannerViewModel.ScanProductCommand.ExecuteAsync(m.Barcode);
            await _scannerViewModel.BasketNavigationCommand.ExecuteAsync(null);
        });
    }
}