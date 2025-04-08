using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barangay_Office.Converters
{
    public class NavigationWidthConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string selectedButton && parameter is string buttonName)
            {
                return selectedButton == buttonName ? 30 : 0; // Return 30 if selected, otherwise 0
            }
            return 0; // Default case
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
