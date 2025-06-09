// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ScareCrow3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ScareCrow3 : CreaturePickUpModel
{
  public PickUpModel_ScareCrow3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ScareCrow3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ScareCrow3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ScareCrow3_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370073), (EmotionCardAbilityBase) new PickUpModel_ScareCrow3.LogEmotionCardAbility_ScareCrow3(), model);
  }

  public class LogEmotionCardAbility_ScareCrow3 : EmotionCardAbilityBase
  {
    public override void OnMakeBreakState(BattleUnitModel target)
    {
      base.OnMakeBreakState(target);
      if (target == null || target == this._owner)
        return;
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Scarecrow_Dead");
      List<BattleDiceCardModel> battleDiceCardModelList = new List<BattleDiceCardModel>();
      switch (this._owner.Book.ClassInfo.RangeType)
      {
        case EquipRangeType.Melee:
          battleDiceCardModelList = target.allyCardDetail.GetAllDeck().FindAll((Predicate<BattleDiceCardModel>) (x => x.GetSpec().Ranged == CardRange.Near));
          break;
        case EquipRangeType.Range:
          battleDiceCardModelList = target.allyCardDetail.GetAllDeck().FindAll((Predicate<BattleDiceCardModel>) (x => x.GetSpec().Ranged == CardRange.Far));
          break;
        case EquipRangeType.Hybrid:
          battleDiceCardModelList = target.allyCardDetail.GetAllDeck().FindAll((Predicate<BattleDiceCardModel>) (x => x.GetSpec().Ranged == CardRange.Near || x.GetSpec().Ranged == CardRange.Far));
          break;
      }
      battleDiceCardModelList.RemoveAll((Predicate<BattleDiceCardModel>) (x => x.temporary));
      foreach (int index in MathUtil.Combination(2, battleDiceCardModelList.Count))
      {
        BattleDiceCardModel battleDiceCardModel = this._owner.allyCardDetail.AddNewCard(battleDiceCardModelList[index].GetID());
        battleDiceCardModel.SetCurrentCost(battleDiceCardModel.GetOriginCost() - 2);
        battleDiceCardModel.XmlData.optionList.Add(CardOption.ExhaustOnUse);
        battleDiceCardModel.AddBuf((BattleDiceCardBuf) new PickUpModel_ScareCrow3.LogEmotionCardAbility_ScareCrow3.BattleDiceCardBuf_scarecrowCreated());
      }
    }

    public class BattleDiceCardBuf_scarecrowCreated : BattleDiceCardBuf
    {
      public override void OnUseCard(BattleUnitModel owner)
      {
        Singleton<StageController>.Instance.waveHistory.AddRakeCreatedUsed();
      }
    }
  }
}
}
