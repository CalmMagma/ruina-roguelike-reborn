// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_LumberJack3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_LumberJack3 : CreaturePickUpModel
{
  public PickUpModel_LumberJack3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_LumberJack3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_LumberJack3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_LumberJack3_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370173), (EmotionCardAbilityBase) new PickUpModel_LumberJack3.LogEmotionCardAbility_LumberJack3(), model);
  }

  public class LogEmotionCardAbility_LumberJack3 : EmotionCardAbilityBase
  {
    public const int _powMax = 2;
    public bool trigger;

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      this.trigger = false;
      BattleUnitModel target = behavior?.card?.target;
      if (target == null || !this.IsAttackDice(behavior.Detail))
        return;
      int num1 = target.cardSlotDetail.PlayPoint - target.cardSlotDetail.ReservedPlayPoint;
      int num2 = this._owner.cardSlotDetail.PlayPoint - this._owner.cardSlotDetail.ReservedPlayPoint;
      if (num1 > num2)
      {
        int num3 = num1;
        if (num3 > 0)
        {
          if (num3 > 2)
            num3 = 2;
          this.trigger = true;
          behavior.ApplyDiceStatBonus(new DiceStatBonus()
          {
            power = num3
          });
        }
      }
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      base.OnSucceedAttack(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target != null && this.trigger)
      {
        target.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Bloodmeet", 2f);
        target.battleCardResultLog?.SetCreatureEffectSound("Creature/WoodMachine_Kill");
      }
      this.trigger = false;
    }
  }
}
}
