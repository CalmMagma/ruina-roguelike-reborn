// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ScorchedGirl1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ScorchedGirl1 : CreaturePickUpModel
{
  public PickUpModel_ScorchedGirl1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ScorchedGirl1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ScorchedGirl1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ScorchedGirl1_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370011), (EmotionCardAbilityBase) new PickUpModel_ScorchedGirl1.LogEmotionCardAbility_ScorchedGirl1(), model);
  }

  public class LogEmotionCardAbility_ScorchedGirl1 : EmotionCardAbilityBase
  {
    public const float _prob = 0.4f;
    public const int _burnMin = 1;
    public const int _burnMax = 3;
    public bool trigger;

    public static bool Prob => (double) RandomUtil.valueForProb < 0.40000000596046448;

    public static int Burn => RandomUtil.Range(1, 3);

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (!this.trigger)
        return;
      this.trigger = false;
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new EmotionCardAbility_burnningGirl.BattleUnitBuf_Emotion_BurningGirl_Burn());
    }

    public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
    {
      int burn = PickUpModel_ScorchedGirl1.LogEmotionCardAbility_ScorchedGirl1.Burn;
      atkDice?.owner?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, burn, this._owner);
      this._owner.battleCardResultLog?.SetCreatureAbilityEffect("1/MatchGirl_Ash", 1f);
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MatchGirl_Barrier");
      if (!PickUpModel_ScorchedGirl1.LogEmotionCardAbility_ScorchedGirl1.Prob)
        return;
      this.trigger = true;
    }

    public class BattleUnitBuf_Emotion_BurningGirl_Burn : BattleUnitBuf
    {
      public const int _burnMin = 1;
      public const int _burnMax = 1;

      public override string keywordId => "BurningGirl_Burn";

      public override string keywordIconId => "Burning_Match";

      public static int Burn => RandomUtil.Range(1, 1);

      public override void OnSuccessAttack(BattleDiceBehavior behavior)
      {
        base.OnSuccessAttack(behavior);
        behavior?.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, PickUpModel_ScorchedGirl1.LogEmotionCardAbility_ScorchedGirl1.BattleUnitBuf_Emotion_BurningGirl_Burn.Burn, this._owner);
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
