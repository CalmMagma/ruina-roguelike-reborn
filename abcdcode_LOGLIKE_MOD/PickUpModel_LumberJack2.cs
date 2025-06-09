// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_LumberJack2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_LumberJack2 : CreaturePickUpModel
{
  public PickUpModel_LumberJack2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_LumberJack2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_LumberJack2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_LumberJack2_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370172), (EmotionCardAbilityBase) new PickUpModel_LumberJack2.LogEmotionCardAbility_LumberJack2(), model);
  }

  public class LogEmotionCardAbility_LumberJack2 : EmotionCardAbilityBase
  {
    public const int _dmg = 15;
    public const int _targetNum = 2;
    public int accumulatedDmg;
    public bool dmged;
    public bool killed;

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (this.killed || this.dmged)
        this.Ability();
      this.accumulatedDmg = 0;
      this.killed = false;
      this.dmged = false;
    }

    public override void AfterGiveDamage(BattleUnitModel target, int dmg)
    {
      base.AfterGiveDamage(target, dmg);
      if (this.dmged)
        return;
      this.accumulatedDmg += dmg;
      if (this.accumulatedDmg >= 15)
      {
        this.dmged = true;
        this.Effect();
      }
    }

    public override void OnKill(BattleUnitModel target)
    {
      base.OnKill(target);
      this.killed = true;
      this.Effect();
    }

    public void Effect()
    {
      if (!Singleton<StageController>.Instance.IsLogState())
      {
        SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Heart", 1f, this._owner.view, this._owner.view, 1.5f);
        SoundEffectPlayer.PlaySound("Creature/WoodMachine_AtkStrong");
        SoundEffectPlayer.PlaySound("Creature/Heart_Guard");
      }
      else
      {
        this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Heart", 1.5f);
        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/WoodMachine_AtkStrong");
        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Heart_Guard");
      }
    }

    public void Ability()
    {
      List<BattleDiceCardModel> battleDiceCardModelList = new List<BattleDiceCardModel>();
      battleDiceCardModelList.AddRange((IEnumerable<BattleDiceCardModel>) this._owner.allyCardDetail.GetHand());
      if (battleDiceCardModelList.Count <= 0)
        return;
      if (this.killed)
      {
        int num = 2;
        while (battleDiceCardModelList.Count > 0)
        {
          if (num <= 0)
            break;
          battleDiceCardModelList.Sort((Comparison<BattleDiceCardModel>) ((x, y) => y.GetCost() - x.GetCost()));
          int targetCost = battleDiceCardModelList[0].GetCost();
          BattleDiceCardModel battleDiceCardModel = RandomUtil.SelectOne<BattleDiceCardModel>(battleDiceCardModelList.FindAll((Predicate<BattleDiceCardModel>) (x => x.GetCost() == targetCost)));
          battleDiceCardModelList.Remove(battleDiceCardModel);
          battleDiceCardModel.AddBuf((BattleDiceCardBuf) new PickUpModel_LumberJack2.LogEmotionCardAbility_LumberJack2.BattleDiceCardBuf_Lumberjack_Emotion_Killed());
          --num;
        }
      }
      else
      {
        if (!this.dmged)
          return;
        battleDiceCardModelList.Sort((Comparison<BattleDiceCardModel>) ((x, y) => y.GetCost() - x.GetCost()));
        int targetCost = battleDiceCardModelList[0].GetCost();
        RandomUtil.SelectOne<BattleDiceCardModel>(battleDiceCardModelList.FindAll((Predicate<BattleDiceCardModel>) (x => x.GetCost() == targetCost))).AddBuf((BattleDiceCardBuf) new PickUpModel_LumberJack2.LogEmotionCardAbility_LumberJack2.BattleDiceCardBuf_Lumberjack_Emotion());
      }
    }

    public class BattleDiceCardBuf_Lumberjack_Emotion : BattleDiceCardBuf
    {
      public override void OnUseCard(BattleUnitModel owner)
      {
        base.OnUseCard(owner);
        this.Destroy();
      }

      public override int GetCost(int oldCost) => oldCost - 1;
    }

    public class BattleDiceCardBuf_Lumberjack_Emotion_Killed : 
      PickUpModel_LumberJack2.LogEmotionCardAbility_LumberJack2.BattleDiceCardBuf_Lumberjack_Emotion
    {
      public override int GetCost(int oldCost) => 0;
    }
  }
}
}
