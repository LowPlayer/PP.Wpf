using System;
using System.Windows;
using System.Windows.Controls;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 实现没有网络线的Grid
    /// </summary>
    public class SimplePanel : Panel
    {
        /// <summary>
        /// 在派生类中重写时，测量子元素在布局中所需的大小，并确定由 System.Windows.FrameworkElement 派生的类的大小。
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            var maxSize = new Size();

            foreach (UIElement child in InternalChildren)
            {
                if (child != null)
                {
                    child.Measure(constraint);
                    maxSize.Width = Math.Max(maxSize.Width, child.DesiredSize.Width);
                    maxSize.Height = Math.Max(maxSize.Height, child.DesiredSize.Height);
                }
            }

            return maxSize;
        }

        /// <summary>
        /// 在派生类中重写时，为 System.Windows.FrameworkElement 派生类定位子元素并确定大小。
        /// </summary>
        /// <param name="arrangeSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                child?.Arrange(new Rect(arrangeSize));
            }

            return arrangeSize;
        }
    }
}
