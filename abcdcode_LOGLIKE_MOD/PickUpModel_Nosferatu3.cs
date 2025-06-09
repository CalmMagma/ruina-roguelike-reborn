// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Nosferatu3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Nosferatu3 : CreaturePickUpModel
{
  public PickUpModel_Nosferatu3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Nosferatu3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Nosferatu3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Nosferatu3_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370363), (EmotionCardAbilityBase) new PickUpModel_Nosferatu3.LogEmotionCardAbility_Nosferatu3(), model);
  }

  public class LogEmotionCardAbility_Nosferatu3 : EmotionCardAbilityBase
  {
    public const int _dmgMin = 2;
    public const int _dmgMax = 4;
    public const int _healMin = 2;
    public const int _healMax = 5;

    public int Dmg => RandomUtil.Range(2, 4);

    public int Heal => RandomUtil.Range(2, 5);

    public override void OnRollDice(BattleDiceBehavior behavior)
    {
      base.OnRollDice(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null || target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) <= 0)
        return;
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        dmg = this.Dmg
      });
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      base.OnSucceedAttack(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null || target.bufListDetail.GetKewordBufStack(KeywordBuf.Bleeding) <= 0)
        return;
      target.battleCardResultLog?.SetCreatureAbilityEffect("6/Nosferatu_Emotion_Bat", 3f);
      target.battleCardResultLog?.SetCreatureEffectSound("Nosferatu_Atk_Bat");
      this._owner.RecoverHP(this.Heal);
    }
  }
}
}
