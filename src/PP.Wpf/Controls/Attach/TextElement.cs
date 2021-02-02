using System;
using System.Windows;

namespace PP.Wpf.Controls.Attach
{
    /// <summary>
    /// 文本元素
    /// </summary>
    public static class TextElement
    {
        /// <summary>
        /// 占位符
        /// </summary>
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.RegisterAttached("Placeholder", typeof(String), typeof(TextElement));
        /// <summary>
        /// 获取占位符
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static String GetPlaceholder(DependencyObject element) => (String)element.GetValue(PlaceholderProperty);
        /// <summary>
        /// 设置占位符
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetPlaceholder(DependencyObject element, String value) => element.SetValue(PlaceholderProperty, value);
    }
}
