// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.GlobalLogueEffectBase
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using GameSave;
using LOR_DiceSystem;
using RogueLike_Mod_Reborn;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{
    /// <summary>
    /// Base class for inventory items.<br></br>
    /// Do mind the class name is saved to disk- please use unique class names to avoid conflicts.
    /// </summary>
    [HideFromItemCatalog]
    public class GlobalLogueEffectBase : Savable
    {
        /// <value>
        /// Override this with the ID provided within the effect's respective localization XML.
        /// </value>
        public virtual string KeywordId { get; }

        /// <value>
        /// Override this with the filename of the effect's icon. Defaults to <see cref="KeywordId"/> if not provided.
        /// </value>
        public virtual string KeywordIconId { get; }

        /// <summary>
        /// Used for storing persistent information to save file.<br></br>
        /// It is recommended to start the method like so:
        /// <code>SaveData data = base.GetSaveData();</code><br></br>
        /// As base.GetSaveData contains the TypeName of the effect,<br></br>
        /// which is necessary for loading the effect from a save file.
        /// </summary>
        /// <returns>The <see cref="SaveData"/> containing any data that </returns>
        public virtual SaveData GetSaveData()
        {
            SaveData saveData = new SaveData();
            saveData.AddData("TypeName", new SaveData(this.GetType().Name));
            return saveData;
        }

        public static GlobalLogueEffectBase CreateGlobalEffectBySave(SaveData save)
        {
            string stringSelf = save.GetData("TypeName").GetStringSelf();
            Debug.Log(("CGEBS tryfind : " + stringSelf));
            foreach (Assembly assem in LogLikeMod.GetAssemList())
            {
                foreach (System.Type type in assem.GetTypes())
                {
                    if (type.Name == stringSelf)
                    {
                        Debug.Log(("CGEBS find : " + stringSelf));
                        return Activator.CreateInstance(type) as GlobalLogueEffectBase;
                    }
                }
            }
            return (GlobalLogueEffectBase)null;
        }

        /// <summary>
        /// Used for loading persistent information from save file.<br></br>
        /// </summary>
        /// <param name="save">The SaveData for this effect that is being loaded.</param>
        public virtual void LoadFromSaveData(SaveData save)
        {
        }

        /// <summary>
        /// Runs immediately when the effect is added to the player's inventory.
        /// </summary>
        public virtual void AddedNew()
        {
        }

        /// <summary>
        /// Determines whether or not the item can stack.<br></br>
        /// <b>Does not prevent multiple copies of an item from being given.</b>
        /// </summary>
        public virtual bool CanDupliacte() => false;

        /// <summary>
        /// 
        /// </summary>
        public virtual void Destroy() => Singleton<GlobalLogueEffectManager>.Instance.RemoveEffect(this);

        public virtual void OnDestroy()
        {
        }

        public virtual void OnAddSubPlayer(UnitDataModel model)
        {
        }

        public virtual float CraftCostMultiple(CraftEffect effect) => 1f;

        public virtual LorId InvenAddCardChange(LorId baseid) => baseid;

        public virtual void RewardInStageInterrupt()
        {
        }

        public virtual void RewardClearStageInterrupt()
        {
        }

        public virtual void ChangeShopCard(ref DiceCardXmlInfo card)
        {
        }

        public virtual void ChangeCardReward(ref List<DiceCardXmlInfo> cardlist)
        {
        }

        public virtual int ChangeSuccCostValue() => 0;

        public virtual void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
        {
        }

        public virtual void ChangeRestChoice(MysteryBase currest, ref List<RewardPassiveInfo> choices)
        {
        }

        public virtual void OnRoundStart(StageController stage)
        {
        }

        public virtual float DmgFactor(
          BattleUnitModel model,
          int dmg,
          DamageType type = DamageType.ETC,
          KeywordBuf keyword = KeywordBuf.None)
        {
            return 1f;
        }

        public virtual void OnKillUnit(BattleUnitModel killer, BattleUnitModel target)
        {
        }

        public virtual void OnDieUnit(BattleUnitModel unit)
        {
        }

        public virtual void OnStartBattle(BattlePlayingCardDataInUnitModel card)
        {
        }

        public virtual void BeforeRollDice(BattleDiceBehavior behavior)
        {
        }

        public virtual void OnUseCard(BattlePlayingCardDataInUnitModel cardmodel)
        {
        }

        public virtual void ChangeShopCardList(ShopBase shop, ref CardDropValueXmlInfo list)
        {
        }

        public virtual void OnShopCardListCreate(ShopBase shop)
        {
        }

        public virtual void OnPickCardReward(List<DiceCardXmlInfo> cardlist, DiceCardXmlInfo pick)
        {
        }

        public virtual void OnSkipCardRewardChoose(List<DiceCardXmlInfo> cardlist)
        {
        }

        public virtual bool CanShopPurchase(ShopBase shop, ShopGoods goods) => true;

        public virtual void OnEnterShop(ShopBase shop)
        {
        }

        public virtual void OnLeaveShop(ShopBase shop)
        {
        }

        public virtual void OnEndBattle()
        {
        }

        public virtual void OnStartBattle()
        {
        }

        public virtual void OnStartBattleAfter()
        {
        }

        public virtual void OnCreateLibrarian(BattleUnitModel model)
        {
        }

        public virtual void OnCreateLibrarians()
        {
        }

        public virtual void OnClick()
        {
        }

        public virtual Sprite GetSprite()
        {
            Sprite sprite = null;
            string id;
            try
            {
                id = RMRCore.ClassIds[this.GetType().Assembly.FullName];
            }
            catch
            {
                return null;
            }
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    if (id == RMRCore.packageId)
                        sprite = LogLikeMod.ArtWorks[KeywordIconId ?? KeywordId];
                    else
                        sprite = LogLikeMod.ModdedArtWorks[(id, KeywordIconId ?? KeywordId)];
                }
                catch
                {
                    return null;
                }
            }
            return sprite;
        }

        public virtual string GetEffectName()
        {
            LogueEffectXmlInfo info = null;
            if (!string.IsNullOrEmpty(this.KeywordId))
            {
                try
                {
                    info = LogueEffectXmlList.Instance.GetEffectInfo(KeywordId, RMRCore.ClassIds[this.GetType().Assembly.FullName], this.GetStack());
                }
                catch
                {
                    info = null;
                }
            }
            return info == null ? "" : info.Name;
        }

        public virtual string GetEffectDesc()
        {
            LogueEffectXmlInfo info = null;
            if (!string.IsNullOrEmpty(this.KeywordId))
            {
                try
                {
                    info = LogueEffectXmlList.Instance.GetEffectInfo(KeywordId, RMRCore.ClassIds[this.GetType().Assembly.FullName], this.GetStack());
                }
                catch
                {
                    info = null;
                }
            }
            return info == null ? "" : info.Desc + "\n\n" + info.FlavorText;
        }

        public virtual string GetCredenzaEntry()
        {
            LogueEffectXmlInfo info = null;
            if (!string.IsNullOrEmpty(this.KeywordId))
            {
                try
                {
                    info = LogueEffectXmlList.Instance.GetEffectInfo(KeywordId, RMRCore.ClassIds[this.GetType().Assembly.FullName], this.GetStack());
                }
                catch
                {
                    info = null;
                }
            }
            return info == null ? this.GetEffectDesc() : info.CatalogDesc;
        }

        public virtual int GetStack() => -1;

        public virtual void OnCrit(BattleUnitModel critter, BattleUnitModel target)
        {
        }
    }
}
