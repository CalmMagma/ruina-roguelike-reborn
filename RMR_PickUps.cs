using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;
using GameSave;

namespace RogueLike_Mod_Reborn
{
    #region -- SHOP PICK UPS --
    [HideFromItemCatalog]
    public class PickUpModel_RMR_BigBrothersChains : ShopPickUpModel
    {
        public PickUpModel_RMR_BigBrothersChains() : base()
        {
            this.id = new LorId(LogLikeMod.ModId, 90047);
            this.rewardinfo = RewardPassivesList.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 90047));
        }
        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_BigBrotherChains());
        }
        public override bool IsCanAddShop()
        {
            return !LogueBookModels.shopPick.Contains(this.id);
        }
        public override string KeywordIconId => "RMR_BigBrothersChains";

        public override string KeywordId => "RMR_BigBrothersChains";

    }

    [HideFromItemCatalog]
    public class PickUpModel_RMR_Crowbar : ShopPickUpModel
    {
        public PickUpModel_RMR_Crowbar() : base()
        {
            this.id = new LorId(LogLikeMod.ModId, 90048);
            this.rewardinfo = RewardPassivesList.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 90048));            
        }
        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_Crowbar());
        }
        public override bool IsCanAddShop()
        {
            return !LogueBookModels.shopPick.Contains(this.id);
        }

        public override string KeywordIconId => "RMR_Crowbar";

        public override string KeywordId => "RMR_Crowbar";

    }

    [HideFromItemCatalog]
    public class PickUpModel_RMR_HarvestScythe : ShopPickUpModel
    {
        public PickUpModel_RMR_HarvestScythe() : base()
        {
            this.id = new LorId(LogLikeMod.ModId, 90049);
            this.rewardinfo = RewardPassivesList.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 90049));
        }
        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_HarvesterScythe());
        }

        public override bool IsCanAddShop()
        {
            return !LogueBookModels.shopPick.Contains(this.id);
        }

        public override string KeywordIconId => "RMR_HarvestScythe";

        public override string KeywordId => "RMR_HarvestScythe";

    }

    [HideFromItemCatalog]
    public class PickUpModel_RMR_Remote : ShopPickUpModel
    {
        public PickUpModel_RMR_Remote() : base()
        {
            this.id = new LorId(LogLikeMod.ModId, 90050);
            this.rewardinfo = RewardPassivesList.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 90050));
        }
        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_Remote());
        }

        public override string KeywordIconId => "RMR_Remote";

        public override string KeywordId => "RMR_Remote";

    }


    [HideFromItemCatalog]
    public class PickUpModel_RMR_BleedingSpleen : ShopPickUpModel
    {
        public PickUpModel_RMR_BleedingSpleen() : base()
        {
            this.id = new LorId(LogLikeMod.ModId, 90051);
            this.rewardinfo = RewardPassivesList.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 90051));
        }
        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_BleedingSpleen());
        }

        public override bool IsCanAddShop()
        {
            return !LogueBookModels.shopPick.Contains(this.id);
        }

        public override string KeywordIconId => "RMR_BleedingSpleen";

        public override string KeywordId => "RMR_BleedingSpleen";

    }


    [HideFromItemCatalog]
    public class PickUpModel_RMR_CorrodedChains : ShopPickUpModel
    {
        public PickUpModel_RMR_CorrodedChains() : base()
        {
            this.id = new LorId(LogLikeMod.ModId, 90052);
            this.rewardinfo = RewardPassivesList.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 90052));
        }
        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_CorrodedChains());
        }

        public override bool IsCanAddShop()
        {
            return !LogueBookModels.shopPick.Contains(this.id);
        }

        public override string KeywordIconId => "RMR_CorrodedChains";

        public override string KeywordId => "RMR_CorrodedChains";

    }


    public class PickUpModel_RMR_Polyhedra : ShopPickUpModel
    {
        public PickUpModel_RMR_Polyhedra() : base()
        {
            this.id = new LorId(LogLikeMod.ModId, 90069);
            this.rewardinfo = RewardPassivesList.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 90069));
        }
        public override void OnPickUpShop(ShopGoods good)
        {
            AddPassiveReward(this.id);
        }

        public override bool IsCanAddShop()
        {
            return LogueBookModels.shopPick.Contains(this.id);
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            LogueBookModels.AddPlayerStat(model.UnitData, new LogStatAdder()
            {
                maxhp = 1,
                maxbreak = 1,
                speedmax = 1,
                speedmin = 1,
                maxplaypoint = 1,
                startplaypoint = 1
            });
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_Hidden_Polyhedra() { unitIndex = LogueBookModels.GetIndexOfUnit(model) });
        }

        public override string KeywordIconId => "RMRPickUp_Polyhedra";

        public override string KeywordId => "RMRPickUp_Polyhedra";

        [HideFromItemCatalog]
        public class RMREffect_Hidden_Polyhedra : GlobalLogueEffectBase
        {
            public int unitIndex;

            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (behavior.owner.UnitData.unitData == LogueBookModels.playerModel[unitIndex])
                {
                    behavior.ApplyDiceStatBonus(new DiceStatBonus
                    {
                        dmg = 1,
                        breakDmg = 1,
                        guardBreakAdder = 1,
                        min = 1,
                        max = 1
                    });
                }
            }

            public override SaveData GetSaveData()
            {
                SaveData data = base.GetSaveData();
                data.AddData("savedUnit", unitIndex);
                return data;
            }

            public override void LoadFromSaveData(SaveData save)
            {
                base.LoadFromSaveData(save);
                this.unitIndex = save.GetInt("savedUnit");
            }
        }
    }


    [HideFromItemCatalog]
    public class PickUpModel_RMR_WelltunedWeapons : ShopPickUpModel
    {
        public PickUpModel_RMR_WelltunedWeapons() : base()
        {
            this.id = new LorId(LogLikeMod.ModId, 90053);
            this.rewardinfo = RewardPassivesList.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 90053));
        }
        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_WelltunedWeapons());
        }

        public override bool IsCanAddShop()
        {
            return !LogueBookModels.shopPick.Contains(this.id);
        }
        public override Rarity ItemRarity => Rarity.Uncommon;
        public override string KeywordIconId => "RMR_WelltunedWeapons";

        public override string KeywordId => "RMR_WelltunedWeapons";

    }

    [HideFromItemCatalog]
    public class PickUpModel_RMR_GamblerEye : ShopPickUpModel
    {
        public PickUpModel_RMR_GamblerEye() : base()
        {
            this.id = new LorId(LogLikeMod.ModId, 90054);
            this.rewardinfo = RewardPassivesList.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 90054));
        }
        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_GamblerEye());
        }

        public override bool IsCanAddShop()
        {
            return !LogueBookModels.shopPick.Contains(this.id);
        }

        public override Rarity ItemRarity => Rarity.Unique;
        public override string KeywordIconId => "RMR_GamblerEye";

        public override string KeywordId => "RMR_GamblerEye";

    }

    [HideFromItemCatalog]
    public class PickUpModel_RMR_OrdinaryClothes : ShopPickUpModel
    {
        public PickUpModel_RMR_OrdinaryClothes() : base()
        {
            this.id = new LorId(LogLikeMod.ModId, 90055);
            this.rewardinfo = RewardPassivesList.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 90055));
        }
        public override void OnPickUpShop(ShopGoods good)
        {
            Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RMREffect_OrdinaryClothes());
        }

        public override bool IsCanAddShop()
        {
            return !LogueBookModels.shopPick.Contains(this.id);
        }

        public override Rarity ItemRarity => Rarity.Uncommon;
        public override string KeywordIconId => "RMR_OrdinaryClothes";

        public override string KeywordId => "RMR_OrdinaryClothes";

    }



    #endregion

    #region -- STAGE PICK UPS --
    [HideFromItemCatalog]
    public class PickUpModel_RMR_CopleyConsequences : PickUpModelBase
    {
        public override void LoadFromSaveData(LogueStageInfo stage) => stage.type = abcdcode_LOGLIKE_MOD.StageType.Mystery;

        public PickUpModel_RMR_CopleyConsequences()
        {
            this.Name = TextDataModel.GetText("Stage_Mystery");
            this.Desc = TextDataModel.GetText("Stage_Mystery_Desc");
            this.FlaverText = "";
            this.ArtWork = "Stage_Mystery";
        }
    }
    #endregion
}
