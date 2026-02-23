using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Products;

namespace Mde.Project.Mobile.Pages.App.Products;

public partial class CategoryUpsertPage : SfContentPage
{
	private readonly CategoryUpsertViewModel _categoryUpsertViewModel;
    protected override Type ViewModelType { get; set; } = typeof(CategoryUpsertViewModel);
    public CategoryUpsertPage(CategoryUpsertViewModel categoryUpsertViewModel)
	{
        _categoryUpsertViewModel = categoryUpsertViewModel;
        BindingContext = _categoryUpsertViewModel;
        InitializeComponent();
	}
}