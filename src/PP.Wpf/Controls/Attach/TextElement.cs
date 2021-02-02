using System;
using System.Windows;
using System.Windows.Controls;

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



        /// <summary>
        /// 是否监听PasswordBox的PasswordChanged事件
        /// </summary>
        public static readonly DependencyProperty AttachPasswordBoxProperty = DependencyProperty.RegisterAttached("AttachPasswordBox", typeof(Boolean), typeof(TextElement), new PropertyMetadata(OnAttachPasswordBoxPropertyChanged));
        /// <summary>
        /// 获取是否监听PasswordBox的PasswordChanged事件
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Boolean GetAttachPasswordBox(DependencyObject element) => (Boolean)element.GetValue(AttachPasswordBoxProperty);
        /// <summary>
        /// 设置是否监听PasswordBox的PasswordChanged事件
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetAttachPasswordBox(DependencyObject element, Boolean value) => element.SetValue(AttachPasswordBoxProperty, value);

        private static void OnAttachPasswordBoxPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pb)
            {
                if ((Boolean)e.NewValue)
                    pb.PasswordChanged += OnPasswordChanged;
                else
                    pb.PasswordChanged -= OnPasswordChanged;
            }
        }

        private static void OnPasswordChanged(Object sender, RoutedEventArgs e)
        {
            var pb = (PasswordBox)sender;
            SetPassword(pb, pb.Password);
        }



        /// <summary>
        /// 密码
        /// </summary>
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(String), typeof(TextElement), new PropertyMetadata(OnPasswordPropertyChanged));
        /// <summary>
        /// 获取密码
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static String GetPassword(DependencyObject element) => (String)element.GetValue(PasswordProperty);
        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetPassword(DependencyObject element, String value) => element.SetValue(PasswordProperty, value);

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pb)
            {
                var newVal = (String)e.NewValue;

                if (pb.Password != newVal)
                    pb.Password = newVal;
            }
        }
    }
}
