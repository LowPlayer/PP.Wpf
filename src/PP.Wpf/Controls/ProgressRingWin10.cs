using System;
using System.Windows;
using System.Windows.Controls;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// Win10加载动画
    /// </summary>
    [TemplateVisualState(Name = "Large", GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Small", GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Inactive", GroupName = "ActiveStates")]
    [TemplateVisualState(Name = "Active", GroupName = "ActiveStates")]
    public sealed class ProgressRingWin10 : Control
    {
        static ProgressRingWin10()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRingWin10), new FrameworkPropertyMetadata(typeof(ProgressRingWin10)));
        }

        /// <summary>
        /// Win10加载动画
        /// </summary>
        public ProgressRingWin10()
        {
            this.IsHitTestVisible = false;
            this.SizeChanged += OnSizeChanged;
            this.IsVisibleChanged += OnIsVisibleChanged;
        }

        public override void OnApplyTemplate()
        {
            //make sure the states get updated
            UpdateLargeState();
            UpdateActiveState();

            base.OnApplyTemplate();
        }

        #region 依赖属性

        public static readonly DependencyPropertyKey BindableWidthPropertyKey = DependencyProperty.RegisterReadOnly("BindableWidth", typeof(Double), typeof(ProgressRingWin10), new PropertyMetadata(OnBindableWidthPropertyChangedCallback));

        public static readonly DependencyProperty BindableWidthProperty = BindableWidthPropertyKey.DependencyProperty;

        public Double BindableWidth { get => (Double)GetValue(BindableWidthProperty); private set => SetValue(BindableWidthPropertyKey, value); }

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(Boolean), typeof(ProgressRingWin10), new PropertyMetadata(true, OnIsActivePropertyChanged));

        public Boolean IsActive { get => (Boolean)GetValue(IsActiveProperty); set => SetValue(IsActiveProperty, value); }

        public static readonly DependencyProperty IsLargeProperty = DependencyProperty.Register("IsLarge", typeof(Boolean), typeof(ProgressRingWin10), new PropertyMetadata(true, OnIsLargePropertyChangedCallback));

        public Boolean IsLarge { get => (Boolean)GetValue(IsLargeProperty); set => SetValue(IsLargeProperty, value); }

        public static readonly DependencyPropertyKey MaxSideLengthPropertyKey = DependencyProperty.RegisterReadOnly("MaxSideLength", typeof(Double), typeof(ProgressRingWin10), new PropertyMetadata(default));

        public static readonly DependencyProperty MaxSideLengthProperty = MaxSideLengthPropertyKey.DependencyProperty;

        public Double MaxSideLength { get => (Double)GetValue(MaxSideLengthProperty); private set => SetValue(MaxSideLengthPropertyKey, value); }

        public static readonly DependencyPropertyKey EllipseDiameterPropertyKey = DependencyProperty.RegisterReadOnly("EllipseDiameter", typeof(Double), typeof(ProgressRingWin10), new PropertyMetadata(default));

        public static readonly DependencyProperty EllipseDiameterProperty = EllipseDiameterPropertyKey.DependencyProperty;

        public Double EllipseDiameter { get => (Double)GetValue(EllipseDiameterProperty); private set => SetValue(EllipseDiameterPropertyKey, value); }

        public static readonly DependencyPropertyKey EllipseOffsetPropertyKey = DependencyProperty.RegisterReadOnly("EllipseOffset", typeof(Thickness), typeof(ProgressRingWin10), new PropertyMetadata(default));

        public static readonly DependencyProperty EllipseOffsetProperty = EllipseOffsetPropertyKey.DependencyProperty;

        public Thickness EllipseOffset { get => (Thickness)GetValue(EllipseOffsetProperty); private set => SetValue(EllipseOffsetPropertyKey, value); }

        public static readonly DependencyProperty EllipseDiameterScaleProperty = DependencyProperty.Register("EllipseDiameterScale", typeof(Double), typeof(ProgressRingWin10), new PropertyMetadata(1D));

        public Double EllipseDiameterScale { get => (Double)GetValue(EllipseDiameterScaleProperty); set => SetValue(EllipseDiameterScaleProperty, value); }

        #endregion

        #region 私有方法

        private static void OnBindableWidthPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ring = (ProgressRingWin10)d;
            var val = (Double)e.NewValue;

            ring.SetEllipseDiameter(val);
            ring.SetEllipseOffset(val);
            ring.SetMaxSideLength(val);
        }

        private static void OnIsLargePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProgressRingWin10)d).UpdateLargeState();
        }

        private static void OnIsActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProgressRingWin10)d).UpdateActiveState();
        }

        private void SetMaxSideLength(Double width)
        {
            MaxSideLength = width <= 20 ? 20 : width;
        }

        private void SetEllipseDiameter(Double width)
        {
            EllipseDiameter = (width / 8) * EllipseDiameterScale;
        }

        private void SetEllipseOffset(Double width)
        {
            EllipseOffset = new Thickness(0, width / 2, 0, 0);
        }

        private void UpdateLargeState()
        {
            VisualStateManager.GoToState(this, IsLarge ? "Large" : "Small", true);
        }

        private void OnSizeChanged(Object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            BindableWidth = ActualWidth;
        }

        private void OnIsVisibleChanged(Object sender, DependencyPropertyChangedEventArgs e)
        {
            ((ProgressRingWin10)sender).SetCurrentValue(IsActiveProperty, e.NewValue);
        }

        private void UpdateActiveState()
        {
            VisualStateManager.GoToState(this, IsActive ? "Active" : "Inactive", true);
        }

        #endregion
    }
}
