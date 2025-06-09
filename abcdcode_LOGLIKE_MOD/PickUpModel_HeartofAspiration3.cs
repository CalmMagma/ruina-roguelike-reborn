// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_HeartofAspiration3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_HeartofAspiration3 : CreaturePickUpModel
{
  public PickUpModel_HeartofAspiration3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_HeartofAspiration3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_HeartofAspiration3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_HeartofAspiration3_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370103), (EmotionCardAbilityBase) new PickUpModel_HeartofAspiration3.LogEmotionCardAbility_HeartofAspiration3(), model);
  }

  public class LogEmotionCardAbility_HeartofAspiration3 : EmotionCardAbilityBase
  {
    public const int _maxTurn = 3;
    public const int _str = 4;
    public const int _endur = 4;
    public const int _quick = 4;
    public const int _prot = 4;
    public int count;

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 4, this._owner);
      this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 4, this._owner);
      this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 4, this._owner);
      this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 4, this._owner);
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      ++this.count;
      if (this.count < 3)
        return;
      this._owner.Die();
    }

    public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
    {
      return this.count < 3 ? AtkResist.Resist : base.GetResistBP(origin, detail);
    }

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this.count = 0;
      SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_FastBeat", 1f, this._owner.view, this._owner.view);
      SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Heart_Fast");
      if ((Object) soundEffectPlayer == (Object) null)
        return;
      soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
    }

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_FastBeat", 1f, this._owner.view, this._owner.view);
      SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Heart_Fast");
      if ((Object) soundEffectPlayer == (Object) null)
        return;
      soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
    }

    public override void OnKill(BattleUnitModel target)
    {
      if (target.faction != Faction.Enemy)
        return;
      Singleton<StageController>.Instance.GetStageModel().AddHeartKillCount();
    }
  }
}
}
