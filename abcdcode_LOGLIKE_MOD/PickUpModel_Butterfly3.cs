// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Butterfly3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Butterfly3 : CreaturePickUpModel
{
  public PickUpModel_Butterfly3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Butterfly3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Butterfly3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Butterfly3_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370323), (EmotionCardAbilityBase) new PickUpModel_Butterfly3.LogEmotionCardAbility_Butterfly3(), model);
  }

  public class LogEmotionCardAbility_Butterfly3 : EmotionCardAbilityBase
  {
    public const float _hpRate = 0.5f;
    public const int _cntMax = 2;
    public int cnt;

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      this.cnt = 0;
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      base.OnSucceedAttack(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null || (double) target.hp > (double) target.MaxHp * 0.5)
        return;
      if (!behavior.card.card.XmlData.IsFloorEgo())
      {
        if (behavior.card.card.XmlData.Spec.Ranged == CardRange.Near)
          this._owner.battleCardResultLog?.SetCreatureAbilityEffect("2/Butterfly_Emotion_Effect_Spread_Near", 1f);
        else
          this._owner.battleCardResultLog?.SetCreatureAbilityEffect("2/Butterfly_Emotion_Effect_Spread", 1f);
      }
      if (this.cnt < 2)
      {
        ++this.cnt;
        BattleUnitBuf battleUnitBuf = target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_Butterfly3.LogEmotionCardAbility_Butterfly3.BattleUnitBuf_Butterfly_Emotion_Seal));
        if (battleUnitBuf == null)
        {
          PickUpModel_Butterfly3.LogEmotionCardAbility_Butterfly3.BattleUnitBuf_Butterfly_Emotion_Seal buf = new PickUpModel_Butterfly3.LogEmotionCardAbility_Butterfly3.BattleUnitBuf_Butterfly_Emotion_Seal();
          target.bufListDetail.AddBuf((BattleUnitBuf) buf);
          buf.Add();
        }
        else
        {
          if (!(battleUnitBuf is PickUpModel_Butterfly3.LogEmotionCardAbility_Butterfly3.BattleUnitBuf_Butterfly_Emotion_Seal butterflyEmotionSeal))
            return;
          butterflyEmotionSeal.Add();
        }
      }
    }

    public class BattleUnitBuf_Butterfly_Emotion_Seal : BattleUnitBuf
    {
      public int addedThisTurn;
      public int deleteThisTurn;

      public override string keywordId => "Butterfly_Seal";

      public BattleUnitBuf_Butterfly_Emotion_Seal() => this.stack = 0;

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        this.stack -= this.deleteThisTurn;
        if (this.stack > 0)
          return;
        this.Destroy();
      }

      public override int SpeedDiceBreakedAdder() => this.stack;

      public override void OnRoundStart()
      {
        base.OnRoundStart();
        this.deleteThisTurn = this.addedThisTurn;
        this.addedThisTurn = 0;
      }

      public void Add()
      {
        ++this.stack;
        ++this.addedThisTurn;
        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/ButterFlyMan_Lock");
        int index = this._owner.passiveDetail.SpeedDiceNumAdder() - this._owner.passiveDetail.SpeedDiceBreakAdder() - this._owner.bufListDetail.SpeedDiceBreakAdder() + this.addedThisTurn - 1;
        if (index < 0 || index >= this._owner.speedDiceResult.Count || this._owner.cardSlotDetail.cardAry[index].GetRemainingAbilityCount() <= 0)
          return;
        this._owner.cardSlotDetail.cardAry[index].DestroyDice(DiceMatch.AllDice);
      }
    }
  }
}
}
