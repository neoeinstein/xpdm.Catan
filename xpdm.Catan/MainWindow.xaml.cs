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
            var board = new Gameboard(CountY, CountX);
            tiles = new Tile[CountX][];
            for (int i = 0; i < CountX; ++i) {
                tiles[i] = new Tile[CountY];
                for (int j = 0; j < CountY; ++j) {
                    Tile h = new Tile(board, i,j);

                    //h.SetValue(FrameworkElement.MarginProperty, new Thickness(h.Position.X, h.Position.Y, 0, 0));
                    //h.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                    //h.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);

                    h.SetValue(HexagonalGrid.RowProperty, j);
                    h.SetValue(HexagonalGrid.ColumnProperty, i);
                    //Canvas.SetLeft(h, h.Position.X);
                    //Canvas.SetTop(h, h.Position.Y);

                    tiles[i][j] = h;
                }
            }

            InitializeComponent();
            this.TheGrid.Gameboard = board;
            foreach (var t in AllTiles)
            {
                //this.TileLayer.Children.Add(t);
                this.TheGrid.Children.Add(t);
            }
            this.TheGrid.InvalidateMeasure();
            this.TheGrid.InvalidateArrange();
            
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
            foreach (var t in AllTiles) t.Chits.Clear();

            IList<ProductionChit> chitRandom = new ArrayList<ProductionChit>();
            chitRandom.AddAll(from chit in chits orderby RNG.Next() select chit);
            foreach (Tile h in (from t in tiles where t.HexTile != null && t.HexTile.IsLand && t.HexTile.TileType != Core.TileType.Desert select t))
            {
                h.Chits.Add(chitRandom.RemoveLast());
            }
        }

        private SCG.IEnumerable<ProductionChit> GetChitGroup()
        {
            switch(LayoutComboBox.SelectedIndex)
            {
                case 1:
                    return ProductionChit.ExtendedChits;
                case 2:
                    return ProductionChit.DefaultChits.Where(c => c.AlphaOrder != "H").Take(14);
                default:
                    return ProductionChit.DefaultChits;
            }
        }

        private void DefaultRandomLayout()
        {
            RandomBoardLayout(AllTiles, HexTile.DefaultTiles, 4, 3, 2);
        }

        private void ExtendedRandomLayout()
        {
            RandomBoardLayout(AllTiles, HexTile.ExtendedTiles, 4, 3, 4, 4, 3, 3);
        }

        private void TwoPlayerRandomLayout()
        {
            RandomBoardLayout(AllTiles, HexTile.TwoPlayerTiles, 4, 2, 4, 3, 2, 2);
        }

        private static void ExchangeChits(Tile a, Tile b)
        {
            Trace.TraceInformation("Exchanging chits: ({0},{1}:{{{2}}}), ({3},{4}:{{{5}}})",
                b.X, b.Y, string.Join(",", from c in b.Chits select c.ProducesOn),
                a.X, a.Y, string.Join(",", from c in a.Chits select c.ProducesOn));

            var temp = a.Chits.ToArray();
            a.Chits.Clear();
            foreach (var c in b.Chits)
            {
                a.Chits.Add(c);
            }
            b.Chits.Clear();
            foreach (var c in temp)
            {
                b.Chits.Add(c);
            }

            Trace.TraceInformation("Exchanged chits: ({0},{1}:{{{2}}}), ({3},{4}:{{{5}}})",
                b.X, b.Y, string.Join(",", from c in b.Chits select c.ProducesOn),
                a.X, a.Y, string.Join(",", from c in a.Chits select c.ProducesOn));
        }

        private void EnforceCommonChitRule()
        {
            Trace.TraceInformation("Beginning Chit Rule Enforcement");
            try
            {
                Trace.Indent();
                var commonTiles = from tile in AllTiles where tile.Chits.Any(c => c.IsCommon) select tile;
                var commonLimit = commonTiles.Count();
                var tooClose = (from tile1 in commonTiles from tile2 in commonTiles where tile1.DistanceTo(tile2) == 1 orderby RNG.Next() select new { Tile1 = tile1, Tile2 = tile2 }).FirstOrDefault();
                while (tooClose != null && commonLimit > 0)
                {
                    Trace.TraceInformation("Found common chits too close together: ({0},{1}:{{{2}}}), ({3},{4}:{{{5}}})",
                        tooClose.Tile1.X, tooClose.Tile1.Y, string.Join(",", from c in tooClose.Tile1.Chits select c.ProducesOn),
                        tooClose.Tile2.X, tooClose.Tile2.Y, string.Join(",", from c in tooClose.Tile2.Chits select c.ProducesOn));
                    var targetLocation = (from tile in AllTiles where commonTiles.All(t => t.DistanceTo(tile) > 1) && tile.Chits.Count != 0 orderby RNG.Next() select tile).FirstOrDefault();
                    try
                    {
                        Trace.Indent();
                        if (targetLocation == null)
                        {
                            Trace.TraceWarning("Unable to find valid exchange location. Stopping.");
                            break;
                        }

                        var distances = (from tile in commonTiles select tile.DistanceTo(targetLocation)).ToArray();
                        Trace.TraceInformation("Will exchange with ({0},{1}:{{{2}}}) Distances: ({3})",
                            targetLocation.X, targetLocation.Y, string.Join(",", from c in targetLocation.Chits select c.ProducesOn),
                            string.Join(",", distances));
                        ExchangeChits(tooClose.Tile1, targetLocation);

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

        private bool EnsureDesertHasNoChits()
        {
            var didExchange = false;
            Trace.TraceInformation("Ensuring Deserts have no chits");
            try
            {
                Trace.Indent();
                var desertTilesWithChits = from tile in AllTiles where tile.Chits.Count > 0 && tile.HexTile.TileType == TileType.Desert select tile;
                foreach (var t in desertTilesWithChits)
                {
                    var randomNonChittedTile = (from tile in AllTiles where tile.Chits.Count == 0 && tile.HexTile != null && tile.HexTile.IsLand && tile.HexTile.TileType != TileType.Desert orderby RNG.Next() select tile).FirstOrDefault();
                    if (randomNonChittedTile != null)
                    {
                        ExchangeChits(randomNonChittedTile, t);
                        didExchange = true;
                    }
                    else
                    {
                        Trace.TraceWarning("Unable to find chitted non-desert. Abandoning.");
                        break;
                    }
                }
            }
            finally
            {
                Trace.Unindent();
            }
            Trace.TraceInformation("Ensured Deserts have no chits");
            return didExchange;
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
            DependencyProperty.Register("EnforceChitRule", typeof(bool?), typeof(MainWindow), new FrameworkPropertyMetadata((bool?)true, CommonChitRuleEnforcementChanged));

        private static void CommonChitRuleEnforcementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var w = (MainWindow) d;
            if((bool)e.NewValue == true)
                w.EnforceCommonChitRule();
        }

        public bool? EnforceChitRule
        {
            get { return (bool?)GetValue(MainWindow.EnforceChitRuleProperty); }
            set { SetValue(MainWindow.EnforceChitRuleProperty, value); }
        }

        private void ReLayout(bool tiles, bool chits)
        {
            if (!this.IsLoaded)
                return;

            if (tiles)
            {
                if (TilePlacementComboBox.SelectedIndex == 0)
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
                    if (!chits)
                    {
                        if (EnsureDesertHasNoChits() && EnforceChitRule == true)
                        {
                            EnforceCommonChitRule();
                        }
                    }
                }
            }
            if (chits)
            {
                switch (ChitPlacementComboBox.SelectedIndex)
                {
                    case 1:
                        return;
                    default:
                        RandomChitLayout(AllTiles, GetChitGroup());
                        break;
                }
                if (LayoutComboBox.SelectedIndex == 2)
                {
                    AllTiles.First(t => t.Chits.Any(c => c.AlphaOrder == "B")).Chits.Add(ProductionChit.DefaultChits.First(c => c.AlphaOrder == "H"));
                }
                if (EnforceChitRule == true)
                {
                    EnforceCommonChitRule();
                }
            }
        }

        private void ShuffleTilesClicked(object sender, RoutedEventArgs e)
        {
            ReLayout(true, false);
        }
        private void ShuffleChitsClicked(object sender, RoutedEventArgs e)
        {
            ReLayout(false, true);
        }
        private void ShuffleBothClicked(object sender, RoutedEventArgs e)
        {
            ReLayout(true, true);
        }

        private void LayoutComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                ReLayout(true, true);
            }
        }
        private void ChitComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                //ReLayout(false, true);
            }
        }
        private void TileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                //ReLayout(true, false);
            }
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

        private void Self_Loaded(object sender, RoutedEventArgs e)
        {
            ReLayout(true, true);
        }
    }
}
