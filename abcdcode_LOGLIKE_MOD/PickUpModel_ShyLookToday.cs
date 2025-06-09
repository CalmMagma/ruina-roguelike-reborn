// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShyLookToday
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ShyLookToday : PickUpModelBase
{
  public PickUpModel_ShyLookToday()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ShyLookToday_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ShyLookToday_Desc");
    this.FlaverText = "???";
  }

  public override bool IsCanPickUp(UnitDataModel target) => true;

  public override void OnPickUp(BattleUnitModel model)
  {
    List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
    List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
    RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370031));
    collection.Add(passiveInfo1);
    emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
    RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370032));
    collection.Add(passiveInfo2);
    emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
    RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370033));
    collection.Add(passiveInfo3);
    emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo3));
    MysteryBase.curinfo = new MysteryBase.MysteryAbnormalInfo()
    {
      abnormal = emotionCardXmlInfoList,
      level = 1
    };
    LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>) collection);
  }
}
}
