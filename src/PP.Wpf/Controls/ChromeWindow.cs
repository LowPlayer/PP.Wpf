using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 使用WindowChorme创建的自定义窗体
    /// </summary>
    [TemplatePart(Name = "PART_TitleBackground", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_Min", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Max", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Close", Type = typeof(Button))]
    public class ChromeWindow : Window
    {
        #region DependencyProperties

        /// <summary>
        /// 最大化时，内容要设置的外边距
        /// </summary>
        public static readonly DependencyPropertyKey LayoutMarginPropertyKey = DependencyProperty.RegisterReadOnly("LayoutMargin", typeof(Thickness), typeof(ChromeWindow), new PropertyMetadata(new Thickness(8)));
        /// <summary>
        /// 最大化时，内容要设置的外边距
        /// </summary>
        public static readonly DependencyProperty LayoutMarginProperty = LayoutMarginPropertyKey.DependencyProperty;
        /// <summary>
        /// 最大化时，内容要设置的外边距
        /// </summary>
        public Thickness LayoutMargin { get => (Thickness)GetValue(LayoutMarginProperty); protected set => SetValue(LayoutMarginPropertyKey, value); }



        /// <summary>
        /// 标题内容
        /// </summary>

        public static readonly DependencyProperty TitleContentProperty = DependencyProperty.Register("TitleContent", typeof(Object), typeof(ChromeWindow));
        /// <summary>
        /// 标题内容
        /// </summary>
        public Object TitleContent { get => GetValue(TitleContentProperty); set => SetValue(TitleContentProperty, value); }



        /// <summary>
        /// 标题内容数据模板
        /// </summary>
        public static readonly DependencyProperty TitleContentTemplateProperty = DependencyProperty.Register("TitleContentTemplate", typeof(DataTemplate), typeof(ChromeWindow));
        /// <summary>
        /// 标题内容数据模板
        /// </summary>
        public DataTemplate TitleContentTemplate { get => (DataTemplate)GetValue(TitleContentTemplateProperty); set => SetValue(TitleContentTemplateProperty, value); }

        #endregion

        static ChromeWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromeWindow), new FrameworkPropertyMetadata(typeof(ChromeWindow)));
        }

        /// <summary>
        /// 使用WindowChorme创建的自定义窗体
        /// </summary>
        public ChromeWindow()
        {
            var w = (SystemParameters.MaximizedPrimaryScreenWidth - SystemParameters.WorkArea.Width) / 2;
            var h = (SystemParameters.MaximizedPrimaryScreenHeight - SystemParameters.WorkArea.Height) / 2;
            LayoutMargin = new Thickness(w, h, w, h);
        }

        #region Methods

        #region Override Methods

        /// <summary>
        /// 应用模板
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (ctrl_title != null)
                ctrl_title.MouseLeftButtonDown -= OnTitleMouseLeftButtonDown;
            if (btn_min != null)
                btn_min.Click -= OnMinButtonClick;
            if (btn_max != null)
                btn_max.Click -= OnMaxButtonClick;
            if (btn_close != null)
                btn_close.Click -= OnCloseButtonClick;

            ctrl_title = this.Template.FindName("PART_TitleBackground", this) as FrameworkElement;
            btn_min = this.Template.FindName("PART_Min", this) as Button;
            btn_max = this.Template.FindName("PART_Max", this) as Button;
            btn_close = this.Template.FindName("PART_Close", this) as Button;

            if (ctrl_title != null)
                ctrl_title.MouseLeftButtonDown += OnTitleMouseLeftButtonDown;
            if (btn_min != null)
                btn_min.Click += OnMinButtonClick;
            if (btn_max != null)
                btn_max.Click += OnMaxButtonClick;
            if (btn_close != null)
                btn_close.Click += OnCloseButtonClick;
        }

        #endregion

        #region Private Methods

        private void OnMinButtonClick(Object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnMaxButtonClick(Object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void OnCloseButtonClick(Object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnTitleMouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip)
                {
                    this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                    e.Handled = true;
                }
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
                e.Handled = true;
            }
        }

        #endregion

        #endregion

        #region Fields

        private FrameworkElement ctrl_title;
        private Button btn_min, btn_max, btn_close;

        #endregion
    }
}
