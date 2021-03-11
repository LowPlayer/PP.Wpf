using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
        /// 鼠标悬浮时拖动块宽度（横向时指高度）
        /// </summary>
        public static readonly DependencyProperty HoverThumbWidthProperty = DependencyProperty.RegisterAttached("HoverThumbWidth", typeof(Double), typeof(ScrollBarElement), new PropertyMetadata(0d, new PropertyChangedCallback(OnHoverThumbWidthPropertyChanged)));

        private static void OnHoverThumbWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollBar = d as ScrollBar;

            if (scrollBar == null)
                return;

            scrollBar.MouseEnter -= OnMouseEnter;
            scrollBar.MouseEnter += OnMouseEnter;
            scrollBar.MouseLeave -= OnMouseLeave;
            scrollBar.MouseLeave += OnMouseLeave;

            void OnMouseEnter(object sender, MouseEventArgs args)
            {
                var bar = (ScrollBar)sender;
                var val = GetHoverThumbWidth(bar);

                var ani = new DoubleAnimation
                {
                    To = val,
                    Duration = TimeSpan.FromSeconds(0.1)
                };

                if (bar.Orientation == System.Windows.Controls.Orientation.Vertical)
                {
                    ani.From = bar.ActualWidth;
                    bar.BeginAnimation(FrameworkElement.WidthProperty, ani);
                }
                else
                {
                    ani.From = bar.ActualHeight;
                    bar.BeginAnimation(FrameworkElement.HeightProperty, ani);
                }
            }

            void OnMouseLeave(object sender, MouseEventArgs args)
            {
                var bar = (ScrollBar)sender;

                if (bar.Orientation == System.Windows.Controls.Orientation.Vertical)
                    bar.BeginAnimation(FrameworkElement.WidthProperty, null);
                else
                    bar.BeginAnimation(FrameworkElement.HeightProperty, null);
            }
        }

        /// <summary>
        /// 获取鼠标悬浮时拖动块宽度（横向时指高度）
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Double GetHoverThumbWidth(DependencyObject element) => (Double)element.GetValue(HoverThumbWidthProperty);
        /// <summary>
        /// 设置鼠标悬浮时拖动块宽度（横向时指高度）
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetHoverThumbWidth(DependencyObject element, Double value) => element.SetValue(HoverThumbWidthProperty, value);



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



        /// <summary>
        /// 鼠标悬浮时拖动块圆角大小
        /// </summary>
        public static readonly DependencyProperty HoverThumbRadiusProperty = DependencyProperty.RegisterAttached("HoverThumbRadius", typeof(Double), typeof(ScrollBarElement));
        /// <summary>
        /// 获取鼠标悬浮时拖动块圆角大小
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Double GetHoverThumbRadius(DependencyObject element) => (Double)element.GetValue(HoverThumbRadiusProperty);
        /// <summary>
        /// 设置鼠标悬浮时拖动块圆角大小
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetHoverThumbRadius(DependencyObject element, Double value) => element.SetValue(HoverThumbRadiusProperty, value);



        /// <summary>
        /// 拖动块最小高度（横向时指宽度）
        /// </summary>
        public static readonly DependencyProperty ThumbMinLengthProperty = DependencyProperty.RegisterAttached("ThumbMinLength", typeof(Double), typeof(ScrollBarElement), new PropertyMetadata(new PropertyChangedCallback(OnThumbMinLengthPropertyChanged)));

        private static void OnThumbMinLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;

            if (scrollViewer == null)
                return;

            scrollViewer.ScrollChanged -= OnScrollChanged;
            scrollViewer.ScrollChanged += OnScrollChanged;

            void OnScrollChanged(object sender, ScrollChangedEventArgs args)
            {
                var sv = (ScrollViewer)sender;

                if ((args.ViewportHeightChange != 0 || args.ExtentHeightChange != 0))
                {
                    if (sv.Template.FindName("PART_VerticalScrollBar", sv) is ScrollBar bar)
                    {
                        if (bar.Template.FindName("PART_Track", bar) is Track track)
                        {
                            var val = GetThumbMinLength(sv);
                            var thumb = track.Thumb;

                            track.ViewportSize = track.ActualHeight;
                            thumb.Height = Double.NaN;

                            thumb.SizeChanged -= OnVerticalThumbSizeChanged;
                            thumb.SizeChanged += OnVerticalThumbSizeChanged;

                            void OnVerticalThumbSizeChanged(object sender1, SizeChangedEventArgs args1)
                            {
                                thumb.SizeChanged -= OnVerticalThumbSizeChanged;

                                if (thumb.ActualHeight < val)
                                {
                                    track.ViewportSize = Double.NaN;
                                    thumb.Height = val;
                                }
                            }
                        }
                    }
                }
                if (args.ViewportWidthChange != 0 || args.ExtentWidthChange != 0)
                {
                    if (sv.Template.FindName("PART_HorizontalScrollBar", sv) is ScrollBar bar)
                    {
                        if (bar.Template.FindName("PART_Track", bar) is Track track)
                        {
                            var val = GetThumbMinLength(sv);
                            var thumb = track.Thumb;

                            track.ViewportSize = track.ActualWidth;
                            thumb.Width = Double.NaN;

                            thumb.SizeChanged -= OnHorizontalThumbSizeChanged;
                            thumb.SizeChanged += OnHorizontalThumbSizeChanged;

                            void OnHorizontalThumbSizeChanged(object sender1, SizeChangedEventArgs args1)
                            {
                                thumb.SizeChanged -= OnHorizontalThumbSizeChanged;

                                if (thumb.ActualWidth < val)
                                {
                                    track.ViewportSize = Double.NaN;
                                    thumb.Width = val;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取拖动块最小高度（横向时指宽度）
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Double GetThumbMinLength(DependencyObject element) => (Double)element.GetValue(ThumbMinLengthProperty);
        /// <summary>
        /// 设置拖动块最小高度（横向时指宽度）
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetThumbMinLength(DependencyObject element, Double value) => element.SetValue(ThumbMinLengthProperty, value);
    }
}
