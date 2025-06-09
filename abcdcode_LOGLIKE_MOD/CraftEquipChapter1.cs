// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.CraftEquipChapter1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UI;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class CraftEquipChapter1 : CraftEffect
{
  public override Sprite GetCraftSprite() => LogLikeMod.ArtWorks["Chapter1Icon"];

  public override string GetCraftName() => TextDataModel.GetText("CraftEquipChapter1Name");

  public override string GetCraftDesc() => TextDataModel.GetText("CraftEquipChapter1Desc");

  public override int GetCraftCost() => 10;

  public override bool CanCraft(int costresult)
  {
    if (CraftEffect.CheckCreaftEquipLimit(ChapterGrade.Grade1) != null)
      return base.CanCraft(costresult);
    UIAlarmPopup.instance.SetAlarmText(TextDataModel.GetText("CraftEquipCant"));
    return false;
  }

  public override void Crafting()
  {
    base.Crafting();
    CraftEffect.CraftEquipByChapter(ChapterGrade.Grade1);
  }
}
}
