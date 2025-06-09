// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_ChildofGalaxy2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_ChildofGalaxy2 : CreaturePickUpModel
{
  public PickUpModel_ChildofGalaxy2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_ChildofGalaxy2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_ChildofGalaxy2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_ChildofGalaxy2_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370142), (EmotionCardAbilityBase) new PickUpModel_ChildofGalaxy2.LogEmotionCardAbility_ChildofGalaxy2(), model);
  }

  public class LogEmotionCardAbility_ChildofGalaxy2 : EmotionCardAbilityBase
  {
    public Battle.CreatureEffect.CreatureEffect _effect;
    public List<Battle.CreatureEffect.CreatureEffect> _damagedEffects = new List<Battle.CreatureEffect.CreatureEffect>();

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      base.BeforeRollDice(behavior);
      if (behavior == null)
        return;
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = 1
      });
    }

    public override void OnDie(BattleUnitModel killer)
    {
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
      {
        if (alive != this._owner)
        {
          float hp = alive.hp;
          int v = Mathf.Min(this._owner.MaxHp / 2, 60);
          int damage = alive.TakeDamage(v, DamageType.Emotion, this._owner);
          alive.view.PrintBloodSprites(damage, hp);
          Battle.CreatureEffect.CreatureEffect creatureEffect = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("4/GalaxyBoy_Damaged", 1f, alive.view, (BattleUnitView) null, 2f);
          creatureEffect.SetLayer("Character");
          this._damagedEffects.Add(creatureEffect);
          creatureEffect.gameObject.SetActive(false);
          creatureEffect.SetLayer("Effect");
          alive.view.Damaged(damage, BehaviourDetail.None, 0, (BattleUnitModel) null);
        }
      }
      this._effect = this.MakeEffect("4/GalaxyBoy_Dust", destroyTime: 3f);
      Battle.CreatureEffect.CreatureEffect effect1 = this._effect;
      if ((Object) effect1 != (Object) null)
        effect1.gameObject.SetActive(false);
      Battle.CreatureEffect.CreatureEffect effect2 = this._effect;
      if ((Object) effect2 != (Object) null)
        effect2.AttachEffectLayer();
      this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/GalaxyBoy_Cry");
    }

    public override void OnPrintEffect(BattleDiceBehavior behavior)
    {
      if (!(bool) (Object) this._effect)
        return;
      this._effect.gameObject.SetActive(true);
      this._effect = (Battle.CreatureEffect.CreatureEffect) null;
      foreach (Component damagedEffect in this._damagedEffects)
        damagedEffect.gameObject.SetActive(true);
      this._damagedEffects.Clear();
    }
  }
}
}
