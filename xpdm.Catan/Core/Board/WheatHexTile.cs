using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Catan.Core.Board
{
    class WheatHexTile : HexTile
    {
        public WheatHexTile() : this("A") { }

        public WheatHexTile(string variant) : base(variant)
        {
        }

        public override TileType TileType
        {
            get { return TileType.Wheat; }
        }

        public override Resource ResourceCreated
        {
            get
            {
                return Resource.Wheat;
            }
        }
    }
}
