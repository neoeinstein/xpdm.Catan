using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Catan.Core.Board
{
    class BrickHexTile : HexTile
    {
        public BrickHexTile() : this("A") { }

        public BrickHexTile(string variant) : base(variant)
        {
        }

        public override TileType TileType
        {
            get { return TileType.Brick; }
        }

        public override Resource ResourceCreated
        {
            get
            {
                return Resource.Brick;
            }
        }
    }
}
