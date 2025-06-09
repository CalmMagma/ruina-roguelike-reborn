// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_UniverseZogak1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System.Collections;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_UniverseZogak1 : CreaturePickUpModel
{
  public PickUpModel_UniverseZogak1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_UniverseZogak1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_UniverseZogak1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_UniverseZogak1_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370041), (EmotionCardAbilityBase) new PickUpModel_UniverseZogak1.LogEmotionCardAbility_UniverseZogak1(), model, true);
  }

  public class LogEmotionCardAbility_UniverseZogak1 : EmotionCardAbilityBase
  {
    public const int _brkDmgMin = 5;
    public const int _brkDmgMax = 10;
    public const int _recoverBpMin = 5;
    public const int _recoverBpMax = 10;

    public static int BrkDmg => RandomUtil.Range(5, 10);

    public static int RecoverBP => RandomUtil.Range(5, 10);

    public override void OnSelectEmotionOnce()
    {
      base.OnSelectEmotionOnce();
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
      {
        if (alive.faction != this._owner.faction)
          alive.TakeBreakDamage(PickUpModel_UniverseZogak1.LogEmotionCardAbility_UniverseZogak1.BrkDmg, DamageType.Emotion, this._owner);
        else
          alive.breakDetail.RecoverBreak(PickUpModel_UniverseZogak1.LogEmotionCardAbility_UniverseZogak1.RecoverBP);
      }
      Camera effectCam = SingletonBehavior<BattleCamManager>.Instance.EffectCam;
      CameraFilterPack_Distortion_Dream2 r = effectCam.GetComponent<CameraFilterPack_Distortion_Dream2>();
      if ((Object) r == (Object) null)
        r = effectCam.gameObject.AddComponent<CameraFilterPack_Distortion_Dream2>();
      BattleCamManager instance = SingletonBehavior<BattleCamManager>.Instance;
      if ((Object) instance != (Object) null)
        instance.StartCoroutine(this.DistortionRoutine(r));
      SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Cosmos_Sing");
      if ((Object) soundEffectPlayer == (Object) null)
        return;
      soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
    }

    public IEnumerator DistortionRoutine(CameraFilterPack_Distortion_Dream2 r)
    {
      float e = 0.0f;
      float amount = Random.Range(20f, 30f);
      int speed = 15;
      while ((double) e < 1.0)
      {
        e += Time.deltaTime * 2f;
        r.Distortion = Mathf.Lerp(amount, 0.0f, e);
        r.Speed = Mathf.Lerp((float) speed, 0.0f, e);
        yield return (object) null;
      }
      Object.Destroy((Object) r);
    }
  }
}
}
