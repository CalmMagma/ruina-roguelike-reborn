// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Greed2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Greed2 : CreaturePickUpModel
{
  public PickUpModel_Greed2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Greed2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Greed2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Greed2_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370252), (EmotionCardAbilityBase) new PickUpModel_Greed2.LogEmotionCardAbility_Greed2(), model);
  }

  public class LogEmotionCardAbility_Greed2 : EmotionCardAbilityBase
  {
    public const int _stackMax = 3;
    public int count;
    public Battle.CreatureEffect.CreatureEffect aura;

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KingOfGreed_Yellow", false, 2f);
    }

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      this.DestroyAura();
      if (this.count > 0)
      {
        this.aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("5_T/FX_IllusionCard_5_T_Happiness", 1f, this._owner.view, this._owner.view);
        SoundEffectPlayer.PlaySound("Creature/Greed_MakeDiamond");
      }
      this.count = 0;
    }

    public override void OnDie(BattleUnitModel killer)
    {
      base.OnDie(killer);
      this.DestroyAura();
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      this.DestroyAura();
      if (this.count <= 0)
        return;
      this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, this.GetStack(this.count), this._owner);
    }

    public override void OnWinParrying(BattleDiceBehavior behavior)
    {
      base.OnWinParrying(behavior);
      ++this.count;
    }

    public void DestroyAura()
    {
      if (!((Object) this.aura != (Object) null))
        return;
      Object.Destroy((Object) this.aura.gameObject);
      this.aura = (Battle.CreatureEffect.CreatureEffect) null;
    }

    public int GetStack(int cnt) => Mathf.Min(3, cnt);
  }
}
}
