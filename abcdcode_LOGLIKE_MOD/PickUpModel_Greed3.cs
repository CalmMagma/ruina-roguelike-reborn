// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Greed3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Greed3 : CreaturePickUpModel
{
  public PickUpModel_Greed3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Greed3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Greed3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Greed3_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370253), (EmotionCardAbilityBase) new PickUpModel_Greed3.LogEmotionCardAbility_Greed3(), model);
  }

  public class LogEmotionCardAbility_Greed3 : EmotionCardAbilityBase
  {
    public const int _condition = 4;
    public const int _dmgMin = 3;
    public const int _dmgMax = 7;
    public const int _healMin = 2;
    public const int _healMax = 5;

    public static int Dmg => RandomUtil.Range(3, 7);

    public static int Heal => RandomUtil.Range(2, 5);

    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      base.OnWinParrying(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null)
        return;
      try
      {
        int diceResultValue1 = behavior.DiceResultValue;
        BattleDiceBehavior targetDice = behavior.TargetDice;
        int num1 = diceResultValue1;
        int? diceResultValue2 = targetDice?.DiceResultValue;
        int? nullable = diceResultValue2.HasValue ? new int?(num1 - diceResultValue2.GetValueOrDefault()) : new int?();
        int num2 = 4;
        if (!(nullable.GetValueOrDefault() >= num2 & nullable.HasValue))
          return;
        target.TakeDamage(PickUpModel_Greed3.LogEmotionCardAbility_Greed3.Dmg, DamageType.Emotion, this._owner);
        this._owner.RecoverHP(PickUpModel_Greed3.LogEmotionCardAbility_Greed3.Heal);
        this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_GoldCrash", 1.5f);
        target.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_GoldCrash", 1.5f);
        target.battleCardResultLog?.SetCreatureEffectSound("Creature/Greed_Vert_Change");
      }
      catch
      {
      }
    }

    public override void OnLoseParrying(BattleDiceBehavior behavior)
    {
      base.OnLoseParrying(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null)
        return;
      if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is EmotionCardAbility_clownofnihil3.BattleUnitBuf_Emotion_Nihil)) != null)
        return;
      try
      {
        int? diceResultValue1 = behavior.TargetDice?.DiceResultValue;
        int diceResultValue2 = behavior.DiceResultValue;
        int? nullable = diceResultValue1.HasValue ? new int?(diceResultValue1.GetValueOrDefault() - diceResultValue2) : new int?();
        int num = 4;
        if (!(nullable.GetValueOrDefault() >= num & nullable.HasValue))
          return;
        this._owner.TakeDamage(PickUpModel_Greed3.LogEmotionCardAbility_Greed3.Dmg, DamageType.Emotion, this._owner);
        target.RecoverHP(PickUpModel_Greed3.LogEmotionCardAbility_Greed3.Heal);
        this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_GoldCrash", 1.5f);
        target.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_GoldCrash", 1.5f);
        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Greed_Vert_Change");
      }
      catch
      {
      }
    }
  }
}
}
