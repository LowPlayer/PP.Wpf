﻿using PP.Wpf.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static PP.Wpf.Controls.ListPager;

namespace PP.Wpf.Controls
{
    /// <summary>
    /// 分页控件
    /// </summary>
    public sealed class Pager : ContentControl
    {
        #region DependencyProperties

        public static readonly DependencyProperty TotalCountProperty = DependencyProperty.Register("TotalCount", typeof(Int32), typeof(Pager), new PropertyMetadata(OnTotalCountPropertyChanged));

        private static void OnTotalCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Pager)d).OnTotalCountChanged();
        }

        /// <summary>
        /// 条目数
        /// </summary>
        public Int32 TotalCount { get => (Int32)GetValue(TotalCountProperty); set => SetValue(TotalCountProperty, value); }

        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register("PageSize", typeof(Int32), typeof(Pager), new PropertyMetadata(10, new PropertyChangedCallback(OnPageSizePropertyChanged), new CoerceValueCallback(OnPageSizePropertyCoerceValueCallback)), new ValidateValueCallback(OnPageSizePropertyValidateValueCallback));

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
            ((Pager)d).OnPageSizeChanged();
        }

        /// <summary>
        /// 一页大小
        /// </summary>
        public Int32 PageSize { get => (Int32)GetValue(PageSizeProperty); set => SetValue(PageSizeProperty, value); }

        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register("PageIndex", typeof(Int32), typeof(Pager), new PropertyMetadata(1, new PropertyChangedCallback(OnPageIndexPropertyChanged)));

        private static void OnPageIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Pager)d).OnPageIndexChanged();
        }

        /// <summary>
        /// 第几页
        /// </summary>
        public Int32 PageIndex { get => (Int32)GetValue(PageIndexProperty); set => SetValue(PageIndexProperty, value); }

        public static readonly DependencyPropertyKey PageCountPropertyKey = DependencyProperty.RegisterReadOnly("PageCount", typeof(Int32), typeof(Pager), new PropertyMetadata(0));

        public static readonly DependencyProperty PageCountProperty = PageCountPropertyKey.DependencyProperty;
        /// <summary>
        /// 页数
        /// </summary>
        public Int32 PageCount { get => (Int32)GetValue(PageCountProperty); private set => SetValue(PageCountPropertyKey, value); }

        public static readonly DependencyPropertyKey PageNumbersPropertyKey = DependencyProperty.RegisterReadOnly("PageNumbers", typeof(IEnumerable<PageNumber>), typeof(Pager), new PropertyMetadata(default));

        public static readonly DependencyProperty PageNumbersProperty = PageNumbersPropertyKey.DependencyProperty;

        /// <summary>
        /// 页码
        /// </summary>
        public IEnumerable<PageNumber> PageNumbers { get => (IEnumerable<PageNumber>)GetValue(PageNumbersProperty); private set => SetValue(PageNumbersPropertyKey, value); }

        #endregion

        static Pager()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Pager), new FrameworkPropertyMetadata(typeof(Pager)));
        }

        public Pager()
        {
            CommandBindings.Add(new CommandBinding(PageCommands.FirstPage, OnFirstPageExecute, OnFirstPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.LastPage, OnLastPageExecute, OnLastPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.PrevPage, OnPrevPageExecute, OnPrevPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.NextPage, OnNextPageExecute, OnNextPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.SkipPage, OnSkipPageExecute, OnSkipPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.PrevMorePage, OnPrevMoreExecute, OnPrevPageCanExecute));
            CommandBindings.Add(new CommandBinding(PageCommands.NextMorePage, OnNextMoreExecute, OnNextPageCanExecute));
        }

        #region Private Method

        private void OnTotalCountChanged()
        {
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

        private void OnPageSizeChanged()
        {
            PageCount = GetPageCount();

            if (PageIndex != 1)
                PageIndex = 1;
            else
                OnPageIndexChanged();
        }

        private void OnPageIndexChanged()
        {
            PageNumbers = GetPageNumbers();
            PageIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        private int GetPageCount()
        {
            return (Int32)Math.Ceiling((Double)TotalCount / PageSize);
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

        public event EventHandler PageIndexChanged;

        #endregion
    }
}
