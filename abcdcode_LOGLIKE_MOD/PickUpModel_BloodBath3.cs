// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BloodBath3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_BloodBath3 : CreaturePickUpModel
{
  public PickUpModel_BloodBath3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_BloodBath3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_BloodBath3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_BloodBath3_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370003), (EmotionCardAbilityBase) new PickUpModel_BloodBath3.LogEmotionCardAbility_bloodbath3(), model);
  }

  public class LogEmotionCardAbility_bloodbath3 : EmotionCardAbilityBase
  {
    public const int _dmgMin = 3;
    public const int _dmgMax = 10;
    public const int _maxStack = 3;
    public BattleUnitModel _target;
    public int _stack;

    public override int GetCounter() => this._stack;

    public override void OnRollDice(BattleDiceBehavior behavior)
    {
      base.OnRollDice(behavior);
      if (!this.IsAttackDice(behavior.Detail) || this._target == null || this._target == behavior.card.target)
        return;
      if (this._target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf)) is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf buf)
        this._target.bufListDetail.RemoveBuf((BattleUnitBuf) buf);
      this._target = (BattleUnitModel) null;
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      base.OnSucceedAttack(behavior);
      if (this._target == behavior.card.target)
      {
        ++this._stack;
        if (this._target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf)) is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf bloodBathHandDebuf)
          bloodBathHandDebuf.OnHit();
        if (this._stack < 3)
          return;
        this.Ability();
      }
      else
      {
        this._target = behavior.card.target;
        this._stack = 1;
        this._target.bufListDetail.AddBuf((BattleUnitBuf) new EmotionCardAbility_bloodbath3.BloodBath_HandDebuf());
        if (!(this._target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf)) is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf bloodBathHandDebuf))
          return;
        bloodBathHandDebuf.OnHit();
      }
    }

    public void Ability()
    {
      if (this._target == null)
        return;
      this._target.TakeBreakDamage(RandomUtil.Range(3, 10), DamageType.Emotion, this._owner);
      if (this._target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf)) is EmotionCardAbility_bloodbath3.BloodBath_HandDebuf buf)
        this._target.bufListDetail.RemoveBuf((BattleUnitBuf) buf);
      this._target.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_PaleHand_Hit", 3f);
      this._target = (BattleUnitModel) null;
      this._stack = 0;
    }

    public class BloodBath_HandDebuf : BattleUnitBuf
    {
      public override string keywordIconId => "BloodBath_Hand";

      public override string keywordId => "Bloodbath_Hands";

      public BloodBath_HandDebuf() => this.stack = 0;

      public void OnHit() => ++this.stack;
    }
  }
}
}
