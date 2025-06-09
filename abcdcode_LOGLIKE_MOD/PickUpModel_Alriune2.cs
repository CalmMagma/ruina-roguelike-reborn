// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Alriune2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Alriune2 : CreaturePickUpModel
{
  public PickUpModel_Alriune2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Alriune2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Alriune2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Alriune2_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370342), (EmotionCardAbilityBase) new PickUpModel_Alriune2.LogEmotionCardAbility_Alriune2(), model);
  }

  public class LogEmotionCardAbility_Alriune2 : EmotionCardAbilityBase
  {
    public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
    {
      base.OnTakeDamageByAttack(atkDice, dmg);
      BattleUnitModel owner = atkDice?.owner;
      if (owner == null)
        return;
      owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Alriune_Debuf, 1, this._owner);
      owner.battleCardResultLog?.SetCreatureAbilityEffect("0/Alriune_Stun_Effect", 1f);
      owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Ali_Guard");
    }
  }
}
}
