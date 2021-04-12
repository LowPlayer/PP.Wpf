using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PP.Wpf.Behaviors
{
    public sealed class TextBeatingBehavior : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.IsVisibleChanged += OnIsVisibleChanged;
            descriptor.AddValueChanged(this.AssociatedObject, OnTextPropertyChanged);

            if (this.AssociatedObject.IsVisible && !String.IsNullOrEmpty(this.AssociatedObject.Text))
                OnTextPropertyChanged(null, null);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.IsVisibleChanged -= OnIsVisibleChanged;
            descriptor.RemoveValueChanged(this.AssociatedObject, OnTextPropertyChanged);

            if (sb != null)
            {
                //移除动画
                sb.Stop();
                sb.Remove();
                sb = null;
            }
        }

        #region 私有方法

        private void OnIsVisibleChanged(Object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((Boolean)e.NewValue)
            {
                if (sb == null)
                    CreateStoryboard();

                sb?.Begin();
            }
            else
                sb?.Stop();
        }

        /// <summary>
        /// 创建动画
        /// </summary>
        private void CreateStoryboard()
        {
            var tb = this.AssociatedObject;

            if (sb != null)
            {
                //移除旧动画
                sb.Stop();
                sb.Remove();
                sb = null;
            }

            var text = tb.Text;

            tb.TextEffects.Clear();

            if (String.IsNullOrWhiteSpace(text))
                return;

            sb = new Storyboard { RepeatBehavior = RepeatBehavior.Forever };

            var duration = TimeSpan.FromMilliseconds(Interval / 2);
            var interval = Interval / 3;

            var index = 0;

            for (var i = 0; i < text.Length; i++)
            {
                if (Char.IsWhiteSpace(text, i) || Char.IsControl(text, i))
                    continue;

                tb.TextEffects.Add(new TextEffect
                {
                    PositionStart = i,
                    PositionCount = 1,
                    Transform = new TranslateTransform()
                });

                var ani = new DoubleAnimation
                {
                    From = 0,
                    To = -Scale * tb.FontSize,
                    Duration = duration,
                    BeginTime = TimeSpan.FromMilliseconds(interval * index),
                    AutoReverse = true,
                    EasingFunction = new CircleEase { EasingMode = EasingMode.EaseOut }
                };

                Storyboard.SetTarget(ani, tb);
                Storyboard.SetTargetProperty(ani, new PropertyPath($"TextEffects[{index}].Transform.Y"));

                sb.Children.Add(ani);

                index++;
            }
        }

        private void OnTextPropertyChanged(Object sender, EventArgs e)
        {
            if (!this.AssociatedObject.IsVisible)
                return;

            sb?.Stop();
            CreateStoryboard();
            sb?.Begin();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 一个跳动的时间，单位毫秒
        /// </summary>
        public Double Interval { get; set; } = 300;

        public Double Scale { get; set; } = 1;

        #endregion

        #region 字段

        private Storyboard sb;
        private DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(TextBlock));

        #endregion
    }
}
