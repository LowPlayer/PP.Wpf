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
        /// 鼠标悬浮时背景颜色
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached("Background", typeof(Brush), typeof(BackgroundElement));
        /// <summary>
        /// 获取鼠标悬浮时背景颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetBackground(DependencyObject element) => (Brush)element.GetValue(BackgroundProperty);
        /// <summary>
        /// 设置鼠标悬浮时背景颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetBackground(DependencyObject element, Brush value) => element.SetValue(BackgroundProperty, value);



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



        /// <summary>
        /// 鼠标按压时背景颜色
        /// </summary>
        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.RegisterAttached("PressedBackground", typeof(Brush), typeof(BackgroundElement));
        /// <summary>
        /// 获取鼠标按压时背景颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetPressedBackground(DependencyObject element) => (Brush)element.GetValue(PressedBackgroundProperty);
        /// <summary>
        /// 设置鼠标按压时背景颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetPressedBackground(DependencyObject element, Brush value) => element.SetValue(PressedBackgroundProperty, value);



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
        /// 聚焦时背景颜色
        /// </summary>
        public static readonly DependencyProperty FocusBackgroundProperty = DependencyProperty.RegisterAttached("FocusBackground", typeof(Brush), typeof(BackgroundElement));
        /// <summary>
        /// 获取聚焦时背景颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetFocusBackground(DependencyObject element) => (Brush)element.GetValue(FocusBackgroundProperty);
        /// <summary>
        /// 设置聚焦时背景颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetFocusBackground(DependencyObject element, Brush value) => element.SetValue(FocusBackgroundProperty, value);
    }
}
