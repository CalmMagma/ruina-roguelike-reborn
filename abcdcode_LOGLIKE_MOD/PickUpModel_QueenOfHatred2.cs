// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_QueenOfHatred2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_QueenOfHatred2 : CreaturePickUpModel
{
  public PickUpModel_QueenOfHatred2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_QueenOfHatred2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_QueenOfHatred2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_QueenOfHatred2_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370052), (EmotionCardAbilityBase) new PickUpModel_QueenOfHatred2.LogEmotionCardAbility_QueenOfHatred2(), model);
  }

  public class LogEmotionCardAbility_QueenOfHatred2 : EmotionCardAbilityBase
  {
    public const int _dmgMin = 3;
    public const int _dmgMax = 5;
    public BattleUnitModel target;
    public Battle.CreatureEffect.CreatureEffect effect;
    public int max;

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      BattleUnitModel battleUnitModel = (BattleUnitModel) null;
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
      {
        int damageToEnemyAtRound = alive.history.damageToEnemyAtRound;
        if (damageToEnemyAtRound > this.max)
        {
          battleUnitModel = alive;
          this.max = damageToEnemyAtRound;
        }
      }
      this.target = battleUnitModel;
      this.max = 0;
      if (!((Object) this.effect != (Object) null))
        return;
      Object.Destroy((Object) this.effect.gameObject);
      this.effect = (Battle.CreatureEffect.CreatureEffect) null;
    }

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (this.target == null || this.target.IsDead())
        return;
      this.effect = this.MakeEffect("5/MagicalGirl_Villain", target: this.target);
    }

    public override void BeforeGiveDamage(BattleDiceBehavior behavior)
    {
      base.BeforeGiveDamage(behavior);
      if (this.target != behavior.card?.target)
        return;
      int num = RandomUtil.Range(3, 5);
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        dmg = num
      });
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MagicalGirl_Gun");
      this._owner.battleCardResultLog.SetAttackEffectFilter(typeof (ImageFilter_ColorBlend_Pink));
    }
  }
}
}
