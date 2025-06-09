// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_RedShoes3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_RedShoes3 : CreaturePickUpModel
{
  public PickUpModel_RedShoes3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_RedShoes3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_RedShoes3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_RedShoes3_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370133), (EmotionCardAbilityBase) new PickUpModel_RedShoes3.LogEmotionCardAbility_RedShoes3(), model);
  }

  public class LogEmotionCardAbility_RedShoes3 : EmotionCardAbilityBase
  {
    public const int _powMin = 1;
    public const int _powMax = 3;
    public const int _loseMin = 2;
    public const int _loseMax = 5;

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      if (behavior.Detail != BehaviourDetail.Slash)
        return;
      int num = RandomUtil.Range(1, 3);
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = num
      });
    }

    public override void OnLoseParrying(BattleDiceBehavior behavior)
    {
      base.OnLoseParrying(behavior);
      if (behavior.Detail != BehaviourDetail.Slash)
        return;
      this._owner.breakDetail.TakeBreakDamage(RandomUtil.Range(2, 5), DamageType.Passive, this._owner);
    }

    public override void OnDrawParrying(BattleDiceBehavior behavior)
    {
      base.OnDrawParrying(behavior);
      if (behavior.Detail != BehaviourDetail.Slash)
        return;
      this._owner.breakDetail.TakeBreakDamage(RandomUtil.Range(2, 5), DamageType.Emotion, this._owner);
    }

    public override void OnRollDice(BattleDiceBehavior behavior)
    {
      base.OnRollDice(behavior);
      if (behavior.Detail != BehaviourDetail.Slash)
        return;
      SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedShoes_SlashHit");
      if ((Object) soundEffectPlayer != (Object) null)
        soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
      this._owner.battleCardResultLog.SetAttackEffectFilter(typeof (ImageFilter_ColorBlend_Red));
    }
  }
}
}
