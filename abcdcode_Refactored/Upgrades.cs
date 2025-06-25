using LOR_DiceSystem;

namespace abcdcode_LOGLIKE_MOD
{
    public class UpgradeModel_awl1_1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 612002);
        }
    }


    public class UpgradeModel_awl1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 612001);
        }
    }


    public class UpgradeModel_awl3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 612003);
        }
    }


    public class UpgradeModel_awl4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 0, 2);
            this.upgradeinfo.SetAbility(2, "binding2atk");
            this.baseid = new LorId(LogLikeMod.ModId, 612004);
        }
    }


    public class UpgradeModel_awl5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 3);
            this.upgradeinfo.SetAbility(1, "binding2pwLog");
            this.baseid = new LorId(LogLikeMod.ModId, 612005);
        }
    }


    public class UpgradeModel_awl6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetSelfAbility("awlofnight1Log");
            this.baseid = new LorId(LogLikeMod.ModId, 612006);
        }
    }


    public class UpgradeModel_bayyard1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 616001);
        }
    }


    public class UpgradeModel_bayyard2_1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.upgradeinfo.SetSelfAbility("DontMoveDrawLog");
            this.baseid = new LorId(LogLikeMod.ModId, 616005);
        }
    }


    public class UpgradeModel_bayyard2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 616002);
        }
    }


    public class UpgradeModel_bayyard3_1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 0, 2);
            this.upgradeinfo.SetSelfAbility("DontMoveDrawLog");
            this.baseid = new LorId(LogLikeMod.ModId, 616004);
        }
    }


    public class UpgradeModel_bayyard3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 616003);
        }
    }


    public class UpgradeModel_bayyard4_1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 3, 6);
            this.baseid = new LorId(LogLikeMod.ModId, 616007);
        }
    }


    public class UpgradeModel_bayyard4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("selfstunwithabilLog");
            this.baseid = new LorId(LogLikeMod.ModId, 616006);
        }
    }





    // Hard Rehearsal
    public class UpgradeModel_Bremen3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 407003);
        }
    }

    // Tendon Chords
    public class UpgradeModel_Bremen6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 407006);
        }
    }



    public class UpgradeModel_Cane1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 4);
            this.baseid = new LorId(LogLikeMod.ModId, 603001);
        }
    }


    public class UpgradeModel_Cane2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetSelfAbility("warpCharge7Log");
            this.baseid = new LorId(LogLikeMod.ModId, 603002);
        }
    }


    public class UpgradeModel_Cane3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 3);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.SetSelfAbility("warpCharge6");
            this.baseid = new LorId(LogLikeMod.ModId, 603003);
        }
    }


    public class UpgradeModel_Cane4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.upgradeinfo.SetSelfAbility("badaCardRemakeLog");
            this.baseid = new LorId(LogLikeMod.ModId, 603004);
        }
    }


    public class UpgradeModel_Cane5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 603005);
        }
    }


    public class UpgradeModel_Cane6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.upgradeinfo.SetSelfAbility("warpCharge3");
            this.baseid = new LorId(LogLikeMod.ModId, 603006);
        }
    }


    public class UpgradeModel_Cane7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetSelfAbility("chargeEnergyLog");
            this.baseid = new LorId(LogLikeMod.ModId, 603007);
        }
    }


    public class UpgradeModel_Cane8 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 3);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetAbility(0, "breakChargeRemakeLog");
            this.baseid = new LorId(LogLikeMod.ModId, 603008);
        }
    }


    public class UpgradeModel_ch1_1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetAbility(0, "RMR_RecycleOnClashLose");
            this.upgradeinfo.SetDice(1, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 100001);
        }
    }

    public class UpgradeModel_ch1_4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("powerup2highspeed");
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 100004);
        }
    }


    public class UpgradeModel_ch1_5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetSelfAbility("quickness2");
            this.baseid = new LorId(LogLikeMod.ModId, 100005);
        }
    }


    public class UpgradeModel_ch2_1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 3, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 200001);
        }
    }


    public class UpgradeModel_ch2_4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 200004);
        }
    }










    public class UpgradeModel_Circus1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 503001);
        }
    }


    public class UpgradeModel_Circus2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 503002);
        }
    }


    public class UpgradeModel_Circus3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 3);
            this.baseid = new LorId(LogLikeMod.ModId, 503003);
        }

        public override DiceCardXmlInfo GetUpgradeInfo(int index, int count)
        {
            this.upxmlinfo = base.GetUpgradeInfo(index, count);
            this.upxmlinfo.optionList.Remove(CardOption.ExhaustOnUse);
            return this.upxmlinfo;
        }
    }


    public class UpgradeModel_Circus4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 503004);
        }

        public override DiceCardXmlInfo GetUpgradeInfo(int index, int count)
        {
            this.upxmlinfo = base.GetUpgradeInfo(index, count);
            this.upxmlinfo.optionList.Remove(CardOption.ExhaustOnUse);
            return this.upxmlinfo;
        }
    }

    public class UpgradeModel_Gear1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetAbility(0, "smoke4atk");
            this.baseid = new LorId(LogLikeMod.ModId, 604001);
        }
    }


    public class UpgradeModel_Gear2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.upgradeinfo.SetSelfAbility("smokeSelf5Log");
            this.baseid = new LorId(LogLikeMod.ModId, 604002);
        }
    }


    public class UpgradeModel_Gear3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetSelfAbility("smokeSelf3");
            this.upgradeinfo.SetAbility(0, "smoke3atk");
            this.baseid = new LorId(LogLikeMod.ModId, 604003);
        }
    }


    public class UpgradeModel_Gear4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.SetSelfAbility("smokePowerUpLog");
            this.baseid = new LorId(LogLikeMod.ModId, 604004);
        }
    }


    public class UpgradeModel_Gear5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetSelfAbility("energy2Smoke3Log");
            this.baseid = new LorId(LogLikeMod.ModId, 604005);
        }
    }

    //Overpower
    public class UpgradeModel_Hook2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 104001);
        }
    }

    //Mutilate
    public class UpgradeModel_Hook4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.AddDice(1, 5, BehaviourDetail.Slash, "recoverHp1atk", BehaviourType.Atk);
            this.baseid = new LorId(LogLikeMod.ModId, 104006);
        }
    }


    public class UpgradeModel_IndexSecond1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 3, 2);
            this.upgradeinfo.SetAbility(0, "gloriaDice");
            this.baseid = new LorId(LogLikeMod.ModId, 605001);
        }
    }


    public class UpgradeModel_IndexSecond10 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.AddDice(1, 4, BehaviourDetail.Hit, "", MotionDetail.H, BehaviourType.Atk, "IndexUnit_H", "");
            this.baseid = new LorId(LogLikeMod.ModId, 605010);
        }
    }


    public class UpgradeModel_IndexSecond2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetSelfAbility("estherCardLog");
            this.upgradeinfo.SetAbility(0, "estherDiceLog");
            this.baseid = new LorId(LogLikeMod.ModId, 605002);
        }
    }


    public class UpgradeModel_IndexSecond3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 3);
            this.upgradeinfo.SetAbility(0, "hubertDiceLog");
            this.baseid = new LorId(LogLikeMod.ModId, 605003);
        }
    }


    public class UpgradeModel_IndexSecond4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.upgradeinfo.SetAbility(1, "powerUpDice6highlanderLog");
            this.baseid = new LorId(LogLikeMod.ModId, 605004);
        }
    }


    public class UpgradeModel_IndexSecond5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetAbility(0, "powerUp2targetHp50Log");
            this.upgradeinfo.SetAbility(1, "powerUp2targetHp50Log");
            this.baseid = new LorId(LogLikeMod.ModId, 605005);
        }
    }


    public class UpgradeModel_IndexSecond6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetSelfAbility("cardPowerDown2targetHighlanderLog");
            this.baseid = new LorId(LogLikeMod.ModId, 605006);
        }
    }


    public class UpgradeModel_IndexSecond7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 2, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 605007);
        }
    }


    public class UpgradeModel_IndexSecond8 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetSelfAbility("indexRelease1Log");
            this.baseid = new LorId(LogLikeMod.ModId, 605008);
        }
    }


    public class UpgradeModel_IndexSecond9 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetSelfAbility("indexRelease2Log");
            this.baseid = new LorId(LogLikeMod.ModId, 605009);
        }
    }












    public class UpgradeModel_Liu1_1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 606001);
        }
    }


    public class UpgradeModel_Liu1_2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 2, 2);
            this.upgradeinfo.SetAbility(1, "burn8atkLog");
            this.baseid = new LorId(LogLikeMod.ModId, 606002);
        }
    }


    public class UpgradeModel_Liu1_3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 606003);
        }
    }


    public class UpgradeModel_Liu1_4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetAbility(0, "burn1pwLog");
            this.baseid = new LorId(LogLikeMod.ModId, 606004);
        }
    }


    public class UpgradeModel_Liu1_5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetAbility(1, "burn1atk");
            this.baseid = new LorId(LogLikeMod.ModId, 606005);
        }
    }


    public class UpgradeModel_Liu1_6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetAbility(0, "burn2atk");
            this.baseid = new LorId(LogLikeMod.ModId, 606006);
        }
    }


    public class UpgradeModel_Liu1_7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetAbility(0, "burn2atk");
            this.baseid = new LorId(LogLikeMod.ModId, 606007);
        }
    }


    public class UpgradeModel_Liu2_1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.SetDice(3, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 601001);
        }
    }


    public class UpgradeModel_Liu2_10 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 601010);
        }
    }


    public class UpgradeModel_Liu2_11 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 601011);
        }
    }


    public class UpgradeModel_Liu2_12 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 601012);
        }
    }


    public class UpgradeModel_Liu2_13 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetSelfAbility("gainPositive2Log");
            this.baseid = new LorId(LogLikeMod.ModId, 601013);
        }
    }


    public class UpgradeModel_Liu2_2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetSelfAbility("gainPositive2Log");
            this.baseid = new LorId(LogLikeMod.ModId, 601002);
        }
    }


    public class UpgradeModel_Liu2_3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 601003);
        }
    }


    public class UpgradeModel_Liu2_4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 0, 3);
            this.upgradeinfo.SetDice(2, 0, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 601004);
        }
    }


    public class UpgradeModel_Liu2_5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetAbility(1, "quickness2atkLog");
            this.baseid = new LorId(LogLikeMod.ModId, 601005);
        }
    }


    public class UpgradeModel_Liu2_6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 601006);
        }
    }


    public class UpgradeModel_Liu2_7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 2, 2);
            this.upgradeinfo.SetAbility(0, "burn1atk");
            this.baseid = new LorId(LogLikeMod.ModId, 601007);
        }
    }


    public class UpgradeModel_Liu2_8_1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 601014);
        }
    }


    public class UpgradeModel_Liu2_8 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetSelfAbility("lowelonlyLog");
            this.baseid = new LorId(LogLikeMod.ModId, 601008);
        }
    }


    public class UpgradeModel_Liu2_9 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 2);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.SetAbility(1, "liuBurnDiceLog");
            this.baseid = new LorId(LogLikeMod.ModId, 601009);
        }
    }


    public class UpgradeModel_LogicSpecial : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(0, 2, 1);
            this.upgradeinfo.SetDice(0, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 706108);
        }
    }


    public class UpgradeModel_mirae1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 614001);
        }
    }


    public class UpgradeModel_mirae2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetSelfAbility("plutoCardLog");
            this.baseid = new LorId(LogLikeMod.ModId, 614002);
        }
    }


    public class UpgradeModel_mirae3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 614003);
        }
    }


    public class UpgradeModel_mirae4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.upgradeinfo.SetSelfAbility("discardAllEnergy1Log");
            this.baseid = new LorId(LogLikeMod.ModId, 614004);
        }
    }


    public class UpgradeModel_mirae5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(1, 0, 2);
            this.upgradeinfo.SetSelfAbility("handCountpowerUpLog");
            this.baseid = new LorId(LogLikeMod.ModId, 614005);
        }
    }


    public class UpgradeModel_mirae6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetAbility(0, "lifeInsuranceAreaAtkLog");
            this.baseid = new LorId(LogLikeMod.ModId, 614006);
        }
    }








    public class UpgradeModel_Molar5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 302002);
        }
    }

    public class UpgradeModel_Molar7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 2, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 302004);
        }
    }


    public class UpgradeModel_Pierr7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetAbility(0, "paralysis1pw");
            this.baseid = new LorId(LogLikeMod.ModId, 201004);
        }
    }


    public class UpgradeModel_Puppet1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.upgradeinfo.SetDice(3, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 502001);
        }
    }


    public class UpgradeModel_Puppet2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 502002);
        }
    }


    public class UpgradeModel_Puppet3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.SetDice(3, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 502003);
        }
    }


    public class UpgradeModel_Puppet4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 3);
            this.baseid = new LorId(LogLikeMod.ModId, 502004);
        }
    }


    public class UpgradeModel_Puppet5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 502005);
        }
    }


    public class UpgradeModel_Puppet6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 502006);
        }
    }


    public class UpgradeModel_Puppet7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 502007);
        }
    }


    public class UpgradeModel_Purple1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 609001);
        }
    }


    public class UpgradeModel_Purple10 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.AddDice(4, 8, BehaviourDetail.Guard, "", MotionDetail.G, BehaviourType.Standby, "ThePurpleTear", "");
            this.baseid = new LorId(LogLikeMod.ModId, 609010);
        }
    }


    public class UpgradeModel_Purple11 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetAbility(1, "recoverBreak3atk");
            this.baseid = new LorId(LogLikeMod.ModId, 609011);
        }
    }


    public class UpgradeModel_Purple12 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.AddDice(4, 9, BehaviourDetail.Slash, "", MotionDetail.H, BehaviourType.Atk, "ThePurpleTear_H", "");
            this.baseid = new LorId(LogLikeMod.ModId, 609012);
        }
    }


    public class UpgradeModel_Purple13 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetAbility(0, "purpleAreaDice1Log");
            this.upgradeinfo.SetAbility(1, "purpleAreaDice2Log");
            this.upgradeinfo.SetAbility(2, "purpleAreaDice3Log");
            this.baseid = new LorId(LogLikeMod.ModId, 609013);
        }
    }


    public class UpgradeModel_Purple2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 5);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 609002);
        }
    }


    public class UpgradeModel_Purple3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 609003);
        }
    }


    public class UpgradeModel_Purple4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetAbility(0, "bleeding1atk");
            this.baseid = new LorId(LogLikeMod.ModId, 609004);
        }
    }


    public class UpgradeModel_Purple5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.SetDice(3, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 609005);
        }
    }


    public class UpgradeModel_Purple6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 609006);
        }
    }


    public class UpgradeModel_Purple7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetAbility(0, "recoverBreak5atk");
            this.baseid = new LorId(LogLikeMod.ModId, 609007);
        }
    }


    public class UpgradeModel_Purple8 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetAbility(1, "recoverBreak5atk");
            this.baseid = new LorId(LogLikeMod.ModId, 609008);
        }
    }


    public class UpgradeModel_Purple9 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.SetDice(3, 1, 1);
            this.upgradeinfo.SetAbility(0, "powerDownNext3pwTargetLog");
            this.upgradeinfo.SetAbility(1, "powerDownNext3pwTargetLog");
            this.upgradeinfo.SetAbility(2, "powerDownNext3pwTargetLog");
            this.baseid = new LorId(LogLikeMod.ModId, 609009);
        }
    }


    public class UpgradeModel_Rats1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetAbility(1, "bleeding1atk");
            this.baseid = new LorId(LogLikeMod.ModId, 101004);
        }
    }


    public class UpgradeModel_Rats2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("rhinoCard");
            this.upgradeinfo.SetAbility(1, "bleeding2atk");
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 101002);
        }
    }


    public class UpgradeModel_Rats3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetAbility(0, "bleeding2atk");
            this.baseid = new LorId(LogLikeMod.ModId, 101003);
        }
    }


    public class UpgradeModel_Rcorp1_1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 608017);
        }
    }


    public class UpgradeModel_Rcorp1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 608001);
        }
    }


    public class UpgradeModel_Rcorp10 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetSelfAbility("deerCardLog");
            this.upgradeinfo.SetAbility(2, "deerDiceLog");
            this.baseid = new LorId(LogLikeMod.ModId, 608010);
        }
    }


    public class UpgradeModel_Rcorp11 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 2, 2);
            this.upgradeinfo.SetSelfAbility("warpCharge6");
            this.baseid = new LorId(LogLikeMod.ModId, 608011);
        }
    }


    public class UpgradeModel_Rcorp12 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 608012);
        }
    }


    public class UpgradeModel_Rcorp13 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.upgradeinfo.SetSelfAbility("warpCharge5");
            this.baseid = new LorId(LogLikeMod.ModId, 608013);
        }
    }


    public class UpgradeModel_Rcorp14 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetSelfAbility("energyAndChargeLog");
            this.baseid = new LorId(LogLikeMod.ModId, 608014);
        }
    }


    public class UpgradeModel_Rcorp15 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetSelfAbility("warpCharge9Log");
            this.baseid = new LorId(LogLikeMod.ModId, 608015);
        }
    }


    public class UpgradeModel_Rcorp16 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 608016);
        }
    }


    public class UpgradeModel_Rcorp2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.SetAbility(0, "rudolphDice");
            this.upgradeinfo.SetAbility(1, "rudolphDice");
            this.upgradeinfo.SetAbility(2, "rudolphDice");
            this.baseid = new LorId(LogLikeMod.ModId, 608002);
        }
    }


    public class UpgradeModel_Rcorp3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 5);
            this.baseid = new LorId(LogLikeMod.ModId, 608003);
        }
    }


    public class UpgradeModel_Rcorp4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 5, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 608004);
        }
    }


    public class UpgradeModel_Rcorp5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetSelfAbility("deerCardLog");
            this.upgradeinfo.SetAbility(0, "deerDiceLog");
            this.baseid = new LorId(LogLikeMod.ModId, 608005);
        }
    }


    public class UpgradeModel_Rcorp6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 608006);
        }
    }


    public class UpgradeModel_Rcorp7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetSelfAbility("powerUp1speed5exhaustLog");
            this.baseid = new LorId(LogLikeMod.ModId, 608007);
        }
    }


    public class UpgradeModel_Rcorp8 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetSelfAbility("powerUp3speed5exhaustLog");
            this.baseid = new LorId(LogLikeMod.ModId, 608008);
        }
    }


    public class UpgradeModel_Rcorp9 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetSelfAbility("otherAlly3strength1thisRoundLog");
            this.baseid = new LorId(LogLikeMod.ModId, 608009);
        }
    }


    public class UpgradeModel_RedMist1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 8);
            this.baseid = new LorId(LogLikeMod.ModId, 607001);
        }
    }


    public class UpgradeModel_RedMist2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 5, 6);
            this.baseid = new LorId(LogLikeMod.ModId, 607002);
        }
    }


    public class UpgradeModel_RedMist3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 607003);
        }
    }


    public class UpgradeModel_RedMist4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 607004);
        }
    }


    public class UpgradeModel_RedMist5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 607005);
        }
    }


    public class UpgradeModel_RedMist6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 607006);
        }
    }


    public class UpgradeModel_RedMist7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 6);
            this.baseid = new LorId(LogLikeMod.ModId, 607007);
        }
    }








    public class UpgradeModel_Seven_Engagement : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetSelfAbility("Seven_EngagementLog");
            this.baseid = new LorId(LogLikeMod.ModId, 511001);
        }
    }


    public class UpgradeModel_Seven_EnGarde : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetSelfAbility("Seven_EnGardeLog");
            this.baseid = new LorId(LogLikeMod.ModId, 511005);
        }
    }


    public class UpgradeModel_Seven_Fleche : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetAbility(0, "Seven_FlecheLog");
            this.baseid = new LorId(LogLikeMod.ModId, 511003);
        }
    }


    public class UpgradeModel_Seven_Moulinet : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 511004);
        }
    }


    public class UpgradeModel_Seven_Riposte : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetAbility(0, "powerUpNext2pw");
            this.baseid = new LorId(LogLikeMod.ModId, 511002);
        }
    }


    public class UpgradeModel_Shi1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 501001);
        }
    }


    public class UpgradeModel_Shi2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 3);
            this.upgradeinfo.SetAbility(0, "disarm2atk");
            this.baseid = new LorId(LogLikeMod.ModId, 501002);
        }
    }


    public class UpgradeModel_Shi3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetSelfAbility("powerUp2speed5Log");
            this.baseid = new LorId(LogLikeMod.ModId, 501003);
        }
    }


    public class UpgradeModel_Shi4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 501004);
        }
    }


    public class UpgradeModel_Shi5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 501005);
        }
    }


    public class UpgradeModel_Shi6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 501006);
        }
    }


    public class UpgradeModel_Shi7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetAbility(0, "energy2atkAndHpLog");
            this.baseid = new LorId(LogLikeMod.ModId, 501007);
        }
    }


    public class UpgradeModel_Shyao1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 610001);
        }
    }


    public class UpgradeModel_Shyao10 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.SetDice(3, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 610010);
        }
    }


    public class UpgradeModel_Shyao11 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 0, 2);
            this.upgradeinfo.SetSelfAbility("allyBufLog");
            this.baseid = new LorId(LogLikeMod.ModId, 610011);
        }
    }


    public class UpgradeModel_Shyao2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 610002);
        }
    }


    public class UpgradeModel_Shyao3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetAbility(0, "burn1pwLog");
            this.baseid = new LorId(LogLikeMod.ModId, 610003);
        }
    }


    public class UpgradeModel_Shyao4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 610004);
        }
    }


    public class UpgradeModel_Shyao5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 610005);
        }
    }


    public class UpgradeModel_Shyao6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 3, 5);
            this.baseid = new LorId(LogLikeMod.ModId, 610006);
        }
    }


    public class UpgradeModel_Shyao8 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.SetSelfAbility("shyaoSelfBurnLog");
            this.baseid = new LorId(LogLikeMod.ModId, 610008);
        }
    }


    public class UpgradeModel_Shyao9 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 5, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 610009);
        }
    }


    public class UpgradeModel_Smile1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 3, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetAbility(0, "smoke9atkLog");
            this.baseid = new LorId(LogLikeMod.ModId, 507001);
        }
    }


    public class UpgradeModel_Smile2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetAbility(1, "bleedingXsmokeLog");
            this.baseid = new LorId(LogLikeMod.ModId, 507002);
        }
    }


    public class UpgradeModel_Smile3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetSelfAbility("smokeSelf4");
            this.baseid = new LorId(LogLikeMod.ModId, 507003);
        }
    }


    public class UpgradeModel_Smile4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetAbility(0, "smoke2atk");
            this.baseid = new LorId(LogLikeMod.ModId, 507004);
        }
    }


    public class UpgradeModel_Smile5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 507005);
        }
    }


    public class UpgradeModel_Smile6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.upgradeinfo.SetSelfAbility("energySmokeLog");
            this.baseid = new LorId(LogLikeMod.ModId, 507006);
        }
    }


    public class UpgradeModel_Smile7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.upgradeinfo.SetAbility(1, "debufsSmokeLog");
            this.baseid = new LorId(LogLikeMod.ModId, 507007);
        }
    }


    public class UpgradeModel_StigmaBurn : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("Stigma8_0Up");
            this.baseid = new LorId(LogLikeMod.ModId, 8582008);
        }
    }





    public class UpgradeModel_Stray6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 303009);
        }
    }


    public class UpgradeModel_Stray7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetDice(3, 1, 0);
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 303002);
        }
    }


    public class UpgradeModel_Stray8 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 303001);
        }
    }


    public class UpgradeModel_Stray9 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 303007);
        }
    }




    public class UpgradeModel_Street1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 202006);
        }
    }




    public class UpgradeModel_Street3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 202005);
        }
    }


    public class UpgradeModel_Street8 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(1, 3, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 202004);
        }
    }


    public class UpgradeModel_Sweepers1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 504001);
        }
    }


    public class UpgradeModel_Sweepers2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.SetDice(3, 1, 1);
            this.upgradeinfo.SetSelfAbility("sweeperOnlyLog");
            this.baseid = new LorId(LogLikeMod.ModId, 504002);
        }
    }


    public class UpgradeModel_Sweepers3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 504003);
        }
    }


    public class UpgradeModel_Sweepers4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetSelfAbility("ally3protection1thisRoundLog");
            this.baseid = new LorId(LogLikeMod.ModId, 504004);
        }
    }


    public class UpgradeModel_Sweepers5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 504005);
        }
    }


    public class UpgradeModel_Sweepers6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 504006);
        }
    }


    public class UpgradeModel_sword1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 512001);
        }
    }


    public class UpgradeModel_sword2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.ChangeDiceType(0, BehaviourDetail.Slash);
            this.baseid = new LorId(LogLikeMod.ModId, 512002);
        }
    }


    public class UpgradeModel_sword3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetAbility(0, "Sword_getbonecuttingcardplLog");
            this.baseid = new LorId(LogLikeMod.ModId, 512003);
        }
    }


    public class UpgradeModel_sword4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 512004);
        }
    }


    public class UpgradeModel_sword5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 512005);
        }
    }


    public class UpgradeModel_sword6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 7);
            this.upgradeinfo.SetAbility(0, "paralysis10bleeding5atkLog");
            this.baseid = new LorId(LogLikeMod.ModId, 512006);
        }
    }


    public class UpgradeModel_TheIndex1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetAbility(0, "destroyNextHighlander");
            this.baseid = new LorId(LogLikeMod.ModId, 505001);
        }
    }


    public class UpgradeModel_TheIndex2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 505002);
        }
    }


    public class UpgradeModel_TheIndex3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetAbility(0, "powerUpDice6highlanderLog");
            this.baseid = new LorId(LogLikeMod.ModId, 505003);
        }
    }


    public class UpgradeModel_TheIndex4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 505004);
        }
    }


    public class UpgradeModel_TheIndex5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 505005);
        }
    }


    public class UpgradeModel_TheIndex6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.SetSelfAbility("powerUp2fullCardLog");
            this.baseid = new LorId(LogLikeMod.ModId, 505006);
        }
    }


    public class UpgradeModel_TheIndex7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 505007);
        }
    }


    public class UpgradeModel_Thumb1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetSelfAbility("AddBullet1AndDrawLog");
            this.baseid = new LorId(LogLikeMod.ModId, 602001);
        }
    }


    public class UpgradeModel_Thumb10 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.upgradeinfo.SetDice(3, 0, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 602010);
        }
    }


    public class UpgradeModel_Thumb11 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.upgradeinfo.SetSelfAbility("dmgUp3AllLog");
            this.baseid = new LorId(LogLikeMod.ModId, 602011);
        }
    }


    public class UpgradeModel_Thumb12 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetAbility(0, "bleeding3atk");
            this.baseid = new LorId(LogLikeMod.ModId, 602012);
        }
    }


    public class UpgradeModel_Thumb2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetSelfAbility("AddBullet2Log");
            this.baseid = new LorId(LogLikeMod.ModId, 602002);
        }
    }


    public class UpgradeModel_Thumb20 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetSelfAbility("thumbBullet1Log");
            this.baseid = new LorId(LogLikeMod.ModId, 602020);
        }
    }


    public class UpgradeModel_Thumb21 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetSelfAbility("thumbBullet2Log");
            this.baseid = new LorId(LogLikeMod.ModId, 602021);
        }
    }


    public class UpgradeModel_Thumb22 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetSelfAbility("thumbBullet3Log");
            this.baseid = new LorId(LogLikeMod.ModId, 602022);
        }
    }


    public class UpgradeModel_Thumb3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 602003);
        }
    }


    public class UpgradeModel_Thumb4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 602004);
        }
    }


    public class UpgradeModel_Thumb6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 602006);
        }
    }


    public class UpgradeModel_Thumb7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 602007);
        }
    }


    public class UpgradeModel_Thumb8 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 602008);
        }
    }


    public class UpgradeModel_Thumb9 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 602009);
        }
    }


    public class UpgradeModel_usett1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 613001);
        }
    }


    public class UpgradeModel_usett2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 613002);
        }
    }


    public class UpgradeModel_usett3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 0);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 613003);
        }
    }


    public class UpgradeModel_usett4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 613004);
        }
    }


    public class UpgradeModel_usett5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 613005);
        }
    }


    public class UpgradeModel_usett6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 2, 0);
            this.upgradeinfo.SetDice(2, 2, 1);
            this.upgradeinfo.SetAbility(2, "usett_eyeLog");
            this.baseid = new LorId(LogLikeMod.ModId, 613006);
        }
    }


    public class UpgradeModel_Warp1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("warpSkillLog");
            this.baseid = new LorId(LogLikeMod.ModId, 506001);
        }
    }


    public class UpgradeModel_Warp2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.AddDice(2, 7, BehaviourDetail.Hit, "", MotionDetail.H, BehaviourType.Atk, "WarpCrew_H", "");
            this.baseid = new LorId(LogLikeMod.ModId, 506002);
        }
    }


    public class UpgradeModel_Warp3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(1, 2, 1);
            this.upgradeinfo.SetAbility(0, "powerUpNext4plLog");
            this.baseid = new LorId(LogLikeMod.ModId, 506003);
        }
    }


    public class UpgradeModel_Warp4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.upgradeinfo.SetDice(3, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 506004);
        }
    }


    public class UpgradeModel_Warp5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 0, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 2, 0);
            this.upgradeinfo.SetSelfAbility("warpCharge5");
            this.upgradeinfo.SetAbility(2, "recoverHp4atk");
            this.baseid = new LorId(LogLikeMod.ModId, 506005);
        }
    }


    public class UpgradeModel_Warp6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetSelfAbility("drawCardWarpLog");
            this.baseid = new LorId(LogLikeMod.ModId, 506006);
        }
    }


    public class UpgradeModel_Warp7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 0);
            this.upgradeinfo.SetSelfAbility("warpCharge4");
            this.baseid = new LorId(LogLikeMod.ModId, 506007);
        }
    }



    // Feather Shield
    public class UpgradeModel_Wedge10 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 408013);
        }
    }
    // Sparking Spear
    public class UpgradeModel_Wedge2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 408002);
        }
    }

    // Searing Sword
    public class UpgradeModel_Wedge8 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 408012);
        }
    }

    public class UpgradeModel_work1_1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetSelfAbility("powerup2byselfsmoke8Log");
            this.upgradeinfo.SetAbility(1, "smoke3atk");
            this.baseid = new LorId(LogLikeMod.ModId, 615003);
        }
    }

    public class UpgradeModel_work1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetSelfAbility("smokeSelf4");
            this.baseid = new LorId(LogLikeMod.ModId, 615001);
        }
    }

    public class UpgradeModel_work2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetSelfAbility("smokeSelf4");
            this.baseid = new LorId(LogLikeMod.ModId, 615002);
        }
    }

    public class UpgradeModel_work3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetSelfAbility("smokeSelf3");
            this.upgradeinfo.SetAbility(1, "smoke4atk");
            this.baseid = new LorId(LogLikeMod.ModId, 615004);
        }
    }

    public class UpgradeModel_work4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 2, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 615006);
        }
    }

    public class UpgradeModel_work5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 615005);
        }
    }
    // Mend Weapon
    public class UpgradeModel_workshopfixer1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 410001);
        }
    }

    public class UpgradeModel_Yan1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 2);
            this.upgradeinfo.SetAbility(0, "yanAreaDiceLog");
            this.baseid = new LorId(LogLikeMod.ModId, 611001);
        }
    }

    public class UpgradeModel_Yan2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 2);
            this.upgradeinfo.SetDice(2, 0, 1);
            this.upgradeinfo.SetAbility(1, "yanOnly1DiceLog");
            this.baseid = new LorId(LogLikeMod.ModId, 611002);
        }
    }

    public class UpgradeModel_Yan3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 2, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 611003);
        }
    }

    public class UpgradeModel_Yan4 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("yanInstantEnergyLog");
            this.baseid = new LorId(LogLikeMod.ModId, 611004);
        }
    }

    public class UpgradeModel_Yan5 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.upgradeinfo.SetAbility(2, "yanOnly1Dice");
            this.baseid = new LorId(LogLikeMod.ModId, 611005);
        }
    }

    public class UpgradeModel_Yan6 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.upgradeinfo.SetDice(2, 1, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 611006);
        }
    }

    public class UpgradeModel_Yan7 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.baseid = new LorId(LogLikeMod.ModId, 611007);
        }
    }

    public class UpgradeModel_Yun1 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 3, 0);
            this.baseid = new LorId(LogLikeMod.ModId, 102005);
        }
    }

    public class UpgradeModel_Yun2 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetSelfAbility("protection2friendLog");
            this.upgradeinfo.SetDice(0, 0, 0);
            this.upgradeinfo.SetDice(1, 0, 0);
            this.upgradeinfo.SetDice(2, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 102001);
        }
    }

    public class UpgradeModel_Yun3 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetCost(1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 102006);
        }
    }

    public class UpgradeModel_Zwei10 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 2, 1);
            this.upgradeinfo.SetDice(1, 1, 1);
            this.baseid = new LorId(LogLikeMod.ModId, 301008);
        }
    }

    public class UpgradeModel_Zwei9 : UpgradeBase
    {
        public override void Init()
        {
            this.upgradeinfo = new UpgradeBase.UpgradeInfo();
            this.upgradeinfo.SetDice(0, 1, 1);
            this.upgradeinfo.SetDice(1, 0, 1);
            this.upgradeinfo.SetDice(2, 0, 2);
            this.baseid = new LorId(LogLikeMod.ModId, 301005);
        }
    }
}