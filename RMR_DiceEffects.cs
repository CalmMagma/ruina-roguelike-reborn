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

    public class DiceCardAbility_RMR_Recover2HPSRatk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Recover_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            owner.RecoverHP(2);
            owner.breakDetail.RecoverBreak(2);
        }
    }

    public class DiceCardAbility_RMR_BackstreetsDashDie : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                breakDmg = behavior.TargetDice.DiceResultValue
            });
        }
    }

    public class DiceCardAbility_RMR_Add2PowerAllpw : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                power = 2
            });
        }
    }

    public class DiceCardAbility_RMR_whythefuckistherenoendurance1atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Endurance_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Endurance, 1, owner);
        }
    }

    public class DiceCardAbility_RMR_whythefuckistherenobleeding1pw : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 1, owner);
        }
    }

    public class DiceCardAbility_RMR_CrueltyDie : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[2] { "Bleeding_Keyword", "Recover_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            int? num = base.card.target?.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding);
            if (num > 0)
            {
                if (num > 30)
                {
                    num = 30;
                }
                base.owner.RecoverHP(num ?? 0);
                base.owner.breakDetail.RecoverBreak(num ?? 0);
            }
        }
    }

    public class DiceCardAbility_RMR_CrueltyUpgradeDie : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[2] { "Bleeding_Keyword", "Recover_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            int? num = base.card.target?.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding);
            if (num > 0)
            {
                num = num * 2;
                if (num > 40)
                {
                    num = 40;
                }
                base.owner.RecoverHP(num ?? 0);
                base.owner.breakDetail.RecoverBreak(num ?? 0);
            }
        }
    }

    public class DiceCardAbility_RMR_SetFireUpgradeDie : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[1] { "Burn_Keyword"};
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 5, owner);
            owner.TakeDamage(3, DamageType.Card_Ability);
        }
    }
}
