using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Catan.Core.Board
{
    class DesertHexTile : HexTile
    {
        public DesertHexTile() : this("A") { }

        public DesertHexTile(string variant) : base(variant)
        {
        }

        public override TileType TileType
        {
            get { return TileType.Desert; }
        }
    }
}
