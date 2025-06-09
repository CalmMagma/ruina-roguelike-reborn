// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_SpiderBud3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_SpiderBud3 : CreaturePickUpModel
{
  public PickUpModel_SpiderBud3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_SpiderBud3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_SpiderBud3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_SpiderBud3_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370233), (EmotionCardAbilityBase) new PickUpModel_SpiderBud3.LogEmotionCardAbility_SpiderBud3(), model);
  }

  public class LogEmotionCardAbility_SpiderBud3 : EmotionCardAbilityBase
  {
    public const int _powRed = 1;
    public const float _prob = 0.5f;
    public const int _bind = 1;
    public const int _vuln = 1;

    public bool Prob => (double) RandomUtil.valueForProb < 0.5;

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Spidermom_Down");
      if ((Object) soundEffectPlayer != (Object) null)
        soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
      this.MakeEffect("3/Spider_RedEye", target: this._owner, apply: false);
    }

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      if (!behavior.IsParrying())
        return;
      BattleDiceBehavior targetDice = behavior?.TargetDice;
      if (targetDice == null)
        return;
      targetDice.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = -1
      });
    }

    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      base.OnWinParrying(behavior);
      if (!this.Prob)
        return;
      behavior.card?.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, 1, this._owner);
      behavior.card?.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 1, this._owner);
    }
  }
}
}
