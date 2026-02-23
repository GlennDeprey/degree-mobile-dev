using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Converters
{
    public class QuantityChangeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int quantityChange)
            {
                return new FontImageSource
                {
                    FontFamily = "MaterialRegular",
                    Glyph = quantityChange >= 0 ? "\ue147" : "\ue15c",
                    Color = quantityChange >= 0 ? Colors.YellowGreen : Colors.IndianRed
                };
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
