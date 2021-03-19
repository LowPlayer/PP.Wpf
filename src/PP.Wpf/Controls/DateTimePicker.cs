using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 时间日期选择器
    /// </summary>
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_IconButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_ConfirmButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_NowButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Calendar", Type = typeof(Calendar))]
    public class DateTimePicker : Control
    {
        #region DependencyProperties

        /// <summary>
        /// 选择的时间
        /// </summary>
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DateTimePicker), new PropertyMetadata(OnSelectedDatePropertyChanged));

        private static void OnSelectedDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DateTimePicker)d).OnSelectedDateChanged((DateTime?)e.NewValue);
        }

        /// <summary>
        /// 选择的时间
        /// </summary>
        public DateTime? SelectedDate { get => (DateTime?)GetValue(SelectedDateProperty); set => SetValue(SelectedDateProperty, value); }



        /// <summary>
        /// 显示文本
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(DateTimePicker), new PropertyMetadata(OnTextPropertyChanged));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = (DateTimePicker)d;
            var txt = (String)e.NewValue;

            if (DateTime.TryParseExact(txt, picker.DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime time))
                picker.SelectedDate = time;
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        public String Text { get => (String)GetValue(TextProperty); set => SetValue(TextProperty, value); }



        /// <summary>
        /// 日期时间显示格式
        /// </summary>
        public static readonly DependencyProperty DateTimeFormatProperty = DependencyProperty.Register("DateTimeFormat", typeof(String), typeof(DateTimePicker), new PropertyMetadata("yyyy-MM-dd HH:mm:ss"));
        /// <summary>
        /// 日期时间显示格式
        /// </summary>
        public String DateTimeFormat { get => (String)GetValue(DateTimeFormatProperty); set => SetValue(DateTimeFormatProperty, value); }



        /// <summary>
        /// 小时列表
        /// </summary>
        public static readonly DependencyPropertyKey HoursPropertyKey = DependencyProperty.RegisterReadOnly("Hours", typeof(IEnumerable<Int32>), typeof(DateTimePicker), new PropertyMetadata(default));
        /// <summary>
        /// 小时列表
        /// </summary>
        public static readonly DependencyProperty HoursProperty = HoursPropertyKey.DependencyProperty;
        /// <summary>
        /// 小时列表
        /// </summary>
        public IEnumerable<Int32> Hours { get => (IEnumerable<Int32>)GetValue(HoursProperty); private set => SetValue(HoursPropertyKey, value); }



        /// <summary>
        /// 小时
        /// </summary>
        public static readonly DependencyProperty HourProperty = DependencyProperty.Register("Hour", typeof(Int32), typeof(DateTimePicker), new PropertyMetadata(OnHourPropertyChagned));

        private static void OnHourPropertyChagned(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DateTimePicker)d).BeginUpdatePreText();
        }

        /// <summary>
        /// 小时
        /// </summary>
        public Int32 Hour { get => (Int32)GetValue(HourProperty); set => SetValue(HourProperty, value); }



        /// <summary>
        /// 分钟列表
        /// </summary>
        public static readonly DependencyPropertyKey MinutesPropertyKey = DependencyProperty.RegisterReadOnly("Minutes", typeof(IEnumerable<Int32>), typeof(DateTimePicker), new PropertyMetadata(default));
        /// <summary>
        /// 分钟列表
        /// </summary>
        public static readonly DependencyProperty MinutesProperty = MinutesPropertyKey.DependencyProperty;
        /// <summary>
        /// 分钟列表
        /// </summary>
        public IEnumerable<Int32> Minutes { get => (IEnumerable<Int32>)GetValue(MinutesProperty); private set => SetValue(MinutesPropertyKey, value); }



        /// <summary>
        /// 分钟
        /// </summary>
        public static readonly DependencyProperty MinuteProperty = DependencyProperty.Register("Minute", typeof(Int32), typeof(DateTimePicker), new PropertyMetadata(OnMinutePropertyChagned));

        private static void OnMinutePropertyChagned(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DateTimePicker)d).BeginUpdatePreText();
        }

        /// <summary>
        /// 分钟
        /// </summary>
        public Int32 Minute { get => (Int32)GetValue(MinuteProperty); set => SetValue(MinuteProperty, value); }



        /// <summary>
        /// 秒列表
        /// </summary>
        public static readonly DependencyPropertyKey SecondsPropertyKey = DependencyProperty.RegisterReadOnly("Seconds", typeof(IEnumerable<Int32>), typeof(DateTimePicker), new PropertyMetadata(default));
        /// <summary>
        /// 秒列表
        /// </summary>
        public static readonly DependencyProperty SecondsProperty = SecondsPropertyKey.DependencyProperty;
        /// <summary>
        /// 秒列表
        /// </summary>
        public IEnumerable<Int32> Seconds { get => (IEnumerable<Int32>)GetValue(SecondsProperty); private set => SetValue(SecondsPropertyKey, value); }



        /// <summary>
        /// 秒
        /// </summary>
        public static readonly DependencyProperty SecondProperty = DependencyProperty.Register("Second", typeof(Int32), typeof(DateTimePicker), new PropertyMetadata(OnSecondPropertyChagned));

        private static void OnSecondPropertyChagned(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DateTimePicker)d).BeginUpdatePreText();
        }

        /// <summary>
        /// 秒
        /// </summary>
        public Int32 Second { get => (Int32)GetValue(SecondProperty); set => SetValue(SecondProperty, value); }



        /// <summary>
        /// 日期预览
        /// </summary>
        public static readonly DependencyPropertyKey PrevTextPropertyKey = DependencyProperty.RegisterReadOnly("PrevText", typeof(String), typeof(DateTimePicker), new PropertyMetadata(default));
        /// <summary>
        /// 日期预览
        /// </summary>
        public static readonly DependencyProperty PrevTextProperty = PrevTextPropertyKey.DependencyProperty;
        /// <summary>
        /// 日期预览
        /// </summary>
        public String PrevText { get => (String)GetValue(PrevTextProperty); private set => SetValue(PrevTextPropertyKey, value); }

        #endregion

        static DateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimePicker), new FrameworkPropertyMetadata(typeof(DateTimePicker)));
        }

        /// <summary>
        /// 时间日期选择器
        /// </summary>
        public DateTimePicker()
        {
            Hours = new List<Int32>(Range(0, 23));
            Minutes = Seconds = new List<Int32>(Range(0, 59));

            SelectedDate = SelectedDate ?? DateTime.Now;
        }

        #region Public Methods

        public void SelectAll()
        {
            input?.SelectAll();
        }

        #endregion

        #region Override Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (input != null && popup != null && calendar != null)
            {
                input.PreviewMouseLeftButtonUp -= OnInputPreviewMouseLeftButtonUp;
                input.LostFocus -= OnInputLostFocus;

                calendar.SelectedDatesChanged -= OnCalendarSelectedDatesChanged;

                if (btn_icon != null)
                    btn_icon.Click -= OnPrevOpenPopup;

                if (btn_confirm != null)
                    btn_confirm.Click -= OnConfirmButtonClick;

                if (btn_now != null)
                    btn_now.Click -= OnNowButtonClick;

                if (btn_clear != null)
                    btn_clear.Click -= OnClearButtonClick;

                var lists = new List<ListBox> { list1, list2, list3 };

                foreach (var list in lists)
                {
                    if (list == null)
                        continue;

                    list.SelectionChanged -= OnListBoxSelectionChanged;
                    list.Loaded -= OnListBoxLoaded;
                }
            }

            input = this.Template.FindName("PART_TextBox", this) as TextBox;
            popup = this.Template.FindName("PART_Popup", this) as Popup;
            calendar = popup?.FindName("PART_Calendar") as Calendar;

            if (input != null && popup != null && calendar != null)
            {
                input.PreviewMouseLeftButtonUp += OnInputPreviewMouseLeftButtonUp;
                input.LostFocus += OnInputLostFocus;

                calendar.SelectionMode = CalendarSelectionMode.SingleDate;
                calendar.SelectedDatesChanged += OnCalendarSelectedDatesChanged;
                calendar.SelectedDate = SelectedDate;

                btn_icon = this.Template.FindName("PART_IconButton", this) as Button;
                btn_confirm = this.Template.FindName("PART_ConfirmButton", this) as Button;
                btn_now = this.Template.FindName("PART_NowButton", this) as Button;
                btn_clear = this.Template.FindName("PART_ClearButton", this) as Button;

                if (btn_icon != null)
                    btn_icon.Click += OnPrevOpenPopup;

                if (btn_confirm != null)
                    btn_confirm.Click += OnConfirmButtonClick;

                if (btn_now != null)
                    btn_now.Click += OnNowButtonClick;

                if (btn_clear != null)
                    btn_clear.Click += OnClearButtonClick;

                list1 = popup.FindName("list1") as ListBox;
                list2 = popup.FindName("list2") as ListBox;
                list3 = popup.FindName("list3") as ListBox;

                var lists = new List<ListBox> { list1, list2, list3 };

                foreach (var list in lists)
                {
                    if (list == null)
                        continue;

                    list.SelectionMode = SelectionMode.Single;
                    list.SelectionChanged += OnListBoxSelectionChanged;
                    list.Loaded += OnListBoxLoaded;
                }
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (input != null && input.Focus())
                e.Handled = true;
            else
                base.OnGotFocus(e);
        }

        #endregion

        #region Private Methods

        private IEnumerable<Int32> Range(Int32 from, Int32 to)
        {
            for (var i = from; i <= to; i++)
            {
                yield return i;
            }
        }

        private void OnSelectedDateChanged(DateTime? date)
        {
            var today = DateTime.Now;

            DateTime time;

            if (date.HasValue)
            {
                time = date.Value;
                Text = time.ToString(DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                Text = null;
                time = today;
            }

            year = time.Year;
            month = time.Month;
            day = time.Day;
            Hour = time.Hour;
            Minute = time.Minute;
            Second = time.Second;

            if (calendar != null)
                calendar.SelectedDate = time;
        }

        private void OnPrevOpenPopup(Object sender, RoutedEventArgs e)
        {
            popup.IsOpen = true;
        }

        private void OnClearButtonClick(Object sender, RoutedEventArgs e)
        {
            SelectedDate = null;
        }

        private void OnNowButtonClick(Object sender, RoutedEventArgs e)
        {
            SelectedDate = DateTime.Now;
        }


        private void OnInputPreviewMouseLeftButtonUp(Object sender, MouseButtonEventArgs e)
        {
            Dispatcher.InvokeAsync(() =>
            {
                if (input.IsFocused)
                    OnPrevOpenPopup(sender, e);
            });
        }

        private void OnInputLostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Text) || !DateTime.TryParse(Text, out DateTime date))
            {
                Text = null;
                SelectedDate = null;
            }
            else
                SelectedDate = date;
        }

        private void OnConfirmButtonClick(Object sender, RoutedEventArgs e)
        {
            SelectedDate = prevDate;

            popup.IsOpen = false;

            input.Focus();
            input.SelectionLength = 0;
            input.SelectionStart = input.Text == null ? 0 : input.Text.Length;
        }

        private void OnCalendarSelectedDatesChanged(Object sender, SelectionChangedEventArgs e)
        {
            calendar.ReleaseStylusCapture();

            var time = calendar.SelectedDate ?? DateTime.Now;

            year = time.Year;
            month = time.Month;
            day = time.Day;

            BeginUpdatePreText();
        }

        private void OnListBoxSelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            ScrollToCenter((ListBox)sender);
        }

        private void OnListBoxLoaded(object sender, RoutedEventArgs e)
        {
            ScrollToCenter((ListBox)sender);
        }

        private void ScrollToCenter(ListBox list)
        {
            if (!list.IsLoaded || list.SelectedItem == null || (list.IsMouseOver && Mouse.LeftButton == MouseButtonState.Pressed))
                return;

            var item = (ListBoxItem)list.ItemContainerGenerator.ContainerFromIndex(list.SelectedIndex);

            if (item == null)
                return;

            var sv = list.Template.FindName("PART_ScrollViewer", list) as ScrollViewer;

            if (sv == null)
                return;

            var offsetY = item.TranslatePoint(new Point(0, -(sv.ViewportHeight - item.ActualHeight) / 2), sv).Y;

            sv.ScrollToVerticalOffset(sv.VerticalOffset + offsetY);
        }

        private void BeginUpdatePreText()
        {
            action = UpdatePreText;

            Dispatcher.InvokeAsync(() =>
            {
                if (action != null)
                {
                    action.Invoke();
                    action = null;
                }
            });
        }

        private void UpdatePreText()
        {
            prevDate = new DateTime(year, month, day, Hour, Minute, Second);
            PrevText = prevDate.ToString(DateTimeFormat);
        }

        #endregion

        #region Fields

        private TextBox input;
        private Popup popup;
        private Button btn_icon, btn_confirm, btn_now, btn_clear;
        private Calendar calendar;
        private Action action;
        private ListBox list1, list2, list3;
        private DateTime prevDate;
        private Int32 year, month, day;

        #endregion
    }
}
