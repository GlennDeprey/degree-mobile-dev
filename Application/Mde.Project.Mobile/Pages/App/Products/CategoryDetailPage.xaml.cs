using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Products;

namespace Mde.Project.Mobile.Pages.App.Products;

public partial class CategoryDetailPage : SfContentPage
{
	private readonly CategoryDetailViewModel _categoryDetailViewModel;
    protected override Type ViewModelType { get; set; } = typeof(CategoryDetailViewModel);
    public CategoryDetailPage(CategoryDetailViewModel categoryDetailViewModel)
	{
        _categoryDetailViewModel = categoryDetailViewModel;
        BindingContext = _categoryDetailViewModel;
        InitializeComponent();
	}
}