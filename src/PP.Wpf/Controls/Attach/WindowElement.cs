using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace PP.Wpf.Controls.Attach
{
    /// <summary>
    /// 窗体元素
    /// </summary>
    public static class WindowElement
    {
        /// <summary>
        /// 标题栏高度
        /// </summary>
        public static readonly DependencyProperty TitleHeightProperty = DependencyProperty.RegisterAttached("TitleHeight", typeof(Double), typeof(WindowElement));
        /// <summary>
        /// 获取标题栏高度
        /// </summary>
        public static Double GetTitleHeight(DependencyObject element) => (Double)element.GetValue(TitleHeightProperty);
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetTitleHeight(DependencyObject element, Double value) => element.SetValue(TitleHeightProperty, value);



        /// <summary>
        /// 标题栏背景色
        /// </summary>
        public static readonly DependencyProperty TitleBackgroundProperty = DependencyProperty.RegisterAttached("TitleBackground", typeof(Brush), typeof(WindowElement));
        /// <summary>
        /// 获取标题栏背景色
        /// </summary>
        public static Brush GetTitleBackground(DependencyObject element) => (Brush)element.GetValue(TitleBackgroundProperty);
        /// <summary>
        /// 设置标题栏背景色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetTitleBackground(DependencyObject element, Brush value) => element.SetValue(TitleBackgroundProperty, value);



        /// <summary>
        /// 标题栏前景色
        /// </summary>
        public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.RegisterAttached("TitleForeground", typeof(Brush), typeof(WindowElement));
        /// <summary>
        /// 获取标题栏前景色
        /// </summary>
        public static Brush GetTitleForeground(DependencyObject element) => (Brush)element.GetValue(TitleForegroundProperty);
        /// <summary>
        /// 设置标题栏前景色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetTitleForeground(DependencyObject element, Brush value) => element.SetValue(TitleForegroundProperty, value);



        /// <summary>
        /// NoActive标题栏背景色
        /// </summary>
        public static readonly DependencyProperty NoActiveTitleBackgroundProperty = DependencyProperty.RegisterAttached("NoActiveTitleBackground", typeof(Brush), typeof(WindowElement));
        /// <summary>
        /// 获取NoActive标题栏背景色
        /// </summary>
        public static Brush GetNoActiveTitleBackground(DependencyObject element) => (Brush)element.GetValue(NoActiveTitleBackgroundProperty);
        /// <summary>
        /// 设置NoActive标题栏背景色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetNoActiveTitleBackground(DependencyObject element, Brush value) => element.SetValue(NoActiveTitleBackgroundProperty, value);



        /// <summary>
        /// NoActive边框颜色
        /// </summary>
        public static readonly DependencyProperty NoActiveBorderBrushProperty = DependencyProperty.RegisterAttached("NoActiveBorderBrush", typeof(Brush), typeof(WindowElement));
        /// <summary>
        /// 获取NoActive边框颜色
        /// </summary>
        public static Brush GetNoActiveBorderBrush(DependencyObject element) => (Brush)element.GetValue(NoActiveBorderBrushProperty);
        /// <summary>
        /// 设置NoActive边框颜色
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetNoActiveBorderBrush(DependencyObject element, Brush value) => element.SetValue(NoActiveBorderBrushProperty, value);



        /// <summary>
        /// 窗口按钮的宽度
        /// </summary>
        public static readonly DependencyProperty WindowButtonWidthProperty = DependencyProperty.RegisterAttached("WindowButtonWidth", typeof(Double), typeof(WindowElement));
        /// <summary>
        /// 获取窗口按钮的宽度
        /// </summary>
        public static Double GetWindowButtonWidth(DependencyObject element) => (Double)element.GetValue(WindowButtonWidthProperty);
        /// <summary>
        /// 设置窗口按钮的宽度
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetWindowButtonWidth(DependencyObject element, Double value) => element.SetValue(WindowButtonWidthProperty, value);



        /// <summary>
        /// 标题栏字体大小
        /// </summary>
        public static readonly DependencyProperty TitleFontsizeProperty = DependencyProperty.RegisterAttached("TitleFontsize", typeof(Double), typeof(WindowElement));
        /// <summary>
        /// 获取标题栏字体大小
        /// </summary>
        public static Double GetTitleFontsize(DependencyObject element) => (Double)element.GetValue(TitleFontsizeProperty);
        /// <summary>
        /// 设置标题栏字体大小
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetTitleFontsize(DependencyObject element, Double value) => element.SetValue(TitleFontsizeProperty, value);



        /// <summary>
        /// 标题栏边距
        /// </summary>
        public static readonly DependencyProperty TitlePaddingProperty = DependencyProperty.RegisterAttached("TitlePadding", typeof(Thickness), typeof(WindowElement));
        /// <summary>
        /// 获取标题栏边距
        /// </summary>
        public static Thickness GetTitlePadding(DependencyObject element) => (Thickness)element.GetValue(TitlePaddingProperty);
        /// <summary>
        /// 设置标题栏边距
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetTitlePadding(DependencyObject element, Thickness value) => element.SetValue(TitlePaddingProperty, value);



        /// <summary>
        /// 保持最大化（窗口只能最小化和最大化，不能正常化）
        /// </summary>
        public static readonly DependencyProperty KeepMaximizedProperty = DependencyProperty.RegisterAttached("KeepMaximized", typeof(Boolean), typeof(WindowElement), new PropertyMetadata(OnKeepMaximizedPropertyChanged));

        private static readonly DependencyPropertyDescriptor descriptorWindowState = DependencyPropertyDescriptor.FromProperty(Window.WindowStateProperty, typeof(Window));

        private static void OnKeepMaximizedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window win)
            {
                if ((Boolean)e.NewValue)
                {
                    descriptorWindowState.AddValueChanged(win, OnMaximizedPropertyChanged);

                    if (win.WindowState == WindowState.Normal)
                        win.WindowState = WindowState.Maximized;
                }
                else
                    descriptorWindowState.RemoveValueChanged(win, OnMaximizedPropertyChanged);

                void OnMaximizedPropertyChanged(Object sender, EventArgs args)
                {
                    var w = (Window)sender;

                    if (w.WindowState == WindowState.Normal)
                    {
                        w.Dispatcher.InvokeAsync(() =>
                        {
                            if (w.WindowState == WindowState.Normal)
                                w.WindowState = WindowState.Maximized;
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 获取标题栏边距
        /// </summary>
        public static Boolean GetKeepMaximized(DependencyObject element) => (Boolean)element.GetValue(KeepMaximizedProperty);
        /// <summary>
        /// 设置标题栏边距
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetKeepMaximized(DependencyObject element, Boolean value) => element.SetValue(KeepMaximizedProperty, value);
    }
}
