using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Catan.Core.Board
{
    class GoldHexTile : HexTile
    {
        public GoldHexTile() : this("A") { }

        public GoldHexTile(string variant) : base(variant)
        {
        }

        public override TileType TileType
        {
            get { return TileType.Gold; }
        }

        public override Resource ResourceCreated
        {
            get
            {
                return Resource.AnyResource;
            }
        }
    }
}
