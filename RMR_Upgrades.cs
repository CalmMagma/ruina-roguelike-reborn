using System;
using System.Collections.Generic;
using System.Linq;
using abcdcode_LOGLIKE_MOD;
using LOR_DiceSystem;

namespace RogueLike_Mod_Reborn
{
    public class UpgradeModel_RMR_Track : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_TrackUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 104005);
        }
    }

    public class UpgradeModel_RMR_Chargeup : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_ChargeupUpgrade");
            this.upgradeinfo.SetAbility(0, "paralysis1atk");
            this.baseid = new LorId(LogLikeMod.ModId, 103003);
        }
    }

    public class UpgradeModel_RMR_BackstreetsDash : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_BackstreetsDashDie");
            this.upgradeinfo.SetDice(0, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 101005);
        }
    }

    public class UpgradeModel_RMR_SkitterAway : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_SkitterAwayUpgrade");
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 101006);
        }
    }

    public class UpgradeModel_RMR_Endure : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_EndureUpgrade");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 103004);
        }
    }

    public class UpgradeModel_RMR_BlowItUp : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 3, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 103001);
        }
    }

    public class UpgradeModel_RMR_YoureTooSlow : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 102002);
        }
    }

    public class UpgradeModel_RMR_Driedup : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_DriedupUpgrade");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 103005);
        }
    }

    public class UpgradeModel_RMR_FendThisOff : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 104003);
        }
    }

    public class UpgradeModel_RMR_TimeForTest : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 3, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 102004);
        }
    }

    public class UpgradeModel_RMR_ChopItOff : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 103006);
        }
    }

    public class UpgradeModel_RMR_GoinFirst : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.upgradeinfo.SetDice(2, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 104002);
        }
    }

    public class UpgradeModel_RMR_Struggle : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_StruggleUpgrade");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 102003);
        }
    }

    public class UpgradeModel_RMR_YouOnlyLiveOnce : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 3, 3);
            this.baseid = new LorId(LogLikeMod.ModId, 103002);
        }
    }

    public class UpgradeModel_RMR_GutHarvesting : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_GutHarvestingUpgrade");
            this.upgradeinfo.SetAbility(0, "bleeding2atk");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 101001);
        }
    }

    public class UpgradeModel_RMR_Rampage : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 104004);
        }
    }

    public class UpgradeModel_RMR_GatherIntel : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_GatherIntelUpgrade");
            this.upgradeinfo.SetDice(0, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 202009);
        }
    }

    public class UpgradeModel_RMR_Appetite : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "bleeding2atk");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 201007);
        }
    }

    public class UpgradeModel_RMR_TrimIngredients : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(1, "bleeding2pw");
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 201002);
        }
    }

    public class UpgradeModel_RMR_PreparedMindLulu : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_PreparedMindLuluUpgrade");
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetAbility(1, "burn2atk");
            this.baseid = new LorId(LogLikeMod.ModId, 202012);
        }
    }

    public class UpgradeModel_RMR_Cruelty : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_CrueltyUpgradeDie");
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 201006);
        }
    }

    public class UpgradeModel_RMR_PreparedMindMars : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_Endurance2start");
            this.upgradeinfo.SetAbility(0, "endurance2pw");
            this.upgradeinfo.SetDice(0, 2, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 202002);
        }
    }

    public class UpgradeModel_RMR_KeepItFresh : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "bleeding2atk");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 201001);
        }
    }

    public class UpgradeModel_RMR_Multiblock : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_MultiblockUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 200002);
        }
    }

    public class UpgradeModel_RMR_FleetFootsteps : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 202008);
        }
    }

    public class UpgradeModel_RMR_Deflect : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 202001);
        }
    }

    public class UpgradeModel_RMR_IngredientHunt : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetAbility(1, "bleeding2atk");
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 201005);
        }
    }

    public class UpgradeModel_RMR_CookAnything : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetAbility(1, "recoverHp5atkLog");
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 201003);
        }
    }

    public class UpgradeModel_RMR_SetFire : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "RMR_SetFireUpgradeDie");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetAbility(1, "RMR_SetFireUpgradeDie");
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 202003);
        }
    }

    public class UpgradeModel_RMR_Multihit : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 2, 1);
            this.upgradeinfo.SetDice(2, 2, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 200003);
        }
    }

    public class UpgradeModel_RMR_StrongStrike : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "recoverBreak3atk");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetAbility(1, "recoverBreak3atk");
            this.baseid = new LorId(LogLikeMod.ModId, 200005);
        }
    }

    public class UpgradeModel_RMR_NonstopAssault : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_NonstopAssaultUpgrade");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 202007);
        }
    }

}
