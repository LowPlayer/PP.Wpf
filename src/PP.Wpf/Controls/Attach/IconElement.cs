using System;
using System.Windows;
using System.Windows.Media;

namespace PP.Wpf.Controls.Attach
{
    /// <summary>
    /// 图标元素
    /// </summary>
    public static class IconElement
    {
        /// <summary>
        /// 矢量图标
        /// </summary>
        public static readonly DependencyProperty GeometryProperty = DependencyProperty.RegisterAttached("Geometry", typeof(Geometry), typeof(IconElement));
        /// <summary>
        /// 获取矢量图标
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Geometry GetGeometry(DependencyObject element) => (Geometry)element.GetValue(GeometryProperty);
        /// <summary>
        /// 设置矢量图标
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetGeometry(DependencyObject element, Geometry value) => element.SetValue(GeometryProperty, value);



        /// <summary>
        /// 选择时矢量图标
        /// </summary>
        public static readonly DependencyProperty GeometrySelectedProperty = DependencyProperty.RegisterAttached("GeometrySelected", typeof(Geometry), typeof(IconElement));
        /// <summary>
        /// 获取选择时矢量图标
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Geometry GetGeometrySelected(DependencyObject element) => (Geometry)element.GetValue(GeometrySelectedProperty);
        /// <summary>
        /// 设置选择时矢量图标
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetGeometrySelected(DependencyObject element, Geometry value) => element.SetValue(GeometrySelectedProperty, value);



        /// <summary>
        /// 图像图标
        /// </summary>
        public static readonly DependencyProperty ImageProperty = DependencyProperty.RegisterAttached("Image", typeof(ImageSource), typeof(IconElement));
        /// <summary>
        /// 设置图像图标
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetImage(DependencyObject element, ImageSource value) => element.SetValue(ImageProperty, value);
        /// <summary>
        /// 获取图像图标
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static ImageSource GetImage(DependencyObject element) => (ImageSource)element.GetValue(ImageProperty);



        /// <summary>
        /// 选中时图像图标
        /// </summary>
        public static readonly DependencyProperty ImageSelectedProperty = DependencyProperty.RegisterAttached("ImageSelected", typeof(ImageSource), typeof(IconElement));
        /// <summary>
        /// 获取选中时图像图标
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static ImageSource GetImageSelected(DependencyObject element) => (ImageSource)element.GetValue(ImageSelectedProperty);
        /// <summary>
        /// 设置选中时图像图标
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetImageSelected(DependencyObject element, ImageSource value) => element.SetValue(ImageSelectedProperty, value);



        /// <summary>
        /// 图标宽度
        /// </summary>
        public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached("Width", typeof(Double), typeof(IconElement), new PropertyMetadata(Double.NaN));
        /// <summary>
        /// 获取图标宽度
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Double GetWidth(DependencyObject element) => (Double)element.GetValue(WidthProperty);
        /// <summary>
        /// 设置图标宽度
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetWidth(DependencyObject element, Double value) => element.SetValue(WidthProperty, value);



        /// <summary>
        /// 图标高度
        /// </summary>
        public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached("Height", typeof(Double), typeof(IconElement), new PropertyMetadata(Double.NaN));
        /// <summary>
        /// 获取图标高度
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Double GetHeight(DependencyObject element) => (Double)element.GetValue(HeightProperty);
        /// <summary>
        /// 设置图标高度
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetHeight(DependencyObject element, Double value) => element.SetValue(HeightProperty, value);



        /// <summary>
        /// 图标内边距
        /// </summary>
        public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached("Margin", typeof(Thickness), typeof(IconElement));
        /// <summary>
        /// 获取图标外边距
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Thickness GetMargin(DependencyObject element) => (Thickness)element.GetValue(MarginProperty);
        /// <summary>
        /// 设置图标外边距
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetMargin(DependencyObject element, Thickness value) => element.SetValue(MarginProperty, value);
    }
}
