// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShyLookToday3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ShyLookToday3 : CreaturePickUpModel
{
  public PickUpModel_ShyLookToday3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ShyLookToday3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ShyLookToday3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ShyLookToday3_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370033), (EmotionCardAbilityBase) new PickUpModel_ShyLookToday3.LogEmotionCardAbility_ShyLookToday3(), model);
  }

  public class LogEmotionCardAbility_ShyLookToday3 : EmotionCardAbilityBase
  {
    public Queue<bool> triggers = new Queue<bool>();
    public int count;
    public Coroutine effect;

    public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
    {
      base.OnUseCard(curCard);
      this.count = 0;
      foreach (DiceBehaviour originalDiceBehavior in curCard.GetOriginalDiceBehaviorList())
      {
        if (originalDiceBehavior.Type == BehaviourType.Def)
          ++this.count;
      }
    }

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      if (behavior.Detail != BehaviourDetail.Guard)
        return;
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = this.count
      });
    }

    public override void OnRollDice(BattleDiceBehavior behavior)
    {
      base.OnRollDice(behavior);
      if (!this.IsDefenseDice(behavior.Detail))
        return;
      this._owner?.battleCardResultLog?.SetRolldiceEvent(new BattleCardBehaviourResult.BehaviourEvent(this.ChangeColor));
    }

    public void ChangeColorDefault()
    {
      if (this._owner.view.gameObject.activeInHierarchy && this.effect != null)
        this._owner.view.charAppearance.StopCoroutine(this.effect);
      this.effect = this._owner.view.charAppearance.ChangeColor(Color.white, 0.75f);
    }

    public void ChangeColor()
    {
      if (this._owner.view.gameObject.activeInHierarchy && this.effect != null)
        this._owner.view.charAppearance.StopCoroutine(this.effect);
      this.effect = this._owner.view.charAppearance.ChangeColor(new Color(1f, 0.0f, 0.0f, 1f), 0.75f, new Action(this.ChangeColorDefault));
    }

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      this.triggers.Clear();
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      this.triggers.Clear();
    }
  }
}
}
