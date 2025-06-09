// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Angry2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Angry2 : CreaturePickUpModel
{
  public PickUpModel_Angry2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Angry2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Angry2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Angry2_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370352), (EmotionCardAbilityBase) new PickUpModel_Angry2.LogEmotionCardAbility_Angry2(), model);
  }

  public class LogEmotionCardAbility_Angry2 : EmotionCardAbilityBase
  {
    public const int _pp = 3;
    public const int _hp = 10;
    public const int _break = 10;

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      base.OnSucceedAttack(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null)
        return;
      if (this.GetAliveFriend() == null)
      {
        target.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_Angry2.LogEmotionCardAbility_Angry2.BattleUnitBuf_Emotion_Wrath_Friend());
      }
      else
      {
        if (this.GetAliveFriend() != target)
          return;
        target.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_ATKMarker", 1.5f);
        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Angry_R_StrongAtk");
      }
    }

    public override void OnKill(BattleUnitModel target)
    {
      base.OnKill(target);
      if (target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (y => y is PickUpModel_Angry2.LogEmotionCardAbility_Angry2.BattleUnitBuf_Emotion_Wrath_Friend)) != null)
      {
        foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
        {
          alive.cardSlotDetail.RecoverPlayPoint(3);
          alive.breakDetail.RecoverBreak(10);
          alive.RecoverHP(10);
        }
        this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.KillEffect));
      }
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Angry_Vert2");
    }

    public void KillEffect()
    {
      CameraFilterUtil.EarthQuake(0.08f, 0.02f, 50f, 0.6f);
      Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/5_T/FX_IllusionCard_5_T_SmokeWater");
      if (!((UnityEngine.Object) original != (UnityEngine.Object) null))
        return;
      Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
      if (((UnityEngine.Object) creatureEffect != (UnityEngine.Object) null ? (UnityEngine.Object) creatureEffect.gameObject.GetComponent<AutoDestruct>() : (UnityEngine.Object) null) == (UnityEngine.Object) null)
      {
        AutoDestruct autoDestruct = (UnityEngine.Object) creatureEffect != (UnityEngine.Object) null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct) null;
        if ((UnityEngine.Object) autoDestruct != (UnityEngine.Object) null)
        {
          autoDestruct.time = 3f;
          autoDestruct.DestroyWhenDisable();
        }
      }
    }

    public BattleUnitModel GetAliveFriend()
    {
      return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((Predicate<BattleUnitModel>) (x => x.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (y => y is PickUpModel_Angry2.LogEmotionCardAbility_Angry2.BattleUnitBuf_Emotion_Wrath_Friend)) != null));
    }

    public class BattleUnitBuf_Emotion_Wrath_Friend : BattleUnitBuf
    {
      public override string keywordId => "Angry_Friend";

      public override string keywordIconId => "Reclus_Head";

      public BattleUnitBuf_Emotion_Wrath_Friend() => this.stack = 0;
    }
  }
}
}
