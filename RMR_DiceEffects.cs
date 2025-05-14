using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;

namespace RogueLike_Mod_Reborn
{
    public class DiceCardAbility_RMR_RecycleOnClashLose : DiceCardAbilityBase
    {
        public override void OnLoseParrying()
        {
            base.OnLoseParrying();
            ActivateBonusAttackDice();
        }

        // public static string Desc = "[On Clash Lose] Recycle this die";
    }
}
