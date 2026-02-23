
namespace Mde.Project.Mobile.Controls
{
    public class SfToolBarItem : ToolbarItem
    {
        public static readonly BindableProperty IsVisibleProperty =
            BindableProperty.Create(
                nameof(IsVisible),
                typeof(bool),
                typeof(SfToolBarItem),
                true,
                BindingMode.TwoWay,
                propertyChanged: OnIsVisibleChanged);

        public bool IsVisible
        {
            get => (bool)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }

        private static void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfToolBarItem item)
            {
                item.IsVisible = (bool)newValue;
            }
        }
    }
}