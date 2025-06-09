// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_FairyCarnival3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_FairyCarnival3 : CreaturePickUpModel
{
  public PickUpModel_FairyCarnival3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_FairyCarnival3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_FairyCarnival3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_FairyCarnival3_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370213), (EmotionCardAbilityBase) new PickUpModel_FairyCarnival3.LogEmotionCardAbility_FairyCarnival3(), model);
  }

  public class LogEmotionCardAbility_FairyCarnival3 : EmotionCardAbilityBase
  {
    public const int _loseHp = 10;
    public const int _pow = 3;
    public const int _haste = 3;
    public int lostHp;

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this.lostHp = 0;
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
      {
        if (alive != this._owner)
        {
          int num = alive.LoseHp(10);
          if (num > 0)
            this.lostHp += num;
        }
      }
      if (this.lostHp > 0)
        this._owner.RecoverHP(this.lostHp);
      this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 3, this._owner);
      this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, 3, this._owner);
      SoundEffectPlayer.PlaySound("Creature/Fairy_QueenEat");
      SoundEffectPlayer.PlaySound("Creature/Fairy_QueenChange");
      this.SetFilter("1/Fairy_Filter");
    }
  }
}
}
