// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_QueenOfHatred3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_QueenOfHatred3 : CreaturePickUpModel
{
  public PickUpModel_QueenOfHatred3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_QueenOfHatred3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_QueenOfHatred3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_QueenOfHatred3_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370053), (EmotionCardAbilityBase) new PickUpModel_QueenOfHatred3.LogEmotionCardAbility_QueenOfHatred3(), model);
  }

  public class LogEmotionCardAbility_QueenOfHatred3 : EmotionCardAbilityBase
  {
    public const int _strMax = 2;
    public const int _costRedCond = 3;
    public const int _bDmgMin = 2;
    public const int _bDmgMax = 4;
    public int cnt;

    public static int BDmg => RandomUtil.Range(2, 4);

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      this.cnt = 0;
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      this.cnt = 0;
    }

    public override void OnLoseParrying(BattleDiceBehavior behavior)
    {
      base.OnLoseParrying(behavior);
      if (behavior == null || behavior.card?.target == null)
        return;
      ++this.cnt;
      this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_HeartBroken", 2f);
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Oz_Atk_Boom");
      if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is EmotionCardAbility_clownofnihil3.BattleUnitBuf_Emotion_Nihil)) == null)
        this._owner.TakeBreakDamage(PickUpModel_QueenOfHatred3.LogEmotionCardAbility_QueenOfHatred3.BDmg, DamageType.Emotion, this._owner);
      if (this.cnt <= 2)
        this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this._owner);
      if (this.cnt != 3)
        return;
      this.ReduceCost();
    }

    public void ReduceCost()
    {
      List<BattleDiceCardModel> battleDiceCardModelList = new List<BattleDiceCardModel>();
      battleDiceCardModelList.AddRange((IEnumerable<BattleDiceCardModel>) this._owner.allyCardDetail.GetHand());
      if (battleDiceCardModelList.Count <= 0)
        return;
      battleDiceCardModelList.Sort((Comparison<BattleDiceCardModel>) ((x, y) => y.GetCost() - x.GetCost()));
      int targetCost = battleDiceCardModelList[0].GetCost();
      List<BattleDiceCardModel> all = battleDiceCardModelList.FindAll((Predicate<BattleDiceCardModel>) (x => x.GetCost() == targetCost));
      if (all.Count <= 0)
        Debug.LogError((object) "???");
      else
        RandomUtil.SelectOne<BattleDiceCardModel>(all).AddCost(-1);
    }
  }
}
}
