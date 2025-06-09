// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Angry3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Angry3 : CreaturePickUpModel
{
  public PickUpModel_Angry3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Angry3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Angry3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Angry3_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370353), (EmotionCardAbilityBase) new PickUpModel_Angry3.LogEmotionCardAbility_Angry3(), model);
  }

  public class LogEmotionCardAbility_Angry3 : EmotionCardAbilityBase
  {
    public const int _decay = 5;
    public bool _effect;

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
        alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 5, this._owner);
    }

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (!this._effect)
      {
        this._effect = true;
        Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/5/Servant_Emotion_Effect");
        if ((Object) original != (Object) null)
        {
          Battle.CreatureEffect.CreatureEffect creatureEffect = Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
          if (((Object) creatureEffect != (Object) null ? (Object) creatureEffect.gameObject.GetComponent<AutoDestruct>() : (Object) null) == (Object) null)
          {
            AutoDestruct autoDestruct = (Object) creatureEffect != (Object) null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct) null;
            if ((Object) autoDestruct != (Object) null)
            {
              autoDestruct.time = 3f;
              autoDestruct.DestroyWhenDisable();
            }
          }
        }
        SoundEffectPlayer.PlaySound("Creature/Angry_StrongFinish");
      }
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
      {
        int kewordBufStack = alive.bufListDetail.GetKewordBufStack(KeywordBuf.Decay);
        if (kewordBufStack > 0)
          alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Binding, kewordBufStack, this._owner);
      }
    }
  }
}
}
