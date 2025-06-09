// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.MysteryModel_Mystery_Ch4_2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class MysteryModel_Mystery_Ch4_2 : MysteryBase
{
  public override void OnClickChoice(int choiceid)
  {
    if (this.curFrame.FrameID == 0 && choiceid == 0)
      LogueBookModels.AddMoney(Random.Range(30, 51));
    if (this.curFrame.FrameID == 1)
    {
      LogueStageInfo stageInfo = Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.ModId, 1400021));
      stageInfo.type = StageType.Normal;
      MysteryBase.AddStageList(stageInfo, ChapterGrade.Grade4);
      MysteryBase.AddStageList(stageInfo, ChapterGrade.Grade4);
      MysteryBase.AddStageList(stageInfo, ChapterGrade.Grade4);
    }
    base.OnClickChoice(choiceid);
  }
}
}
