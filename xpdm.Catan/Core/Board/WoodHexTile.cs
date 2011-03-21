using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Catan.Core.Board
{
    class WoodHexTile : HexTile
    {
        public WoodHexTile() : this("A") { }

        public WoodHexTile(string variant) : base(variant)
        {
        }

        public override TileType TileType
        {
            get { return TileType.Wood; }
        }

        public override Resource ResourceCreated
        {
            get
            {
                return Resource.Wood;
            }
        }

        public override Resource CommodityCreated
        {
            get
            {
                return Resource.Paper;
            }
        }
    }
}
