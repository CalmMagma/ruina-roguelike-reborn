// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_FairyCarnival2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_FairyCarnival2 : CreaturePickUpModel
{
  public PickUpModel_FairyCarnival2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_FairyCarnival2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_FairyCarnival2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_FairyCarnival2_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370212), (EmotionCardAbilityBase) new PickUpModel_FairyCarnival2.LogEmotionCardAbility_FairyCarnival2(), model);
  }

  public class LogEmotionCardAbility_FairyCarnival2 : EmotionCardAbilityBase
  {
    public const int _addMin = 1;
    public const int _addMax = 3;
    public const int _recoverMin = 2;
    public const int _recoverMax = 5;

    public override void BeforeGiveDamage(BattleDiceBehavior behavior)
    {
      base.BeforeGiveDamage(behavior);
      BattleUnitModel target = behavior.card.target;
      if (target == null || target.history.takeDamageAtOneRound <= 0)
        return;
      int num = RandomUtil.Range(1, 3);
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        dmg = num
      });
      target.battleCardResultLog?.SetCreatureAbilityEffect("1/Fairy_Gluttony2");
      this._owner.RecoverHP(RandomUtil.Range(2, 5));
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Fairy_Dead");
    }
  }
}
}
