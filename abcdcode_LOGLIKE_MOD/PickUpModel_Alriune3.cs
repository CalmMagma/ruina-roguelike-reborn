// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_Alriune3
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_Alriune3 : CreaturePickUpModel
{
  public PickUpModel_Alriune3()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_Alriune3_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_Alriune3_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_Alriune3_FlaverText");
    this.level = 4;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370343), (EmotionCardAbilityBase) new PickUpModel_Alriune3.LogEmotionCardAbility_Alriune3(), model);
  }

  public class LogEmotionCardAbility_Alriune3 : EmotionCardAbilityBase
  {
    public override void OnRoundStart()
    {
      base.OnRoundStart();
      List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(Faction.Enemy);
      if (aliveList.Count <= 0)
        return;
      aliveList[Random.Range(0, aliveList.Count)].bufListDetail.AddBuf((BattleUnitBuf) new EmotionCardAbility_alriune3.BattleUnitBuf_Emotion_Alriune(this._owner));
    }

    public class BattleUnitBuf_Emotion_Alriune : BattleUnitBuf
    {
      public const int _bDmgMin = 3;
      public const int _bDmgMax = 7;
      public const int _maxCnt = 4;
      public BattleUnitModel _target;
      public int cnt;
      public Battle.CreatureEffect.CreatureEffect _aura;

      public static int BDmg => RandomUtil.Range(3, 7);

      public override string keywordId => "Alriune_Flower";

      public override string keywordIconId => "Alriune_Petal";

      public BattleUnitBuf_Emotion_Alriune(BattleUnitModel target)
      {
        this._target = target;
        this.stack = 0;
      }

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        this._aura = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_Spring", 1f, this._owner.view, this._owner.view);
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
        if (!((Object) this._aura != (Object) null))
          return;
        Object.Destroy((Object) this._aura.gameObject);
        this._aura = (Battle.CreatureEffect.CreatureEffect) null;
      }

      public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
      {
        base.OnTakeDamageByAttack(atkDice, dmg);
        BattleUnitModel owner = atkDice?.card?.owner;
        if (owner == null || owner != this._target || this.cnt >= 4)
          return;
        ++this.cnt;
        if (this.cnt >= 4)
        {
          foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            alive.TakeBreakDamage(PickUpModel_Alriune3.LogEmotionCardAbility_Alriune3.BattleUnitBuf_Emotion_Alriune.BDmg, DamageType.Buf, this._owner);
          this._target?.bufListDetail.AddBuf((BattleUnitBuf) new EmotionCardAbility_alriune3.BattleUnitBuf_Emotion_Alriune2());
        }
      }

      public void Curtain()
      {
        Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/4_N/FX_IllusionCard_4_N_SpringAct");
        if ((Object) original != (Object) null)
        {
          Battle.CreatureEffect.CreatureEffect creatureEffect = Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleManagerUI>.Instance.EffectLayer);
          if (((Object) creatureEffect != (Object) null ? (Object) creatureEffect.gameObject.GetComponent<AutoDestruct>() : (Object) null) == (Object) null)
          {
            AutoDestruct autoDestruct = (Object) creatureEffect != (Object) null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct) null;
            if ((Object) autoDestruct != (Object) null)
              autoDestruct.time = 3f;
          }
        }
        SoundEffectPlayer.PlaySound("Creature/Ali_curtain");
      }
    }

    public class BattleUnitBuf_Emotion_Alriune2 : BattleUnitBuf
    {
      public bool added = true;
      public bool effect;

      public override string keywordId => "NoTargeting";

      public override string keywordIconId => "Alriune_Attacker";

      public override bool IsTargetable() => this.added;

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        if (this.added)
          this.added = false;
        else
          this.Destroy();
      }

      public override void OnRoundStart()
      {
        base.OnRoundStart();
        if (this.effect)
          return;
        this.effect = true;
        this.Curtain();
      }

      public void Curtain()
      {
        Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/4_N/FX_IllusionCard_4_N_SpringAct");
        if (!((Object) original != (Object) null))
          return;
        Battle.CreatureEffect.CreatureEffect creatureEffect = Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleManagerUI>.Instance.EffectLayer);
        if (((Object) creatureEffect != (Object) null ? (Object) creatureEffect.gameObject.GetComponent<AutoDestruct>() : (Object) null) == (Object) null)
        {
          AutoDestruct autoDestruct = (Object) creatureEffect != (Object) null ? creatureEffect.gameObject.AddComponent<AutoDestruct>() : (AutoDestruct) null;
          if ((Object) autoDestruct != (Object) null)
            autoDestruct.time = 3f;
        }
      }
    }
  }
}
}
