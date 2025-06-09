// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_QueenBee3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_QueenBee3 : CreaturePickUpModel
{
  public PickUpModel_QueenBee3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_QueenBee3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_QueenBee3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_QueenBee3_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370313), (EmotionCardAbilityBase) new PickUpModel_QueenBee3.LogEmotionCardAbility_QueenBee3(), model);
  }

  public class LogEmotionCardAbility_QueenBee3 : EmotionCardAbilityBase
  {
    public const int _rate = 5;
    public int _dmg;

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      int stack = (int) Math.Round((double) this._dmg * 1.0 / 5.0);
      if (stack > 0)
      {
        new GameObject().AddComponent<SpriteFilter_Queenbee_Spore>().Init("EmotionCardFilter/QueenBee_Filter_Spore", false, 2f);
        SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/QueenBee_Funga");
        if ((UnityEngine.Object) soundEffectPlayer != (UnityEngine.Object) null)
          soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
        foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
          alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, stack);
      }
      this._dmg = 0;
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      int damageAtOneRound = this._owner.history.takeDamageAtOneRound;
      if (damageAtOneRound <= 0)
        return;
      this._dmg = damageAtOneRound;
    }
  }
}
}
