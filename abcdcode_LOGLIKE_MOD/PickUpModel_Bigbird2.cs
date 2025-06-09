// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Bigbird2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Bigbird2 : CreaturePickUpModel
{
  public PickUpModel_Bigbird2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Bigbird2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Bigbird2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Bigbird2_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370082), (EmotionCardAbilityBase) new PickUpModel_Bigbird2.LogEmotionCardAbility_Bigbird2(), model);
  }

  public class LogEmotionCardAbility_Bigbird2 : EmotionCardAbilityBase
  {
    public override bool CanForcelyAggro() => true;

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_Lamp", 1f, this._owner.view, this._owner.view, 3f);
      SoundEffectPlayer.PlaySound("Creature/Bigbird_Attract");
    }
  }
}
}
