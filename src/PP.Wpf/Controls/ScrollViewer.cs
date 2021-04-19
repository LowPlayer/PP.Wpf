using System.Windows.Controls;
using System.Windows.Input;

namespace PP.Wpf.Controls
{
    public sealed class ScrollViewer : System.Windows.Controls.ScrollViewer
    {
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (VerticalScrollBarVisibility == ScrollBarVisibility.Disabled || (e.Delta < 0 && VerticalOffset == ScrollableHeight) || (e.Delta > 0 && VerticalOffset == 0))
                return;

            base.OnMouseWheel(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (VerticalScrollBarVisibility == ScrollBarVisibility.Disabled || VerticalOffset == 0)
                        return;
                    break;
                case Key.Down:
                    if (VerticalScrollBarVisibility == ScrollBarVisibility.Disabled || VerticalOffset == ScrollableHeight)
                        return;
                    break;
                case Key.Left:
                    if (HorizontalScrollBarVisibility == ScrollBarVisibility.Disabled || HorizontalOffset == 0)
                        return;
                    break;
                case Key.Right:
                    if (HorizontalScrollBarVisibility == ScrollBarVisibility.Disabled || HorizontalOffset == ScrollableWidth)
                        return;
                    break;
            }

            base.OnKeyDown(e);
        }
    }
}
