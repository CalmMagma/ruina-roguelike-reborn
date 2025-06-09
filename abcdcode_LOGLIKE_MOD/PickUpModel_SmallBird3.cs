// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_SmallBird3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_SmallBird3 : CreaturePickUpModel
{
  public PickUpModel_SmallBird3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_SmallBird3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_SmallBird3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_SmallBird3_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370183), (EmotionCardAbilityBase) new PickUpModel_SmallBird3.LogEmotionCardAbility_SmallBird3(), model);
  }

  public class LogEmotionCardAbility_SmallBird3 : EmotionCardAbilityBase
  {
    public const int _evadePowMin = 1;
    public const int _evadePowMax = 2;
    public const int _powByEvadeMin = 1;
    public const int _powByEvadeMax = 2;

    public static int EvadePow => RandomUtil.Range(1, 2);

    public static int PowByEvade => RandomUtil.Range(1, 2);

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      if (behavior.Detail != BehaviourDetail.Evasion)
        return;
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = PickUpModel_SmallBird3.LogEmotionCardAbility_SmallBird3.EvadePow
      });
    }

    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      base.OnWinParrying(behavior);
      if (behavior.Detail != BehaviourDetail.Evasion)
        return;
      BattlePlayingCardDataInUnitModel card = behavior.card;
      if (card != null)
        card.ApplyDiceStatBonus(DiceMatch.NextAttackDice, new DiceStatBonus()
        {
          power = PickUpModel_SmallBird3.LogEmotionCardAbility_SmallBird3.PowByEvade
        });
      this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Feather", 3f);
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Smallbird_Wing");
    }
  }
}
}
