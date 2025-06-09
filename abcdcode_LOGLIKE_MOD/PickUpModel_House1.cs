// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_House1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_House1 : CreaturePickUpModel
{
  public PickUpModel_House1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_House1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_House1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_House1_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370271), (EmotionCardAbilityBase) new PickUpModel_House1.LogEmotionCardAbility_House1(), model);
  }

  public class LogEmotionCardAbility_House1 : EmotionCardAbilityBase
  {
    public const int _penalty = -2;
    public GameObject _aura;
    public bool _sound;

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      SoundEffectPlayer.PlaySound("Creature/House_Lion_Poison");
    }

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      int stack = BattleObjectManager.instance.GetAliveList(this._owner.faction).Count - 2;
      if (stack > 0)
      {
        this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, stack, this._owner);
        this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, stack, this._owner);
        Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Aura_Lion", 1f, this._owner.view, this._owner.view);
        this._aura = (Object) fxCreatureEffect != (Object) null ? fxCreatureEffect.gameObject : (GameObject) null;
      }
      else if (stack < 0)
      {
        if (!this._sound)
        {
          this._sound = true;
          SoundEffectPlayer.PlaySound("Creature/House_Lion_Change");
        }
        this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, -stack, this._owner);
        this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Vulnerable, -stack, this._owner);
        Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Aura_Cat", 1f, this._owner.view, this._owner.view);
        this._aura = (Object) fxCreatureEffect != (Object) null ? fxCreatureEffect.gameObject : (GameObject) null;
      }
      else
      {
        Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Aura_Lion", 1f, this._owner.view, this._owner.view);
        this._aura = (Object) fxCreatureEffect != (Object) null ? fxCreatureEffect.gameObject : (GameObject) null;
      }
    }

    public override void OnDie(BattleUnitModel killer)
    {
      base.OnDie(killer);
      this.DestroyAura();
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      this.DestroyAura();
    }

    public void DestroyAura()
    {
      if (!((Object) this._aura != (Object) null))
        return;
      Object.Destroy((Object) this._aura);
      this._aura = (GameObject) null;
    }
  }
}
}
