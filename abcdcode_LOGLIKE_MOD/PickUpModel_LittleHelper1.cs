// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_LittleHelper1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_LittleHelper1 : CreaturePickUpModel
{
  public PickUpModel_LittleHelper1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_LittleHelper1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_LittleHelper1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_LittleHelper1_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370121), (EmotionCardAbilityBase) new PickUpModel_LittleHelper1.LogEmotionCardAbility_LittleHelper1(), model);
  }

  public class LogEmotionCardAbility_LittleHelper1 : EmotionCardAbilityBase
  {
    public GameObject _aura;

    public override void OnRoundStart_after()
    {
      base.OnRoundStart_after();
      if (this._owner.cardSlotDetail.PlayPoint < this._owner.cardSlotDetail.GetMaxPlayPoint())
        return;
      if ((Object) this._aura != (Object) null)
        this.DestroyAura();
      Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("2_Y/FX_IllusionCard_2_Y_Charge", 1f, this._owner.view, this._owner.view);
      this._aura = (Object) fxCreatureEffect != (Object) null ? fxCreatureEffect.gameObject : (GameObject) null;
      SoundEffectPlayer.PlaySound("Creature/Helper_FullCharge");
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_LittleHelper1.LogEmotionCardAbility_LittleHelper1.BattleUnitBuf_Emotion_Helper_Charge());
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
      if (!((Object) this._aura != (Object) null))
        return;
      Object.Destroy((Object) this._aura);
      this._aura = (GameObject) null;
    }

    public class BattleUnitBuf_Emotion_Helper_Charge : BattleUnitBuf
    {
      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, RandomUtil.Range(1, 2), owner);
      }

      public override void BeforeGiveDamage(BattleDiceBehavior behavior)
      {
        base.BeforeGiveDamage(behavior);
        if (behavior == null)
          return;
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          dmg = RandomUtil.Range(2, 7)
        });
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
