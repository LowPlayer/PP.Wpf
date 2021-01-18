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
    /// ListBoxDragSelectViewerView.xaml 的交互逻辑
    /// </summary>
    public partial class ListBoxDragSelectViewerView : UserControl
    {
        public ListBoxDragSelectViewerView()
        {
            InitializeComponent();

            var nums = new Int32[1000];

            for (var i = 0; i < 1000;)
            {
                nums[i] = i++;
            }

            listbox.ItemsSource = nums;
        }
    }
}
