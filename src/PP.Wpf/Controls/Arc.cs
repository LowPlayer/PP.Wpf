using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 圆弧
    /// </summary>
    public sealed class Arc : Shape
    {
        #region DependencyProperties

        public static readonly DependencyProperty RadianStartProperty = DependencyProperty.Register("RadianStart", typeof(Double), typeof(Arc), new FrameworkPropertyMetadata(-90d, FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(OnAutoAnimatablePropertyChangedCallback)));

        private static void OnAutoAnimatablePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is Double oldVal && e.NewValue is Double newVal)
                ((Arc)d).AutoAnimate(e.Property, oldVal, newVal);
        }

        /// <summary>
        /// 开始弧度
        /// </summary>
        public Double RadianStart { get => (Double)GetValue(RadianStartProperty); set => SetValue(RadianStartProperty, value); }



        public static readonly DependencyProperty RadianProperty = DependencyProperty.Register("Radian", typeof(Double), typeof(Arc), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(OnAutoAnimatablePropertyChangedCallback)));

        /// <summary>
        /// 弧度
        /// </summary>
        public Double Radian { get => (Double)GetValue(RadianProperty); set => SetValue(RadianProperty, value); }



        public static readonly DependencyProperty IsSectorProperty = DependencyProperty.Register("IsSector", typeof(Boolean), typeof(Arc), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 是否扇形
        /// </summary>
        public Boolean IsSector { get => (Boolean)GetValue(IsSectorProperty); set => SetValue(IsSectorProperty, value); }



        public static readonly DependencyProperty IsAutoAnimateProperty = DependencyProperty.Register("IsAutoAnimate", typeof(Boolean), typeof(Arc), new PropertyMetadata(new PropertyChangedCallback(OnIsAutoAnimatePropertyChangedCallback)));

        private static void OnIsAutoAnimatePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(Boolean)e.NewValue)
            {
                var arc = ((Arc)d);
                var dic = arc.autoAnimateDic;

                foreach (var dp in dic.Keys)
                {
                    arc.BeginAnimation(dp, null);
                }

                dic.Clear();
            }
        }

        /// <summary>
        /// 自动使用动画
        /// </summary>
        public Boolean IsAutoAnimate { get => (Boolean)GetValue(IsAutoAnimateProperty); set => SetValue(IsAutoAnimateProperty, value); }

        #endregion

        #region Method

        private Geometry DrawGeometry()
        {
            // 圆心
            var cx = RenderSize.Width / 2;
            var cy = RenderSize.Height / 2;

            // 若控件大小为0，或弧度为0，则不绘制
            if (cx == 0 || cy == 0 || Radian == 0)
                return Geometry.Empty;

            var r = Math.Min(cx, cy) - StrokeThickness / 2;  // 半径
            var d = 2 * r; // 直径

            // 若弧度恰好为一个整圆，则绘制两个半圆
            if (Radian % 360 == 0)
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
                var e = CoordMap(cx, cy, r, RadianStart + Radian);

                // 判断是否为大圆
                var lenghty = Radian % 360 > 180 ? 1 : 0;

                string desc = null;

                // 判断是否扇形
                if (IsSector)
                    desc = $"M0 0 M{d} {d} M{cx} {cy} L{s.X} {s.Y} A{r} {r} 0 {lenghty} 1 {e.X} {e.Y} Z";
                else
                    desc = $"M0 0 M{d} {d} M{s.X} {s.Y} A{r} {r} 0 {lenghty} 1 {e.X} {e.Y}";

                var geometry = Geometry.Parse(desc);
                geometry.Freeze();

                return geometry;
            }
        }
        /// <summary>
        /// 极坐标转换
        /// </summary>
        /// <param name="x">圆心x</param>
        /// <param name="y">圆心y</param>
        /// <param name="r">半径</param>
        /// <param name="a">弧度</param>
        /// <returns></returns>
        private Point CoordMap(Double x, Double y, Double r, Double a)
        {
            var ta = (360 - a) * Math.PI / 180;
            var tx = r * Math.Cos(ta); // 角度邻边
            var ty = r * Math.Sin(ta); // 角度的对边
            return new Point(x + tx, y - ty);
        }

        private void AutoAnimate(DependencyProperty dp, Double oldVal, Double newVal)
        {
            if (!IsAutoAnimate || autoAnimateDic.ContainsKey(dp) || Double.IsNaN(oldVal) || Double.IsInfinity(oldVal) || Double.IsNaN(newVal) || Double.IsInfinity(newVal))
                return;

            var ani = new DoubleAnimation
            {
                From = oldVal,
                To = newVal,
                Duration = TimeSpan.FromMilliseconds(300),
                FillBehavior = FillBehavior.Stop
            };

            ani.Completed += OnAnimationCompleted;

            void OnAnimationCompleted(object sender, EventArgs e)
            {
                autoAnimateDic.Remove(dp);
            }

            BeginAnimation(dp, ani, HandoffBehavior.SnapshotAndReplace);

            autoAnimateDic.Add(dp, ani);
        }

        #endregion

        #region Property

        protected override Geometry DefiningGeometry => DrawGeometry();

        #endregion

        #region Field

        private Dictionary<DependencyProperty, AnimationTimeline> autoAnimateDic = new Dictionary<DependencyProperty, AnimationTimeline>();

        #endregion
    }
}
