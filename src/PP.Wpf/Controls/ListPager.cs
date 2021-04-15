using PP.Wpf.Commands;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 列表分页控件
    /// </summary>
    public sealed class ListPager : Control
    {
        #region DependencyProperties

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(IEnumerable), typeof(ListPager), new PropertyMetadata(OnSourcePropertyChanged));

        private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ListPager)d).OnSourceChanged((IEnumerable)e.NewValue);
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public IEnumerable Source { get => (IEnumerable)GetValue(SourceProperty); set => SetValue(SourceProperty, value); }

        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register("PageSize", typeof(Int32), typeof(ListPager), new PropertyMetadata(10, new PropertyChangedCallback(OnPageSizePropertyChanged), new CoerceValueCallback(OnPageSizePropertyCoerceValueCallback)));

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

        public static readonly DependencyPropertyKey PageCountPropertyKey = DependencyProperty.RegisterReadOnly("PageCountPropertyKey", typeof(Int32), typeof(ListPager), new PropertyMetadata(0));

        public static readonly DependencyProperty PageCountProperty = PageCountPropertyKey.DependencyProperty;
        /// <summary>
        /// 页数
        /// </summary>
        public Int32 PageCount { get => (Int32)GetValue(PageCountProperty); private set => SetValue(PageCountPropertyKey, value); }

        public static readonly DependencyPropertyKey DisplaySourcePropertyKey = DependencyProperty.RegisterReadOnly("DisplaySource", typeof(IEnumerable), typeof(ListPager), new PropertyMetadata(default));

        public static readonly DependencyProperty DisplaySourceProperty = DisplaySourcePropertyKey.DependencyProperty;
        /// <summary>
        /// 展示的数据源
        /// </summary>
        public IEnumerable DisplaySource { get => (IEnumerable)GetValue(DisplaySourceProperty); private set => SetValue(DisplaySourcePropertyKey, value); }

        #endregion

        static ListPager()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListPager), new FrameworkPropertyMetadata(typeof(ListPager)));
        }

        public ListPager()
        {
            CommandBindings.Add(new CommandBinding(PageCommands.FirstPage, OnFirstPageExecute, OnFirstPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.LastPage, OnLastPageExecute, OnLastPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.NextPage, OnNextPageExecute, OnNextPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.PrevPage, OnPrevPageExecute, OnPrevPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.SkipPage, OnSkipPageExecute, OnSkipPageCanExecute));
        }

        #region Private Methods

        private void OnSourceChanged(IEnumerable source)
        {
            PageCount = GetPageCount();

            if (PageIndex != 1)
                PageIndex = 1;
            else
                OnPageIndexChanged();
        }

        private void OnPageSizeChanged()
        {
            DisplaySource = GetDisplaySource();
        }

        private void OnPageIndexChanged()
        {
            DisplaySource = GetDisplaySource();
        }

        private Int32 GetPageCount()
        {
            if (Source == null)
                return 0;

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

            return (Int32)Math.Ceiling((Double)count / PageSize);
        }

        private IEnumerable GetDisplaySource()
        {
            var start = (PageIndex - 1) * PageSize;
            var end = start + PageSize;

            if (Source is IList list)
            {
                for (var i = start; i <= end; i++)
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
            e.CanExecute = PageCount > 0 && e.Parameter != null && Int32.TryParse(e.Parameter.ToString(), out Int32 index) && index >= 1 && index <= PageCount;
        }

        private void OnSkipPageExecute(Object sender, ExecutedRoutedEventArgs e)
        {
            PageIndex = Int32.Parse(e.Parameter.ToString());
        }

        #endregion
    }
}
