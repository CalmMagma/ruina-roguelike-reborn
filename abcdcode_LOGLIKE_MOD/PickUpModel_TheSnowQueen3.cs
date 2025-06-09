// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_TheSnowQueen3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_TheSnowQueen3 : CreaturePickUpModel
{
  public PickUpModel_TheSnowQueen3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_TheSnowQueen3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_TheSnowQueen3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_TheSnowQueen3_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370303), (EmotionCardAbilityBase) new PickUpModel_TheSnowQueen3.LogEmotionCardAbility_TheSnowQueen3(), model);
  }

  public class LogEmotionCardAbility_TheSnowQueen3 : EmotionCardAbilityBase
  {
    public bool _effect;

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (this._effect)
        return;
      this._effect = true;
      Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/0_K/FX_IllusionCard_0_K_Blizzard");
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
      SoundEffectPlayer.PlaySound("Creature/SnowQueen_StrongAtk2");
    }

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
      {
        if (alive != this._owner)
          alive.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_TheSnowQueen3.LogEmotionCardAbility_TheSnowQueen3.BattleUnitBuf_Emotion_SnowQueen_Stun(this._owner));
      }
    }

    public class BattleUnitBuf_Emotion_SnowQueen_Stun : BattleUnitBuf
    {
      public const int _bindMin = 6;
      public const int _bindMax = 6;
      public BattleUnitModel _attacker;
      public Battle.CreatureEffect.CreatureEffect _aura;

      public static int Bind => RandomUtil.Range(6, 6);

      public override string keywordId => "SnowQueen_Emotion_Stun";

      public override string keywordIconId => "SnowQueen_Stun";

      public BattleUnitBuf_Emotion_SnowQueen_Stun(BattleUnitModel attacker)
      {
        this._attacker = attacker;
      }

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Stun, 1, this._attacker);
      }

      public override void OnRoundStart()
      {
        base.OnRoundStart();
        if (this._owner.bufListDetail.GetActivatedBuf(KeywordBuf.Stun) == null || this._owner.IsImmune(KeywordBuf.Stun) || this._owner.bufListDetail.IsImmune(BufPositiveType.Negative))
          return;
        this._owner.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
        this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("0/SnowQueen_Emotion_Frozen", 1f, this._owner.view, this._owner.view);
      }

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        if (this._owner.faction != this._attacker.faction)
          this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, PickUpModel_TheSnowQueen3.LogEmotionCardAbility_TheSnowQueen3.BattleUnitBuf_Emotion_SnowQueen_Stun.Bind, this._attacker);
        if ((Object) this._aura != (Object) null)
        {
          Object.Destroy((Object) this._aura.gameObject);
          this._aura = (Battle.CreatureEffect.CreatureEffect) null;
          this._owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
        }
        this.Destroy();
      }
    }
  }
}
}
