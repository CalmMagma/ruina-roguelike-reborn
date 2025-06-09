// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Bloodytree3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Bloodytree3 : CreaturePickUpModel
{
  public PickUpModel_Bloodytree3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Bloodytree3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Bloodytree3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Bloodytree3_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370093), (EmotionCardAbilityBase) new PickUpModel_Bloodytree3.LogEmotionCardAbility_Bloodytree3(), model);
  }

  public class LogEmotionCardAbility_Bloodytree3 : EmotionCardAbilityBase
  {
    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      base.OnWinParrying(behavior);
      if (behavior.Detail != BehaviourDetail.Guard)
        return;
      this._owner.battleCardResultLog?.SetCreatureAbilityEffect("9/HokmaFirst_Guard", 0.8f);
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/SnowWhite_NormalAtk");
    }

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null)
        return;
      BattlePlayingCardDataInUnitModel currentDiceAction = target.currentDiceAction;
      if (currentDiceAction != null)
      {
        BattleDiceBehavior currentBehavior = currentDiceAction.currentBehavior;
        if (currentBehavior != null)
          currentBehavior.ApplyDiceStatBonus(new DiceStatBonus()
          {
            guardBreakMultiplier = 2
          });
      }
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        guardBreakMultiplier = 2
      });
    }
  }
}
}
