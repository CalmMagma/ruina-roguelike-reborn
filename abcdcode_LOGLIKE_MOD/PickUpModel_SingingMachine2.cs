// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_SingingMachine2
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Sound;
using System;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_SingingMachine2 : CreaturePickUpModel
{
  public PickUpModel_SingingMachine2()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_SingingMachine2_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_SingingMachine2_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_SingingMachine2_FlaverText");
    this.level = 3;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370222), (EmotionCardAbilityBase) new PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2(), model);
  }

  public class LogEmotionCardAbility_SingingMachine2 : EmotionCardAbilityBase
  {
    public override void OnRoundStart()
    {
      base.OnRoundStart();
      this.GetBuf();
    }

    public void GetBuf()
    {
      if (!(this._owner.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm)) is PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm singingMachineRhythm))
        this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm(1));
      else
        ++singingMachineRhythm.stack;
    }

    public class BattleUnitBuf_Emotion_SingingMachine_Rhythm : BattleUnitBuf
    {
      public const int _brkDmgMin = 2;
      public const int _brkDmgMax = 5;
      public const int _str = 1;
      public const float _prob = 0.25f;
      public Battle.CreatureEffect.CreatureEffect _effect;
      public int reserve;

      public override string keywordId => "SingingMachine_Rhythm";

      public static int BrkDmg => RandomUtil.Range(2, 5);

      public static bool Prob => (double) RandomUtil.valueForProb < 0.25;

      public BattleUnitBuf_Emotion_SingingMachine_Rhythm(int value = 0)
      {
        this.stack = value;
        this.reserve = Mathf.Max(0, 1 - value);
      }

      public override void Init(BattleUnitModel owner)
      {
        base.Init(owner);
        if (this.stack <= 0)
          return;
        this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1, this._owner);
      }

      public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
      {
        base.OnTakeDamageByAttack(atkDice, dmg);
        if (this.stack <= 0)
          return;
        this._owner.TakeBreakDamage(PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm.BrkDmg, DamageType.Buf, this._owner);
      }

      public override void OnRoundEnd()
      {
        base.OnRoundEnd();
        if (this.stack > 0)
          --this.stack;
        this.stack += this.reserve;
        this.reserve = 0;
        if (this.stack <= 0)
          this.Destroy();
        else
          this._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this._owner);
      }

      public override void OnRoundStart()
      {
        base.OnRoundStart();
        if (!((UnityEngine.Object) this._effect == (UnityEngine.Object) null))
          return;
        this._effect = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("4/SingingMachine_NoteAura", 1f, this._owner.view, (BattleUnitView) null);
        Battle.CreatureEffect.CreatureEffect effect = this._effect;
        if ((UnityEngine.Object) effect != (UnityEngine.Object) null)
          effect.SetLayer("Character");
        SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Singing_Rhythm");
        if ((UnityEngine.Object) soundEffectPlayer == (UnityEngine.Object) null)
          return;
        soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
      }

      public override void Destroy()
      {
        if ((UnityEngine.Object) this._effect != (UnityEngine.Object) null)
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) this._effect.gameObject);
          this._effect = (Battle.CreatureEffect.CreatureEffect) null;
        }
        base.Destroy();
      }

      public override void OnLayerChanged(string layerName)
      {
        if (!((UnityEngine.Object) this._effect != (UnityEngine.Object) null))
          return;
        this._effect.SetLayer(layerName);
      }

      public void Reserve(int value = 1) => this.reserve += value;

      public override void OnSuccessAttack(BattleDiceBehavior behavior)
      {
        base.OnSuccessAttack(behavior);
        if (!PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm.Prob)
          return;
        this._owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Singing_Rhythm");
        this.Ability();
      }

      public void Ability()
      {
        List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(Faction.Player);
        if (aliveList.Count == 0)
          return;
        List<BattleUnitModel> battleUnitModelList = new List<BattleUnitModel>();
        foreach (BattleUnitModel battleUnitModel in aliveList)
        {
          if (battleUnitModel.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm)) == null)
            battleUnitModelList.Add(battleUnitModel);
        }
        if (battleUnitModelList.Count > 0)
        {
          battleUnitModelList[UnityEngine.Random.Range(0, battleUnitModelList.Count)].bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm());
        }
        else
        {
          battleUnitModelList.AddRange((IEnumerable<BattleUnitModel>) aliveList);
          if (battleUnitModelList.Count <= 0 || !(battleUnitModelList[UnityEngine.Random.Range(0, battleUnitModelList.Count)].bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>) (x => x is PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm)) is PickUpModel_SingingMachine2.LogEmotionCardAbility_SingingMachine2.BattleUnitBuf_Emotion_SingingMachine_Rhythm singingMachineRhythm))
            return;
          singingMachineRhythm.Reserve();
        }
      }
    }
  }
}
}
