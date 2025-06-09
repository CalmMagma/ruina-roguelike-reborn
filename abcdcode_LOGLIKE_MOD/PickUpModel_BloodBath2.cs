// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BloodBath2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_BloodBath2 : CreaturePickUpModel
{
  public PickUpModel_BloodBath2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_BloodBath2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_BloodBath2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_BloodBath2_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370002), (EmotionCardAbilityBase) new PickUpModel_BloodBath2.LogEmotionCardAbility_bloodbath2(), model);
  }

  public class LogEmotionCardAbility_bloodbath2 : EmotionCardAbilityBase
  {
    public const float _prob = 0.2f;
    public const int _redMin = 2;
    public const int _redMax = 5;

    public static bool Prob => (double) RandomUtil.valueForProb < 0.20000000298023224;

    public static int Reduce => RandomUtil.Range(2, 5);

    public override int GetDamageReduction(BattleDiceBehavior behavior)
    {
      if (PickUpModel_BloodBath2.LogEmotionCardAbility_bloodbath2.Prob)
      {
        this._owner.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_Scar", 1f);
        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/BloodBath_Barrier");
        return 9999;
      }
      if (behavior.Detail != BehaviourDetail.Slash)
        return base.GetDamageReduction(behavior);
      int reduce = PickUpModel_BloodBath2.LogEmotionCardAbility_bloodbath2.Reduce;
      this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Sign, -reduce);
      this._owner.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_Scar", 1f);
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/BloodBath_Barrier");
      return reduce;
    }
  }
}
}
