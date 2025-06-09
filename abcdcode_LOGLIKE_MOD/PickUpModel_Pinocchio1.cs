// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Pinocchio1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Pinocchio1 : CreaturePickUpModel
{
  public PickUpModel_Pinocchio1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Pinocchio1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Pinocchio1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Pinocchio1_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370201), (EmotionCardAbilityBase) new PickUpModel_Pinocchio1.LogEmotionCardAbility_Pinocchio1(), model);
  }

  public class LogEmotionCardAbility_Pinocchio1 : EmotionCardAbilityBase
  {
    public override void OnWaveStart()
    {
      this._owner.allyCardDetail.AddNewCardToDeck(this.GetTargetCardId());
    }

    public override void OnSelectEmotion()
    {
      this._owner.allyCardDetail.AddNewCard(this.GetTargetCardId());
      SoundEffectPlayer.PlaySound("Creature/Pino_On");
    }

    public int GetTargetCardId()
    {
      int targetCardId = 1100001;
      if (this._owner.Book.ClassInfo.RangeType == EquipRangeType.Range)
        targetCardId = 1100002;
      return targetCardId;
    }
  }
}
}
