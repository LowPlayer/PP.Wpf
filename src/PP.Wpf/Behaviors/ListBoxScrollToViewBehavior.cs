using Microsoft.Xaml.Behaviors;
using PP.Wpf.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PP.Wpf.Behaviors
{
    public sealed class ListBoxScrollToViewBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            var list = AssociatedObject;

            list.SelectionChanged += OnSelectionChanged;
            list.Loaded += OnLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            var list = AssociatedObject;

            list.SelectionChanged -= OnSelectionChanged;
            list.Loaded -= OnLoaded;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScrollToView();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ScrollToView();
        }

        private void ScrollToView()
        {
            var list = AssociatedObject;

            if (!list.IsLoaded || list.SelectedItem == null || (list.IsMouseOver && Mouse.LeftButton == MouseButtonState.Pressed))
                return;

            var item = (ListBoxItem)list.ItemContainerGenerator.ContainerFromItem(list.SelectedItem);

            if (item == null)
                return;

            var sv = list.FindChildByType<ScrollViewer>();

            if (sv == null)
                return;

            Point point;

            switch (Position)
            {
                case Positions.Center:
                    point = new Point(0, -(sv.ViewportHeight - item.ActualHeight) / 2);
                    break;
                case Positions.Bottom:
                    point = new Point(0, -(sv.ViewportHeight - item.ActualHeight));
                    break;
                default:
                    point = new Point();
                    break;
            }

            var offsetY = item.TranslatePoint(point, sv).Y;

            sv.ScrollToVerticalOffset(sv.VerticalOffset + offsetY);
        }

        #region Properties

        public Positions Position { get; set; }

        #endregion

        public enum Positions
        {
            Top,
            Center,
            Bottom
        }
    }
}
