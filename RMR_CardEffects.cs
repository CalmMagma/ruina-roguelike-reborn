using System;
using System.Collections.Generic;
using System.Linq;
using abcdcode_LOGLIKE_MOD;
using LOR_BattleUnit_UI;
using LOR_DiceSystem;
using LOR_XML;
using UnityEngine;

namespace RogueLike_Mod_Reborn
{
    public class DiceCardSelfAbility_RMR_Starter_Evade : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.allyCardDetail.DrawCards(1);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 1, owner);
        }

        // public static string Desc = "[On Use] Draw 1 page and gain 1 Endurance this Scene";
        public override string[] Keywords => new string[]{
        "DrawCard_Keyword", "Endurance_Keyword"
        };
    }
    public class DiceCardSelfAbility_RMR_Starter_CoordinatedStrikes : DiceCardSelfAbilityBase
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
        public class ScrollAbility_RMR_Shiv : ScrollAbilityBase
        {
            public override void OnScrollDown(BattleUnitModel unit, BattleDiceCardModel self)
            {
                base.OnScrollDown(unit, self);
                if (self.GetCost() > 0)
                {
                    var card = self.XmlData;
                    if (self._originalXmlData == null)
                        self.CopySelf();
                    self.AddCost(-1);
                    List<DiceBehaviour> dicelist = card.DiceBehaviourList;
                    dicelist[0].Min = 1 + (2 * self.GetCost());
                    dicelist[0].Dice = 3 + (2 * self.GetCost());
                    card.DiceBehaviourList = dicelist;
                }
            }


            public override void OnScrollUp(BattleUnitModel unit, BattleDiceCardModel self)
            {
                base.OnScrollUp(unit, self);
                if (self.GetCost() < 5 && self.GetCost() + 1 <= unit.PlayPoint - unit.cardSlotDetail.ReservedPlayPoint)
                {
                    var card = self.XmlData;
                    if (self._originalXmlData == null)
                        self.CopySelf();
                    self.AddCost(1);
                    List<DiceBehaviour> dicelist = card.DiceBehaviourList;
                    dicelist[0].Min = 1 + (2 * self.GetCost());
                    dicelist[0].Dice = 3 + (2 * self.GetCost());
                    card.DiceBehaviourList = dicelist;
                }
            }
        }

        public override void OnAddToHand(BattleUnitModel owner)
        {
            base.OnAddToHand(owner);
            owner.AddScrollAbility<ScrollAbility_RMR_Shiv>(card.card);
            card.card.SetCurrentCost(card.card.GetOriginCost());
            card.card.ResetToOriginalData();
        }
        
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            self.exhaust = true;
            BattleDiceBehavior battleDiceBehavior = self.CreateDiceCardBehaviorList()[0];
            var list = unit.allyCardDetail.GetHand();
            list.RemoveAll(x => x.GetID() == self.GetID());
            list.SortByCost();
            if (list[0] != null)
            {
                if (list[0]._originalXmlData == null)
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
            GlobalLogueEffectBase effect = GlobalLogueEffectManager.Instance.GetEffect<RMREffect_Remote>();
            if (effect != null)
            {
                effect.Destroy();
            }
            
        }
    }

    #region Canard
    public class DiceCardSelfAbility_RMR_Track : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Recover_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.RecoverHP(4);
        }
    }

    public class DiceCardSelfAbility_RMR_TrackUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.RecoverHP(4);
        }

        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddBuf(new BattleUnitBuf_rmrtrackbuf());
        }
        public override string[] Keywords => new string[] { "Recover_Keyword" };
        public class BattleUnitBuf_rmrtrackbuf : BattleUnitBuf
        {
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (!behavior.abilityList.Contains(new DiceCardAbility_recoverHp1atk()))
                {
                    behavior.AddAbility(new DiceCardAbility_recoverHp1atk());
                }
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }

    public class DiceCardSelfAbility_RMR_Chargeup : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddBuf(new BattleUnitBuf_rmrchargeupbuf());
        }
        public override string[] Keywords => new string[] { "Paralysis_Keyword", "Energy_Keyword" };
        public class BattleUnitBuf_rmrchargeupbuf : BattleUnitBuf
        {
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                _owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Paralysis, 1, _owner);
                _owner.cardSlotDetail.RecoverPlayPointByCard(1);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_ChargeupUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.allyCardDetail.ExhaustACard(this.card.card);
            card.card.exhaust = true;
            owner.bufListDetail.AddBuf(new BattleUnitBuf_rmrchargeupbuf());
            owner.allyCardDetail.DrawCards(1);
        }
        public override string[] Keywords => new string[] { "DrawCard_Keyword","Paralysis_Keyword","Energy_Keyword" };
        public class BattleUnitBuf_rmrchargeupbuf : BattleUnitBuf
        {

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                _owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Paralysis, 1, _owner);
                _owner.cardSlotDetail.RecoverPlayPointByCard(1);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_BackstreetsDash : DiceCardSelfAbilityBase
    {
        public override void OnStartParrying()
        {
            base.OnStartParrying();
            card.target?.currentDiceAction?.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                breakDmg = -5
            });
        }
    }

    public class DiceCardSelfAbility_RMR_SkitterAway : DiceCardSelfAbilityBase
    {
        public override void OnApplyCard()
        {
            base.OnApplyCard();
            RMRUtilityExtensions.AddSpeedImmediately(owner, owner.cardOrder, +3);
        }

        public override void OnReleaseCard()
        {
            base.OnReleaseCard();
            RMRUtilityExtensions.AddSpeedImmediately(owner, owner.cardOrder, -3);
        }
    }

    public class DiceCardSelfAbility_RMR_SkitterAwayUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnApplyCard()
        {
            base.OnApplyCard();
            RMRUtilityExtensions.AddSpeedImmediately(owner, owner.cardOrder, +5);
        }

        public override void OnReleaseCard()
        {
            base.OnReleaseCard();
            RMRUtilityExtensions.AddSpeedImmediately(owner, owner.cardOrder, -5);
        }
    }

    public class DiceCardSelfAbility_RMR_EndureUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Protection_Keyword", "BreakProtection_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 1, owner);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 1, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_DriedupUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Strength_Keyword", "Quickness_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 2, owner);
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 2, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_ChopItOff : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Paralysis_Keyword" };
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            int stack = behavior.card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Paralysis);
            if (stack > 3)
            {
                stack = 3;
            }
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                power = stack
            });
        }
    }

    public class DiceCardSelfAbility_RMR_GoinFirst : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            if (owner.cardSlotDetail.cardQueue.Peek() == card)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                {
                    min = 2
                });
            }
        }
    }

    public class DiceCardSelfAbility_RMR_StruggleUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Protection_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 2, owner);
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Protection, 2, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_YouOnlyLiveOnce : DiceCardSelfAbilityBase
    {
        public override void OnStartParrying()
        {
            base.OnStartParrying();
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                dmgRate = 50,
                breakRate = 50
            });
            card.target?.currentDiceAction?.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                dmgRate = 50,
                breakRate = 50
            });
        }
    }

    public class DiceCardSelfAbility_RMR_GutHarvesting : DiceCardSelfAbilityBase
    {

        public override void OnEndBattle()
        {
            base.OnEndBattle();
            if (card.target.IsDead())
            {
                LogueBookModels.AddMoney(4);
            }
            this.card.card.XmlData.Script = "";
        }
    }

    public class DiceCardSelfAbility_RMR_GutHarvestingUpgrade : DiceCardSelfAbilityBase
    {

        public override void OnEndBattle()
        {
            base.OnEndBattle();
            if (card.target.IsDead())
            {
                switch (card.target.Book.Rarity)
                {
                    case Rarity.Common:
                        LogueBookModels.AddMoney(4);
                        break;
                    case Rarity.Uncommon:
                        LogueBookModels.AddMoney(7);
                        break;
                    case Rarity.Rare:
                        LogueBookModels.AddMoney(10);
                        break;
                    case Rarity.Unique:
                        LogueBookModels.AddMoney(15);
                        break;
                    case Rarity.Special:
                        LogueBookModels.AddMoney(15);
                        break;
                    default:
                        LogueBookModels.AddMoney(4);
                        break;
                }
                
            }
            this.card.card.XmlData.Script = "";
        }
    }

    public class DiceCardSelfAbility_RMR_break3atkcard : DiceCardSelfAbilityBase
    {
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            card.target.breakDetail.TakeBreakDamage(3, DamageType.Card_Ability);
        }
    }

    #endregion

    #region Urban Myth

    public class DiceCardSelfAbility_RMR_GatherIntelUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "DrawCard_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            List<BattleDiceCardModel> drawpile = owner.allyCardDetail.GetDeck();
            if (drawpile.Count > 0)
            {
                 owner.allyCardDetail.DrawCardsAllSpecific(drawpile.SortReturn((x, y) => x.GetRarity() - y.GetRarity()).Last().GetID());
            }
        }
    }

    public class DiceCardSelfAbility_RMR_Appetite : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword", "Recover_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            int stack = card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding);
            if (stack > 15)
            {
                stack = 15;
            }
            owner.RecoverHP(stack);
        }
    }

    public class DiceCardSelfAbility_RMR_QuickAttack : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Quickness_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 2, owner);
            if (owner.speedDiceResult[card.slotOrder].value > card.target.speedDiceResult[card.targetSlotOrder].value)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                {
                    power = 2
                });
            }
        }
    }
    

    public class DiceCardSelfAbility_RMR_bleed2atkcard : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 2, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_PreparedMindLulu : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Burn_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddBuf(new BattleUnitBuf_burnPlus());
        }

        public class BattleUnitBuf_burnPlus : BattleUnitBuf
        {
            public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
            {
                if (cardBuf.bufType == KeywordBuf.Burn)
                {
                    return 1;
                }
                return 0;
            }

            public override void OnRoundEnd()
            {
                Destroy();
            }
        }
    }

    public class DiceCardSelfAbility_RMR_PreparedMindLuluUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddBuf(new BattleUnitBuf_rmrpreparedmindbuf());
        }
        public override string[] Keywords => new string[] { "Burn_Keyword","Endurance_Keyword", };

        public class BattleUnitBuf_rmrpreparedmindbuf : BattleUnitBuf
        {
            public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
            {
                if (cardBuf.bufType == KeywordBuf.Burn)
                {
                    _owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 1, _owner);
                    return 1;
                }
                return 0;
            }


            public override void OnRoundEnd()
            {
                Destroy();
            }
        }
    }

    public class DiceCardSelfAbility_RMR_Endurance1start : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Endurance_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 1, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_Endurance2start : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Endurance_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 2, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_Multiblock : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Protection_Keyword", "BreakProtection_Keyword", "RMR_Shield_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 3, owner);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 3, owner);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRShield, 5, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_MultiblockUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Protection_Keyword", "BreakProtection_Keyword", "RMR_Shield_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 4, owner);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 4, owner);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRShield, 10, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_FleetFootsteps : DiceCardSelfAbilityBase
    {
        public override void OnApplyCard()
        {
            base.OnApplyCard();
            RMRUtilityExtensions.AddSpeedImmediately(owner, owner.cardOrder, +2);
        }

        public override void OnReleaseCard()
        {
            base.OnReleaseCard();
            RMRUtilityExtensions.AddSpeedImmediately(owner, owner.cardOrder, -2);
        }
    }

    public class DiceCardSelfAbility_RMR_Deflect : DiceCardSelfAbilityBase
    {
        public override void OnWinParryingAtk()
        {
            base.OnWinParryingAtk();
            card?.target?.currentDiceAction?.ApplyDiceStatBonus(DiceMatch.NextDice, new DiceStatBonus
            {
                power = -1
            });
        }
        public override void OnWinParryingDef()
        {
            base.OnWinParryingDef();
            card?.target?.currentDiceAction?.ApplyDiceStatBonus(DiceMatch.NextDice, new DiceStatBonus
            {
                power = -1
            });
        }
    }

    public class DiceCardSelfAbility_RMR_IngredientHunt : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            if (card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding) != null)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                {
                    power = 1
                });
            }
        }
    }

    public class DiceCardSelfAbility_RMR_Multihit : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            card.ForeachQueue(DiceMatch.AllDice, behavior => behavior.AddAbility(new DiceCardAbility_RMR_Multhitdie()));
        }
    }

    public class DiceCardAbility_RMR_Multhitdie : DiceCardAbilityBase
    {
        bool trigger;
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            if (!trigger)
            {
                if (RandomUtil.valueForProb <= 0.33)
                {
                    base.ActivateBonusAttackDice();
                    trigger = true;
                }
               

            }
        }
    }

    public class DiceCardSelfAbility_RMR_NonstopAssault : DiceCardSelfAbilityBase
    {
        public class BattleUnitBuf_rmrnonstopbuf : BattleUnitBuf
        {
            public BattleDiceCardModel card;

            public BattleUnitBuf_rmrnonstopbuf(BattleDiceCardModel card)
            {
                this.card = card;
            }

            public override void OnRoundEndTheLast()
            {
                _owner.allyCardDetail.DrawCardsAllSpecific(card.GetID());
                Destroy();
            }
        }

        public override string[] Keywords => new string[1] { "DrawCard_Keyword" };

        public override void OnUseCard()
        {
            base.owner.bufListDetail.AddBuf(new BattleUnitBuf_rmrnonstopbuf(card.card));
        }


    }

    public class DiceCardSelfAbility_RMR_NonstopAssaultUpgrade : DiceCardSelfAbilityBase
    {
        public class BattleUnitBuf_rmrnonstopbuf : BattleUnitBuf
        {
            public BattleDiceCardModel card;

            public BattleUnitBuf_rmrnonstopbuf(BattleDiceCardModel card)
            {
                this.card = card;
            }

            public override void OnRoundEndTheLast()
            {
                _owner.allyCardDetail.DrawCardsAllSpecific(card.GetID());
                Destroy();
            }
        }

        public override string[] Keywords => new string[1] { "DrawCard_Keyword" };

        public override void OnUseCard()
        {
            base.owner.cardSlotDetail.RecoverPlayPointByCard(1);
            base.owner.bufListDetail.AddBuf(new BattleUnitBuf_rmrnonstopbuf(card.card));
        }


    }

    #endregion

    #region Urban Legend
    public class DiceCardAbilityBase_RMR_LawOrder : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            /* DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(LogLikeMod.ModId, 390001));
            List <BattleDiceBehavior> list = new List<BattleDiceBehavior>();
            int num = 0;
            foreach (DiceBehaviour diceBehaviour in cardItem.DiceBehaviourList)
            {
                BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
                battleDiceBehavior.behaviourInCard = diceBehaviour.Copy();
                battleDiceBehavior.SetIndex(num++);
                list.Add(battleDiceBehavior);
            }
            owner.cardSlotDetail.keepCard.AddBehaviours(cardItem, list); */
            BattleDiceBehavior die = new BattleDiceBehavior
            {
                behaviourInCard = new DiceBehaviour
                {
                    Min = 3,
                    Dice = 8,
                    Detail = BehaviourDetail.Guard,
                    Type = BehaviourType.Def,
                    MotionDetail = MotionDetail.G,
                    MotionDetailDefault = MotionDetail.N,
                    Script = ""
                }
            };
            owner.cardSlotDetail.keepCard.AddBehaviourForOnlyDefense(this.card.card, die);
        }
    }

    public class DiceCardAbilityBase_RMR_LawOrderUpgrade : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            /*  DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(LogLikeMod.ModId, 390002));
              List<BattleDiceBehavior> list = new List<BattleDiceBehavior>();
              int num = 0;
              foreach (DiceBehaviour diceBehaviour in cardItem.DiceBehaviourList)
              {
                  BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
                  battleDiceBehavior.behaviourInCard = diceBehaviour.Copy();
                  battleDiceBehavior.SetIndex(num++);
                  list.Add(battleDiceBehavior);
              }
              owner.cardSlotDetail.keepCard.AddBehaviours(cardItem, list); */
            BattleDiceBehavior die = new BattleDiceBehavior
            {
                behaviourInCard = new DiceBehaviour
                {
                    Min = 3,
                    Dice = 8,
                    Detail = BehaviourDetail.Guard,
                    Type = BehaviourType.Def,
                    MotionDetail = MotionDetail.G,
                    MotionDetailDefault = MotionDetail.N,
                    Script = ""
                }
            };
            owner.cardSlotDetail.keepCard.AddBehaviourForOnlyDefense(this.card.card, die);
            owner.cardSlotDetail.keepCard.AddBehaviourForOnlyDefense(this.card.card, die);
        }
    }

    public class DiceCardSelfAbility_RMR_Diversion : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            if (this.card.targetSlotOrder < this.card.target.cardSlotDetail.cardAry.Count)
            {
                BattlePlayingCardDataInUnitModel fish = this.card.target.cardSlotDetail.cardAry[this.card.targetSlotOrder];
                if (fish != null)
                {
                    if (fish.target != this.owner)
                    {
                        fish.target = this.owner;
                        fish.targetSlotOrder = this.card.slotOrder;
                    }

                }
            }
        }
    }

    public class DiceCardSelfAbility_RMR_SharpSwipe : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddBuf(new agagagagbleeeeed());
        }

        public class agagagagbleeeeed : BattleUnitBuf
        {
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                behavior.AddAbility(new DiceCardAbility_bleeding1atk());
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }

    public class DiceCardSelfAbility_RMR_Standoff : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_Shield_Keyword", "RMR_StaggerShield_Keyword" };
        public override void OnStartParrying()
        {
            base.OnStartParrying();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRShield, 5, owner);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRStaggerShield, 5, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_StandoffUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_Shield_Keyword", "RMR_StaggerShield_Keyword" };
        public override void OnStartParrying()
        {
            base.OnStartParrying();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRShield, 8, owner);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRStaggerShield, 8, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_Avert : DiceCardSelfAbilityBase
    {
        public override void OnStartParrying()
        {
            base.OnStartParrying();
            if (card?.target?.currentDiceAction?.earlyTarget?.faction == owner.faction && card?.target?.currentDiceAction?.earlyTarget != owner)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                {
                    power = 2
                });
            }
        }
    }

    public class DiceCardSelfAbility_RMR_StartinLightly : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.HitPowerUp, 1, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_GuardianUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Endurance_Keyword", "Quickness_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            foreach (BattleUnitModel amog in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                amog.bufListDetail.AddKeywordBufByCard(KeywordBuf.Endurance, 1, owner);
                amog.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 1, owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_GambleUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_CriticalStrike_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            var x = owner.allyCardDetail.DiscardACardLowest();
            if (x.GetCost() != 0)
            {
                owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.CritChance, x.GetCost()*2, owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_BackAttack : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            foreach (var unit in BattleObjectManager.instance.GetAliveList())
            {
                foreach (var c in unit.cardSlotDetail.cardAry)
                {
                    if (c != null && c.target == card.target && c.card.GetCost() >= 3 && unit.faction == owner.faction)
                    {
                        card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus { power = 1 });
                    }
                }
            }
        }
    }

    public class DiceCardSelfAbility_RMR_SliceUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.allyCardDetail.DiscardACardLowest();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.SlashPowerUp, 1, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_StayClamUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "DrawCard_Keyword", "RMR_StaggerShield_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.allyCardDetail.DrawCards(1);
        }

        public override void OnDiscard(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnDiscard(unit, self);
            unit.allyCardDetail.DrawCards(1);
            unit.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRStaggerShield, 3, unit);
        }
    }

    public class DiceCardSelfAbility_RMR_FishOnslaughtUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.allyCardDetail.DiscardACardLowest();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.DmgUp, 1, owner);
        }
    }

    public class DiceCardAbility_RMR_recover5breakpw : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            owner.breakDetail.RecoverBreak(5);
        }
    }

    public class DiceCardSelfAbility_RMR_SearingBlow : DiceCardSelfAbilityBase
    {
        // empty for description
    }

    public class DiceCardSelfAbility_RMR_Outburst : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Energy_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.allyCardDetail.DiscardACardLowest();
            owner.cardSlotDetail.RecoverPlayPointByCard(3);
        }
    }

    public class DiceCardAbility_RMR_allydraw1pw : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "DrawCard_Keyword" };
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            List<BattleUnitModel> gooners = BattleObjectManager.instance.GetAliveList(owner.faction).FindAll((BattleUnitModel x) => x != owner);
            if (gooners.Count > 0)
            {
                RandomUtil.SelectOne<BattleUnitModel>(gooners).allyCardDetail.DrawCards(1);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_break2atk : DiceCardSelfAbilityBase
    {
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            card.target.breakDetail.TakeBreakDamage(2, DamageType.Card_Ability, owner);
        }
    }

    public class DiceCardAbility_RMR_paralysisbind1atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Paralysis_Keyword", "Binding_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Paralysis, 1, owner);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Binding, 1, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_CutIn : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            if (card.target.cardSlotDetail.cardQueue.Count > 0)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                {
                    min = StageController.Instance._allCardList.Count(x => x.owner == card.target)
                });
            }
        }
    }
    
    public class DiceCardSelfAbility_RMR_MeatJamUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            unit.RecoverHP(30);
            BookModel bookItem = unit.UnitData.unitData.bookItem;
            if (!bookItem.GetDeckAll_nocopy()[bookItem.GetCurrentDeckIndex()].MoveCardToInventory(new LorId(LogLikeMod.ModId, 2000002)))
              return;
            LogueBookModels.DeleteCard(new LorId(LogLikeMod.ModId, 2000002));
        }
    }

    public class DiceCardSelFAbility_RMR_5shield : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_Shield_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRShield, 5, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_BladeWhirl : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Endurance_Keyword", "DrawCard_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            if (owner.bufListDetail.GetActivatedBuf(KeywordBuf.Endurance) != null)
            {
                owner.allyCardDetail.DrawCards(1);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_BladeWhirlUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Endurance_Keyword", "DrawCard_Keyword", "Energy_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            if (owner.bufListDetail.GetActivatedBuf(KeywordBuf.Endurance) != null)
            {
                owner.allyCardDetail.DrawCards(1);
                owner.cardSlotDetail.RecoverPlayPointByCard(1);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_HandlingWorkUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddBuf(new googoogaagaaresistances());
        }

        public class googoogaagaaresistances : BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
            public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
            {
                if (origin == AtkResist.Vulnerable || origin == AtkResist.Weak)
                {
                    return AtkResist.Normal;
                }
                return base.GetResistBP(origin, detail);
            }
            public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
            {
                if (origin == AtkResist.Vulnerable || origin == AtkResist.Weak)
                {
                    return AtkResist.Normal;
                }
                return base.GetResistHP(origin, detail);
            }
        }
    }

    #endregion

    #region Urban Plague
    public class DiceCardSelfAbility_RMR_TasteChain : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword", "Energy_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(base.owner.faction))
            {
                if (battleUnitModel != base.owner)
                {
                    battleUnitModel.bufListDetail.AddBuf(new BattleUnitBuf_luxunitybuf());
                }
            }
        }

        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.cardSlotDetail.RecoverPlayPointByCard(1);
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 1, owner);
        }
        public class BattleUnitBuf_luxunitybuf : BattleUnitBuf
        {
            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                base.OnUseCard(card);
                if (card.cardAbility != null)
                {
                    if (card.cardAbility.IsUniteCard)
                    {
                        _owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 1, _owner);
                    }
                }
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }

        /* skeleton for unity effects
         
        public override bool IsUniteCard => true;
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(base.owner.faction))
            {
                if (battleUnitModel != base.owner && !battleUnitModel.bufListDetail.HasBuf<BattleUnitBuf_luxunitybuf>())
                {
                    battleUnitModel.bufListDetail.AddBuf(new BattleUnitBuf_luxunitybuf());
                }
            }
        }
        public class BattleUnitBuf_luxunitybuf : BattleUnitBuf
        {
            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                base.OnUseCard(card);
                if (card.cardAbility != null)
                {
                    if (card.cardAbility.IsUniteCard)
                    {
                        
                    }
                }
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        } */
    }

    public class DiceCardSelfAbility_RMR_LowNight : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Strength_Keyword" };
        public override void OnDiscard(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnDiscard(unit, self);
            if (unit.hp > unit.MaxHp/2)
            {
                unit.TakeDamage(4, DamageType.Card_Ability);
                unit.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1, owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_LowNightUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Strength_Keyword", "RMR_Shield_Keyword" };
        public override void OnDiscard(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnDiscard(unit, self);
            if (unit.hp > unit.MaxHp / 2)
            {
                unit.TakeDamage(4, DamageType.Card_Ability);
                unit.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1, unit);
            }
            else if (unit.hp < unit.MaxHp / 2)
            {
                unit.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRShield, 4, unit);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_RulesBackstreets : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            if (card.target != null && card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) >= 4)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                {
                    min = 2
                });
            }
        }
    }

    public class DiceCardAbility_RMR_bleed2draw1atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword", "DrawCard_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 2, owner);
            owner.allyCardDetail.DrawCards(1);
        }
    }

    public class DiceCardSelfAbility_RMR_HandleRequest : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Burn_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddBuf(new BattleUnitBuf_burnPlus());
        }

        public class BattleUnitBuf_burnPlus : BattleUnitBuf
        {
            public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
            {
                if (cardBuf.bufType == KeywordBuf.Burn)
                {
                    return 1;
                }
                return 0;
            }

            public override void OnRoundEnd()
            {
                Destroy();
            }
        }

    }

    public class DiceCardAbility_RMR_nodeflectdamage : DiceCardAbilityBase
    {
        public override void BeforeRollDice()
        {
            base.BeforeRollDice();
            if (behavior.TargetDice != null)
            {
                behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus
                {
                    guardBreakAdder = -9999
                });
            }
        }
    }

    public class DiceCardAbility_RMR_boostdiceminvalue2pl : DiceCardAbilityBase
    {
        public override void OnLoseParrying()
        {
            base.OnLoseParrying();
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                min = 2
            });
        }
    }

    public class DiceCardSelfAbility_RMR_PrescriptOrder : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_Zeal_Keyword", "DrawCard_Keyword" };
        bool trigger;
        public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnEnterCardPhase(unit, self);
            if (!trigger)
            {
                trigger = true;
                foreach (BattleUnitModel amog in BattleObjectManager.instance.GetAliveList(unit.faction).FindAll((BattleUnitModel x) => x != unit))
                {
                    amog.allyCardDetail.AddNewCardToDeck(self.GetID());
                }
            }
        }
        public override void OnUseCard()
        {
            base.OnUseCard();
            if (owner.allyCardDetail.IsHighlander())
            {
                owner.allyCardDetail.DrawCards(1);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_PrescriptOrderUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_Zeal_Keyword", "DrawCard_Keyword", "RMR_StaggerShield_Keyword" };
        bool trigger;
        public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnEnterCardPhase(unit, self);
            if (!trigger)
            {
                trigger = true;
                foreach (BattleUnitModel amog in BattleObjectManager.instance.GetAliveList(unit.faction).FindAll((BattleUnitModel x) => x != unit))
                {
                    amog.allyCardDetail.AddNewCardToDeck(self.GetID());
                }
            }
        }
        public override void OnUseCard()
        {
            base.OnUseCard();
            if (owner.allyCardDetail.IsHighlander())
            {
                owner.allyCardDetail.DrawCards(1);
            }
            else
            {
                owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRStaggerShield, owner.allyCardDetail.GetHand().Count, owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_TargetSpotted : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_CriticalStrike_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.CritChance, 5, owner);
        }
    }

    public class DiceCardAbility_RMR_binding3crit : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Binding_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            if (owner.isCrit())
            {
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Binding, 3, owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_UnavoidableGazeUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Paralysis_Keyword" };
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            int stack = behavior.card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Paralysis);
            if (stack > 5)
            {
                stack = 5;
            }
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                power = stack
            });
        }
    }

    public class DiceCardSelfAbility_RMR_YoureSoft : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword", "RMR_BleedProtection_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 2, owner);
        }

        public override bool IsUniteCard => true;
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(base.owner.faction))
            {
                if (battleUnitModel != base.owner)
                {
                    battleUnitModel.bufListDetail.AddBuf(new BattleUnitBuf_luxunitybuf());
                }
            }
        }
        public class BattleUnitBuf_luxunitybuf : BattleUnitBuf
        {
            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                base.OnUseCard(card);
                if (card.cardAbility != null)
                {
                    if (card.cardAbility.IsUniteCard)
                    {
                        _owner.bufListDetail.AddKeywordBufByCard(RoguelikeBufs.BleedProtection, 1, _owner);
                    }
                }
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }

    public class DiceCardSelfAbility_RMR_Relay : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "DrawCard_Keyword" };
        public override void OnDiscard(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnDiscard(unit, self);
            RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(unit.faction).FindAll((BattleUnitModel x) => x != unit)).allyCardDetail.DrawCards(1);
        }
    }

    public class DiceCardSelfAbility_RMR_RelayUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "DrawCard_Keyword" };
        public override void OnDiscard(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnDiscard(unit, self);
            RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(unit.faction).FindAll((BattleUnitModel x) => x != unit)).allyCardDetail.DrawCards(1);
        }

        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.allyCardDetail.DrawCards(1);
        }
    }

    public class DiceCardSelfAbility_RMR_DarkCloudUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddBuf(new guggugubleeeeedd());
        }

        public class guggugubleeeeedd : BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (!behavior.abilityList.Contains(new DiceCardAbility_RMR_bleeding1atk2lessbreakdmg()))
                {
                    behavior.AddAbility(new DiceCardAbility_RMR_bleeding1atk2lessbreakdmg());
                }
            }
        }

    }

    public class DiceCardAbility_RMR_bleeding1atk2lessbreakdmg : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 1, owner);
        }
        public override void BeforeGiveDamage()
        {
            base.BeforeGiveDamage();
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                breakDmg = -2
            });
        }
    }

    public class DiceCardSelfAbility_RMR_UpbeatPerformance : DiceCardSelfAbilityBase
    {
        public override void OnWinParryingAtk()
        {
            base.OnWinParryingAtk();
            Effect();
        }

        public override void OnWinParryingDef()
        {
            base.OnWinParryingDef();
            Effect();
        }

        private void Effect()
        {
            foreach (BattleUnitModel amog in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                int count = amog.emotionDetail.CreateEmotionCoin(EmotionCoinType.Positive);
                SingletonBehavior<BattleManagerUI>.Instance.ui_battleEmotionCoinUI.OnAcquireCoin(amog, EmotionCoinType.Positive, count);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_ReturnFireUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_CriticalStrike_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.CritChance, 8, owner);
        }
    }

    public class DiceCardAbility_RMR_1weakcrit : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Weak_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            if (owner.isCrit())
            {
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Weak, 1, owner);
            }
        }
    }

    public class DiceCardAbility_RMR_2weakcrit : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Weak_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            if (owner.isCrit())
            {
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Weak, 2, owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_FrontalDodge : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.breakDetail.RecoverBreak(8);
        }
        public override bool IsUniteCard => true;
    }

    public class DiceCardSelfAbility_RMR_FlankAttack : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 1, owner);
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(base.owner.faction))
            {
                if (!battleUnitModel.bufListDetail.HasBuf<BattleUnitBuf_luxunitybuf>())
                {
                    battleUnitModel.bufListDetail.AddBuf(new BattleUnitBuf_luxunitybuf());
                }
            }
        }
        public class BattleUnitBuf_luxunitybuf : BattleUnitBuf
        {
            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                base.OnUseCard(card);
                if (card.cardAbility != null)
                {
                    if (card.cardAbility.IsUniteCard)
                    {
                        card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                        {
                            power = 1
                        });
                    }
                }
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }

    public class DiceCardSelfAbility_RMR_FlankAttackUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override bool IsUniteCard => true;
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 1, owner);
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(base.owner.faction))
            {
                if (!battleUnitModel.bufListDetail.HasBuf<DiceCardSelfAbility_RMR_FlankAttack.BattleUnitBuf_luxunitybuf>())
                {
                    battleUnitModel.bufListDetail.AddBuf(new DiceCardSelfAbility_RMR_FlankAttack.BattleUnitBuf_luxunitybuf());
                }
            }
        }
    }

    public class DiceCardSelfAbility_RMR_RedNotes : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Strength_Keyword", "Endurance_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            List<BattleUnitModel> goobers = BattleObjectManager.instance.GetAliveList(owner.faction).FindAll((BattleUnitModel x) => x.bufListDetail.GetActivatedBuf(KeywordBuf.Strength) == null);
            if (goobers.Count > 0)
            {
                BattleUnitModel goober = RandomUtil.SelectOne<BattleUnitModel>(goobers);
                goober.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1, owner);
                goober.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 1, owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_RedNotesUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Strength_Keyword", "Endurance_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            for (int i = 0; i < 2; i++)
            {
                List<BattleUnitModel> goobers = BattleObjectManager.instance.GetAliveList(owner.faction).FindAll((BattleUnitModel x) => x.bufListDetail.GetActivatedBuf(KeywordBuf.Strength) == null);
                if (goobers.Count > 0)
                {
                    BattleUnitModel goober = RandomUtil.SelectOne<BattleUnitModel>(goobers);
                    goober.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1, owner);
                    goober.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 1, owner);
                }
            }
        }
    }

    public class DiceCardSelfAbility_RMR_WillBeTasty : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Recover_Keyword"};
        public override void OnUseCard()
        {
            base.OnUseCard();
            if (card.target.IsBreakLifeZero())
            {
                owner.RecoverHP(4);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_IHATECQC : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Protection_Keyword", "BreakProtection_Keyword", "RMR_CriticalStrike_Keyword" };
        public override void OnLoseParrying()
        {
            base.OnLoseParrying();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 1, owner);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 1, owner);
            owner.bufListDetail.AddKeywordBufByCard(RoguelikeBufs.CritChance, 3, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_IHATECQCUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Protection_Keyword", "BreakProtection_Keyword", "RMR_CriticalStrike_Keyword" };
        public override void OnLoseParrying()
        {
            base.OnLoseParrying();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 1, owner);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 1, owner);
            owner.bufListDetail.AddKeywordBufByCard(RoguelikeBufs.CritChance, 5, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_TakeTheShot : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "DrawCard_Keyword", "Energy_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            if (owner.isCrit())
            {
                owner.allyCardDetail.DrawCards(1);
                owner.cardSlotDetail.RecoverPlayPointByCard(1);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_TakeTheShotUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "DrawCard_Keyword", "Energy_Keyword", "RMR_CriticalStrike_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.CritChance, 5, owner);
        }
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            if (owner.isCrit())
            {
                owner.allyCardDetail.DrawCards(1);
                owner.cardSlotDetail.RecoverPlayPointByCard(1);
            }
        }
    }

    public class DiceCardAbility_RMR_5breakdmgcrit : DiceCardAbilityBase
    {
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            if (owner.isCrit())
            {
                target.breakDetail.TakeBreakDamage(5, DamageType.Card_Ability, owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_OpportunitySpotted : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            int count = 0;
            foreach (BattleUnitBuf buf in card.target.bufListDetail.GetActivatedBufList())
            {
                if (buf.positiveType == BufPositiveType.Negative)
                {
                    count++;
                }
            }
            if (count > 3)
            {
                count = 3;
            }
            card.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus
            {
                max = count
            });
        }
    }

    public class DiceCardSelfAbility_RMR_YoureHindranceUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Disarm_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Disarm, 1, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_CumulusWall : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_Shield_Keyword", "RMR_StaggerShield_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRShield, 5, owner);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRStaggerShield, 5, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_CumulusWallUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_Shield_Keyword", "RMR_StaggerShield_Keyword", "Bleeding_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRShield, 5, owner);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRStaggerShield, 5, owner);
        }

        public override void OnUseCard()
        {
            base.OnUseCard();
            if (card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) >= 4)
            {
                owner.bufListDetail.AddKeywordBufByCard(RoguelikeBufs.RMRShield, 5, owner);
                owner.bufListDetail.AddKeywordBufByCard(RoguelikeBufs.RMRStaggerShield, 5, owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_ShrineToMusic : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Weak_Keyword", "Disarm_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            List<BattleUnitModel> goobers = BattleObjectManager.instance.GetAliveList_opponent(owner.faction).FindAll((BattleUnitModel x) => x.bufListDetail.GetActivatedBuf(KeywordBuf.Weak) == null);
            if (goobers.Count > 0)
            {
                BattleUnitModel goober = RandomUtil.SelectOne<BattleUnitModel>(goobers);
                goober.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Weak, 1, owner);
                goober.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Disarm, 1, owner);
            }
        }
    }
    public class DiceCardSelfAbility_RMR_ShrineToMusicUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Weak_Keyword", "Disarm_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            for (int i = 0; i < 2; i++)
            {
                List<BattleUnitModel> goobers = BattleObjectManager.instance.GetAliveList_opponent(owner.faction).FindAll((BattleUnitModel x) => x.bufListDetail.GetActivatedBuf(KeywordBuf.Weak) == null);
                if (goobers.Count > 0)
                {
                    BattleUnitModel goober = RandomUtil.SelectOne<BattleUnitModel>(goobers);
                    goober.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Weak, 1, owner);
                    goober.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Disarm, 1, owner);
                }
            }
        }
    }

    public class DiceCardSelfAbility_RMR_Headshot : DiceCardSelfAbilityBase
    {
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            if (owner.isCrit())
            {
                card.target?.currentDiceAction?.DestroyDice(DiceMatch.NextDice, DiceUITiming.Start);
            }
        }
    }

    public class DiceCardAbility_RMR_dmgupquickness1ally2atk : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Quickness_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            foreach (BattleUnitModel item in BattleObjectManager.instance.GetAliveList_random(base.owner.faction, 2))
            {
                item.bufListDetail.AddKeywordBufByCard(KeywordBuf.DmgUp, 1, base.owner);
                item.bufListDetail.AddKeywordBufByCard(KeywordBuf.Quickness, 1, base.owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_SpearedSweep : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Disarm_Keyword" };
        public override void OnStartParrying()
        {
            base.OnStartParrying();
            card.target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Disarm, 2, owner);
        }

        public override void OnEndBattle()
        {
            base.OnEndBattle();
            card.target.bufListDetail.RemoveBufAll(KeywordBuf.Disarm);
        }
    }

    public class DiceCardSelfAbility_RMR_SpearedSweepUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Disarm_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            card.target.TakeDamage(1, DamageType.Card_Ability, owner);
        }
        public override void OnStartParrying()
        {
            base.OnStartParrying();
            card.target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Disarm, 2, owner);
        }

        public override void OnEndBattle()
        {
            base.OnEndBattle();
            card.target.bufListDetail.RemoveBufAll(KeywordBuf.Disarm);
        }
    }

    public class DiceCardAbility_RMR_SpearSweepDie : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            BattleDiceBehavior die = new BattleDiceBehavior
            {
                behaviourInCard = new DiceBehaviour
                {
                    Min = 3,
                    Dice = 5,
                    Detail = BehaviourDetail.Penetrate,
                    Type = BehaviourType.Atk,
                    MotionDetail = MotionDetail.Z,
                    MotionDetailDefault = MotionDetail.N,
                    Script = ""
                }
            };
            behavior.card.AddDice(die);
        }
    }

    public class DiceCardAbility_RMR_SpearSweepDieUpgrade : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            BattleDiceBehavior die = new BattleDiceBehavior
            {
                behaviourInCard = new DiceBehaviour
                {
                    Min = 4,
                    Dice = 5,
                    Detail = BehaviourDetail.Penetrate,
                    Type = BehaviourType.Atk,
                    MotionDetail = MotionDetail.Z,
                    MotionDetailDefault = MotionDetail.N,
                    Script = ""
                }
            };
            behavior.card.AddDice(die);
        }
    }

    public class DiceCardSelfAbility_RMR_FullStopToLife : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_CriticalStrike_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            List<BattleUnitModel> goobers = BattleObjectManager.instance.GetAliveList_opponent(owner.faction).FindAll((BattleUnitModel x) => x != owner);
            if (goobers.Count > 0)
            {
                RandomUtil.SelectOne<BattleUnitModel>(goobers).bufListDetail.AddKeywordBufByCard(RoguelikeBufs.CritChance, 4, owner);
            }
            owner.bufListDetail.AddKeywordBufByCard(RoguelikeBufs.CritChance, 4, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_FullStopToLifeUpgrade1 : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_CriticalStrike_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            List<BattleUnitModel> goobers = BattleObjectManager.instance.GetAliveList_opponent(owner.faction).FindAll((BattleUnitModel x) => x != owner);
            if (goobers.Count > 0)
            {
                RandomUtil.SelectOne<BattleUnitModel>(goobers).bufListDetail.AddKeywordBufByCard(RoguelikeBufs.CritChance, 8, owner);
            }
            owner.bufListDetail.AddKeywordBufByCard(RoguelikeBufs.CritChance, 8, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_FullStopToLifeUpgrade2 : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_CriticalStrike_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.CritChance, 5, owner);
            owner.bufListDetail.AddKeywordBufByCard(RoguelikeBufs.CritChance, 8, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_ElectricShock : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Paralysis_Keyword" };
        public override void OnEndBattle()
        {
            base.OnEndBattle();
            BattleUnitBuf para = card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Paralysis);
            if (para != null)
            {
                card.target.breakDetail.TakeBreakDamage(para.stack * 2, DamageType.Card_Ability, owner);
                para.Destroy();
                card.target.bufListDetail.RemoveBuf(para);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_ElectricShockUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Paralysis_Keyword" };
        public override void OnEndBattle()
        {
            base.OnEndBattle();
            BattleUnitBuf para = card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Paralysis);
            if (para != null)
            {
                owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.WarpCharge, para.stack * 2, owner);
                para.Destroy();
                card.target.bufListDetail.RemoveBuf(para);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_HeresMyChance : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override bool IsUniteCard => true;
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                power = 1
            });
        }
    }

    public class DiceCardSelfAbility_RMR_Whet : DiceCardSelfAbilityBase
    {
        public override void OnEndBattle()
        {
            base.OnEndBattle();
            owner.bufListDetail.AddBuf(new googoogaagaapower());
        }

        public class googoogaagaapower : BattleUnitBuf
        {
            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                base.OnUseCard(card);
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                {
                    power = 1
                });
                this.Destroy();
            }
        }
    }

    public class DiceCardAbility_RMR_Detonate : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Burn_Keyword" };
        public override void BeforeGiveDamage()
        {
            base.BeforeGiveDamage();
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                dmgRate = -9999
            });
        }
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, behavior.DiceResultValue, owner);
        }
    }

    public class DiceCardAbility_RMR_SkyCut : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void BeforeGiveDamage()
        {
            base.BeforeGiveDamage();
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                breakRate = -9999
            });
        }
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, behavior.DiceResultValue, owner);
        }
    }

    public class DiceCardAbility_RMR_add5powerpl : DiceCardAbilityBase
    {
        public override void OnLoseParrying()
        {
            base.OnLoseParrying();
            card.ApplyDiceStatBonus(DiceMatch.NextDice, new DiceStatBonus
            {
                power = 5
            });
        }
    }

    public class DiceCardSelfAbility_RMR_SearingSword : DiceCardSelfAbilityBase
    {
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                dmgRate = -50
            });
        }
    }

    public class DiceCardSelfAbility_RMR_HeadtoHead : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 2, owner);
        }
        public override bool IsUniteCard => true;
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(base.owner.faction))
            {
                if (battleUnitModel != base.owner && !battleUnitModel.bufListDetail.HasBuf<BattleUnitBuf_luxunitybuf>())
                {
                    battleUnitModel.bufListDetail.AddBuf(new BattleUnitBuf_luxunitybuf());
                }
            }
        }
        public class BattleUnitBuf_luxunitybuf : BattleUnitBuf
        {
            public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
            {
                base.OnStartParrying(card);
                if (card.cardAbility != null)
                {
                    if (card.cardAbility.IsUniteCard)
                    {
                        int amount = _owner.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding);
                        card.target.currentDiceAction?.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                        {
                            dmg = -amount,
                            breakDmg = -amount 
                        });
                    }
                }
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }

    public class DiceCardSelfAbility_RMR_HeadtoHeadUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override bool IsUniteCard => true;
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(base.owner.faction))
            {
                if (battleUnitModel != base.owner && !battleUnitModel.bufListDetail.HasBuf<DiceCardSelfAbility_RMR_HeadtoHead.BattleUnitBuf_luxunitybuf>())
                {
                    battleUnitModel.bufListDetail.AddBuf(new DiceCardSelfAbility_RMR_HeadtoHead.BattleUnitBuf_luxunitybuf());
                }
            }
        }

        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 2, owner);
        }

        public override void OnStartParrying()
        {
            base.OnStartParrying();
            owner.allyCardDetail.DrawCards(1);
        }
    }

    public class DiceCardSelfAbility_RMR_SunsetBlade : DiceCardSelfAbilityBase
    {
        public override void OnStartParrying()
        {
            base.OnStartParrying();
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                min = 2,
                dmg = -3
            });
        }
    }

    public class DiceCardSelfAbility_RMR_StructuralAnalysis : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Paralysis_Keyword" };
        public override void OnUseCard()
        {
            if (card.target == null)
            {
                return;
            }
            List<BehaviourDetail> list = new List<BehaviourDetail>();
            int resistValue = GetResistValue(BehaviourDetail.Slash);
            int resistValue2 = GetResistValue(BehaviourDetail.Penetrate);
            int resistValue3 = GetResistValue(BehaviourDetail.Hit);
            int num = resistValue;
            if (resistValue2 > num)
            {
                num = resistValue2;
            }
            if (resistValue3 > num)
            {
                num = resistValue3;
            }
            if (num == resistValue)
            {
                list.Add(BehaviourDetail.Slash);
            }
            if (num == resistValue2)
            {
                list.Add(BehaviourDetail.Penetrate);
            }
            if (num == resistValue3)
            {
                list.Add(BehaviourDetail.Hit);
            }
            BehaviourDetail detail = RandomUtil.SelectOne(list);
            foreach (BattleDiceBehavior diceBehavior in card.GetDiceBehaviorList())
            {
                if (IsAttackDice(diceBehavior.behaviourInCard.Detail))
                {
                    diceBehavior.behaviourInCard = diceBehavior.behaviourInCard.Copy();
                    diceBehavior.behaviourInCard.Detail = detail;
                }
            }
        }

        private int GetResistValue(BehaviourDetail detail)
        {
            return Mathf.RoundToInt((0f + BookModel.GetResistRate(card.target.GetResistHP(detail)) + BookModel.GetResistRate(card.target.GetResistBP(detail))) * 10f);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (behavior.Detail == BehaviourDetail.Penetrate)
            {
                card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Paralysis, 1, owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_Feast : DiceCardSelfAbilityBase
    {
        public override void OnEndBattle()
        {
            base.OnEndBattle();
            if (card.target.IsDead())
            {
                owner.allyCardDetail.ExhaustACardAnywhere(card.card);
                card.card.exhaust = true;
                if (owner.faction == Faction.Player && LogueBookModels.playerBattleModel.Contains<UnitBattleDataModel>(owner.UnitData))
                {
                    LogueBookModels.AddPlayerStat(LogueBookModels.playerBattleModel.Find((UnitBattleDataModel x) => x == owner.UnitData), new LogStatAdder { maxhp = 2 });
                }
                else
                {
                    owner.bufListDetail.AddBuf(new maxhphehehehe());
                }
                // add max hp increase
            }
        }

        public class maxhphehehehe : BattleUnitBuf
        {
            public override StatBonus GetStatBonus()
            {
                return new StatBonus
                {
                    hpAdder = 2
                };
            }
        }
    }

    public class DiceCardSelfAbility_RMR_FeastUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnEndBattle()
        {
            base.OnEndBattle();
            if (card.target.IsDead())
            {
                owner.allyCardDetail.ExhaustACardAnywhere(card.card);
                card.card.exhaust = true;
                // add max hp increase
                if (owner.faction == Faction.Player && LogueBookModels.playerBattleModel.Contains<UnitBattleDataModel>(owner.UnitData))
                {
                    LogueBookModels.AddPlayerStat(LogueBookModels.playerBattleModel.Find((UnitBattleDataModel x) => x == owner.UnitData), new LogStatAdder { maxhp = 3 });
                }
                else
                {
                    owner.bufListDetail.AddBuf(new maxhphehehehe());
                }
            }
        }

        public class maxhphehehehe : BattleUnitBuf
        {
            public override StatBonus GetStatBonus()
            {
                return new StatBonus
                {
                    hpAdder = 3
                };
            }
        }
    }

    public class DiceCardSelfAbility_RMR_Eject : DiceCardSelfAbilityBase
    {
        public override void OnStartParrying()
        {
            base.OnStartParrying();
            card.target.currentDiceAction?.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                dmg = -2,
                breakDmg = -2
            });
        }
    }

    public class DiceCardSelfAbility_RMR_EjectUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnStartParrying()
        {
            base.OnStartParrying();
            card.target.currentDiceAction?.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                dmg = -4,
                breakDmg = -4
            });
        }
    }

    public class DiceCardSelfAbility_RMR_NotAnotherStep : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_CriticalStrike_Keyword" };
        public override void OnStartParrying()
        {
            base.OnStartParrying();
            if (owner.speedDiceResult[card.slotOrder].value < card.target.speedDiceResult[card.targetSlotOrder].value)
            {
                owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.CritChance, 10, owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_NotAnotherStepUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_CriticalStrike_Keyword" };
        public override void OnStartParrying()
        {
            base.OnStartParrying();
            if (owner.speedDiceResult[card.slotOrder].value < card.target.speedDiceResult[card.targetSlotOrder].value)
            {
                owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.CritChance, 15, owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_SharpenedBlade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddBuf(new hehehehebleeeedILVOVEVEBLEEEED());
        }

        public class hehehehebleeeedILVOVEVEBLEEEED : BattleUnitBuf
        {
            public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
            {
                if (cardBuf.bufType == KeywordBuf.Bleeding)
                {
                    return 1;
                }
                return base.OnGiveKeywordBufByCard(cardBuf, stack, target);
            }
            public override void BeforeGiveDamage(BattleDiceBehavior behavior)
            {
                base.BeforeGiveDamage(behavior);
                behavior.ApplyDiceStatBonus(new DiceStatBonus
                {
                    dmg = -2,
                    breakDmg = -2
                });
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }

    public class DiceCardSelfAbility_RMR_SharpenedBladeUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddBuf(new hehehehebleeeedILVOVEVEBLEEEED());
        }

        public class hehehehebleeeedILVOVEVEBLEEEED : BattleUnitBuf
        {
            public override int OnGiveKeywordBufByCard(BattleUnitBuf cardBuf, int stack, BattleUnitModel target)
            {
                if (cardBuf.bufType == KeywordBuf.Bleeding)
                {
                    return 2;
                }
                return base.OnGiveKeywordBufByCard(cardBuf, stack, target);
            }
            public override void BeforeGiveDamage(BattleDiceBehavior behavior)
            {
                base.BeforeGiveDamage(behavior);
                behavior.ApplyDiceStatBonus(new DiceStatBonus
                {
                    dmg = -4,
                    breakDmg = -4
                });
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }

    public class DiceCardSelfAbility_RMR_SilentMist : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Protection_Keyword", "Bleeding_Keyword", "BreakProtection_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Protection, 3, owner);
            if (card.target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) >= 4)
            {
                owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.BreakProtection, 2, owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_ButterflySlash : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Burn_Keyword" };
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                dmg = -4
            });
        }
        public override void OnWinParryingAtk()
        {
            base.OnWinParryingAtk();
            card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 1, owner);
        }

        public override void OnWinParryingDef()
        {
            base.OnWinParryingDef();
            card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 1, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_IndiscriminateShots : DiceCardSelfAbilityBase
    {
        public override void OnStartBattleAfterCreateBehaviour()
        {
            base.OnStartBattleAfterCreateBehaviour();

        }
    }

    public class DiceCardAbility_RMR_IndiscriminateShotsDie1 : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            if (owner.isCrit())
            {
                owner.bufListDetail.AddBuf(new gagagapowerrINFINTIPOWEREWRR());
            }
        }

        public class gagagapowerrINFINTIPOWEREWRR : BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
            public override void OnRollDice(BattleDiceBehavior behavior)
            {
                base.OnRollDice(behavior);
                if (behavior.abilityList.Exists((DiceCardAbilityBase x) => x is DiceCardAbility_RMR_IndiscriminateShotsDie2))
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus
                    {
                        power = 2
                    });
                    this.Destroy();
                }
            }
        }
    }

    public class DiceCardAbility_RMR_IndiscriminateShotsDie1Upgrade : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            if (owner.isCrit())
            {
                owner.bufListDetail.AddBuf(new gagagapowerrINFINTIPOWEREWRR());
            }
        }

        public class gagagapowerrINFINTIPOWEREWRR : BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
            public override void OnRollDice(BattleDiceBehavior behavior)
            {
                base.OnRollDice(behavior);
                if (behavior.abilityList.Exists((DiceCardAbilityBase x) => x is DiceCardAbility_RMR_IndiscriminateShotsDie2))
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus
                    {
                        power = 4
                    });
                    this.Destroy();
                }
                
            }
        }
    }

    public class DiceCardAbility_RMR_IndiscriminateShotsDie2 : DiceCardAbilityBase
    {
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.TakeDamage(behavior.DiceResultValue, DamageType.Card_Ability, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_BeyondShadow : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_CriticalStrike_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            if (owner.bufListDetail.GetKewordBufStack(RoguelikeBufs.CritChance)/100 > RandomUtil.valueForProb)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                {
                    power = 2
                });
            }
        }
    }

    public class DiceCardAbility_RMR_2bleedcrit : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            if (owner.isCrit())
            {
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 2, owner);
            }
        }
    }

    public class DiceCardAbility_RMR_3bleedcrit : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            if (owner.isCrit())
            {
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 3, owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_ScatteringSlash : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_Shield_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRShield, 12, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_ScatteringSlashUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "RMR_Shield_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.RMRShield, 16, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_UnforgettableMelody : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Strength_Keyword", "DrawCard_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            foreach (BattleUnitModel goofy in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (goofy.bufListDetail.GetActivatedBuf(KeywordBuf.Strength) != null)
                {
                    goofy.allyCardDetail.DrawCards(1);
                }
            }
        }
    }

    public class DiceCardSelfAbility_RMR_UnforgettableMelodyUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Strength_Keyword", "DrawCard_Keyword" };
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1, owner);
            foreach (BattleUnitModel goofy in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (goofy.bufListDetail.GetActivatedBuf(KeywordBuf.Strength) != null)
                {
                    goofy.allyCardDetail.DrawCards(1);
                }
            }
        }
    }

    public class DiceCardSelfAbility_RMR_FaintMemories : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.breakDetail.TakeBreakDamage(7, DamageType.Card_Ability);
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                power = 2
            });
        }
    }

    public class DiceCardSelfAbility_RMR_FaintMemoriesUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.breakDetail.TakeBreakDamage(9, DamageType.Card_Ability);
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                power = 3
            });
        }
    }

    public class DiceCardAbility_RMR_KillMotherfuckerOfChoice : DiceCardAbilityBase
    {
        public override void BeforeGiveDamage()
        {
            base.BeforeGiveDamage();
            if (owner.isCrit())
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus
                {
                    dmgRate = 50,
                    breakRate = 50
                }); ;
            }
        }
    }

    public class DiceCardAbility_RMR_KillMotherfuckerOfChoiceUpgrade : DiceCardAbilityBase
    {
        public override void BeforeGiveDamage()
        {
            base.BeforeGiveDamage();
            if (owner.isCrit())
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus
                {
                    dmgRate = 75,
                    breakRate = 75
                }); ;
            }
        }
    }

    public class DiceCardSelfAbility_RMR_BindingArmsUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            List<BattleDiceCardBuf> list = card.card.GetBufList().FindAll((BattleDiceCardBuf x) => x is CostDownSelfBuf);
            if (list.Count < 3)
            {
                card.card.AddBuf(new CostDownSelfBuf());
            }
        }

        public class CostDownSelfBuf : BattleDiceCardBuf
        {
            public override int GetCost(int oldCost)
            {
                return oldCost - 1;
            }
        }
    }

    public class DiceCardSelfAbility_RMR_HeavyPeaks : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Strength_Keyword", "Endurance_Keyword" };
        public override void OnEndBattle()
        {
            base.OnEndBattle();
            if (owner.emotionDetail._emotionCoins.Count >= owner.emotionDetail.MaximumCoinNumber)
            {
                owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 3, owner);
                owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 3, owner);
            }
        }
    }

    public class DiceCardAbility_RMR_nodamage : DiceCardAbilityBase
    {
        public override void BeforeGiveDamage()
        {
            base.BeforeGiveDamage();
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                dmgRate = -9999,
                breakRate = -9999
            });
        }
    }

    public class DiceCardSelfAbility_RMR_Tailoring : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "DrawCard_Keyword" };
        public class BattleDiceCardBuf_costDownCard : BattleDiceCardBuf
        {
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }

            public override int GetCost(int oldCost)
            {
                return oldCost - 1;
            }
        }
        public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnEnterCardPhase(unit, self);
            if (unit.allyCardDetail.GetHand().Exists((BattleDiceCardModel x) => x.GetID() == new LorId(LogLikeMod.ModId, 390001)))
            {
                self.AddBufWithoutDuplication(new BattleDiceCardBuf_costDownCard());
            }
        }

        public override void OnUseCard()
        {
            base.OnUseCard();
            if (!owner.allyCardDetail.GetHand().Exists((BattleDiceCardModel x) => x.GetID() == new LorId(LogLikeMod.ModId, 390001)))
            {
                owner.allyCardDetail.DrawCards(2);
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                {
                    dmg = -4,
                    breakDmg = -4
                });
            }
        }
    }

    public class DiceCardSelfAbility_RMR_TailoringUpgrade : DiceCardSelfAbilityBase
    {
        public class BattleDiceCardBuf_costDownCard : BattleDiceCardBuf
        {
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }

            public override int GetCost(int oldCost)
            {
                return oldCost - 1;
            }
        }
        public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnEnterCardPhase(unit, self);
            if (unit.allyCardDetail.GetHand().Exists((BattleDiceCardModel x) => x.GetID() == new LorId(LogLikeMod.ModId, 390001)))
            {
                self.AddBufWithoutDuplication(new BattleDiceCardBuf_costDownCard());
            }
        }

        public override void OnEndBattle()
        {
            base.OnEndBattle();
            if (card.target.IsDead())
            {
                foreach (var book in LogLikeMod.rewards_lastKill)
                {
                    if (LogLikeMod.rewards.Remove(book))
                    {
                        DropBookXmlInfo book2 = Singleton<DropBookXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, (book.chapter * 1000) + (book.id.id - book.chapter * 1000) + (book.id.id.ToString().EndsWith("4") ? 0 : 1)));
                        LogLikeMod.rewards.Add(book2);
                    }
                }
            }
        }
    }


    public class DiceCardSelfAbility_RMR_FascinatingFabric : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Protection_Keyword", "BreakProtection_Keyword" };
        public override bool BeforeAddToHand(BattleUnitModel unit, BattleDiceCardModel self)
        {
            if (unit.allyCardDetail.GetAllDeck().FindAll((BattleDiceCardModel x) => x.GetID() == self.GetID()).Count >= 3)
            {
                return false;
            }
            return true;
        }
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 1, owner);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 1, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_FascinatingFabricUpgrade : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Protection_Keyword", "BreakProtection_Keyword" };
        public override bool BeforeAddToHand(BattleUnitModel unit, BattleDiceCardModel self)
        {
            if (unit.allyCardDetail.GetAllDeck().FindAll((BattleDiceCardModel x) => x.GetID() == self.GetID()).Count >= 3)
            {
                return false;
            }
            return true;
        }
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 2, owner);
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 2, owner);
        }
    }

    public class DiceCardAbility_RMR_CrackOfDawnDie : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Burn_Keyword" };
        public override void BeforeGiveDamage()
        {
            base.BeforeGiveDamage();
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                dmgRate = -9999
            });
        }
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 9, owner);
        }
    }

    public class DiceCardAbility_RMR_CrackOfDawnDieUpgrade : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Burn_Keyword" };
        public override void BeforeGiveDamage()
        {
            base.BeforeGiveDamage();
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                dmgRate = -9999
            });
        }
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 11, owner);
        }
    }

    public class DiceCardSelfAbility_RMR_NowDie : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 5, owner);
        }
        public override bool IsUniteCard => true;
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(base.owner.faction))
            {
                if (battleUnitModel != base.owner && !battleUnitModel.bufListDetail.HasBuf<BattleUnitBuf_luxunitybuf>())
                {
                    battleUnitModel.bufListDetail.AddBuf(new BattleUnitBuf_luxunitybuf());
                }
            }
        }
        public class BattleUnitBuf_luxunitybuf : BattleUnitBuf
        {
            public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
            {
                base.OnUseCard(card);
                if (card.cardAbility != null)
                {
                    if (card.cardAbility.IsUniteCard)
                    {
                        _owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.DmgUp, _owner.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) / 3, _owner);
                    }
                }
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
        }
    }

    public class DiceCardSelfAbility_RMR_Refine : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            owner.bufListDetail.AddBuf(new refinebuf());
        }

        public class refinebuf :BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior.Detail == BehaviourDetail.Penetrate)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus
                    {
                        power = 2
                    });
                }
            }

            public override void BeforeGiveDamage(BattleDiceBehavior behavior)
            {
                base.BeforeGiveDamage(behavior);
                if (behavior.Detail == BehaviourDetail.Penetrate)
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus
                    {
                        dmg = -2,
                        breakDmg = -2
                    });
                }
            }
        }
    }

    public class DiceCardSelfAbility_RMR_Sakura : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            int count = owner.allyCardDetail.GetHand().Count;
            if (count > 0)
            {
                for (int i = 0; i < count && i < 4; i++)
                {
                    owner.allyCardDetail.DisCardACardRandom();
                    card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                    {
                        min = 1
                    });
                }
            }
        }
    }

    public class DiceCardSelfAbility_RMR_SakuraUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            int count = owner.allyCardDetail.GetHand().Count;
            if (count > 0)
            {
                for (int i = 0; i < count && i < 4; i++)
                {
                    owner.allyCardDetail.DisCardACardRandom();
                    card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
                    {
                        min = 1
                    });
                    if (i == 3)
                    {
                        owner.bufListDetail.AddBuf(new googoogaagaaresistances());
                    }
                }
            }
        }
     

        public class googoogaagaaresistances : BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                this.Destroy();
            }
            public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
            {
                if (origin == AtkResist.Vulnerable || origin == AtkResist.Weak)
                {
                    return AtkResist.Normal;
                }
                return base.GetResistBP(origin, detail);
            }
            public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
            {
                if (origin == AtkResist.Vulnerable || origin == AtkResist.Weak)
                {
                    return AtkResist.Normal;
                }
                return base.GetResistHP(origin, detail);
            }
        }
    
    }

    public class DiceCardAbility_RMR_InkOverDie : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword", "DrawCard_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            if (target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) >= 4)
            {
                int count = target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) / 4;
                if (count > 2)
                {
                    count = 2;
                }
                owner.allyCardDetail.DrawCards(count);
            }
        }
    }

    public class DiceCardAbility_RMR_InkOverDieUpgrade : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword", "DrawCard_Keyword" };
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            if (target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) >= 3)
            {
                int count = target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) / 3;
                if (count > 3)
                {
                    count = 3;
                }
                owner.allyCardDetail.DrawCards(count);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_energy2draw1 : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Energy_Keyword", "DrawCard_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.cardSlotDetail.RecoverPlayPointByCard(2);
            owner.allyCardDetail.DrawCards(1);
        }
    }

    public class DiceCardSelfAbility_RMR_Observe : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddBuf(new BattleUnitBuf_rmrobservebuf());
        }
        public override string[] Keywords => new string[] { "Strength_Keyword" };
        public class BattleUnitBuf_rmrobservebuf : BattleUnitBuf
        {
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                _owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1, _owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_ObserveUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 2, owner);
            owner.bufListDetail.AddBuf(new BattleUnitBuf_rmrobservebuf());
        }
        public override string[] Keywords => new string[] { "Strength_Keyword" };
        public class BattleUnitBuf_rmrobservebuf : BattleUnitBuf
        {
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                _owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1, _owner);
            }
        }
    }

    public class DiceCardSelfAbility_RMR_WrathOfTormentUpgrade : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            owner.TakeDamage(9, DamageType.Card_Ability);
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
            {
                power = 3
            });
        }
    }

    public class DiceCardSelfAbility_RMR_bleed1atkcard : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Bleeding_Keyword" };
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 1, owner);
        }
    }

    #endregion
}
