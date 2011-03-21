using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Catan.Core.Board
{
    class OreHexTile : HexTile
    {
        public OreHexTile() : this("A") { }

        public OreHexTile(string variant) : base(variant)
        {
        }

        public override TileType TileType
        {
            get { return TileType.Ore; }
        }

        public override Resource ResourceCreated
        {
            get
            {
                return Resource.Ore;
            }
        }

        public override Resource CommodityCreated
        {
            get
            {
                return Resource.Coin;
            }
        }
    }
}
