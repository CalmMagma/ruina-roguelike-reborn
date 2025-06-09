// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_RedShoes1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using Battle.CreatureEffect;
using Sound;
using System;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_RedShoes1 : CreaturePickUpModel
{
  public PickUpModel_RedShoes1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_RedShoes1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_RedShoes1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_RedShoes1_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370131), (EmotionCardAbilityBase) new PickUpModel_RedShoes1.LogEmotionCardAbility_RedShoes1(), model);
  }

  public class LogEmotionCardAbility_RedShoes1 : EmotionCardAbilityBase
  {
    public int value;
    public const int _powMin = 1;
    public const int _powMax = 2;

    public override void OnRoundStart()
    {
      foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
      {
        if (alive.faction != this._owner.faction && (double) RandomUtil.valueForProb < 0.5)
        {
          alive.bufListDetail.AddBufWithoutDuplication((BattleUnitBuf) new PickUpModel_RedShoes1.LogEmotionCardAbility_RedShoes1.BattleUnitBuf_redshoes(this._emotionCard, alive, this._owner, this));
          SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedShoes_On");
          if ((UnityEngine.Object) soundEffectPlayer != (UnityEngine.Object) null)
            soundEffectPlayer.SetGlobalPosition(alive.view.WorldPosition);
        }
      }
    }

    public override void BeforeRollDice(BattleDiceBehavior behavior)
    {
      if (behavior.card.target == null || behavior.card.target.bufListDetail.GetKewordBufStack(KeywordBuf.RedShoes) <= 0)
        return;
      this.value = RandomUtil.Range(1, 2);
      behavior.ApplyDiceStatBonus(new DiceStatBonus()
      {
        power = this.value
      });
    }

    public override void OnSucceedAttack(BattleDiceBehavior behavior)
    {
      if (this.value > 0)
        this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 1, ResultOption.Default, this.value);
      this.value = 0;
    }

    public CreatureEffect_FaceAttacher MakeFaceEffect(BattleUnitView target)
    {
      CreatureEffect_FaceAttacher effectFaceAttacher = this.MakeEffect("3/RedShoes_Attract", apply: false) as CreatureEffect_FaceAttacher;
      effectFaceAttacher.AttachTarget(target);
      return effectFaceAttacher;
    }

    public class BattleUnitBuf_redshoes : BattleUnitBuf
    {
      public BattleUnitModel _target;
      public BattleEmotionCardModel _emotionCard;
      public PickUpModel_RedShoes1.LogEmotionCardAbility_RedShoes1 _script;
      public List<CreatureEffect_FaceAttacher> _faceEffect = new List<CreatureEffect_FaceAttacher>();
      public int value;
      public const int _dmgMin = 2;
      public const int _dmgMax = 4;

      public override KeywordBuf bufType => KeywordBuf.RedShoes;

      public BattleUnitBuf_redshoes(
        BattleEmotionCardModel emotionCard,
        BattleUnitModel owner,
        BattleUnitModel target,
        PickUpModel_RedShoes1.LogEmotionCardAbility_RedShoes1 script)
      {
        this._emotionCard = emotionCard;
        this._target = target;
        this._script = script;
        try
        {
          Debug.Log($"Tagetting: {owner.view.charAppearance.gameObject.name} to {target.view.charAppearance.gameObject.name}");
        }
        catch
        {
          Debug.LogError("Failed to print targetting");
        }
      }

      public override void BeforeRollDice(BattleDiceBehavior behavior)
      {
        this.value = RandomUtil.Range(2, 4);
        behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
          dmg = this.value
        });
      }

      public override void OnRoundStart()
      {
        CreatureEffect_FaceAttacher effectFaceAttacher = this._script.MakeFaceEffect(this._owner.view);
        effectFaceAttacher.SetLayer("Character");
        if (!(bool) (UnityEngine.Object) effectFaceAttacher)
          return;
        this._faceEffect.Add(effectFaceAttacher);
      }

      public override void OnSuccessAttack(BattleDiceBehavior behavior)
      {
        this._owner.battleCardResultLog?.SetEmotionAbility(true, this._emotionCard, 1, ResultOption.Default, this.value);
      }

      public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlot)
      {
        return this._target.IsDead() ? (BattleUnitModel) null : this._target;
      }

      public override void OnRoundEnd()
      {
        foreach (CreatureEffect_FaceAttacher effectFaceAttacher in this._faceEffect)
        {
          if ((UnityEngine.Object) effectFaceAttacher != (UnityEngine.Object) null)
            effectFaceAttacher.ManualDestroy();
        }
        this._faceEffect.Clear();
        this.Destroy();
      }

      public override void Destroy()
      {
        foreach (CreatureEffect_FaceAttacher effectFaceAttacher in this._faceEffect)
        {
          if ((UnityEngine.Object) effectFaceAttacher != (UnityEngine.Object) null)
            effectFaceAttacher.ManualDestroy();
        }
        this._faceEffect.Clear();
        base.Destroy();
      }

      public override void OnLayerChanged(string layerName)
      {
        foreach (Battle.CreatureEffect.CreatureEffect creatureEffect in this._faceEffect)
          creatureEffect.SetLayer(layerName);
      }
    }
  }
}
}
