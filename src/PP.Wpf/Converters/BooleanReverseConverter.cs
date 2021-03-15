using System;
using System.Globalization;
using System.Windows.Data;

namespace PP.Wpf.Converters
{
    /// <summary>
    /// BooleanReverseConverter
    /// </summary>
    public sealed class BooleanReverseConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return !(Boolean)value;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return !(Boolean)value;
        }

        private static readonly Lazy<BooleanReverseConverter> instance = new Lazy<BooleanReverseConverter>();
        /// <summary>
        /// 反转Boolean实例
        /// </summary>
        public static BooleanReverseConverter Instance => instance.Value;
    }
}
