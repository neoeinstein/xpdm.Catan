using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using xpdm.Catan.Core.Board;

namespace xpdm.Catan.Controls
{
    /// <summary>
    /// Interaction logic for Chit.xaml
    /// </summary>
    public partial class Chit : UserControl
    {
        public static DependencyProperty IsHidden;

        static Chit()
        {
        }

        public ProductionChit ProductionChit
        {
            get;
            private set;
        }

        public Chit(ProductionChit pc)
        {
            ProductionChit = pc;
            InitializeComponent();
        }

        public bool IsCommon
        {
            get { return ProductionChit != null ? ProductionChit.IsCommon : false; }
        }
    }
}
