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
        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="value">绑定源生成的值</param>
        /// <param name="targetType">绑定目标属性的类型</param>
        /// <param name="parameter">要使用的转换器参数</param>
        /// <param name="culture">要用在转换器中的区域性</param>
        /// <returns></returns>
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return (value == null || String.IsNullOrEmpty(value.ToString())) ? EmptyValue : Visibility.Visible;
        }

        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="value">绑定目标生成的值</param>
        /// <param name="targetType">要转换为的类型</param>
        /// <param name="parameter">要使用的转换器参数</param>
        /// <param name="culture">要用在转换器中的区域性</param>
        /// <returns></returns>
        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        /// <summary>
        /// 空字符串时的Visibility
        /// </summary>
        public Visibility EmptyValue { get; set; } = Visibility.Collapsed;

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
    }
}
