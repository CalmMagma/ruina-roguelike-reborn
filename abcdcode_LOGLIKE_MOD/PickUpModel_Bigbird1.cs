// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Bigbird1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Bigbird1 : CreaturePickUpModel
{
  public PickUpModel_Bigbird1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Bigbird1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Bigbird1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Bigbird1_FlaverText");
    this.level = 1;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370081), (EmotionCardAbilityBase) new PickUpModel_Bigbird1.LogEmotionCardAbility_Bigbird1(), model);
  }

  public class LogEmotionCardAbility_Bigbird1 : EmotionCardAbilityBase
  {
    public const int _addedRecoverPP = 1;
    public bool _effect;

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this._owner.cardSlotDetail.SetRecoverPoint(1 + this._owner.cardSlotDetail.GetRecoverPlayPoint());
    }

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this._owner.cardSlotDetail.RecoverPlayPoint(this._owner.cardSlotDetail.GetMaxPlayPoint());
      this._owner.cardSlotDetail.SetRecoverPoint(1 + this._owner.cardSlotDetail.GetRecoverPlayPoint());
    }

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (this._effect)
        return;
      this._effect = true;
      new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/BigBird_Filter_Bg", false, 2f);
      new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/BigBird_Filter_Fg", false, 2f);
      SoundEffectPlayer.PlaySound("Creature/Bigbird_Eyes");
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      List<BattleUnitModel> list = new List<BattleUnitModel>();
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
      {
        if (alive != this._owner)
        {
          alive.cardSlotDetail.SetRecoverPointDefault();
          list.Add(alive);
        }
      }
      if (list.Count <= 0)
        return;
      RandomUtil.SelectOne<BattleUnitModel>(list)?.cardSlotDetail.SetRecoverPoint(0);
    }
  }
}
}
