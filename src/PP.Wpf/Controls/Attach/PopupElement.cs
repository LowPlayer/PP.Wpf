using System;
using System.Windows;

namespace PP.Wpf.Controls.Attach
{
    /// <summary>
    /// Popup弹窗元素
    /// </summary>
    public static class PopupElement
    {
        /// <summary>
        /// 最小高度
        /// </summary>
        public static readonly DependencyProperty MinDropDownHeightProperty = DependencyProperty.RegisterAttached("MinDropDownHeight", typeof(Double), typeof(PopupElement));
        /// <summary>
        /// 获取最小高度
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Double GetMinDropDownHeight(DependencyObject element) => (Double)element.GetValue(MinDropDownHeightProperty);
        /// <summary>
        /// 设置最小高度
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetMinDropDownHeight(DependencyObject element, Double value) => element.SetValue(MinDropDownHeightProperty, value);



        /// <summary>
        /// 最大高度
        /// </summary>
        public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.RegisterAttached("MaxDropDownHeight", typeof(Double), typeof(PopupElement));
        /// <summary>
        /// 获取最大高度
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Double GetMaxDropDownHeight(DependencyObject element) => (Double)element.GetValue(MaxDropDownHeightProperty);
        /// <summary>
        /// 设置最大高度
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetMaxDropDownHeight(DependencyObject element, Double value) => element.SetValue(MaxDropDownHeightProperty, value);
    }
}
