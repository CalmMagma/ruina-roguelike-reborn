using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;
using LOR_DiceSystem;
using UnityEngine;

namespace RogueLike_Mod_Reborn
{
    #region STARTER ITEMS
    public class RMREffect_IronHeart : GlobalLogueEffectBase
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

    public class RMREffect_HunterCloak : GlobalLogueEffectBase
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

    public class RMREffect_ViciousGlasses : GlobalLogueEffectBase
    {
        public override void OnRoundStart(StageController stage)
        {
            base.OnRoundStart(stage);   
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

    public class RMREffect_LightsGuidance : GlobalLogueEffectBase
    {
        public override void OnRoundStart(StageController stage)
        {
            base.OnRoundStart(stage);
            var list = BattleObjectManager.instance.GetAliveList(Faction.Player);
            List<KeywordBuf> list2 = new List<KeywordBuf> { KeywordBuf.SlashPowerUp, KeywordBuf.PenetratePowerUp, KeywordBuf.HitPowerUp, KeywordBuf.DefensePowerUp };
            for (int i = 0; i < list.Count; i++)
            {
                list[i].bufListDetail.AddKeywordBufByEtc(RandomUtil.SelectOne<KeywordBuf>(list2), 1);
            }
        }

        public static Rarity ItemRarity = Rarity.Common;

        public override string KeywordId => "RMR_LightGuidance";

        public override string KeywordIconId => "RMR_LightGuidance";
    }

    public class RMREffect_StrangeOrb : GlobalLogueEffectBase
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

            public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
            {
                return (AtkResist)Math.Min((int)LogLikeMod.curchaptergrade + 1, 4);
            }
        }


    }
    #endregion

    public class RMREffect_CorrodedChains : GlobalLogueEffectBase
    {
        public static Rarity ItemRarity = Rarity.Uncommon;
        public override string KeywordId => "RMR_CorrodedChains";
        public override string KeywordIconId => "RMR_CorrodedChains";

        /*
        public override void OnSuccessAttack(BattleDiceBehavior behavior)
        {
            base.OnSuccessAttack(behavior);
            int bleedStacks = this._owner.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding);
            if (bleedStacks > 0)
            {
                bleedStacks /= 2;
                BattleUnitModel target = behavior.card.target;
                if (target != null)
                {
                    target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, bleedStacks, _owner);
                }
            }
        }
        */
    }

    public class RMREffect_BleedingSpleen : GlobalLogueEffectBase
    {
        public static Rarity ItemRarity = Rarity.Rare;
        public override string KeywordId => "RMR_BleedingSpleen";
        public override string KeywordIconId => "RMR_BleedingSpleen";
        public override void OnRoundStart(StageController stage)
        {
            base.OnRoundStart(stage);
            var list = BattleObjectManager.instance.GetAliveList(Faction.Player);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].bufListDetail.AddKeywordBufByEtc(RoguelikeBufs.CritChance, 5);
            }
        }
        public override void OnCrit(BattleUnitModel critter, BattleUnitModel target)
        {
            base.OnCrit(critter, target);
            target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 2, critter);
        }
        public override void OnKillUnit(BattleUnitModel killer, BattleUnitModel target)
        {
            base.OnKillUnit(killer, target);
            int BleedToTransfer = target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding);
            if (BleedToTransfer < 10) return;
            BleedToTransfer /= 2;
            List<BattleUnitModel> EnemyList = target.Team.GetList();
            for (int i = 0; i < EnemyList.Count; i++)
            {
                if (EnemyList[i] != target)
                {
                    EnemyList[i].TakeDamage(BleedToTransfer, DamageType.ETC, killer);
                }
            }
        }
    }
    public class RMREffect_HarvesterScythe : GlobalLogueEffectBase
    {
        public static Rarity ItemRarity = Rarity.Uncommon;
        public override string KeywordId => "RMR_HarvestScythe";
        public override string KeywordIconId => "RMR_HarvestScythe";
        public override void OnRoundStart(StageController stage)
        {
            base.OnRoundStart(stage);
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

    public class RMREffect_Remote : GlobalLogueEffectBase
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

    public class RMREffect_BigBrotherChains : GlobalLogueEffectBase
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
    public class RMREffect_ZeroCounterplay : GlobalLogueEffectBase
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
            if (ZCActive && behavior.TargetDice != null && behavior.TargetDice.card.GetDiceBehaviorList().Count() > 0)
            {
                var list = behavior.TargetDice.card.owner.cardSlotDetail.keepCard;
                if (list != null && behavior.TargetDice.card == list)
                {
                    foreach (var d in list.cardBehaviorQueue)
                    {
                        if (diceTicks > 0)
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

    public class RMREffect_Crowbar : GlobalLogueEffectBase
    {
        public class CrowbarDamageBuf : BattleUnitBuf
        {
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                var enemy = behavior.card.target;
                // this is how you select the enemy 
                if (enemy == null) return;
                if (enemy.hp / (float)enemy.MaxHp >= 0.9) // float thingy means 0.9 = 90%
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

        public static Rarity ItemRarity = Rarity.Uncommon;

        public override string KeywordId => "RMR_Crowbar";

        public override string KeywordIconId => "RMR_Crowbar";
    }


    public class RMREffect_Prescript : OnceEffect
    {
        public bool disable = false;

        public override void Use()
        {

        }

        public override void OnClick()
        {
            
        }

        public override void OnStartBattleAfter()
        {
            base.OnStartBattleAfter();
            if (!disable)
            {
                for (int i = 0; i < this.stack; i++)
                {
                    List<BattleUnitModel> list = BattleObjectManager.instance.GetAliveList(Faction.Player).FindAll((BattleUnitModel x) => x.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf y) => y is BattleUnitBuf_RMRPrescriptbuf) == null);
                    if (list.Count > 0)
                    {
                        RandomUtil.SelectOne<BattleUnitModel>(list).bufListDetail.AddBuf(new BattleUnitBuf_RMRPrescriptbuf());
                    }
                }
            }
        }

        public class BattleUnitBuf_RMRPrescriptbuf : BattleUnitBuf
        {
            public override string keywordId => "RMR_Prescriptbuf";
            public override string keywordIconId => "RMR_Prescriptbuf";

            BattleUnitModel target;
            int count;

            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                base.OnUseCard(card);
                if (target != null)
                {
                    if (target != card.target)
                    {
                        target = card.target;
                        count = 0;
                        card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                        {
                            dmgRate = -50,
                            breakRate = -50
                        });
                    }
                    else if (target == card.target)
                    {
                        count++;
                        DmgIncrease(card);
                    }
                }
                else
                {
                    count++;
                    target = card.target;
                    DmgIncrease(card);
                }
            }

            private void DmgIncrease(BattlePlayingCardDataInUnitModel card)
            {
                if (count > 3)
                {
                    count = 3;
                }
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                {
                    dmgRate = 10 * count,
                    breakRate = 10 * count
                });
            }
        }
        public static Rarity ItemRarity = Rarity.Special;

        public override Sprite GetSprite()
        {
            return disable ? null : base.GetSprite();
        }

        public override string KeywordId => "RMR_Prescript";

        public override string KeywordIconId => "RMR_Prescript";
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

