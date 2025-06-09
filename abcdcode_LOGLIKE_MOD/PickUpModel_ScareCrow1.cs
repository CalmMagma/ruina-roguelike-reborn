// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ScareCrow1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ScareCrow1 : CreaturePickUpModel
{
  public PickUpModel_ScareCrow1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ScareCrow1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ScareCrow1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ScareCrow1_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370071), (EmotionCardAbilityBase) new PickUpModel_ScareCrow1.LogEmotionCardAbility_ScareCrow1(), model);
  }

  public class LogEmotionCardAbility_ScareCrow1 : EmotionCardAbilityBase
  {
    public const int _discard = 1;
    public const int _ppMin = 1;
    public const int _ppMax = 1;
    public const float _filterTime = 1.5f;
    public const float _filterX = 0.5f;
    public const float _filterY = 0.5f;
    public const float _filterSpeed = 1.2f;

    public int RecoverPP => RandomUtil.Range(1, 1);

    public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
    {
      base.OnUseCard(curCard);
      if (this._owner.allyCardDetail.GetHand().Count == 0)
        return;
      this._owner.allyCardDetail.DiscardACardRandomlyByAbility(1);
      this._owner.cardSlotDetail.RecoverPlayPoint(this.RecoverPP);
      this._owner.battleCardResultLog?.SetEndCardActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.PrintSound));
    }

    public void PrintSound()
    {
      SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Scarecrow_Special");
    }

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this.SetFilter();
    }

    public void SetFilter()
    {
      GameObject gameObject = SingletonBehavior<BattleCamManager>.Instance.EffectCam.gameObject;
      if (!((Object) gameObject != (Object) null))
        return;
      CameraFilterPack_Distortion_ShockWave distortionShockWave = gameObject.AddComponent<CameraFilterPack_Distortion_ShockWave>();
      distortionShockWave.PosX = 0.5f;
      distortionShockWave.PosY = 0.5f;
      distortionShockWave.Speed = 1.2f;
      BattleCamManager instance = SingletonBehavior<BattleCamManager>.Instance;
      AutoScriptDestruct autoScriptDestruct = ((Object) instance != (Object) null ? instance.EffectCam.gameObject.AddComponent<AutoScriptDestruct>() : (AutoScriptDestruct) null) ?? (AutoScriptDestruct) null;
      if ((Object) autoScriptDestruct != (Object) null)
      {
        autoScriptDestruct.targetScript = (MonoBehaviour) distortionShockWave;
        autoScriptDestruct.time = 1.5f;
      }
    }
  }
}
}
