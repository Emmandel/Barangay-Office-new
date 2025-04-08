using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barangay_Office.Models;

namespace Barangay_Office.Converters
{
    public class SenderToAlignmentConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string sender)
            {
                // If the sender is "Admin", align to the left, otherwise to the right
                return sender.Contains("Admin") ? LayoutOptions.Start : LayoutOptions.End;
            }
            return LayoutOptions.Start;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SenderToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string sender)
            {
                // Admin messages are light blue, customer messages are light green
                return sender.Contains("Admin") ? Color.FromArgb("#E3F2FD") : Color.FromArgb("#E8F5E9");
            }
            return Colors.LightGray;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
