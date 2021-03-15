using PP.Wpf.Extensions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PP.Wpf.Converters
{
    /// <summary>
    /// BooleanToVisibilityConverter
    /// </summary>
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return value.ChangeType<Boolean>() ? TrueValue : FalseValue;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        #region Properties

        /// <summary>
        /// False时Visibility值
        /// </summary>
        public Visibility FalseValue { get; set; } = Visibility.Collapsed;
        /// <summary>
        /// True时Visibility值
        /// </summary>
        public Visibility TrueValue { get; set; }


        private static readonly Lazy<BooleanToVisibilityConverter> falseToCollapsed = new Lazy<BooleanToVisibilityConverter>();
        /// <summary>
        /// False时Visibility=Collapsed
        /// </summary>
        public static BooleanToVisibilityConverter FalseToCollapsed => falseToCollapsed.Value;


        private static readonly Lazy<BooleanToVisibilityConverter> falseToHidden = new Lazy<BooleanToVisibilityConverter>(() => new BooleanToVisibilityConverter { FalseValue = Visibility.Hidden });
        /// <summary>
        /// False时Visibility=Hidden
        /// </summary>
        public static BooleanToVisibilityConverter FalseToHidden => falseToHidden.Value;


        private static readonly Lazy<BooleanToVisibilityConverter> trueToCollapsed = new Lazy<BooleanToVisibilityConverter>(() => new BooleanToVisibilityConverter { FalseValue = Visibility.Visible, TrueValue = Visibility.Collapsed });
        /// <summary>
        /// True时Visibility=Collapsed
        /// </summary>
        public static BooleanToVisibilityConverter TrueToCollapsed => trueToCollapsed.Value;


        private static readonly Lazy<BooleanToVisibilityConverter> trueToHidden = new Lazy<BooleanToVisibilityConverter>(() => new BooleanToVisibilityConverter { FalseValue = Visibility.Visible, TrueValue = Visibility.Hidden });
        /// <summary>
        /// True时Visibility=Hidden
        /// </summary>
        public static BooleanToVisibilityConverter TrueToHidden => trueToHidden.Value;

        #endregion
    }
}
