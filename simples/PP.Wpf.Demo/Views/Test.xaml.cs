using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PP.Wpf.Demo.Views
{
    /// <summary>
    /// Test.xaml 的交互逻辑
    /// </summary>
    public partial class Test : UserControl
    {
        public Test()
        {
            InitializeComponent();
        }

        private Point CoordMap(Double x, Double y, Double r, Double a)
        {
            var ta = (360 - a) * Math.PI / 180;
            var tx = r * Math.Cos(ta); // 角度邻边
            var ty = r * Math.Sin(ta); // 角度的对边
            return new Point(x + tx, y - ty);
        }

        private void DrawPath(double progress)
        {
            var r = 100;    // 半径

            // 计算当前的进度对应的角度值
            var degrees = progress * 360;

            if (progress == 1)
            {
                var descriptions = $"M0 0 M{2 * r} {2 * r} M{r} 0 A{r} {r} 0 0 1 {r} {r * 2} M{r} 0 A{r} {r} 0 0 0 {r} {r * 2}";
                path.Data = Geometry.Parse(descriptions);
            }
            else
            {
                var s = CoordMap(r, r, r, -90);
                var e = CoordMap(r, r, r, degrees - 90);

                var lenghty = degrees > 180 ? 1 : 0;

                var descriptions = $"M0 0 M{2 * r} {2 * r} M{s.X} {s.Y} A{r} {r} 0 {lenghty} 1 {e.X} {e.Y}";
                path.Data = Geometry.Parse(descriptions);
            }

            //// 计算当前角度对应的弧度值
            //var rad = (360 - degrees) * Math.PI / 180;

            ////极坐标转换成直角坐标
            //var tx = r * Math.Cos(rad); // 角度邻边
            //var ty = r * Math.Sin(rad); // 角度的对边

            //var x = r + tx;
            //var y = r - ty;

            ////大于180度时候画大角度弧，小于180度的画小角度弧，(deg > 180) ? 1 : 0
            //var lenghty = degrees > 180 ? 1 : 0;

            ////path 属性
            //var descriptions = $"M{r} {r} A{r} {r} 0 {lenghty} 1 {x} {y}";

            // 给path 设置属性

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var val = (Double)e.NewValue;
            DrawPath(val / 100);
        }
    }
}
