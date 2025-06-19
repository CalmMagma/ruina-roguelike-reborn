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
    /* uncomment this if its fine
    public class UpgradeModel_RMR_MeatJam : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_MeatJamUpgrade");
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 2000002);
        }
    } */


    public class UpgradeModel_RMR_LawOrder : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetAbility(0, "RMR_LawOrderUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 301011);
        }
    }

    public class UpgradeModel_RMR_Diversion : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 300001);
        }
    }

    public class UpgradeModel_RMR_SharpSwipe : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetAbility(0, "bleeding2atk");
            this.baseid = new LorId(LogLikeMod.ModId, 300002);
        }
    }

    public class UpgradeModel_RMR_Standoff : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_StandoffUpgrade");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 300004);
        }
    }

    public class UpgradeModel_RMR_Avert : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 301009);
        }
    }

    public class UpgradeModel_RMR_ArtOfSteps : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("quickness2");
            this.upgradeinfo.SetDice(0, 1, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 302006);
        }
    }

    public class UpgradeModel_RMR_StartinLightly : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.ChangeDiceType(1, BehaviourDetail.Hit);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 303004);
        }
    }

    public class UpgradeModel_RMR_Guardian : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_GuardianUpgrade");
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 301007);
        } 
    }
    public class UpgradeModel_RMR_Gamble : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_GambleUpgrade");
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 302003);
        }
    }

    public class UpgradeModel_RMR_BackAttack : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 300003);
        }
    }

    public class UpgradeModel_RMR_Slice : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_SliceUpgrade");
            this.upgradeinfo.ChangeDiceType(2, BehaviourDetail.Slash);
            this.baseid = new LorId(LogLikeMod.ModId, 302007);
        }
    }

    public class UpgradeModel_RMR_SilentNight : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetDice(0, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 301002);
        }
    }

    public class UpgradeModel_RMR_StayClam : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_StayClamUpgrade");
            this.baseid = new LorId(LogLikeMod.ModId, 302005);
        }
    }

    public class UpgradeModel_RMR_Fence : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.baseid = new LorId(LogLikeMod.ModId, 301003);
        }

        public override DiceCardXmlInfo GetUpgradeInfo(int index, int count)
        {
            this.upxmlinfo = base.GetUpgradeInfo(index, count);

            // the way I'm doing this is probably stupid and could be using Sort() or some other function but I cannot come up with a solution and I will bruteforce it instead - Lux
            this.upxmlinfo.DiceBehaviourList.Clear();
            DiceBehaviour die = new DiceBehaviour
            {

                Min = 3,
                Dice = 6,
                Detail = BehaviourDetail.Evasion,
                Type = BehaviourType.Def,
                MotionDetail = MotionDetail.E,
                MotionDetailDefault = MotionDetail.N,
                Script = ""
            };
            DiceBehaviour die2 = new DiceBehaviour
            {

                Min = 4,
                Dice = 5,
                Detail = BehaviourDetail.Guard,
                Type = BehaviourType.Def,
                MotionDetail = MotionDetail.G,
                MotionDetailDefault = MotionDetail.N,
                Script = ""
            };
            
            DiceBehaviour die3 = new DiceBehaviour
            {

                Min = 1,
                Dice = 6,
                Detail = BehaviourDetail.Slash,
                Type = BehaviourType.Atk,
                MotionDetail = MotionDetail.J,
                MotionDetailDefault = MotionDetail.N,
                Script = ""
            };
            this.upxmlinfo.DiceBehaviourList.Add(die);
            this.upxmlinfo.DiceBehaviourList.Add(die2);
            this.upxmlinfo.DiceBehaviourList.Add(die3);
            
            return base.GetUpgradeInfo(index, count);
        }
    }

    public class UpgradeModel_RMR_FishOnslaught : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_FishOnslaughtUpgrade");
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 302001);
        }
    }

    public class UpgradeModel_RMR_DestructiveImpact : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 5);
            this.baseid = new LorId(LogLikeMod.ModId, 303006);
        }
    }

    public class UpgradeModel_RMR_SearingBlow : UpgradeBase
    {
        public override bool CanRepeatUpgrade() => true;
        public override void Init()
        {
            base.Init();
            // uhh idk if upgradebase persists between upgrades so Idk if I can make it increment max increase by 1
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 303005);
        }
    }

    public class UpgradeModel_RMR_Outburst : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetAbility(0, "RMR_allydraw1pw");
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 300005);
        }
    }

    public class UpgradeModel_RMR_Crush : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetDice(0, 0, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 303008);
        }
    }

    public class UpgradeModel_RMR_EndYou : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetAbility(0, "RMR_paralysisbind1atk");
            this.upgradeinfo.SetAbility(1, "RMR_paralysisbind1atk");
            this.baseid = new LorId(LogLikeMod.ModId, 303003);
        }
    }

    public class UpgradeModel_RMR_CutIn : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 0, 3);
            this.upgradeinfo.SetDice(2, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 303007);
        }
    }

    public class UpgradeModel_RMR_YourShield : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 301004);
        }
    }

    public class UpgradeModel_RMR_BladeWhirl : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_BladeWhirlUpgrade");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 301006);
        }
    }

    public class UpgradeModel_RMR_HandlingWork : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("RMR_HandlingWorkUpgrade");
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 301001);
        }
    }

    public class UpgradeModel_RMR_Retaliate : UpgradeBase
    {
        public override void Init()
        {
            base.Init();
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 301010);
        }
    }
}
