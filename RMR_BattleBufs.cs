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
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            if (behavior.owner?.currentDiceAction?.target != null)
            {
                var target = behavior.owner?.currentDiceAction?.target;
                var critRoll = RandomUtil.Range(0, 100);
                if (critRoll <= this.stack)
                {
                    onCrit = true;
                }
                if (onCrit)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus
                    {
                        dmgRate = 50,
                        breakRate = 50
                    });
                    GlobalLogueEffectManager.Instance.OnCrit(_owner, target);
                }
            }
        }

        public override void OnSuccessAttack(BattleDiceBehavior behavior)
        {
            base.OnSuccessAttack(behavior);
            // reset crit here
            onCrit = false;

        }
    }

}
