// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ScorchedGirl3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ScorchedGirl3 : CreaturePickUpModel
{
  public PickUpModel_ScorchedGirl3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ScorchedGirl3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ScorchedGirl3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ScorchedGirl3_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370013), (EmotionCardAbilityBase) new PickUpModel_ScorchedGirl3.LogEmotionCardAbility_ScorchedGirl3(), model);
  }

  public class LogEmotionCardAbility_ScorchedGirl3 : EmotionCardAbilityBase
  {
    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new EmotionCardAbility_burnningGirl3.BattleUnitBuf_burningGirl_Ember_New());
    }

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      BattleUnitBuf battleUnitBuf = this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is EmotionCardAbility_burnningGirl3.BattleUnitBuf_burningGirl_Ember_New));
      if (battleUnitBuf == null)
      {
        this._owner.bufListDetail.AddBuf((BattleUnitBuf) new EmotionCardAbility_burnningGirl3.BattleUnitBuf_burningGirl_Ember_New());
      }
      else
      {
        List<LorId> lorIdList = battleUnitBuf is EmotionCardAbility_burnningGirl3.BattleUnitBuf_burningGirl_Ember_New burningGirlEmberNew ? burningGirlEmberNew.TargetIDs() : (List<LorId>) null;
        foreach (BattleDiceCardModel battleDiceCardModel in this._owner.allyCardDetail.GetAllDeck())
        {
          if (lorIdList.Contains(battleDiceCardModel.GetID()))
          {
            battleDiceCardModel.AddBuf((BattleDiceCardBuf) new EmotionCardAbility_burnningGirl3.BattleDiceCardBuf_Emotion_BurningGirl());
            battleDiceCardModel.SetAddedIcon("Burning_Match", -1);
          }
        }
      }
    }

    public class BattleUnitBuf_burningGirl_Ember_New : BattleUnitBuf
    {
      public const float _prob = 0.25f;
      public const int _triggerStack = 4;
      public bool _triggered;
      public int _max;
      public List<LorId> _targetIDs = new List<LorId>();

      public override string keywordId => "BurningGirl_Ember";

      public override string keywordIconId => "Burning_Match";

      public BattleUnitBuf_burningGirl_Ember_New() => this.stack = 0;

      public override void AfterDiceAction(BattleDiceBehavior behavior)
      {
        base.AfterDiceAction(behavior);
        if (!this.CheckCondition(behavior))
          return;
        if (this.stack >= 4 && (double) RandomUtil.valueForProb < 0.25)
        {
          this._triggered = true;
          this._max = behavior.GetDiceMax();
          this._owner.TakeDamage(this._max, DamageType.Buf);
        }
        else
          this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MatchGirl_NoBoom");
      }

      public override void BeforeRollDice(BattleDiceBehavior behavior)
      {
        base.BeforeRollDice(behavior);
        if (!this.CheckCondition(behavior))
          return;
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          max = this.stack
        });
      }

      public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
      {
        base.OnUseCard(card);
        if (card == null || card.isKeepedCard)
          return;
        if (this._targetIDs.Count < 2)
        {
          this._targetIDs.Add(card.card.GetID());
          foreach (BattleDiceCardModel battleDiceCardModel in this._owner.allyCardDetail.GetAllDeck())
          {
            if (battleDiceCardModel.GetID() == card.card.GetID())
            {
              battleDiceCardModel.AddBuf((BattleDiceCardBuf) new EmotionCardAbility_burnningGirl3.BattleDiceCardBuf_Emotion_BurningGirl());
              battleDiceCardModel.SetAddedIcon("Burning_Match");
            }
          }
        }
        if (!this._targetIDs.Contains(card.card.GetID()))
          return;
        ++this.stack;
      }

      public override void OnSuccessAttack(BattleDiceBehavior behavior)
      {
        base.OnSuccessAttack(behavior);
        if (!this.CheckCondition(behavior))
          return;
        int stack = this.stack;
        if (stack > 0)
          behavior.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, stack, this._owner);
      }

      public override void OnRollDiceInRecounter()
      {
        base.OnRollDiceInRecounter();
        if (!this._triggered)
          return;
        SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("1/MatchGirl_Footfall", 1f, this._owner.view, (BattleUnitView) null, 2f).AttachEffectLayer();
        SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/MatchGirl_Explosion");
        if ((UnityEngine.Object) soundEffectPlayer != (UnityEngine.Object) null)
          soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
        this._triggered = false;
      }

      public bool CheckCondition(BattleDiceBehavior behavior)
      {
        BattlePlayingCardDataInUnitModel card = behavior?.card;
        return card != null && this._targetIDs.Contains(card.card.GetID()) && this.IsFirstDice(behavior);
      }

      public bool IsFirstDice(BattleDiceBehavior behavior)
      {
        return behavior != null && behavior.Index == 0;
      }

      public List<LorId> TargetIDs() => this._targetIDs;
    }

    public class BattleDiceCardBuf_Emotion_BurningGirl : BattleDiceCardBuf
    {
      public override string keywordIconId => "Burning_Match";
    }
  }
}
}
