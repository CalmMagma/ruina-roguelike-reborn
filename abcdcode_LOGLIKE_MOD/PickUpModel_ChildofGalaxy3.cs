// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ChildofGalaxy3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ChildofGalaxy3 : CreaturePickUpModel
{
  public PickUpModel_ChildofGalaxy3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ChildofGalaxy3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ChildofGalaxy3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ChildofGalaxy3_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370143), (EmotionCardAbilityBase) new PickUpModel_ChildofGalaxy3.LogEmotionCardAbility_ChildofGalaxy3(), model);
  }

  public class LogEmotionCardAbility_ChildofGalaxy3 : EmotionCardAbilityBase
  {
    public int _roundCount;

    public override void OnRoundStart()
    {
      if (this._roundCount >= 3)
        return;
      ++this._roundCount;
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new EmotionCardAbility_galaxyChild3.BattleUnitBuf_galaxyChild_Friend());
      int v = Mathf.Min(this._owner.MaxHp / 10, 12);
      this._owner.RecoverHP(v);
      this._owner.ShowTypoTemporary(this._emotionCard, 0, ResultOption.Default, v);
      SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_GalaxyCard_O", 1f, this._owner.view, this._owner.view, 2f);
    }

    public override void OnSelectEmotion() => this._owner.view.unitBottomStatUI.SetBufs();

    public class BattleUnitBuf_galaxyChild_Friend : BattleUnitBuf
    {
      public override string keywordId => "GalaxyBoy_Stone";

      public override string keywordIconId => "GalaxyBoy_Stone";

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        this.stack = 0;
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
