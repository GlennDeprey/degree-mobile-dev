using CommunityToolkit.Mvvm.ComponentModel;

namespace Mde.Project.Mobile.ViewModels.App.Scanner
{
    public partial class ScannerItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private Guid _id;

        [ObservableProperty]
        private string _brand;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _imageSource;

        [ObservableProperty]
        private int _quantity;

        [ObservableProperty]
        private decimal _price;

        [ObservableProperty]
        private string _barcode;
    }
}
