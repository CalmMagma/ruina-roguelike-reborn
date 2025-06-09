// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_TheSnowQueen1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_TheSnowQueen1 : CreaturePickUpModel
{
  public PickUpModel_TheSnowQueen1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_TheSnowQueen1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_TheSnowQueen1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_TheSnowQueen1_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370301), (EmotionCardAbilityBase) new PickUpModel_TheSnowQueen1.LogEmotionCardAbility_TheSnowQueen1(), model);
  }

  public class LogEmotionCardAbility_TheSnowQueen1 : EmotionCardAbilityBase
  {
    public const float _prob = 0.5f;
    public const int _bindMin = 1;
    public const int _bindMax = 3;
    public const int _dmgMin = 2;
    public const int _dmgMax = 5;

    public static bool Prob => (double) RandomUtil.valueForProb < 0.5;

    public static int Bind => RandomUtil.Range(1, 3);

    public static int Dmg => RandomUtil.Range(2, 5);

    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      base.OnWinParrying(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null || !PickUpModel_TheSnowQueen1.LogEmotionCardAbility_TheSnowQueen1.Prob)
        return;
      target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, PickUpModel_TheSnowQueen1.LogEmotionCardAbility_TheSnowQueen1.Bind, this._owner);
      if (target.bufListDetail.GetReadyBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_TheSnowQueen1.LogEmotionCardAbility_TheSnowQueen1.BattleUnitBuf_Emotion_Snowqueen_Aura)) == null)
        target.bufListDetail.AddReadyBuf((BattleUnitBuf) new PickUpModel_TheSnowQueen1.LogEmotionCardAbility_TheSnowQueen1.BattleUnitBuf_Emotion_Snowqueen_Aura());
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      base.OnSucceedAttack(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null || target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x.bufType == KeywordBuf.Binding)) == null)
        return;
      target.battleCardResultLog?.SetNewCreatureAbilityEffect("0_K/FX_IllusionCard_0_K_SnowUnATK", 2f);
      target.battleCardResultLog?.SetCreatureEffectSound("Creature/SnowQueen_Atk");
    }

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      BattleUnitModel target = behavior?.card?.target;
      if (target == null || !this.IsAttackDice(behavior.Detail) || target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x.bufType == KeywordBuf.Binding)) == null)
        return;
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        breakDmg = PickUpModel_TheSnowQueen1.LogEmotionCardAbility_TheSnowQueen1.Dmg
      });
    }

    public class BattleUnitBuf_Emotion_Snowqueen_Aura : BattleUnitBuf
    {
      public GameObject aura;

      public override bool Hide => true;

      public override void OnRoundStart()
      {
        base.OnRoundStart();
        if (this._owner != null)
        {
          Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_SnowAura", 1f, this._owner.view, this._owner.view);
          this.aura = (UnityEngine.Object) fxCreatureEffect != (UnityEngine.Object) null ? fxCreatureEffect.gameObject : (GameObject) null;
        }
        SoundEffectPlayer.PlaySound("Creature/SnowQueen_Freeze");
      }

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        this.Destroy();
      }

      public override void OnDie()
      {
        base.OnDie();
        this.Destroy();
      }

      public override void Destroy()
      {
        base.Destroy();
        this.DestroyAura();
      }

      public void DestroyAura()
      {
        if (!((UnityEngine.Object) this.aura != (UnityEngine.Object) null))
          return;
        UnityEngine.Object.Destroy((UnityEngine.Object) this.aura.gameObject);
        this.aura = (GameObject) null;
      }
    }
  }
}
}
