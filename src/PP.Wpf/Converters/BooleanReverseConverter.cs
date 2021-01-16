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
            return !(Boolean)value;
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
            return !(Boolean)value;
        }

        private static readonly Lazy<BooleanReverseConverter> instance = new Lazy<BooleanReverseConverter>();
        /// <summary>
        /// 反转Boolean实例
        /// </summary>
        public static BooleanReverseConverter Instance => instance.Value;
    }
}
