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
    /// ListPagerView.xaml 的交互逻辑
    /// </summary>
    public partial class ListPagerView : UserControl
    {
        public ListPagerView()
        {
            InitializeComponent();

            var dic = new Dictionary<Int32, String>
            {
                [10] = "10条/页",
                [20] = "20条/页",
                [30] = "30条/页",
                [40] = "40条/页",
            };

            combo.ItemsSource = dic;
            combo.SelectedValue = 10;

            pager.Source = Enumerable.Range(1, 55);
        }
    }
}
