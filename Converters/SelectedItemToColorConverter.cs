using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barangay_Office.Controls;
using CommunityToolkit.Maui.Converters;

namespace Barangay_Office.Converters
{
    public class SelectedItemToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string currentLanguage && parameter is LanguageDropdown dropdown)
            {
                var highlight = Application.Current?.Resources["Gray500"];
                return currentLanguage == dropdown.SelectedLanguage && highlight != null ? highlight : Colors.Transparent;
            }
            return Colors.Transparent; // Default Color
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
