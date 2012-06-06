using System.Windows.Controls;
using xpdm.Catan.Core.Board;

namespace xpdm.Catan
{
    class BoardDesignerViewModel
    {
        private Gameboard gameboard;

        public UIElementCollection AllTiles
        {
            get { return _allTiles; }
        }
    }
}
