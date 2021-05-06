using System.Windows.Input;

namespace PP.Wpf.Commands
{
    public static class PageCommands
    {
        public static readonly RoutedUICommand FirstPage = new RoutedUICommand("首页", "FirstPage", typeof(PageCommands));
        public static readonly RoutedUICommand LastPage = new RoutedUICommand("尾页", "LastPage", typeof(PageCommands));
        public static readonly RoutedUICommand PrevPage = new RoutedUICommand("上一页", "PrevPage", typeof(PageCommands));
        public static readonly RoutedUICommand NextPage = new RoutedUICommand("下一页", "NextPage", typeof(PageCommands));
        public static readonly RoutedUICommand SkipPage = new RoutedUICommand("跳转页面", "SkipPage", typeof(PageCommands));
        public static readonly RoutedUICommand PrevMorePage = new RoutedUICommand("上几页", "PrevMorePage", typeof(PageCommands));
        public static readonly RoutedUICommand NextMorePage = new RoutedUICommand("下几页", "NextMorePage", typeof(PageCommands));
    }
}
