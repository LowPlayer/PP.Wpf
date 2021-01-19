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
    /// AutoGridCanvasView.xaml 的交互逻辑
    /// </summary>
    public partial class AutoGridCanvasView : UserControl
    {
        public AutoGridCanvasView()
        {
            InitializeComponent();

            btn.Click += OnClicked;
        }

        private void OnClicked(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(input.Text, out Int32 num))
            {
                var array = new Int32[num];

                for (var i = 0; i < num;)
                {
                    array[i] = ++i;
                }

                agc.ItemsSource = array;
            }
        }
    }
}
