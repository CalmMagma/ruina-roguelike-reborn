// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_RedShoes2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_RedShoes2 : CreaturePickUpModel
{
  public PickUpModel_RedShoes2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_RedShoes2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_RedShoes2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_RedShoes2_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp()
  {
    base.OnPickUp();
    SoundEffectPlayer.PlaySound("Creature/RedShoes_On");
  }

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370132), (EmotionCardAbilityBase) new PickUpModel_RedShoes2.LogEmotionCardAbility_RedShoes2(), model);
  }

  public class LogEmotionCardAbility_RedShoes2 : EmotionCardAbilityBase
  {
    public override void OnSelectEmotionOnce()
    {
      base.OnSelectEmotionOnce();
      SoundEffectPlayer.PlaySound("Creature/RedShoes_On");
    }

    public override int OnGiveKeywordBufByCard(
      BattleUnitBuf buf,
      int stack,
      BattleUnitModel target)
    {
      return buf.bufType == KeywordBuf.Bleeding ? stack : 0;
    }

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      this._owner.battleCardResultLog.SetAttackEffectFilter(typeof (ImageFilter_ColorBlend));
    }
  }
}
}
