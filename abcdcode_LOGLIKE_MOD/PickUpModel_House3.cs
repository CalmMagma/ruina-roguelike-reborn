// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_House3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_House3 : CreaturePickUpModel
{
  public PickUpModel_House3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_House3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_House3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_House3_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370273), (EmotionCardAbilityBase) new PickUpModel_House3.LogEmotionCardAbility_House3(), model);
  }

  public class LogEmotionCardAbility_House3 : EmotionCardAbilityBase
  {
    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this.GiveBuf();
    }

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this.GiveBuf();
    }

    public override void OnRoundStart() => this.GiveBuf();

    public void GiveBuf()
    {
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
      {
        if (alive == this._owner)
        {
          if (alive.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_House3.LogEmotionCardAbility_House3.BattleUnitBuf_Emotion_WayBackHome_Protected)) == null)
          {
            BattleUnitBuf buf = (BattleUnitBuf) new PickUpModel_House3.LogEmotionCardAbility_House3.BattleUnitBuf_Emotion_WayBackHome_Protected();
            alive.bufListDetail.AddBuf(buf);
          }
        }
        else if (alive.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_House3.LogEmotionCardAbility_House3.BattleUnitBuf_Emotion_WayBackHome_Protect)) == null)
        {
          BattleUnitBuf buf = (BattleUnitBuf) new PickUpModel_House3.LogEmotionCardAbility_House3.BattleUnitBuf_Emotion_WayBackHome_Protect();
          alive.bufListDetail.AddBuf(buf);
        }
      }
    }

    public class BattleUnitBuf_Emotion_WayBackHome_Protected : BattleUnitBuf
    {
      public override string keywordId => "WayBackHome_Emotion_Protected";
    }

    public class BattleUnitBuf_Emotion_WayBackHome_Protect : BattleUnitBuf
    {
      public const int _powMin = 1;
      public const int _powMax = 2;

      public int Pow => RandomUtil.Range(1, 2);

      public override bool Hide => true;

      public override string keywordId => "WayBackHome_Emotion_Protected";

      public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
      {
        base.OnStartParrying(card);
        BattlePlayingCardDataInUnitModel currentDiceAction = card?.target.currentDiceAction;
        bool flag;
        if (currentDiceAction == null)
        {
          flag = false;
        }
        else
        {
          BattleUnitModel earlyTarget = currentDiceAction.earlyTarget;
          flag = earlyTarget != null && earlyTarget.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_House3.LogEmotionCardAbility_House3.BattleUnitBuf_Emotion_WayBackHome_Protected)) != null;
        }
        if (!flag)
          return;
        card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
        {
          power = this.Pow
        });
        this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Together", 2f);
        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/House_MakeRoad");
      }
    }
  }
}
}
