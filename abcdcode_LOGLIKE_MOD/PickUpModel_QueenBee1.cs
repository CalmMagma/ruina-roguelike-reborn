// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_QueenBee1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_QueenBee1 : CreaturePickUpModel
{
  public PickUpModel_QueenBee1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_QueenBee1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_QueenBee1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_QueenBee1_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370311), (EmotionCardAbilityBase) new PickUpModel_QueenBee1.LogEmotionCardAbility_QueenBee1(), model);
  }

  public class LogEmotionCardAbility_QueenBee1 : EmotionCardAbilityBase
  {
    public const int _bleedMin = 1;
    public const int _bleedMax = 3;
    public const int _burnMin = 1;
    public const int _burnMax = 3;

    public static int Bleed => RandomUtil.Range(1, 3);

    public static int Burn => RandomUtil.Range(1, 3);

    public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
    {
      base.OnTakeDamageByAttack(atkDice, dmg);
      BattleUnitModel owner = atkDice.owner;
      if (owner == null)
        return;
      owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, PickUpModel_QueenBee1.LogEmotionCardAbility_QueenBee1.Bleed, this._owner);
      owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, PickUpModel_QueenBee1.LogEmotionCardAbility_QueenBee1.Burn, this._owner);
      owner.battleCardResultLog?.SetCreatureEffectSound("Creature/QueenBee_Funga");
      this._owner.battleCardResultLog?.SetCreatureAbilityEffect("1/Queenbee_Spore", 2f);
    }
  }
}
}
