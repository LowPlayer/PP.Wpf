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
    /// ScrollViewerView.xaml 的交互逻辑
    /// </summary>
    public partial class ScrollViewerView : UserControl
    {
        public ScrollViewerView()
        {
            InitializeComponent();
        }

        private void OnRadioClick(object sender, RoutedEventArgs e)
        {
            var styleStr = sender == radio_inline ? "PP.Styles.ScrollViewer.Inline" : "PP.Styles.ScrollViewer.Overlay";

            if (this.FindResource(styleStr) is Style style)
                scroll.Style = style;
        }
    }
}
