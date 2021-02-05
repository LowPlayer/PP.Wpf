using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 滚动块
    /// </summary>
    [TemplatePart(Name = "PART_Canvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_Content", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "PART_Mirror", Type = typeof(FrameworkElement))]
    public class RunningBlock : ContentControl
    {
        #region DependencyProperties

        /// <summary>
        /// 间距
        /// </summary>
        public static readonly DependencyProperty SpaceProperty = RunningText.SpaceProperty.AddOwner(typeof(RunningBlock), new PropertyMetadata(Double.NaN, OnSpacePropertyChanged));

        private static void OnSpacePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RunningBlock)d).BeginUpdate();
        }

        /// <summary>
        /// 间距
        /// </summary>
        public Double Space { get => (Double)GetValue(SpaceProperty); set => SetValue(SpaceProperty, value); }



        /// <summary>
        /// 滚动速度（每秒wpf单位数）
        /// </summary>
        public static readonly DependencyProperty SpeedProperty = RunningText.SpeedProperty.AddOwner(typeof(RunningBlock), new PropertyMetadata(120d, OnSpeedPropertyChanged));

        private static void OnSpeedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RunningBlock)d).BeginUpdate();
        }

        /// <summary>
        /// 滚动速度（每秒wpf单位数）
        /// </summary>
        public Double Speed { get => (Double)GetValue(SpeedProperty); set => SetValue(SpeedProperty, value); }



        /// <summary>
        /// 滚动方向
        /// </summary>
        public static readonly DependencyProperty DirectionProperty = RunningText.DirectionProperty.AddOwner(typeof(RunningBlock), new PropertyMetadata(RunningDirection.RightToLeft, OnDirectionPropertyChanged));

        private static void OnDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RunningBlock)d).BeginUpdate();
        }

        /// <summary>
        /// 滚动方向
        /// </summary>
        public RunningDirection Direction { get => (RunningDirection)GetValue(DirectionProperty); set => SetValue(DirectionProperty, value); }

        #endregion

        static RunningBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RunningBlock), new FrameworkPropertyMetadata(typeof(RunningBlock)));        // 重写默认样式
        }

        /// <summary>
        /// 滚动块
        /// </summary>
        public RunningBlock()
        {
            IsVisibleChanged += delegate { BeginUpdate(); };
        }

        #region Methods

        /// <summary>
        /// 应用样式
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_canvas != null)
                _canvas.SizeChanged -= OnSizeChanged;

            if (_content != null)
                _content.SizeChanged -= OnSizeChanged;

            _canvas = this.Template.FindName("PART_Canvas", this) as Canvas;
            _content = this.Template.FindName("PART_Content", this) as ContentPresenter;
            _mirror = this.Template.FindName("PART_Mirror", this) as FrameworkElement;

            if (_canvas == null || _content == null || _mirror == null)
                return;

            _canvas.SizeChanged += OnSizeChanged;
            _content.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(Object sender, SizeChangedEventArgs e)
        {
            BeginUpdate();
        }

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
                _storyboard.Remove();
                _storyboard = null;
            }

            if (_canvas == null || _content == null || _mirror == null || !HasContent || !IsVisible)
                return;

            switch (Direction)
            {
                case RunningDirection.RightToLeft:
                    UpdateRightToLeft();
                    break;
                case RunningDirection.LeftToRight:
                    UpdateLeftToRight();
                    break;
                case RunningDirection.BottomToTop:
                    UpdateBottomToTop();
                    break;
                case RunningDirection.TopToBottom:
                    UpdateTopToBottom();
                    break;
            }
        }

        private void UpdateRightToLeft()
        {
            GetHorizontal(out Double to, out Double from, out Double len);
            UpdateHorizontal(from, to, len);
        }

        private void UpdateLeftToRight()
        {
            GetHorizontal(out Double from, out Double to, out Double len);
            UpdateHorizontal(from, to, len);
        }

        private void UpdateBottomToTop()
        {
            GetVertical(out Double to, out Double from, out Double len);
            UpdateVertical(from, to, len);
        }

        private void UpdateTopToBottom()
        {
            GetVertical(out Double from, out Double to, out Double len);
            UpdateVertical(from, to, len);
        }

        private void GetHorizontal(out Double from, out Double to, out Double len)
        {
            // 计算起始位置
            var width_canvas = _canvas.ActualWidth;
            var width_content = _content.ActualWidth;

            from = -width_content;
            to = width_canvas;

            if (Double.IsNaN(Space) || Space < 0)
                len = width_content < width_canvas ? width_canvas : width_content + width_canvas;
            else
                len = width_content < width_canvas - Space ? width_canvas : width_content + Space;
        }

        private void UpdateHorizontal(Double from, Double to, Double len)
        {
            // 复位
            Canvas.SetLeft(_content, from);
            Canvas.SetLeft(_mirror, from);
            _content.SetCurrentValue(Canvas.LeftProperty, from);
            _mirror.SetCurrentValue(Canvas.LeftProperty, from);

            var begin = TimeSpan.FromSeconds(len / Speed);      // 第二个动画延迟时间
            var duration = TimeSpan.FromSeconds(Math.Abs((from - to)) / Speed);     // 动画从开始到结束的时间
            var total = begin + begin;      // 加上延迟，一次动画的时间

            _storyboard = new Storyboard();

            var ani1 = new DoubleAnimationUsingKeyFrames
            {
                Duration = total,
                RepeatBehavior = RepeatBehavior.Forever
            };

            ani1.KeyFrames.Add(new DiscreteDoubleKeyFrame(from, TimeSpan.FromSeconds(0)));
            ani1.KeyFrames.Add(new LinearDoubleKeyFrame(to, duration));

            Storyboard.SetTarget(ani1, _content);
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

            Storyboard.SetTarget(ani2, _mirror);
            Storyboard.SetTargetProperty(ani2, new PropertyPath(Canvas.LeftProperty));

            _storyboard.Children.Add(ani2);

            _storyboard.Begin();
        }

        private void GetVertical(out Double from, out Double to, out Double len)
        {
            // 计算起始位置
            var heigth_canvas = _canvas.ActualHeight;
            var height_content = _content.ActualHeight;

            from = -height_content;
            to = heigth_canvas;

            if (Double.IsNaN(Space) || Space < 0)
                len = height_content < heigth_canvas ? heigth_canvas : height_content + heigth_canvas;
            else
                len = height_content < heigth_canvas - Space ? heigth_canvas : height_content + Space;
        }

        private void UpdateVertical(Double from, Double to, Double len)
        {
            // 复位
            Canvas.SetTop(_content, from);
            Canvas.SetTop(_mirror, from);
            _content.SetCurrentValue(Canvas.TopProperty, from);
            _mirror.SetCurrentValue(Canvas.TopProperty, from);

            var begin = TimeSpan.FromSeconds(len / Speed);      // 第二个动画延迟时间
            var duration = TimeSpan.FromSeconds(Math.Abs((from - to)) / Speed);     // 动画从开始到结束的时间
            var total = begin + begin;      // 加上延迟，一次动画的时间

            _storyboard = new Storyboard();

            var ani1 = new DoubleAnimationUsingKeyFrames
            {
                Duration = total,
                RepeatBehavior = RepeatBehavior.Forever
            };

            ani1.KeyFrames.Add(new DiscreteDoubleKeyFrame(from, TimeSpan.FromSeconds(0)));
            ani1.KeyFrames.Add(new LinearDoubleKeyFrame(to, duration));

            Storyboard.SetTarget(ani1, _content);
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

            Storyboard.SetTarget(ani2, _mirror);
            Storyboard.SetTargetProperty(ani2, new PropertyPath(Canvas.TopProperty));

            _storyboard.Children.Add(ani2);

            _storyboard.Begin();
        }

        #endregion

        #region Fields

        private Canvas _canvas;
        private ContentPresenter _content;
        private FrameworkElement _mirror;
        private Action _delayUpdate;
        private Storyboard _storyboard;

        #endregion
    }
}
