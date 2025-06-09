// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Laetitia2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Laetitia2 : CreaturePickUpModel
{
  public PickUpModel_Laetitia2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Laetitia2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Laetitia2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Laetitia2_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370332), (EmotionCardAbilityBase) new PickUpModel_Laetitia2.LogEmotionCardAbility_Laetitia2(), model);
  }

  public class LogEmotionCardAbility_Laetitia2 : EmotionCardAbilityBase
  {
    public const int _powMin = 2;
    public const int _powMax = 4;

    public static int Pow => RandomUtil.Range(2, 4);

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      List<BattleDiceCardModel> hand = this._owner.allyCardDetail.GetHand();
      hand.RemoveAll((Predicate<BattleDiceCardModel>) (x => x.GetSpec().Ranged == CardRange.Instance));
      if (hand.Count <= 0 || hand.Find((Predicate<BattleDiceCardModel>) (x => x.GetBufList().Find((Predicate<BattleDiceCardBuf>) (y => y is PickUpModel_Laetitia2.LogEmotionCardAbility_Laetitia2.BattleDiceCardBuf_Emotion_Heart)) != null)) != null)
        return;
      BattleDiceCardModel battleDiceCardModel = RandomUtil.SelectOne<BattleDiceCardModel>(hand);
      battleDiceCardModel.AddBuf((BattleDiceCardBuf) new PickUpModel_Laetitia2.LogEmotionCardAbility_Laetitia2.BattleDiceCardBuf_Emotion_Heart());
      battleDiceCardModel.SetAddedIcon("Latitia_Heart");
    }

    public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
    {
      base.OnUseCard(curCard);
      BattleDiceCardBuf battleDiceCardBuf = curCard?.card?.GetBufList().Find((Predicate<BattleDiceCardBuf>) (y => y is PickUpModel_Laetitia2.LogEmotionCardAbility_Laetitia2.BattleDiceCardBuf_Emotion_Heart));
      if (battleDiceCardBuf == null)
        return;
      curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
      {
        power = PickUpModel_Laetitia2.LogEmotionCardAbility_Laetitia2.Pow
      });
      battleDiceCardBuf.Destroy();
    }

    public class BattleDiceCardBuf_Emotion_Heart : BattleDiceCardBuf
    {
      public const int _dmgMin = 2;
      public const int _dmgMax = 7;
      public const int _turn = 2;
      public int turn;
      public bool used;

      public static int Dmg => RandomUtil.Range(2, 7);

      public override string keywordIconId => "Latitia_Heart";

      public override void OnUseCard(BattleUnitModel owner)
      {
        base.OnUseCard(owner);
        this.used = true;
      }

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        if (this.used)
        {
          this.Destroy();
        }
        else
        {
          ++this.turn;
          if (this.turn < 2)
            return;
          BattleUnitModel owner = this._card?.owner;
          owner?.TakeDamage(PickUpModel_Laetitia2.LogEmotionCardAbility_Laetitia2.BattleDiceCardBuf_Emotion_Heart.Dmg, DamageType.Emotion, owner);
          this._card.temporary = true;
          new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Latitia_Filter_Grey", false, 2f);
          this.Destroy();
        }
      }
    }
  }
}
}
