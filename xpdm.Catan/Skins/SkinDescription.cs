using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace xpdm.Catan.Skins
{
    public class SkinDescription : FrameworkElement
    {
        public string DisplayName
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public Visual Preview
        {
            get;
            set;
        }

        public string CopyrightNotice
        {
            get;
            set;
        }

        public string License
        {
            get;
            set;
        }

        public string LicenseUri
        {
            get;
            set;
        }
    }
}
