// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BossReward3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_BossReward3 : PickUpModelBase
{
  public static int[] MoneyRewardTable = new int[6]
  {
    20,
    25,
    30,
    35,
    40,
    50
  };

  public PickUpModel_BossReward3()
  {
    int index = (int) LogLikeMod.curchaptergrade;
    if (index > 5)
      index = 5;
    this.Name = TextDataModel.GetText("BossReward3Name");
    this.Desc = TextDataModel.GetText("BossReward3Desc", (object) PickUpModel_BossReward3.MoneyRewardTable[index]);
    this.FlaverText = TextDataModel.GetText("BossRewardFlaverText");
    this.ArtWork = "BossReward3";
  }

  public override void OnPickUp()
  {
    base.OnPickUp();
    int index = (int) LogLikeMod.curchaptergrade;
    if (index > 5)
      index = 5;
    LogueBookModels.AddMoney(PickUpModel_BossReward3.MoneyRewardTable[index]);
    MysteryBase.AddStageList(Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 111001)), LogLikeMod.curchaptergrade + 1);
  }
}
}
