// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Laetitia1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Laetitia1 : CreaturePickUpModel
{
  public PickUpModel_Laetitia1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Laetitia1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Laetitia1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Laetitia1_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370331), (EmotionCardAbilityBase) new PickUpModel_Laetitia1.LogEmotionCardAbility_Laetitia1(), model);
  }

  public class LogEmotionCardAbility_Laetitia1 : EmotionCardAbilityBase
  {
    public BattleUnitModel _target;

    public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
    {
      base.OnParryingStart(card);
      BattleUnitModel target = card?.target;
      if (target == null || this._target != null)
        return;
      PickUpModel_Laetitia1.LogEmotionCardAbility_Laetitia1.BattleUnitBuf_Emotion_Latitia_Gift buf = new PickUpModel_Laetitia1.LogEmotionCardAbility_Laetitia1.BattleUnitBuf_Emotion_Latitia_Gift(this._owner);
      target.bufListDetail.AddBuf((BattleUnitBuf) buf);
      this._target = target;
    }

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this._target = (BattleUnitModel) null;
    }

    public override void OnDieOtherUnit(BattleUnitModel unit)
    {
      base.OnDieOtherUnit(unit);
      if (unit != this._target)
        return;
      this._target = (BattleUnitModel) null;
    }

    public class BattleUnitBuf_Emotion_Latitia_Gift : BattleUnitBuf
    {
      public const float _prob = 0.4f;
      public const int _dmgMin = 2;
      public const int _dmgMax = 7;
      public const int _bleedMin = 2;
      public const int _bleedMax = 2;
      public BattleUnitModel _giver;

      public static bool Prob => (double) RandomUtil.valueForProb < 0.40000000596046448;

      public static int Dmg => RandomUtil.Range(2, 7);

      public static int Bleed => RandomUtil.Range(2, 2);

      public override string keywordId => "Latitia_Gift";

      public BattleUnitBuf_Emotion_Latitia_Gift(BattleUnitModel giver) => this._giver = giver;

      public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
      {
        base.OnStartParrying(card);
        BattleUnitModel target = card?.target;
        if (target == null || this._giver == null || this._giver == target || !PickUpModel_Laetitia1.LogEmotionCardAbility_Laetitia1.BattleUnitBuf_Emotion_Latitia_Gift.Prob)
          return;
        this._owner.battleCardResultLog?.SetCreatureAbilityEffect("3/Latitia_Boom", 1.5f);
        this._owner.TakeDamage(PickUpModel_Laetitia1.LogEmotionCardAbility_Laetitia1.BattleUnitBuf_Emotion_Latitia_Gift.Dmg, DamageType.Emotion, this._giver);
        this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, PickUpModel_Laetitia1.LogEmotionCardAbility_Laetitia1.BattleUnitBuf_Emotion_Latitia_Gift.Bleed, this._giver);
      }
    }
  }
}
}
