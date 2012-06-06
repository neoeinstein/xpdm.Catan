using System;
using System.Collections.Generic;

namespace xpdm.Catan.Core
{
    class GenerationDieRollEffect : IDieRollEffect
    {
        private IGameManager _gameManager;

        public GenerationDieRollEffect(IGameManager gameManager, IList<int>)
        {
            _gameManager = gameManager;
        }

        public IList<DieResult> EffectiveDieRolls
        {
            get { throw new NotImplementedException(); }
        }

        public void ExecuteEffect()
        {
            var dieResult = _gameManager.GetGameService<IDiceService>().LastStandardDiceRoll;
            if (EffectiveDieRolls.Contains(dieResult[DieType.Yellow] + (int)dieResult[DieType.Red]))
            {
                
            }
        }
    }
}
