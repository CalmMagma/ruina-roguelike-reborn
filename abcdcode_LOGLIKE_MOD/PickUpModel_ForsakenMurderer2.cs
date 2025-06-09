// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ForsakenMurderer2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ForsakenMurderer2 : CreaturePickUpModel
{
  public PickUpModel_ForsakenMurderer2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ForsakenMurderer2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ForsakenMurderer2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ForsakenMurderer2_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370022), (EmotionCardAbilityBase) new PickUpModel_ForsakenMurderer2.LogEmotionCardAbility_ForsakenMurderer2(), model);
  }

  public class LogEmotionCardAbility_ForsakenMurderer2 : EmotionCardAbilityBase
  {
    public override int GetSpeedDiceAdder() => -100;

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      if (behavior.Detail != BehaviourDetail.Hit)
        return;
      int num = RandomUtil.Range(1, 3);
      this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Sign, num);
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = num
      });
    }

    public override void OnSelectEmotion()
    {
      SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Abandoned_Breathe");
      if ((Object) soundEffectPlayer != (Object) null)
        soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
      this._owner.view.charAppearance.SetTemporaryGift("Gift_AbandonedMurder", GiftPosition.Mouth);
    }
  }
}
}
