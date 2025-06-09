// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Mountain1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using Unity.Mathematics;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Mountain1 : CreaturePickUpModel
{
  public PickUpModel_Mountain1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Mountain1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Mountain1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Mountain1_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370261), (EmotionCardAbilityBase) new PickUpModel_Mountain1.LogEmotionCardAbility_Mountain1(), model);
  }

  public class LogEmotionCardAbility_Mountain1 : EmotionCardAbilityBase
  {
    public const float _healRate = 0.2f;
    public const int _healMax = 20;
    public const float _strRate = 0.1f;
    public bool _nextWave;

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this._nextWave = true;
    }

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_Mountain1.LogEmotionCardAbility_Mountain1.BattleUnitBuf_Emotion_DanggoCreature_Healed());
    }

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (this._nextWave)
        return;
      int stack = Math.Min(3, (int) ((double) this._owner.UnitData.historyInWave.healed / (double) ((float) this._owner.MaxHp * 0.1f)));
      if (stack <= 0)
        return;
      this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, stack, this._owner);
    }

    public override void OnKill(BattleUnitModel target)
    {
      base.OnKill(target);
      if (this._nextWave || target.faction == this._owner.faction)
        return;
      this._owner.RecoverHP((int) math.min(20f, 0.2f * (float) this._owner.MaxHp));
      target.battleCardResultLog?.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_Meet", 2f);
      this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.KillEffect));
    }

    public void KillEffect()
    {
      CameraFilterUtil.EarthQuake(0.18f, 0.16f, 90f, 0.45f);
      Battle.CreatureEffect.CreatureEffect original1 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/6/Dango_Emotion_Effect");
      if ((UnityEngine.Object) original1 != (UnityEngine.Object) null)
      {
        Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
        if (((UnityEngine.Object) creatureEffect != (UnityEngine.Object) null ? (UnityEngine.Object) creatureEffect.gameObject.GetComponent<AutoDestruct>() : (UnityEngine.Object) null) == (UnityEngine.Object) null)
        {
          AutoDestruct autoDestruct = (UnityEngine.Object) creatureEffect != (UnityEngine.Object) null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct) null;
          if ((UnityEngine.Object) autoDestruct != (UnityEngine.Object) null)
          {
            autoDestruct.time = 3f;
            autoDestruct.DestroyWhenDisable();
          }
        }
      }
      Battle.CreatureEffect.CreatureEffect original2 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/7/Lumberjack_final_blood_1st");
      if (!((UnityEngine.Object) original2 != (UnityEngine.Object) null))
        return;
      Battle.CreatureEffect.CreatureEffect creatureEffect1 = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
      if (((UnityEngine.Object) creatureEffect1 != (UnityEngine.Object) null ? (UnityEngine.Object) creatureEffect1.gameObject.GetComponent<AutoDestruct>() : (UnityEngine.Object) null) == (UnityEngine.Object) null)
      {
        AutoDestruct autoDestruct = (UnityEngine.Object) creatureEffect1 != (UnityEngine.Object) null ? creatureEffect1.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct) null;
        if ((UnityEngine.Object) autoDestruct != (UnityEngine.Object) null)
        {
          autoDestruct.time = 3f;
          autoDestruct.DestroyWhenDisable();
        }
      }
    }

    public class BattleUnitBuf_Emotion_DanggoCreature_Healed : BattleUnitBuf
    {
      public override string keywordId => "DangoCreature_Emotion_Healed";

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        this.SetStack();
      }

      public override void OnRoundStart()
      {
        base.OnRoundStart();
        this.SetStack();
      }

      public override void OnRoundStartAfter()
      {
        base.OnRoundStartAfter();
        this.SetStack();
      }

      public override void OnRoundEndTheLast()
      {
        base.OnRoundEndTheLast();
        this.SetStack();
      }

      public void SetStack() => this.stack = this._owner.UnitData.historyInWave.healed;
    }
  }
}
}
