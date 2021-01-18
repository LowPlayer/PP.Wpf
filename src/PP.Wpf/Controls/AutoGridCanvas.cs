using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 自动分栏且子项可拖动
    /// </summary>
    public sealed class AutoGridCanvas : SimplePanel
    {
        #region DependencyProperties

        #region ItemsSource

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ICollection), typeof(AutoGridCanvas), new PropertyMetadata(OnItemsSourcePropertyChanged));

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoGridCanvas)d).OnItemsSourceChanged((ICollection)e.OldValue, (ICollection)e.NewValue);
        }

        public ICollection ItemsSource { get => (ICollection)GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }

        public static readonly DependencyProperty ItemTemplateProperty = ListBox.ItemTemplateProperty.AddOwner(typeof(AutoGridCanvas), new PropertyMetadata(OnItemTemplatePropertyChanged));

        private static void OnItemTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var gridCanvas = (AutoGridCanvas)d;

            foreach (AutoGridCanvasItem item in gridCanvas.canvas.Children)
            {
                item.SetCurrentValue(ContentControl.ContentTemplateProperty, e.NewValue);
            }
        }

        public DataTemplate ItemTemplate { get => (DataTemplate)GetValue(ItemTemplateProperty); set => SetValue(ItemTemplateProperty, value); }

        #endregion

        #region GridLine

        public static readonly DependencyProperty StrokeProperty = Line.StrokeProperty.AddOwner(typeof(AutoGridCanvas), new PropertyMetadata(OnStrokePropertyChanged));

        private static void OnStrokePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoGridCanvas)d).UpdateGridLineStroke();
        }

        /// <summary>
        /// 网络线颜色
        /// </summary>
        public Brush Stroke { get => (Brush)GetValue(StrokeProperty); set => SetValue(StrokeProperty, value); }

        public static readonly DependencyProperty StrokeThicknessProperty = Line.StrokeThicknessProperty.AddOwner(typeof(AutoGridCanvas), new PropertyMetadata(OnStrokeThicknessPropertyChanged));

        private static void OnStrokeThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoGridCanvas)d).UpdateGridLineStrokeThickness();
        }

        /// <summary>
        /// 网络线宽度
        /// </summary>
        public Double StrokeThickness { get => (Double)GetValue(StrokeThicknessProperty); set => SetValue(StrokeThicknessProperty, value); }

        #endregion

        #region Rows Columns

        public static readonly DependencyPropertyKey RowsProperty = DependencyProperty.RegisterReadOnly("Rows", typeof(Int32), typeof(AutoGridCanvas), new PropertyMetadata(1));
        /// <summary>
        /// 行数
        /// </summary>
        public Int32 Rows { get => (Int32)GetValue(RowsProperty.DependencyProperty); }

        public static readonly DependencyPropertyKey ColumnsProperty = DependencyProperty.RegisterReadOnly("Columns", typeof(Int32), typeof(AutoGridCanvas), new PropertyMetadata(1));
        /// <summary>
        /// 列数
        /// </summary>
        public Int32 Columns { get => (Int32)GetValue(ColumnsProperty.DependencyProperty); }

        #endregion

        #endregion

        public AutoGridCanvas()
        {
            this.canvas = new Canvas();
            this.lineGrid = new Grid();

            this.Children.Add(lineGrid);
            this.Children.Add(canvas);

            this.SizeChanged += OnSizeChanged;
            this.Loaded += OnLoaded;
        }

        #region Methods

        private void OnLoaded(Object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoaded;
            Reset();
        }

        private void OnSizeChanged(Object sender, SizeChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            position_action = UpdateItemsPosition;

            this.Dispatcher.InvokeAsync(() =>
            {
                if (position_action != null)
                {
                    position_action.Invoke();
                    position_action = null;
                }

            }, DispatcherPriority.Background);
        }

        private void OnItemsSourceChanged(ICollection olds, ICollection news)
        {
            if (olds is INotifyCollectionChanged old_oc)
                old_oc.CollectionChanged -= OnCollectionChanged;

            if (news is INotifyCollectionChanged new_oc)
                new_oc.CollectionChanged += OnCollectionChanged;

            if (!IsLoaded)
                return;

            Reset();
        }

        private void OnCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            collection_action = DoCollectionChanged;

            this.Dispatcher.InvokeAsync(() =>
            {
                if (collection_action != null)
                {
                    collection_action.Invoke();
                    collection_action = null;
                }

            }, DispatcherPriority.Background);
        }

        private void DoCollectionChanged()
        {
            GetRowsAndColumns(Count, out Int32 rows, out Int32 columns);

            if (rows != Rows || columns != Columns)
            {
                SetValue(RowsProperty, rows);
                SetValue(ColumnsProperty, columns);

                DrawGridLine();
            }

            DrawItems();
        }

        private void Reset()
        {
            GetRowsAndColumns(Count, out Int32 rows, out Int32 columns);

            SetValue(RowsProperty, rows);
            SetValue(ColumnsProperty, columns);

            DrawGridLine();
            DrawItems();
        }

        #region GridLine

        /// <summary>
        /// 根据数量获取行列数
        /// </summary>
        /// <param name="count"></param>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        private void GetRowsAndColumns(Int32 count, out Int32 rows, out Int32 columns)
        {
            rows = 1;
            columns = 1;

            var sum = rows * columns;

            while (sum < count)
            {
                columns++;
                sum = rows * columns;

                if (sum >= count)
                    return;

                rows++;
                sum = rows * columns;
            }
        }

        /// <summary>
        /// 绘制网络线
        /// </summary>
        private void DrawGridLine()
        {
            lineGrid.Children.Clear();
            lineGrid.RowDefinitions.Clear();
            lineGrid.ColumnDefinitions.Clear();

            for (var i = 0; i < Rows; i++)
            {
                lineGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                if (i < Rows - 1)
                {
                    lineGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    var line = new Line
                    {
                        X2 = 1,
                        Stretch = Stretch.Uniform,
                        StrokeThickness = StrokeThickness,
                        Stroke = Stroke
                    };

                    Grid.SetRow(line, i * 2 + 1);
                    Grid.SetColumnSpan(line, Columns * 2 - 1);

                    lineGrid.Children.Add(line);
                }
            }

            for (var i = 0; i < Columns; i++)
            {
                lineGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                if (i < Columns - 1)
                {
                    lineGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    var line = new Line
                    {
                        Y2 = 1,
                        Stretch = Stretch.Uniform,
                        StrokeThickness = StrokeThickness,
                        Stroke = Stroke
                    };

                    Grid.SetColumn(line, i * 2 + 1);
                    Grid.SetRowSpan(line, Rows * 2 - 1);

                    lineGrid.Children.Add(line);
                }
            }
        }

        /// <summary>
        /// 更新网络线颜色
        /// </summary>
        private void UpdateGridLineStroke()
        {
            foreach (Line line in lineGrid.Children)
            {
                line.Stroke = Stroke;
            }
        }

        /// <summary>
        /// 更新网络线宽度
        /// </summary>
        private void UpdateGridLineStrokeThickness()
        {
            foreach (Line line in lineGrid.Children)
            {
                line.StrokeThickness = StrokeThickness;
            }

            foreach (AutoGridCanvasItem item in canvas.Children)
            {
                item.StrokeThickness = StrokeThickness;
            }

            position_action = UpdateItemsPosition;

            this.Dispatcher.InvokeAsync(() =>
            {
                if (position_action != null)
                {
                    position_action.Invoke();
                    position_action = null;
                }

            }, DispatcherPriority.Background);
        }

        #endregion

        #region Canvas

        private void DrawItems()
        {
            this.canvas.Children.Clear();

            if (Count == 0)
                return;

            var index = 0;

            foreach (var data in ItemsSource)
            {
                var item = new AutoGridCanvasItem(canvas)
                {
                    DataContext = data,
                    Content = data,
                    ContentTemplate = ItemTemplate,
                    Rows = Rows,
                    Columns = Columns,
                    StrokeThickness = StrokeThickness,
                    Rect = new Rect(index % Columns, index / Columns, 1, 1),
                    IsCell = true
                };

                this.canvas.Children.Add(item);
                index++;
            }

            UpdateItemsPosition();
        }

        /// <summary>
        /// 更新位置
        /// </summary>
        private void UpdateItemsPosition()
        {
            var size = canvas.RenderSize;
            var line = Math.Ceiling(StrokeThickness / 2);

            foreach (AutoGridCanvasItem item in canvas.Children)
            {
                var rect = item.Rect;

                var x = rect.X * size.Width / Columns;
                var y = rect.Y * size.Height / Rows;
                var w = rect.Width * size.Width / Columns;
                var h = rect.Height * size.Height / Rows;

                if (item.IsCell)
                {
                    if (rect.X != 0)
                    {
                        x += line;
                        w -= line;
                    }
                    if (rect.Y != 0)
                    {
                        y += line;
                        h -= line;
                    }

                    if (rect.X + rect.Width != Columns)
                        w -= line;
                    if (rect.Y + rect.Height != Rows)
                        h -= line;
                }

                item.SetCurrentValue(Canvas.LeftProperty, x);
                item.SetCurrentValue(Canvas.TopProperty, y);
                item.SetCurrentValue(WidthProperty, w);
                item.SetCurrentValue(HeightProperty, h);
            }
        }

        #endregion

        #endregion

        #region Propertys

        public Int32 Count => ItemsSource == null ? 0 : ItemsSource.Count;

        #endregion

        #region Fields

        private Canvas canvas;
        private Grid lineGrid;
        private Action collection_action;
        private Action position_action;

        #endregion
    }


    [TemplatePart(Name = "PART_ResizeTopLeft", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_ResizeTop", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_ResizeTopRight", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_ResizeRight", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_ResizeBottomRight", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_ResizeBottom", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_ResizeBottomLeft", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_ResizeLeft", Type = typeof(UIElement))]
    public sealed class AutoGridCanvasItem : ContentControl
    {
        #region DependencyProperties

        public static readonly DependencyProperty ResizeBorderThicknessProperty = DependencyProperty.Register("ResizeBorderThickness", typeof(Double), typeof(AutoGridCanvasItem), new PropertyMetadata(6d));

        public Double ResizeBorderThickness { get => (Double)GetValue(ResizeBorderThicknessProperty); set => SetValue(ResizeBorderThicknessProperty, value); }

        #endregion

        static AutoGridCanvasItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoGridCanvasItem), new FrameworkPropertyMetadata(typeof(AutoGridCanvasItem)));
        }

        public AutoGridCanvasItem(Canvas panel)
        {
            this.panel = panel;
            this.RenderTransform = trans = new MatrixTransform();
        }

        #region Methods

        #region Override Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (tl != null)
                tl.MouseLeftButtonDown -= OnResizeBorderMouseLeftButtonDown;
            if (t != null)
                t.MouseLeftButtonDown -= OnResizeBorderMouseLeftButtonDown;
            if (tr != null)
                tr.MouseLeftButtonDown -= OnResizeBorderMouseLeftButtonDown;
            if (r != null)
                r.MouseLeftButtonDown -= OnResizeBorderMouseLeftButtonDown;
            if (br != null)
                br.MouseLeftButtonDown -= OnResizeBorderMouseLeftButtonDown;
            if (b != null)
                b.MouseLeftButtonDown -= OnResizeBorderMouseLeftButtonDown;
            if (bl != null)
                bl.MouseLeftButtonDown -= OnResizeBorderMouseLeftButtonDown;
            if (l != null)
                l.MouseLeftButtonDown -= OnResizeBorderMouseLeftButtonDown;

            tl = this.Template.FindName("PART_ResizeTopLeft", this) as UIElement;
            t = this.Template.FindName("PART_ResizeTop", this) as UIElement;
            tr = this.Template.FindName("PART_ResizeTopRight", this) as UIElement;
            r = this.Template.FindName("PART_ResizeRight", this) as UIElement;
            br = this.Template.FindName("PART_ResizeBottomRight", this) as UIElement;
            b = this.Template.FindName("PART_ResizeBottom", this) as UIElement;
            bl = this.Template.FindName("PART_ResizeBottomLeft", this) as UIElement;
            l = this.Template.FindName("PART_ResizeLeft", this) as UIElement;

            if (tl != null)
                tl.MouseLeftButtonDown += OnResizeBorderMouseLeftButtonDown;
            if (t != null)
                t.MouseLeftButtonDown += OnResizeBorderMouseLeftButtonDown;
            if (tr != null)
                tr.MouseLeftButtonDown += OnResizeBorderMouseLeftButtonDown;
            if (r != null)
                r.MouseLeftButtonDown += OnResizeBorderMouseLeftButtonDown;
            if (br != null)
                br.MouseLeftButtonDown += OnResizeBorderMouseLeftButtonDown;
            if (b != null)
                b.MouseLeftButtonDown += OnResizeBorderMouseLeftButtonDown;
            if (bl != null)
                bl.MouseLeftButtonDown += OnResizeBorderMouseLeftButtonDown;
            if (l != null)
                l.MouseLeftButtonDown += OnResizeBorderMouseLeftButtonDown;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            SetZIndex();

            point = e.GetPosition(panel);
            e.Handled = this.CaptureMouse();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton != MouseButtonState.Pressed || isMouseWheel)
                return;

            var matrix = this.trans.Matrix;

            var temp = e.GetPosition(panel);

            var offsetX = temp.X - this.point.X;
            var offsetY = temp.Y - this.point.Y;

            this.point = temp;

            matrix.Translate(offsetX, offsetY);

            this.trans.Matrix = matrix;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (IsMouseCaptured)
            {
                this.ReleaseMouseCapture();
                ClearMatrix();
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (Mouse.Captured != null && Mouse.Captured != this)
                return;

            isMouseWheel = true;

            var matrix = this.trans.Matrix;
            var scale = 1 + e.Delta / 2400.0;

            var center = new Point(this.ActualWidth / 2, this.ActualHeight / 2);
            center = matrix.Transform(center);
            matrix.ScaleAt(scale, scale, center.X, center.Y);
            this.trans.Matrix = matrix;

            ClearMatrix(false);

            isMouseWheel = false;
        }

        #endregion

        #region Private Methods

        private void SetZIndex()
        {
            var brothers = panel.Children.Cast<AutoGridCanvasItem>().Where(q => !Object.ReferenceEquals(q, this)).OrderBy(q => Panel.GetZIndex(q)).ToList();

            Panel.SetZIndex(this, panel.Children.Count - 1);

            for (var i = 0; i < brothers.Count; i++)
            {
                Panel.SetZIndex(brothers[i], i);
            }
        }

        private void ClearMatrix(Boolean audoDock = true)
        {
            var matrix = this.trans.Matrix;

            var left = Canvas.GetLeft(this) + matrix.OffsetX;
            var top = Canvas.GetTop(this) + matrix.OffsetY;
            var width = this.ActualWidth * matrix.M11;
            var height = this.ActualHeight * matrix.M22;

            if (audoDock)
            {
                var bound = ((FrameworkElement)this.Parent).RenderSize;
                AutoDock(ref left, ref top, ref width, ref height, ref bound);
            }

            //移除MatrixTransform效果，改变真实的位置及大小
            this.SetCurrentValue(Canvas.LeftProperty, left);
            this.SetCurrentValue(Canvas.TopProperty, top);
            this.SetCurrentValue(WidthProperty, width);
            this.SetCurrentValue(HeightProperty, height);
            this.trans.Matrix = new Matrix();
        }

        /// <summary>
        /// 自动吸附
        /// </summary>
        private void AutoDock(ref Double left, ref Double top, ref Double width, ref Double height, ref Size bound)
        {
            var lenX = (width * 0.05) * Columns / bound.Width;
            var lenY = (height * 0.05) * Rows / bound.Height;

            var rectX = left * Columns / bound.Width;
            var rectY = top * Rows / bound.Height;
            var rectW = width * Columns / bound.Width;
            var rectH = height * Rows / bound.Height;

            var x = (Int32)Math.Round(rectX);
            var y = (Int32)Math.Round(rectY);
            var w = (Int32)Math.Round(rectW);
            var h = (Int32)Math.Round(rectH);

            if (Math.Abs(x - rectX) > lenX || Math.Abs(y - rectY) > lenY
                || Math.Abs(w - rectW) > lenX * 2 || Math.Abs(h - rectH) > lenY * 2)
            {
                Rect = new Rect(rectX, rectY, rectW, rectH);
                IsCell = false;
            }
            else
            {
                Rect = new Rect(x, y, w, h);
                IsCell = true;

                var line = Math.Ceiling(StrokeThickness / 2);

                left = x * bound.Width / Columns;
                top = y * bound.Height / Rows;
                width = w * bound.Width / Columns;
                height = h * bound.Height / Rows;

                if (x != 0)
                {
                    left += line;
                    width -= line;
                }
                if (y != 0)
                {
                    top += line;
                    height -= line;
                }

                if (x + w != Columns)
                    width -= line;
                if (y + h != Rows)
                    height -= line;
            }
        }

        private void OnResizeBorderMouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            var el = (UIElement)sender;

            SetZIndex();

            point = e.GetPosition(panel);

            if (e.Handled = el.CaptureMouse())
            {
                el.MouseMove += OnResizeBorderMouseMove;
                el.MouseLeftButtonUp += OnResizeBorderMouseLeftButtonUp;
            }
        }

        private void OnResizeBorderMouseMove(Object sender, MouseEventArgs e)
        {
            var temp = e.GetPosition(this.panel);

            var offsetX = temp.X - this.point.X;
            var offsetY = temp.Y - this.point.Y;

            this.point = temp;

            var flag = false;
            if ((flag = Object.ReferenceEquals(sender, l) || Object.ReferenceEquals(sender, tl) || Object.ReferenceEquals(sender, bl))
                || Object.ReferenceEquals(sender, r) || Object.ReferenceEquals(sender, tr) || Object.ReferenceEquals(sender, br))
            {
                var width = this.ActualWidth + (flag ? -offsetX : offsetX);

                if (width < minSize)
                {
                    width = minSize;
                    offsetX = this.ActualWidth - width;
                }

                this.SetCurrentValue(WidthProperty, width);
            }

            if ((flag = Object.ReferenceEquals(sender, t) || Object.ReferenceEquals(sender, tl) || Object.ReferenceEquals(sender, tr))
                || Object.ReferenceEquals(sender, b) || Object.ReferenceEquals(sender, bl) || Object.ReferenceEquals(sender, br))
            {
                var height = this.ActualHeight + (flag ? -offsetY : offsetY);

                if (height < minSize)
                {
                    height = minSize;
                    offsetY = this.ActualHeight - height;
                }

                this.SetCurrentValue(HeightProperty, height);
            }

            if (Object.ReferenceEquals(sender, l) || Object.ReferenceEquals(sender, tl) || Object.ReferenceEquals(sender, bl))
            {
                var left = Canvas.GetLeft(this) + offsetX;
                this.SetCurrentValue(Canvas.LeftProperty, left);
            }

            if (Object.ReferenceEquals(sender, t) || Object.ReferenceEquals(sender, tl) || Object.ReferenceEquals(sender, tr))
            {
                var top = Canvas.GetTop(this) + offsetY;
                this.SetCurrentValue(Canvas.TopProperty, top);
            }
        }

        private void OnResizeBorderMouseLeftButtonUp(Object sender, MouseButtonEventArgs e)
        {
            var el = (UIElement)sender;

            el.MouseMove -= OnResizeBorderMouseMove;
            el.MouseLeftButtonUp -= OnResizeBorderMouseLeftButtonUp;

            el.ReleaseMouseCapture();

            var bound = ((FrameworkElement)this.Parent).RenderSize;
            Double left = Canvas.GetLeft(this), top = Canvas.GetTop(this), width = this.ActualWidth, height = this.ActualHeight;

            AutoDock(ref left, ref top, ref width, ref height, ref bound);

            this.SetCurrentValue(Canvas.LeftProperty, left);
            this.SetCurrentValue(Canvas.TopProperty, top);
            this.SetCurrentValue(WidthProperty, width);
            this.SetCurrentValue(HeightProperty, height);
        }

        #endregion

        #endregion

        #region Propertys

        public Int32 Rows { get; set; }
        public Int32 Columns { get; set; }
        public Double StrokeThickness { get; set; }
        public Rect Rect { get; set; }
        /// <summary>
        /// 是否在格子里面
        /// </summary>
        public Boolean IsCell { get; set; }

        #endregion

        #region Fields

        private Canvas panel;
        private UIElement tl, t, tr, r, br, b, bl, l;
        private MatrixTransform trans;
        private Point point;
        private Boolean isMouseWheel;
        private Double minSize = 60;

        #endregion
    }
}
