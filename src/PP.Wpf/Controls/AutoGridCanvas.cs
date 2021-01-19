using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 自动分栏且子项可拖动
    /// </summary>
    [ContentProperty]
    public class AutoGridCanvas : Canvas
    {
        #region DependencyProperties

        #region ItemsSource

        /// <summary>
        /// 数据项
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ICollection), typeof(AutoGridCanvas), new PropertyMetadata(OnItemsSourcePropertyChanged));
        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoGridCanvas)d).OnItemsSourceChanged((ICollection)e.OldValue, (ICollection)e.NewValue);
        }
        /// <summary>
        /// 数据项
        /// </summary>
        public ICollection ItemsSource { get => (ICollection)GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }



        /// <summary>
        /// 子项数据模板
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty = ListBox.ItemTemplateProperty.AddOwner(typeof(AutoGridCanvas), new PropertyMetadata(OnItemTemplatePropertyChanged));
        private static void OnItemTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var children = ((AutoGridCanvas)d).Children;

            for (var i = children.Count - 1; i > 0; i--)
            {
                var item = children[i];
                item.SetCurrentValue(ContentControl.ContentTemplateProperty, e.NewValue);
            }
        }
        /// <summary>
        /// 子项数据模板
        /// </summary>
        public DataTemplate ItemTemplate { get => (DataTemplate)GetValue(ItemTemplateProperty); set => SetValue(ItemTemplateProperty, value); }

        #endregion

        #region GridLine

        /// <summary>
        /// 网络线颜色
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = Line.StrokeProperty.AddOwner(typeof(AutoGridCanvas), new PropertyMetadata(OnStrokePropertyChanged));
        private static void OnStrokePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoGridCanvas)d).UpdateGridLineStroke();
        }
        /// <summary>
        /// 网络线颜色
        /// </summary>
        public Brush Stroke { get => (Brush)GetValue(StrokeProperty); set => SetValue(StrokeProperty, value); }



        /// <summary>
        /// 网络线大小
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = Line.StrokeThicknessProperty.AddOwner(typeof(AutoGridCanvas), new PropertyMetadata(OnStrokeThicknessPropertyChanged));
        private static void OnStrokeThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoGridCanvas)d).UpdateGridLineStrokeThickness();
        }
        /// <summary>
        /// 网络线大小
        /// </summary>
        public Double StrokeThickness { get => (Double)GetValue(StrokeThicknessProperty); set => SetValue(StrokeThicknessProperty, value); }

        #endregion

        #region Rows Columns

        /// <summary>
        /// 行数
        /// </summary>
        public static readonly DependencyPropertyKey RowsPropertyKey = DependencyProperty.RegisterReadOnly("Rows", typeof(Int32), typeof(AutoGridCanvas), new PropertyMetadata(1));
        /// <summary>
        /// 行数
        /// </summary>
        public static readonly DependencyProperty RowsProperty = RowsPropertyKey.DependencyProperty;
        /// <summary>
        /// 行数
        /// </summary>
        public Int32 Rows { get => (Int32)GetValue(RowsProperty); protected set => SetValue(RowsPropertyKey, value); }



        /// <summary>
        /// 列数
        /// </summary>
        public static readonly DependencyPropertyKey ColumnsPropertyKey = DependencyProperty.RegisterReadOnly("Columns", typeof(Int32), typeof(AutoGridCanvas), new PropertyMetadata(1));
        /// <summary>
        /// 列数
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty = ColumnsPropertyKey.DependencyProperty;
        /// <summary>
        /// 列数
        /// </summary>
        public Int32 Columns { get => (Int32)GetValue(ColumnsProperty); protected set => SetValue(ColumnsPropertyKey, value); }

        #endregion

        #endregion

        static AutoGridCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoGridCanvas), new FrameworkPropertyMetadata(typeof(AutoGridCanvas)));
        }

        /// <summary>
        /// 自动分栏且子项可拖动
        /// </summary>
        public AutoGridCanvas()
        {
            lineGrid = new Grid();

            lineGrid.SetBinding(WidthProperty, new Binding { Source = this, Path = new PropertyPath(ActualWidthProperty), Mode = BindingMode.OneWay });
            lineGrid.SetBinding(HeightProperty, new Binding { Source = this, Path = new PropertyPath(ActualHeightProperty), Mode = BindingMode.OneWay });

            Children.Add(lineGrid);

            SizeChanged += OnSizeChanged;
            Loaded += OnLoaded;
        }

        #region Methods

        private void OnLoaded(Object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            Reset();
        }

        private void OnSizeChanged(Object sender, SizeChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            sizeChagned_action = DoSizeChanged;

            this.Dispatcher.InvokeAsync(() =>
            {
                if (sizeChagned_action != null)
                {
                    sizeChagned_action.Invoke();
                    sizeChagned_action = null;
                }

            }, DispatcherPriority.Background);
        }

        private void DoSizeChanged()
        {
            GetRowsAndColumns(Count, out Int32 rows, out Int32 columns);

            if (rows != Rows || columns != Columns)
            {
                SetValue(RowsPropertyKey, rows);
                SetValue(ColumnsPropertyKey, columns);

                DrawGridLine();
            }

            UpdateItemsPosition();
        }

        private void OnItemsSourceChanged(ICollection olds, ICollection news)
        {
            if (olds is INotifyCollectionChanged old_oc)
                old_oc.CollectionChanged -= OnCollectionChanged;

            if (news is INotifyCollectionChanged new_oc)
                new_oc.CollectionChanged += OnCollectionChanged;

            count = news == null ? 0 : news.Count;

            if (!IsLoaded)
                return;

            Reset();
        }

        private void OnCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            count = ItemsSource.Count;

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
                SetValue(RowsPropertyKey, rows);
                SetValue(ColumnsPropertyKey, columns);

                DrawGridLine();
            }

            DrawItems();
        }

        private void Reset()
        {
            GetRowsAndColumns(Count, out Int32 rows, out Int32 columns);

            SetValue(RowsPropertyKey, rows);
            SetValue(ColumnsPropertyKey, columns);

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

            var ratio = ActualWidth / ActualHeight;

            var sum = rows * columns;

            while (sum < count)
            {
                if (columns / (Double)rows <= ratio)
                    columns++;
                else
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
            for (var i = Children.Count - 1; i > 0; i--)
            {
                Children.RemoveAt(i);
            }

            if (Count == 0)
                return;

            var index = 0;

            foreach (var data in ItemsSource)
            {
                var row = index / Columns;
                var column = index % Columns;

                var item = new AutoGridCanvasItem(this, new Rect(column, row, 1, 1), true)
                {
                    DataContext = data,
                    Content = data,
                    ContentTemplate = ItemTemplate
                };

                Grid.SetRow(item, row);
                Grid.SetColumn(item, column);
                Grid.SetRowSpan(item, 1);
                Grid.SetColumnSpan(item, 1);

                Children.Add(item);
                index++;
            }

            UpdateItemsPosition();
        }

        /// <summary>
        /// 更新子项位置
        /// </summary>
        private void UpdateItemsPosition()
        {
            var size = RenderSize;
            var columns = Columns;
            var rows = Rows;
            var line = StrokeThickness;
            var cellW = (size.Width - line * (columns - 1)) / columns;
            var cellH = (size.Height - line * (rows - 1)) / rows;

            for (var i = Children.Count - 1; i > 0; i--)
            {
                var item = (AutoGridCanvasItem)Children[i];
                Double x = 0, y = 0, w = 0, h = 0;

                if (item.IsCell)
                {
                    var row = Grid.GetRow(item);
                    var column = Grid.GetColumn(item);
                    var rowSpan = Grid.GetRowSpan(item);
                    var columnSpan = Grid.GetColumnSpan(item);

                    x = column * (cellW + line);
                    y = row * (cellH + line);
                    w = columnSpan * cellW + (columnSpan - 1) * line;
                    h = rowSpan * cellH + (rowSpan - 1) * line;
                }
                else
                {
                    var rect = item.Rect;

                    x = rect.X * size.Width / columns;
                    y = rect.Y * size.Height / rows;
                    w = rect.Width * size.Width / columns;
                    h = rect.Height * size.Height / rows;
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

        /// <summary>
        /// 数据项数量
        /// </summary>
        public Int32 Count => count;

        #endregion

        #region Fields

        private Action collection_action;
        private Action position_action;
        private Action sizeChagned_action;
        private Grid lineGrid;
        private Int32 count;

        #endregion
    }



    /// <summary>
    /// AutoGridCanvas的数据项
    /// </summary>
    [TemplatePart(Name = "PART_ResizeTopLeft", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_ResizeTop", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_ResizeTopRight", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_ResizeRight", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_ResizeBottomRight", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_ResizeBottom", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_ResizeBottomLeft", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_ResizeLeft", Type = typeof(UIElement))]
    [ContentProperty]
    public sealed class AutoGridCanvasItem : ContentControl
    {
        #region DependencyProperties

        /// <summary>
        /// 改变大小边框的边宽
        /// </summary>
        public static readonly DependencyProperty ResizeBorderThicknessProperty = DependencyProperty.Register("ResizeBorderThickness", typeof(Double), typeof(AutoGridCanvasItem), new PropertyMetadata(6d));
        /// <summary>
        /// 改变大小边框的边宽
        /// </summary>
        public Double ResizeBorderThickness { get => (Double)GetValue(ResizeBorderThicknessProperty); set => SetValue(ResizeBorderThicknessProperty, value); }

        #endregion

        static AutoGridCanvasItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoGridCanvasItem), new FrameworkPropertyMetadata(typeof(AutoGridCanvasItem)));
        }

        /// <summary>
        /// AutoGridCanvas的数据项
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="rect"></param>
        /// <param name="isCell"></param>
        public AutoGridCanvasItem(AutoGridCanvas panel, Rect rect, Boolean isCell)
        {
            this.panel = panel;
            this.rect = rect;
            this.isCell = isCell;
            this.RenderTransform = trans = new MatrixTransform();
        }

        #region Methods

        #region Override Methods

        /// <summary>
        /// 应用模板
        /// </summary>
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

        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            SetZIndex();

            point = e.GetPosition(panel);
            e.Handled = this.CaptureMouse();
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="e"></param>
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

        /// <summary>
        /// 鼠标左键弹起
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (IsMouseCaptured)
            {
                this.ReleaseMouseCapture();
                ClearMatrix();
            }
        }

        /// <summary>
        /// 鼠标滚轮滚动
        /// </summary>
        /// <param name="e"></param>
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
            var brothers = panel.Children.Cast<UIElement>().Where(q => !Object.ReferenceEquals(q, this)).OrderBy(q => Panel.GetZIndex(q)).ToList();

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
                var bound = panel.RenderSize;
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
            var columns = panel.Columns;
            var rows = panel.Rows;

            var lenX = (width * 0.05) * columns / bound.Width;
            var lenY = (height * 0.05) * rows / bound.Height;

            var rectX = left * columns / bound.Width;
            var rectY = top * rows / bound.Height;
            var rectW = width * columns / bound.Width;
            var rectH = height * rows / bound.Height;

            var column = (Int32)Math.Round(rectX);
            var row = (Int32)Math.Round(rectY);
            var columnSpan = (Int32)Math.Round(rectW);
            var rowSpan = (Int32)Math.Round(rectH);

            if (Math.Abs(column - rectX) > lenX || Math.Abs(row - rectY) > lenY
                || Math.Abs(columnSpan - rectW) > lenX * 2 || Math.Abs(rowSpan - rectH) > lenY * 2)
            {
                rect = new Rect(rectX, rectY, rectW, rectH);
                isCell = false;
            }
            else
            {
                rect = new Rect(column, row, columnSpan, rowSpan);
                isCell = true;

                Grid.SetRow(this, row);
                Grid.SetColumn(this, column);
                Grid.SetRowSpan(this, rowSpan);
                Grid.SetColumnSpan(this, columnSpan);

                var line = panel.StrokeThickness;
                var cellW = (bound.Width - line * (columns - 1)) / columns;
                var cellH = (bound.Height - line * (rows - 1)) / rows;

                left = column * (cellW + line);
                top = row * (cellH + line);
                width = columnSpan * cellW + (columnSpan - 1) * line;
                height = rowSpan * cellH + (rowSpan - 1) * line;
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

            var bound = panel.RenderSize;
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

        /// <summary>
        /// 相对区域
        /// </summary>
        public Rect Rect => rect;
        /// <summary>
        /// 是否在格子里面
        /// </summary>
        public Boolean IsCell => isCell;

        #endregion

        #region Fields

        private AutoGridCanvas panel;
        private Rect rect;          // 相对区域
        private Boolean isCell;     // 是否完全在格子里面
        private UIElement tl, t, tr, r, br, b, bl, l;
        private MatrixTransform trans;
        private Point point;
        private Boolean isMouseWheel;
        private Double minSize = 60;

        #endregion
    }
}
