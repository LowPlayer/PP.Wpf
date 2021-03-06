﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace PP.Wpf.Extensions
{
    /// <summary>
    /// UIElement类扩展方法
    /// </summary>
    public static class UIElementExtension
    {
        /// <summary>
        /// Loaded的时候执行，如果IsLoaded=True，立即执行
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="action"></param>
        public static void ExecuteWhenLoaded(this FrameworkElement ele, Action action)
        {
            if (ele.IsLoaded)
                action.Invoke();
            else
            {
                ele.Loaded += OnLoaded;

                void OnLoaded(Object sender, RoutedEventArgs e)
                {
                    ele.Loaded -= OnLoaded;
                    action.Invoke();
                }
            }
        }

        /// <summary>
        /// Unloaded的时候执行
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="action"></param>
        public static void ExecuteWhenUnloaded(this FrameworkElement ele, Action action)
        {
            ele.Unloaded += OnUnloaded;

            void OnUnloaded(Object sender, RoutedEventArgs e)
            {
                ele.Unloaded -= OnUnloaded;
                action.Invoke();
            }
        }

        /// <summary>
        /// 窗口关闭时执行
        /// </summary>
        /// <param name="win"></param>
        /// <param name="action"></param>
        public static void ExecuteWhenClosed(this Window win, Action action)
        {
            win.Closed += OnClosed;

            void OnClosed(Object sender, EventArgs e)
            {
                win.Closed -= OnClosed;
                action.Invoke();
            }
        }

        /// <summary>
        /// 根据类型，查找父节点
        /// </summary>
        /// <typeparam name="T">查找对象类型</typeparam>
        /// <param name="d">查找源点</param>
        /// <param name="filter">筛选条件</param>
        /// <returns>查找对象</returns>
        public static T FindParent<T>(this DependencyObject d, Func<T, Boolean> filter = null) where T : Visual
        {
            if (d == null)
                return null;

            var parent = LogicalTreeHelper.GetParent(d) ?? VisualTreeHelper.GetParent(d);

            while (parent != null)
            {
                if (parent is T t && (filter == null || filter.Invoke(t)))
                    return t;

                parent = LogicalTreeHelper.GetParent(parent) ?? VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary>
        /// 根据Type查找所有子节点
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="root">根节点</param>
        /// <param name="list">查找结果</param>
        /// <param name="filter">筛选条件</param>
        public static void FindChildrenByType<T>(this DependencyObject root, ref List<T> list, Func<T, Boolean> filter = null) where T : Visual
        {
            if (list == null)
                list = new List<T>();

            var count = VisualTreeHelper.GetChildrenCount(root);

            for (int i = 0; i < count; i++)
            {
                var el = VisualTreeHelper.GetChild(root, i);

                if (el == null)
                    continue;

                if (el is T t && (filter == null || filter.Invoke(t)))
                    list.Add(t);
                else
                    FindChildrenByType<T>(el, ref list, filter);
            }
        }

        /// <summary>
        /// 根据Type查找子节点
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="root">根节点</param>
        /// <param name="filter">筛选条件</param>
        /// <returns>查找结果</returns>
        public static T FindChildByType<T>(this DependencyObject root, Func<T, Boolean> filter = null) where T : Visual
        {
            var count = VisualTreeHelper.GetChildrenCount(root);

            for (int i = 0; i < count; i++)
            {
                var el = VisualTreeHelper.GetChild(root, i);

                if (el == null)
                    continue;

                if (el is T t && (filter == null || filter.Invoke(t)))
                    return t;
                else
                {
                    var ret = FindChildByType<T>(el, filter);

                    if (ret != null)
                        return ret;
                }
            }

            return null;
        }
    }
}
