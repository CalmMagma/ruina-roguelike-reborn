using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;
using LOR_BattleUnit_UI;
using LOR_DiceSystem;
using LOR_XML;

namespace RogueLike_Mod_Reborn
{
    public class DiceSelfCardAbility_RMR_Starter_Evade : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.allyCardDetail.DrawCards(1);
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Endurance, 1, owner);
        }

        // public static string Desc = "[On Use] Draw 1 page and gain 1 Endurance this Scene";
        public override string[] Keywords => new string[]{
        "DrawCard_Keyword", "Endurance_Keyword"
        };
    }
    public class DiceSelfCardAbility_RMR_Starter_CoordinatedStrikes : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            int pow = 0;
            foreach (var unit in BattleObjectManager.instance.GetAliveList())
            {
                foreach (var c in unit.cardSlotDetail.cardAry)
                {
                    if (c != null && c.target == card.target && c.card.GetID() == card.card.GetID())
                        pow++;
                }
            }
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus { power = pow });
        }

        // public static string Desc = "[On Use] Dice on this page gain +1 Power for each copy of this page being used against the same target";
    }

    public class DiceCardSelfAbility_RMR_Gain2AhnOnKill : DiceCardSelfAbilityBase
    {
        public override void OnEndBattle()
        {
            if (card.target.IsDead())
            {
                LogueBookModels.AddMoney(2);
            }
        }

        // public static string Desc = "[On Kill] Gain 2 Ahn";
    }

    public class DiceCardSelfAbility_RMR_ShivThrow : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            self.exhaust = true;
            BattleDiceCardModel cardItem = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(new LorId(RMRCore.packageId, -100)));
            BattleDiceBehavior battleDiceBehavior = cardItem.CreateDiceCardBehaviorList()[0];
            var list = unit.allyCardDetail.GetHand();
            list.SortByCost();
            if (list[0] != null)
            {
                if (list[0]._originalXmlData != null)
                    list[0].CopySelf();
                List<DiceBehaviour> dicelist = list[0].XmlData.DiceBehaviourList;
                dicelist.Add(battleDiceBehavior.behaviourInCard);
                list[0].XmlData.DiceBehaviourList = dicelist;
            }
        }
    }
    public class DiceCardSelfAbility_RMR_Remote : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            self.exhaust = true;
            var list = targetUnit.cardSlotDetail.cardAry;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null && list[i].card.CurCost > 0)
                {
                    targetUnit.cardSlotDetail.cardAry[i] = null;
                }
            }
            targetUnit.cardSlotDetail.SetPlayPoint(0);
            SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.UpdateTargetList();
            GlobalLogueEffectBase effect = GlobalLogueEffectManager.Instance.GetEffectList().Find(x => x is RMREffect_Remote);
            if (effect != null)
            {
                GlobalLogueEffectManager.Instance.RemoveEffect(effect);
            }
            
        }
    }
}
