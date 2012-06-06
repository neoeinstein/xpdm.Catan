using System;
using System.Collections.Generic;

namespace xpdm.Catan.Core
{
    class GameManager : IGameManager
    {
        private Dictionary<Type, object> _gameServices = new Dictionary<Type, object>();

        public T GetGameService<T>() where T : class
        {
            return _gameServices[typeof(T)] as T;
        }

        public bool IsGameServiceInitialized<T>()
        {
            return _gameServices.ContainsKey(typeof(T));
        }

        public void InitializeGameService<T>(T gameService)
        {
            _gameServices.Add(typeof(T), gameService);
        }
    }
}
