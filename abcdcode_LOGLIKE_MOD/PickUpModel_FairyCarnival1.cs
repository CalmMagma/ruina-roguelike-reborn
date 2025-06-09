// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_FairyCarnival1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Battle.CreatureEffect;
using Sound;
using System;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_FairyCarnival1 : CreaturePickUpModel
{
  public PickUpModel_FairyCarnival1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_FairyCarnival1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_FairyCarnival1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_FairyCarnival1_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370211), (EmotionCardAbilityBase) new PickUpModel_FairyCarnival1.LogEmotionCardAbility_FairyCarnival1(), model);
  }

  public class LogEmotionCardAbility_FairyCarnival1 : EmotionCardAbilityBase
  {
    public const int _maxHeal = 18;
    public const float _healRate = 0.15f;
    public int _count;
    public int _takeDamageCount;
    public CreatureEffect_Anim _effect;
    public bool _hit;
    public bool _destroy;
    public int _hitCount;

    public override void OnSelectEmotion()
    {
      try
      {
        this._effect = this.MakeEffect("1/Fairy_Gluttony") as CreatureEffect_Anim;
        CreatureEffect_Anim effect = this._effect;
        if ((UnityEngine.Object) effect != (UnityEngine.Object) null)
          effect.SetLayer("Character");
        SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Fariy_Special");
        if (!((UnityEngine.Object) soundEffectPlayer != (UnityEngine.Object) null))
          return;
        soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
      }
    }

    public override void OnRoundEnd()
    {
      if (this._count < 3)
      {
        int num = Mathf.Min((int) ((double) this._owner.MaxHp * 0.15000000596046448), 18);
        this._owner.RecoverHP(num);
        ++this._count;
        if ((UnityEngine.Object) this._effect != (UnityEngine.Object) null)
          this._effect.SetTrigger("Recover");
        this._owner.view.RecoverHp(num);
      }
      if (this._count < 3)
        return;
      if ((UnityEngine.Object) this._effect != (UnityEngine.Object) null)
      {
        this._effect.SetTrigger("Disappear");
        this._effect.gameObject.AddComponent<AutoDestruct>().time = 2f;
      }
      this._effect = (CreatureEffect_Anim) null;
    }

    public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
    {
      if (this._takeDamageCount >= 3 || this._count >= 3)
        return;
      ++this._takeDamageCount;
      if (this._takeDamageCount >= 3)
      {
        this._count = 5;
        int dmg1 = this._owner.LoseHp(Mathf.Min((int) this._owner.hp * 25 / 100, 30));
        this._owner.battleCardResultLog?.SetDamageTaken(dmg1, atkDice.behaviourInCard.Dice, atkDice.Detail);
        this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 1, ResultOption.Default, dmg1);
        this._destroy = true;
        if ((bool) (UnityEngine.Object) this._effect)
          this.ApplyCreatureEffect((Battle.CreatureEffect.CreatureEffect) this._effect);
      }
      else
      {
        this._hit = true;
        ++this._hitCount;
        if ((bool) (UnityEngine.Object) this._effect)
          this.ApplyCreatureEffect((Battle.CreatureEffect.CreatureEffect) this._effect);
      }
      this._owner?.battleCardResultLog?.SetCreatureEffectSound("Creature/Fairy_Dead");
    }

    public override void OnPrintEffect(BattleDiceBehavior behavior)
    {
      if (this._hit)
      {
        if ((UnityEngine.Object) this._effect != (UnityEngine.Object) null)
          this._effect.SetTrigger("Hit");
        --this._hitCount;
        if (this._hitCount == 0)
          this._hit = false;
      }
      if (!this._destroy)
        return;
      this._destroy = false;
      if ((UnityEngine.Object) this._effect != (UnityEngine.Object) null)
        this._effect.SetTrigger("Disappear");
      this._effect = (CreatureEffect_Anim) null;
    }

    public override void OnLayerChanged(string layerName)
    {
      if (!((UnityEngine.Object) this._effect == (UnityEngine.Object) null))
        return;
      this._effect.SetLayer(layerName);
    }
  }
}
}
