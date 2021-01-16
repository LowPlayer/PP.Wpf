using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

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
        public Boolean IsDragging { get => (Boolean)GetValue(IsDraggingProperty); protected set => SetValue(IsDraggingProperty, value); }



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

        #region Methods

        /// <summary>
        /// 应用模板
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            scrollViewer = this.Template.FindName("PART_ScrollViewer", this) as ScrollViewer;
            canvas = this.Template.FindName("PART_Canvas", this) as Canvas;
        }

        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            listbox = this.TemplatedParent as ListBox;

            if (listbox == null || listbox.SelectionMode == SelectionMode.Single || canvas == null)
                return;

            startPoint = e.GetPosition(canvas);

            if (e.Handled = this.CaptureMouse())
            {
                IsDragging = true;
                DragRect = new Rect(startPoint, new Size());
            }
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!IsDragging)
                return;

            e.Handled = true;

            var point = e.GetPosition(canvas);
            var rect = DragRect = new Rect(startPoint, point);

            foreach (var item in listbox.Items)
            {
                var ele = (ListBoxItem)listbox.ItemContainerGenerator.ContainerFromItem(item);

                var item_rect = new Rect(ele.TranslatePoint(new Point(), this), ele.RenderSize);

                if (rect.IntersectsWith(item_rect))
                    ele.SetCurrentValue(ListBoxItem.IsSelectedProperty, true);
                else
                    ele.SetCurrentValue(ListBoxItem.IsSelectedProperty, false);
            }


        }

        /// <summary>
        /// 鼠标左键弹起
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (!IsDragging)
                return;

            e.Handled = true;
            this.ReleaseMouseCapture();

            IsDragging = false;
            this.ClearValue(DragRectProperty);
        }

        #endregion

        #region Fields

        private ScrollViewer scrollViewer;
        private Canvas canvas;
        private ListBox listbox;
        private Point startPoint;

        #endregion
    }
}
