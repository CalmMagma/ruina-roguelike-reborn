// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_House2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_House2 : CreaturePickUpModel
{
  public PickUpModel_House2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_House2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_House2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_House2_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370272), (EmotionCardAbilityBase) new PickUpModel_House2.LogEmotionCardAbility_House2(), model);
  }

  public class LogEmotionCardAbility_House2 : EmotionCardAbilityBase
  {
    public const int _dmgPerStack = 3;
    public int stack = 1;

    public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
    {
      base.OnParryingStart(card);
      BattleUnitModel target = card?.target;
      if (target == null)
        return;
      if (!this.CheckAbility(target))
      {
        foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
        {
          foreach (BattleEmotionCardModel passive in alive.emotionDetail.PassiveList)
          {
            if (passive.AbilityList.Find((Predicate<EmotionCardAbilityBase>) (x => x is PickUpModel_House2.LogEmotionCardAbility_House2)) is PickUpModel_House2.LogEmotionCardAbility_House2 cardAbilityHouse2)
              cardAbilityHouse2.StackToZero();
          }
        }
      }
      else
      {
        card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
        {
          dmg = 3 * this.stack
        });
        foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
        {
          foreach (BattleEmotionCardModel passive in alive.emotionDetail.PassiveList)
          {
            if (passive.AbilityList.Find((Predicate<EmotionCardAbilityBase>) (x => x is PickUpModel_House2.LogEmotionCardAbility_House2)) is PickUpModel_House2.LogEmotionCardAbility_House2 cardAbilityHouse2)
              cardAbilityHouse2.StackAdd();
          }
        }
        this._owner.battleCardResultLog?.SetCreatureAbilityEffect("7/WayBeckHome_Emotion_Atk", 1f);
      }
    }

    public override void OnStartOneSideAction(BattlePlayingCardDataInUnitModel curCard)
    {
      base.OnStartOneSideAction(curCard);
      BattleUnitModel target = curCard?.target;
      if (target == null)
        return;
      if (!this.CheckAbility(target))
      {
        foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
        {
          foreach (BattleEmotionCardModel passive in alive.emotionDetail.PassiveList)
          {
            if (passive.AbilityList.Find((Predicate<EmotionCardAbilityBase>) (x => x is PickUpModel_House2.LogEmotionCardAbility_House2)) is PickUpModel_House2.LogEmotionCardAbility_House2 cardAbilityHouse2)
              cardAbilityHouse2.StackToZero();
          }
        }
      }
      else
      {
        curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
        {
          dmg = 3 * this.stack
        });
        foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
        {
          foreach (BattleEmotionCardModel passive in alive.emotionDetail.PassiveList)
          {
            if (passive.AbilityList.Find((Predicate<EmotionCardAbilityBase>) (x => x is PickUpModel_House2.LogEmotionCardAbility_House2)) is PickUpModel_House2.LogEmotionCardAbility_House2 cardAbilityHouse2)
              cardAbilityHouse2.StackAdd();
          }
        }
        this._owner.battleCardResultLog?.SetCreatureAbilityEffect("7/WayBeckHome_Emotion_Atk", 1f);
        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/House_NormalAtk");
      }
    }

    public override void OnRoundStartOnce()
    {
      base.OnRoundStartOnce();
      this.stack = 1;
      List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player);
      double num1 = Math.Ceiling((double) aliveList.Count * 0.5);
      int num2 = 0;
      while (aliveList.Count > 0 && num1 > 0.0)
      {
        BattleUnitModel battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(aliveList);
        if (battleUnitModel != null)
        {
          ++num2;
          PickUpModel_House2.LogEmotionCardAbility_House2.BattleUnitBuf_Emotion_WayBackHome_Target buf = new PickUpModel_House2.LogEmotionCardAbility_House2.BattleUnitBuf_Emotion_WayBackHome_Target(num2);
          battleUnitModel.bufListDetail.AddBuf((BattleUnitBuf) buf);
          --num1;
        }
        aliveList.Remove(battleUnitModel);
      }
    }

    public bool CheckAbility(BattleUnitModel target)
    {
      BattleUnitBuf battleUnitBuf = target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_House2.LogEmotionCardAbility_House2.BattleUnitBuf_Emotion_WayBackHome_Target));
      return battleUnitBuf != null && battleUnitBuf.stack == this.stack;
    }

    public void StackToZero() => this.stack = 0;

    public void StackAdd() => ++this.stack;

    public class BattleUnitBuf_Emotion_WayBackHome_Target : BattleUnitBuf
    {
      public GameObject aura;

      public override string keywordId => "WayBackHome_Emotion_Target";

      public override string keywordIconId => "WayBackHome_Target";

      public BattleUnitBuf_Emotion_WayBackHome_Target(int value) => this.stack = value;

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        Battle.CreatureEffect.CreatureEffect creatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("7/WayBeckHome_Emotion_Way", 1f, owner.view, owner.view);
        this.aura = (UnityEngine.Object) creatureEffect != (UnityEngine.Object) null ? creatureEffect.gameObject : (GameObject) null;
      }

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        this.Destroy();
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
