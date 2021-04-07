using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PP.Wpf.Controls
{
    [TemplateVisualState(Name = "Inactive", GroupName = "ActiveStates")]
    [TemplateVisualState(Name = "Active", GroupName = "ActiveStates")]
    public sealed class ProgressRing3Color : Control
    {
        static ProgressRing3Color()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing3Color), new FrameworkPropertyMetadata(typeof(ProgressRing3Color)));
        }

        public ProgressRing3Color()
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

        public static readonly DependencyProperty IsActiveProperty = ProgressRingWin10.IsActiveProperty.AddOwner(typeof(ProgressRing3Color), new PropertyMetadata(true, OnIsActivePropertyChanged));

        public Boolean IsActive { get => (Boolean)GetValue(IsActiveProperty); set => SetValue(IsActiveProperty, value); }

        public static readonly DependencyProperty Color1Property = DependencyProperty.Register("Color1", typeof(Brush), typeof(ProgressRing3Color), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(158, 158, 158))));

        public Brush Color1 { get => (Brush)GetValue(Color1Property); set => SetValue(Color1Property, value); }

        public static readonly DependencyProperty Color2Property = DependencyProperty.Register("Color2", typeof(Brush), typeof(ProgressRing3Color), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(106, 187, 244))));

        public Brush Color2 { get => (Brush)GetValue(Color2Property); set => SetValue(Color2Property, value); }

        public static readonly DependencyProperty Color3Property = DependencyProperty.Register("Color3", typeof(Brush), typeof(ProgressRing3Color), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(230, 76, 76))));

        public Brush Color3 { get => (Brush)GetValue(Color3Property); set => SetValue(Color3Property, value); }

        #endregion

        private void OnIsVisibleChanged(Object sender, DependencyPropertyChangedEventArgs e)
        {
            ((ProgressRing3Color)sender).SetCurrentValue(IsActiveProperty, e.NewValue);
        }

        private static void OnIsActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProgressRing3Color)d).UpdateActiveState();
        }

        private void UpdateActiveState()
        {
            VisualStateManager.GoToState(this, IsActive ? "Active" : "Inactive", true);
        }
    }
}
