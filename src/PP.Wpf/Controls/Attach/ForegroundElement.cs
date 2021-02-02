using System.Windows;
using System.Windows.Media;

namespace PP.Wpf.Controls.Attach
{
    /// <summary>
    /// 字体元素
    /// </summary>
    public static class ForegroundElement
    {
        /// <summary>
        /// 鼠标悬浮时字体颜色
        /// </summary>
        public static readonly DependencyProperty HoverForegroundProperty = DependencyProperty.RegisterAttached("HoverForeground", typeof(Brush), typeof(ForegroundElement));
        /// <summary>
        /// 获取鼠标悬浮时字体颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetHoverForeground(DependencyObject element) => (Brush)element.GetValue(HoverForegroundProperty);
        /// <summary>
        /// 设置鼠标悬浮时字体颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetHoverForeground(DependencyObject element, Brush value) => element.SetValue(HoverForegroundProperty, value);



        /// <summary>
        /// 鼠标按下时字体颜色
        /// </summary>
        public static readonly DependencyProperty PressedForegroundProperty = DependencyProperty.RegisterAttached("PressedForeground", typeof(Brush), typeof(ForegroundElement));
        /// <summary>
        /// 获取鼠标按下时字体颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetPressedForeground(DependencyObject element) => (Brush)element.GetValue(PressedForegroundProperty);
        /// <summary>
        /// 设置鼠标按下时字体颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetPressedForeground(DependencyObject element, Brush value) => element.SetValue(PressedForegroundProperty, value);



        /// <summary>
        /// 选中时字体颜色
        /// </summary>
        public static readonly DependencyProperty SelectedForegroundProperty = DependencyProperty.RegisterAttached("SelectedForeground", typeof(Brush), typeof(ForegroundElement));
        /// <summary>
        /// 获取选中时字体颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetSelectedForeground(DependencyObject element) => (Brush)element.GetValue(SelectedForegroundProperty);
        /// <summary>
        /// 设置选中时字体颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetSelectedForeground(DependencyObject element, Brush value) => element.SetValue(SelectedForegroundProperty, value);
    }
}
