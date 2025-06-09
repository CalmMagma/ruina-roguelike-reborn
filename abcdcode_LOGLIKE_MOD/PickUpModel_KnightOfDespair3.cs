// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_KnightOfDespair3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_KnightOfDespair3 : CreaturePickUpModel
{
  public PickUpModel_KnightOfDespair3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_KnightOfDespair3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_KnightOfDespair3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_KnightOfDespair3_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370153), (EmotionCardAbilityBase) new PickUpModel_KnightOfDespair3.LogEmotionCardAbility_KnightOfDespair3(), model);
  }

  public class LogEmotionCardAbility_KnightOfDespair3 : EmotionCardAbilityBase
  {
    public const int _dmgRateMin = 10;
    public const int _dmgRateMax = 10;
    public const int _dmgMax = 12;
    public bool _bMaxDiceValue;

    public static int DmgRate => RandomUtil.Range(10, 10);

    public override void BeforeGiveDamage(BattleDiceBehavior behavior)
    {
      base.BeforeGiveDamage(behavior);
      this._bMaxDiceValue = false;
      if (behavior.Detail != BehaviourDetail.Penetrate || behavior.DiceVanillaValue != behavior.GetDiceMax())
        return;
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/KnightOfDespair_Atk_Strong");
      this._owner.battleCardResultLog.SetAttackEffectFilter(typeof (ImageFilter_ColorBlend_Despair));
      this._bMaxDiceValue = true;
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      base.OnSucceedAttack(behavior);
      if (!this._bMaxDiceValue)
        return;
      BattleUnitModel target = behavior.card?.target;
      if (target == null)
        return;
      int v = Mathf.Min(12, target.MaxHp * PickUpModel_KnightOfDespair3.LogEmotionCardAbility_KnightOfDespair3.DmgRate / 100);
      target.TakeDamage(v, DamageType.Emotion, this._owner);
    }
  }
}
}
