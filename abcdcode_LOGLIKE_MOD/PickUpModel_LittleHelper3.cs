// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_LittleHelper3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_LittleHelper3 : CreaturePickUpModel
{
  public PickUpModel_LittleHelper3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_LittleHelper3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_LittleHelper3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_LittleHelper3_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370123), (EmotionCardAbilityBase) new PickUpModel_LittleHelper3.LogEmotionCardAbility_LittleHelper3(), model);
  }

  public class LogEmotionCardAbility_LittleHelper3 : EmotionCardAbilityBase
  {
    public const int _addMax = 3;

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      if (behavior == null || behavior.card?.target == null)
        return;
      int a = behavior.card.speedDiceResultValue - behavior.card.target.speedDiceResult[behavior.card.targetSlotOrder].value;
      if (a > 0 && this.IsAttackDice(behavior.Detail))
      {
        int num = Mathf.Min(a, 3);
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          power = num
        });
      }
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      BattleUnitModel target = behavior.card?.target;
      int? speedDiceResultValue = behavior.card?.speedDiceResultValue;
      int? nullable = target?.speedDiceResult[behavior.card.targetSlotOrder].value;
      if (speedDiceResultValue.GetValueOrDefault() > nullable.GetValueOrDefault() & speedDiceResultValue.HasValue & nullable.HasValue)
        target.battleCardResultLog?.SetCreatureAbilityEffect("2/Helper_Hit", 1.5f);
      if (behavior.Detail != BehaviourDetail.Slash || target == null)
        return;
      target.battleCardResultLog?.SetCreatureEffectSound("Creature / Helper_Atk");
    }
  }
}
}
