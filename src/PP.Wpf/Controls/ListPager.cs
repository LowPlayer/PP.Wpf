using PP.Wpf.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 列表分页控件
    /// </summary>
    public sealed class ListPager : ContentControl
    {
        #region DependencyProperties

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(IEnumerable), typeof(ListPager), new PropertyMetadata(OnSourcePropertyChanged));

        private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ListPager)d).OnSourceChanged(e.OldValue, e.NewValue);
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public IEnumerable Source { get => (IEnumerable)GetValue(SourceProperty); set => SetValue(SourceProperty, value); }

        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register("PageSize", typeof(Int32), typeof(ListPager), new PropertyMetadata(10, new PropertyChangedCallback(OnPageSizePropertyChanged), new CoerceValueCallback(OnPageSizePropertyCoerceValueCallback)), new ValidateValueCallback(OnPageSizePropertyValidateValueCallback));

        private static Boolean OnPageSizePropertyValidateValueCallback(Object value)
        {
            var val = (Int32)value;
            return val > 0 && !Double.IsPositiveInfinity(val);
        }

        private static Object OnPageSizePropertyCoerceValueCallback(DependencyObject d, Object baseValue)
        {
            var val = (Int32)baseValue;

            if (val <= 0)
                return 10;

            return baseValue;
        }

        private static void OnPageSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ListPager)d).OnPageSizeChanged();
        }

        /// <summary>
        /// 一页大小
        /// </summary>
        public Int32 PageSize { get => (Int32)GetValue(PageSizeProperty); set => SetValue(PageSizeProperty, value); }

        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register("PageIndex", typeof(Int32), typeof(ListPager), new PropertyMetadata(1, new PropertyChangedCallback(OnPageIndexPropertyChanged)));

        private static void OnPageIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ListPager)d).OnPageIndexChanged();
        }

        /// <summary>
        /// 第几页
        /// </summary>
        public Int32 PageIndex { get => (Int32)GetValue(PageIndexProperty); set => SetValue(PageIndexProperty, value); }

        public static readonly DependencyPropertyKey PageCountPropertyKey = DependencyProperty.RegisterReadOnly("PageCount", typeof(Int32), typeof(ListPager), new PropertyMetadata(0));

        public static readonly DependencyProperty PageCountProperty = PageCountPropertyKey.DependencyProperty;
        /// <summary>
        /// 页数
        /// </summary>
        public Int32 PageCount { get => (Int32)GetValue(PageCountProperty); private set => SetValue(PageCountPropertyKey, value); }

        public static readonly DependencyPropertyKey TotalCountPropertyKey = DependencyProperty.RegisterReadOnly("TotalCount", typeof(Int32), typeof(ListPager), new PropertyMetadata(0));

        public static readonly DependencyProperty TotalCountProperty = TotalCountPropertyKey.DependencyProperty;
        /// <summary>
        /// 条目数
        /// </summary>
        public Int32 TotalCount { get => (Int32)GetValue(TotalCountProperty); private set => SetValue(TotalCountPropertyKey, value); }

        public static readonly DependencyPropertyKey DisplaySourcePropertyKey = DependencyProperty.RegisterReadOnly("DisplaySource", typeof(IEnumerable), typeof(ListPager), new PropertyMetadata(new PropertyChangedCallback(OnDisplaySourcePropertyChanged)));

        public static readonly DependencyProperty DisplaySourceProperty = DisplaySourcePropertyKey.DependencyProperty;

        private static void OnDisplaySourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pager = (ListPager)d;
            pager.DisplaySourceChanged?.Invoke(pager, EventArgs.Empty);
        }

        /// <summary>
        /// 展示的数据源
        /// </summary>
        public IEnumerable DisplaySource { get => (IEnumerable)GetValue(DisplaySourceProperty); private set => SetValue(DisplaySourcePropertyKey, value); }

        public static readonly DependencyPropertyKey PageNumbersPropertyKey = DependencyProperty.RegisterReadOnly("PageNumbers", typeof(IEnumerable<PageNumber>), typeof(ListPager), new PropertyMetadata(default));

        public static readonly DependencyProperty PageNumbersProperty = PageNumbersPropertyKey.DependencyProperty;

        /// <summary>
        /// 页码
        /// </summary>
        public IEnumerable<PageNumber> PageNumbers { get => (IEnumerable<PageNumber>)GetValue(PageNumbersProperty); private set => SetValue(PageNumbersPropertyKey, value); }

        #endregion

        static ListPager()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListPager), new FrameworkPropertyMetadata(typeof(ListPager)));
        }

        public ListPager()
        {
            CommandBindings.Add(new CommandBinding(PageCommands.FirstPage, OnFirstPageExecute, OnFirstPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.LastPage, OnLastPageExecute, OnLastPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.PrevPage, OnPrevPageExecute, OnPrevPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.NextPage, OnNextPageExecute, OnNextPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.SkipPage, OnSkipPageExecute, OnSkipPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.PrevMorePage, OnPrevMoreExecute, OnPrevPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.NextMorePage, OnNextMoreExecute, OnNextPageCanExecute));
        }

        #region Private Methods

        private void OnSourceChanged(Object oldVal, Object newVal)
        {
            if (oldVal is INotifyCollectionChanged oldNotify)
                oldNotify.CollectionChanged -= OnCollectionChanged;
            if (newVal is INotifyCollectionChanged newNotify)
                newNotify.CollectionChanged += OnCollectionChanged;

            OnSourceChanged();
        }

        private void OnSourceChanged()
        {
            GetPageCount();

            if (PageCount == 0)
            {
                if (PageIndex != 1)
                    PageIndex = 1;
                else
                    OnPageIndexChanged();
            }
            else if (PageIndex > PageCount)
                PageIndex = PageCount;
            else
                OnPageIndexChanged();
        }

        private void OnCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            OnSourceChanged();
        }

        private void OnPageSizeChanged()
        {
            GetPageCount();

            if (PageIndex != 1)
                PageIndex = 1;
            else
                OnPageIndexChanged();
        }

        private void OnPageIndexChanged()
        {
            DisplaySource = GetDisplaySource();
            PageNumbers = GetPageNumbers();
            PageIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        private void GetPageCount()
        {
            if (Source == null)
            {
                PageCount = TotalCount = 0;
                return;
            }

            var count = 0;

            if (Source is ICollection collection)
                count = collection.Count;
            else
            {
                foreach (var i in Source)
                {
                    count++;
                }
            }

            TotalCount = count;
            PageCount = (Int32)Math.Ceiling((Double)count / PageSize);
        }

        private IEnumerable GetDisplaySource()
        {
            if (Source == null)
                yield return null;
            else
            {
                var start = (PageIndex - 1) * PageSize;
                var end = start + PageSize;

                if (Source is IList list)
                {
                    var max = Math.Min(list.Count, end);

                    for (var i = start; i < max; i++)
                    {
                        yield return list[i];
                    }
                }
                else
                {
                    var i = -1;

                    foreach (var item in Source)
                    {
                        i++;

                        if (i < start)
                            continue;

                        if (i > end)
                            break;

                        yield return item;
                    }
                }
            }
        }

        private IEnumerable<PageNumber> GetPageNumbers()
        {
            var count = PageCount;

            if (count == 0)
                return null;

            var index = PageIndex;

            var list = new List<PageNumber>();

            var from = Math.Max(1, index - 2);
            var to = Math.Min(count, index + 2);

            if (from != 1)
                list.Add(new PageNumber(1, 0));
            if (from > 2)
            {
                if (from == 3)
                    list.Add(new PageNumber(2, 0));
                else
                    list.Add(new PageNumber(0, 2));
            }

            for (var i = from; i <= to; i++)
            {
                list.Add(new PageNumber(i, i == index ? 1 : 0));
            }

            if (to < count - 1)
            {
                if (to == count - 2)
                    list.Add(new PageNumber(count - 1, 0));
                else
                    list.Add(new PageNumber(0, 3));
            }
            if (to != count)
                list.Add(new PageNumber(count, 0));

            return list;
        }

        private void OnFirstPageCanExecute(Object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = PageCount > 0 && PageIndex != 1;
        }

        private void OnFirstPageExecute(Object sender, ExecutedRoutedEventArgs e)
        {
            PageIndex = 1;
        }

        private void OnLastPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = PageCount > 0 && PageIndex != PageCount;
        }
        private void OnLastPageExecute(object sender, ExecutedRoutedEventArgs e)
        {
            PageIndex = PageCount;
        }

        private void OnNextPageCanExecute(Object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = PageCount > 0 && PageIndex != PageCount;
        }

        private void OnNextPageExecute(Object sender, ExecutedRoutedEventArgs e)
        {
            PageIndex++;
        }

        private void OnPrevPageCanExecute(Object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = PageCount > 0 && PageIndex != 1;
        }

        private void OnPrevPageExecute(Object sender, ExecutedRoutedEventArgs e)
        {
            PageIndex--;
        }

        private void OnSkipPageCanExecute(Object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = PageCount > 0 && e.Parameter != null && Int32.TryParse(e.Parameter.ToString(), out Int32 index);
        }

        private void OnSkipPageExecute(Object sender, ExecutedRoutedEventArgs e)
        {
            var index = Int32.Parse(e.Parameter.ToString());

            if (index < 1)
                index = 1;
            else if (index > PageCount)
                index = PageCount;

            PageIndex = index;
        }

        private void OnPrevMoreExecute(Object sender, ExecutedRoutedEventArgs e)
        {
            var index = PageIndex - 5;

            if (index < 2)
                index = 2;

            PageIndex = index;
        }

        private void OnNextMoreExecute(Object sender, ExecutedRoutedEventArgs e)
        {
            var index = PageIndex + 5;

            if (index >= PageCount)
                index = PageCount - 1;

            PageIndex = index;
        }

        #endregion

        #region Events

        public event EventHandler DisplaySourceChanged;
        public event EventHandler PageIndexChanged;

        #endregion

        /// <summary>
        /// 页码
        /// </summary>
        public struct PageNumber
        {
            /// <summary>
            /// 页码
            /// </summary>
            /// <param name="no">页码</param>
            /// <param name="status">0:普通页|1:当前页|2:向前省略|3:向后省略</param>
            public PageNumber(Int32 no, Int32 status)
            {
                No = no;
                Status = status;
            }

            /// <summary>
            /// 页码
            /// </summary>
            public Int32 No { get; }
            /// <summary>
            /// 0:普通页|1:当前页|2:向前省略|3:向后省略
            /// </summary>
            public Int32 Status { get; }
        }
    }
}
