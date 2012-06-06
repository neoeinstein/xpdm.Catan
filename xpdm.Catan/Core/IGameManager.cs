
namespace xpdm.Catan.Core
{
    interface IGameManager
    {
        T GetGameService<T>() where T : class;
    }
}
