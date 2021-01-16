﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PP.Wpf.Converters
{
    /// <summary>
    /// NullToVisibilityConverter
    /// </summary>
    public sealed class NullToVisibilityConverter : IValueConverter
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
            return value == null ? NullValue : Visibility.Visible;
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

        #region Properties

        private static readonly Lazy<NullToVisibilityConverter> nullToCollapsed = new Lazy<NullToVisibilityConverter>();
        /// <summary>
        /// Null时Visibility=Collapsed
        /// </summary>
        public static NullToVisibilityConverter NullToCollapsed => nullToCollapsed.Value;

        private static readonly Lazy<NullToVisibilityConverter> nullToHidden = new Lazy<NullToVisibilityConverter>(() => new NullToVisibilityConverter { NullValue = Visibility.Hidden });
        /// <summary>
        /// Null时Visibility=Hidden
        /// </summary>
        public static NullToVisibilityConverter NullToHidden => nullToHidden.Value;

        private Visibility nullValue = Visibility.Collapsed;
        /// <summary>
        /// Null时Visibility值
        /// </summary>
        public Visibility NullValue
        {
            get { return nullValue; }
            set { nullValue = value; }
        }

        #endregion
    }
}
