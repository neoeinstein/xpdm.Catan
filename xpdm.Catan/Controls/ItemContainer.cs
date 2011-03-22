using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace xpdm.Catan.Controls
{
    public class ItemContainer : ContentControl
    {
        static ItemContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemContainer), new FrameworkPropertyMetadata(typeof(ItemContainer)));
        }
    }
}
