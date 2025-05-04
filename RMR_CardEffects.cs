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
