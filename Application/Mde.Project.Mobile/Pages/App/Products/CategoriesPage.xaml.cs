using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Products;

namespace Mde.Project.Mobile.Pages.App.Products;

public partial class CategoriesPage : SfContentPage
{
    private readonly CategoriesViewModel _categoriesViewModel;
    protected override Type ViewModelType { get; set; } = typeof(CategoriesViewModel);
    public CategoriesPage(CategoriesViewModel categoriesViewModel)
	{
        _categoriesViewModel = categoriesViewModel;
        BindingContext = _categoriesViewModel;
        InitializeComponent();
    }

    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            await _categoriesViewModel.InitializeCategoriesCommand.ExecuteAsync(null);
        }
    }
}