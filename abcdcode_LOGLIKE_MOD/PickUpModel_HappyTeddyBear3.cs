// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_HappyTeddyBear3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_HappyTeddyBear3 : CreaturePickUpModel
{
  public PickUpModel_HappyTeddyBear3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_HappyTeddyBear3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_HappyTeddyBear3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_HappyTeddyBear3_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370113), (EmotionCardAbilityBase) new PickUpModel_HappyTeddyBear3.LogEmotionCardAbility_HappyTeddyBear3(), model);
  }

  public class LogEmotionCardAbility_HappyTeddyBear3 : EmotionCardAbilityBase
  {
    public const int _addMin = 1;
    public const int _addMax = 2;
    public const int _redMin = 1;
    public const int _redMax = 2;

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      if (!behavior.IsParrying())
      {
        int num = RandomUtil.Range(1, 2);
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          power = -num
        });
      }
      else
      {
        int num = RandomUtil.Range(1, 2);
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          power = num
        });
        this._owner.battleCardResultLog?.SetCreatureAbilityEffect("1/Teddy_Heart");
        behavior.card.target?.battleCardResultLog?.SetCreatureEffectSound("Creature/Teddy_Guard");
      }
    }
  }
}
}
