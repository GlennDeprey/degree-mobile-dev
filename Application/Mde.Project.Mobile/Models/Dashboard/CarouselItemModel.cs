using SkiaSharp.Extended.UI.Controls;

namespace Mde.Project.Mobile.Models.Dashboard
{
    public class CarouselItemModel
    {
        public object AnimationSource { get; set; }
        public Thickness Margin { get; set; }
        public double Scale { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string Description { get; set; }
    }
}
