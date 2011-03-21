using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Catan.Core.Board
{
    class EmptyHexTile : HexTile
    {
        public override TileType TileType
        {
            get { return TileType.None; }
        }
    }
}
