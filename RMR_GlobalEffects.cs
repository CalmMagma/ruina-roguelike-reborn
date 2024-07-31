using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;
using static UnityEngine.UI.GridLayoutGroup;

namespace RogueLike_Mod_Reborn
{

    public class RMREffect_IronHeart : GlobalRebornEffectBase
    {
        public override void OnStartBattleAfter()
        {
            BattleUnitModel model = BattleObjectManager.instance.GetPatron();
            if (model == null)
                return;
            model.RecoverHP(6);
        }

        public override string KeywordId => "RMR_IronHeart";

        public override string KeywordIconId => "RMR_IronHeart";
    }


    public class RMREffect_HunterCloak : GlobalRebornEffectBase
    {
        public override void OnStartBattleAfter()
        {
            BattleUnitModel model = BattleObjectManager.instance.GetPatron();
            if (model == null)
                return;
            model.allyCardDetail.DrawCards(2);
        }

        public override void OnRoundStart(StageController stage)
        {
            BattleUnitModel model = BattleObjectManager.instance.GetPatron();
            if (model == null)
                return;
            model.allyCardDetail.AddNewCard(new LorId(RMRCore.packageId, -100), true);
        }

        public override string KeywordId => "RMR_HunterCloak";

        public override string KeywordIconId => "RMR_HunterCloak";
    }


    public class RMREffect_StrangeOrb : GlobalRebornEffectBase
    {
        public override void OnStartBattleAfter()
        {
            StageController.Instance.AddModdedUnit(Faction.Player, new LorId(RMRCore.packageId, -100), -1, 170, new XmlVector2 { x = 20, y = 0 });
            UnitUtil.RefreshCombatUI();
        }

        public override string KeywordId => "RMR_StrangeOrb";

        public override string KeywordIconId => "RMR_StrangeOrb";
    }

    public class PassiveAbility_RMR_StrangeOrbPassive : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            var list = BattleObjectManager.instance.GetAliveList(owner.faction).Shuffle();
            switch (UnityEngine.Random.Range(0, 4))
            {
                case 0:
                    foreach (var enemy in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                        enemy?.bufListDetail?.AddKeywordBufThisRoundByEtc(KeywordBuf.Paralysis, 1, owner);
                    break;
                case 1:
                    var unit = list[UnityEngine.Random.Range(0, list.Count)];
                    if (unit != null) unit.cardSlotDetail.RecoverPlayPoint(1);
                    else break;
                    list.Remove(unit);
                    if (list.Any())
                    {
                        unit = list[UnityEngine.Random.Range(0, list.Count)];
                        if (unit != null) unit.cardSlotDetail.RecoverPlayPoint(1);
                    }
                    break;
                case 2:
                    var unit2 = list[UnityEngine.Random.Range(0, list.Count)];
                    if (unit2 != null) unit2.allyCardDetail.DrawCards(1);
                    else break;
                    list.Remove(unit2);
                    if (list.Any())
                    {
                        unit2 = list[UnityEngine.Random.Range(0, list.Count)];
                        if (unit2 != null) unit2.allyCardDetail.DrawCards(1);
                    }
                    break;
                case 3:
                    foreach (var ally in BattleObjectManager.instance.GetAliveList(owner.faction))
                        ally?.bufListDetail?.AddKeywordBufThisRoundByEtc(KeywordBuf.DmgUp, 1, owner);
                    break;
                default:
                    break;
            }
        }

        public override bool IsImmuneBreakDmg(DamageType type)
        {
            return true;
        }
    }

    public class RMREffect_StillWater : GlobalRebornEffectBase
    {
        public override void OnRoundStart(StageController stage)
        {
            
        }

        public override void OnStartBattleAfter()
        {
            base.OnStartBattleAfter();
            var list = BattleObjectManager.instance.GetAliveList(Faction.Player);
            for(int i = 0; i < list.Count; i++)
            {
                list[i].bufListDetail.AddKeywordBufByEtc(RoguelikeBufs.CritChance, 4);
            }
        }

        public override string KeywordId => "RMR_StillWater";

        public override string KeywordIconId => "RMR_StillWater";
    }
}
