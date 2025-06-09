// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Ozma3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Ozma3 : CreaturePickUpModel
{
  public PickUpModel_Ozma3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Ozma3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Ozma3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Ozma3_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370373), (EmotionCardAbilityBase) new PickUpModel_Ozma3.LogEmotionCardAbility_Ozma3(), model);
  }

  public class LogEmotionCardAbility_Ozma3 : EmotionCardAbilityBase
  {
    public bool _activated;
    public bool _effect;

    public override void OnRoundStart()
    {
      if (!this._effect)
      {
        this._effect = true;
        SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Particle", 1f, this._owner.view, this._owner.view, 3f);
        SoundEffectPlayer.PlaySound("CreatureOzma_FarAtk");
      }
      if (this._activated)
      {
        this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_Ozma3.LogEmotionCardAbility_Ozma3.BattleUnitBuf_ozmaReviveCheck))?.Destroy();
      }
      else
      {
        if (this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_Ozma3.LogEmotionCardAbility_Ozma3.BattleUnitBuf_ozmaReviveCheck)) != null)
          return;
        this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_Ozma3.LogEmotionCardAbility_Ozma3.BattleUnitBuf_ozmaReviveCheck());
      }
    }

    public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
    {
      if (this._activated || (double) this._owner.hp > (double) dmg)
        return false;
      this._activated = true;
      this._owner.RecoverHP(Mathf.Min(24, (int) ((double) this._owner.MaxHp * 0.20000000298023224)));
      if (Singleton<StageController>.Instance.IsLogState())
      {
        this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Particle", 3f);
        this._owner.battleCardResultLog?.SetCreatureEffectSound("CreatureOzma_FarAtk");
      }
      else
      {
        SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Particle", 1f, this._owner.view, this._owner.view, 3f);
        SoundEffectPlayer.PlaySound("CreatureOzma_FarAtk");
      }
      return true;
    }

    public class BattleUnitBuf_ozmaReviveCheck : BattleUnitBuf
    {
      public override string keywordId => "Ozma_revive";

      public override string keywordIconId => "Ozma_AwakenPumpkin";

      public BattleUnitBuf_ozmaReviveCheck() => this.stack = 0;
    }
  }
}
}
