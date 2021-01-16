using System.Windows;
using System.Windows.Media;

namespace PP.Wpf.Controls.Attach
{
    /// <summary>
    /// 背景元素
    /// </summary>
    public static class BackgroundElement
    {
        /// <summary>
        /// 选中时背景颜色
        /// </summary>
        public static readonly DependencyProperty SelectedBackgroundProperty = DependencyProperty.RegisterAttached("SelectedBackground", typeof(Brush), typeof(BackgroundElement));
        /// <summary>
        /// 获取选中时背景颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetSelectedBackground(DependencyObject element) => (Brush)element.GetValue(SelectedBackgroundProperty);
        /// <summary>
        /// 设置选中时背景颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetSelectedBackground(DependencyObject element, Brush value) => element.SetValue(SelectedBackgroundProperty, value);



        /// <summary>
        /// 鼠标悬浮时背景颜色
        /// </summary>
        public static readonly DependencyProperty HoverBackgroundProperty = DependencyProperty.RegisterAttached("HoverBackground", typeof(Brush), typeof(BackgroundElement));
        /// <summary>
        /// 获取鼠标悬浮时背景颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetHoverBackground(DependencyObject element) => (Brush)element.GetValue(HoverBackgroundProperty);
        /// <summary>
        /// 设置鼠标悬浮时背景颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetHoverBackground(DependencyObject element, Brush value) => element.SetValue(HoverBackgroundProperty, value);
    }
}
