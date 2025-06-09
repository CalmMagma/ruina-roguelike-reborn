// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_LumberJack1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_LumberJack1 : CreaturePickUpModel
{
  public PickUpModel_LumberJack1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_LumberJack1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_LumberJack1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_LumberJack1_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370171), (EmotionCardAbilityBase) new PickUpModel_LumberJack1.LogEmotionCardAbility_LumberJack1(), model);
  }

  public class LogEmotionCardAbility_LumberJack1 : EmotionCardAbilityBase
  {
    public const int _stack = 3;
    public int pp;

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (this.pp >= 3)
        this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_LumberJack1.LogEmotionCardAbility_LumberJack1.BattleUnitBuf_Lumberjack_emotion());
      this.pp = 0;
    }

    public override void OnRoundEnd()
    {
      base.OnRoundEnd();
      this.pp = this._owner.PlayPoint;
    }

    public class BattleUnitBuf_Lumberjack_emotion : BattleUnitBuf
    {
      public const int _powMin = 1;
      public const int _powMax = 2;
      public SoundEffectPlayer sound;
      public GameObject aura;

      public override string keywordId => "Lumberjack_Cut";

      public override string keywordIconId => "Lumberjack_Heart";

      public int Pow => RandomUtil.Range(1, 2);

      public override void BeforeRollDice(BattleDiceBehavior behavior)
      {
        base.BeforeRollDice(behavior);
        if (!this.IsAttackDice(behavior.Detail))
          return;
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          power = this.Pow
        });
      }

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        Battle.CreatureEffect.CreatureEffect fxCreatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Mind", 1f, owner.view, owner.view);
        this.aura = (Object) fxCreatureEffect != (Object) null ? fxCreatureEffect.gameObject : (GameObject) null;
        this.sound = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/WoodMachine_Default", true, parent: owner.view.characterRotationCenter.transform);
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
        if ((Object) this.aura != (Object) null)
        {
          Object.Destroy((Object) this.aura);
          this.aura = (GameObject) null;
        }
        if (!((Object) this.sound != (Object) null))
          return;
        this.sound.ManualDestroy();
        this.sound = (SoundEffectPlayer) null;
      }
    }
  }
}
}
