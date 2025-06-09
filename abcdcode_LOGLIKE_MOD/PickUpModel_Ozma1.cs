// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Ozma1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Ozma1 : CreaturePickUpModel
{
  public PickUpModel_Ozma1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Ozma1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Ozma1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Ozma1_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370371), (EmotionCardAbilityBase) new PickUpModel_Ozma1.LogEmotionCardAbility_Ozma1(), model);
  }

  public class LogEmotionCardAbility_Ozma1 : EmotionCardAbilityBase
  {
    public bool _effect;

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (this._effect)
        return;
      this._effect = true;
      SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Jack_Start", 1f, this._owner.view, this._owner.view, 2f);
      SoundEffectPlayer.PlaySound("Creature/Ozma_RealPumkin_GetCard");
    }

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      int num = RandomUtil.Range(3, 4);
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = num
      });
      if (!this.IsAttackDice(behavior.Detail))
      {
        if (!this.IsDefenseDice(behavior.Detail))
          return;
        this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Jack_G", 2f);
      }
      else
        this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Jack_P", 2f);
    }

    public override int GetCardCostAdder(BattleDiceCardModel card) => 1;
  }
}
}
