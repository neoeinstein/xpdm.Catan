using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Catan.Core.Board
{
    class OceanHexTile : HexTile
    {
        public OceanHexTile() : this("A") { }

        public OceanHexTile(string variant) : base(variant)
        {
        }

        public override TileType TileType
        {
            get { return TileType.Ocean; }
        }

    }
}
