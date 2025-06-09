// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BigBadWolf2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_BigBadWolf2 : CreaturePickUpModel
{
  public PickUpModel_BigBadWolf2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_BigBadWolf2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_BigBadWolf2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_BigBadWolf2_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370162), (EmotionCardAbilityBase) new PickUpModel_BigBadWolf2.LogEmotionCardAbility_BigBadWolf2(), model);
  }

  public class LogEmotionCardAbility_BigBadWolf2 : EmotionCardAbilityBase
  {
    public const float _dmgCondition = 0.25f;
    public int _accumulatedDmg;
    public bool trigger;
    public Battle.CreatureEffect.CreatureEffect aura;
    public string path = "6/BigBadWolf_Emotion_Aura";

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      if (!((Object) this.aura != (Object) null))
        return;
      this.DestroyAura();
    }

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this._accumulatedDmg = 0;
    }

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (!this.trigger)
        return;
      this.trigger = false;
      this._accumulatedDmg = 0;
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_BigBadWolf2.LogEmotionCardAbility_BigBadWolf2.BattleUnitBuf_Emotion_Wolf_Claw());
      if ((Object) this.aura == (Object) null)
        this.aura = this.MakeEffect(this.path, target: this._owner, apply: false);
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      this.DestroyAura();
    }

    public override void OnDie(BattleUnitModel killer)
    {
      base.OnDie(killer);
      this.DestroyAura();
    }

    public void DestroyAura()
    {
      if ((Object) this.aura != (Object) null && (Object) this.aura.gameObject != (Object) null)
        Object.Destroy((Object) this.aura.gameObject);
      this.aura = (Battle.CreatureEffect.CreatureEffect) null;
    }

    public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
    {
      base.BeforeTakeDamage(attacker, dmg);
      if (!this._owner.passiveDetail.IsInvincible())
        this._accumulatedDmg += dmg;
      if ((double) this._accumulatedDmg >= (double) this._owner.MaxHp * 0.25)
        this.trigger = true;
      return false;
    }

    public class BattleUnitBuf_Emotion_Wolf_Claw : BattleUnitBuf
    {
      public const int _strMin = 2;
      public const int _strMax = 2;
      public const int _bleedMin = 1;
      public const int _bleedMax = 1;

      public override string keywordId => "Wolf_Claw";

      public static int Str => RandomUtil.Range(2, 2);

      public static int Bleed => RandomUtil.Range(1, 1);

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, PickUpModel_BigBadWolf2.LogEmotionCardAbility_BigBadWolf2.BattleUnitBuf_Emotion_Wolf_Claw.Str, owner);
        SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Wolf_FogChange");
      }

      public override void OnSuccessAttack(BattleDiceBehavior behavior)
      {
        base.OnSuccessAttack(behavior);
        behavior?.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, PickUpModel_BigBadWolf2.LogEmotionCardAbility_BigBadWolf2.BattleUnitBuf_Emotion_Wolf_Claw.Bleed, this._owner);
      }

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        this.Destroy();
      }

      public override bool IsTargetable() => false;

      public override bool DirectAttack() => true;
    }
  }
}
}
