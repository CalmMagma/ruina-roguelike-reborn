using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;

namespace RogueLike_Mod_Reborn
{
    public static class RoguelikeBufs
    {
        public static KeywordBuf CritChance;
    }

    public class BattleUnitBuf_RMR_CritChance : BattleUnitBuf
    {
        public override string keywordId => "RMR_CriticalStrike";
        public override string keywordIconId => "RMRBuf_CriticalStrike";
        public bool onCrit;
        public override KeywordBuf bufType
        {
            get
            {
                return RoguelikeBufs.CritChance;
            }
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            // reset crit here
            onCrit = false;
            base.BeforeRollDice(behavior);
            var critRoll = RandomUtil.Range(0, 100);
            if (critRoll <= this.stack)
            {
                // apply crit = true here
                onCrit = true;
                behavior.ApplyDiceStatBonus(new DiceStatBonus
                {
                    dmgRate = 50,
                    breakRate = 50
                });
            }
        }

    }

}
