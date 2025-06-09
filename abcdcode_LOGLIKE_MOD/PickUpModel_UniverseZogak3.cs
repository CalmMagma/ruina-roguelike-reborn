// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_UniverseZogak3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Battle.CameraFilter;
using Sound;
using System;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_UniverseZogak3 : CreaturePickUpModel
{
  public PickUpModel_UniverseZogak3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_UniverseZogak3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_UniverseZogak3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_UniverseZogak3_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370043), (EmotionCardAbilityBase) new PickUpModel_UniverseZogak3.LogEmotionCardAbility_UniverseZogak3(), model);
  }

  public class LogEmotionCardAbility_UniverseZogak3 : EmotionCardAbilityBase
  {
    public const float _prob = 0.5f;
    public const float _probPerHit = 0.1f;
    public const int _weak = 1;
    public const int _disarm = 1;
    public const int _maxCnt = 3;
    public int cnt;
    public int hitCnt;
    public bool activated;

    public bool Prob()
    {
      return (double) RandomUtil.valueForProb < 0.5 + (double) this.hitCnt * 0.10000000149011612;
    }

    public override void OnStartOneSideAction(BattlePlayingCardDataInUnitModel curCard)
    {
      base.OnStartOneSideAction(curCard);
      this.activated = false;
      this.cnt = 0;
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      base.OnSucceedAttack(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null || behavior.IsParrying())
        return;
      if (this.Prob() && this.cnt < 3)
      {
        ++this.cnt;
        target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Weak, 1, this._owner);
        target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Disarm, 1, this._owner);
        if (!this.activated)
        {
          this._owner.battleCardResultLog.SetEndCardActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Effect));
          this.activated = true;
        }
      }
      ++this.hitCnt;
    }

    public void Effect()
    {
      try
      {
        SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Cosmos_Sing");
        if ((UnityEngine.Object) soundEffectPlayer != (UnityEngine.Object) null)
          soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
        BattleCamManager instance = SingletonBehavior<BattleCamManager>.Instance;
        if (!((UnityEngine.Object) instance != (UnityEngine.Object) null))
          return;
        instance.AddCameraFilter<CameraFilterCustom_universe>(true);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Camera Filter Adding Failed " + ex?.ToString()));
      }
    }
  }
}
}
