// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ChildofGalaxy1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using Sound;
using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ChildofGalaxy1 : CreaturePickUpModel
{
  public PickUpModel_ChildofGalaxy1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ChildofGalaxy1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ChildofGalaxy1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ChildofGalaxy1_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370141), (EmotionCardAbilityBase) new PickUpModel_ChildofGalaxy1.LogEmotionCardAbility_ChildofGalaxy1(), model);
  }

  public class LogEmotionCardAbility_ChildofGalaxy1 : EmotionCardAbilityBase
  {
    public const int _targetNum = 3;
    public const int _recoverMin = 3;
    public const int _recoverMax = 7;
    public const int _loseMin = 1;
    public const int _loseMax = 3;
    public const string _icon = "GalaxyBoy_Stone";

    public static int Recover => RandomUtil.Range(3, 7);

    public static int Lose => RandomUtil.Range(1, 3);

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      List<BattleDiceCardModel> battleDiceCardModelList = new List<BattleDiceCardModel>();
      List<BattleDiceCardModel> hand = this._owner.allyCardDetail.GetHand();
      hand.RemoveAll((Predicate<BattleDiceCardModel>) (x => x.GetSpec().Ranged == CardRange.Instance));
      if (hand.Count <= 3)
      {
        battleDiceCardModelList.AddRange((IEnumerable<BattleDiceCardModel>) hand);
      }
      else
      {
        for (int index = 0; index < 3 && hand.Count > 0; ++index)
        {
          BattleDiceCardModel battleDiceCardModel = RandomUtil.SelectOne<BattleDiceCardModel>(hand);
          hand.Remove(battleDiceCardModel);
          battleDiceCardModelList.Add(battleDiceCardModel);
        }
      }
      if (battleDiceCardModelList.Count <= 0)
        return;
      foreach (BattleDiceCardModel battleDiceCardModel in battleDiceCardModelList)
      {
        battleDiceCardModel.AddBuf((BattleDiceCardBuf) new PickUpModel_ChildofGalaxy1.LogEmotionCardAbility_ChildofGalaxy1.GalaxyChildPebbleBuf());
        battleDiceCardModel.SetAddedIcon("GalaxyBoy_Stone");
      }
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      foreach (BattleDiceCardModel battleDiceCardModel in this._owner.allyCardDetail.GetAllDeck())
      {
        BattleDiceCardBuf buf = battleDiceCardModel.GetBufList().Find((Predicate<BattleDiceCardBuf>) (x => x is PickUpModel_ChildofGalaxy1.LogEmotionCardAbility_ChildofGalaxy1.GalaxyChildPebbleBuf));
        if (buf != null)
        {
          battleDiceCardModel.RemoveBuf(buf);
          battleDiceCardModel.RemoveAddedIcon("GalaxyBoy_Stone");
        }
      }
    }

    public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
    {
      base.OnUseCard(curCard);
      if (curCard.card.GetBufList().FindAll((Predicate<BattleDiceCardBuf>) (x => x is PickUpModel_ChildofGalaxy1.LogEmotionCardAbility_ChildofGalaxy1.GalaxyChildPebbleBuf)).Count > 0)
      {
        this._owner.RecoverHP(PickUpModel_ChildofGalaxy1.LogEmotionCardAbility_ChildofGalaxy1.Recover);
        if (!Singleton<StageController>.Instance.IsLogState())
        {
          SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_GalaxyCard_O", 1f, this._owner.view, this._owner.view, 2f);
          SoundEffectPlayer.PlaySound("Creature/GalaxyBoy_Heal");
        }
        else
        {
          this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("4_N/FX_IllusionCard_4_N_GalaxyCard_O", 2f);
          this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/GalaxyBoy_Heal");
        }
      }
      else
      {
        this._owner.LoseHp(PickUpModel_ChildofGalaxy1.LogEmotionCardAbility_ChildofGalaxy1.Lose);
        if (!Singleton<StageController>.Instance.IsLogState())
        {
          SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_GalaxyCard_X", 1f, this._owner.view, this._owner.view, 2f);
          SoundEffectPlayer.PlaySound("Creature/GalaxyBoy_Deal");
        }
        else
        {
          this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("4_N/FX_IllusionCard_4_N_GalaxyCard_X", 2f);
          this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/GalaxyBoy_Deal");
        }
      }
    }

    public class GalaxyChildPebbleBuf : BattleDiceCardBuf
    {
      public override string keywordIconId => "GalaxyBoy_Stone";
    }
  }
}
}
