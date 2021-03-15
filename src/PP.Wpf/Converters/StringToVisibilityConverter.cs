using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PP.Wpf.Converters
{
    /// <summary>
    /// StringToVisibilityConverter
    /// </summary>
    public sealed class StringToVisibilityConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return (value == null || String.IsNullOrEmpty(value.ToString())) ? EmptyValue : NotEmptyValue;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        /// <summary>
        /// 空字符串时的Visibility
        /// </summary>
        public Visibility EmptyValue { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// 非空字符串时的Visibility
        /// </summary>
        public Visibility NotEmptyValue { get; set; } = Visibility.Visible;

        private static readonly Lazy<StringToVisibilityConverter> emptyToCollapsed = new Lazy<StringToVisibilityConverter>();
        /// <summary>
        /// 空字符串时设置Visibility=Collapsed
        /// </summary>
        public static StringToVisibilityConverter EmptyToCollapsed => emptyToCollapsed.Value;


        private static readonly Lazy<StringToVisibilityConverter> emptyToHidden = new Lazy<StringToVisibilityConverter>(() => new StringToVisibilityConverter { EmptyValue = Visibility.Hidden });
        /// <summary>
        /// 空字符串时设置Visibility=Hidden
        /// </summary>
        public static StringToVisibilityConverter EmptyToHidden => emptyToHidden.Value;


        private static readonly Lazy<StringToVisibilityConverter> emptyToVisible = new Lazy<StringToVisibilityConverter>(() => new StringToVisibilityConverter { EmptyValue = Visibility.Visible, NotEmptyValue = Visibility.Collapsed });
        /// <summary>
        /// 空字符串时设置Visibility=Visible
        /// </summary>
        public static StringToVisibilityConverter EmptyToVisible => emptyToVisible.Value;
    }
}
