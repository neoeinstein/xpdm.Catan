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
            Tile.Chits2PropertyKey = DependencyProperty.RegisterReadOnly("Chits2", typeof(IList<ProductionChit>), typeof(Tile), new FrameworkPropertyMetadata(new ArrayList<ProductionChit>()));
            Tile.Chits2Property = Tile.Chits2PropertyKey.DependencyProperty;
            Tile.HexTileProperty = DependencyProperty.Register("HexTile", typeof(HexTile), typeof(Tile), new FrameworkPropertyMetadata(new EmptyHexTile(), FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnHexTilePropertyChanged)));
            Tile.HexTileBackgroundProperty = DependencyProperty.Register("HexTileBackground", typeof(Brush), typeof(Tile), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        public IList<ProductionChit> Chits = new ArrayList<ProductionChit>();

        public Tile(Gameboard gameboard, int X, int Y)
        {
            
            var lst = new ArrayList<ProductionChit>();
            Chits.CollectionChanged += Chits_CollectionChanged;
            lst.CollectionChanged += new CollectionChangedHandler<ProductionChit>(Chits_CollectionChanged);
            SetValue(Tile.Chits2PropertyKey, lst);

            this.X = X;
            this.Y = Y;

            var offsetX = 192 * X;
            var offsetY = heightConstant * 256 * Y;
            if (X % 2 == 0)
                offsetY += heightConstant * 128;

            this.Position = new Point(offsetX, offsetY);

            InitializeComponent();
            this.Location.Text = X + "," + Y;

            /*var points = new [] {
                new Point(0, heightConstant),
                new Point(.5, 0),
                new Point(1.5, 0),
                new Point(2, heightConstant),
                new Point(1.5, heightConstant * 2),
                new Point(1, heightConstant * 2),
            };
            var t = from point in points select new Point(point.X * Width / 2, point.Y * Height / heightConstant * 2);*/
            //Hex.Points = new PointCollection(t);
        }

        void Chits_CollectionChanged(object sender)
        {
            ChitTest.Items.Clear();
            foreach (var chit in Chits)
            {
                ChitTest.Items.Add(new Chit(chit));
            }
            ChitTest.InvalidateArrange();
/*            ChitSpace.Children.Clear();
            var count = Chits.Count;
            Chit chit;
            switch (Chits.Count)
            {
                case 0:
                    break;
                case 1:
                    chit = new Chit(Chits[0]);
                    Grid.SetColumn(chit, 0);
                    Grid.SetRow(chit, 0);
                    Grid.SetColumnSpan(chit, 2);
                    Grid.SetRowSpan(chit, 2);
                    ChitSpace.Children.Add(chit);
                    break;
                case 2:
                    chit = new Chit(Chits[0]);
                    Grid.SetColumn(chit, 0);
                    Grid.SetRow(chit, 0);
                    Grid.SetRowSpan(chit, 2);
                    ChitSpace.Children.Add(chit);
                    chit = new Chit(Chits[1]);
                    Grid.SetColumn(chit, 1);
                    Grid.SetRow(chit, 0);
                    Grid.SetRowSpan(chit, 2);
                    ChitSpace.Children.Add(chit);
                    break;
                case 3:
                    chit = new Chit(Chits[0]);
                    Grid.SetColumn(chit, 0);
                    Grid.SetRow(chit, 0);
                    ChitSpace.Children.Add(chit);
                    chit = new Chit(Chits[1]);
                    Grid.SetColumn(chit, 1);
                    Grid.SetRow(chit, 0);
                    ChitSpace.Children.Add(chit);
                    chit = new Chit(Chits[2]);
                    Grid.SetColumn(chit, 0);
                    Grid.SetRow(chit, 1);
                    Grid.SetColumnSpan(chit, 2);
                    ChitSpace.Children.Add(chit);
                    break;
                default:
                    chit = new Chit(Chits[0]);
                    Grid.SetColumn(chit, 0);
                    Grid.SetRow(chit, 0);
                    ChitSpace.Children.Add(chit);
                    chit = new Chit(Chits[1]);
                    Grid.SetColumn(chit, 1);
                    Grid.SetRow(chit, 0);
                    ChitSpace.Children.Add(chit);
                    chit = new Chit(Chits[2]);
                    Grid.SetColumn(chit, 0);
                    Grid.SetRow(chit, 1);
                    ChitSpace.Children.Add(chit);
                    chit = new Chit(Chits[3]);
                    Grid.SetColumn(chit, 1);
                    Grid.SetRow(chit, 1);
                    ChitSpace.Children.Add(chit);
                    break;
            }

            /*
            for (int i = 0; i < Chits.Count; ++i)
            {
                var chit = new Chit(Chits[i]);
                Grid.SetColumn(chit, i % 2);
                Grid.SetRow(chit, i / 2);
                if (i == Chits.Count - 1 && i % 2 == 0)
                    Grid.SetColumnSpan(chit, 2);
                ChitSpace.Children.Add(chit);
            }*/
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

        /*public Brush Fill
        {
            get { return Hex.Fill; }
            set
            {
                if (value == null)
                    Hex.Stroke = null;
                else
                    Hex.Stroke = Brushes.LightGray;
                Hex.Fill = value;
            }
        }*/

        private string CalculateHexTileBackground(HexTile tile)
        {
            if (tile == null)
                return "Transparent";

            return tile.TileType.ToString() + tile.CustomTileType + tile.TileVariant + "TileBackground";

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
            }
            //return back;
        }

        private static readonly DependencyPropertyKey Chits2PropertyKey;
        public static readonly DependencyProperty Chits2Property;

        public IList<ProductionChit> Chits2
        {
            get { return (IList<ProductionChit>)GetValue(Tile.Chits2Property); }
        }

        public static readonly DependencyProperty HexTileProperty;

        public HexTile HexTile
        {
            get { return (HexTile)GetValue(Tile.HexTileProperty); }
            set 
            {
                SetValue(Tile.HexTileProperty, value);
                Hex.SetResourceReference(Polygon.FillProperty, CalculateHexTileBackground(value));
                //SetResourceReference(Tile.HexTileBackgroundProperty, CalculateHexTileBackground(value));
            }
        }

        private static void OnHexTilePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ((HexTile)e.OldValue).PropertyChanged -= HexTileChitsChanged;
            }
            if (e.NewValue != null)
            {
                ((HexTile)e.NewValue).PropertyChanged += HexTileChitsChanged;
            }
        }

        private static void HexTileChitsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        public static readonly DependencyProperty HexTileBackgroundProperty;

        public Brush HexTileBackground
        {
            get { return (Brush)GetValue(Tile.HexTileBackgroundProperty); }
        }
    }
}
