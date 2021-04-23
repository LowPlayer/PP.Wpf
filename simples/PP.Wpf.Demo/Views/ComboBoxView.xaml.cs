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
    /// ComboBoxView.xaml 的交互逻辑
    /// </summary>
    public partial class ComboBoxView : UserControl
    {
        public ComboBoxView()
        {
            InitializeComponent();

            var typeDic = new Dictionary<int, string>
            {
                [1] = "打包工具",
                [2] = "客服系统",
                [3] = "应用服务器信息"
            };

            cb1.DisplayMemberPath = "Value";
            cb1.SelectedValuePath = "Key";
            cb1.ItemsSource = typeDic;
        }
    }
}
