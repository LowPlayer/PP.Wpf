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

        public static readonly DependencyProperty RadianStartProperty = DependencyProperty.Register("RadianStart", typeof(Double), typeof(Arc), new FrameworkPropertyMetadata(-90d, FrameworkPropertyMetadataOptions.AffectsRender));


        /// <summary>
        /// 开始弧度
        /// </summary>
        public Double RadianStart { get => (Double)GetValue(RadianStartProperty); set => SetValue(RadianStartProperty, value); }

        public static readonly DependencyProperty RadianEndProperty = DependencyProperty.Register("RadianEnd", typeof(Double), typeof(Arc), new FrameworkPropertyMetadata(-90d, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 结束弧度
        /// </summary>
        public Double RadianEnd { get => (Double)GetValue(RadianEndProperty); set => SetValue(RadianEndProperty, value); }

        #endregion

        private Geometry DrawGeometry()
        {
            // 圆心
            var cx = RenderSize.Width / 2;
            var cy = RenderSize.Height / 2;

            if (cx == 0 || cy == 0 || RadianStart == RadianEnd)
                return Geometry.Empty;

            var r = Math.Min(cx, cy) - StrokeThickness / 2;  // 半径
            var d = 2 * r; // 直径

            if ((RadianEnd - RadianStart) % 360 == 0)
            {
                // 计算开始、结束弧度坐标点
                var s = CoordMap(cx, cy, r, 0);
                var e = CoordMap(cx, cy, r, 180);

                var desc = $"M0 0 M{d} {d} M{s.X} {s.Y} A{r} {r} 0 0 1 {e.X} {e.Y} A{r} {r} 0 0 1 {s.X} {s.Y}";
                var geometry = Geometry.Parse(desc);
                geometry.Freeze();

                return geometry;
            }
            else
            {
                // 计算开始、结束弧度坐标点
                var s = CoordMap(cx, cy, r, RadianStart);
                var e = CoordMap(cx, cy, r, RadianEnd);

                var lenghty = (RadianEnd - RadianStart) % 360 > 180 ? 1 : 0;
                var desc = $"M0 0 M{d} {d} M{s.X} {s.Y} A{r} {r} 0 {lenghty} 1 {e.X} {e.Y}";

                var geometry = Geometry.Parse(desc);
                geometry.Freeze();

                return geometry;
            }
        }

        private Point CoordMap(Double x, Double y, Double r, Double a)
        {
            var ta = (360 - a) * Math.PI / 180;
            var tx = r * Math.Cos(ta); // 角度邻边
            var ty = r * Math.Sin(ta); // 角度的对边
            return new Point(x + tx, y - ty);
        }

        protected override Geometry DefiningGeometry => DrawGeometry();
    }
}
