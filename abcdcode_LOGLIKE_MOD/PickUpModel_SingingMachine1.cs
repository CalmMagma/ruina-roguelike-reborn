// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_SingingMachine1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_SingingMachine1 : CreaturePickUpModel
{
  public PickUpModel_SingingMachine1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_SingingMachine1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_SingingMachine1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_SingingMachine1_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370221), (EmotionCardAbilityBase) new PickUpModel_SingingMachine1.LogEmotionCardAbility_SingingMachine1(), model);
  }

  public class LogEmotionCardAbility_SingingMachine1 : EmotionCardAbilityBase
  {
    public const int _dmgMin = 4;
    public const int _dmgMax = 8;
    public const int _brkDmgMin = 4;
    public const int _brkDmgMax = 8;
    public const int _heal = 15;
    public const int _bHeal = 15;

    public override void OnSelectEmotionOnce()
    {
      SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Singing_Atk");
      Util.LoadPrefab("Battle/CreatureCard/SingingMachineCard_play_particle", SingletonBehavior<BattleSceneRoot>.Instance.transform);
    }

    public override void OnKill(BattleUnitModel target)
    {
      base.OnKill(target);
      if (this._owner.faction != Faction.Player || target == null || target.faction != Faction.Enemy)
        return;
      this._owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Filter));
      this._owner.RecoverHP(15);
      this._owner.breakDetail.RecoverBreak(15);
    }

    public void Filter()
    {
      new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/SingingMachine_Filter_Special", false, 2f);
      SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/SingingMachine_Eat");
    }

    public override void BeforeGiveDamage(BattleDiceBehavior behavior)
    {
      int num = RandomUtil.Range(4, 8);
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        dmg = num
      });
    }

    public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
    {
      this._owner.breakDetail.TakeBreakDamage(RandomUtil.Range(4, 8), DamageType.Emotion);
    }
  }
}
}
