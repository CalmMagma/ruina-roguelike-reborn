// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.CraftEquipChapter7
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UI;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class CraftEquipChapter7 : CraftEffect
{
  public override Sprite GetCraftSprite() => LogLikeMod.ArtWorks["Chapter7Icon"];

  public override string GetCraftName() => TextDataModel.GetText("CraftEquipChapter7Name");

  public override string GetCraftDesc() => TextDataModel.GetText("CraftEquipChapter7Desc");

  public override int GetCraftCost() => 35;

  public override bool CanCraft(int costresult)
  {
    bool flag;
    if (CraftEffect.CheckCreaftEquipLimit(ChapterGrade.Grade7) == null)
    {
      UIAlarmPopup.instance.SetAlarmText(TextDataModel.GetText("CraftEquipCant"));
      flag = false;
    }
    else
      flag = base.CanCraft(costresult);
    return flag;
  }

  public override void Crafting()
  {
    base.Crafting();
    CraftEffect.CraftEquipByChapter(ChapterGrade.Grade7);
  }
}
}
