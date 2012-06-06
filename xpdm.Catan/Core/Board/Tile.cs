using System.Collections.Generic;

namespace xpdm.Catan.Core.Board
{
    class Tile
    {
        public IList<ITileFeature> Features;
        public Landscape Landscape;

        public void RotateFeatures()
        {
        }
    }
}
