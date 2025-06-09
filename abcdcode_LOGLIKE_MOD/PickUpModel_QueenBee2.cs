// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_QueenBee2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;
using System.Collections.Generic;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_QueenBee2 : CreaturePickUpModel
{
  public PickUpModel_QueenBee2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_QueenBee2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_QueenBee2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_QueenBee2_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370312), (EmotionCardAbilityBase) new PickUpModel_QueenBee2.LogEmotionCardAbility_QueenBee2(), model);
  }

  public class LogEmotionCardAbility_QueenBee2 : EmotionCardAbilityBase
  {
    public Dictionary<BattleUnitModel, int> dmgData = new Dictionary<BattleUnitModel, int>();

    public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
    {
      base.OnTakeDamageByAttack(atkDice, dmg);
      BattleUnitModel owner = atkDice.owner;
      if (owner == null || owner.faction != Faction.Enemy)
        return;
      if (!this.dmgData.ContainsKey(owner))
        this.dmgData.Add(owner, dmg);
      else
        this.dmgData[owner] += dmg;
    }

    public override void OnRoundStart()
    {
      base.OnRoundStart();
      if (this.dmgData.Count > 0)
      {
        int num = 0;
        BattleUnitModel battleUnitModel = (BattleUnitModel) null;
        foreach (KeyValuePair<BattleUnitModel, int> keyValuePair in this.dmgData)
        {
          if (keyValuePair.Value > num && !keyValuePair.Key.IsDead())
          {
            num = keyValuePair.Value;
            battleUnitModel = keyValuePair.Key;
          }
        }
        if (battleUnitModel != null)
        {
          battleUnitModel.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_QueenBee2.LogEmotionCardAbility_QueenBee2.BattleUnitBuf_queenbee_punish());
          SoundEffectPlayer.PlaySound("Creature/QueenBee_AtkMode");
        }
      }
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this._owner.faction))
      {
        if (alive != this._owner)
          alive.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_QueenBee2.LogEmotionCardAbility_QueenBee2.BattleUnitBuf_queenbee_attacker());
      }
      this.dmgData.Clear();
    }

    public class BattleUnitBuf_queenbee_punish : BattleUnitBuf
    {
      public Battle.CreatureEffect.CreatureEffect _aura;

      public override string keywordId => "Queenbee_Punish";

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("1_M/FX_IllusionCard_1_M_BeeMark", 1f, owner.view, owner.view);
      }

      public override void OnDie()
      {
        base.OnDie();
        this.Destroy();
      }

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        this.Destroy();
      }

      public override void Destroy()
      {
        base.Destroy();
        this.DestroyAura();
      }

      public void DestroyAura()
      {
        if (!((UnityEngine.Object) this._aura != (UnityEngine.Object) null))
          return;
        UnityEngine.Object.Destroy((UnityEngine.Object) this._aura.gameObject);
        this._aura = (Battle.CreatureEffect.CreatureEffect) null;
      }
    }

    public class BattleUnitBuf_queenbee_attacker : BattleUnitBuf
    {
      public const int _dmgMin = 2;
      public const int _dmgMax = 4;

      public static int Dmg => RandomUtil.Range(2, 4);

      public override bool Hide => true;

      public override string keywordId => "Queenbee_Punish";

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        this.hide = true;
      }

      public override void BeforeRollDice(BattleDiceBehavior behavior)
      {
        base.BeforeRollDice(behavior);
        BattleUnitModel target = behavior?.card?.target;
        if (target == null || target.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_QueenBee2.LogEmotionCardAbility_QueenBee2.BattleUnitBuf_queenbee_punish)) == null)
          return;
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          dmg = PickUpModel_QueenBee2.LogEmotionCardAbility_QueenBee2.BattleUnitBuf_queenbee_attacker.Dmg
        });
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
