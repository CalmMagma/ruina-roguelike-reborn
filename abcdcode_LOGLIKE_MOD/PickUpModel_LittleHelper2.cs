// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_LittleHelper2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_LittleHelper2 : CreaturePickUpModel
{
  public PickUpModel_LittleHelper2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_LittleHelper2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_LittleHelper2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_LittleHelper2_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370122), (EmotionCardAbilityBase) new PickUpModel_LittleHelper2.LogEmotionCardAbility_LittleHelper2(), model);
  }

  public class LogEmotionCardAbility_LittleHelper2 : EmotionCardAbilityBase
  {
    public const int _recoverPP = 1;
    public const int _count = 3;
    public const int _quickMin = 1;
    public const int _quickMax = 2;
    public int cnt;
    public int quick;

    public static int Quick => RandomUtil.Range(1, 2);

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (this.quick <= 0)
        return;
      SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("2_Y/FX_IllusionCard_2_Y_Scan_Start", 1f, this._owner.view, this._owner.view, 3f);
      SoundEffectPlayer.PlaySound("Creature/Helper_On");
      this.quick = 0;
    }

    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      base.OnWinParrying(behavior);
      ++this.cnt;
      if (this.cnt < 3)
        return;
      this.cnt %= 3;
      this._owner.cardSlotDetail.RecoverPlayPoint(1);
      ++this.quick;
      this._owner.battleCardResultLog?.SetNewCreatureAbilityEffect("2_Y/FX_IllusionCard_2_Y_Scan", 3f);
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Helper_On");
    }

    public override void OnRoundEnd()
    {
      if (this.quick <= 0)
        return;
      for (int index = 0; index < this.quick; ++index)
        this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, PickUpModel_LittleHelper2.LogEmotionCardAbility_LittleHelper2.Quick, this._owner);
    }
  }
}
}
