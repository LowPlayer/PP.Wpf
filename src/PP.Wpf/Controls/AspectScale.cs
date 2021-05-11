using System;
using System.Windows;
using System.Windows.Controls;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 纵横比
    /// </summary>
    public class AspectScale : Decorator
    {
        #region DependencyProperty

        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(Int32), typeof(AspectScale), new FrameworkPropertyMetadata { AffectsMeasure = true, AffectsRender = true, AffectsParentMeasure = true, AffectsParentArrange = true });
        /// <summary>
        /// 宽比例
        /// </summary>
        public Int32 X { get => (Int32)GetValue(XProperty); set => SetValue(XProperty, value); }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(Int32), typeof(AspectScale), new FrameworkPropertyMetadata { AffectsMeasure = true, AffectsRender = true, AffectsParentMeasure = true, AffectsParentArrange = true });
        /// <summary>
        /// 高比例
        /// </summary>
        public Int32 Y { get => (Int32)GetValue(YProperty); set => SetValue(YProperty, value); }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(MeasureMode), typeof(AspectScale), new FrameworkPropertyMetadata { AffectsMeasure = true, AffectsRender = true, AffectsParentMeasure = true, AffectsParentArrange = true });
        /// <summary>
        /// 测量模式
        /// </summary>
        public MeasureMode Mode { get => (MeasureMode)GetValue(ModeProperty); set => SetValue(ModeProperty, value); }

        #endregion

        #region 

        protected override Size MeasureOverride(Size constraint)
        {
            if (X <= 0 || Y <= 0)
                return base.MeasureOverride(constraint);

            switch (Mode)
            {
                case MeasureMode.ByWidth:
                    constraint.Height = Y * constraint.Width / X;
                    break;
                case MeasureMode.ByHeight:
                    constraint.Width = X * constraint.Height / Y;
                    break;
                default:
                    if (Double.IsInfinity(constraint.Width))
                        constraint.Width = 0;

                    if (Double.IsInfinity(constraint.Height))
                        constraint.Height = 0;

                    var isbigger = Mode == MeasureMode.Bigger;

                    var _width = X * constraint.Height / Y;

                    if (isbigger ^ (_width <= constraint.Width))
                        constraint.Width = _width;
                    else
                        constraint.Height = Y * constraint.Width / X;

                    if (!isbigger)
                        constraint = base.MeasureOverride(constraint);
                    break;
            }

            return constraint;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            if (X <= 0 || Y <= 0)
                return base.ArrangeOverride(arrangeBounds);

            switch (Mode)
            {
                case MeasureMode.ByWidth:
                    arrangeBounds.Height = Y * arrangeBounds.Width / X;
                    break;
                case MeasureMode.ByHeight:
                    arrangeBounds.Width = X * arrangeBounds.Height / Y;
                    break;
                default:
                    var isbigger = Mode == MeasureMode.Bigger;

                    var _width = X * arrangeBounds.Height / Y;

                    if (isbigger ^ (_width <= arrangeBounds.Width))
                        arrangeBounds.Width = _width;
                    else
                        arrangeBounds.Height = Y * arrangeBounds.Width / X;
                    break;
            }

            return base.ArrangeOverride(arrangeBounds);
        }

        #endregion

        public enum MeasureMode
        {
            Smaller,
            Bigger,
            ByWidth,
            ByHeight
        }
    }
}
