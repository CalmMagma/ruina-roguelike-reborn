// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_SpiderBud1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_SpiderBud1 : CreaturePickUpModel
{
  public PickUpModel_SpiderBud1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_SpiderBud1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_SpiderBud1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_SpiderBud1_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370231), (EmotionCardAbilityBase) new PickUpModel_SpiderBud1.LogEmotionCardAbility_SpiderBud1(), model);
  }

  public class LogEmotionCardAbility_SpiderBud1 : EmotionCardAbilityBase
  {
    public const int _valueMin = 1;
    public const int _valueMax = 2;

    public int Value => RandomUtil.Range(1, 2);

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      if (behavior.DiceVanillaValue != behavior.GetDiceMax())
        return;
      BattleUnitModel target = behavior.card.target;
      if (target == null)
        return;
      target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Paralysis, this.Value, this._owner);
      target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, this.Value, this._owner);
      target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, this.Value, this._owner);
      behavior.card.target?.battleCardResultLog?.SetCreatureAbilityEffect("3/Spider_Cocoon", 2f);
    }
  }
}
}
