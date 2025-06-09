// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Pinocchio3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Pinocchio3 : CreaturePickUpModel
{
  public PickUpModel_Pinocchio3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Pinocchio3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Pinocchio3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Pinocchio3_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370203), (EmotionCardAbilityBase) new PickUpModel_Pinocchio3.LogEmotionCardAbility_Pinocchio3(), model);
  }

  public class LogEmotionCardAbility_Pinocchio3 : EmotionCardAbilityBase
  {
    public const int _draw = 4;

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Pino_Success");
      if ((Object) soundEffectPlayer == (Object) null)
        return;
      soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
    }

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      this._owner.allyCardDetail.ReturnAllToDeck();
      this._owner.allyCardDetail.DrawCards(4);
      this.MakeEffect("0/Pinocchio_Curiosity", destroyTime: 3f);
    }
  }
}
}
