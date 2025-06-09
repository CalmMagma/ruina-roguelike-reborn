// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_LongBird2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_LongBird2 : CreaturePickUpModel
{
  public PickUpModel_LongBird2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_LongBird2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_LongBird2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_LongBird2_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370282), (EmotionCardAbilityBase) new PickUpModel_LongBird2.LogEmotionCardAbility_LongBird2(), model);
  }

  public class LogEmotionCardAbility_LongBird2 : EmotionCardAbilityBase
  {
    public override void OnRoundStart()
    {
      base.OnRoundStart();
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
      {
        if ((double) RandomUtil.valueForProb < 0.5)
        {
          BattleUnitBuf buf = alive.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is EmotionCardAbility_longbird2.BattleUnitBuf_LongBird_Emotion_Sin));
          if (buf == null)
          {
            buf = (BattleUnitBuf) new EmotionCardAbility_longbird2.BattleUnitBuf_LongBird_Emotion_Sin();
            alive.bufListDetail.AddBuf(buf);
          }
          ++buf.stack;
        }
      }
    }

    public class BattleUnitBuf_LongBird_Emotion_Sin : BattleUnitBuf
    {
      public const int _stackMax = 5;
      public const float _hpRate = 0.1f;
      public bool triggered;

      public override KeywordBuf bufType => KeywordBuf.Emotion_Sin;

      public override string keywordId => "Sin_AbnormalityCard";

      public override string keywordIconId => "Sin_Abnormality";

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        this.stack = 0;
      }

      public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
      {
        base.OnUseCard(card);
        this.triggered = false;
      }

      public override void OnSuccessAttack(BattleDiceBehavior behavior)
      {
        base.OnSuccessAttack(behavior);
        BattleUnitModel target = behavior.card?.target;
        if (target == null || this.triggered || this.stack <= 0)
          return;
        --this.stack;
        this.triggered = true;
        BattleUnitBuf buf = target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_LongBird2.LogEmotionCardAbility_LongBird2.BattleUnitBuf_LongBird_Emotion_Sin));
        if (buf == null)
        {
          buf = (BattleUnitBuf) new PickUpModel_LongBird2.LogEmotionCardAbility_LongBird2.BattleUnitBuf_LongBird_Emotion_Sin();
          target.bufListDetail.AddBuf(buf);
        }
        ++buf.stack;
      }

      public override void OnRoundEndTheLast()
      {
        base.OnRoundEndTheLast();
        if (this.stack < 5)
          return;
        this._owner.TakeDamage(Mathf.RoundToInt((float) this._owner.MaxHp * 0.1f), DamageType.Buf, this._owner);
        SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_Judgement", 1f, this._owner.view, this._owner.view, 3f);
        SoundEffectPlayer.PlaySound("Creature/LongBird_Down");
        this.Destroy();
      }
    }
  }
}
}
