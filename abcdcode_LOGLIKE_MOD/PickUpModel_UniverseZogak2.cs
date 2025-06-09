// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_UniverseZogak2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_UniverseZogak2 : CreaturePickUpModel
{
  public PickUpModel_UniverseZogak2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_UniverseZogak2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_UniverseZogak2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_UniverseZogak2_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370042), (EmotionCardAbilityBase) new PickUpModel_UniverseZogak2.LogEmotionCardAbility_UniverseZogak2(), model);
  }

  public class LogEmotionCardAbility_UniverseZogak2 : EmotionCardAbilityBase
  {
    public Battle.CreatureEffect.CreatureEffect _hitEffect;

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      if (behavior.Detail != BehaviourDetail.Penetrate)
        return;
      this._hitEffect = this.MakeEffect("4/Fragment_Hit", destroyTime: 1f);
      Battle.CreatureEffect.CreatureEffect hitEffect = this._hitEffect;
      if ((Object) hitEffect != (Object) null)
        hitEffect.gameObject.SetActive(false);
      if (behavior.card.target != null)
      {
        SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Cosmos_Hit");
        if ((Object) soundEffectPlayer == (Object) null)
          return;
        soundEffectPlayer.SetGlobalPosition(behavior.card.target.view.WorldPosition);
      }
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      BattleUnitModel target = behavior.card?.target;
      if (target == null)
        return;
      float num = (float) target.breakDetail.breakGauge / (float) target.breakDetail.GetDefaultBreakGauge();
      if (behavior.Detail == BehaviourDetail.Penetrate)
      {
        BattleUnitBuf activatedBuf = target.bufListDetail.GetActivatedBuf(KeywordBuf.UniverseCardBuf);
        if (activatedBuf != null)
          ++activatedBuf.stack;
        else
          target.bufListDetail.AddBuf((BattleUnitBuf) new EmotionCardAbility_fragmentSpace2.UniverseBuf());
      }
      if (!target.IsBreakLifeZero())
      {
        target.breakDetail.breakGauge = (int) ((double) target.breakDetail.GetDefaultBreakGauge() * (double) num);
        if (target.breakDetail.breakGauge < 1)
        {
          target.breakDetail.breakLife = 0;
          target.breakDetail.breakGauge = 0;
          target.breakDetail.DestroyBreakPoint();
        }
      }
    }

    public override void OnPrintEffect(BattleDiceBehavior behavior)
    {
      if (!(bool) (Object) this._hitEffect)
        return;
      this._hitEffect.gameObject.SetActive(true);
      this._hitEffect = (Battle.CreatureEffect.CreatureEffect) null;
    }

    public class UniverseBuf : BattleUnitBuf
    {
      public override KeywordBuf bufType => KeywordBuf.UniverseCardBuf;

      public override StatBonus GetStatBonus()
      {
        return new StatBonus()
        {
          breakRate = -Mathf.Min(50, this.stack * 5)
        };
      }
    }
  }
}
}
