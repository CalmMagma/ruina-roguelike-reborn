// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Clock2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;
using System.Collections;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Clock2 : CreaturePickUpModel
{
  public PickUpModel_Clock2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Clock2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Clock2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Clock2_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370192), (EmotionCardAbilityBase) new PickUpModel_Clock2.LogEmotionCardAbility_Clock2(), model);
  }

  public class LogEmotionCardAbility_Clock2 : EmotionCardAbilityBase
  {
    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this.GiveBuf();
    }

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this.GiveBuf();
    }

    public void GiveBuf()
    {
      if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_Clock2.LogEmotionCardAbility_Clock2.BattleUnitBuf_Emotion_Silence_Bell)) != null)
        return;
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_Clock2.LogEmotionCardAbility_Clock2.BattleUnitBuf_Emotion_Silence_Bell());
    }

    public class BattleUnitBuf_Emotion_Silence_Bell : BattleUnitBuf
    {
      public const int _stackMax = 13;
      public const int _powMin = 1;
      public const int _powMax = 2;
      public bool triggered;

      public override string keywordId => "Clock_Thirteen";

      public override string keywordIconId => "Silence_Emotion_Bell";

      public static int Pow => RandomUtil.Range(1, 2);

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        this.stack = 0;
      }

      public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
      {
        base.OnUseCard(card);
        this.triggered = false;
        ++this.stack;
        if (this.stack < 13)
          return;
        this.stack = 0;
        this.triggered = true;
        this._owner.battleCardResultLog?.SetEndCardActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.ResetTrigger));
      }

      public override void BeforeRollDice(BattleDiceBehavior behavior)
      {
        base.BeforeRollDice(behavior);
        if (!this.triggered || behavior == null)
          return;
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          power = PickUpModel_Clock2.LogEmotionCardAbility_Clock2.BattleUnitBuf_Emotion_Silence_Bell.Pow
        });
      }

      public override void OnSuccessAttack(BattleDiceBehavior behavior)
      {
        base.OnSuccessAttack(behavior);
        if (!this.triggered)
          return;
        BattleUnitModel target = behavior?.card?.target;
        if (target != null)
        {
          int v = Mathf.RoundToInt((float) (target.MaxHp * behavior.DiceResultValue) * 0.01f);
          target.TakeDamage(v, DamageType.Card_Ability, this._owner);
          this._owner.battleCardResultLog?.SetSucceedAtkEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Groggy));
        }
      }

      public void ResetTrigger() => this.triggered = false;

      public void Groggy()
      {
        BattleCamManager instance = SingletonBehavior<BattleCamManager>.Instance;
        Camera effectCam = (UnityEngine.Object) instance != (UnityEngine.Object) null ? instance.EffectCam : (Camera) null;
        if ((UnityEngine.Object) effectCam.GetComponent<CameraFilterPack_Broken_Screen>() == (UnityEngine.Object) null)
        {
          CameraFilterPack_Broken_Screen r = effectCam.gameObject.AddComponent<CameraFilterPack_Broken_Screen>();
          AutoScriptDestruct autoScriptDestruct = effectCam.gameObject.AddComponent<AutoScriptDestruct>();
          autoScriptDestruct.targetScript = (MonoBehaviour) r;
          autoScriptDestruct.time = 1f;
          autoScriptDestruct.StartCoroutine(this.BrokenRoutine(r));
        }
        SoundEffectPlayer.PlaySound("Creature/Clock_NoCreate");
      }

      public IEnumerator BrokenRoutine(CameraFilterPack_Broken_Screen r)
      {
        float elapsed = 0.0f;
        while ((double) elapsed < 1.0)
        {
          elapsed += Time.deltaTime * 2f;
          if ((UnityEngine.Object) r != (UnityEngine.Object) null)
            r.Fade = (float) (0.75 - (double) elapsed * 0.75);
          yield return (object) YieldCache.waitFixedUpdate;
        }
      }
    }
  }
}
}
