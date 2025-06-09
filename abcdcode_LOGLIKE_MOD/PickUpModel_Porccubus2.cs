// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Porccubus2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Porccubus2 : CreaturePickUpModel
{
  public PickUpModel_Porccubus2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Porccubus2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Porccubus2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Porccubus2_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370242), (EmotionCardAbilityBase) new PickUpModel_Porccubus2.LogEmotionCardAbility_Porccubus2(), model);
  }

  public class LogEmotionCardAbility_Porccubus2 : EmotionCardAbilityBase
  {
    public override StatBonus GetStatBonus()
    {
      return new StatBonus() { breakRate = -50 };
    }

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      if (behavior == null)
        return;
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = 1
      });
    }

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this.Filter();
    }

    public void Filter()
    {
      new GameObject().AddComponent<SpriteFilter_Porccubus>().Init("EmotionCardFilter/Porccubus_Filter", false);
      SoundEffectPlayer.PlaySound("Creature/Porccu_Nodmg");
    }
  }
}
}
