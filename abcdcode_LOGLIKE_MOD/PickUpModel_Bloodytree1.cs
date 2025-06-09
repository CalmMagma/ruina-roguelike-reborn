// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Bloodytree1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Bloodytree1 : CreaturePickUpModel
{
  public PickUpModel_Bloodytree1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Bloodytree1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Bloodytree1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Bloodytree1_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370091), (EmotionCardAbilityBase) new PickUpModel_Bloodytree1.LogEmotionCardAbility_Bloodytree1(), model);
  }

  public class LogEmotionCardAbility_Bloodytree1 : EmotionCardAbilityBase
  {
    public override void OnGiveDeflect(BattleDiceBehavior behavior)
    {
      int num = Mathf.RoundToInt((float) behavior.DiceResultValue * 0.5f);
      behavior.card?.target?.TakeDamage(num, DamageType.Emotion, this._owner);
      if (num > 0)
        Singleton<StageController>.Instance.waveHistory.AddHokmaAchievementCount(num);
      this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("9_H/FX_IllusionCard_9_H_StickPiercing", 2f);
    }
  }
}
}
