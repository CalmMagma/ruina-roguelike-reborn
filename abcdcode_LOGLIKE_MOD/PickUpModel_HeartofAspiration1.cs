// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_HeartofAspiration1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System.Collections;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_HeartofAspiration1 : CreaturePickUpModel
{
  public PickUpModel_HeartofAspiration1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_HeartofAspiration1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_HeartofAspiration1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_HeartofAspiration1_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp()
  {
    base.OnPickUp();
    SoundEffectPlayer.PlaySound("Creature/Heartbeat");
  }

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370101), (EmotionCardAbilityBase) new PickUpModel_HeartofAspiration1.LogEmotionCardAbility_HeartofAspiration1(), model);
  }

  public class LogEmotionCardAbility_HeartofAspiration1 : EmotionCardAbilityBase
  {
    public Battle.CreatureEffect.CreatureEffect _heartBeatEffect;

    public override void OnSelectEmotion()
    {
      BattleCamManager instance1 = SingletonBehavior<BattleCamManager>.Instance;
      CameraFilterPack_Blur_Radial r = (Object) instance1 != (Object) null ? instance1.AddCameraFilter<CameraFilterPack_Blur_Radial>() : (CameraFilterPack_Blur_Radial) null;
      BattleCamManager instance2 = SingletonBehavior<BattleCamManager>.Instance;
      if ((Object) instance2 == (Object) null)
        return;
      instance2.StartCoroutine(this.Pinpong(r));
    }

    public override void OnSelectEmotionOnce()
    {
      base.OnSelectEmotionOnce();
      SoundEffectPlayer.PlaySound("Creature/Heartbeat");
    }

    public IEnumerator Pinpong(CameraFilterPack_Blur_Radial r)
    {
      float elapsedTime = 0.0f;
      while ((double) elapsedTime < 1.0)
      {
        elapsedTime += Time.deltaTime;
        r.Intensity = Mathf.PingPong(Time.time, 0.05f);
        yield return (object) new WaitForEndOfFrame();
      }
      BattleCamManager instance = SingletonBehavior<BattleCamManager>.Instance;
      if ((Object) instance != (Object) null)
        instance.RemoveCameraFilter<CameraFilterPack_Blur_Radial>();
    }

    public override void OnRoundEnd()
    {
      this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, RandomUtil.Range(1, 2), this._owner);
      if (this._owner.history.damageAtOneRound > 0)
        return;
      this._owner.LoseHp(Mathf.Min(this._owner.MaxHp / 4, 30));
    }
  }
}
}
