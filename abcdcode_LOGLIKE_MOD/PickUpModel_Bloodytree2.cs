// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Bloodytree2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using Sound;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Bloodytree2 : CreaturePickUpModel
{
  public PickUpModel_Bloodytree2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Bloodytree2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Bloodytree2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Bloodytree2_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370092), (EmotionCardAbilityBase) new PickUpModel_Bloodytree2.LogEmotionCardAbility_Bloodytree2(), model);
  }

  public class LogEmotionCardAbility_Bloodytree2 : EmotionCardAbilityBase
  {
    public const int _dmgAddMin = 2;
    public const int _dmgAddMax = 4;
    public const int _dmgRedMin = 3;
    public const int _dmgRedMax = 6;
    public const int _powAddMin = 1;
    public const int _powAddMax = 3;

    public int DmgAdd => RandomUtil.Range(2, 4);

    public int DmgRed => RandomUtil.Range(3, 6);

    public int PowAdd => RandomUtil.Range(1, 3);

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Eye", 1f, this._owner.view, this._owner.view);
      SoundEffectPlayer.PlaySound("Creature/MustSee_Wake_Storng");
    }

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Eye", 1f, this._owner.view, this._owner.view);
    }

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      if (!behavior.IsParrying())
      {
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          dmg = this.DmgAdd
        });
      }
      else
      {
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          dmg = -this.DmgRed
        });
        if (behavior.Detail != BehaviourDetail.Guard)
          return;
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          power = this.PowAdd
        });
      }
    }
  }
}
}
