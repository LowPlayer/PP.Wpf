using System.Windows;
using System.Windows.Media;

namespace PP.Wpf.Controls.Attach
{
    /// <summary>
    /// 边框元素
    /// </summary>
    public static class BorderElement
    {
        /// <summary>
        /// 鼠标悬停时边框颜色
        /// </summary>
        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.RegisterAttached("BorderBrush", typeof(Brush), typeof(BorderElement));
        /// <summary>
        /// 获取鼠标悬停时边框颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetBorderBrush(DependencyObject element) => (Brush)element.GetValue(BorderBrushProperty);
        /// <summary>
        /// 设置鼠标悬停时边框颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetBorderBrush(DependencyObject element, Brush value) => element.SetValue(BorderBrushProperty, value);



        /// <summary>
        /// 鼠标悬停时边框颜色
        /// </summary>
        public static readonly DependencyProperty HoverBorderBrushProperty = DependencyProperty.RegisterAttached("HoverBorderBrush", typeof(Brush), typeof(BorderElement));
        /// <summary>
        /// 获取鼠标悬停时边框颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetHoverBorderBrush(DependencyObject element) => (Brush)element.GetValue(HoverBorderBrushProperty);
        /// <summary>
        /// 设置鼠标悬停时边框颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetHoverBorderBrush(DependencyObject element, Brush value) => element.SetValue(HoverBorderBrushProperty, value);



        /// <summary>
        /// 鼠标按压时边框颜色
        /// </summary>
        public static readonly DependencyProperty PressedBorderBrushProperty = DependencyProperty.RegisterAttached("PressedBorderBrush", typeof(Brush), typeof(BorderElement));
        /// <summary>
        /// 获取鼠标按压时边框颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetPressedBorderBrush(DependencyObject element) => (Brush)element.GetValue(PressedBorderBrushProperty);
        /// <summary>
        /// 设置鼠标按压时边框颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetPressedBorderBrush(DependencyObject element, Brush value) => element.SetValue(PressedBorderBrushProperty, value);



        /// <summary>
        /// 选中时边框颜色
        /// </summary>
        public static readonly DependencyProperty SelectedBorderBrushProperty = DependencyProperty.RegisterAttached("SelectedBorderBrush", typeof(Brush), typeof(BorderElement));
        /// <summary>
        /// 获取选中时边框颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetSelectedBorderBrush(DependencyObject element) => (Brush)element.GetValue(SelectedBorderBrushProperty);
        /// <summary>
        /// 设置选中时边框颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetSelectedBorderBrush(DependencyObject element, Brush value) => element.SetValue(SelectedBorderBrushProperty, value);



        /// <summary>
        /// 聚焦时边框颜色
        /// </summary>
        public static readonly DependencyProperty FocusBorderBrushProperty = DependencyProperty.RegisterAttached("FocusBorderBrush", typeof(Brush), typeof(BorderElement));
        /// <summary>
        /// 聚焦时边框颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetFocusBorderBrush(DependencyObject element) => (Brush)element.GetValue(FocusBorderBrushProperty);
        /// <summary>
        /// 聚焦时边框颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetFocusBorderBrush(DependencyObject element, Brush value) => element.SetValue(FocusBorderBrushProperty, value);



        /// <summary>
        /// 圆角弧度
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(BorderElement));
        /// <summary>
        /// 获取圆角弧度
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static CornerRadius GetCornerRadius(DependencyObject element) => (CornerRadius)element.GetValue(CornerRadiusProperty);
        /// <summary>
        /// 设置圆角弧度
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetCornerRadius(DependencyObject element, CornerRadius value) => element.SetValue(CornerRadiusProperty, value);
    }
}
