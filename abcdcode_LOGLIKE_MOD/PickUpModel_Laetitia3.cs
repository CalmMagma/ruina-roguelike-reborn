// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Laetitia3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Laetitia3 : CreaturePickUpModel
{
  public PickUpModel_Laetitia3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Laetitia3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Laetitia3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Laetitia3_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370333), (EmotionCardAbilityBase) new PickUpModel_Laetitia3.LogEmotionCardAbility_Laetitia3(), model);
  }

  public class LogEmotionCardAbility_Laetitia3 : EmotionCardAbilityBase
  {
    public const int _dmgMin = 2;
    public const int _dmgMax = 4;
    public const float _prob = 0.5f;

    public static int Dmg => RandomUtil.Range(2, 4);

    public static bool Prob => (double) RandomUtil.valueForProb < 0.5;

    public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
    {
      base.ChangeDiceResult(behavior, ref diceResult);
      if (behavior.Index != 0)
        return;
      if (PickUpModel_Laetitia3.LogEmotionCardAbility_Laetitia3.Prob)
      {
        diceResult = behavior.GetDiceMax();
      }
      else
      {
        diceResult = 1;
        this._owner.battleCardResultLog?.SetCreatureAbilityEffect("3/Latitia_Boom", 1.5f);
        this._owner.TakeDamage(PickUpModel_Laetitia3.LogEmotionCardAbility_Laetitia3.Dmg, DamageType.Emotion, this._owner);
      }
    }
  }
}
}
