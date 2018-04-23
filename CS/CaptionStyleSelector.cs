using DevExpress.Xpf.Docking;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1
{
    public class CaptionStyleSelector : StyleSelector
    {
        public Style AddNewTabStyle { get; set; }
        public override Style SelectStyle(object item, DependencyObject container) {
            if (item is ContentItem && ((ContentItem)item).Content is AddNewTabViewModel)
                return AddNewTabStyle;
            return base.SelectStyle(item, container);
        }
    }
}
