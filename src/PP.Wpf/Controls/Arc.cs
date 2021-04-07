using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 圆弧
    /// </summary>
    public sealed class Arc : Shape
    {
        #region DependencyProperties

        public static readonly DependencyProperty RadianStartProperty = DependencyProperty.Register("RadianStart", typeof(Double), typeof(Arc)
            , new FrameworkPropertyMetadata(-90d, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnRadianStartPropertyChanged)));

        private static void OnRadianStartPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Arc)d).DrawGeometry();
        }

        /// <summary>
        /// 开始弧度
        /// </summary>
        public Double RadianStart { get => (Double)GetValue(RadianStartProperty); set => SetValue(RadianStartProperty, value); }

        public static readonly DependencyProperty RadianEndProperty = DependencyProperty.Register("RadianEnd", typeof(Double), typeof(Arc)
            , new FrameworkPropertyMetadata(-90d, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnRadianEndPropertyChanged)));

        private static void OnRadianEndPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Arc)d).DrawGeometry();
        }

        /// <summary>
        /// 结束弧度
        /// </summary>
        public Double RadianEnd { get => (Double)GetValue(RadianEndProperty); set => SetValue(RadianEndProperty, value); }

        #endregion

        public Arc()
        {
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DrawGeometry();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawGeometry();
        }

        private void DrawGeometry()
        {
            // 圆心
            var cx = ActualWidth / 2;
            var cy = ActualHeight / 2;

            if (cx == 0 || cy == 0 || RadianStart == RadianEnd)
            {
                geometry = new StreamGeometry();
                return;
            }

            // 半径
            var r = Math.Min(cx, cy);
            var d = 2 * r;

            if ((RadianEnd - RadianStart) % 360 == 0)
            {
                var desc = $"M0 0 M{d} {d} M{r} 0 A{r} {r} 0 0 1 {r} {d} M{r} 0 A{r} {r} 0 0 0 {r} {d}";
                geometry = Geometry.Parse(desc);
                geometry.Freeze();
            }
            else
            {
                // 计算开始、结束弧度坐标点
                var s = CoordMap(cx, cy, r, RadianStart);
                var e = CoordMap(cx, cy, r, RadianEnd);

                var lenghty = (RadianEnd - RadianStart) % 360 > 180 ? 1 : 0;
                var desc = $"M0 0 M{2 * r} {2 * r} M{s.X} {s.Y} A{r} {r} 0 {lenghty} 1 {e.X} {e.Y}";

                geometry = Geometry.Parse(desc);
                geometry.Freeze();
            }
        }

        private Point CoordMap(Double x, Double y, Double r, Double a)
        {
            var ta = (360 - a) * Math.PI / 180;
            var tx = r * Math.Cos(ta); // 角度邻边
            var ty = r * Math.Sin(ta); // 角度的对边
            return new Point(x + tx, y - ty);
        }

        protected override Geometry DefiningGeometry => geometry;

        private Geometry geometry = new StreamGeometry();
    }
}
