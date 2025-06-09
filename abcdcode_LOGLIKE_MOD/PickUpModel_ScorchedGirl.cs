// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ScorchedGirl
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ScorchedGirl : PickUpModelBase
{
  public PickUpModel_ScorchedGirl()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ScorchedGirl_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ScorchedGirl_Desc");
    this.FlaverText = "???";
  }

  public override bool IsCanPickUp(UnitDataModel target) => true;

  public override void OnPickUp(BattleUnitModel model)
  {
    List<RewardPassiveInfo> collection = new List<RewardPassiveInfo>();
    List<EmotionCardXmlInfo> emotionCardXmlInfoList = new List<EmotionCardXmlInfo>();
    RewardPassiveInfo passiveInfo1 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370011));
    collection.Add(passiveInfo1);
    emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo1));
    RewardPassiveInfo passiveInfo2 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370012));
    collection.Add(passiveInfo2);
    emotionCardXmlInfoList.Add(LogLikeMod.GetRegisteredPickUpXml(passiveInfo2));
    RewardPassiveInfo passiveInfo3 = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.ModId, 15370013));
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
