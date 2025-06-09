// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Nosferatu2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Nosferatu2 : CreaturePickUpModel
{
  public PickUpModel_Nosferatu2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Nosferatu2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Nosferatu2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Nosferatu2_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370362), (EmotionCardAbilityBase) new PickUpModel_Nosferatu2.LogEmotionCardAbility_Nosferatu2(), model);
  }

  public class LogEmotionCardAbility_Nosferatu2 : EmotionCardAbilityBase
  {
    public const int _heal = 10;
    public bool _trigger;

    public override void OnKill(BattleUnitModel target)
    {
      base.OnKill(target);
      if (target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding) == null)
        return;
      target.battleCardResultLog?.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_TeathATK");
      target.battleCardResultLog?.SetCreatureAbilityEffect("6/Nosferatu_Emotion_BloodDrain");
      target.battleCardResultLog?.SetCreatureEffectSound("Creature/Nosferatu_Change");
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
        alive.RecoverHP(10);
      this._trigger = true;
    }

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (this._trigger && this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is BattleUnitBuf_Nosferatu_Emotion_Blood)) is BattleUnitBuf_Nosferatu_Emotion_Blood nosferatuEmotionBlood)
        nosferatuEmotionBlood.StackToMax();
      this._trigger = false;
    }
  }
}
}
