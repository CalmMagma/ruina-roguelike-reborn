// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_BlueStar2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_BlueStar2 : CreaturePickUpModel
{
  public PickUpModel_BlueStar2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_BlueStar2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_BlueStar2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_BlueStar2_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370292), (EmotionCardAbilityBase) new PickUpModel_BlueStar2.LogEmotionCardAbility_BlueStar2(), model);
  }

  public class LogEmotionCardAbility_BlueStar2 : EmotionCardAbilityBase
  {
    public const int _dmgMin = 3;
    public const int _dmgMax = 7;
    public const int _str = 1;
    public bool triggered;

    public int Dmg => RandomUtil.Range(3, 7);

    public override void OnRoundEndTheLast()
    {
      base.OnRoundEndTheLast();
      this.triggered = false;
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
      {
        if (alive.IsBreakLifeZero())
        {
          this.triggered = true;
          alive.TakeDamage(this.Dmg, DamageType.Emotion, this._owner);
          SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_JudgementExplo", 1f, alive.view, alive.view, 2f);
          SoundEffectPlayer.PlaySound("Creature/BlueStar_Suicide");
        }
      }
      if (!this.triggered)
        return;
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
        alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this._owner);
    }
  }
}
}
