// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Redhood3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Redhood3 : CreaturePickUpModel
{
  public PickUpModel_Redhood3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Redhood3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Redhood3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Redhood3_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370063), (EmotionCardAbilityBase) new PickUpModel_Redhood3.LogEmotionCardAbility_Redhood3(), model);
  }

  public class LogEmotionCardAbility_Redhood3 : EmotionCardAbilityBase
  {
    public Battle.CreatureEffect.CreatureEffect aura;
    public string path = "6/RedHood_Emotion_Aura";

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      if (!((Object) this.aura != (Object) null))
        return;
      this.DestroyAura();
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      this.DestroyAura();
      if ((double) this._owner.hp > (double) this._owner.MaxHp * 0.5)
        return;
      if ((double) this._owner.hp <= (double) this._owner.MaxHp * 0.34999999403953552)
      {
        if ((double) this._owner.hp <= (double) this._owner.MaxHp * 0.25)
          this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 4, this._owner);
        else
          this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 2, this._owner);
      }
      else
        this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this._owner);
      SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedHood_Change");
      if ((Object) this.aura == (Object) null)
        this.aura = this.MakeEffect(this.path, target: this._owner, apply: false);
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
  }
}
}
