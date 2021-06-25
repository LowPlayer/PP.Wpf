using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SimpleDateTimePicker : Control
    {
        #region DependencyProperties

        /// <summary>
        /// 选择的时间
        /// </summary>
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(SimpleDateTimePicker), new PropertyMetadata(OnSelectedDatePropertyChanged));

        private static void OnSelectedDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SimpleDateTimePicker)d).OnSelectedDateChanged((DateTime?)e.NewValue);
        }

        /// <summary>
        /// 选择的时间
        /// </summary>
        public DateTime? SelectedDate { get => (DateTime?)GetValue(SelectedDateProperty); set => SetValue(SelectedDateProperty, value); }



        /// <summary>
        /// 显示文本
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(SimpleDateTimePicker), new PropertyMetadata(OnTextPropertyChanged));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = (SimpleDateTimePicker)d;
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
        public static readonly DependencyProperty DateTimeFormatProperty = DependencyProperty.Register("DateTimeFormat", typeof(String), typeof(SimpleDateTimePicker), new PropertyMetadata("yyyy-MM-dd HH:mm:ss"));
        /// <summary>
        /// 日期时间显示格式
        /// </summary>
        public String DateTimeFormat { get => (String)GetValue(DateTimeFormatProperty); set => SetValue(DateTimeFormatProperty, value); }



        /// <summary>
        /// 年列表
        /// </summary>
        public static readonly DependencyPropertyKey YearsPropertyKey = DependencyProperty.RegisterReadOnly("Years", typeof(IEnumerable<Int32>), typeof(SimpleDateTimePicker), new PropertyMetadata(default));
        /// <summary>
        /// 年列表
        /// </summary>
        public static readonly DependencyProperty YearsProperty = YearsPropertyKey.DependencyProperty;
        /// <summary>
        /// 年列表
        /// </summary>
        public IEnumerable<Int32> Years { get => (IEnumerable<Int32>)GetValue(YearsProperty); private set => SetValue(YearsPropertyKey, value); }



        /// <summary>
        /// 年
        /// </summary>
        public static readonly DependencyProperty YearProperty = DependencyProperty.Register("Year", typeof(Int32), typeof(SimpleDateTimePicker), new PropertyMetadata(1970, OnYearPropertyChagned));

        private static void OnYearPropertyChagned(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = (SimpleDateTimePicker)d;

            picker.Days = Enumerable.Range(1, DateTime.DaysInMonth(picker.Year, picker.Month));
            picker.BeginUpdatePreText();
        }

        /// <summary>
        /// 年
        /// </summary>
        public Int32 Year { get => (Int32)GetValue(YearProperty); set => SetValue(YearProperty, value); }



        /// <summary>
        /// 月列表
        /// </summary>
        public static readonly DependencyPropertyKey MonthsPropertyKey = DependencyProperty.RegisterReadOnly("Months", typeof(IEnumerable<Int32>), typeof(SimpleDateTimePicker), new PropertyMetadata(default));
        /// <summary>
        /// 月列表
        /// </summary>
        public static readonly DependencyProperty MonthsProperty = MonthsPropertyKey.DependencyProperty;
        /// <summary>
        /// 月列表
        /// </summary>
        public IEnumerable<Int32> Months { get => (IEnumerable<Int32>)GetValue(MonthsProperty); private set => SetValue(MonthsPropertyKey, value); }



        /// <summary>
        /// 月
        /// </summary>
        public static readonly DependencyProperty MonthProperty = DependencyProperty.Register("Month", typeof(Int32), typeof(SimpleDateTimePicker), new PropertyMetadata(1, OnMonthPropertyChagned));

        private static void OnMonthPropertyChagned(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = (SimpleDateTimePicker)d;

            picker.Days = Enumerable.Range(1, DateTime.DaysInMonth(picker.Year, picker.Month));
            picker.BeginUpdatePreText();
        }

        /// <summary>
        /// 月
        /// </summary>
        public Int32 Month { get => (Int32)GetValue(MonthProperty); set => SetValue(MonthProperty, value); }



        /// <summary>
        /// 日列表
        /// </summary>
        public static readonly DependencyPropertyKey DaysPropertyKey = DependencyProperty.RegisterReadOnly("Days", typeof(IEnumerable<Int32>), typeof(SimpleDateTimePicker), new PropertyMetadata(default));
        /// <summary>
        /// 日列表
        /// </summary>
        public static readonly DependencyProperty DaysProperty = DaysPropertyKey.DependencyProperty;
        /// <summary>
        /// 日列表
        /// </summary>
        public IEnumerable<Int32> Days { get => (IEnumerable<Int32>)GetValue(DaysProperty); private set => SetValue(DaysPropertyKey, value); }



        /// <summary>
        /// 日
        /// </summary>
        public static readonly DependencyProperty DayProperty = DependencyProperty.Register("Day", typeof(Int32), typeof(SimpleDateTimePicker), new PropertyMetadata(OnDayPropertyChagned));

        private static void OnDayPropertyChagned(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SimpleDateTimePicker)d).BeginUpdatePreText();
        }

        /// <summary>
        /// 日
        /// </summary>
        public Int32 Day { get => (Int32)GetValue(DayProperty); set => SetValue(DayProperty, value); }



        /// <summary>
        /// 小时列表
        /// </summary>
        public static readonly DependencyPropertyKey HoursPropertyKey = DependencyProperty.RegisterReadOnly("Hours", typeof(IEnumerable<Int32>), typeof(SimpleDateTimePicker), new PropertyMetadata(default));
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
        public static readonly DependencyProperty HourProperty = DependencyProperty.Register("Hour", typeof(Int32), typeof(SimpleDateTimePicker), new PropertyMetadata(OnHourPropertyChagned));

        private static void OnHourPropertyChagned(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SimpleDateTimePicker)d).BeginUpdatePreText();
        }

        /// <summary>
        /// 小时
        /// </summary>
        public Int32 Hour { get => (Int32)GetValue(HourProperty); set => SetValue(HourProperty, value); }



        /// <summary>
        /// 分钟列表
        /// </summary>
        public static readonly DependencyPropertyKey MinutesPropertyKey = DependencyProperty.RegisterReadOnly("Minutes", typeof(IEnumerable<Int32>), typeof(SimpleDateTimePicker), new PropertyMetadata(default));
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
        public static readonly DependencyProperty MinuteProperty = DependencyProperty.Register("Minute", typeof(Int32), typeof(SimpleDateTimePicker), new PropertyMetadata(OnMinutePropertyChagned));

        private static void OnMinutePropertyChagned(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SimpleDateTimePicker)d).BeginUpdatePreText();
        }

        /// <summary>
        /// 分钟
        /// </summary>
        public Int32 Minute { get => (Int32)GetValue(MinuteProperty); set => SetValue(MinuteProperty, value); }



        /// <summary>
        /// 秒列表
        /// </summary>
        public static readonly DependencyPropertyKey SecondsPropertyKey = DependencyProperty.RegisterReadOnly("Seconds", typeof(IEnumerable<Int32>), typeof(SimpleDateTimePicker), new PropertyMetadata(default));
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
        public static readonly DependencyProperty SecondProperty = DependencyProperty.Register("Second", typeof(Int32), typeof(SimpleDateTimePicker), new PropertyMetadata(OnSecondPropertyChagned));

        private static void OnSecondPropertyChagned(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SimpleDateTimePicker)d).BeginUpdatePreText();
        }

        /// <summary>
        /// 秒
        /// </summary>
        public Int32 Second { get => (Int32)GetValue(SecondProperty); set => SetValue(SecondProperty, value); }



        /// <summary>
        /// 日期预览
        /// </summary>
        public static readonly DependencyPropertyKey PrevTextPropertyKey = DependencyProperty.RegisterReadOnly("PrevText", typeof(String), typeof(SimpleDateTimePicker), new PropertyMetadata(default));
        /// <summary>
        /// 日期预览
        /// </summary>
        public static readonly DependencyProperty PrevTextProperty = PrevTextPropertyKey.DependencyProperty;
        /// <summary>
        /// 日期预览
        /// </summary>
        public String PrevText { get => (String)GetValue(PrevTextProperty); private set => SetValue(PrevTextPropertyKey, value); }

        public static readonly DependencyProperty IsReadOnlyProperty = TextBox.IsReadOnlyProperty.AddOwner(typeof(SimpleDateTimePicker));
        /// <summary>
        /// 只读
        /// </summary>
        public Boolean IsReadOnly { get => (Boolean)GetValue(IsReadOnlyProperty); set => SetValue(IsReadOnlyProperty, value); }


        #endregion

        static SimpleDateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SimpleDateTimePicker), new FrameworkPropertyMetadata(typeof(SimpleDateTimePicker)));
        }

        /// <summary>
        /// 时间日期选择器
        /// </summary>
        public SimpleDateTimePicker()
        {
            Months = Enumerable.Range(1, 12);
            Hours = Enumerable.Range(0, 24);
            Minutes = Seconds = Enumerable.Range(0, 60);
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

            if (input != null && popup != null)
            {
                input.PreviewMouseLeftButtonUp -= OnInputPreviewMouseLeftButtonUp;
                input.LostFocus -= OnInputLostFocus;

                if (btn_icon != null)
                    btn_icon.Click -= OnPrevOpenPopup;

                if (btn_confirm != null)
                    btn_confirm.Click -= OnConfirmButtonClick;

                if (btn_now != null)
                    btn_now.Click -= OnNowButtonClick;

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
            }

            input = this.Template.FindName("PART_TextBox", this) as TextBox;
            popup = this.Template.FindName("PART_Popup", this) as Popup;

            if (input != null && popup != null)
            {
                OnSelectedDateChanged(SelectedDate);

                input.PreviewMouseLeftButtonUp += OnInputPreviewMouseLeftButtonUp;
                input.LostFocus += OnInputLostFocus;

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

            var max = time > today ? time : today;

            Years = Enumerable.Range(1970, max.Year - 1970 + 11);
            Year = time.Year;

            Month = time.Month;

            Days = Enumerable.Range(1, DateTime.DaysInMonth(time.Year, time.Month));
            Day = time.Day;

            Hour = time.Hour;
            Minute = time.Minute;
            Second = time.Second;
        }

        private void OnPrevOpenPopup(Object sender, RoutedEventArgs e)
        {
            popup.IsOpen = true;
        }

        private void OnClearButtonClick(Object sender, RoutedEventArgs e)
        {
            if (SelectedDate == null)
                OnSelectedDateChanged(null);
            else
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
            prevDate = new DateTime(Year, Month, Day, Hour, Minute, Second);
            PrevText = prevDate.ToString(DateTimeFormat);
        }

        #endregion

        #region Fields

        private TextBox input;
        private Popup popup;
        private Button btn_icon, btn_confirm, btn_now, btn_clear;
        private ListBox list1, list2, list3, list4, list5, list6;
        private Action action;
        private DateTime prevDate;

        #endregion
    }
}
