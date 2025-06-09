// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ShyLookToday2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ShyLookToday2 : CreaturePickUpModel
{
  public PickUpModel_ShyLookToday2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ShyLookToday2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ShyLookToday2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ShyLookToday2_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370032), (EmotionCardAbilityBase) new PickUpModel_ShyLookToday2.LogEmotionCardAbility_ShyLookToday2(), model);
  }

  public class LogEmotionCardAbility_ShyLookToday2 : EmotionCardAbilityBase
  {
    public const int _addMin = 1;
    public const int _addMax = 2;
    public const int _dmgMin = 2;
    public const int _dmgMax = 7;

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      if (behavior.Detail != BehaviourDetail.Guard)
        return;
      int num1 = RandomUtil.Range(1, 2);
      int num2 = RandomUtil.Range(2, 7);
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = num1,
        guardBreakAdder = num2
      });
    }

    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      base.OnWinParrying(behavior);
      if (behavior.Detail != BehaviourDetail.Guard)
        return;
      this._owner.battleCardResultLog?.SetCreatureAbilityEffect("3/ShyLookToday_Guard", 0.8f);
    }
  }
}
}
