// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Mountain3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Mountain3 : CreaturePickUpModel
{
  public PickUpModel_Mountain3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Mountain3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Mountain3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Mountain3_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370263), (EmotionCardAbilityBase) new PickUpModel_Mountain3.LogEmotionCardAbility_Mountain3(), model);
  }

  public class LogEmotionCardAbility_Mountain3 : EmotionCardAbilityBase
  {
    public int stack;
    public bool _effect;

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this._owner.faction);
      aliveList.Remove(this._owner);
      this.stack = aliveList.Count;
      if (this.stack > 0)
      {
        foreach (BattleUnitModel battleUnitModel in aliveList)
          battleUnitModel.Die();
        this._owner.cardSlotDetail.SetRecoverPoint(this._owner.cardSlotDetail.GetRecoverPlayPoint() + this.stack);
      }
      Singleton<StageController>.Instance.GetStageModel().danggoUsed = true;
    }

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this._owner.cardSlotDetail.SetRecoverPoint(this._owner.cardSlotDetail.GetRecoverPlayPoint() + this.stack);
      this.MakeEffect("6/Dango_Emotion_Spread", target: this._owner);
    }

    public override int MaxPlayPointAdder() => this.stack;

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (!this._effect)
      {
        this._effect = true;
        CameraFilterUtil.EarthQuake(0.18f, 0.16f, 90f, 0.45f);
        Battle.CreatureEffect.CreatureEffect original1 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/6/Dango_Emotion_Effect");
        if ((Object) original1 != (Object) null)
        {
          Battle.CreatureEffect.CreatureEffect creatureEffect1 = Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
          Battle.CreatureEffect.CreatureEffect creatureEffect2 = Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
          Battle.CreatureEffect.CreatureEffect creatureEffect3 = Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original1, SingletonBehavior<BattleSceneRoot>.Instance.transform);
          if (((Object) creatureEffect1 != (Object) null ? (Object) creatureEffect1.gameObject.GetComponent<AutoDestruct>() : (Object) null) == (Object) null)
          {
            AutoDestruct autoDestruct = (Object) creatureEffect1 != (Object) null ? creatureEffect1.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct) null;
            if ((Object) autoDestruct != (Object) null)
            {
              autoDestruct.time = 3f;
              autoDestruct.DestroyWhenDisable();
            }
          }
          if (((Object) creatureEffect2 != (Object) null ? (Object) creatureEffect2.gameObject.GetComponent<AutoDestruct>() : (Object) null) == (Object) null)
          {
            AutoDestruct autoDestruct = (Object) creatureEffect2 != (Object) null ? creatureEffect2.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct) null;
            if ((Object) autoDestruct != (Object) null)
            {
              autoDestruct.time = 3f;
              autoDestruct.DestroyWhenDisable();
            }
          }
          if (((Object) creatureEffect3 != (Object) null ? (Object) creatureEffect3.gameObject.GetComponent<AutoDestruct>() : (Object) null) == (Object) null)
          {
            AutoDestruct autoDestruct = (Object) creatureEffect3 != (Object) null ? creatureEffect3.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct) null;
            if ((Object) autoDestruct != (Object) null)
            {
              autoDestruct.time = 3f;
              autoDestruct.DestroyWhenDisable();
            }
          }
        }
        Battle.CreatureEffect.CreatureEffect original2 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/7/Lumberjack_final_blood_1st");
        if ((Object) original2 != (Object) null)
        {
          Battle.CreatureEffect.CreatureEffect creatureEffect4 = Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
          Battle.CreatureEffect.CreatureEffect creatureEffect5 = Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
          Battle.CreatureEffect.CreatureEffect creatureEffect6 = Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original2, SingletonBehavior<BattleSceneRoot>.Instance.transform);
          if (((Object) creatureEffect4 != (Object) null ? (Object) creatureEffect4.gameObject.GetComponent<AutoDestruct>() : (Object) null) == (Object) null)
          {
            AutoDestruct autoDestruct = (Object) creatureEffect4 != (Object) null ? creatureEffect4.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct) null;
            if ((Object) autoDestruct != (Object) null)
            {
              autoDestruct.time = 3f;
              autoDestruct.DestroyWhenDisable();
            }
          }
          if (((Object) creatureEffect5 != (Object) null ? (Object) creatureEffect5.gameObject.GetComponent<AutoDestruct>() : (Object) null) == (Object) null)
          {
            AutoDestruct autoDestruct = (Object) creatureEffect5 != (Object) null ? creatureEffect5.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct) null;
            if ((Object) autoDestruct != (Object) null)
            {
              autoDestruct.time = 3f;
              autoDestruct.DestroyWhenDisable();
            }
          }
          if (((Object) creatureEffect6 != (Object) null ? (Object) creatureEffect6.gameObject.GetComponent<AutoDestruct>() : (Object) null) == (Object) null)
          {
            AutoDestruct autoDestruct = (Object) creatureEffect6 != (Object) null ? creatureEffect6.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct) null;
            if ((Object) autoDestruct != (Object) null)
            {
              autoDestruct.time = 3f;
              autoDestruct.DestroyWhenDisable();
            }
          }
        }
        this.MakeEffect("6/Dango_Emotion_Spread", target: this._owner);
        SoundEffectPlayer.PlaySound("Creature/Danggo_LvUp");
        SoundEffectPlayer.PlaySound("Creature/Danggo_Birth");
      }
      if (this.stack <= 0)
        return;
      this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, this.stack, this._owner);
      this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, this.stack, this._owner);
      this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, this.stack, this._owner);
    }
  }
}
}
