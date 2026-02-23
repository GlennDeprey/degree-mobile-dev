using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.Models.Products;
using Mde.Project.Mobile.ViewModels.App.Products;

namespace Mde.Project.Mobile.Pages.App.Products;

public partial class CategoryDeletePage : SfContentPage
{
    private readonly CategoryDeleteViewModel _categoryDeleteViewModel;
    protected override Type ViewModelType { get; set; } = typeof(CategoryDeleteViewModel);
    public CategoryDisplayModel Category { get; set; }
    public CategoryDeletePage(CategoryDeleteViewModel categoryDeleteViewModel)
	{
        _categoryDeleteViewModel = categoryDeleteViewModel;
        BindingContext = _categoryDeleteViewModel;
        InitializeComponent();
	}
}