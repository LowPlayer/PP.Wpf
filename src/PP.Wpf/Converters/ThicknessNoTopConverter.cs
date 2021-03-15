using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PP.Wpf.Converters
{
    /// <summary>
    /// ThicknessNoTopConverter
    /// </summary>
    public sealed class ThicknessNoTopConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var t = (Thickness)value;
            return new Thickness(t.Left, 0, t.Right, t.Bottom);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
