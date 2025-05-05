using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using abcdcode_LOGLIKE_MOD;

namespace RogueLike_Mod_Reborn
{
    [HideFromItemCatalog]
    public class PickUpModel_RMR_BigBrothersChains : ShopPickUpRebornModel
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

        public override string KeywordIconId => "RMR_BigBrothersChains";

        public override string KeywordId => "RMR_BigBrothersChains";

    }

    [HideFromItemCatalog]
    public class PickUpModel_RMR_Crowbar : ShopPickUpRebornModel
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

        public override string KeywordIconId => "RMR_Crowbar";

        public override string KeywordId => "RMR_Crowbar";

    }

    [HideFromItemCatalog]
    public class PickUpModel_RMR_HarvestScythe : ShopPickUpRebornModel
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

        public override string KeywordIconId => "RMR_HarvestScythe";

        public override string KeywordId => "RMR_HarvestScythe";

    }

    [HideFromItemCatalog]
    public class PickUpModel_RMR_Remote : ShopPickUpRebornModel
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

        public override string KeywordIconId => "RMR_HarvestScythe";

        public override string KeywordId => "RMR_HarvestScythe";

    }
}
