using System;
using System.Diagnostics.Contracts;
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
using xpdm.Catan.Controls;
using xpdm.Catan.Core.Board;
using System.Windows.Markup;
using xpdm.Catan.Core;
using System.Diagnostics;

namespace xpdm.Catan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal partial class MainWindow : Window
    {
        int CountX = 9;
        int CountY = 8;
        Tile[][] tiles;

        public MainWindow()
        {
            tiles = new Tile[CountX][];
            for (int i = 0; i < CountX; ++i) {
                tiles[i] = new Tile[CountY];
                for (int j = 0; j < CountY; ++j) {
                    Tile h = new Tile(null, i,j);

                    h.SetValue(FrameworkElement.MarginProperty, new Thickness(h.Position.X, h.Position.Y, 0, 0));
                    h.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                    h.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);
                    //Canvas.SetLeft(h, h.Position.X);
                    //Canvas.SetTop(h, h.Position.Y);

                    tiles[i][j] = h;
                }
            }

            InitializeComponent();
            foreach (var t in AllTiles)
            {
                this.TileLayer.Children.Add(t);
            }
            
            var count = CountX * CountY;
        }

        private static readonly Random RNG = new Random();

        private void RandomBoardLayout(SCG.IEnumerable<Tile> tiles, SCG.IEnumerable<HexTile> hexes, int x, int y, int maxd)
        {
            RandomBoardLayout(tiles, hexes, x, y, x, y, maxd, maxd);
        }
        private void RandomBoardLayout(SCG.IEnumerable<Tile> tiles, SCG.IEnumerable<HexTile> hexes, int x1, int y1, int x2, int y2, int maxd1, int maxd2)
        {
            IList<HexTile> hexRandom = new ArrayList<HexTile>();
            hexRandom.AddAll(from hex in hexes orderby RNG.Next() select hex);
            foreach (var t in tiles)
            {
                t.Chits2.Clear();
                var d1 = this.tiles[x1][y1].DistanceTo(t);
                var d2 = this.tiles[x2][y2].DistanceTo(t);
                if (d1 == maxd1 && d2 >= maxd2 + 1 || d1 >= maxd1 + 1 && d2 == maxd2 || d1 == maxd1 + 1 && d2 == maxd2 + 1)
                {
                    t.HexTile = new OceanHexTile();
                }
                else if (d1 <= maxd1 && d2 <= maxd2)
                {
                    if (hexRandom.Count == 0)
                    {
                        t.HexTile = new DesertHexTile("");
                    }
                    else
                    {
                        t.HexTile = hexRandom.RemoveLast();
                    }
                }
                else
                {
                    t.HexTile = null;
                }
            }
        }

        private void RandomChitLayout(SCG.IEnumerable<Tile> tiles, SCG.IEnumerable<ProductionChit> chits)
        {
            IList<ProductionChit> chitRandom = new ArrayList<ProductionChit>();
            chitRandom.AddAll(from chit in chits orderby RNG.Next() select chit);
            foreach (Tile h in (from t in tiles where t.HexTile != null && t.HexTile.IsLand && t.HexTile.TileType != Core.TileType.Desert select t))
            {
                h.Chits.Add(chitRandom.RemoveLast());
            }
        }

        private void DefaultRandomLayout()
        {
            RandomBoardLayout(AllTiles, HexTile.DefaultTiles, 4, 3, 2);
            RandomChitLayout(AllTiles, ProductionChit.DefaultChits);
        }

        private void ExtendedRandomLayout()
        {
            RandomBoardLayout(AllTiles, HexTile.ExtendedTiles, 4, 3, 4, 4, 3, 3);
            RandomChitLayout(AllTiles, ProductionChit.ExtendedChits);
        }

        private void TwoPlayerRandomLayout()
        {
            RandomBoardLayout(AllTiles, HexTile.TwoPlayerTiles, 4, 2, 4, 3, 2, 2);
            RandomChitLayout(AllTiles, ProductionChit.DefaultChits.Where(c => c.AlphaOrder != "H").Take(14));
            AllTiles.First(t => t.Chits.Exists(c => c.AlphaOrder == "B")).Chits.Add(ProductionChit.DefaultChits.First(c => c.AlphaOrder == "H"));
        }

        private void EnforceCommonChitRule()
        {
            Trace.TraceInformation("Beginning Chit Rule Enforcement");
            try
            {
                Trace.Indent();
                var commonTiles = from tile in AllTiles where tile.Chits.Exists(c => c.IsCommon) select tile;
                var commonLimit = commonTiles.Count();
                var tooClose = (from tile1 in commonTiles from tile2 in commonTiles where tile1.DistanceTo(tile2) == 1 select new { Tile1 = tile1, Tile2 = tile2 }).FirstOrDefault();
                while (tooClose != null && commonLimit > 0)
                {
                    Trace.TraceInformation("Found common chits too close together: ({0},{1}:{2}), ({3},{4}:{5})",
                        tooClose.Tile1.X, tooClose.Tile1.Y, tooClose.Tile1.Chits.First.ProducesOn,
                        tooClose.Tile2.X, tooClose.Tile2.Y, tooClose.Tile2.Chits.First.ProducesOn);
                    var targetLocation = (from tile in AllTiles where commonTiles.All(t => t.DistanceTo(tile) > 1) && !tile.Chits.IsEmpty orderby RNG.Next() select tile).FirstOrDefault();
                    try
                    {
                        Trace.Indent();
                        if (targetLocation == null)
                        {
                            Trace.TraceWarning("Unable to find valid exchange location. Stopping.");
                            break;
                        }
                        var distances = (from tile in commonTiles select tile.DistanceTo(targetLocation)).ToArray();
                        Trace.TraceInformation("Will exchange with ({0},{1}:{2}) Distances: ({3})",
                            targetLocation.X, targetLocation.Y, targetLocation.Chits.First.ProducesOn,
                            string.Join(",", distances));
                        var temp = targetLocation.Chits.ToArray();
                        targetLocation.Chits.Clear();
                        targetLocation.Chits.AddAll(tooClose.Tile1.Chits);
                        tooClose.Tile1.Chits.Clear();
                        tooClose.Tile1.Chits.AddAll(temp);
                        Trace.TraceInformation("Exchanged chits: ({0},{1}:{2}), ({3},{4}:{5})",
                            tooClose.Tile1.X, tooClose.Tile1.Y, tooClose.Tile1.Chits.First.ProducesOn,
                            targetLocation.X, targetLocation.Y, targetLocation.Chits.First.ProducesOn);
                        commonLimit--;
                        tooClose = (from tile1 in commonTiles from tile2 in commonTiles where tile1.DistanceTo(tile2) == 1 select new { Tile1 = tile1, Tile2 = tile2 }).FirstOrDefault();
                    }
                    finally
                    {
                        Trace.Unindent();
                    }
                }
                if (commonLimit == 0)
                {
                    Trace.TraceWarning("More than expected exchanges required. Stopping.");
                }
            }
            finally
            {
                Trace.Unindent();
            }
            Trace.TraceInformation("Done Enforcing Chit Rule");
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            FindAvailableSkins();
        }

        private void FindAvailableSkins()
        {
            var dirs = from dir in System.IO.Directory.EnumerateDirectories("Skins")
                       where System.IO.File.Exists(System.IO.Path.Combine(dir, "Style.xaml"))
                       select new ComboBoxItem{Content=System.IO.Path.GetFileName(dir)};
            foreach (var dir in dirs)
            {
                SkinList.Items.Add(dir);
            }
        }
        
        private void PrintCanvas()
        {
            new PrintDialog().PrintVisual(View, "Settlers Board Game");
        }

        /// <summary>
        /// Test Summary
        /// </summary>
        /// <exception cref="NullReferenceException"/>
        [Pure]
        public SCG.IEnumerable<Tile> AllTiles
        {
            get
            {
                Contract.Ensures(Contract.Result<SCG.IEnumerable<Tile>>() != null);
                return tiles.SelectMany(t => t);
            }
        }

        public static readonly DependencyProperty EnforceChitRuleProperty =
            DependencyProperty.Register("EnforceChitRule", typeof(bool?), typeof(MainWindow), new FrameworkPropertyMetadata((bool?)true));

        public bool? EnforceChitRule
        {
            get { return (bool?)GetValue(MainWindow.EnforceChitRuleProperty); }
            set { SetValue(MainWindow.EnforceChitRuleProperty, value); }
        }

        private void ReLayout()
        {
            switch (LayoutComboBox.SelectedIndex)
            {
                case 1:
                    ExtendedRandomLayout();
                    break;
                case 2:
                    TwoPlayerRandomLayout();
                    break;
                default:
                    DefaultRandomLayout();
                    break;
            }
            if (EnforceChitRule == true)
            {
                EnforceCommonChitRule();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ReLayout();
        }

        private void LayoutComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                ReLayout();
            }
        }
        private void ChitComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void TileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void SkinComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var newSkinName = ((ComboBoxItem)e.AddedItems[0]).Content.ToString();

                var oldSkinDefinition = Application.Current.Properties["CurrentSkin"] as ResourceDictionary;
                if (oldSkinDefinition != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(oldSkinDefinition);
                }
                if (newSkinName != "Default")
                {
                    ResourceDictionary newSkinDefinition = null;
                    var newSkinPath = "Skins/" + newSkinName + "/Style.xaml";
                    using (System.IO.FileStream s = System.IO.File.OpenRead(newSkinPath))
                    {
                        newSkinDefinition = (ResourceDictionary)XamlReader.Load(s, new ParserContext { BaseUri = new Uri("pack://application:,,,/" + newSkinPath, UriKind.Absolute) });
                    }
                    if (newSkinDefinition != null)
                    {
                        Application.Current.Resources.MergedDictionaries.Add(newSkinDefinition);
                        Application.Current.Properties["CurrentSkin"] = newSkinDefinition;
                    }
                }
            }
        }
    }
}
