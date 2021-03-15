using Microsoft.Xaml.Behaviors;
using System;
using System.Windows.Controls;

namespace PP.Wpf.Behaviors
{
    /// <summary>
    /// 文本框获取焦点
    /// </summary>
    public sealed class TextBoxFocusBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += OnAssociatedObjectLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
        }

        private void OnAssociatedObjectLoaded(Object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.AssociatedObject.Focus() && IsSelectAll)
                this.AssociatedObject.SelectAll();
        }

        #region Property

        /// <summary>
        /// 具有焦点时是否全选
        /// </summary>
        public Boolean IsSelectAll { get; set; }

        #endregion
    }
}
