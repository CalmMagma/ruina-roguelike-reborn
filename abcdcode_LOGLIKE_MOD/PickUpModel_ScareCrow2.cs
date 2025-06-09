// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ScareCrow2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ScareCrow2 : CreaturePickUpModel
{
  public PickUpModel_ScareCrow2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ScareCrow2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ScareCrow2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ScareCrow2_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370072), (EmotionCardAbilityBase) new PickUpModel_ScareCrow2.LogEmotionCardAbility_ScareCrow2(), model);
  }

  public class LogEmotionCardAbility_ScareCrow2 : EmotionCardAbilityBase
  {
    public const int _bpMin = 2;
    public const int _bpMax = 5;
    public const int _bDmgMin = 2;
    public const int _bDmgMax = 4;
    public bool trigger;

    public int RecoverBP => RandomUtil.Range(2, 5);

    public int BDmg => RandomUtil.Range(2, 4);

    public override void BeforeGiveDamage(BattleDiceBehavior behavior)
    {
      base.BeforeGiveDamage(behavior);
      BattleUnitModel target = behavior.card?.target;
      if (target == null || target.breakDetail.breakGauge <= this._owner.breakDetail.breakGauge)
        return;
      this.trigger = true;
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      base.OnSucceedAttack(behavior);
      if (!this.trigger)
        return;
      this.trigger = false;
      this._owner.breakDetail.RecoverBreak(this.RecoverBP);
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Scarecrow_Drink");
      behavior?.card?.target.TakeBreakDamage(this.BDmg, DamageType.Emotion, this._owner);
    }
  }
}
}
