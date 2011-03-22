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
            Chit.ProductionChitProperty = DependencyProperty.Register("ProductionChit", typeof (ProductionChit),
                                                                      typeof (Chit), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnChitPropertyChanged)));
            Chit.IsCommonPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsCommon", typeof(bool),
                                                                      typeof(Chit), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
            Chit.IsCommonProperty = Chit.IsCommonPropertyKey.DependencyProperty;
        }

        public static readonly DependencyProperty ProductionChitProperty;

        public ProductionChit ProductionChit
        {
            get { return (ProductionChit) GetValue(Chit.ProductionChitProperty); }
            set { SetValue(Chit.ProductionChitProperty, value); }
        }

        private static readonly DependencyPropertyKey IsCommonPropertyKey;
        public static readonly DependencyProperty IsCommonProperty;

        public bool IsCommon
        {
            get { return (bool) GetValue(Chit.IsCommonProperty); }
            private set { SetValue(Chit.IsCommonPropertyKey, value); }
        }

        public static bool GetIsCommon(Chit chit)
        {
            return (bool) chit.GetValue(Chit.IsCommonProperty);
        }

        public Chit()
        {
            InitializeComponent();
        }

        public Chit(ProductionChit pc) : this()
        {
            ProductionChit = pc;
        }

        private static void OnChitPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as Chit;
            if (c == null)
                return;

            if (e.Property == Chit.ProductionChitProperty)
            {
                c.IsCommon = c.ProductionChit != null ? c.ProductionChit.IsCommon : false;
            }
        }
    }
}
