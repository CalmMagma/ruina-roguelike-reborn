// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Butterfly2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Butterfly2 : CreaturePickUpModel
{
  public PickUpModel_Butterfly2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Butterfly2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Butterfly2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Butterfly2_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370322), (EmotionCardAbilityBase) new PickUpModel_Butterfly2.LogEmotionCardAbility_Butterfly2(), model);
  }

  public class LogEmotionCardAbility_Butterfly2 : EmotionCardAbilityBase
  {
    public const int _strMin = 1;
    public const int _strMax = 2;

    public int Str => RandomUtil.Range(1, 2);

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
        alive.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_Butterfly2.LogEmotionCardAbility_Butterfly2.BattleUnitBuf_Emotion_Butterfly_DmgByDebuf());
      if (this._owner.bufListDetail.GetNegativeBufTypeCount() <= 0 && this._owner.bufListDetail.GetReadyBufList().Find((Predicate<BattleUnitBuf>) (x => x.positiveType == BufPositiveType.Negative)) == null)
        return;
      SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("2_Y/FX_IllusionCard_2_Y_Fly", 1f, this._owner.view, this._owner.view, 2f);
      this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, this.Str, this._owner);
    }

    public class BattleUnitBuf_Emotion_Butterfly_DmgByDebuf : BattleUnitBuf
    {
      public const int _dmgAddMin = 2;
      public const int _dmgAddMax = 4;

      public int DmgAdd => RandomUtil.Range(2, 4);

      public override bool Hide => true;

      public override int GetDamageReduction(BattleDiceBehavior behavior)
      {
        return this._owner.bufListDetail.GetNegativeBufTypeCount() > 0 ? -this.DmgAdd : base.GetDamageReduction(behavior);
      }

      public override void BeforeTakeDamage(BattleUnitModel attacker, int dmg)
      {
        base.BeforeTakeDamage(attacker, dmg);
        if (this._owner.bufListDetail.GetNegativeBufTypeCount() <= 0)
          return;
        if (Singleton<StageController>.Instance.IsLogState())
        {
          this._owner.battleCardResultLog?.SetCreatureAbilityEffect("2/ButterflyEffect_White", 1f);
          this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/ButterFlyMan_Atk_White");
        }
        else
        {
          SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("2/ButterflyEffect_White", 1f, this._owner.view, this._owner.view, 1f);
          SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/ButterFlyMan_Atk_White");
        }
      }

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        this.Destroy();
      }
    }
  }
}
}
