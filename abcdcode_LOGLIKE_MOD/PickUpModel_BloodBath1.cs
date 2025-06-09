// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BloodBath1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_BloodBath1 : CreaturePickUpModel
{
  public PickUpModel_BloodBath1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_BloodBath1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_BloodBath1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_BloodBath1_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370001), (EmotionCardAbilityBase) new PickUpModel_BloodBath1.LogEmotionCardAbility_bloodbath1(), model);
  }

  public class LogEmotionCardAbility_bloodbath1 : EmotionCardAbilityBase
  {
    public Battle.CreatureEffect.CreatureEffect effect;

    public static int Pow => RandomUtil.Range(1, 2);

    public static int BrDmg => RandomUtil.Range(3, 5);

    public override void OnSelectEmotion()
    {
      this.effect = this.MakeEffect("0/BloodyBath_Blood");
      if ((Object) this.effect != (Object) null)
        this.effect.transform.SetParent(this._owner.view.characterRotationCenter.transform.parent);
      SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/BloodBath_Water");
      if ((Object) soundEffectPlayer == (Object) null)
        return;
      soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
    }

    public override int GetBreakDamageReduction(BattleDiceBehavior behavior)
    {
      int brDmg = PickUpModel_BloodBath1.LogEmotionCardAbility_bloodbath1.BrDmg;
      this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Sign, brDmg);
      return -brDmg;
    }

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      if (!this.IsDefenseDice(behavior.Detail))
        return;
      int pow = PickUpModel_BloodBath1.LogEmotionCardAbility_bloodbath1.Pow;
      this._owner.battleCardResultLog?.SetEmotionAbility(false, this._emotionCard, 1, ResultOption.Default);
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = pow
      });
    }

    public override void OnLayerChanged(string layerName)
    {
      if (layerName == "Character")
        layerName = "CharacterUI";
      if (!((Object) this.effect != (Object) null))
        return;
      this.effect.SetLayer(layerName);
    }
  }
}
}
