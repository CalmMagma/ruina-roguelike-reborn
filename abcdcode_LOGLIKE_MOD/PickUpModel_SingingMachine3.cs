// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_SingingMachine3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_SingingMachine3 : CreaturePickUpModel
{
  public PickUpModel_SingingMachine3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_SingingMachine3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_SingingMachine3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_SingingMachine3_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370223), (EmotionCardAbilityBase) new PickUpModel_SingingMachine3.LogEmotionCardAbility_SingingMachine3(), model);
  }

  public class LogEmotionCardAbility_SingingMachine3 : EmotionCardAbilityBase
  {
    public const int _addMin = 1;
    public const int _addMax = 3;

    public static int Add => RandomUtil.Range(1, 3);

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/SingingMachine_Filter_Aura", false, 2f);
      SoundEffectPlayer.PlaySound("Creature/SingingMachine_Open");
    }

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      if (this.IsAttackDice(behavior.Detail))
      {
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          power = PickUpModel_SingingMachine3.LogEmotionCardAbility_SingingMachine3.Add
        });
      }
      else
      {
        if (!this.IsDefenseDice(behavior.Detail))
          return;
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          max = -300
        });
      }
    }
  }
}
}
