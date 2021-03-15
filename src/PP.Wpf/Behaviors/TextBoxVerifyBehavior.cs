using Microsoft.Xaml.Behaviors;
using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace PP.Wpf.Behaviors
{
    /// <summary>
    /// 文本框输入前验证
    /// </summary>
    public sealed class TextBoxVerifyBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.PreviewTextInput += AssociatedObject_PreviewTextInput;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.PreviewTextInput -= AssociatedObject_PreviewTextInput;
        }

        private void AssociatedObject_PreviewTextInput(Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var text = this.AssociatedObject.Text;
            var selectedText = this.AssociatedObject.SelectedText;

            if (!String.IsNullOrEmpty(selectedText))
                text = text.Remove(this.AssociatedObject.CaretIndex, selectedText.Length);

            text = text.Insert(this.AssociatedObject.CaretIndex, e.Text);

            if (!String.IsNullOrEmpty(PrevInputReg) && !Regex.IsMatch(text, PrevInputReg))
            {
                e.Handled = true;
                return;
            }

            if (IsNumber)
            {
                if (!Double.TryParse(text, out Double val) || val < MinValue || val > MaxValue)
                    e.Handled = true;
            }
        }

        #region 属性

        /// <summary>
        /// 预输入正则表达式(筛选不可输入的字符)
        /// </summary>
        public String PrevInputReg { get; set; }

        /// <summary>
        /// 是否数字
        /// </summary>
        public Boolean IsNumber { get; set; }

        /// <summary>
        /// 数值的最大值
        /// </summary>
        public Double MaxValue { get; set; } = Int32.MaxValue;

        /// <summary>
        /// 数值的最小值
        /// </summary>
        public Double MinValue { get; set; } = Int32.MinValue;

        #endregion
    }
}
