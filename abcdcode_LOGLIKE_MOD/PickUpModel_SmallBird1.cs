// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_SmallBird1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_SmallBird1 : CreaturePickUpModel
{
  public PickUpModel_SmallBird1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_SmallBird1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_SmallBird1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_SmallBird1_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370181), (EmotionCardAbilityBase) new PickUpModel_SmallBird1.LogEmotionCardAbility_SmallBird1(), model);
  }

  public class LogEmotionCardAbility_SmallBird1 : EmotionCardAbilityBase
  {
    public const int _powMin = 2;
    public const int _powMax = 4;
    public bool dmged;

    public static int Pow => RandomUtil.Range(2, 4);

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this.dmged = false;
    }

    public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
    {
      base.BeforeTakeDamage(attacker, dmg);
      if (this._owner.IsImmuneDmg() || this._owner.IsInvincibleHp((BattleUnitModel) null))
        return false;
      this.dmged = true;
      return false;
    }

    public override void OnRoundEndTheLast()
    {
      base.OnRoundEndTheLast();
      if (this.dmged)
        this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_SmallBird1.LogEmotionCardAbility_SmallBird1.BattleUnitBuf_Emotion_SmallBird_Punish());
      this.dmged = false;
    }

    public class BattleUnitBuf_Emotion_SmallBird_Punish : BattleUnitBuf
    {
      public bool powUp = true;
      public GameObject aura;

      public override string keywordId => "SmallBird_Punishment";

      public override string keywordIconId => "SmallBird_Emotion_Punish";

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        this.stack = 0;
      }

      public override void OnRoundStart()
      {
        base.OnRoundStart();
        if ((Object) this.aura == (Object) null)
        {
          Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_Punising", 1f, this._owner.view, this._owner.view);
          this.aura = (Object) fxCreatureEffect != (Object) null ? fxCreatureEffect.gameObject : (GameObject) null;
        }
        SoundEffectPlayer.PlaySound("Creature/SmallBird_StrongAtk");
      }

      public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
      {
        base.OnUseCard(curCard);
        if (!this.powUp)
          return;
        this.powUp = false;
        curCard.ApplyDiceStatBonus(DiceMatch.NextDice, new DiceStatBonus()
        {
          power = PickUpModel_SmallBird1.LogEmotionCardAbility_SmallBird1.Pow
        });
      }

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        this.Destroy();
      }

      public override void OnDie()
      {
        base.OnDie();
        this.Destroy();
      }

      public override void Destroy()
      {
        base.Destroy();
        this.DestroyAura();
      }

      public void DestroyAura()
      {
        if (!((Object) this.aura != (Object) null))
          return;
        Object.Destroy((Object) this.aura);
        this.aura = (GameObject) null;
      }
    }
  }
}
}
