// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_HappyTeddyBear2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using Sound;
using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_HappyTeddyBear2 : CreaturePickUpModel
{
  public PickUpModel_HappyTeddyBear2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_HappyTeddyBear2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_HappyTeddyBear2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_HappyTeddyBear2_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370112), (EmotionCardAbilityBase) new PickUpModel_HappyTeddyBear2.LogEmotionCardAbility_HappyTeddyBear2(), model);
  }

  public class LogEmotionCardAbility_HappyTeddyBear2 : EmotionCardAbilityBase
  {
    public LorId _id = LorId.None;

    public override void OnRoundStart()
    {
      if (this._id == LorId.None)
      {
        List<BattleDiceCardModel> hand = this._owner.allyCardDetail.GetHand();
        hand.RemoveAll((Predicate<BattleDiceCardModel>) (x => x.GetSpec().Ranged == CardRange.Instance));
        int highest = 0;
        foreach (BattleDiceCardModel battleDiceCardModel in hand)
        {
          int cost = battleDiceCardModel.GetCost();
          if (highest < cost)
            highest = cost;
        }
        List<BattleDiceCardModel> all = hand.FindAll((Predicate<BattleDiceCardModel>) (x => x.GetCost() == highest));
        this._id = all.Count <= 0 ? LorId.None : RandomUtil.SelectOne<BattleDiceCardModel>(all).GetID();
      }
      this.Ability();
    }

    public override void OnSelectEmotion() => SoundEffectPlayer.PlaySound("Creature/Teddy_On");

    public void Ability()
    {
      if (this._id == LorId.None)
        return;
      foreach (BattleDiceCardModel battleDiceCardModel in this._owner.allyCardDetail.GetAllDeck().FindAll((Predicate<BattleDiceCardModel>) (x => x.GetID() == this._id)))
      {
        battleDiceCardModel.AddBufWithoutDuplication((BattleDiceCardBuf) new PickUpModel_HappyTeddyBear2.LogEmotionCardAbility_HappyTeddyBear2.TeddyCardBuf());
        battleDiceCardModel.SetAddedIcon("TeddyIcon");
      }
    }

    public class TeddyCardBuf : BattleDiceCardBuf
    {
      public override DiceCardBufType bufType => DiceCardBufType.Teddy;

      public override string keywordIconId => "TeddyIcon";

      public override void OnUseCard(BattleUnitModel owner)
      {
        List<BattleDiceCardModel> allDeck = owner.allyCardDetail.GetAllDeck();
        if (this._card.GetOriginCost() == 4 && this._card.GetCost() <= 0)
          PlatformManager.Instance.UnlockAchievement(AchievementEnum.ONCE_FLOOR1);
        foreach (BattleDiceCardModel battleDiceCardModel in allDeck.FindAll((Predicate<BattleDiceCardModel>) (x => x.GetID() == this._card.GetID())))
          battleDiceCardModel.AddCost(-1);
      }
    }
  }
}
}
