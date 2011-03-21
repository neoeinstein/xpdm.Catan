using System;
using C5;
using SCG=System.Collections.Generic;
using System.Linq;
using System.Text;
using xpdm.Catan.Core;
using System.ComponentModel;

namespace xpdm.Catan.Core.Board
{
    abstract class HexTile : INotifyPropertyChanged
    {
        protected HexTile() : this(String.Empty, String.Empty) { }

        protected HexTile(string variant) : this(variant, String.Empty) { }

        protected HexTile(string variant, string customTileType)
        {
            var lst = new ArrayList<ProductionChit>();
            lst.CollectionChanged += new CollectionChangedHandler<ProductionChit>(ProductionChits_CollectionChanged);
            ProductionChits = lst;
            CustomTileType = customTileType;
            TileVariant = variant;
        }

        public abstract TileType TileType
        {
            get;
        }

        public virtual string CustomTileType
        {
            get;
            private set;
        }

        public virtual string TileVariant
        {
            get;
            private set;
        }

        public virtual bool IsLand
        {
            get
            {
                switch (TileType)
                {
                    case TileType.Brick:
                    case TileType.Desert:
                    case TileType.Gold:
                    case TileType.Ore:
                    case TileType.Sheep:
                    case TileType.Wood:
                    case TileType.Wheat:
                        return true;
                }
                return false;
            }
        }

        public virtual Resource ResourceCreated
        {
            get { return Resource.None; }
        }

        public virtual Resource CommodityCreated
        {
            get { return Resource.None; }
        }

        public ICollection<ProductionChit> ProductionChits
        {
            get;
            private set;
        }

        public bool ProducesOnA(int dieRoll)
        {
            return ProductionChits.Any(c => c.ProducesOn == dieRoll);
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ProductionChits_CollectionChanged(object sender)
        {
            OnPropertyChanged("ProductionChits");
        }

        public static readonly ICollection<HexTile> TwoPlayerTiles = new GuardedCollection<HexTile>(new ArrayList<HexTile>
        {
            new BrickHexTile("A"),
            new BrickHexTile("B"),
            new BrickHexTile("C"),
            new OreHexTile("A"),
            new OreHexTile("B"),
            new SheepHexTile("A"),
            new SheepHexTile("B"),
            new SheepHexTile("C"),
            new WheatHexTile("A"),
            new WheatHexTile("B"),
            new WheatHexTile("C"),
            new WoodHexTile("A"),
            new WoodHexTile("B"),
            new WoodHexTile("C"),
        });
        public static readonly ICollection<HexTile> DefaultTiles = new GuardedCollection<HexTile>(new ArrayList<HexTile>
        {
            new DesertHexTile("A"),
            new BrickHexTile("A"),
            new BrickHexTile("B"),
            new BrickHexTile("C"),
            new OreHexTile("A"),
            new OreHexTile("B"),
            new OreHexTile("C"),
            new SheepHexTile("A"),
            new SheepHexTile("B"),
            new SheepHexTile("C"),
            new SheepHexTile("D"),
            new WheatHexTile("A"),
            new WheatHexTile("B"),
            new WheatHexTile("C"),
            new WheatHexTile("D"),
            new WoodHexTile("A"),
            new WoodHexTile("B"),
            new WoodHexTile("C"),
            new WoodHexTile("D"),
        });
        public static readonly ICollection<HexTile> ExtendedTiles = new GuardedCollection<HexTile>(new ArrayList<HexTile>
        {
            new DesertHexTile("A"),
            new DesertHexTile("B"),
            new BrickHexTile("A"),
            new BrickHexTile("B"),
            new BrickHexTile("C"),
            new BrickHexTile("D"),
            new BrickHexTile("E"),
            new OreHexTile("A"),
            new OreHexTile("B"),
            new OreHexTile("C"),
            new OreHexTile("D"),
            new OreHexTile("E"),
            new SheepHexTile("A"),
            new SheepHexTile("B"),
            new SheepHexTile("C"),
            new SheepHexTile("D"),
            new SheepHexTile("E"),
            new SheepHexTile("F"),
            new WheatHexTile("A"),
            new WheatHexTile("B"),
            new WheatHexTile("C"),
            new WheatHexTile("D"),
            new WheatHexTile("E"),
            new WheatHexTile("F"),
            new WoodHexTile("A"),
            new WoodHexTile("B"),
            new WoodHexTile("C"),
            new WoodHexTile("D"),
            new WoodHexTile("E"),
            new WoodHexTile("F"),
        });

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
