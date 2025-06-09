// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ForsakenMurderer3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ForsakenMurderer3 : CreaturePickUpModel
{
  public PickUpModel_ForsakenMurderer3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ForsakenMurderer3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ForsakenMurderer3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ForsakenMurderer3_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370023), (EmotionCardAbilityBase) new PickUpModel_ForsakenMurderer3.LogEmotionCardAbility_ForsakenMurderer3(), model);
  }

  public class LogEmotionCardAbility_ForsakenMurderer3 : EmotionCardAbilityBase
  {
    public const int _min = -1;
    public const int _max = 3;

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      if (behavior == null)
        return;
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        min = -1,
        max = 3
      });
    }

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Murderer_Filter", false, 2f);
      SoundEffectPlayer.PlaySound("Creature/Abandoned_Angry");
    }
  }
}
}
