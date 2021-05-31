using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Calendar = System.Windows.Controls.Calendar;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 时间日期范围选择控件
    /// </summary>
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_IconButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_ConfirmButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CalendarBegin", Type = typeof(Calendar))]
    [TemplatePart(Name = "PART_CalendarEnd", Type = typeof(Calendar))]
    [TemplatePart(Name = "PART_TodayButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_WeekButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_MonthButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_HalfYearButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_OneYearButton", Type = typeof(Button))]
    public class DateTimeScopePicker : Control
    {
        #region DependencyProperty

        /// <summary>
        /// 开始时间
        /// </summary>
        public static readonly DependencyProperty BeginTimeProperty = DependencyProperty.Register("BeginTime", typeof(DateTime?), typeof(DateTimeScopePicker), new PropertyMetadata(OnBeginTimePropertyChanged));

        private static void OnBeginTimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DateTimeScopePicker)d).OnBeginTimeChanged((DateTime?)e.NewValue);
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get => (DateTime?)GetValue(BeginTimeProperty); set => SetValue(BeginTimeProperty, value); }



        /// <summary>
        /// 结束时间
        /// </summary>
        public static readonly DependencyProperty EndTimeProperty = DependencyProperty.Register("EndTime", typeof(DateTime?), typeof(DateTimeScopePicker), new PropertyMetadata(OnEndTimePropertyChanged));

        private static void OnEndTimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DateTimeScopePicker)d).OnEndTimeChanged((DateTime?)e.NewValue);
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get => (DateTime?)GetValue(EndTimeProperty); set => SetValue(EndTimeProperty, value); }



        /// <summary>
        /// 显示文本
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(DateTimeScopePicker), new PropertyMetadata(OnTextPropertyChanged));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DateTimeScopePicker)d).OnTextChanged((String)e.NewValue);
        }


        /// <summary>
        /// 显示文本
        /// </summary>
        public String Text { get => (String)GetValue(TextProperty); set => SetValue(TextProperty, value); }



        /// <summary>
        /// 日期时间显示格式
        /// </summary>
        public static readonly DependencyProperty DateTimeFormatProperty = DateTimePicker.DateTimeFormatProperty.AddOwner(typeof(DateTimeScopePicker));
        /// <summary>
        /// 日期时间显示格式
        /// </summary>
        public String DateTimeFormat { get => (String)GetValue(DateTimeFormatProperty); set => SetValue(DateTimeFormatProperty, value); }



        /// <summary>
        /// 小时列表
        /// </summary>
        public static readonly DependencyPropertyKey HoursPropertyKey = DependencyProperty.RegisterReadOnly("Hours", typeof(IEnumerable<Int32>), typeof(DateTimeScopePicker), new PropertyMetadata(default));
        /// <summary>
        /// 小时列表
        /// </summary>
        public static readonly DependencyProperty HoursProperty = HoursPropertyKey.DependencyProperty;
        /// <summary>
        /// 小时列表
        /// </summary>
        public IEnumerable<Int32> Hours { get => (IEnumerable<Int32>)GetValue(HoursProperty); private set => SetValue(HoursPropertyKey, value); }



        /// <summary>
        /// 分钟列表
        /// </summary>
        public static readonly DependencyPropertyKey MinutesPropertyKey = DependencyProperty.RegisterReadOnly("Minutes", typeof(IEnumerable<Int32>), typeof(DateTimeScopePicker), new PropertyMetadata(default));
        /// <summary>
        /// 分钟列表
        /// </summary>
        public static readonly DependencyProperty MinutesProperty = MinutesPropertyKey.DependencyProperty;
        /// <summary>
        /// 分钟列表
        /// </summary>
        public IEnumerable<Int32> Minutes { get => (IEnumerable<Int32>)GetValue(MinutesProperty); private set => SetValue(MinutesPropertyKey, value); }



        /// <summary>
        /// 秒列表
        /// </summary>
        public static readonly DependencyPropertyKey SecondsPropertyKey = DependencyProperty.RegisterReadOnly("Seconds", typeof(IEnumerable<Int32>), typeof(DateTimeScopePicker), new PropertyMetadata(default));
        /// <summary>
        /// 秒列表
        /// </summary>
        public static readonly DependencyProperty SecondsProperty = SecondsPropertyKey.DependencyProperty;
        /// <summary>
        /// 秒列表
        /// </summary>
        public IEnumerable<Int32> Seconds { get => (IEnumerable<Int32>)GetValue(SecondsProperty); private set => SetValue(SecondsPropertyKey, value); }



        /// <summary>
        /// 小时（开始时间）
        /// </summary>
        public static readonly DependencyProperty BeginHourProperty = DependencyProperty.Register("BeginHour", typeof(Int32), typeof(DateTimeScopePicker), new PropertyMetadata(OnTimePropertyChagned));

        private static void OnTimePropertyChagned(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DateTimeScopePicker)d).BeginUpdatePreText();
        }

        /// <summary>
        /// 小时（开始时间）
        /// </summary>
        public Int32 BeginHour { get => (Int32)GetValue(BeginHourProperty); set => SetValue(BeginHourProperty, value); }



        /// <summary>
        /// 分钟（开始时间）
        /// </summary>
        public static readonly DependencyProperty BeginMinuteProperty = DependencyProperty.Register("BeginMinute", typeof(Int32), typeof(DateTimeScopePicker), new PropertyMetadata(OnTimePropertyChagned));

        /// <summary>
        /// 分钟（开始时间）
        /// </summary>
        public Int32 BeginMinute { get => (Int32)GetValue(BeginMinuteProperty); set => SetValue(BeginMinuteProperty, value); }



        /// <summary>
        /// 秒（开始时间）
        /// </summary>
        public static readonly DependencyProperty BeginSecondProperty = DependencyProperty.Register("BeginSecond", typeof(Int32), typeof(DateTimeScopePicker), new PropertyMetadata(OnTimePropertyChagned));

        /// <summary>
        /// 秒（开始时间）
        /// </summary>
        public Int32 BeginSecond { get => (Int32)GetValue(BeginSecondProperty); set => SetValue(BeginSecondProperty, value); }



        /// <summary>
        /// 小时（结束时间）
        /// </summary>
        public static readonly DependencyProperty EndHourProperty = DependencyProperty.Register("EndHour", typeof(Int32), typeof(DateTimeScopePicker), new PropertyMetadata(OnTimePropertyChagned));

        /// <summary>
        /// 小时（结束时间）
        /// </summary>
        public Int32 EndHour { get => (Int32)GetValue(EndHourProperty); set => SetValue(EndHourProperty, value); }



        /// <summary>
        /// 分钟（结束时间）
        /// </summary>
        public static readonly DependencyProperty EndMinuteProperty = DependencyProperty.Register("EndMinute", typeof(Int32), typeof(DateTimeScopePicker), new PropertyMetadata(OnTimePropertyChagned));

        /// <summary>
        /// 分钟（结束时间）
        /// </summary>
        public Int32 EndMinute { get => (Int32)GetValue(EndMinuteProperty); set => SetValue(EndMinuteProperty, value); }



        /// <summary>
        /// 秒（结束时间）
        /// </summary>
        public static readonly DependencyProperty EndSecondProperty = DependencyProperty.Register("EndSecond", typeof(Int32), typeof(DateTimeScopePicker), new PropertyMetadata(OnTimePropertyChagned));

        /// <summary>
        /// 秒（结束时间）
        /// </summary>
        public Int32 EndSecond { get => (Int32)GetValue(EndSecondProperty); set => SetValue(EndSecondProperty, value); }



        /// <summary>
        /// 日期预览
        /// </summary>
        public static readonly DependencyPropertyKey PrevTextPropertyKey = DependencyProperty.RegisterReadOnly("PrevText", typeof(String), typeof(DateTimeScopePicker), new PropertyMetadata(default));
        /// <summary>
        /// 日期预览
        /// </summary>
        public static readonly DependencyProperty PrevTextProperty = PrevTextPropertyKey.DependencyProperty;
        /// <summary>
        /// 日期预览
        /// </summary>
        public String PrevText { get => (String)GetValue(PrevTextProperty); private set => SetValue(PrevTextPropertyKey, value); }

        #endregion

        static DateTimeScopePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimeScopePicker), new FrameworkPropertyMetadata(typeof(DateTimeScopePicker)));
        }

        /// <summary>
        /// 时间日期范围选择器
        /// </summary>
        public DateTimeScopePicker()
        {
            Hours = Enumerable.Range(0, 24);
            Minutes = Seconds = Enumerable.Range(0, 60);
        }

        #region Override Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (input != null && popup != null && calendarBegin != null && calendarEnd != null)
            {
                input.PreviewMouseLeftButtonUp -= OnInputPreviewMouseLeftButtonUp;

                calendarBegin.SelectedDatesChanged += OnCalendarSelectedDatesChanged;
                calendarEnd.SelectedDatesChanged += OnCalendarSelectedDatesChanged;

                if (btn_icon != null)
                    btn_icon.Click -= OnPrevOpenPopup;

                if (btn_confirm != null)
                    btn_confirm.Click -= OnConfirmButtonClick;

                if (btn_clear != null)
                    btn_clear.Click -= OnClearButtonClick;

                var lists = new List<ListBox> { list1, list2, list3, list4, list5, list6 };

                foreach (var list in lists)
                {
                    if (list == null)
                        continue;

                    list.SelectionChanged -= OnListBoxSelectionChanged;
                    list.Loaded -= OnListBoxLoaded;
                }

                if (btn_today != null)
                    btn_today.Click -= OnTimePeriodButtonClick;
                if (btn_week != null)
                    btn_week.Click -= OnTimePeriodButtonClick;
                if (btn_month != null)
                    btn_month.Click -= OnTimePeriodButtonClick;
                if (btn_half_year != null)
                    btn_half_year.Click -= OnTimePeriodButtonClick;
                if (btn_one_year != null)
                    btn_one_year.Click -= OnTimePeriodButtonClick;
            }

            input = this.Template.FindName("PART_TextBox", this) as TextBox;
            popup = this.Template.FindName("PART_Popup", this) as Popup;
            calendarBegin = popup?.FindName("PART_CalendarBegin") as Calendar;
            calendarEnd = popup?.FindName("PART_CalendarEnd") as Calendar;

            if (input != null && popup != null && calendarBegin != null && calendarEnd != null)
            {
                OnBeginTimeChanged(BeginTime);
                OnEndTimeChanged(EndTime);

                input.PreviewMouseLeftButtonUp += OnInputPreviewMouseLeftButtonUp;

                calendarBegin.SelectionMode = calendarEnd.SelectionMode = CalendarSelectionMode.SingleDate;
                calendarBegin.SelectedDatesChanged += OnCalendarSelectedDatesChanged;
                calendarEnd.SelectedDatesChanged += OnCalendarSelectedDatesChanged;

                btn_icon = this.Template.FindName("PART_IconButton", this) as Button;
                btn_confirm = this.Template.FindName("PART_ConfirmButton", this) as Button;
                btn_clear = this.Template.FindName("PART_ClearButton", this) as Button;

                if (btn_icon != null)
                    btn_icon.Click += OnPrevOpenPopup;

                if (btn_confirm != null)
                    btn_confirm.Click += OnConfirmButtonClick;

                if (btn_clear != null)
                    btn_clear.Click += OnClearButtonClick;

                list1 = popup.FindName("list1") as ListBox;
                list2 = popup.FindName("list2") as ListBox;
                list3 = popup.FindName("list3") as ListBox;
                list4 = popup.FindName("list4") as ListBox;
                list5 = popup.FindName("list5") as ListBox;
                list6 = popup.FindName("list6") as ListBox;

                var lists = new List<ListBox> { list1, list2, list3, list4, list5, list6 };

                foreach (var list in lists)
                {
                    if (list == null)
                        continue;

                    list.SelectionMode = SelectionMode.Single;
                    list.SelectionChanged += OnListBoxSelectionChanged;
                    list.Loaded += OnListBoxLoaded;
                }

                btn_today = this.Template.FindName("PART_TodayButton", this) as Button;
                btn_week = this.Template.FindName("PART_WeekButton", this) as Button;
                btn_month = this.Template.FindName("PART_MonthButton", this) as Button;
                btn_half_year = this.Template.FindName("PART_HalfYearButton", this) as Button;
                btn_one_year = this.Template.FindName("PART_OneYearButton", this) as Button;

                if (btn_today != null)
                    btn_today.Click += OnTimePeriodButtonClick;
                if (btn_week != null)
                    btn_week.Click += OnTimePeriodButtonClick;
                if (btn_month != null)
                    btn_month.Click += OnTimePeriodButtonClick;
                if (btn_half_year != null)
                    btn_half_year.Click += OnTimePeriodButtonClick;
                if (btn_one_year != null)
                    btn_one_year.Click += OnTimePeriodButtonClick;
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

        #region Private Method

        private void OnBeginTimeChanged(DateTime? date)
        {
            var time = date ?? DateTime.Today;

            beginYear = time.Year;
            beginMonth = time.Month;
            beginDay = time.Day;
            BeginHour = time.Hour;
            BeginMinute = time.Minute;
            BeginSecond = time.Second;

            if (calendarBegin != null)
                calendarBegin.SelectedDate = time;

            BeginUpdateText();
        }

        private void OnEndTimeChanged(DateTime? date)
        {
            var time = date ?? DateTime.Today.AddDays(1).AddSeconds(-1);

            endYear = time.Year;
            endMonth = time.Month;
            endDay = time.Day;
            EndHour = time.Hour;
            EndMinute = time.Minute;
            EndSecond = time.Second;

            if (calendarEnd != null)
                calendarEnd.SelectedDate = time;

            BeginUpdateText();
        }

        private void BeginUpdateText()
        {
            actionText = UpdateText;

            Dispatcher.InvokeAsync(() =>
            {
                if (actionText != null)
                {
                    actionText.Invoke();
                    actionText = null;
                }
            });
        }

        private void UpdateText()
        {
            if (BeginTime.HasValue || EndTime.HasValue)
                Text = $"{(BeginTime.HasValue ? BeginTime.Value.ToString(DateTimeFormat, CultureInfo.InvariantCulture) : null)} - {(EndTime.HasValue ? EndTime.Value.ToString(DateTimeFormat, CultureInfo.InvariantCulture) : null)}";
            else
                Text = null;
        }

        private void OnTextChanged(String txt)
        {
            if (String.IsNullOrWhiteSpace(txt) || txt.Length < DateTimeFormat.Length + 3)
                return;

            var txtStart = txt.Substring(0, DateTimeFormat.Length);
            var txtEnd = txt.Substring(txt.Length - DateTimeFormat.Length);

            if (DateTime.TryParseExact(txtStart, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time))
                BeginTime = time;
            if (DateTime.TryParseExact(txtEnd, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                EndTime = time;
        }

        private void BeginUpdatePreText()
        {
            actionPrevText = UpdatePreText;

            Dispatcher.InvokeAsync(() =>
            {
                if (actionPrevText != null)
                {
                    actionPrevText.Invoke();
                    actionPrevText = null;
                }
            });
        }

        private void UpdatePreText()
        {
            prevBeginDate = new DateTime(beginYear, beginMonth, beginDay, BeginHour, BeginMinute, BeginSecond);
            prevEndDate = new DateTime(endYear, endMonth, endDay, EndHour, EndMinute, EndSecond);
            PrevText = $"{prevBeginDate.ToString(DateTimeFormat)} - {prevEndDate.ToString(DateTimeFormat)}";
        }



        private void OnInputPreviewMouseLeftButtonUp(Object sender, MouseButtonEventArgs e)
        {
            Dispatcher.InvokeAsync(() =>
            {
                if (input.IsFocused)
                    OnPrevOpenPopup(sender, e);
            });
        }

        private void OnCalendarSelectedDatesChanged(Object sender, SelectionChangedEventArgs e)
        {
            var calendar = (Calendar)sender;
            calendar.ReleaseStylusCapture();


            if (calendar == calendarBegin)
            {
                var time = calendar.SelectedDate ?? DateTime.Today;

                beginYear = time.Year;
                beginMonth = time.Month;
                beginDay = time.Day;
            }
            else
            {
                var time = calendar.SelectedDate ?? DateTime.Today.AddDays(1).AddSeconds(-1);

                endYear = time.Year;
                endMonth = time.Month;
                endDay = time.Day;
            }

            BeginUpdatePreText();
        }

        private void OnPrevOpenPopup(Object sender, RoutedEventArgs e)
        {
            popup.IsOpen = true;
        }

        private void OnConfirmButtonClick(Object sender, RoutedEventArgs e)
        {
            if (prevBeginDate == prevEndDate)
                OnClearButtonClick(sender, e);
            else
            {
                BeginTime = prevBeginDate < prevEndDate ? prevBeginDate : prevEndDate;
                EndTime = prevBeginDate > prevEndDate ? prevBeginDate : prevEndDate;
            }

            popup.IsOpen = false;

            input.Focus();
            input.SelectionLength = 0;
            input.SelectionStart = input.Text == null ? 0 : input.Text.Length;
        }

        private void OnClearButtonClick(Object sender, RoutedEventArgs e)
        {
            if (BeginTime == null)
                OnBeginTimeChanged(null);
            else
                BeginTime = null;

            if (EndTime == null)
                OnEndTimeChanged(null);
            else
                EndTime = null;
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

        private void OnTimePeriodButtonClick(Object sender, RoutedEventArgs e)
        {
            var today = DateTime.Today;
            var start = today;
            var end = today.AddDays(1).AddSeconds(-1);

            if (sender == btn_week)
                start = today.AddDays(-6);
            else if (sender == btn_month)
                start = today.AddMonths(-1);
            else if (sender == btn_half_year)
                start = today.AddMonths(-6);
            else if (sender == btn_one_year)
                start = today.AddMonths(-12);

            OnBeginTimeChanged(start);
            OnEndTimeChanged(end);
        }

        #endregion

        #region Fields

        private TextBox input;
        private Popup popup;
        private Button btn_icon, btn_confirm, btn_clear;
        private Calendar calendarBegin, calendarEnd;
        private Action actionPrevText, actionText;
        private ListBox list1, list2, list3, list4, list5, list6;
        private DateTime prevBeginDate, prevEndDate;
        private Int32 beginYear, beginMonth, beginDay, endYear, endMonth, endDay;
        private Button btn_today, btn_week, btn_month, btn_half_year, btn_one_year;

        #endregion
    }
}
