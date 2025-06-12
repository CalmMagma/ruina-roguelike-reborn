// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.PickUpModelBase
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using RogueLike_Mod_Reborn;
using System;
using System.Collections.Generic;


namespace abcdcode_LOGLIKE_MOD
{

    public class PickUpModelBase
    {
        public LorId id;
        public RewardPassiveInfo rewardinfo;
        public string Name = "";
        public string Desc = "";
        public string FlaverText = "";
        public string ArtWork = "";

        /// <summary>
        /// Do <b>NOT</b> forget to inherit this constructor on your derived <see cref="PickUpModelBase"/>.
        /// <code>
        /// // You can do it like this:
        /// public class PickUpModel_MyCoolItem : PickUpModelBase
        /// {
        ///     public PickUpModel_MyCoolItem() : base() // do not forget this : base() part
        ///     {
        ///         this.id = new LorId(myModInitializer.packageId, 1984);
        ///         this.rewardinfo = RewardPassivesList.Instance.GetPassiveInfo(new LorId(myModInitializer.packageId, 1984));
        ///     }
        /// }</code>
        /// </summary>
        public PickUpModelBase()
        {
            if (!string.IsNullOrEmpty(this.KeywordId))
            {
                var info = LogueEffectXmlList.Instance.GetEffectInfo(KeywordId, RMRCore.ClassIds[this.GetType().Assembly.FullName]);
                if (info != null)
                {
                    this.Name = info.Name;
                    this.Desc = info.Desc;
                    this.FlaverText = info.FlavorText;
                    this.ArtWork = KeywordIconId ?? KeywordId;
                }
            }
        }

        /// <value>
        /// Override this with the ID provided within the effect's respective localization XML.
        /// </value>
        public virtual string KeywordId { get; }

        /// <value>
        /// Override this with the filename of the effect's icon. Defaults to <see cref="KeywordId"/> if not provided.
        /// </value>
        public virtual string KeywordIconId { get; }

        public virtual Rarity ItemRarity 
        {
            get
            {
                return Rarity.Common;
            }
        }

        public virtual string GetCredenzaEntry()
        {
            LogueEffectXmlInfo info = null;
            if (!string.IsNullOrEmpty(this.KeywordId))
            {
                try
                {
                    info = LogueEffectXmlList.Instance.GetEffectInfo(KeywordId, RMRCore.ClassIds[this.GetType().Assembly.FullName]);
                }
                catch
                {
                    info = null;
                }
            }
            return info == null ? this.Desc : info.CatalogDesc;
        }

        public virtual void LoadFromSaveData(LogueStageInfo stage)
        {
        }

        public void GivePassive(LorId id, BattleUnitModel model)
        {
            model.UnitData.unitData.bookItem.ClassInfo.EquipEffect.PassiveList.Add(id);
            model.UnitData.unitData.bookItem.TryGainUniquePassive();
            LogueBookModels.playersperpassives[model.UnitData.unitData].Add(id);
        }

        public void RemovePassive(LorId id, BattleUnitModel model)
        {
            model.UnitData.unitData.bookItem.ClassInfo.EquipEffect.PassiveList.RemoveAll((Predicate<LorId>)(x => x == id));
            model.UnitData.unitData.bookItem.TryGainUniquePassive();
            LogueBookModels.playersperpassives[model.UnitData.unitData].RemoveAll((Predicate<LorId>)(x => x == id));
        }

        public virtual List<BattleUnitModel> GetPickupTarget() => (List<BattleUnitModel>)null;

        public virtual bool IsCanPickUp(UnitDataModel target)
        {
            RewardPassiveInfo passiveInfo = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(this.id);
            if (passiveInfo.passivetype == RewardPassiveType.Nolimit)
                return true;
            if (passiveInfo.passivetype == RewardPassiveType.EachOther)
            {
                if (!LogueBookModels.playersPick.ContainsKey(target))
                    return true;
                if (LogueBookModels.playersPick[target].Contains(this.id))
                    return false;
            }
            if (passiveInfo.passivetype == RewardPassiveType.OnlyOne)
            {
                foreach (KeyValuePair<UnitDataModel, List<LorId>> keyValuePair in LogueBookModels.playersPick)
                {
                    if (keyValuePair.Value.Contains(this.id))
                        return false;
                }
            }
            return true;
        }

        public virtual void OnPickUp()
        {
        }

        public virtual void OnPickUp(BattleUnitModel model)
        {
        }

        public static bool CheckDead(UnitDataModel model)
        {
            return BattleObjectManager.instance.GetAliveList().Find((Predicate<BattleUnitModel>)(x => x.UnitData.unitData == model)) == null;
        }
    }
}
