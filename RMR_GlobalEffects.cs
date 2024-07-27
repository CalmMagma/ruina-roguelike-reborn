using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;

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

        protected override string KeywordId => "RMR_IronHeart";

        protected override string KeywordIconId => "RMR_IronHeart";
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

        protected override string KeywordId => "RMR_HunterCloak";

        protected override string KeywordIconId => "RMR_HunterCloak";
    }


    public class RMREffect_StrangeOrb : GlobalRebornEffectBase
    {
        public override void OnStartBattleAfter()
        {
            StageController.Instance.AddModdedUnit(Faction.Player, new LorId(RMRCore.packageId, -100), -1, 170, new XmlVector2 { x = 20, y = 0 });
            UnitUtil.RefreshCombatUI();
        }

        protected override string KeywordId => "RMR_StrangeOrb";

        protected override string KeywordIconId => "RMR_StrangeOrb";
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
                    list[0].cardSlotDetail.RecoverPlayPoint(1);
                    list = list.Shuffle();
                    list[0].cardSlotDetail.RecoverPlayPoint(1);
                    break;
                case 2:
                    list[0].allyCardDetail.DrawCards(1);
                    list = list.Shuffle();
                    list[0].allyCardDetail.DrawCards(1);
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

        protected override string KeywordId => "RMR_StillWater";

        protected override string KeywordIconId => "RMR_StillWater";
    }
}
