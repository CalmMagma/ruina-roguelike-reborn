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

        public override string KeywordId
        {
            get
            {
                if (LogLikeMod.itemCatalogActive)
                {
                    return "RMR_StrangeOrb";
                }
                switch (LogLikeMod.curchaptergrade)
                {
                    case ChapterGrade.Grade1:
                        return "RMR_StrangeOrb";
                    case ChapterGrade.Grade2:
                        return "RMR_StrangeOrbGrade2";
                    case ChapterGrade.Grade3:
                        return "RMR_StrangeOrbGrade3";
                    case ChapterGrade.Grade4:
                        return "RMR_StrangeOrbGrade4";
                    case ChapterGrade.Grade5:
                        return "RMR_StrangeOrbGrade5";
                    case ChapterGrade.Grade6:
                        return "RMR_StrangeOrbGrade6";
                    default:
                       return "RMR_StrangeOrb";
                }
            }
        }

        public override string KeywordIconId => "RMR_StrangeOrb";
        public class PassiveAbility_RMR_StrangeOrbPassive : PassiveAbilityBase
        {
            public override bool isTargetable => false;
            public class orbhpbuf : BattleUnitBuf
            {
                public override StatBonus GetStatBonus()
                {
                    return new StatBonus
                    {
                        hpAdder = 10
                    };
                }
            }
            public override void Init(BattleUnitModel self)
            {
                base.Init(self);
                if (LogLikeMod.curchaptergrade >= ChapterGrade.Grade3)
                {
                    self.bufListDetail.AddBuf(new orbhpbuf());
                }
                if (LogLikeMod.curchaptergrade >= ChapterGrade.Grade6)
                {
                    self.bufListDetail.AddBuf(new orbhpbuf());
                }
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                var alivelist = BattleObjectManager.instance.GetAliveList(owner.faction).FindAll((BattleUnitModel x) => !x.IsDead() && x != owner);
                if (alivelist.Count > 0)
                {
                    var list = BattleObjectManager.instance.GetAliveList(owner.faction).Shuffle();
                    switch (LogLikeMod.curchaptergrade)
                    {
                        case ChapterGrade.Grade1:
                            Effects(1, list);
                            break;
                        case ChapterGrade.Grade2:
                            Effects(1, list);
                            Effects(2, list);
                            break;
                        case ChapterGrade.Grade3:
                            Effects(1, list);
                            Effects(2, list);
                            Effects(3, list);
                            break;
                        case ChapterGrade.Grade4:
                            Effects(1, list);
                            Effects(2, list);
                            Effects(3, list);
                            Effects(4, list);
                            break;
                        case ChapterGrade.Grade5:
                            Effects(1, list);
                            Effects(2, list);
                            Effects(3, list);
                            Effects(4, list);
                            Effects(5, list);
                            break;
                        case ChapterGrade.Grade6:
                            Effects(1, list);
                            Effects(2, list);
                            Effects(3, list);
                            Effects(4, list);
                            Effects(5, list);
                            Effects(6, list);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    owner.Die();
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

            private void Effects(int effect, List<BattleUnitModel> list)
            {
                switch (effect)
                {
                    case 1:
                        owner.allyCardDetail.AddNewCard(new LorId(RMRCore.packageId, -104), true);
                        break;

                    case 2:
                        foreach (BattleUnitModel guy in list)
                        {
                            guy.bufListDetail.AddKeywordBufThisRoundByEtc(RoguelikeBufs.RMRShield, 3);
                        }
                        if (Singleton<StageController>.Instance.RoundTurn % 2 == 0)
                        {
                            foreach (BattleUnitModel guy in list)
                            {
                                guy.breakDetail.RecoverBreak(3);
                            }
                        }
                        break;

                    case 3:
                        if (Singleton<StageController>.Instance.RoundTurn == 3)
                        {
                            foreach (BattleUnitModel guy in list)
                            {
                                guy.cardSlotDetail.RecoverPlayPoint(1);
                            }
                        }
                        break;

                    case 4:
                        int random = RandomUtil.Range(1, 3);
                        switch (random)
                        {
                            case 1:
                                foreach (BattleUnitModel guy in list)
                                {
                                    guy.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.DmgUp, 1);
                                }
                                break;

                            case 2:
                                foreach (BattleUnitModel guy in list)
                                {
                                    guy.bufListDetail.AddKeywordBufThisRoundByEtc(RoguelikeBufs.BleedProtection, 1);
                                }
                                break;

                            case 3:
                                foreach (BattleUnitModel guy in list)
                                {
                                    guy.bufListDetail.AddKeywordBufThisRoundByEtc(RoguelikeBufs.BurnProtection, 3);
                                }
                                break;
                        }
                        break;

                    case 5:
                        if (Singleton<StageController>.Instance.RoundTurn == 4)
                        {
                            foreach (BattleUnitModel guy in list)
                            {
                                guy.allyCardDetail.DrawCards(1);
                            }
                        }
                        if (Singleton<StageController>.Instance.RoundTurn >= 4)
                        {
                            RandomUtil.SelectOne<BattleUnitModel>(list.FindAll((BattleUnitModel x) => x != owner)).allyCardDetail.DrawCards(1);
                        }
                        break;

                    case 6:
                        var handlist = new List<BattleDiceCardModel>();
                        foreach (BattleDiceCardModel card in RandomUtil.SelectOne<BattleUnitModel>(list.FindAll((BattleUnitModel x) => x != owner)).allyCardDetail.GetHand())
                        {
                            if (card.GetCost() > 0)
                            {
                                handlist.Add(card);
                            }
                        }
                        if (handlist.Count > 0)
                        {
                            RandomUtil.SelectOne<BattleDiceCardModel>(handlist).AddBuf(new cardcostreductionorb());
                        }
                        break;

                    default:
                        break;
                }
            }

            public class cardcostreductionorb : BattleDiceCardBuf
            {
                public override int GetCost(int oldCost)
                {
                    return oldCost - 1;
                }

                public override void OnUseCard(BattleUnitModel owner)
                {
                    base.OnUseCard(owner);
                    this.Destroy();
                }
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

