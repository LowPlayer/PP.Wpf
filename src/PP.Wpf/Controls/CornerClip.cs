using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 圆角裁剪
    /// </summary>
    public class CornerClip : Decorator
    {
        #region DependencyProperties

        /// <summary>
        /// 圆角弧度
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = Border.CornerRadiusProperty.AddOwner(typeof(CornerClip), new PropertyMetadata(OnCornerRadiusPropertyChanged));

        private static void OnCornerRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CornerClip)d).BeginSetClip();
        }

        /// <summary>
        /// 圆角弧度
        /// </summary>
        public CornerRadius CornerRadius { get => (CornerRadius)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }

        #endregion

        /// <summary>
        /// 圆角裁剪
        /// </summary>
        public CornerClip()
        {
            this.Loaded += OnLoaded;
            this.SizeChanged += OnSizeChanged;
        }

        #region Methods

        private void OnLoaded(Object sender, RoutedEventArgs e)
        {
            BeginSetClip();
        }

        private void OnSizeChanged(Object sender, SizeChangedEventArgs e)
        {
            BeginSetClip();
        }

        private void BeginSetClip()
        {
            delayAction = SetClip;

            Dispatcher.InvokeAsync(() =>
            {
                if (delayAction != null)
                {
                    delayAction.Invoke();
                    delayAction = null;
                }
            }, System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private void SetClip()
        {
            var cr = CornerRadius;

            if (cr.TopLeft == 0 && cr.TopRight == 0 && cr.BottomRight == 0 && cr.BottomLeft == 0)
                Clip = null;
            else
            {
                var w = RenderSize.Width;
                var h = RenderSize.Height;

                Double tll, tlt, trt, trr, brr, brb, blb, bll;
                tll = tlt = cr.TopLeft;
                trt = trr = cr.TopRight;
                brr = brb = cr.BottomRight;
                blb = bll = cr.BottomLeft;

                if (tlt + trt > w)
                {
                    var a = tlt;
                    var b = trt;
                    var c = tlt + trt;
                    tlt = Math.Round(w * a / c);
                    trt = Math.Round(w * b / c);
                }
                if (blb + brb > w)
                {
                    var a = blb;
                    var b = brb;
                    var c = blb + brb;
                    blb = Math.Round(w * a / c);
                    brb = Math.Round(w * b / c);
                }
                if (trr + brr > h)
                {
                    var a = trr;
                    var b = brr;
                    var c = trr + brr;
                    trr = Math.Round(h * a / c);
                    brr = Math.Round(h * b / c);
                }
                if (tll + bll > h)
                {
                    var a = tll;
                    var b = bll;
                    var c = tll + bll;
                    tll = Math.Round(h * a / c);
                    bll = Math.Round(h * b / c);
                }

                Clip = Geometry.Parse($"M0 {tll} Q0 0 {tlt} 0 H{w - trt} Q{w} 0 {w} {trr} V{h - brr} Q{w} {h} {w - brb} {h} H{blb} Q0 {h} 0 {h - bll} Z");
            }
        }

        #endregion

        #region Fields

        private Action delayAction;

        #endregion
    }
}
