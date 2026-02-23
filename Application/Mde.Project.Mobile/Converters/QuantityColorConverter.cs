using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Converters
{
    public class QuantityColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int quantity)
            {
                if (quantity < 20)
                {
                    return Color.FromArgb("#FF4C4C");
                }
                    
                if (quantity < 50)
                {
                    return Color.FromArgb("#FFA500");
                }
                    
                return Color.FromArgb("#64783E");
            }
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

    public class QuantityToStrokeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int quantity)
            {
                if (quantity < 20)
                {
                    return Color.FromArgb("#CC0000");
                }
                if (quantity < 50)
                {
                    return Color.FromArgb("#CC8400");
                }

                return Color.FromArgb("#64783E");
            }
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
