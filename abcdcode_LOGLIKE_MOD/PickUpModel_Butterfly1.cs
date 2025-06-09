// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Butterfly1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Butterfly1 : CreaturePickUpModel
{
  public PickUpModel_Butterfly1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Butterfly1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Butterfly1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Butterfly1_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370321), (EmotionCardAbilityBase) new PickUpModel_Butterfly1.LogEmotionCardAbility_Butterfly1(), model);
  }

  public class LogEmotionCardAbility_Butterfly1 : EmotionCardAbilityBase
  {
    public const int _dmgMin = 2;
    public const int _dmgMax = 7;
    public const int _bDmgMin = 2;
    public const int _bDmgMax = 5;
    public bool trigger;

    public static int Dmg => Random.Range(2, 7);

    public static int BDmg => Random.Range(2, 5);

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      this.trigger = false;
      if (!this.IsAttackDice(behavior.Detail))
        return;
      BattleUnitModel target = behavior?.card?.target;
      if (target == null || !this.IsAttackDice(behavior.Detail))
        return;
      float num = -1f;
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
      {
        if ((double) alive.hp > 0.0 && ((double) num < 0.0 || (double) alive.hp < (double) num))
          num = alive.hp;
      }
      if ((double) target.hp <= (double) num)
      {
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          dmg = PickUpModel_Butterfly1.LogEmotionCardAbility_Butterfly1.Dmg,
          breakDmg = PickUpModel_Butterfly1.LogEmotionCardAbility_Butterfly1.BDmg
        });
        this.trigger = true;
      }
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      base.OnSucceedAttack(behavior);
      if (!this.trigger)
        return;
      behavior?.card?.target?.battleCardResultLog?.SetCreatureAbilityEffect("2/ButterflyEffect_Black", 1f);
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/ButterFlyMan_Atk_Black");
      this.trigger = false;
    }
  }
}
}
