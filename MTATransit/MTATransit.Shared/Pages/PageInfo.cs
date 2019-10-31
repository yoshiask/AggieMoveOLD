using Windows.UI.Xaml.Controls;

namespace MTATransit.Shared.Pages
{
    public class PageInfo
    {
        public string Title { get; set; }
        public string Subhead { get; set; }
        public Symbol Icon { get; set; }
        public System.Type PageType { get; set; }
        public string Tooltip { get; set; }

        public NavigationViewItem NavViewItem {
            get {
                var item = new NavigationViewItem()
                {
                    Icon = new SymbolIcon(Icon),
                    Content = Title,
                    FontFamily = Common.DINFont
                };
                ToolTipService.SetToolTip(item, new ToolTip() { Content = Tooltip });

                return item;
            }
        }
    }
}
