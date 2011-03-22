using System;
using C5;
using SCG = System.Collections.Generic;
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
using System.Collections.ObjectModel;

namespace xpdm.Catan.Controls
{
    /// <summary>
    /// Interaction logic for Hexagon.xaml
    /// </summary>
    internal partial class Tile : UserControl
    {
        // Sqrt(.75)
        private const double heightConstant = 0.86602540378443864676372317075293618347140262690519031402790348972596650845440001854057309337862428783781307070770335151498497254749947623940582775604718682426404661595115279103398741005054233746163250765617163345166144332533612733;

        static Tile()
        {
            Tile.ChitsPropertyKey = DependencyProperty.RegisterReadOnly("Chits", typeof(ObservableCollection<ProductionChit>), typeof(Tile), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, OnDependencyPropertyChanged));
            Tile.ChitsProperty = Tile.ChitsPropertyKey.DependencyProperty;
            Tile.HexTileProperty = DependencyProperty.Register("HexTile", typeof(HexTile), typeof(Tile), new FrameworkPropertyMetadata(new EmptyHexTile(), FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnDependencyPropertyChanged)));
        }

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static readonly DependencyPropertyKey ChitsPropertyKey;
        public static readonly DependencyProperty ChitsProperty;

        public SCG.IList<ProductionChit> Chits
        {
            get { return (ObservableCollection<ProductionChit>) GetValue(Tile.ChitsProperty); }
            private set { SetValue(Tile.ChitsPropertyKey, value); }
        }

        public Tile(Gameboard gameboard, int X, int Y)
        {
            Chits = new ObservableCollection<ProductionChit>();
            //Chits.CollectionChanged += Chits_CollectionChanged;

            this.X = X;
            this.Y = Y;

            var offsetX = 192 * X;
            var offsetY = heightConstant * 256 * Y;
            if (X % 2 == 0)
                offsetY += heightConstant * 128;

            this.Position = new Point(offsetX, offsetY);

            InitializeComponent();
            this.Location.Text = X + "," + Y;
        }

        public bool IsOffset
        {
            get { return (X % 2 == 0); }
        }

        public int DistanceTo(Tile h)
        {
            return DistanceTo(h.X, h.Y);
        }

        public int DistanceTo(int x, int y)
        {
            var dY = Math.Abs(y - Y);
            if (x == X)
                return dY;

            var dX = Math.Abs(x - X);
            if (y == Y)
                return dX;

            var d = (!IsOffset && (x % 2 == 0) && y < Y || IsOffset && (x % 2 != 0) && y > Y);
            return dX + dY - Math.Min(dX, (d ? 0 : 2) + dY) / 2 - (d ? 1 : 0);
        }

        public Point Position
        {
            get;
            private set;
        }

        public int X
        {
            get;
            private set;
        }

        public int Y
        {
            get;
            private set;
        }

        private object CalculateHexTileBackground(HexTile tile)
        {
            if (tile == null)
                return Brushes.Transparent;

            return tile.TileType.ToString() + tile.CustomTileType + tile.TileVariant + "TileBackground";
            /*
            var back = TryFindResource(tile.TileType.ToString() + tile.CustomTileType + tile.TileVariant + "TileBackground") as Brush;
            if (back == null)
            {
                back = TryFindResource(tile.TileType.ToString() + tile.CustomTileType + "TileBackground") as Brush;
            }
            if (back == null)
            {
                back = TryFindResource(tile.TileType.ToString() + "TileBackground") as Brush;
            }
            if (back == null)
            {
                back = TryFindResource("TileBackground") as Brush;
            }
            if (back == null)
            {
                System.Diagnostics.Trace.TraceWarning("Unable to find background resource '{0}'", tile.TileType.ToString() + tile.CustomTileType + tile.TileVariant + "TileBackground");
                return "Transparent";
            }*/
            //return back;
        }

        private static Binding TryCreateBinding(FrameworkElement e, string resourceName)
        {
            var resource = e.TryFindResource(resourceName);
            if (resource != null)
                return new Binding { Source = resource, IsAsync = true };
            return null;
        }

        private const string HighQualityResourceQualifier = "HighQuality";

        private static Binding TryCreateHighQualityBinding(FrameworkElement e, string resourceName)
        {
            return TryCreateBinding(e, resourceName + HighQualityResourceQualifier);
        }

        private BindingBase CreateHexTileBackgroundBinding(HexTile tile)
        {
            var baseBinding = new PriorityBinding();

            if (tile != null)
            {
                BindingBase bind;

                bind = TryCreateHighQualityBinding(this, tile.TileType.ToString() + tile.CustomTileType + tile.TileVariant + "TileBackground");
                if (bind != null)
                    baseBinding.Bindings.Add(bind);
                
                bind = TryCreateBinding(this, tile.TileType.ToString() + tile.CustomTileType + tile.TileVariant + "TileBackground");
                if (bind != null)
                    baseBinding.Bindings.Add(bind);

                bind = TryCreateHighQualityBinding(this, tile.TileType.ToString() + tile.CustomTileType + "TileBackground");
                if (bind != null)
                    baseBinding.Bindings.Add(bind);

                bind = TryCreateBinding(this, tile.TileType.ToString() + tile.CustomTileType + "TileBackground");
                if (bind != null)
                    baseBinding.Bindings.Add(bind);

                bind = TryCreateHighQualityBinding(this, tile.TileType.ToString() + "TileBackground");
                if (bind != null)
                    baseBinding.Bindings.Add(bind);

                bind = TryCreateBinding(this, tile.TileType.ToString() + "TileBackground");
                if (bind != null)
                    baseBinding.Bindings.Add(bind);

                bind = TryCreateHighQualityBinding(this, "TileBackground");
                if (bind != null)
                    baseBinding.Bindings.Add(bind);

                bind = TryCreateBinding(this, "TileBackground");
                if (bind != null)
                    baseBinding.Bindings.Add(bind);

            }

            baseBinding.Bindings.Add(new Binding { Source = Brushes.Red });
            //baseBinding.FallbackValue = Brushes.Transparent;

            return baseBinding;
        }

        public static readonly DependencyProperty HexTileProperty;

        public HexTile HexTile
        {
            get { return (HexTile)GetValue(Tile.HexTileProperty); }
            set 
            {
                SetValue(Tile.HexTileProperty, value);
                //Hex.ClearValue(Polygon.FillProperty);
                //Hex.SetBinding(Polygon.FillProperty, CreateHexTileBackgroundBinding(value));
                
                var bg = CalculateHexTileBackground(value);
                if (bg is string)
                {
                    Hex.SetResourceReference(Polygon.FillProperty, CalculateHexTileBackground(value));
                }
                else
                {
                    Hex.Fill = bg as Brush;
                }
            }
        }
    }
}
