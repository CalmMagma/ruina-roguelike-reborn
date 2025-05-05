using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;
using LOR_DiceSystem;

namespace RogueLike_Mod_Reborn
{
    #region STARTER ITEMS
    public class RMREffect_IronHeart : GlobalRebornEffectBase
    {
        public override void OnStartBattleAfter()
        {
            BattleUnitModel model = BattleObjectManager.instance.GetPatron();
            if (model == null)
                return;
            model.RecoverHP(6);
        }

        public static Rarity ItemRarity = Rarity.Common;

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

        public static Rarity ItemRarity = Rarity.Common;

        public override string KeywordId => "RMR_HunterCloak";

        public override string KeywordIconId => "RMR_HunterCloak";
    }

    public class RMREffect_ViciousGlasses : GlobalRebornEffectBase
    {
        public override void OnStartBattleAfter()
        {
            base.OnStartBattleAfter();
            var list = BattleObjectManager.instance.GetAliveList(Faction.Player);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].bufListDetail.AddKeywordBufByEtc(RoguelikeBufs.CritChance, 10);
            }
        }

        public static Rarity ItemRarity = Rarity.Common;

        public override string KeywordId => "RMR_ViciousGlasses";

        public override string KeywordIconId => "RMR_ViciousGlasses";
    }

    public class RMREffect_StrangeOrb : GlobalRebornEffectBase
    {
        public override void OnStartBattleAfter()
        {
            StageController.Instance.AddModdedUnit(Faction.Player, new LorId(RMRCore.packageId, -100), -1, 170, new XmlVector2 { x = 20, y = 0 });
            UnitUtil.RefreshCombatUI();
        }

        public static Rarity ItemRarity = Rarity.Common;

        public override string KeywordId => "RMR_StrangeOrb";

        public override string KeywordIconId => "RMR_StrangeOrb";
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


    }
    #endregion


    public class RMREffect_HarvesterScythe : GlobalRebornEffectBase
    {
        public static Rarity ItemRarity = Rarity.Uncommon;
        public override string KeywordId => "RMR_HarvestScythe";
        public override string KeywordIconId => "RMR_HarvestScythe";
        public override void OnStartBattleAfter()
        {
            base.OnStartBattleAfter();
            var list = BattleObjectManager.instance.GetAliveList(Faction.Player);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].bufListDetail.AddKeywordBufByEtc(RoguelikeBufs.CritChance, 5);
            }
        }

        public override void OnCrit(BattleUnitModel critter, BattleUnitModel target)
        {
            base.OnCrit(critter, target);
            critter?.RecoverHP(5);
        }
    }
    
    public class RMREffect_Remote : GlobalRebornEffectBase
    {
        public static Rarity ItemRarity = Rarity.Uncommon;
        public override string KeywordId => "RMR_Remote";
        public override string KeywordIconId => "RMR_Remote";
        public override void OnStartBattleAfter()
        {
            base.OnStartBattleAfter();
            BattleUnitModel model = BattleObjectManager.instance.GetPatron();
            if (model != null)
            {
                model.allyCardDetail.AddNewCard(new LorId(RMRCore.packageId, -103));
            }
        }
    }

    public class RMREffect_BigBrotherChains : GlobalRebornEffectBase
    {
        int powerUp = 0;
        public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
        {
            if (card.owner.faction == Faction.Enemy) return;
            base.OnUseCard(card);
            powerUp = 0;
            var list = card.owner.allyCardDetail.GetHand();
            
            foreach (BattleDiceCardModel c in list)
            {
                if (c != null && c.GetID() == card.card.GetID())
                {
                    powerUp++; // if card is the same, add power
                }
            }
            powerUp--; // remove 1 power 'cause one of the cards is the one we played
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                power = powerUp
            });
            // do this here because for some reason we can't apply it on rolling the dice, which is probably something to do with order of operation
        }
        /*public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior.card.owner.faction == Faction.Enemy) return;
            base.BeforeRollDice(behavior);
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                power = powerUp
            });
        }*/
        // omitted until we know for sure the other solution works

        public static Rarity ItemRarity = Rarity.Rare;
        public override string KeywordIconId => "RMR_BigBrothersChains";
        public override string KeywordId => "RMR_BigBrothersChains";
    }

    // NEEDS LOCALIZATION, OBTAINMENT METHOD AND TESTING
    public class RMREffect_ZeroCounterplay : GlobalRebornEffectBase
    {
        bool ZCActive = false;
        int diceTicks = 3;

        public static Rarity ItemRarity = Rarity.Rare;

        public override void OnRoundStart(StageController stage)
        {
            base.OnRoundStart(stage);
            ZCActive = true;
            diceTicks = 3;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if(ZCActive && behavior.TargetDice != null && behavior.TargetDice.card.GetDiceBehaviorList().Count() > 0)
            {
                var list = behavior.TargetDice.card.owner.cardSlotDetail.keepCard;
                if(list != null && behavior.TargetDice.card == list)
                {
                    foreach(var d in list.cardBehaviorQueue)
                    {
                        if(diceTicks > 0)
                        {
                            d.DestroyDice(DiceUITiming.AttackAfter);
                            diceTicks--;
                        }
                    }
                    ZCActive = false;
                }
            }
            // this has yet to be tested
        }
    }

    public class RMREffect_Crowbar : GlobalRebornEffectBase
    {
        public class CrowbarDamageBuf : BattleUnitBuf
        {
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                var enemy = behavior.card.target;
                    // this is how you select the enemy 
                    if (enemy == null) return;
                    if (enemy.hp/(float)enemy.MaxHp >= 0.9) // float thingy means 0.9 = 90%
                    {
                        behavior.ApplyDiceStatBonus(new DiceStatBonus { dmgRate = 50, breakRate = 50 });
                        //applying extra damage to the funny die
                    }
            }           
        }

        //hidden buf because pm are terrible people
        public override void OnStartBattleAfter()
        {
            base.OnStartBattleAfter();
            foreach (var unit in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                unit.bufListDetail.AddBuf(new CrowbarDamageBuf());
            }
        }

        public static Rarity ItemRarity = Rarity.Common;

        public override string KeywordId => "RMR_Crowbar";

        public override string KeywordIconId => "RMR_Crowbar";
    }

    /// <summary>
    /// Hidden effect that is added on gamemode initialization<br></br>
    /// Basekit 20% chance to find upgraded cards
    /// </summary>
    [HideFromItemCatalog]
    public class RMREffect_HiddenUpgradeChanceEffect : GlobalLogueEffectBase
    {
        public override void ChangeShopCard(ref DiceCardXmlInfo card)
        {
            base.ChangeShopCard(ref card);
            if (card.CheckCanUpgrade())
            {
                if (UnityEngine.Random.value < 0.20f)
                {
                    card = Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(card.id);
                }
            }
        }

        public override void ChangeCardReward(ref List<DiceCardXmlInfo> cardlist)
        {
            List<DiceCardXmlInfo> list = new List<DiceCardXmlInfo>();
            foreach (DiceCardXmlInfo diceCardXmlInfo in cardlist)
            {
                if (!diceCardXmlInfo.CheckCanUpgrade())
                {
                    list.Add(diceCardXmlInfo);
                }
                else
                {
                    if (UnityEngine.Random.value < 0.20f)
                    {
                        list.Add(Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(diceCardXmlInfo.id));
                    }
                    else
                    {
                        list.Add(diceCardXmlInfo);
                    }
                }
            }
            cardlist = list;
        }
    }
}

