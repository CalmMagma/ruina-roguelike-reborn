using System;
using System.Collections.Generic;
using System.Linq;
using abcdcode_LOGLIKE_MOD;
using LOR_BattleUnit_UI;
using LOR_DiceSystem;
using LOR_XML;

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
            owner.bufListDetail.AddBuf(new DiceCardSelfAbility_burnPlus.BattleUnitBuf_burnPlus());
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

    public class DiceCardAbilityBase_RMR_LawOrder : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(LogLikeMod.ModId, 390001));
            List <BattleDiceBehavior> list = new List<BattleDiceBehavior>();
            int num = 0;
            foreach (DiceBehaviour diceBehaviour in cardItem.DiceBehaviourList)
            {
                BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
                battleDiceBehavior.behaviourInCard = diceBehaviour.Copy();
                battleDiceBehavior.SetIndex(num++);
                list.Add(battleDiceBehavior);
            }
            owner.cardSlotDetail.keepCard.AddBehaviours(cardItem, list);
        }
    }

    public class DiceCardAbilityBase_RMR_LawOrderUpgrade : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(LogLikeMod.ModId, 390002));
            List<BattleDiceBehavior> list = new List<BattleDiceBehavior>();
            int num = 0;
            foreach (DiceBehaviour diceBehaviour in cardItem.DiceBehaviourList)
            {
                BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
                battleDiceBehavior.behaviourInCard = diceBehaviour.Copy();
                battleDiceBehavior.SetIndex(num++);
                list.Add(battleDiceBehavior);
            }
            owner.cardSlotDetail.keepCard.AddBehaviours(cardItem, list);
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
                // change this to 2 luck once luck is no longer busted, or keep it as is if it feels funny
                owner.bufListDetail.AddKeywordBufThisRoundByCard(RoguelikeBufs.CritChance, x.GetCost(), owner);
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

}
