// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Porccubus3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Porccubus3 : CreaturePickUpModel
{
  public PickUpModel_Porccubus3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Porccubus3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Porccubus3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Porccubus3_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370243), (EmotionCardAbilityBase) new PickUpModel_Porccubus3.LogEmotionCardAbility_Porccubus3(), model);
  }

  public class LogEmotionCardAbility_Porccubus3 : EmotionCardAbilityBase
  {
    public const int _recoverBpMin = 2;
    public const int _recoverBpMax = 4;

    public static int RecoverBP => RandomUtil.Range(2, 4);

    public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
    {
      this._owner.battleCardResultLog?.SetTakeDamagedEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Filter));
      if (this._owner.IsBreakLifeZero())
        return;
      int recoverBp = PickUpModel_Porccubus3.LogEmotionCardAbility_Porccubus3.RecoverBP;
      this._owner.battleCardResultLog?.SetEmotionAbility(false, this._emotionCard, 0, ResultOption.Default);
      this._owner.breakDetail.RecoverBreak(recoverBp);
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Porccu_Hit");
    }

    public void Filter()
    {
      new GameObject().AddComponent<SpriteFilter_Porccubus>().Init("EmotionCardFilter/Porccubus_Filter", false);
    }
  }
}
}
