using System;
using System.Globalization;
using System.Windows.Data;

namespace PP.Wpf.Converters
{
    /// <summary>
    /// 是否空字符转换器
    /// </summary>
    public sealed class StirngToBooleanConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return String.IsNullOrEmpty(value?.ToString());
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }


        private static readonly Lazy<StirngToBooleanConverter> isNullOrEmpty = new Lazy<StirngToBooleanConverter>();
        /// <summary>
        /// 空字符串时设置Visibility=Collapsed
        /// </summary>
        public static StirngToBooleanConverter IsNullOrEmpty => isNullOrEmpty.Value;
    }
}
