// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_SpiderBud2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_SpiderBud2 : CreaturePickUpModel
{
  public PickUpModel_SpiderBud2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_SpiderBud2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_SpiderBud2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_SpiderBud2_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370232), (EmotionCardAbilityBase) new PickUpModel_SpiderBud2.LogEmotionCardAbility_SpiderBud2(), model);
  }

  public class LogEmotionCardAbility_SpiderBud2 : EmotionCardAbilityBase
  {
    public const int _healMin = 2;
    public const int _healMax = 4;
    public bool breaked;

    public static int Heal => RandomUtil.Range(2, 4);

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      this.breaked = false;
      if (behavior == null)
        return;
      bool? nullable = behavior.card?.target?.IsBreakLifeZero();
      bool flag = false;
      if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
        this.breaked = true;
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      if (behavior == null || behavior.card?.target == null || !behavior.card.target.IsBreakLifeZero() || this.breaked)
        return;
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
        alive.RecoverHP(PickUpModel_SpiderBud2.LogEmotionCardAbility_SpiderBud2.Heal);
      string resPath = "";
      switch (behavior.behaviourInCard.MotionDetail)
      {
        case MotionDetail.H:
          resPath = "3/SpiderBud_Spiders_H";
          break;
        case MotionDetail.J:
          resPath = "3/SpiderBud_Spiders_J";
          break;
        case MotionDetail.Z:
          resPath = "3/SpiderBud_Spiders_Z";
          break;
      }
      if (!string.IsNullOrEmpty(resPath))
        this._owner.battleCardResultLog.SetCreatureAbilityEffect(resPath, 1f);
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Spider_gochiDown");
    }
  }
}
}
