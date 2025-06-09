// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Redhood2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;
using Unity.Mathematics;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Redhood2 : CreaturePickUpModel
{
  public PickUpModel_Redhood2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Redhood2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Redhood2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Redhood2_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370062), (EmotionCardAbilityBase) new PickUpModel_Redhood2.LogEmotionCardAbility_Redhood2(), model);
  }

  public class LogEmotionCardAbility_Redhood2 : EmotionCardAbilityBase
  {
    public const float _strRate = 0.2f;
    public const int _stackMax = 2;

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      new GameObject().AddComponent<SpriteFilter_Queenbee_Spore>().Init("EmotionCardFilter/RedHood_Filter", false, 2f);
      SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedHood_Change_mad");
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      if (this._owner.history.takeDamageAtOneRound <= 0)
        return;
      int x = (int) Math.Round((double) this._owner.history.takeDamageAtOneRound * 0.20000000298023224);
      if (x > 0)
        this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, math.min(x, 2), this._owner);
    }
  }
}
}
