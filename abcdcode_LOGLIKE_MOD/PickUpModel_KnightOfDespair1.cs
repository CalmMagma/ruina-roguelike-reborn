// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModel_KnightOfDespair1
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using Sound;
using System;
using System.Collections.Generic;
using UnityEngine;

 
namespace abcdcode_LOGLIKE_MOD {

public class PickUpModel_KnightOfDespair1 : CreaturePickUpModel
{
  public PickUpModel_KnightOfDespair1()
  {
    this.Name = TextDataModel.GetText("PickUpCreature_KnightOfDespair1_Name");
    this.Desc = TextDataModel.GetText("PickUpCreature_KnightOfDespair1_Desc");
    this.FlaverText = TextDataModel.GetText("PickUpCreature_KnightOfDespair1_FlaverText");
    this.level = 2;
  }

  public override bool IsCanPickUp(UnitDataModel target) => !PickUpModelBase.CheckDead(target);

  public override void OnPickUp(BattleUnitModel model)
  {
    base.OnPickUp(model);
    CreaturePickUpModel.ApplyEmotionCard(new LorId(LogLikeMod.ModId, 15370151), (EmotionCardAbilityBase) new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1(), model);
  }

  public class LogEmotionCardAbility_KnightOfDespair1 : EmotionCardAbilityBase
  {
    public List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo> _dmgInfos = new List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>();
    public List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo> _breakdmgInfos = new List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>();

    public override void OnWaveStart()
    {
      base.OnWaveStart();
      this._dmgInfos = new List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>();
      this._breakdmgInfos = new List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>();
      this._dmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
      {
        type = BehaviourDetail.Slash
      });
      this._dmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
      {
        type = BehaviourDetail.Penetrate
      });
      this._dmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
      {
        type = BehaviourDetail.Hit
      });
      this._breakdmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
      {
        type = BehaviourDetail.Slash
      });
      this._breakdmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
      {
        type = BehaviourDetail.Penetrate
      });
      this._breakdmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
      {
        type = BehaviourDetail.Hit
      });
    }

    public override void OnSelectEmotion()
    {
      base.OnSelectEmotion();
      this._dmgInfos = new List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>();
      this._breakdmgInfos = new List<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>();
      this._dmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
      {
        type = BehaviourDetail.Slash
      });
      this._dmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
      {
        type = BehaviourDetail.Penetrate
      });
      this._dmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
      {
        type = BehaviourDetail.Hit
      });
      this._breakdmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
      {
        type = BehaviourDetail.Slash
      });
      this._breakdmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
      {
        type = BehaviourDetail.Penetrate
      });
      this._breakdmgInfos.Add(new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo()
      {
        type = BehaviourDetail.Hit
      });
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.BattleUnitBuf_Gaho());
      new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KnightOfDespair_Gaho", false, 2f);
      try
      {
        SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/KnightOfDespair_Gaho");
        if (!((UnityEngine.Object) soundEffectPlayer != (UnityEngine.Object) null))
          return;
        soundEffectPlayer.SetGlobalPosition(this._owner.view.WorldPosition);
      }
      catch
      {
      }
    }

    public override void OnRoundStart()
    {
      this._dmgInfos.Sort((Comparison<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>) ((x, y) => y.dmg - x.dmg));
      this._breakdmgInfos.Sort((Comparison<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>) ((x, y) => y.dmg - x.dmg));
      BehaviourDetail behaviourDetail1 = BehaviourDetail.None;
      BehaviourDetail behaviourDetail2 = BehaviourDetail.None;
      if (this._dmgInfos[0].dmg > 0)
        behaviourDetail1 = this._dmgInfos[0].type;
      if (this._breakdmgInfos[0].dmg > 0)
        behaviourDetail2 = this._breakdmgInfos[0].type;
      this._owner.bufListDetail.AddBuf((BattleUnitBuf) new PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.BattleUnitBuf_resists()
      {
        hpTarget = behaviourDetail1,
        bpTarget = behaviourDetail2
      });
      this._dmgInfos.ForEach((Action<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>) (x => x.dmg = 0));
      this._breakdmgInfos.ForEach((Action<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>) (x => x.dmg = 0));
    }

    public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
    {
      PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo dmgInfo = this._dmgInfos.Find((Predicate<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>) (x => x.type == atkDice.Detail));
      if (dmgInfo == null)
        return;
      dmgInfo.dmg += dmg;
    }

    public override void OnTakeBreakDamageByAttack(BattleDiceBehavior atkDice, int breakdmg)
    {
      PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo dmgInfo = this._breakdmgInfos.Find((Predicate<PickUpModel_KnightOfDespair1.LogEmotionCardAbility_KnightOfDespair1.DmgInfo>) (x => x.type == atkDice.Detail));
      if (dmgInfo == null)
        return;
      dmgInfo.dmg += breakdmg;
    }

    public class BattleUnitBuf_resists : BattleUnitBuf
    {
      public BehaviourDetail hpTarget = BehaviourDetail.None;
      public BehaviourDetail bpTarget = BehaviourDetail.None;

      public override bool Hide => true;

      public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
      {
        if (this.hpTarget == BehaviourDetail.None)
          return base.GetResistHP(origin, detail);
        return this.hpTarget == detail ? AtkResist.Endure : base.GetResistHP(origin, detail);
      }

      public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
      {
        if (this.bpTarget == BehaviourDetail.None)
          return base.GetResistBP(origin, detail);
        return this.bpTarget == detail ? AtkResist.Endure : base.GetResistBP(origin, detail);
      }

      public override void OnRoundEnd() => this.Destroy();
    }

    public class DmgInfo
    {
      public BehaviourDetail type;
      public int dmg;
    }

    public class BattleUnitBuf_Gaho : BattleUnitBuf
    {
      public override string keywordId => "Gaho";
    }
  }
}
}
