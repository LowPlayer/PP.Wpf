using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 鼠标拖动选择ListBox选项
    /// </summary>
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_Canvas", Type = typeof(Canvas))]
    public class ListBoxDragSelectViewer : ContentControl
    {
        #region DependecyProperties

        /// <summary>
        /// 拖动区域填充颜色
        /// </summary>
        public static readonly DependencyProperty FillProperty = Rectangle.FillProperty.AddOwner(typeof(ListBoxDragSelectViewer));
        /// <summary>
        /// 拖动区域填充颜色
        /// </summary>
        public Brush Fill { get => (Brush)GetValue(FillProperty); set => SetValue(FillProperty, value); }



        /// <summary>
        /// 拖动区域描边大小
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = Rectangle.StrokeThicknessProperty.AddOwner(typeof(ListBoxDragSelectViewer));
        /// <summary>
        /// 拖动区域描边大小
        /// </summary>
        public Double StrokeThickness { get => (Double)GetValue(StrokeThicknessProperty); set => SetValue(StrokeThicknessProperty, value); }



        /// <summary>
        /// 拖动区域描边颜色
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = Rectangle.StrokeProperty.AddOwner(typeof(ListBoxDragSelectViewer));
        /// <summary>
        /// 拖动区域描边颜色
        /// </summary>
        public Brush Stroke { get => (Brush)GetValue(StrokeProperty); set => SetValue(StrokeProperty, value); }



        /// <summary>
        /// 控件具有逻辑焦点和捕获鼠标并按下鼠标左键
        /// </summary>
        public static readonly DependencyPropertyKey IsDraggingPropertyKey = DependencyProperty.RegisterReadOnly("IsDragging", typeof(Boolean), typeof(ListBoxDragSelectViewer), new PropertyMetadata(false));
        /// <summary>
        /// 控件具有逻辑焦点和捕获鼠标并按下鼠标左键
        /// </summary>
        public static readonly DependencyProperty IsDraggingProperty = IsDraggingPropertyKey.DependencyProperty;
        /// <summary>
        /// 控件具有逻辑焦点和捕获鼠标并按下鼠标左键
        /// </summary>
        public Boolean IsDragging { get => (Boolean)GetValue(IsDraggingProperty); protected set => SetValue(IsDraggingPropertyKey, value); }



        /// <summary>
        /// 拖动区域
        /// </summary>
        public static readonly DependencyPropertyKey DragRectPropertyKey = DependencyProperty.RegisterReadOnly("DragRect", typeof(Rect), typeof(ListBoxDragSelectViewer), new PropertyMetadata(Rect.Empty));
        /// <summary>
        /// 拖动区域
        /// </summary>
        public static readonly DependencyProperty DragRectProperty = DragRectPropertyKey.DependencyProperty;
        /// <summary>
        /// 拖动区域
        /// </summary>
        public Rect DragRect { get => (Rect)GetValue(DragRectProperty); protected set => SetValue(DragRectPropertyKey, value); }

        #endregion

        static ListBoxDragSelectViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListBoxDragSelectViewer), new FrameworkPropertyMetadata(typeof(ListBoxDragSelectViewer)));
        }

        /// <summary>
        /// 鼠标拖动选择ListBox选项
        /// </summary>
        public ListBoxDragSelectViewer()
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
            timer.Tick += OnTick;
        }

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (canvas != null)
                canvas.MouseLeftButtonDown -= OnCanvasMouseLeftButtonDown;

            scrollViewer = this.Template.FindName("PART_ScrollViewer", this) as ScrollViewer;
            canvas = this.Template.FindName("PART_Canvas", this) as Canvas;

            if (canvas != null)
                canvas.MouseLeftButtonDown += OnCanvasMouseLeftButtonDown;
        }

        private void OnCanvasMouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            listbox = this.TemplatedParent as ListBox;

            if (listbox == null || listbox.SelectionMode == SelectionMode.Single || canvas == null)
                return;

            startPoint = e.GetPosition(canvas);

            if (e.Handled = canvas.CaptureMouse())
            {
                IsDragging = true;
                DragRect = new Rect(startPoint, new Size());

                canvas.MouseMove += OnCanvasMouseMove;
                canvas.MouseLeftButtonUp += OnCanvasMouseLeftButtonUp;
            }
        }

        private void OnCanvasMouseMove(Object sender, MouseEventArgs e)
        {
            e.Handled = true;

            var point = e.GetPosition(canvas);
            DragRect = new Rect(startPoint, point);
            SelectItems();

            point = e.GetPosition(scrollViewer);

            if (point.X < 0 || point.Y < 0 || point.X > scrollViewer.ViewportWidth || point.Y > scrollViewer.ViewportHeight)
            {
                canvas.MouseMove -= OnCanvasMouseMove;
                UpdateScrollViewerOffset(point);
                timer.Start();
            }
        }

        private void OnCanvasMouseLeftButtonUp(Object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            canvas.MouseMove -= OnCanvasMouseMove;
            canvas.MouseLeftButtonUp -= OnCanvasMouseLeftButtonUp;

            canvas.ReleaseMouseCapture();

            timer.Stop();

            IsDragging = false;
            this.ClearValue(DragRectPropertyKey);
        }

        private void OnTick(Object sender, EventArgs e)
        {
            if (!canvas.IsMouseCaptured)
                return;

            var point = Mouse.GetPosition(canvas);
            DragRect = new Rect(startPoint, point);
            SelectItems();

            point = Mouse.GetPosition(scrollViewer);

            if (point.X < 0 || point.Y < 0 || point.X > scrollViewer.ViewportWidth || point.Y > scrollViewer.ViewportHeight)
                UpdateScrollViewerOffset(point);
            else
            {
                timer.Stop();
                canvas.MouseMove += OnCanvasMouseMove;
            }
        }

        private void UpdateScrollViewerOffset(Point point)
        {
            if (point.X < 0)
            {
                if (scrollViewer.HorizontalOffset > 0)
                    scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - offset);
            }
            else if (point.X > scrollViewer.ViewportWidth)
            {
                if (scrollViewer.HorizontalOffset < scrollViewer.ScrollableWidth)
                    scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + offset);
            }

            if (point.Y < 0)
            {
                if (scrollViewer.VerticalOffset > 0)
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offset);
            }
            else if (point.Y > scrollViewer.ViewportHeight)
            {
                if (scrollViewer.VerticalOffset < scrollViewer.ScrollableHeight)
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offset);
            }
        }

        private void SelectItems()
        {
            var rect = DragRect;

            foreach (var item in listbox.Items)
            {
                var ele = (ListBoxItem)listbox.ItemContainerGenerator.ContainerFromItem(item);

                var item_rect = new Rect(ele.TranslatePoint(new Point(), canvas), ele.RenderSize);

                if (rect.IntersectsWith(item_rect))
                    ele.SetCurrentValue(ListBoxItem.IsSelectedProperty, true);
                else
                    ele.SetCurrentValue(ListBoxItem.IsSelectedProperty, false);
            }
        }

        #endregion

        #region Fields

        private ScrollViewer scrollViewer;
        private Canvas canvas;
        private ListBox listbox;
        private Point startPoint;
        private DispatcherTimer timer;
        private readonly Int32 offset = 10;

        #endregion
    }
}
