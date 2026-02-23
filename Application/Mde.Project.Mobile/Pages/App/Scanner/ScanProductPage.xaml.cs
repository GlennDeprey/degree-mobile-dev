using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Messages.Scanner;

namespace Mde.Project.Mobile.Pages.App.Scanner;

public partial class ScanProductPage : ContentPage
{
    public ScanProductPage()
    {
        InitializeComponent();
        CameraBarcodeReaderView.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormat.Ean13,
            AutoRotate = true,
            Multiple = false
        };
    }

    protected void BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var first = e.Results?.FirstOrDefault();
        if (first is null)
        {
            return;
        }

        WeakReferenceMessenger.Default.Send(new SendBarcodeMessage(first.Value));
    }

    private void Return_OnPressed(object sender, EventArgs e)
    {
        WeakReferenceMessenger.Default.Send(new SendReturnBasketMessage());
    }
}