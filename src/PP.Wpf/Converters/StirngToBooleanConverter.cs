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
        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="value">绑定源生成的值</param>
        /// <param name="targetType">绑定目标属性的类型</param>
        /// <param name="parameter">要使用的转换器参数</param>
        /// <param name="culture">要用在转换器中的区域性</param>
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return String.IsNullOrEmpty(value?.ToString());
        }

        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="value">绑定目标生成的值</param>
        /// <param name="targetType">要转换为的类型</param>
        /// <param name="parameter">要使用的转换器参数</param>
        /// <param name="culture">要用在转换器中的区域性</param>
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
