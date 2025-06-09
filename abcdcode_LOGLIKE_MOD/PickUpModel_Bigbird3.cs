// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Bigbird3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Bigbird3 : CreaturePickUpModel
{
  public PickUpModel_Bigbird3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Bigbird3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Bigbird3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Bigbird3_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370083), (EmotionCardAbilityBase) new PickUpModel_Bigbird3.LogEmotionCardAbility_Bigbird3(), model);
  }

  public class LogEmotionCardAbility_Bigbird3 : EmotionCardAbilityBase
  {
    public const int _pow = 1;
    public const float _hpRate = 0.25f;
    public Battle.CreatureEffect.CreatureEffect _aura;

    public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
    {
      base.OnParryingStart(card);
      this.DestroyAura();
      BattleUnitModel target = card?.target;
      if (target == null || target.speedDiceResult[card.targetSlotOrder].value <= card.speedDiceResultValue)
        return;
      this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_See_Red", 1f, this._owner.view, this._owner.view);
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Bigbird_MouseOpen");
      this._owner.battleCardResultLog?.SetEndCardActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.DestroyAura));
      card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
      {
        power = 1
      });
    }

    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      base.OnWinParrying(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null || (double) target.hp > (double) target.MaxHp * 0.25)
        return;
      target.currentDiceAction?.DestroyDice(DiceMatch.AllDice);
      target.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_DiceBroken", 3f);
      target.battleCardResultLog?.SetCreatureEffectSound("Creature/Bigbird_HeadCut");
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      this.DestroyAura();
    }

    public void DestroyAura()
    {
      if (!((Object) this._aura != (Object) null))
        return;
      Object.Destroy((Object) this._aura.gameObject);
      this._aura = (Battle.CreatureEffect.CreatureEffect) null;
    }
  }
}
}
