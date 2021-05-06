using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PP.Wpf.Demo.Views
{
    /// <summary>
    /// ListPagerView.xaml 的交互逻辑
    /// </summary>
    public partial class PagerView : UserControl
    {
        public PagerView()
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

            pager.PageIndexChanged += OnPageIndexChanged;
            pager.TotalCount = datas.Count();
        }

        private void OnPageIndexChanged(Object sender, EventArgs e)
        {
            list.ItemsSource = datas.Skip((pager.PageIndex - 1) * pager.PageSize).Take(pager.PageSize);
        }

        private IEnumerable<Int32> datas = Enumerable.Range(1, 999);
    }
}
