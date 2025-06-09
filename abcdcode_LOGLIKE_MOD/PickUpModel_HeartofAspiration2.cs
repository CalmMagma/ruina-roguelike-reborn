// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_HeartofAspiration2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_HeartofAspiration2 : CreaturePickUpModel
{
  public PickUpModel_HeartofAspiration2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_HeartofAspiration2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_HeartofAspiration2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_HeartofAspiration2_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370102), (EmotionCardAbilityBase) new PickUpModel_HeartofAspiration2.LogEmotionCardAbility_HeartofAspiration2(), model);
  }

  public class LogEmotionCardAbility_HeartofAspiration2 : EmotionCardAbilityBase
  {
    public Battle.CreatureEffect.CreatureEffect _heartBeatEffect;

    public override void OnSelectEmotion()
    {
      Singleton<StageController>.Instance.onChangePhase += new StageController.OnChangePhaseDelegate(((EmotionCardAbilityBase) this).OnChangeStagePhase);
      SoundEffectPlayer.PlaySound("Creature/Heartbeat");
      SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_Heart", 1f, this._owner.view, this._owner.view, 3f);
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new EmotionCardAbility_heart.BattleUnitBuf_Emotion_Heart_Eager());
      this._owner.view.unitBottomStatUI.SetBufs();
    }

    public override StatBonus GetStatBonus()
    {
      return new StatBonus() { hpRate = 15 };
    }

    public override int GetSpeedDiceAdder()
    {
      int speedDiceAdder = RandomUtil.Range(1, 2);
      this._owner.ShowTypoTemporary(this._emotionCard, 0, ResultOption.Sign, speedDiceAdder);
      return speedDiceAdder;
    }

    public class BattleUnitBuf_Emotion_Heart_Eager : BattleUnitBuf
    {
      public override string keywordId => "HeartofAspiration_Heart";

      public override BufPositiveType positiveType => BufPositiveType.Positive;

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        this.stack = 0;
      }

      public override void OnDie() => this.Destroy();
    }
  }
}
}
