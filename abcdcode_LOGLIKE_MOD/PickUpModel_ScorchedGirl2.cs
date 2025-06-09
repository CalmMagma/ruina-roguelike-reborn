// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ScorchedGirl2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ScorchedGirl2 : CreaturePickUpModel
{
  public PickUpModel_ScorchedGirl2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ScorchedGirl2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ScorchedGirl2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ScorchedGirl2_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370012), (EmotionCardAbilityBase) new PickUpModel_ScorchedGirl2.LogEmotionCardAbility_ScorchedGirl2(), model);
  }

  public class LogEmotionCardAbility_ScorchedGirl2 : EmotionCardAbilityBase
  {
    public const int _burnMin = 1;
    public const int _burnMax = 3;
    public const float _hpRate = 0.2f;
    public const float _dmgRate = 0.3f;
    public const int _maxDmg = 36;
    public Battle.CreatureEffect.CreatureEffect _effect;

    public static int Burn => RandomUtil.Range(1, 3);

    public override void OnStartCardAction(BattlePlayingCardDataInUnitModel curCard)
    {
      if ((double) this._owner.hp > (double) this._owner.MaxHp * 0.20000000298023224)
        return;
      int v = Mathf.Min((int) ((double) this._owner.MaxHp * 0.30000001192092896), 36);
      BattleUnitModel target = curCard?.target;
      target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, PickUpModel_ScorchedGirl2.LogEmotionCardAbility_ScorchedGirl2.Burn, this._owner);
      target?.TakeDamage(v, DamageType.Emotion);
      SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/MatchGirl_Explosion");
      if ((UnityEngine.Object) soundEffectPlayer != (UnityEngine.Object) null)
        soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
      this._effect = this.MakeEffect("1/MatchGirl_Footfall", destroyTime: 2f, apply: false);
      this._effect.AttachEffectLayer();
      this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.RemoveUI));
      try
      {
        this._owner.view.StartDeadEffect(false);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex);
      }
      this._owner.Die();
    }

    public void RemoveUI() => this._owner.view.unitBottomStatUI.EnableCanvas(false);

    public override void OnPrintEffect(BattleDiceBehavior behavior)
    {
      this._effect = (Battle.CreatureEffect.CreatureEffect) null;
    }

    public override void OnSelectEmotion() => SoundEffectPlayer.PlaySound("Creature/MatchGirl_Cry");
  }
}
}
