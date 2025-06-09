// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Greed1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Greed1 : CreaturePickUpModel
{
  public PickUpModel_Greed1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Greed1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Greed1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Greed1_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370251), (EmotionCardAbilityBase) new PickUpModel_Greed1.LogEmotionCardAbility_Greed1(), model);
  }

  public class LogEmotionCardAbility_Greed1 : EmotionCardAbilityBase
  {
    public const float _prob = 0.25f;

    public static bool Prob => (double) RandomUtil.valueForProb < 0.25;

    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      base.OnWinParrying(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null || behavior == null || behavior.Index != 0 || !PickUpModel_Greed1.LogEmotionCardAbility_Greed1.Prob)
        return;
      target.currentDiceAction?.DestroyDice(DiceMatch.AllDice);
      this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Filter));
    }

    public void Filter()
    {
      new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KingOfGreed_Yellow", false);
      SoundEffectPlayer.PlaySound("Creature/Greed_StrongAtk_Defensed");
    }
  }
}
}
