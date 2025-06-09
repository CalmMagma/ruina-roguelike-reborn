// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BlueStar3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_BlueStar3 : CreaturePickUpModel
{
  public PickUpModel_BlueStar3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_BlueStar3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_BlueStar3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_BlueStar3_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370293), (EmotionCardAbilityBase) new PickUpModel_BlueStar3.LogEmotionCardAbility_BlueStar3(), model);
  }

  public class LogEmotionCardAbility_BlueStar3 : EmotionCardAbilityBase
  {
    public bool _effect;
    public SoundEffectPlayer _loop;

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_BlueStar3.LogEmotionCardAbility_BlueStar3.BattleUnitBuf_Emotion_BlueStar_SoundBuf());
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_BlueStar3.LogEmotionCardAbility_BlueStar3.BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool());
    }

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is EmotionCardAbility_bluestar3.BattleUnitBuf_Emotion_BlueStar_SoundBuf)) == null)
        return;
      Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/9_H/FX_IllusionCard_9_H_Voice");
      if ((UnityEngine.Object) original != (UnityEngine.Object) null)
      {
        Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
        if (((UnityEngine.Object) creatureEffect != (UnityEngine.Object) null ? (UnityEngine.Object) creatureEffect.gameObject.GetComponent<AutoDestruct>() : (UnityEngine.Object) null) == (UnityEngine.Object) null)
        {
          AutoDestruct autoDestruct = (UnityEngine.Object) creatureEffect != (UnityEngine.Object) null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct) null;
          if ((UnityEngine.Object) autoDestruct != (UnityEngine.Object) null)
          {
            autoDestruct.time = 5f;
            autoDestruct.DestroyWhenDisable();
          }
        }
      }
      SoundEffectPlayer.PlaySound("Creature/BlueStar_Atk");
      SingletonBehavior<BattleSoundManager>.Instance.EndBgm();
      if ((UnityEngine.Object) this._loop == (UnityEngine.Object) null)
        this._loop = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/BlueStar_Bgm", true, parent: SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.transform);
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      this.DestroyLoopSound();
    }

    public override void OnEndBattlePhase()
    {
      base.OnEndBattlePhase();
      this.DestroyLoopSound();
    }

    public void DestroyLoopSound()
    {
      if (!((UnityEngine.Object) this._loop != (UnityEngine.Object) null))
        return;
      SingletonBehavior<BattleSoundManager>.Instance.StartBgm();
      this._loop.ManualDestroy();
      this._loop = (SoundEffectPlayer) null;
    }

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_BlueStar3.LogEmotionCardAbility_BlueStar3.BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool());
    }

    public class BattleUnitBuf_Emotion_BlueStar_SoundBuf : BattleUnitBuf
    {
      public const int _bDmgMin = 2;
      public const int _bDmgMax = 4;

      public override string keywordId => "Emotion_BlueStar_SoundBuf";

      public int BDmg => RandomUtil.Range(2, 4);

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        this.stack = 0;
      }

      public override void BeforeRollDice(BattleDiceBehavior behavior)
      {
        base.BeforeRollDice(behavior);
        if (behavior == null)
          return;
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          breakDmg = this.BDmg
        });
      }

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        this.Destroy();
      }
    }

    public class BattleUnitBuf_Emotion_BlueStar_SoundBuf_Cool : BattleUnitBuf
    {
      public const int _coolTimeMax = 3;

      public override string keywordId => "Emotion_BlueStar_SoundBuf_Cool";

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        this.stack = 3;
      }

      public override void OnRoundEndTheLast()
      {
        base.OnRoundEndTheLast();
        --this.stack;
        if (this.stack > 0)
          return;
        this.stack = 3;
        this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_BlueStar3.LogEmotionCardAbility_BlueStar3.BattleUnitBuf_Emotion_BlueStar_SoundBuf());
      }
    }
  }
}
}
