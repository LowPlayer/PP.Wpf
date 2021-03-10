using System;
using System.Windows;
using System.Windows.Media;

namespace PP.Wpf.Controls.Attach
{
    /// <summary>
    /// 滚动条元素
    /// </summary>
    public static class ScrollBarElement
    {
        /// <summary>
        /// 滚动条背景颜色
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached("Background", typeof(Brush), typeof(ScrollBarElement));
        /// <summary>
        /// 获取滚动条背景颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetBackground(DependencyObject element) => (Brush)element.GetValue(BackgroundProperty);
        /// <summary>
        /// 设置滚动条背景颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetBackground(DependencyObject element, Brush value) => element.SetValue(BackgroundProperty, value);



        /// <summary>
        /// 拖动块填充颜色
        /// </summary>
        public static readonly DependencyProperty ThumbFillProperty = DependencyProperty.RegisterAttached("ThumbFill", typeof(Brush), typeof(ScrollBarElement));
        /// <summary>
        /// 获取拖动块填充颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetThumbFill(DependencyObject element) => (Brush)element.GetValue(ThumbFillProperty);
        /// <summary>
        /// 设置拖动块填充颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetThumbFill(DependencyObject element, Brush value) => element.SetValue(ThumbFillProperty, value);



        /// <summary>
        /// 拖动块鼠标悬浮时填充颜色
        /// </summary>
        public static readonly DependencyProperty ThumbHoverFillProperty = DependencyProperty.RegisterAttached("ThumbHoverFill", typeof(Brush), typeof(ScrollBarElement));
        /// <summary>
        /// 获取拖动块鼠标悬浮时填充颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetThumbHoverFill(DependencyObject element) => (Brush)element.GetValue(ThumbHoverFillProperty);
        /// <summary>
        /// 设置拖动块鼠标悬浮时填充颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetThumbHoverFill(DependencyObject element, Brush value) => element.SetValue(ThumbHoverFillProperty, value);



        /// <summary>
        /// 拖动块鼠标按压时填充颜色
        /// </summary>
        public static readonly DependencyProperty ThumbPressFillProperty = DependencyProperty.RegisterAttached("ThumbPressFill", typeof(Brush), typeof(ScrollBarElement));
        /// <summary>
        /// 获取拖动块鼠标按压时填充颜色
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetThumbPressFill(DependencyObject element) => (Brush)element.GetValue(ThumbPressFillProperty);
        /// <summary>
        /// 设置拖动块鼠标按压时填充颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetThumbPressFill(DependencyObject element, Brush value) => element.SetValue(ThumbPressFillProperty, value);



        /// <summary>
        /// 拖动块宽度（横向时指高度）
        /// </summary>
        public static readonly DependencyProperty ThumbWidthProperty = DependencyProperty.RegisterAttached("ThumbWidth", typeof(Double), typeof(ScrollBarElement), new PropertyMetadata(12d));
        /// <summary>
        /// 获取拖动块宽度（横向时指高度）
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Double GetThumbWidth(DependencyObject element) => (Double)element.GetValue(ThumbWidthProperty);
        /// <summary>
        /// 设置拖动块宽度（横向时指高度）
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetThumbWidth(DependencyObject element, Double value) => element.SetValue(ThumbWidthProperty, value);



        /// <summary>
        /// 拖动块圆角大小
        /// </summary>
        public static readonly DependencyProperty ThumbRadiusProperty = DependencyProperty.RegisterAttached("ThumbRadius", typeof(Double), typeof(ScrollBarElement));
        /// <summary>
        /// 获取拖动块圆角大小
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Double GetThumbRadius(DependencyObject element) => (Double)element.GetValue(ThumbRadiusProperty);
        /// <summary>
        /// 设置拖动块圆角大小
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetThumbRadius(DependencyObject element, Double value) => element.SetValue(ThumbRadiusProperty, value);
    }
}
