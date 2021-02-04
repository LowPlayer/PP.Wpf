using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 滚动文字
    /// </summary>
    [ContentProperty("Text")]
    [TemplatePart(Name = "PART_Canvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_Txt1", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_Txt2", Type = typeof(TextBlock))]
    public class RunningText : Control
    {
        #region DependencyProperties

        /// <summary>
        /// 滚动的文字
        /// </summary>
        public static readonly DependencyProperty TextProperty = TextBlock.TextProperty.AddOwner(typeof(RunningText));
        /// <summary>
        /// 滚动的文字
        /// </summary>
        public String Text { get => (String)GetValue(TextProperty); set => SetValue(TextProperty, value); }



        /// <summary>
        /// 间距
        /// </summary>
        public static readonly DependencyProperty SpaceProperty = DependencyProperty.Register("Space", typeof(Double), typeof(RunningText), new PropertyMetadata(Double.NaN, OnSpacePropertyChanged));

        private static void OnSpacePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RunningText)d).BeginUpdate();
        }

        /// <summary>
        /// 间距
        /// </summary>
        public Double Space { get => (Double)GetValue(SpaceProperty); set => SetValue(SpaceProperty, value); }



        /// <summary>
        /// 滚动速度（每秒wpf单位数）
        /// </summary>
        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register("Speed", typeof(Double), typeof(RunningText), new PropertyMetadata(120d, OnSpeedPropertyChanged));

        private static void OnSpeedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RunningText)d).BeginUpdate();
        }

        /// <summary>
        /// 滚动速度（每秒wpf单位数）
        /// </summary>
        public Double Speed { get => (Double)GetValue(SpeedProperty); set => SetValue(SpeedProperty, value); }



        /// <summary>
        /// 滚动方向
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(typeof(RunningText), new PropertyMetadata(Orientation.Horizontal, OnOrientationPropertyChanged));

        private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RunningText)d).BeginUpdate();
        }

        /// <summary>
        /// 滚动方向
        /// </summary>
        public Orientation Orientation { get => (Orientation)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

        #endregion

        static RunningText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RunningText), new FrameworkPropertyMetadata(typeof(RunningText)));        // 重写默认样式
        }

        /// <summary>
        /// 滚动文字
        /// </summary>
        public RunningText()
        {
            IsVisibleChanged += delegate { BeginUpdate(); };
        }

        #region Methods

        /// <summary>
        /// 应用模板
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_canvas != null)
                _canvas.SizeChanged -= OnSizeChanged;

            if (_txt1 != null)
                _txt1.SizeChanged -= OnSizeChanged;

            _canvas = this.Template.FindName("PART_Canvas", this) as Canvas;
            _txt1 = this.Template.FindName("PART_Txt1", this) as TextBlock;
            _txt2 = this.Template.FindName("PART_Txt2", this) as TextBlock;

            if (_canvas == null || _txt1 == null || _txt2 == null)
                return;

            _txt1.SizeChanged += OnSizeChanged;
            _canvas.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(Object sender, SizeChangedEventArgs e)
        {
            BeginUpdate();
        }

        /// <summary>
        /// 多个事件同时触发时，仅执行一次
        /// </summary>
        private void BeginUpdate()
        {
            _delayUpdate = Update;

            this.Dispatcher.InvokeAsync(() =>
            {
                if (_delayUpdate != null)
                {
                    _delayUpdate.Invoke();
                    _delayUpdate = null;
                }

            }, System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private void Update()
        {
            // 停用动画
            if (_storyboard != null)
            {
                _storyboard.Stop();
                _storyboard = null;
            }

            if (_canvas == null || _txt1 == null || _txt2 == null)
                return;

            switch (Orientation)
            {
                case Orientation.Horizontal:
                    UpdateHorizontal();
                    break;
                case Orientation.Vertical:
                    UpdateVertical();
                    break;
            }
        }

        private void UpdateHorizontal()
        {
            // 复位
            Canvas.SetLeft(_txt1, _canvas.RenderSize.Width);
            Canvas.SetLeft(_txt2, _canvas.RenderSize.Width);

            // 当不可见时，不启用动画
            if (String.IsNullOrEmpty(Text) || !IsVisible)
                return;

            // 使用新动画
            var from = _canvas.RenderSize.Width; // 起点位置
            var to = -_txt1.RenderSize.Width;    // 终点位置

            Double len;
            if (Double.IsNaN(Space) || Space < 0)
                len = _txt1.RenderSize.Width < _canvas.RenderSize.Width ? _canvas.RenderSize.Width : _txt1.RenderSize.Width + _canvas.RenderSize.Width;
            else
                len = _txt1.RenderSize.Width < _canvas.RenderSize.Width - Space ? _canvas.RenderSize.Width : _txt1.RenderSize.Width + Space;

            var begin = TimeSpan.FromSeconds(len / Speed);      // 第二个动画延迟时间
            var duration = TimeSpan.FromSeconds((from - to) / Speed);     // 动画从开始到结束的时间
            var total = begin + begin;      // 加上延迟，一次动画的时间

            _storyboard = new Storyboard();

            var ani1 = new DoubleAnimationUsingKeyFrames
            {
                Duration = total,
                RepeatBehavior = RepeatBehavior.Forever
            };

            ani1.KeyFrames.Add(new DiscreteDoubleKeyFrame(from, TimeSpan.FromSeconds(0)));
            ani1.KeyFrames.Add(new LinearDoubleKeyFrame(to, duration));

            Storyboard.SetTarget(ani1, _txt1);
            Storyboard.SetTargetProperty(ani1, new PropertyPath(Canvas.LeftProperty));

            _storyboard.Children.Add(ani1);

            var ani2 = new DoubleAnimationUsingKeyFrames()
            {
                BeginTime = begin,
                Duration = total,
                RepeatBehavior = RepeatBehavior.Forever
            };

            ani2.KeyFrames.Add(new DiscreteDoubleKeyFrame(from, TimeSpan.FromSeconds(0)));
            ani2.KeyFrames.Add(new LinearDoubleKeyFrame(to, duration));

            Storyboard.SetTarget(ani2, _txt2);
            Storyboard.SetTargetProperty(ani2, new PropertyPath(Canvas.LeftProperty));

            _storyboard.Children.Add(ani2);

            #region 不显示时隐藏，不造成性能浪费

            _txt1.SetCurrentValue(VisibilityProperty, Visibility.Hidden);
            _txt2.SetCurrentValue(VisibilityProperty, Visibility.Hidden);

            var ani11 = new ObjectAnimationUsingKeyFrames
            {
                Duration = total,
                RepeatBehavior = RepeatBehavior.Forever
            };

            ani11.KeyFrames.Add(new DiscreteObjectKeyFrame { Value = Visibility.Visible, KeyTime = TimeSpan.FromSeconds(0) });
            ani11.KeyFrames.Add(new DiscreteObjectKeyFrame { Value = Visibility.Hidden, KeyTime = duration });

            Storyboard.SetTarget(ani11, _txt1);
            Storyboard.SetTargetProperty(ani11, new PropertyPath(VisibilityProperty));

            _storyboard.Children.Add(ani11);

            var ani22 = new ObjectAnimationUsingKeyFrames
            {
                BeginTime = begin,
                Duration = total,
                RepeatBehavior = RepeatBehavior.Forever
            };

            ani22.KeyFrames.Add(new DiscreteObjectKeyFrame { Value = Visibility.Visible, KeyTime = TimeSpan.FromSeconds(0) });
            ani22.KeyFrames.Add(new DiscreteObjectKeyFrame { Value = Visibility.Hidden, KeyTime = duration });

            Storyboard.SetTarget(ani22, _txt2);
            Storyboard.SetTargetProperty(ani22, new PropertyPath(VisibilityProperty));

            _storyboard.Children.Add(ani22);

            #endregion

            _storyboard.Begin();
        }

        private void UpdateVertical()
        {
            // 复位
            Canvas.SetTop(_txt1, _canvas.RenderSize.Height);
            Canvas.SetTop(_txt2, _canvas.RenderSize.Height);

            // 当不可见时，不启用动画
            if (String.IsNullOrEmpty(Text) || !IsVisible)
                return;

            // 使用新动画
            var from = _canvas.RenderSize.Height; // 起点位置
            var to = -_txt1.RenderSize.Height;    // 终点位置

            Double len;
            if (Double.IsNaN(Space) || Space < 0)
                len = _txt1.RenderSize.Height < _canvas.RenderSize.Height ? _canvas.RenderSize.Height : _txt1.RenderSize.Height + _canvas.RenderSize.Height;
            else
                len = _txt1.RenderSize.Height < _canvas.RenderSize.Height - Space ? _canvas.RenderSize.Height : _txt1.RenderSize.Height + Space;

            var begin = TimeSpan.FromSeconds(len / Speed);      // 第二个动画延迟时间
            var duration = TimeSpan.FromSeconds((from - to) / Speed);     // 动画从开始到结束的时间
            var total = begin + begin;      // 加上延迟，一次动画的时间

            _storyboard = new Storyboard();

            var ani1 = new DoubleAnimationUsingKeyFrames
            {
                Duration = total,
                RepeatBehavior = RepeatBehavior.Forever
            };

            ani1.KeyFrames.Add(new DiscreteDoubleKeyFrame(from, TimeSpan.FromSeconds(0)));
            ani1.KeyFrames.Add(new LinearDoubleKeyFrame(to, duration));

            Storyboard.SetTarget(ani1, _txt1);
            Storyboard.SetTargetProperty(ani1, new PropertyPath(Canvas.TopProperty));

            _storyboard.Children.Add(ani1);

            var ani2 = new DoubleAnimationUsingKeyFrames()
            {
                BeginTime = begin,
                Duration = total,
                RepeatBehavior = RepeatBehavior.Forever
            };

            ani2.KeyFrames.Add(new DiscreteDoubleKeyFrame(from, TimeSpan.FromSeconds(0)));
            ani2.KeyFrames.Add(new LinearDoubleKeyFrame(to, duration));

            Storyboard.SetTarget(ani2, _txt2);
            Storyboard.SetTargetProperty(ani2, new PropertyPath(Canvas.TopProperty));

            _storyboard.Children.Add(ani2);

            #region 不显示时隐藏，不造成性能浪费

            _txt1.SetCurrentValue(VisibilityProperty, Visibility.Hidden);
            _txt2.SetCurrentValue(VisibilityProperty, Visibility.Hidden);

            var ani11 = new ObjectAnimationUsingKeyFrames
            {
                Duration = total,
                RepeatBehavior = RepeatBehavior.Forever
            };

            ani11.KeyFrames.Add(new DiscreteObjectKeyFrame { Value = Visibility.Visible, KeyTime = TimeSpan.FromSeconds(0) });
            ani11.KeyFrames.Add(new DiscreteObjectKeyFrame { Value = Visibility.Hidden, KeyTime = duration });

            Storyboard.SetTarget(ani11, _txt1);
            Storyboard.SetTargetProperty(ani11, new PropertyPath(VisibilityProperty));

            _storyboard.Children.Add(ani11);

            var ani22 = new ObjectAnimationUsingKeyFrames
            {
                BeginTime = begin,
                Duration = total,
                RepeatBehavior = RepeatBehavior.Forever
            };

            ani22.KeyFrames.Add(new DiscreteObjectKeyFrame { Value = Visibility.Visible, KeyTime = TimeSpan.FromSeconds(0) });
            ani22.KeyFrames.Add(new DiscreteObjectKeyFrame { Value = Visibility.Hidden, KeyTime = duration });

            Storyboard.SetTarget(ani22, _txt2);
            Storyboard.SetTargetProperty(ani22, new PropertyPath(VisibilityProperty));

            _storyboard.Children.Add(ani22);

            #endregion

            _storyboard.Begin();
        }

        #endregion

        #region Fields

        private Canvas _canvas;
        private TextBlock _txt1;
        private TextBlock _txt2;
        private Action _delayUpdate;
        private Storyboard _storyboard;

        #endregion
    }
}
