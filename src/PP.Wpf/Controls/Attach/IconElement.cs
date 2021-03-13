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
        /// 画刷
        /// </summary>
        public static readonly DependencyProperty BrushProperty = DependencyProperty.RegisterAttached("Brush", typeof(Brush), typeof(IconElement));
        /// <summary>
        /// 获取画刷
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Brush GetBrush(DependencyObject element) => (Brush)element.GetValue(BrushProperty);
        /// <summary>
        /// 设置画刷
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetBrush(DependencyObject element, Brush value) => element.SetValue(BrushProperty, value);



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
        /// 鼠标悬浮时图像图标
        /// </summary>
        public static readonly DependencyProperty HoverImageProperty = DependencyProperty.RegisterAttached("HoverImage", typeof(ImageSource), typeof(IconElement));
        /// <summary>
        /// 获取鼠标悬浮时图像图标
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static ImageSource GetHoverImage(DependencyObject element) => (ImageSource)element.GetValue(HoverImageProperty);
        /// <summary>
        /// 设置鼠标悬浮时图像图标
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetHoverImage(DependencyObject element, ImageSource value) => element.SetValue(HoverImageProperty, value);



        /// <summary>
        /// 鼠标按压时图像图标
        /// </summary>
        public static readonly DependencyProperty PressedImageProperty = DependencyProperty.RegisterAttached("PressedImage", typeof(ImageSource), typeof(IconElement));
        /// <summary>
        /// 获取鼠标按压时图像图标
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static ImageSource GetPressedImage(DependencyObject element) => (ImageSource)element.GetValue(PressedImageProperty);
        /// <summary>
        /// 设置鼠标按压时图像图标
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetPressedImage(DependencyObject element, ImageSource value) => element.SetValue(PressedImageProperty, value);



        /// <summary>
        /// 不可用时图像图标
        /// </summary>
        public static readonly DependencyProperty DisabledImageProperty = DependencyProperty.RegisterAttached("DisabledImage", typeof(ImageSource), typeof(IconElement));
        /// <summary>
        /// 获取不可用时图像图标
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static ImageSource GetDisabledImage(DependencyObject element) => (ImageSource)element.GetValue(DisabledImageProperty);
        /// <summary>
        /// 设置不可用时图像图标
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetDisabledImage(DependencyObject element, ImageSource value) => element.SetValue(DisabledImageProperty, value);



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



        /// <summary>
        /// 平铺模式
        /// </summary>
        public static readonly DependencyProperty StretchProperty = DependencyProperty.RegisterAttached("Stretch", typeof(Stretch), typeof(IconElement), new PropertyMetadata(Stretch.Uniform));
        /// <summary>
        /// 获取平铺模式
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Stretch GetStretch(DependencyObject element) => (Stretch)element.GetValue(StretchProperty);
        /// <summary>
        /// 设置平铺模式
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetStretch(DependencyObject element, Stretch value) => element.SetValue(StretchProperty, value);



        /// <summary>
        /// 是否选中
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty=DependencyProperty.RegisterAttached("IsSelected", typeof(Boolean), typeof(IconElement));
        /// <summary>
        /// 获取是否选中
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Boolean GetIsSelected(DependencyObject element) => (Boolean)element.GetValue(IsSelectedProperty);
        /// <summary>
        /// 设置是否选中
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetIsSelected(DependencyObject element, Boolean value) => element.SetValue(IsSelectedProperty, value);
    }
}
