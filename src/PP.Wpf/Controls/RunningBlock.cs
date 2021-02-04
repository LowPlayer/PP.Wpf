using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 滚动块
    /// </summary>
    [TemplatePart(Name = "PART_Content", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "PART_Mirror", Type = typeof(FrameworkElement))]
    public class RunningBlock : ContentControl
    {
        /// <summary>
        /// 滚动方向
        /// </summary>
        public enum RunningDirection
        {
            /// <summary>
            /// 从右往左
            /// </summary>
            RightToLeft,
            /// <summary>
            /// 从左往右
            /// </summary>
            LeftToRight,
            /// <summary>
            /// 从下往上
            /// </summary>
            BottomToTop,
            /// <summary>
            /// 从上往下
            /// </summary>
            TopToBottom
        }

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
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(RunningDirection), typeof(RunningBlock), new PropertyMetadata(RunningDirection.RightToLeft, OnDirectionPropertyChanged));

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

            _content = this.Template.FindName("PART_Content", this) as ContentPresenter;
            _mirror = this.Template.FindName("PART_Mirror", this) as FrameworkElement;

            if (_content != null && _mirror != null)
            {
                _content.RenderTransform = new TranslateTransform();
                _mirror.RenderTransform = new TranslateTransform();
            }
        }

        /// <summary>
        /// 内容改变事件
        /// </summary>
        /// <param name="oldContent"></param>
        /// <param name="newContent"></param>
        protected override void OnContentChanged(Object oldContent, Object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
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
                _storyboard = null;
            }

            if (_content == null || _mirror == null)
                return;

            if (_content.RenderTransform is TranslateTransform trans_content && _mirror.RenderTransform is TranslateTransform trans_mirror)
            {
                switch (Direction)
                {
                    case RunningDirection.RightToLeft:
                        UpdateRightToLeft(trans_content, trans_mirror);
                        break;
                    case RunningDirection.LeftToRight:
                        break;
                    case RunningDirection.BottomToTop:
                        break;
                    case RunningDirection.TopToBottom:
                        break;
                }
            }
        }

        private void UpdateRightToLeft(TranslateTransform trans_content, TranslateTransform trans_mirror)
        {

        }

        #endregion

        #region Fields

        private ContentPresenter _content;
        private FrameworkElement _mirror;
        private Action _delayUpdate;
        private Storyboard _storyboard;

        #endregion
    }
}
