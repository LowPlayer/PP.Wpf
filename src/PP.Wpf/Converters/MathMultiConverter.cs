using PP.Wpf.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PP.Wpf.Converters
{
    /// <summary>
    /// 数学运算转换器
    /// </summary>
    [ValueConversion(typeof(Double), typeof(Double))]
    public sealed class MathMultiConverter : IMultiValueConverter
    {
        /// <summary>
        /// 数学运算符
        /// </summary>
        public enum MathOperators
        {
            /// <summary>
            /// 加
            /// </summary>
            Add,
            /// <summary>
            /// 减
            /// </summary>
            Minus,
            /// <summary>
            /// 乘
            /// </summary>
            Multiply,
            /// <summary>
            /// 除
            /// </summary>
            Except,
            /// <summary>
            /// 比较
            /// </summary>
            Compare
        }

        public Object Convert(Object[] values, Type targetType, Object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                throw new ArgumentException("values应传入两个值");

            var a = values[0].ChangeType<Double>();
            var b = values[1].ChangeType<Double>();

            switch (MathOperator)
            {
                case MathOperators.Add:
                    return a + b;
                case MathOperators.Minus:
                    return a - b;
                case MathOperators.Multiply:
                    return a * b;
                case MathOperators.Except:
                    return a / b;
                default:
                    var r = a - b;
                    return r == 0 ? 0 : (r > 0 ? 1 : -1);
            }
        }

        public Object[] ConvertBack(Object value, Type[] targetTypes, Object parameter, CultureInfo culture)
        {
            return new Object[] { Binding.DoNothing, Binding.DoNothing };
        }

        /// <summary>
        /// 数学运算符
        /// </summary>
        public MathOperators MathOperator { get; set; }

        private static readonly Lazy<MathMultiConverter> compare = new Lazy<MathMultiConverter>(() => new MathMultiConverter { MathOperator = MathOperators.Compare });
        /// <summary>
        /// 反转Boolean实例
        /// </summary>
        public static MathMultiConverter Compare => compare.Value;
    }
}
