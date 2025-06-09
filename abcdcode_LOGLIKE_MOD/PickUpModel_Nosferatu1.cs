// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Nosferatu1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Nosferatu1 : CreaturePickUpModel
{
  public PickUpModel_Nosferatu1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Nosferatu1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Nosferatu1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Nosferatu1_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370361), (EmotionCardAbilityBase) new PickUpModel_Nosferatu1.LogEmotionCardAbility_Nosferatu1(), model);
  }

  public class LogEmotionCardAbility_Nosferatu1 : EmotionCardAbilityBase
  {
    public const int _bleed = 1;
    public const int _blood = 1;

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      base.OnSucceedAttack(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null)
        return;
      target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 1, this._owner);
      if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is BattleUnitBuf_Nosferatu_Emotion_Blood)) is BattleUnitBuf_Nosferatu_Emotion_Blood nosferatuEmotionBlood)
        nosferatuEmotionBlood.Add();
      target.battleCardResultLog?.SetCreatureAbilityEffect("6/Nosferatu_Emotion_BloodDrain");
      target.battleCardResultLog?.SetCreatureEffectSound("Nosferatu_Changed_BloodEat");
    }

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new BattleUnitBuf_Nosferatu_Emotion_Blood());
    }

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new BattleUnitBuf_Nosferatu_Emotion_Blood());
    }
  }
}
}
