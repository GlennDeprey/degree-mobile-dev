using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.Messages.Products;
using Mde.Project.Mobile.ViewModels.App.Products;

namespace Mde.Project.Mobile.Pages.App.Products;

public partial class ProductUpsertPage : SfContentPage
{
	private readonly ProductUpsertViewModel _productUpsertViewModel;
    protected override Type ViewModelType { get; set; } = typeof(ProductUpsertViewModel);
    public ProductUpsertPage(ProductUpsertViewModel productUpsertViewModel)
	{
        _productUpsertViewModel = productUpsertViewModel;
        BindingContext = _productUpsertViewModel;
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<SendProductPictureMessage>(this, (r, m) =>
        {
            UpdateProductPicture(m.NewImage);
        });
    }

    private void UpdateProductPicture(ImageSource image)
    {
        if (image != null)
        {
            ProductPicture.Source = image;
        }
    }
}