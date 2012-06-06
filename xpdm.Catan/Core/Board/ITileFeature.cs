
namespace xpdm.Catan.Core.Board
{
    interface ITileFeature
    {
        HextantDirection Location { get; }
        void Rotate();
    }
}
