// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BossReward4
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll


namespace abcdcode_LOGLIKE_MOD
{
    [HideFromItemCatalog]
    public class PickUpModel_BossReward4 : PickUpModelBase
    {
        public PickUpModel_BossReward4()
        {
            this.Name = TextDataModel.GetText("BossReward4Name");
            this.Desc = TextDataModel.GetText("BossReward4Desc");
            this.FlaverText = TextDataModel.GetText("BossRewardFlaverText");
            this.ArtWork = "BossReward4";
        }

        public override void OnPickUp()
        {
            base.OnPickUp();
            LogLikeMod.AddPlayer = false;
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            base.OnPickUp(model);
            LogueBookModels.AddPlayerStat(model.UnitData, (LogStatAdder)new PickUpModel_BossReward4.BossStatAdder());
            LogLikeMod.AddPlayer = false;
        }

        public class BossStatAdder : LogStatAdder
        {
            public BossStatAdder()
            {
                this.maxbreakpercent = 100;
                this.maxhppercent = 100;
            }
        }
    }
}
