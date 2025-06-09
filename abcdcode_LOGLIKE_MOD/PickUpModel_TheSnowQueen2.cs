// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_TheSnowQueen2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_TheSnowQueen2 : CreaturePickUpModel
{
  public PickUpModel_TheSnowQueen2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_TheSnowQueen2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_TheSnowQueen2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_TheSnowQueen2_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370302), (EmotionCardAbilityBase) new PickUpModel_TheSnowQueen2.LogEmotionCardAbility_TheSnowQueen2(), model);
  }

  public class LogEmotionCardAbility_TheSnowQueen2 : EmotionCardAbilityBase
  {
    public int cnt;

    public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
    {
      base.OnParryingStart(card);
      this.cnt = 0;
    }

    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      base.OnWinParrying(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null)
        return;
      ++this.cnt;
      target.battleCardResultLog?.SetNewCreatureAbilityEffect("0_K/FX_IllusionCard_0_K_IceUnATK", 2f);
      target.battleCardResultLog?.SetCreatureEffectSound("Creature/SnowQueen_Guard");
      if (this.cnt != 2)
        return;
      BattleUnitBuf buf = target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_TheSnowQueen2.LogEmotionCardAbility_TheSnowQueen2.BattleUnitBuf_Emotion_SnowQueen_Shard));
      if (buf == null)
      {
        buf = (BattleUnitBuf) new PickUpModel_TheSnowQueen2.LogEmotionCardAbility_TheSnowQueen2.BattleUnitBuf_Emotion_SnowQueen_Shard(this._owner);
        target.bufListDetail.AddBuf(buf);
      }
      if (!(buf is PickUpModel_TheSnowQueen2.LogEmotionCardAbility_TheSnowQueen2.BattleUnitBuf_Emotion_SnowQueen_Shard emotionSnowQueenShard))
        return;
      emotionSnowQueenShard.Add();
    }

    public override void OnEndParrying(BattlePlayingCardDataInUnitModel curCard)
    {
      base.OnEndParrying(curCard);
      this.cnt = 0;
    }

    public class BattleUnitBuf_Emotion_SnowQueen_Shard : BattleUnitBuf
    {
      public const int _stackMax = 3;
      public BattleUnitModel _attacker;

      public override string keywordId => "SnowQueen_Emotion_Shard";

      public override string keywordIconId => "SnowQueen_Debuf";

      public BattleUnitBuf_Emotion_SnowQueen_Shard(BattleUnitModel attacker)
      {
        this._attacker = attacker;
        this.stack = 0;
      }

      public void Add() => ++this.stack;

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        if (this.stack < 3)
          return;
        this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_TheSnowQueen2.LogEmotionCardAbility_TheSnowQueen2.BattleUnitBuf_Emotion_SnowQueen_Stun2(this._attacker));
        this.Destroy();
      }
    }

    public class BattleUnitBuf_Emotion_SnowQueen_Stun2 : BattleUnitBuf
    {
      public BattleUnitModel _attacker;
      public Battle.CreatureEffect.CreatureEffect _aura;

      public override bool Hide => true;

      public BattleUnitBuf_Emotion_SnowQueen_Stun2(BattleUnitModel attacker)
      {
        this._attacker = attacker;
      }

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Stun, 1, this._attacker);
      }

      public override void OnRoundStart()
      {
        base.OnRoundStart();
        if (this._owner.bufListDetail.GetActivatedBuf(KeywordBuf.Stun) == null || this._owner.IsImmune(KeywordBuf.Stun) || this._owner.bufListDetail.IsImmune(BufPositiveType.Negative))
          return;
        this._owner.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
        this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("0/SnowQueen_Emotion_Frozen", 1f, this._owner.view, this._owner.view);
        SoundEffectPlayer.PlaySound("Creature/SnowQueen_Immune");
      }

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        if ((UnityEngine.Object) this._aura != (UnityEngine.Object) null)
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) this._aura.gameObject);
          this._aura = (Battle.CreatureEffect.CreatureEffect) null;
          this._owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
        }
        this.Destroy();
      }
    }
  }
}
}
