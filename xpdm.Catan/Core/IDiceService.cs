using System.Collections.Generic;

namespace xpdm.Catan.Core
{
    interface IDiceService
    {
        IList<IDictionary<DieType, DieResult>> DiceRollHistory { get; }
        IList<DieType> StandardDice { get; set; }
        IDictionary<DieType, DieResult> RollStandardDice();
        IDictionary<DieType, DieResult> LastStandardDiceRoll { get; }
        IDictionary<DieType, DieResult> RollDice(IEnumerable<DieType> dieTypes);
    }
}
