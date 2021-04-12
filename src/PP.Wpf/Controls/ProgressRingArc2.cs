using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PP.Wpf.Controls
{
    [TemplateVisualState(Name = "Inactive", GroupName = "ActiveStates")]
    [TemplateVisualState(Name = "Active", GroupName = "ActiveStates")]
    public sealed class ProgressRingArc2 : Control
    {
        static ProgressRingArc2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRingArc2), new FrameworkPropertyMetadata(typeof(ProgressRingArc2)));
        }

        public ProgressRingArc2()
        {
            this.IsHitTestVisible = false;
            this.IsVisibleChanged += OnIsVisibleChanged;
        }

        public override void OnApplyTemplate()
        {
            //make sure the states get updated
            UpdateActiveState();
            base.OnApplyTemplate();
        }

        #region 依赖属性

        public static readonly DependencyProperty IsActiveProperty = ProgressRingWin10.IsActiveProperty.AddOwner(typeof(ProgressRingArc2), new PropertyMetadata(true, OnIsActivePropertyChanged));

        public Boolean IsActive { get => (Boolean)GetValue(IsActiveProperty); set => SetValue(IsActiveProperty, value); }

        public static readonly DependencyProperty StrokeThicknessProperty = Shape.StrokeThicknessProperty.AddOwner(typeof(ProgressRingArc2), new PropertyMetadata(5d));

        public Double StrokeThickness { get => (Double)GetValue(StrokeThicknessProperty); set => SetValue(StrokeThicknessProperty, value); }

        public static readonly DependencyProperty StrokeProperty = Shape.StrokeProperty.AddOwner(typeof(ProgressRingArc2));

        public Brush Stroke { get => (Brush)GetValue(StrokeProperty); set => SetValue(StrokeProperty, value); }

        #endregion

        private void OnIsVisibleChanged(Object sender, DependencyPropertyChangedEventArgs e)
        {
            ((ProgressRingArc2)sender).SetCurrentValue(IsActiveProperty, e.NewValue);
        }

        private static void OnIsActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProgressRingArc2)d).UpdateActiveState();
        }

        private void UpdateActiveState()
        {
            VisualStateManager.GoToState(this, IsActive ? "Active" : "Inactive", true);
        }
    }
}
