using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using xpdm.Catan.Core.Board;

namespace xpdm.Catan.Controls
{
    [ValueConversion(typeof(ProductionChit), typeof(Chit))]
    public class ProductionChitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var pc = value as ProductionChit;
            if (pc == null)
                return null;
            return new Chit(pc);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var chit = value as Chit;
            if (chit == null)
                return null;
            return chit.ProductionChit;
        }
    }
}
