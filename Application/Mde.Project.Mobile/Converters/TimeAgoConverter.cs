using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Converters
{
    public class TimeAgoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime createdOn)
            {
                var timeSpan = DateTime.Now - createdOn;

                if (timeSpan.TotalSeconds < 60)
                    return "Just Now";
                if (timeSpan.TotalMinutes < 60)
                    return $"{(int)timeSpan.TotalMinutes} minute{(timeSpan.TotalMinutes < 2 ? "" : "s")} ago";
                if (timeSpan.TotalHours < 24)
                    return $"{(int)timeSpan.TotalHours} hour{(timeSpan.TotalHours < 2 ? "" : "s")} ago";
                if (timeSpan.TotalDays < 7)
                    return $"{(int)timeSpan.TotalDays} day{(timeSpan.TotalDays < 2 ? "" : "s")} ago";

                return createdOn.ToString("MMM dd, yyyy", culture);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
