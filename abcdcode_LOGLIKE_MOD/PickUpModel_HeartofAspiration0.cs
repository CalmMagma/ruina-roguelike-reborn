// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_HeartofAspiration0
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_HeartofAspiration0 : CreaturePickUpModel
{
  public PickUpModel_HeartofAspiration0()
  {
    this.level = 2;
    this.ids = new List<LorId>()
    {
      new LorId(LogLikeMod.ModId, 15370101),
      new LorId(LogLikeMod.ModId, 15370102),
      new LorId(LogLikeMod.ModId, 15370103)
    };
  }

  public override List<EmotionCardXmlInfo> GetCreatureList()
  {
    return CreaturePickUpModel.GetEmotionListById(this.ids);
  }

  public override string GetCreatureName()
  {
    return TextDataModel.GetText("PickUpCreature_HeartofAspiration_Name");
  }

  public override Sprite GetSprite() => LogLikeMod.ArtWorks["Creature_HeartofAspiration"];
}
}
