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
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 1, owner);
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
            GlobalLogueEffectBase effect = GlobalLogueEffectManager.Instance.GetEffectList().Find(x => x is RMREffect_Remote);
            if (effect != null)
            {
                GlobalLogueEffectManager.Instance.RemoveEffect(effect);
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
            if (owner.cardHistory.GetCurrentRoundCardList(Singleton<StageController>.Instance.RoundTurn).Count <= 0)
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
                 owner.allyCardDetail.DrawCardsAllSpecific(drawpile.OrderBy(x => x.GetRarity()).Last().GetID());
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
}
