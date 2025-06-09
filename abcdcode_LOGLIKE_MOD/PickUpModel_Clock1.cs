// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Clock1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Clock1 : CreaturePickUpModel
{
  public PickUpModel_Clock1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Clock1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Clock1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Clock1_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370191), (EmotionCardAbilityBase) new PickUpModel_Clock1.LogEmotionCardAbility_Clock1(), model);
  }

  public class LogEmotionCardAbility_Clock1 : EmotionCardAbilityBase
  {
    public const float _START_BATTLE_TIME = 30f;
    public const int _powMin = 1;
    public const int _powMax = 2;
    public bool rolled;
    public float _elapsed;
    public bool _bTimeLimitOvered;
    public Silence_Emotion_Clock _clock;

    public static int Pow => RandomUtil.Range(1, 2);

    public Silence_Emotion_Clock Clock
    {
      get
      {
        if ((Object) this._clock == (Object) null)
          this._clock = SingletonBehavior<BattleManagerUI>.Instance.EffectLayer.GetComponentInChildren<Silence_Emotion_Clock>();
        if ((Object) this._clock == (Object) null)
        {
          Silence_Emotion_Clock original = Resources.Load<Silence_Emotion_Clock>("Prefabs/Battle/CreatureEffect/8/Silence_Emotion_Clock");
          if ((Object) original != (Object) null)
          {
            Silence_Emotion_Clock silenceEmotionClock = Object.Instantiate<Silence_Emotion_Clock>(original);
            silenceEmotionClock.gameObject.transform.SetParent(SingletonBehavior<BattleManagerUI>.Instance.EffectLayer);
            silenceEmotionClock.gameObject.transform.localPosition = new Vector3(0.0f, 800f, 0.0f);
            silenceEmotionClock.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
          }
          this._clock = original;
        }
        return this._clock;
      }
    }

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this.Init();
    }

    public void Init()
    {
      this._elapsed = 0.0f;
      this._bTimeLimitOvered = false;
      this.rolled = false;
    }

    public override void OnFixedUpdateInWaitPhase(float delta)
    {
      base.OnFixedUpdateInWaitPhase(delta);
      if (!this.rolled || this._bTimeLimitOvered)
        return;
      Silence_Emotion_Clock clock = this.Clock;
      if ((Object) clock != (Object) null)
        clock.Run(this._elapsed);
      this._elapsed += delta;
      if ((double) this._elapsed >= 30.0 && !SingletonBehavior<BattleCamManager>.Instance.IsCamIsReturning)
      {
        Singleton<StageController>.Instance.CompleteApplyingLibrarianCardPhase(true);
        this._bTimeLimitOvered = true;
        this._elapsed = 0.0f;
      }
    }

    public override void OnAfterRollSpeedDice()
    {
      base.OnAfterRollSpeedDice();
      this.Init();
      this.rolled = true;
      Silence_Emotion_Clock clock = this.Clock;
      if ((Object) clock == (Object) null)
        return;
      clock.OnStartRollSpeedDice();
    }

    public override void OnStartBattle()
    {
      base.OnStartBattle();
      Silence_Emotion_Clock clock = this.Clock;
      if ((Object) clock == (Object) null)
        return;
      clock.OnStartUnitMoving();
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      this.rolled = false;
    }

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = PickUpModel_Clock1.LogEmotionCardAbility_Clock1.Pow
      });
    }
  }
}
}
