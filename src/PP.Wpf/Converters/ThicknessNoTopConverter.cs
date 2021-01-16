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
            var t = (Thickness)value;
            return new Thickness(t.Left, 0, t.Right, t.Bottom);
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
    }
}
