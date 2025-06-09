// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_LongBird1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_LongBird1 : CreaturePickUpModel
{
  public PickUpModel_LongBird1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_LongBird1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_LongBird1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_LongBird1_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370281), (EmotionCardAbilityBase) new PickUpModel_LongBird1.LogEmotionCardAbility_LongBird1(), model);
  }

  public class LogEmotionCardAbility_LongBird1 : EmotionCardAbilityBase
  {
    public const int _dmgMin = 2;
    public const int _dmgMax = 7;
    public const int _enduMin = 1;
    public const int _enduMax = 2;
    public const int _strMin = 1;
    public const int _strMax = 2;

    public int Dmg => RandomUtil.Range(2, 7);

    public int Endu => RandomUtil.Range(1, 2);

    public int Str => RandomUtil.Range(1, 2);

    public override void OnRollDice(BattleDiceBehavior behavior)
    {
      base.OnRollDice(behavior);
      if (behavior.DiceVanillaValue > behavior.GetDiceMin())
        return;
      this._owner.TakeDamage(this.Dmg, DamageType.Emotion, this._owner);
      this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, this.Endu, this._owner);
      this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, this.Str, this._owner);
      this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Judgement", 3f);
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/LongBird_Stun");
    }
  }
}
}
