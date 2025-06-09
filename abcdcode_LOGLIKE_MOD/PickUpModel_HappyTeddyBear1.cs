// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_HappyTeddyBear1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_HappyTeddyBear1 : CreaturePickUpModel
{
  public PickUpModel_HappyTeddyBear1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_HappyTeddyBear1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_HappyTeddyBear1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_HappyTeddyBear1_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370111), (EmotionCardAbilityBase) new PickUpModel_HappyTeddyBear1.LogEmotionCardAbility_HappyTeddyBear1(), model);
  }

  public class LogEmotionCardAbility_HappyTeddyBear1 : EmotionCardAbilityBase
  {
    public const float _defaultProb = 0.2f;
    public const float _probPerCnt = 0.1f;
    public BattleUnitModel _lastTarget;
    public int _parryingCount;

    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      if ((double) RandomUtil.valueForProb >= 0.20000000298023224 + (double) this._parryingCount * 0.10000000149011612)
        return;
      BattleUnitModel target = behavior?.card?.target;
      if (target != null)
      {
        int diceResultValue = behavior.DiceResultValue;
        this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 0, ResultOption.Default, diceResultValue);
        target.TakeBreakDamage(diceResultValue, DamageType.Emotion, this._owner);
        target.battleCardResultLog?.SetCreatureEffectSound("Creature/Teddy_Atk");
        target.battleCardResultLog?.SetCreatureAbilityEffect("1/HappyTeddy_Hug");
      }
    }

    public override void OnRollDice(BattleDiceBehavior behavior)
    {
      if (!behavior.IsParrying())
        return;
      if (behavior.card.target == this._lastTarget)
      {
        ++this._parryingCount;
      }
      else
      {
        this._parryingCount = 0;
        this._lastTarget = behavior.card?.target;
      }
    }

    public override void OnSelectEmotion()
    {
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_HappyTeddyBear1.LogEmotionCardAbility_HappyTeddyBear1.BattleUnitBuf_teddy_hug());
      this._owner.view.unitBottomStatUI.SetBufs();
    }

    public class BattleUnitBuf_teddy_hug : BattleUnitBuf
    {
      public override string keywordId => "Teddy_Head";

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
