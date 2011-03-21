using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Catan.Core.Board
{
    class SheepHexTile : HexTile
    {
        public SheepHexTile() : this("A") { }

        public SheepHexTile(string variant) : base(variant)
        {
        }

        public override TileType TileType
        {
            get { return TileType.Sheep; }
        }

        public override Resource ResourceCreated
        {
            get
            {
                return Resource.Sheep;
            }
        }

        public override Resource CommodityCreated
        {
            get
            {
                return Resource.Cloth;
            }
        }
    }
}
