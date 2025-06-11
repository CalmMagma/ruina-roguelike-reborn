// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogCardUpgradeManager
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace abcdcode_LOGLIKE_MOD
{

    public class LogCardUpgradeManager : Singleton<LogCardUpgradeManager>
    {
        public List<System.Type> UpgradeInfoCache;
        public static string UpgradeKeyword = "<LogUpgrade>";
        public Dictionary<LorId, Dictionary<int, UpgradeBase>> UpgradeInfoDic = new Dictionary<LorId, Dictionary<int, UpgradeBase>>();

        public void SetInfoCache()
        {
            this.UpgradeInfoCache = new List<System.Type>();
            foreach (Assembly assem in LogLikeMod.GetAssemList())
            {
                foreach (System.Type type in assem.GetTypes())
                {
                    if (type.Name.Contains("UpgradeModel"))
                        this.UpgradeInfoCache.Add(type);
                }
            }
        }

        public UpgradeBase FindUpgradeInfo(LorId cardid) => this.FindUpgradeInfo(cardid, 0, 1);

        public DiceCardXmlInfo GetUpgradeCard(LorId cardid) => this.GetUpgradeCard(cardid, 0, 1);

        public void ReLoadCurAllUpgradeCard()
        {
            foreach (KeyValuePair<LorId, Dictionary<int, UpgradeBase>> keyValuePair1 in this.UpgradeInfoDic)
            {
                if (keyValuePair1.Value != null)
                {
                    foreach (KeyValuePair<int, UpgradeBase> keyValuePair2 in keyValuePair1.Value)
                        ItemXmlDataList.instance.LogAddModCard(keyValuePair2.Value.GetUpgradeInfo(keyValuePair2.Key, 1));
                }
            }
        }

        public Dictionary<int, DiceCardXmlInfo> GetAllUpgradesCard(LorId cardId, int count = 1)
        {
            Dictionary<int, DiceCardXmlInfo> dictionary = new Dictionary<int, DiceCardXmlInfo>();
            Dictionary<int, UpgradeBase> upgrades;
            if (this.UpgradeInfoDic.TryGetValue(cardId, out upgrades))
            {
                foreach (KeyValuePair<int, UpgradeBase> pair in upgrades)
                {
                    DiceCardXmlInfo upgradeInfo = pair.Value.GetUpgradeInfo(pair.Key, count);
                    ItemXmlDataList.instance.LogAddModCard(upgradeInfo);
                    dictionary.Add(pair.Key, upgradeInfo);
                }
            }
            
            foreach (System.Type type in this.UpgradeInfoCache.ToArray())
            {
                try
                {
                    if (Activator.CreateInstance(type) is UpgradeBase instance)
                    {
                        instance.Log("Created Custom UpgradeInfo");
                        instance.Init();
                        instance.baseinfo = ItemXmlDataList.instance.GetCardItem(instance.baseid, true);
                        if (instance.baseinfo == null)
                            this.Log($"Error! baseinfo NULL cardid : {cardId.packageId} _ {cardId.id.ToString()} _ {instance.index}");
                        if (!this.UpgradeInfoDic.TryGetValue(instance.baseid, out Dictionary<int, UpgradeBase> _))
                            this.UpgradeInfoDic.Add(instance.baseid, new Dictionary<int, UpgradeBase>()
                            {
                                {
                                    instance.index,
                                    instance
                                }
                            });
                        else
                            this.UpgradeInfoDic[instance.baseid][instance.index] = instance;
                        if (!instance.CanRepeatUpgrade())
                            this.UpgradeInfoCache.Remove(type);
                        if (instance.baseid == cardId)
                        {
                            DiceCardXmlInfo upgradeInfo = instance.GetUpgradeInfo(instance.index, count);
                            ItemXmlDataList.instance.LogAddModCard(upgradeInfo);
                            dictionary.Add(instance.index, upgradeInfo);
                        }
                    }
                }
                catch
                {
                    this.Log("Error Catched by Create " + type.Name);
                }
            }

            return dictionary;
        }

        public DiceCardXmlInfo GetUpgradeCard(LorId cardid, int index = 0, int count = 1)
        {
            Dictionary<int, UpgradeBase> dictionary;
            if (this.UpgradeInfoDic.TryGetValue(cardid, out dictionary) && dictionary.TryGetValue(index, out UpgradeBase _))
            {
                DiceCardXmlInfo upgradeInfo = dictionary[index].GetUpgradeInfo(index, count);
                ItemXmlDataList.instance.LogAddModCard(upgradeInfo);
                return upgradeInfo;
            }
            DiceCardXmlInfo upgradeInfo1 = this.FindUpgradeInfo(cardid, index, count).GetUpgradeInfo(index, count);
            ItemXmlDataList.instance.LogAddModCard(upgradeInfo1);
            return upgradeInfo1;
        }

        public UpgradeBase FindUpgradeInfo(LorId cardid, int index = 0, int count = 1)
        {
            if (this.UpgradeInfoCache == null)
                this.SetInfoCache();
            if (this.UpgradeInfoCache.Count > 0)
            {
                foreach (System.Type type in this.UpgradeInfoCache.ToArray())
                {
                    try
                    {
                        if (Activator.CreateInstance(type) is UpgradeBase instance)
                        {
                            instance.Log("Created Custom UpgradeInfo");
                            instance.Init();
                            instance.baseinfo = ItemXmlDataList.instance.GetCardItem(instance.baseid, true);
                            if (instance.baseinfo == null)
                                this.Log($"Error! baseinfo NULL cardid : {cardid.packageId} _ {cardid.id.ToString()} _ {instance.index}");
                            if (!this.UpgradeInfoDic.TryGetValue(instance.baseid, out Dictionary<int, UpgradeBase> _))
                                this.UpgradeInfoDic.Add(instance.baseid, new Dictionary<int, UpgradeBase>()
                                {
                                    {
                                        instance.index,
                                        instance
                                    }
                                });
                            else
                                this.UpgradeInfoDic[instance.baseid][instance.index] = instance;
                            if (!instance.CanRepeatUpgrade())
                                this.UpgradeInfoCache.Remove(type);
                            if (instance.baseid == cardid && instance.index == index)
                            {
                                return instance;
                            }
                        }
                    }
                    catch
                    {
                        this.Log("Error Catched by Create " + type.Name);
                    }
                }
            }
            UpgradeBase upgradeInfo = new UpgradeBase();
            upgradeInfo.baseid = cardid;
            upgradeInfo.baseinfo = ItemXmlDataList.instance.GetCardItem(cardid, true);
            if (upgradeInfo.baseinfo == null)
            {
                this.Log("Error! baseinfo NULL cardid : " + cardid.packageId + " _ " + cardid.id.ToString());
            }
            if (!this.UpgradeInfoDic.TryGetValue(upgradeInfo.baseid, out Dictionary<int, UpgradeBase> _))
                this.UpgradeInfoDic.Add(upgradeInfo.baseid, new Dictionary<int, UpgradeBase>()
              {
                {
                  upgradeInfo.index,
                  upgradeInfo
                }
              });
            else
                this.UpgradeInfoDic[upgradeInfo.baseid][upgradeInfo.index] = upgradeInfo;
            return upgradeInfo;
        }
    }
}
