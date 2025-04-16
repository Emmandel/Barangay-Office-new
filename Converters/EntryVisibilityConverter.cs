using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barangay_Office.ViewModels;

namespace Barangay_Office.Converters
{
    public class EntryVisibilityConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is Dictionary<string, bool> visibilityDictionary && parameter is string entryName)
            {
                return visibilityDictionary.TryGetValue(entryName, out var IsVisible) && IsVisible;
            }
            return false;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
