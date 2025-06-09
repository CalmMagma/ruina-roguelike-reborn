// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ForsakenMurderer1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ForsakenMurderer1 : CreaturePickUpModel
{
  public PickUpModel_ForsakenMurderer1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ForsakenMurderer1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ForsakenMurderer1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ForsakenMurderer1_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370021), (EmotionCardAbilityBase) new PickUpModel_ForsakenMurderer1.LogEmotionCardAbility_ForsakenMurderer1(), model);
  }

  public class LogEmotionCardAbility_ForsakenMurderer1 : EmotionCardAbilityBase
  {
    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      if (behavior.Detail != BehaviourDetail.Hit)
        return;
      BattleUnitModel target = behavior.card?.target;
      if (target == null)
        return;
      target.bufListDetail?.AddKeywordBufByEtc(KeywordBuf.Binding, 1, this._owner);
      target.bufListDetail?.AddKeywordBufByEtc(KeywordBuf.Paralysis, 1, this._owner);
      target.battleCardResultLog?.SetCreatureAbilityEffect("2/AbandonedMurder_Hit", 1f);
      target.battleCardResultLog?.SetCreatureEffectSound("Creature/Abandoned_Atk");
    }
  }
}
}
