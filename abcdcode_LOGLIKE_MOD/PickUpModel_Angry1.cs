// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Angry1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Angry1 : CreaturePickUpModel
{
  public PickUpModel_Angry1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Angry1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Angry1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Angry1_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370351), (EmotionCardAbilityBase) new PickUpModel_Angry1.LogEmotionCardAbility_Angry1(), model);
  }

  public class LogEmotionCardAbility_Angry1 : EmotionCardAbilityBase
  {
    public override void OnWaveStart()
    {
      base.OnWaveStart();
      if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_Angry1.LogEmotionCardAbility_Angry1.BattleUnitBuf_Emotion_Wrath_Berserk)) != null)
        return;
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_Angry1.LogEmotionCardAbility_Angry1.BattleUnitBuf_Emotion_Wrath_Berserk());
    }

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_Angry1.LogEmotionCardAbility_Angry1.BattleUnitBuf_Emotion_Wrath_Berserk());
    }

    public class BattleUnitBuf_Emotion_Wrath_Berserk : BattleUnitBuf
    {
      public GameObject aura;
      public const int _str = 2;
      public const int _pp = 2;
      public const int _draw = 2;

      public override string keywordId => "Angry_Angry";

      public override string keywordIconId => "Wrath_Head";

      public override bool IsControllable => this.Controlable();

      public bool Controlable()
      {
        return this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is EmotionCardAbility_clownofnihil3.BattleUnitBuf_Emotion_Nihil)) != null;
      }

      public override bool TeamKill() => true;

      public override void OnRoundEndTheLast()
      {
        base.OnRoundEndTheLast();
        this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 2, this._owner);
      }

      public override void OnRoundStart()
      {
        base.OnRoundStart();
        this._owner.cardSlotDetail.RecoverPlayPoint(2);
        this._owner.allyCardDetail.DrawCards(2);
      }

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Rage", 1f, owner.view, owner.view);
        this.aura = (UnityEngine.Object) fxCreatureEffect != (UnityEngine.Object) null ? fxCreatureEffect.gameObject : (GameObject) null;
        SoundEffectPlayer.PlaySound("Creature/Angry_Meet");
      }

      public override void OnDie()
      {
        base.OnDie();
        this.Destroy();
      }

      public override void Destroy()
      {
        base.Destroy();
        this.DestroyAura();
      }

      public void DestroyAura()
      {
        if (!((UnityEngine.Object) this.aura != (UnityEngine.Object) null))
          return;
        UnityEngine.Object.Destroy((UnityEngine.Object) this.aura);
        this.aura = (GameObject) null;
      }
    }
  }
}
}
