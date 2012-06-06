using System.Collections.Generic;

namespace xpdm.Catan.Core
{
    interface IDieRollEffect
    {
        IList<DieResult> EffectiveDieRolls { get; };
        void ExecuteEffect();
    }
}
