// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogLikeMod
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using BattleCharacterProfile;
using CommonModApi;
using GameSave;
using HarmonyLib;
using LOR_DiceSystem;
using LOR_XML;
using Mod;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Workshop;
using BattleCardEnhancedView;
using RogueLike_Mod_Reborn;


namespace abcdcode_LOGLIKE_MOD
{

    public class LogLikeMod : ModInitializer
    {
        public static LogLikeMod.CacheDic<int, GameObject> LogUIObjs;
        public static bool purpleexcept;
        public static List<string> CheckExceptionModList;
        public static bool saveloading;
        public static bool see;
        public static bool PauseBool = false;
        public static List<Assembly> LogModAssemblys;
        public static EquipChangeOrder NextPlayerOrder;
        public static int curemotion;
        public static UILogCustomSelectable StageRemainPanel;
        public static TextMeshProUGUI StageRemainText;
        public static Button skipPanel;
        public static TextMeshProUGUI skipPanelText;
        public static List<EquipChangeOrder> PlayerEquipOrders;
        public static Dictionary<string, List<EmotionCardXmlInfo>> PickUpXml_Dummy_Stage;
        public static Dictionary<string, List<EmotionCardXmlInfo>> PickUpXml_Dummy_Passive;
        public static Dictionary<string, List<EmotionEgoXmlInfo>> RewardCardDic_Dummy;
        public static int curChStageStep;
        public static int NormalRewardCool = 0;
        public static List<DropBookXmlInfo> rewards;
        public static List<RewardInfo> rewards_passive;
        public static List<RewardInfo> rewards_InStage;
        public static List<LorId> rewardsMystery;
        public static List<EmotionCardXmlInfo> nextlist;
        public static ChapterGrade curchaptergrade;
        public static StageType curstagetype;
        public static LorId curstageid;
        public static Font DefFont;
        public static TMP_FontAsset _DefFont_TMP;
        public static Color _DefFontColor = new Color(0.9372549f, 0.7607843f, 0.5058824f, 1f);
        public static string ModId = "abcdcodecalmmagma.LogueLikeReborn";
        public static string path;
        public static Dictionary<string, Dictionary<ActionDetail, Dictionary<GameObject, SkeletonAnimation>>> spinemotions;
        public static Dictionary<string, SpineStandingData> spinedatas;
        public static List<ModContentInfo> LogMods;
        public static LogLikeMod.CacheDic<string, UnityEngine.Object> AssetBundles;
        public static LogLikeMod.CacheDic<string, Sprite> ArtWorks;
        public static bool CreatedShopEquipPages = false;
        public static bool Debugging;
        public static bool Temp = true;
        public static Button LogOpenButton;
        public static Button LogContinueButton;
        public static ActionDetail LastDetail;
        public static Button ChangeEmotinCardBtn;
        public static Button CraftBtn;
        public static Image CraftBtnFrame;
        public static Button CreatureBtn;
        public static Image CreatureBtnFrame;
        public static Button InvenBtn;
        public static Image InvenBtnFrame;
        public static bool EndBattle;
        public static bool AddPlayer;
        public static bool RecoverPlayers;
        public static Dictionary<string, System.Type> FindPickUpCache;
        public static LogLikeMod.CacheDic<(string, string), Sprite> ModdedArtWorks;
        public static bool itemCatalogActive;

        public static SaveData CreateChSaveData(ChapterGrade grade)
        {
            SaveData data = new SaveData();
            data.AddData("curstagetype", 7);
            data.AddData("curchaptergrade", (int)grade);
            data.AddData("curChStageStep", 0);
            data.AddData("curstage", new LorId(LogLikeMod.ModId, 855).LogGetSaveData());
            data.AddData("Money", 999);
            return data;
        }

        public static SaveData GetSaveData()
        {
            SaveData saveData = new SaveData();
            saveData.AddData("curstagetype", new SaveData((int)LogLikeMod.curstagetype));
            saveData.AddData("curchaptergrade", new SaveData((int)LogLikeMod.curchaptergrade));
            saveData.AddData("curChStageStep", new SaveData(LogLikeMod.curChStageStep));
            saveData.AddData("curstage", LogLikeMod.curstageid.LogGetSaveData());
            saveData.AddData("Money", new SaveData(PassiveAbility_MoneyCheck.GetMoney()));
            return saveData;
        }

        public static void LoadFromSaveData(SaveData data)
        {
            LogLikeMod.curstagetype = (StageType)data.GetData("curstagetype").GetIntSelf();
            LogLikeMod.curchaptergrade = data.GetData("curchaptergrade").GetIntSelf() == 6 ? ChapterGrade.Grade7 : (ChapterGrade)data.GetData("curchaptergrade").GetIntSelf();
            LogLikeMod.curChStageStep = data.GetInt("curChStageStep");
            LogLikeMod.curstageid = ExtensionUtils.LogLoadFromSaveData(data.GetData("curstage"));
            LogLikeMod.SetNextStage(LogLikeMod.curstageid, LogLikeMod.curstagetype, NextStageSetType.BySave);
            PassiveAbility_MoneyCheck.SetMoney(data.GetInt("Money"));
        }

        public static void SetNextStage(LorId stageid, StageType stagetype = StageType.Custom, NextStageSetType settype = NextStageSetType.Default)
        {
            StageModel stageModel = Singleton<StageController>.Instance.GetStageModel();
            if (settype == NextStageSetType.BySave)
                stageModel.Init(Singleton<StageClassInfoList>.Instance.GetData(RMRCore.CurrentGamemode.StageStart), LibraryModel.Instance);
            StageClassInfo data = Singleton<StageClassInfoList>.Instance.GetData(stageid);
            StageWaveInfo wave = data.waveList[0];
            StageWaveModel stageWaveModel = new StageWaveModel();
            stageWaveModel.Init(stageModel, wave);
            List<StageWaveModel> stageWaveModelList = (List<StageWaveModel>)typeof(StageModel).GetField("_waveList", AccessTools.all).GetValue((object)stageModel);
            if (settype == NextStageSetType.BySave || settype == NextStageSetType.CustomGamemode)
                stageWaveModelList[0] = stageWaveModel;
            else
                stageWaveModelList.Add(stageWaveModel);
            stageModel.ClassInfo.mapInfo = data.mapInfo;
            stageModel.SetCurrentMapInfo(0);
            if (settype == NextStageSetType.Default || settype == NextStageSetType.Custom)
            {
                LogLikeMod.nextlist.Clear();
                if (settype == NextStageSetType.Default)
                {
                    if (LogLikeMod.curstagetype == StageType.Boss)
                        LogueBookModels.RemoveStageInlist(stageid, LogLikeMod.curchaptergrade + 1);
                    else
                        LogueBookModels.RemoveStageInlist(stageid, LogLikeMod.curchaptergrade);
                }
                if (LogLikeMod.curstagetype == StageType.Boss)
                {
                    LogLikeMod.AddPlayer = true;
                    LogLikeMod.RecoverPlayers = true;
                    LogLikeMod.curChStageStep = 0;
                    ++LogLikeMod.curchaptergrade;
                    switch (LogLikeMod.curchaptergrade)
                    {
                        case ChapterGrade.Grade2:
                            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter2());
                            break;
                        case ChapterGrade.Grade3:
                            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter3());
                            break;
                        case ChapterGrade.Grade4:
                            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter4());
                            break;
                        case ChapterGrade.Grade5:
                            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter5());
                            break;
                        case ChapterGrade.Grade6:
                            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter6());
                            break;
                        case ChapterGrade.Grade7:
                            Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter7());
                            break;
                    }
                }
                else
                    ++LogLikeMod.curChStageStep;
            }
            LogLikeMod.curstagetype = stagetype;
            LogLikeMod.curstageid = data.id;
        }

        public static GameObject GetLogUIObj(int index)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<Transform>(SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.transform, SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.transform.parent).gameObject;
            UnityEngine.Object.Destroy((UnityEngine.Object)gameObject.GetComponent<LevelUpUI>());
            for (int index1 = 0; index1 < gameObject.transform.childCount; ++index1)
                UnityEngine.Object.Destroy((UnityEngine.Object)gameObject.transform.GetChild(index1).gameObject);
            gameObject.SetActive(true);
            gameObject.AddComponent<LogLikeMod.UIActiveChecker>();
            gameObject.transform.localPosition = SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.transform.localPosition;
            gameObject.transform.localScale = SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.transform.localScale;
            (gameObject.transform as RectTransform).sizeDelta = new Vector2(0.0f, 0.0f);
            gameObject.GetComponent<Canvas>().enabled = true;
            gameObject.GetComponent<Canvas>().sortingOrder += index;
            return gameObject;
        }

        public static void ResetUIs()
        {
            foreach (GameObject gameObject in LogLikeMod.LogUIObjs.dic.Values)
                gameObject.SetActive(false);
            LogLikeMod.BattleMoneyUI.DeActive();
            Singleton<MysteryManager>.Instance.EndMystery();
            Singleton<ShopManager>.Instance.RemoveShop();
            Singleton<GlobalLogueEffectManager>.Instance.ClearList();
        }

        public static T GetFieldValue<T>(object obj, string name)
        {
            return (T)obj.GetType().GetField(name, AccessTools.all | BindingFlags.FlattenHierarchy).GetValue(obj);
        }

        public static void SetFieldValue(object obj, string name, object value)
        {
            obj.GetType().GetField(name, AccessTools.all | BindingFlags.FlattenHierarchy).SetValue(obj, value);
        }

        public static TMP_FontAsset DefFont_TMP
        {
            get
            {
                if ((UnityEngine.Object)LogLikeMod._DefFont_TMP == (UnityEngine.Object)null)
                    LogLikeMod._DefFont_TMP = LogLikeMod.GetFieldValue<TMP_FontAsset>((object)SingletonBehavior<LocalizedFontSetter>.Instance, "font_NotoSans");
                return LogLikeMod._DefFont_TMP;
            }
            set => LogLikeMod._DefFont_TMP = value;
        }

        public static Color DefFontColor
        {
            get => LogLikeMod._DefFontColor;
            set => LogLikeMod._DefFontColor = value;
        }

        public static bool CheckExceptionModExist(out List<string> ExceptModNames)
        {
            ExceptModNames = new List<string>();
            foreach (ModContentInfo allMod in Singleton<ModContentManager>.Instance.GetAllMods())
            {
                if (allMod.activated && LogLikeMod.CheckExceptionModList.Contains(allMod.invInfo.workshopInfo.uniqueId))
                    ExceptModNames.Add(allMod.invInfo.workshopInfo.title);
            }
            return ExceptModNames.Count > 0;
        }

        public static List<ModContentInfo> GetLogMods()
        {
            if (LogLikeMod.LogMods != null)
                return LogLikeMod.LogMods;
            LogLikeMod.LogMods = new List<ModContentInfo>();
            foreach (ModContentInfo allMod in Singleton<ModContentManager>.Instance.GetAllMods())
            {
                if (allMod.activated && Directory.Exists(allMod.GetLogDllPath()))
                    LogLikeMod.LogMods.Add(allMod);
            }
            return LogLikeMod.LogMods;
        }

        public static Sprite GetArtWorks(DirectoryInfo dir, string name)
        {
            if (dir.GetDirectories().Length != 0)
            {
                foreach (DirectoryInfo directory in dir.GetDirectories())
                {
                    Sprite artWorks = LogLikeMod.GetArtWorks(directory, name);
                    if ((UnityEngine.Object)artWorks != (UnityEngine.Object)null)
                        return artWorks;
                }
            }
            foreach (System.IO.FileInfo file in dir.GetFiles())
            {
                if (Path.GetFileNameWithoutExtension(file.FullName) == name)
                {
                    Texture2D texture2D = new Texture2D(2, 2);
                    byte[] data = File.ReadAllBytes(file.FullName);
                    texture2D.LoadImage(data);
                    return Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.0f, 0.0f), 100f, 0U, SpriteMeshType.FullRect);
                }
            }
            return (Sprite)null;
        }

        public static Sprite GetArtWorks(string name)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(LogLikeMod.path + "/ArtWork");
            if (directoryInfo.GetDirectories().Length != 0)
            {
                foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
                {
                    Sprite artWorks = LogLikeMod.GetArtWorks(directory, name);
                    if ((UnityEngine.Object)artWorks != (UnityEngine.Object)null)
                        return artWorks;
                }
            }
            foreach (System.IO.FileInfo file in directoryInfo.GetFiles())
            {
                if (Path.GetFileNameWithoutExtension(file.FullName) == name)
                {
                    Texture2D texture2D = new Texture2D(2, 2);
                    texture2D.LoadImage(File.ReadAllBytes(file.FullName));
                    return Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.0f, 0.0f), 100f, 0U, SpriteMeshType.FullRect);
                }
            }
            return (Sprite)null;
        }

        public static SkeletonDataAsset GetAsset(
          string atlaspath,
          string jsonpath,
          Material[] materials,
          float scale = 0.01f)
        {
            TextAsset atlasText = new TextAsset(File.ReadAllText(atlaspath));
            return SkeletonDataAsset.CreateRuntimeInstance(new TextAsset(File.ReadAllText(jsonpath)), new AtlasAsset[1]
            {
      AtlasAsset.CreateRuntimeInstance(atlasText, materials, true)
            }, true, scale);
        }

        public static Material CreateMaterialForSkel(string imagepath, string name)
        {
            Shader shader = Shader.Find("UI/Default");
            Texture2D tex = new Texture2D(2, 2);
            byte[] data = File.ReadAllBytes(imagepath);
            tex.LoadImage(data);
            tex.name = name;
            return new Material(shader)
            {
                mainTexture = (Texture)tex
            };
        }

        public static string GetPickUpXmlWorkShopId_Stage(EmotionCardXmlInfo info)
        {
            return LogLikeMod.PickUpXml_Dummy_Stage == null ? (string)null : LogLikeMod.PickUpXml_Dummy_Stage.ToList<KeyValuePair<string, List<EmotionCardXmlInfo>>>().Find((Predicate<KeyValuePair<string, List<EmotionCardXmlInfo>>>)(x => x.Value.Find((Predicate<EmotionCardXmlInfo>)(y => y == info)) != null)).Key;
        }

        public static string GetPickUpXmlWorkShopId_Passive(EmotionCardXmlInfo info)
        {
            return LogLikeMod.PickUpXml_Dummy_Passive == null ? (string)null : LogLikeMod.PickUpXml_Dummy_Passive.ToList<KeyValuePair<string, List<EmotionCardXmlInfo>>>().Find((Predicate<KeyValuePair<string, List<EmotionCardXmlInfo>>>)(x => x.Value.Find((Predicate<EmotionCardXmlInfo>)(y => y == info)) != null)).Key;
        }

        public static EmotionCardXmlInfo GetRegisteredPickUpXml(LogueStageInfo info)
        {
            return LogLikeMod.PickUpXml_Dummy_Stage == null ? (EmotionCardXmlInfo)null : LogLikeMod.PickUpXml_Dummy_Stage[info.workshopid].Find((Predicate<EmotionCardXmlInfo>)(x => x.id == info.stageid));
        }

        public static EmotionCardXmlInfo GetRegisteredPickUpXml(RewardPassiveInfo info)
        {
            return LogLikeMod.PickUpXml_Dummy_Passive == null ? (EmotionCardXmlInfo)null : LogLikeMod.PickUpXml_Dummy_Passive[info.workshopID].Find((Predicate<EmotionCardXmlInfo>)(x => x.id == info.passiveid));
        }

        public static void RegisterPickUpXml(LogueStageInfo info)
        {
            if (LogLikeMod.PickUpXml_Dummy_Stage == null)
                LogLikeMod.PickUpXml_Dummy_Stage = new Dictionary<string, List<EmotionCardXmlInfo>>();
            string workshopid = info.workshopid;
            if (!LogLikeMod.PickUpXml_Dummy_Stage.ContainsKey(workshopid))
                LogLikeMod.PickUpXml_Dummy_Stage.Add(workshopid, new List<EmotionCardXmlInfo>());
            LogLikeMod.PickUpXml_Dummy_Stage[workshopid].Add(new EmotionCardXmlInfo()
            {
                Sephirah = SephirahType.None,
                id = info.stageid,
                Level = 1,
                TargetType = EmotionTargetType.All,
                Name = $"{info.workshopid}_{info.stageid.ToString()}",
                Locked = false,
                State = MentalState.Positive,
                Script = new List<string>(),
                EmotionLevel = 2,
                EmotionRate = 0
            });
        }

        public static void RegisterPickUpXml(RewardPassiveInfo info)
        {
            if (LogLikeMod.PickUpXml_Dummy_Passive == null)
                LogLikeMod.PickUpXml_Dummy_Passive = new Dictionary<string, List<EmotionCardXmlInfo>>();
            string packageId = info.id.packageId;
            if (!LogLikeMod.PickUpXml_Dummy_Passive.ContainsKey(packageId))
                LogLikeMod.PickUpXml_Dummy_Passive.Add(packageId, new List<EmotionCardXmlInfo>());
            EmotionCardXmlInfo emotionCardXmlInfo = new EmotionCardXmlInfo();
            emotionCardXmlInfo._artwork = info.artwork;
            emotionCardXmlInfo.Sephirah = SephirahType.None;
            emotionCardXmlInfo.id = info.id.id;
            emotionCardXmlInfo.Level = info.level;
            emotionCardXmlInfo.TargetType = info.targettype;
            emotionCardXmlInfo.Name = info.rewardtype != RewardType.EquipPage ? info.script : $"{info.workshopID}_{info.passiveid.ToString()}";
            emotionCardXmlInfo.Locked = false;
            emotionCardXmlInfo.State = MentalState.Positive;
            if (Singleton<RewardPassivesList>.Instance.infos.Find((Predicate<RewardPassivesInfo>)(x => x.RewardPassiveList.Contains(info))).rewardtype == PassiveRewardListType.Creature)
                emotionCardXmlInfo.State = MentalState.Negative;
            emotionCardXmlInfo.Script = new List<string>()
    {
      info.script
    };
            if (info.rewardtype == RewardType.EquipPage)
                emotionCardXmlInfo.Script[0] = "EquipDefault";
            emotionCardXmlInfo.EmotionLevel = info.level;
            emotionCardXmlInfo.EmotionRate = 0;
            LogLikeMod.PickUpXml_Dummy_Passive[packageId].Add(emotionCardXmlInfo);
        }

        public static void LoadSpineAssets()
        {
            LogLikeMod.spinedatas = new Dictionary<string, SpineStandingData>();
            Material[] materials1 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/MerchantLog/Merchant.png", "Merchant")
            };
            SpineStandingData spineStandingData1 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/MerchantLog/atlas.txt", LogLikeMod.path + "/Spine/MerchantLog/json.txt", materials1, 0.03f));
            spineStandingData1.SetDic(ActionDetail.Default, "idle");
            spineStandingData1.SetDic(ActionDetail.Move, "idle");
            spineStandingData1.SetDic(ActionDetail.Standing, "idle");
            LogLikeMod.spinedatas.Add("MerchantLog", spineStandingData1);
            Material[] materials2 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/MachineDawnLog/Machine_Dawn.png", "Machine_Dawn")
            };
            SpineStandingData spineStandingData2 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/MachineDawnLog/atlas.txt", LogLikeMod.path + "/Spine/MachineDawnLog/json.txt", materials2));
            spineStandingData2.SetDic(ActionDetail.Default, "Standing");
            spineStandingData2.SetDic(ActionDetail.Move, "Walk");
            spineStandingData2.SetDic(ActionDetail.Standing, "Standing");
            spineStandingData2.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData2.SetDic(ActionDetail.Guard, "Standing");
            spineStandingData2.SetDic(ActionDetail.Slash, "Attack_02", 3.5f, false);
            spineStandingData2.SetDic(ActionDetail.Penetrate, "Attack_01", 3.5f, false);
            spineStandingData2.SetDic(ActionDetail.Hit, "Attack_02", 3.5f, false);
            spineStandingData2.SetDic(ActionDetail.Fire, "Standing");
            spineStandingData2.SetDic(ActionDetail.Aim, "Standing");
            spineStandingData2.SetDic(ActionDetail.NONE, "Standing");
            spineStandingData2.SetDic(ActionDetail.S1, "DeadScene", IsLoop: false);
            spineStandingData2.SetScale(new Vector3(-1f, 1f));
            spineStandingData2.SetScale(ActionDetail.Standing, new Vector3(1f, 1f));
            LogLikeMod.spinedatas.Add("MachineDawnLog", spineStandingData2);
            Material[] materials3 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/MachineNoonLog/machine_Noon.png", "machine_Noon")
            };
            SpineStandingData spineStandingData3 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/MachineNoonLog/atlas.txt", LogLikeMod.path + "/Spine/MachineNoonLog/json.txt", materials3, 0.015f));
            spineStandingData3.SetDic(ActionDetail.Default, "Default");
            spineStandingData3.SetDic(ActionDetail.Move, "Default");
            spineStandingData3.SetDic(ActionDetail.Standing, "Default");
            spineStandingData3.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData3.SetDic(ActionDetail.Guard, "Default");
            spineStandingData3.SetDic(ActionDetail.Slash, "Attack_01", 2f, false);
            spineStandingData3.SetDic(ActionDetail.Penetrate, "Attack_01", 2f, false);
            spineStandingData3.SetDic(ActionDetail.Hit, "Attack_01", 2f, false);
            spineStandingData3.SetDic(ActionDetail.Fire, "Walk");
            spineStandingData3.SetDic(ActionDetail.Aim, "Walk");
            spineStandingData3.SetDic(ActionDetail.NONE, "Default");
            spineStandingData3.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("MachineNoonLog", spineStandingData3);
            Material[] materials4 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/MachineDuskLog/machineDusk.png", "machineDusk")
            };
            SpineStandingData spineStandingData4 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/MachineDuskLog/atlas.txt", LogLikeMod.path + "/Spine/MachineDuskLog/json.txt", materials4, 0.015f));
            spineStandingData4.SetDic(ActionDetail.Default, "Default");
            spineStandingData4.SetDic(ActionDetail.Move, "Default");
            spineStandingData4.SetDic(ActionDetail.Standing, "Default");
            spineStandingData4.SetDic(ActionDetail.Damaged, "Dead");
            spineStandingData4.SetDic(ActionDetail.Guard, "Default");
            spineStandingData4.SetDic(ActionDetail.Slash, "Default");
            spineStandingData4.SetDic(ActionDetail.Penetrate, "Default");
            spineStandingData4.SetDic(ActionDetail.Hit, "Default");
            spineStandingData4.SetDic(ActionDetail.Fire, "Default");
            spineStandingData4.SetDic(ActionDetail.Aim, "Default");
            spineStandingData4.SetDic(ActionDetail.NONE, "Default");
            spineStandingData4.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("MachineDuskLog", spineStandingData4);
            Material[] materials5 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/MachineMidnightLog/machine_Midnight.png", "machine_Midnight")
            };
            SpineStandingData spineStandingData5 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/MachineMidnightLog/atlas.txt", LogLikeMod.path + "/Spine/MachineMidnightLog/json.txt", materials5, 0.015f));
            spineStandingData5.SetDic(ActionDetail.Default, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Move, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Standing, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Damaged, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Guard, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Slash, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Penetrate, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Hit, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Fire, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.Aim, "Exterminate");
            spineStandingData5.SetDic(ActionDetail.NONE, "Exterminate");
            spineStandingData5.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("MachineMidnightLog", spineStandingData5);
            Material[] materials6 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/OutterGodDawnLog/cosmic_dawn.png", "cosmic_dawn")
            };
            SpineStandingData spineStandingData6 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/OutterGodDawnLog/atlas.txt", LogLikeMod.path + "/Spine/OutterGodDawnLog/json.txt", materials6, 0.015f));
            spineStandingData6.SetDic(ActionDetail.Default, "Walk");
            spineStandingData6.SetDic(ActionDetail.Move, "Walk");
            spineStandingData6.SetDic(ActionDetail.Standing, "Walk");
            spineStandingData6.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData6.SetDic(ActionDetail.Guard, "Walk");
            spineStandingData6.SetDic(ActionDetail.Slash, "Walk");
            spineStandingData6.SetDic(ActionDetail.Penetrate, "Walk");
            spineStandingData6.SetDic(ActionDetail.Hit, "Walk");
            spineStandingData6.SetDic(ActionDetail.Fire, "Walk");
            spineStandingData6.SetDic(ActionDetail.Aim, "Walk");
            spineStandingData6.SetDic(ActionDetail.NONE, "Walk");
            spineStandingData6.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("OutterGodDawnLog", spineStandingData6);
            Material[] materials7 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/OutterGodNoonLog/stone.png", "stone")
            };
            SpineStandingData spineStandingData7 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/OutterGodNoonLog/atlas.txt", LogLikeMod.path + "/Spine/OutterGodNoonLog/json.txt", materials7, 0.015f));
            spineStandingData7.SetDic(ActionDetail.Default, "Default");
            spineStandingData7.SetDic(ActionDetail.Move, "Default");
            spineStandingData7.SetDic(ActionDetail.Standing, "Default");
            spineStandingData7.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData7.SetDic(ActionDetail.Guard, "Default");
            spineStandingData7.SetDic(ActionDetail.Slash, "Casting");
            spineStandingData7.SetDic(ActionDetail.Penetrate, "Casting");
            spineStandingData7.SetDic(ActionDetail.Hit, "Casting");
            spineStandingData7.SetDic(ActionDetail.Fire, "Default");
            spineStandingData7.SetDic(ActionDetail.Aim, "Default");
            spineStandingData7.SetDic(ActionDetail.NONE, "Default");
            spineStandingData7.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("OutterGodNoonLog", spineStandingData7);
            Material[] materials8 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/BugDawnLog/Bug1.png", "Bug1")
            };
            SpineStandingData spineStandingData8 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/BugDawnLog/atlas.txt", LogLikeMod.path + "/Spine/BugDawnLog/json.txt", materials8, 0.015f));
            spineStandingData8.SetDic(ActionDetail.Default, "Move");
            spineStandingData8.SetDic(ActionDetail.Move, "Move");
            spineStandingData8.SetDic(ActionDetail.Standing, "Move");
            spineStandingData8.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData8.SetDic(ActionDetail.Guard, "Move");
            spineStandingData8.SetDic(ActionDetail.Slash, "Attack", IsLoop: false);
            spineStandingData8.SetDic(ActionDetail.Penetrate, "Attack", IsLoop: false);
            spineStandingData8.SetDic(ActionDetail.Hit, "Attack", IsLoop: false);
            spineStandingData8.SetDic(ActionDetail.Fire, "Move");
            spineStandingData8.SetDic(ActionDetail.Aim, "Move");
            spineStandingData8.SetDic(ActionDetail.NONE, "Move");
            spineStandingData8.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("BugDawnLog", spineStandingData8);
            Material[] materials9 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/BugDuskLog/BugDusk.png", "BugDusk")
            };
            SpineStandingData spineStandingData9 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/BugDuskLog/atlas.txt", LogLikeMod.path + "/Spine/BugDuskLog/json.txt", materials9, 0.015f));
            spineStandingData9.SetDic(ActionDetail.Default, "Move");
            spineStandingData9.SetDic(ActionDetail.Move, "Move");
            spineStandingData9.SetDic(ActionDetail.Standing, "Move");
            spineStandingData9.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData9.SetDic(ActionDetail.Guard, "Move");
            spineStandingData9.SetDic(ActionDetail.Slash, "Eat", IsLoop: false);
            spineStandingData9.SetDic(ActionDetail.Penetrate, "Eat", IsLoop: false);
            spineStandingData9.SetDic(ActionDetail.Hit, "Eat", IsLoop: false);
            spineStandingData9.SetDic(ActionDetail.Fire, "Move");
            spineStandingData9.SetDic(ActionDetail.Aim, "Move");
            spineStandingData9.SetDic(ActionDetail.NONE, "Move");
            spineStandingData9.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("BugDuskLog", spineStandingData9);
            Material[] materials10 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/BugMidnightLog/ThirdType.png", "ThirdType")
            };
            SpineStandingData spineStandingData10 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/BugMidnightLog/atlas.txt", LogLikeMod.path + "/Spine/BugMidnightLog/json.txt", materials10, 0.015f));
            spineStandingData10.SetDic(ActionDetail.Default, "Default");
            spineStandingData10.SetDic(ActionDetail.Move, "Default");
            spineStandingData10.SetDic(ActionDetail.Standing, "Default");
            spineStandingData10.SetDic(ActionDetail.Damaged, "Default", IsLoop: false);
            spineStandingData10.SetDic(ActionDetail.Guard, "Default");
            spineStandingData10.SetDic(ActionDetail.Slash, "Default", IsLoop: false);
            spineStandingData10.SetDic(ActionDetail.Penetrate, "Default", IsLoop: false);
            spineStandingData10.SetDic(ActionDetail.Hit, "Default", IsLoop: false);
            spineStandingData10.SetDic(ActionDetail.Fire, "Default");
            spineStandingData10.SetDic(ActionDetail.Aim, "Default");
            spineStandingData10.SetDic(ActionDetail.NONE, "Default");
            spineStandingData10.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("BugMidnightLog", spineStandingData10);
            Material[] materials11 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/CircusDawnLog/tjzjtm.png", "tjzjtm")
            };
            SpineStandingData spineStandingData11 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/CircusDawnLog/atlas.txt", LogLikeMod.path + "/Spine/CircusDawnLog/json.txt", materials11));
            spineStandingData11.SetDic(ActionDetail.Default, "Default");
            spineStandingData11.SetDic(ActionDetail.Move, "Default");
            spineStandingData11.SetDic(ActionDetail.Standing, "Default");
            spineStandingData11.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData11.SetDic(ActionDetail.Guard, "Default");
            spineStandingData11.SetDic(ActionDetail.Slash, "Trick", IsLoop: false);
            spineStandingData11.SetDic(ActionDetail.Penetrate, "Trick", IsLoop: false);
            spineStandingData11.SetDic(ActionDetail.Hit, "Trick", IsLoop: false);
            spineStandingData11.SetDic(ActionDetail.Fire, "Default");
            spineStandingData11.SetDic(ActionDetail.Aim, "Default");
            spineStandingData11.SetDic(ActionDetail.NONE, "Default");
            spineStandingData11.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("CircusDawnLog", spineStandingData11);
            Material[] materials12 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/CircusNoonLog/CircusNoon.png", "CircusNoon")
            };
            SpineStandingData spineStandingData12 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/CircusNoonLog/atlas.txt", LogLikeMod.path + "/Spine/CircusNoonLog/json.txt", materials12));
            spineStandingData12.SetDic(ActionDetail.Default, "default");
            spineStandingData12.SetDic(ActionDetail.Move, "walk");
            spineStandingData12.SetDic(ActionDetail.Standing, "default");
            spineStandingData12.SetDic(ActionDetail.Damaged, "dead", IsLoop: false);
            spineStandingData12.SetDic(ActionDetail.Guard, "default");
            spineStandingData12.SetDic(ActionDetail.Slash, "attack1", IsLoop: false);
            spineStandingData12.SetDic(ActionDetail.Penetrate, "attack2", IsLoop: false);
            spineStandingData12.SetDic(ActionDetail.Hit, "attack3", IsLoop: false);
            spineStandingData12.SetDic(ActionDetail.Fire, "default");
            spineStandingData12.SetDic(ActionDetail.Aim, "default");
            spineStandingData12.SetDic(ActionDetail.NONE, "default");
            spineStandingData12.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("CircusNoonLog", spineStandingData12);
            Material[] materials13 = new Material[1]
            {
      LogLikeMod.CreateMaterialForSkel(LogLikeMod.path + "/Spine/CircusDuskLog/circus_night.png", "circus_night")
            };
            SpineStandingData spineStandingData13 = new SpineStandingData(LogLikeMod.GetAsset(LogLikeMod.path + "/Spine/CircusDuskLog/atlas.txt", LogLikeMod.path + "/Spine/CircusDuskLog/json.txt", materials13, 0.015f));
            spineStandingData13.SetDic(ActionDetail.Default, "Default");
            spineStandingData13.SetDic(ActionDetail.Move, "Attack_03", 2f);
            spineStandingData13.SetDic(ActionDetail.Standing, "Default");
            spineStandingData13.SetDic(ActionDetail.Damaged, "Dead", IsLoop: false);
            spineStandingData13.SetDic(ActionDetail.Guard, "Default");
            spineStandingData13.SetDic(ActionDetail.Slash, "Attack_01", 3f, false);
            spineStandingData13.SetDic(ActionDetail.Penetrate, "Attack_02", 3f, false);
            spineStandingData13.SetDic(ActionDetail.Hit, "Attack_02", 3f, false);
            spineStandingData13.SetDic(ActionDetail.Fire, "Default");
            spineStandingData13.SetDic(ActionDetail.Aim, "Default");
            spineStandingData13.SetDic(ActionDetail.NONE, "Default");
            spineStandingData13.SetScale(ActionDetail.Standing, new Vector3(-1f, 1f));
            LogLikeMod.spinedatas.Add("CircusDuskLog", spineStandingData13);
        }

        public static void LoadDiceAbilityDesc(string path, string modid)
        {
            Dictionary<string, BattleCardAbilityDesc> fieldValue = LogLikeMod.GetFieldValue<Dictionary<string, BattleCardAbilityDesc>>((object)Singleton<BattleCardAbilityDescXmlList>.Instance, "_dictionary");
            using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
            {
                BattleCardAbilityDescRoot cardAbilityDescRoot = (BattleCardAbilityDescRoot)new XmlSerializer(typeof(BattleCardAbilityDescRoot)).Deserialize((TextReader)stringReader);
                for (int index = 0; index < cardAbilityDescRoot.cardDescList.Count; ++index)
                {
                    BattleCardAbilityDesc cardDesc = cardAbilityDescRoot.cardDescList[index];
                    fieldValue[cardDesc.id] = cardDesc;
                }
            }
        }

        public static void LoadDropBookName(string path, string modid)
        {
            string xml = File.ReadAllText(path);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (XmlNode selectNode in xmlDocument.SelectNodes("localize/text"))
            {
                string str = string.Empty;
                if (selectNode.Attributes.GetNamedItem("id") != null)
                    str = selectNode.Attributes.GetNamedItem("id").InnerText;
                string key = str;
                string innerText = selectNode.InnerText;
                try
                {
                    dictionary.Add(key, innerText);
                }
                catch (Exception ex)
                {
                    Debug.LogError((object)ex);
                }
            }
            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
                KeyValuePair<string, string> keyvalue = keyValuePair;
                List<DropBookXmlInfo> all = Singleton<DropBookXmlList>.Instance.GetList().FindAll((Predicate<DropBookXmlInfo>)(x => x._targetText == keyvalue.Key));
                if (all.Count > 0)
                {
                    foreach (DropBookXmlInfo dropBookXmlInfo in all)
                        dropBookXmlInfo.workshopName = keyvalue.Value;
                }
            }
        }

        public static void LoadEnemyUnitName(string path, string modid)
        {
            using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
            {
                CharactersNameRoot charactersNameRoot = (CharactersNameRoot)new XmlSerializer(typeof(CharactersNameRoot)).Deserialize((TextReader)stringReader);
                List<EnemyUnitClassInfo> all1 = LogLikeMod.GetFieldValue<List<EnemyUnitClassInfo>>((object)Singleton<EnemyUnitClassInfoList>.Instance, "_list").FindAll((Predicate<EnemyUnitClassInfo>)(x => x.workshopID == modid));
                foreach (CharacterName name in charactersNameRoot.nameList)
                {
                    CharacterName desc = name;
                    List<EnemyUnitClassInfo> all2 = all1.FindAll((Predicate<EnemyUnitClassInfo>)(x => x.nameId == desc.ID));
                    if (all2.Count > 0)
                    {
                        foreach (EnemyUnitClassInfo enemyUnitClassInfo in all2)
                            enemyUnitClassInfo.name = desc.name;
                    }
                }
            }
        }

        public static void LoadBookDesc(string path, string modid)
        {
            using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
            {
                BookDescRoot bookDescRoot = (BookDescRoot)new XmlSerializer(typeof(BookDescRoot)).Deserialize((TextReader)stringReader);
                List<BookXmlInfo> list = Singleton<BookXmlList>.Instance.GetList();
                foreach (BookDesc bookDesc in bookDescRoot.bookDescList)
                {
                    BookDesc desc = bookDesc;
                    List<BookXmlInfo> all = list.FindAll((Predicate<BookXmlInfo>)(x => x.TextId == desc.bookID));
                    if (all.Count > 0)
                    {
                        foreach (BookXmlInfo bookXmlInfo in all)
                            bookXmlInfo.InnerName = desc.bookName;
                    }
                }
                Singleton<BookDescXmlList>.Instance.AddBookTextByMod(modid, bookDescRoot.bookDescList);
            }
        }

        public static void LoadCardDesc(string path, string modid)
        {
            using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
            {
                Dictionary<LorId, BattleCardDesc> dictionary = (Dictionary<LorId, BattleCardDesc>)typeof(BattleCardDescXmlList).GetField("_dictionary", AccessTools.all).GetValue((object)Singleton<BattleCardDescXmlList>.Instance);
                foreach (BattleCardDesc cardDesc in ((BattleCardDescRoot)new XmlSerializer(typeof(BattleCardDescRoot)).Deserialize((TextReader)stringReader)).cardDescList)
                {
                    LorId lorId = new LorId(modid, cardDesc.cardID);
                    dictionary[lorId] = cardDesc;
                    DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(lorId, true);
                    if (cardItem != null)
                        cardItem.workshopName = cardDesc.cardName;
                }
            }
        }

        public static void LoadPassiveDesc(string path, string modid)
        {
            using (StringReader stringReader = new StringReader(File.ReadAllText(path)))
            {
                Dictionary<LorId, PassiveDesc> dictionary = (Dictionary<LorId, PassiveDesc>)typeof(PassiveDescXmlList).GetField("_dictionary", AccessTools.all).GetValue((object)Singleton<PassiveDescXmlList>.Instance);
                foreach (PassiveDesc desc1 in ((PassiveDescRoot)new XmlSerializer(typeof(PassiveDescRoot)).Deserialize((TextReader)stringReader)).descList)
                {
                    PassiveDesc desc = desc1;
                    desc.workshopID = modid;
                    dictionary[desc.ID] = desc;
                    BookXmlInfo bookXmlInfo = Singleton<BookXmlList>.Instance.GetAllWorkshopData()[modid].Find((Predicate<BookXmlInfo>)(x => x.id == desc.ID));
                    if (bookXmlInfo != null)
                        bookXmlInfo.InnerName = desc.name;
                }
            }
        }

        public static void LoadLocalizeFile(string path, ref Dictionary<string, string> dic)
        {
            string xml = File.ReadAllText(path);
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(xml);
            }
            catch
            {
                Debug.LogError(("path : " + path));
            }
            foreach (XmlNode selectNode in xmlDocument.SelectNodes("localize/text"))
            {
                string str = string.Empty;
                if (selectNode.Attributes.GetNamedItem("id") != null)
                    str = selectNode.Attributes.GetNamedItem("id").InnerText;
                string key = str;
                string innerText = selectNode.InnerText;
                try
                {
                    dic[key] = innerText;
                }
                catch (Exception ex)
                {
                    Debug.LogError((object)ex);
                }
            }
        }

        public static void LoadTextData(string language)
        {
            string str = "/Localize/" + language;
            abcdcode_LOGLIKE_MOD_Extension.TextDataModel._currentLanguage = language;
            Dictionary<string, string> textDic = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.textDic;
            DirectoryInfo directoryInfo1 = !Directory.Exists(LogLikeMod.path + str) ? new DirectoryInfo(LogLikeMod.path + "/Localize/en") : new DirectoryInfo(LogLikeMod.path + str);
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
                LogLikeMod.LoadLocalizeFile(file.FullName, ref textDic);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + str);
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo2.GetFiles())
                        LogLikeMod.LoadLocalizeFile(file.FullName, ref textDic);
                }
            }
            foreach (FileSystemInfo file in new DirectoryInfo(directoryInfo1.FullName + "/PassiveInfo").GetFiles())
                LogLikeMod.LoadPassiveDesc(file.FullName, LogLikeMod.ModId);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo3 = new DirectoryInfo($"{logMod.GetLogDllPath()}{str}/PassiveInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo3.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo3.GetFiles())
                        LogLikeMod.LoadPassiveDesc(file.FullName, uniqueId);
                }
            }
            foreach (FileSystemInfo file in new DirectoryInfo(directoryInfo1.FullName + "/CardInfo").GetFiles())
                LogLikeMod.LoadCardDesc(file.FullName, LogLikeMod.ModId);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo4 = new DirectoryInfo($"{logMod.GetLogDllPath()}{str}/CardInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo4.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo4.GetFiles())
                        LogLikeMod.LoadCardDesc(file.FullName, uniqueId);
                }
            }
            foreach (FileSystemInfo file in new DirectoryInfo(directoryInfo1.FullName + "/BookInfo").GetFiles())
                LogLikeMod.LoadBookDesc(file.FullName, LogLikeMod.ModId);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo5 = new DirectoryInfo($"{logMod.GetLogDllPath()}{str}/BookInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo5.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo5.GetFiles())
                        LogLikeMod.LoadBookDesc(file.FullName, uniqueId);
                }
            }
            foreach (FileSystemInfo file in new DirectoryInfo(directoryInfo1.FullName + "/EnemyNameInfo").GetFiles())
                LogLikeMod.LoadEnemyUnitName(file.FullName, LogLikeMod.ModId);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo6 = new DirectoryInfo($"{logMod.GetLogDllPath()}{str}/EnemyNameInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo6.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo6.GetFiles())
                        LogLikeMod.LoadEnemyUnitName(file.FullName, uniqueId);
                }
            }
            foreach (FileSystemInfo file in new DirectoryInfo(directoryInfo1.FullName + "/DropBookInfo").GetFiles())
                LogLikeMod.LoadDropBookName(file.FullName, LogLikeMod.ModId);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo7 = new DirectoryInfo($"{logMod.GetLogDllPath()}{str}/DropBookInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo7.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo7.GetFiles())
                        LogLikeMod.LoadDropBookName(file.FullName, uniqueId);
                }
            }
            foreach (FileSystemInfo file in new DirectoryInfo(directoryInfo1.FullName + "/DiceAbilityInfo").GetFiles())
                LogLikeMod.LoadDiceAbilityDesc(file.FullName, LogLikeMod.ModId);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo8 = new DirectoryInfo($"{logMod.GetLogDllPath()}{str}/DiceAbilityInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo8.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo8.GetFiles())
                        LogLikeMod.LoadDiceAbilityDesc(file.FullName, uniqueId);
                }
            }
            try
            {
                LogueEffectXmlList.Instance.Init(language);
                RMRCore.LoadSatelliteBattleTexts(language);
                RMRCore.LoadSatelliteBattleDialog(language);
                RogueMysteryXmlList.Instance.Init(language);
            }
            catch (Exception e)
            {
                Debug.Log("Unable to re-localize modded stuff: " + e);
            }
            abcdcode_LOGLIKE_MOD_Extension.TextDataModel._isLoaded = true;
            Dictionary<string, BattleEffectText> dictionary = (Dictionary<string, BattleEffectText>)typeof(BattleEffectTextsXmlList).GetField("_dictionary", AccessTools.all).GetValue((object)Singleton<BattleEffectTextsXmlList>.Instance);
            dictionary["LogueLikeMod_LuckyBuf"] = new BattleEffectText()
            {
                ID = "LogueLikeMod_LuckyBuf",
                Name = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_LuckyBuf_Name"),
                Desc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_LuckyBuf_Desc")
            };
            dictionary["LogueLikeMod_LuckyBuf_Page"] = new BattleEffectText()
            {
                ID = "LogueLikeMod_LuckyBuf_Page",
                Name = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_LuckyBuf_Page_Name"),
                Desc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_LuckyBuf_Page_Desc")
            };
            dictionary["LogueLikeMod_PuppeteerBuf"] = new BattleEffectText()
            {
                ID = "LogueLikeMod_PuppeteerBuf",
                Name = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_PuppeteerBuf_Name"),
                Desc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_PuppeteerBuf_Desc")
            };
            dictionary["LogueLikeMod_MaxUpMinDownBuf"] = new BattleEffectText()
            {
                ID = "LogueLikeMod_MaxUpMinDownBuf",
                Name = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_MaxUpMinDownBuf_Name"),
                Desc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_MaxUpMinDownBuf_Desc")
            };
            dictionary["LogueLikeMod_MaxDownMinUpBuf"] = new BattleEffectText()
            {
                ID = "LogueLikeMod_MaxDownMinUpBuf",
                Name = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_MaxDownMinUpBuf_Name"),
                Desc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_MaxDownMinUpBuf_Desc")
            };
            dictionary["LogueLikeMod_CricusDawn1Buf"] = new BattleEffectText()
            {
                ID = "LogueLikeMod_CricusDawn1Buf",
                Name = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_CricusDawn1Buf_Name"),
                Desc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_CricusDawn1Buf_Desc")
            };
            dictionary["LogueLikeMod_CricusDawn2Buf"] = new BattleEffectText()
            {
                ID = "LogueLikeMod_CricusDawn2Buf",
                Name = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_CricusDawn2Buf_Name"),
                Desc = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("LogueLikeMod_CricusDawn2Buf_Desc")
            };
        }

        public static MysteryXmlRoot LoadMysteryInfos(string str, string modid)
        {
            MysteryXmlRoot mysteryXmlRoot;
            using (StringReader stringReader = new StringReader(str))
                mysteryXmlRoot = (MysteryXmlRoot)new XmlSerializer(typeof(MysteryXmlRoot)).Deserialize((TextReader)stringReader);
            foreach (MysteryXmlInfo mystery in mysteryXmlRoot.Mysterys)
            {
                if (mystery.WorkShopId == string.Empty)
                    mystery.WorkShopId = modid;
            }
            return mysteryXmlRoot;
        }

        public void LoadMysteryInfos()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/SpecialStaticInfo/MysteryXmlInfos");
            List<MysteryXmlInfo> info = new List<MysteryXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                MysteryXmlRoot mysteryXmlRoot = LogLikeMod.LoadMysteryInfos(File.ReadAllText(file.FullName), LogLikeMod.ModId);
                info.AddRange((IEnumerable<MysteryXmlInfo>)mysteryXmlRoot.Mysterys);
            }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/SpecialStaticInfo/MysteryXmlInfos");
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo2.GetFiles())
                    {
                        MysteryXmlRoot mysteryXmlRoot = LogLikeMod.LoadMysteryInfos(File.ReadAllText(file.FullName), logMod.invInfo.workshopInfo.uniqueId);
                        info.AddRange((IEnumerable<MysteryXmlInfo>)mysteryXmlRoot.Mysterys);
                    }
                }
            }
            Singleton<MysteryXmlList>.Instance.Init(info);
        }

        public static RewardPassivesRoot LoadRewardPassiveInfos(string str, string modid)
        {
            RewardPassivesRoot rewardPassivesRoot;
            using (StringReader stringReader = new StringReader(str))
                rewardPassivesRoot = (RewardPassivesRoot)new XmlSerializer(typeof(RewardPassivesRoot)).Deserialize((TextReader)stringReader);
            foreach (RewardPassivesInfo rewardPassives in rewardPassivesRoot.RewardPassivesList)
            {
                if (rewardPassives.workshopid == string.Empty)
                    rewardPassives.workshopid = modid;
                foreach (RewardPassiveInfo rewardPassive in rewardPassives.RewardPassiveList)
                {
                    if (rewardPassive.workshopID == string.Empty)
                        rewardPassive.workshopID = modid;
                    if (rewardPassive.iconartwork == string.Empty)
                        rewardPassive.iconartwork = rewardPassive.artwork;
                }
            }
            return rewardPassivesRoot;
        }

        public void LoadRewardPassiveInfos()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/SpecialStaticInfo/RewardPassiveInfos");
            List<RewardPassivesInfo> info = new List<RewardPassivesInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                RewardPassivesRoot rewardPassivesRoot = LogLikeMod.LoadRewardPassiveInfos(File.ReadAllText(file.FullName), LogLikeMod.ModId);
                info.AddRange((IEnumerable<RewardPassivesInfo>)rewardPassivesRoot.RewardPassivesList);
            }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/SpecialStaticInfo/RewardPassiveInfos");
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo2.GetFiles())
                    {
                        RewardPassivesRoot rewardPassivesRoot = LogLikeMod.LoadRewardPassiveInfos(File.ReadAllText(file.FullName), logMod.invInfo.workshopInfo.uniqueId);
                        info.AddRange((IEnumerable<RewardPassivesInfo>)rewardPassivesRoot.RewardPassivesList);
                    }
                }
            }
            Singleton<RewardPassivesList>.Instance.Init(info);
        }

        public static StagesXmlRoot LoadStages(string str, string modid)
        {
            StagesXmlRoot stagesXmlRoot;
            using (StringReader stringReader = new StringReader(str))
                stagesXmlRoot = (StagesXmlRoot)new XmlSerializer(typeof(StagesXmlRoot)).Deserialize((TextReader)stringReader);
            foreach (StagesXmlInfo dropValueXml in stagesXmlRoot.DropValueXmlList)
            {
                foreach (LogueStageInfo stage in dropValueXml.Stages)
                {
                    if (stage.workshopid == string.Empty)
                        stage.workshopid = modid;
                }
            }
            return stagesXmlRoot;
        }

        public void LoadStages()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/SpecialStaticInfo/StagesXmlInfos");
            List<StagesXmlInfo> info = new List<StagesXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                StagesXmlRoot stagesXmlRoot = LogLikeMod.LoadStages(File.ReadAllText(file.FullName), LogLikeMod.ModId);
                info.AddRange((IEnumerable<StagesXmlInfo>)stagesXmlRoot.DropValueXmlList);
            }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/SpecialStaticInfo/StagesXmlInfos");
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo2.GetFiles())
                    {
                        StagesXmlRoot stagesXmlRoot = LogLikeMod.LoadStages(File.ReadAllText(file.FullName), logMod.invInfo.workshopInfo.uniqueId);
                        info.AddRange((IEnumerable<StagesXmlInfo>)stagesXmlRoot.DropValueXmlList);
                    }
                }
            }
            Singleton<StagesXmlList>.Instance.Init(info);
        }

        public static CardDropValueXmlRoot LoadDropValues(string str, string modid)
        {
            CardDropValueXmlRoot dropValueXmlRoot;
            using (StringReader stringReader = new StringReader(str))
                dropValueXmlRoot = (CardDropValueXmlRoot)new XmlSerializer(typeof(CardDropValueXmlRoot)).Deserialize((TextReader)stringReader);
            foreach (CardDropValueXmlInfo dropValueXml in dropValueXmlRoot.DropValueXmlList)
            {
                if (dropValueXml.workshopID == string.Empty)
                    dropValueXml.workshopID = modid;
            }
            return dropValueXmlRoot;
        }

        public void LoadDropValues()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/SpecialStaticInfo/DropValueXmlInfos");
            List<CardDropValueXmlInfo> info = new List<CardDropValueXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                CardDropValueXmlRoot dropValueXmlRoot = LogLikeMod.LoadDropValues(File.ReadAllText(file.FullName), LogLikeMod.ModId);
                info.AddRange((IEnumerable<CardDropValueXmlInfo>)dropValueXmlRoot.DropValueXmlList);
            }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/SpecialStaticInfo/DropValueXmlInfos");
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo2.GetFiles())
                    {
                        CardDropValueXmlRoot dropValueXmlRoot = LogLikeMod.LoadDropValues(File.ReadAllText(file.FullName), logMod.invInfo.workshopInfo.uniqueId);
                        info.AddRange((IEnumerable<CardDropValueXmlInfo>)dropValueXmlRoot.DropValueXmlList);
                    }
                }
            }
            Singleton<CardDropValueList>.Instance.Init(info);
        }

        public static LogStoryPathRoot LoadStoryPath(string str, string modid)
        {
            LogStoryPathRoot logStoryPathRoot;
            using (StringReader stringReader = new StringReader(str))
                logStoryPathRoot = (LogStoryPathRoot)new XmlSerializer(typeof(LogStoryPathRoot)).Deserialize((TextReader)stringReader);
            foreach (LogStoryPathInfo logStoryPathInfo in logStoryPathRoot.list)
            {
                if (logStoryPathInfo.pid == string.Empty)
                    logStoryPathInfo.pid = modid;
            }
            return logStoryPathRoot;
        }

        public void LoadStoryPath()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/SpecialStaticInfo/StoryPathInfos");
            List<LogStoryPathInfo> infolist = new List<LogStoryPathInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                LogStoryPathRoot logStoryPathRoot = LogLikeMod.LoadStoryPath(File.ReadAllText(file.FullName), LogLikeMod.ModId);
                infolist.AddRange((IEnumerable<LogStoryPathInfo>)logStoryPathRoot.list);
            }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/SpecialStaticInfo/StoryPathInfos");
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo2.GetFiles())
                    {
                        LogStoryPathRoot logStoryPathRoot = LogLikeMod.LoadStoryPath(File.ReadAllText(file.FullName), logMod.invInfo.workshopInfo.uniqueId);
                        infolist.AddRange((IEnumerable<LogStoryPathInfo>)logStoryPathRoot.list);
                    }
                }
            }
            Singleton<LogStoryPathList>.Instance.AddStoryPathInfo(infolist);
        }

        public static EmotionEgoXmlInfo AddEmotionEgoForReward(DiceCardXmlInfo info)
        {
            if (LogLikeMod.RewardCardDic_Dummy == null)
                LogLikeMod.RewardCardDic_Dummy = new Dictionary<string, List<EmotionEgoXmlInfo>>();
            EmotionEgoXmlInfo emotionEgoXmlInfo = new EmotionEgoXmlInfo();
            emotionEgoXmlInfo._CardId = info.id.id;
            emotionEgoXmlInfo.Sephirah = SephirahType.None;
            emotionEgoXmlInfo.id = -1;
            emotionEgoXmlInfo.isLock = false;
            if (info.id.packageId == string.Empty)
                return emotionEgoXmlInfo;
            if (!LogLikeMod.RewardCardDic_Dummy.ContainsKey(info.id.packageId))
                LogLikeMod.RewardCardDic_Dummy.Add(info.id.packageId, new List<EmotionEgoXmlInfo>());
            LogLikeMod.RewardCardDic_Dummy[info.id.packageId].Add(emotionEgoXmlInfo);
            return emotionEgoXmlInfo;
        }

        public static List<Assembly> GetAssemList()
        {
            Dictionary<string, List<Assembly>> dictionary = (Dictionary<string, List<Assembly>>)typeof(AssemblyManager).GetField("_assemblyDict", AccessTools.all).GetValue((object)Singleton<AssemblyManager>.Instance);
            List<Assembly> assemList = new List<Assembly>();
            if (dictionary != null)
            {
                foreach (List<Assembly> assemblyList in dictionary.Values)
                {
                    foreach (Assembly assembly in assemblyList)
                    {
                        if (!assemList.Contains(assembly))
                            assemList.Add(assembly);
                    }
                }
            }
            if (LogLikeMod.LogModAssemblys != null && LogLikeMod.LogModAssemblys.Count > 0)
            {
                foreach (Assembly logModAssembly in LogLikeMod.LogModAssemblys)
                {
                    if (!assemList.Contains(logModAssembly))
                        assemList.Add(logModAssembly);
                }
            }
            return assemList;
        }

        public static void SetStagePhase(StageController __instance, StageController.StagePhase phase)
        {
            typeof(StageController).GetMethod("set_phase", AccessTools.all).Invoke((object)__instance, new object[1]
            {
      (object) phase
            });
        }

        public static bool IsBattleState()
        {
            UIPhase currentUiPhase = UI.UIController.Instance.CurrentUIPhase;
            int num;
            switch (currentUiPhase)
            {
                case UIPhase.BattleSetting:
                case UIPhase.BattleResult:
                    num = 1;
                    break;
                default:
                    num = currentUiPhase == UIPhase.DUMMY ? 1 : 0;
                    break;
            }
            return num != 0;
        }

        public static bool CheckStage(bool OnBattle = false)
        {
            try
            {
                UIPhase currentUiPhase = UI.UIController.Instance.CurrentUIPhase;
                int num;
                switch (currentUiPhase)
                {
                    case UIPhase.Sephirah:
                    case UIPhase.Main_ItemList:
                    case UIPhase.Librarian_CardList:
                        num = 1;
                        break;
                    default:
                        num = currentUiPhase == UIPhase.Sepiroth ? 1 : 0;
                        break;
                }
                if (num != 0)
                    return false;
                LorId stage = Singleton<StageController>.Instance.GetStageModel().ClassInfo.id;
                if (stage == RMRCore.CurrentGamemode.StageStart || LogLikeMod.saveloading)
                    return true;
                if (stage == new LorId(LogLikeMod.ModId, -855))
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        public static void LoadPassives()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/Passives");
            List<PassiveXmlInfo> list1 = new List<PassiveXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                List<PassiveXmlInfo> collection = LogLikeMod.LoadPassive(file.FullName, LogLikeMod.ModId);
                list1.AddRange((IEnumerable<PassiveXmlInfo>)collection);
            }
            Singleton<PassiveXmlList>.Instance.AddPassivesByMod(list1);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<PassiveXmlInfo> list2 = new List<PassiveXmlInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/Passives");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (System.IO.FileInfo file in directoryInfo2.GetFiles())
                        list2.AddRange((IEnumerable<PassiveXmlInfo>)LogLikeMod.LoadPassive(file.FullName, uniqueId));
                    Singleton<PassiveXmlList>.Instance.AddPassivesByMod(list2);
                }
            }
        }

        public static List<PassiveXmlInfo> LoadPassive(string path, string modid)
        {
            List<PassiveXmlInfo> passiveXmlInfoList = new List<PassiveXmlInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                        passiveXmlInfoList = (new XmlSerializer(typeof(PassiveXmlRoot)).Deserialize((TextReader)streamReader) as PassiveXmlRoot).list;
                }
                foreach (PassiveXmlInfo passiveXmlInfo in passiveXmlInfoList)
                    passiveXmlInfo.workshopID = modid;
            }
            catch (Exception ex)
            {
                Debug.Log((object)(ex.Message + Environment.NewLine + ex.StackTrace));
            }
            return passiveXmlInfoList;
        }

        public static void LoadDecks()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/Deck");
            List<DeckXmlInfo> list1 = new List<DeckXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                List<DeckXmlInfo> collection = LogLikeMod.LoadDeck(file.FullName, LogLikeMod.ModId);
                list1.AddRange((IEnumerable<DeckXmlInfo>)collection);
            }
            Singleton<DeckXmlList>.Instance.AddDeckByMod(list1);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<DeckXmlInfo> list2 = new List<DeckXmlInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/Deck");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (System.IO.FileInfo file in directoryInfo2.GetFiles())
                        list2.AddRange((IEnumerable<DeckXmlInfo>)LogLikeMod.LoadDeck(file.FullName, uniqueId));
                    Singleton<DeckXmlList>.Instance.AddDeckByMod(list2);
                }
            }
        }

        public static List<DeckXmlInfo> LoadDeck(string path, string modid)
        {
            List<DeckXmlInfo> deckXmlInfoList = new List<DeckXmlInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                        deckXmlInfoList = (new XmlSerializer(typeof(DeckXmlRoot)).Deserialize((TextReader)streamReader) as DeckXmlRoot).deckXmlList;
                }
                foreach (DeckXmlInfo deckXmlInfo in deckXmlInfoList)
                {
                    deckXmlInfo.workshopId = modid;
                    LorId.InitializeLorIds<LorIdXml>(deckXmlInfo._cardIdList, deckXmlInfo.cardIdList, modid);
                }
            }
            catch (Exception ex)
            {
                Debug.Log((object)(ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog(ex.Message);
            }
            return deckXmlInfoList;
        }

        public static void LoadDropBooks()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/DropBook");
            List<DropBookXmlInfo> list1 = new List<DropBookXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                List<DropBookXmlInfo> collection = LogLikeMod.LoadDropBook(file.FullName, LogLikeMod.ModId);
                list1.AddRange((IEnumerable<DropBookXmlInfo>)collection);
            }
            Singleton<DropBookXmlList>.Instance.SetDropTableByMod(list1);
            Singleton<DropBookXmlList>.Instance.AddBookByMod(LogLikeMod.ModId, list1);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<DropBookXmlInfo> list2 = new List<DropBookXmlInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/DropBook");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (System.IO.FileInfo file in directoryInfo2.GetFiles())
                    {
                        File.ReadAllText(file.FullName);
                        list2.AddRange((IEnumerable<DropBookXmlInfo>)LogLikeMod.LoadDropBook(file.FullName, uniqueId));
                    }
                    Singleton<DropBookXmlList>.Instance.SetDropTableByMod(list2);
                    Singleton<DropBookXmlList>.Instance.AddBookByMod(uniqueId, list2);
                }
            }
        }

        public static List<DropBookXmlInfo> LoadDropBook(string path, string modid)
        {
            List<DropBookXmlInfo> dropBookXmlInfoList = new List<DropBookXmlInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                    {
                        dropBookXmlInfoList = (new XmlSerializer(typeof(BookUseXmlRoot)).Deserialize((TextReader)streamReader) as BookUseXmlRoot).bookXmlList;
                        foreach (DropBookXmlInfo dropBookXmlInfo in dropBookXmlInfoList)
                        {
                            dropBookXmlInfo.workshopID = modid;
                            dropBookXmlInfo.InitializeDropItemList(modid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log((object)(ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog(ex.Message);
            }
            return dropBookXmlInfoList;
        }

        public static void LoadCardDropTables()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/CardDropTable");
            List<CardDropTableXmlInfo> list1 = new List<CardDropTableXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                List<CardDropTableXmlInfo> collection = LogLikeMod.LoadCardDropTable(file.FullName, LogLikeMod.ModId);
                list1.AddRange((IEnumerable<CardDropTableXmlInfo>)collection);
            }
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<CardDropTableXmlInfo> list2 = new List<CardDropTableXmlInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/CardDropTable");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (FileSystemInfo file in directoryInfo2.GetFiles())
                    {
                        List<CardDropTableXmlInfo> collection = LogLikeMod.LoadCardDropTable(file.FullName, uniqueId);
                        foreach (CardDropTableXmlInfo dropTableXmlInfo1 in new List<CardDropTableXmlInfo>((IEnumerable<CardDropTableXmlInfo>)collection))
                        {
                            CardDropTableXmlInfo tempinfo = dropTableXmlInfo1;
                            CardDropTableXmlInfo dropTableXmlInfo2 = list1.Find((Predicate<CardDropTableXmlInfo>)(x => x.id == tempinfo.id));
                            if (dropTableXmlInfo2 != null)
                            {
                                dropTableXmlInfo2.cardIdList.AddRange((IEnumerable<LorId>)tempinfo.cardIdList);
                                dropTableXmlInfo2._cardIdList.AddRange((IEnumerable<LorIdXml>)tempinfo._cardIdList);
                                collection.Remove(tempinfo);
                            }
                        }
                        list2.AddRange((IEnumerable<CardDropTableXmlInfo>)collection);
                    }
                    Singleton<CardDropTableXmlList>.Instance.AddCardDropTableByMod(uniqueId, list2);
                }
            }
            Singleton<CardDropTableXmlList>.Instance.AddCardDropTableByMod(LogLikeMod.ModId, list1);
        }

        public static List<CardDropTableXmlInfo> LoadCardDropTable(string path, string modid)
        {
            List<CardDropTableXmlInfo> dropTableXmlInfoList = new List<CardDropTableXmlInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                        dropTableXmlInfoList = (new XmlSerializer(typeof(abcdcode_LOGLIKE_MOD_Extension.CardDropTableXmlRoot)).Deserialize((TextReader)streamReader) as abcdcode_LOGLIKE_MOD_Extension.CardDropTableXmlRoot).Convert().dropTableXmlList;
                }
                foreach (CardDropTableXmlInfo dropTableXmlInfo in dropTableXmlInfoList)
                {
                    if (dropTableXmlInfo.workshopId == string.Empty)
                        dropTableXmlInfo.workshopId = modid;
                    dropTableXmlInfo.cardIdList.Clear();
                    LorId.InitializeLorIds<LorIdXml>(dropTableXmlInfo._cardIdList, dropTableXmlInfo.cardIdList, modid);
                }
            }
            catch (Exception ex)
            {
                Debug.Log((object)(ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog(ex.Message);
            }
            return dropTableXmlInfoList;
        }

        public static void LoadCardInfos()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/CardInfo");
            List<DiceCardXmlInfo> list1 = new List<DiceCardXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                List<DiceCardXmlInfo> collection = LogLikeMod.LoadCardInfo(file.FullName, LogLikeMod.ModId);
                list1.AddRange((IEnumerable<DiceCardXmlInfo>)collection);
            }
            ItemXmlDataList.instance.AddCardInfoByMod(LogLikeMod.ModId, list1);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<DiceCardXmlInfo> list2 = new List<DiceCardXmlInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/CardInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (System.IO.FileInfo file in directoryInfo2.GetFiles())
                        list2.AddRange((IEnumerable<DiceCardXmlInfo>)LogLikeMod.LoadCardInfo(file.FullName, uniqueId));
                    ItemXmlDataList.instance.AddCardInfoByMod(uniqueId, list2);
                }
            }
        }

        public static List<DiceCardXmlInfo> LoadCardInfo(string path, string modid)
        {
            List<DiceCardXmlInfo> diceCardXmlInfoList = new List<DiceCardXmlInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                    {
                        diceCardXmlInfoList = (new XmlSerializer(typeof(DiceCardXmlRoot)).Deserialize((TextReader)streamReader) as DiceCardXmlRoot).cardXmlList;
                        foreach (DiceCardXmlInfo diceCardXmlInfo in diceCardXmlInfoList)
                            diceCardXmlInfo.workshopID = modid;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log((object)(ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog(ex.Message);
            }
            return diceCardXmlInfoList;
        }

        public static void LoadStageInfos()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/StageInfo");
            List<StageClassInfo> list1 = new List<StageClassInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                List<StageClassInfo> collection = LogLikeMod.LoadStage(file.FullName, LogLikeMod.ModId);
                list1.AddRange((IEnumerable<StageClassInfo>)collection);
            }
            Singleton<StageClassInfoList>.Instance.AddStageByMod(LogLikeMod.ModId, list1);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<StageClassInfo> list2 = new List<StageClassInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/StageInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (System.IO.FileInfo file in directoryInfo2.GetFiles())
                    {
                        File.ReadAllText(file.FullName);
                        list2.AddRange((IEnumerable<StageClassInfo>)LogLikeMod.LoadStage(file.FullName, uniqueId));
                    }
                    Singleton<StageClassInfoList>.Instance.AddStageByMod(uniqueId, list2);
                }
            }
        }

        public static List<StageClassInfo> LoadStage(string path, string modid)
        {
            List<StageClassInfo> stageClassInfoList = new List<StageClassInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                    {
                        stageClassInfoList = (new XmlSerializer(typeof(StageXmlRoot)).Deserialize((TextReader)streamReader) as StageXmlRoot).list;
                        foreach (StageClassInfo stageClassInfo in stageClassInfoList)
                        {
                            stageClassInfo.workshopID = modid;
                            stageClassInfo.InitializeIds(modid);
                            foreach (StageStoryInfo story in stageClassInfo.storyList)
                            {
                                story.packageId = modid;
                                story.valid = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log((object)(ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog(ex.Message);
            }
            return stageClassInfoList;
        }

        public static void LoadEnemyUnitInfos()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/EnemyUnitInfo");
            List<EnemyUnitClassInfo> list1 = new List<EnemyUnitClassInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                List<EnemyUnitClassInfo> collection = LogLikeMod.LoadEnemyUnit(file.FullName, LogLikeMod.ModId);
                list1.AddRange((IEnumerable<EnemyUnitClassInfo>)collection);
            }
            Singleton<EnemyUnitClassInfoList>.Instance.AddEnemyUnitByMod(LogLikeMod.ModId, list1);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<EnemyUnitClassInfo> list2 = new List<EnemyUnitClassInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/EnemyUnitInfo");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (System.IO.FileInfo file in directoryInfo2.GetFiles())
                    {
                        File.ReadAllText(file.FullName);
                        list2.AddRange((IEnumerable<EnemyUnitClassInfo>)LogLikeMod.LoadEnemyUnit(file.FullName, uniqueId));
                    }
                    Singleton<EnemyUnitClassInfoList>.Instance.AddEnemyUnitByMod(uniqueId, list2);
                }
            }
        }

        public static List<EnemyUnitClassInfo> LoadEnemyUnit(string path, string modid)
        {
            List<EnemyUnitClassInfo> enemyUnitClassInfoList = new List<EnemyUnitClassInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                    {
                        enemyUnitClassInfoList = (new XmlSerializer(typeof(EnemyUnitClassRoot)).Deserialize((TextReader)streamReader) as EnemyUnitClassRoot).list;
                        foreach (EnemyUnitClassInfo enemyUnitClassInfo in enemyUnitClassInfoList)
                        {
                            enemyUnitClassInfo.workshopID = modid;
                            enemyUnitClassInfo.height = RandomUtil.SystemRange(enemyUnitClassInfo.maxHeight - enemyUnitClassInfo.minHeight) + enemyUnitClassInfo.minHeight;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log((object)(ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog(ex.Message);
            }
            return enemyUnitClassInfoList;
        }

        public static bool CheckIsEquipReward(RewardPassiveInfo x)
        {
            PickUpModelBase pickUp = LogLikeMod.FindPickUp(x.script);
            return pickUp is ShopPickUpModel && (pickUp as ShopPickUpModel).IsEquipReward();
        }

        public static void CreateShopEquipPages()
        {
            if (LogLikeMod.CreatedShopEquipPages)
                return;
            List<RewardPassivesInfo> all = Singleton<RewardPassivesList>.Instance.infos.FindAll((Predicate<RewardPassivesInfo>)(x => x.rewardtype == PassiveRewardListType.Shop));
            List<RewardPassiveInfo> rewardPassiveInfoList = new List<RewardPassiveInfo>();
            Dictionary<string, List<BookXmlInfo>> dictionary = new Dictionary<string, List<BookXmlInfo>>();
            foreach (RewardPassivesInfo rewardPassivesInfo in all)
                rewardPassiveInfoList.AddRange((IEnumerable<RewardPassiveInfo>)rewardPassivesInfo.RewardPassiveList.FindAll((Predicate<RewardPassiveInfo>)(x => LogLikeMod.CheckIsEquipReward(x))));
            string path = LogLikeMod.path + "/AddData/EquipPage/EquipPage_ShopDefault.xml";
            foreach (RewardPassiveInfo rewardPassiveInfo in rewardPassiveInfoList)
            {
                ShopPickUpModel pickUp = LogLikeMod.FindPickUp(rewardPassiveInfo.script) as ShopPickUpModel;
                BookXmlInfo bookXmlInfo = LogLikeMod.LoadEquipPage(path, LogLikeMod.ModId)[0];
                bookXmlInfo._bookIcon = "<LogLike>" + rewardPassiveInfo.artwork;
                bookXmlInfo.EquipEffect._PassiveList.Add(new LorIdXml()
                {
                    pid = pickUp.basepassive.workshopID,
                    xmlId = pickUp.basepassive._id
                });
                bookXmlInfo.EquipEffect.PassiveList.Add(pickUp.basepassive.id);
                bookXmlInfo.workshopID = rewardPassiveInfo.workshopID;
                bookXmlInfo._id = rewardPassiveInfo.passiveid;
                bookXmlInfo.skinType = "CUSTOM";
                bookXmlInfo.CharacterSkin[0] = bookXmlInfo._bookIcon;
                if (!dictionary.ContainsKey(rewardPassiveInfo.workshopID))
                    dictionary.Add(rewardPassiveInfo.workshopID, new List<BookXmlInfo>());
                dictionary[rewardPassiveInfo.workshopID].Add(bookXmlInfo);
            }
            foreach (KeyValuePair<string, List<BookXmlInfo>> keyValuePair in dictionary)
            {
                Singleton<BookXmlList>.Instance.AddEquipPageByMod(keyValuePair.Key, keyValuePair.Value);
                Dictionary<string, List<BookXmlInfo>> fieldValue = LogLikeMod.GetFieldValue<Dictionary<string, List<BookXmlInfo>>>((object)Singleton<BookXmlList>.Instance, "_workshopBookDict");
                if (!fieldValue.ContainsKey(keyValuePair.Key))
                    fieldValue.Add(keyValuePair.Key, new List<BookXmlInfo>());
                fieldValue[keyValuePair.Key].AddRange((IEnumerable<BookXmlInfo>)keyValuePair.Value);
            }
            LogLikeMod.CreatedShopEquipPages = true;
        }

        public static void LoadEquipPages()
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(LogLikeMod.path + "/AddData/EquipPage");
            List<BookXmlInfo> list1 = new List<BookXmlInfo>();
            foreach (FileSystemInfo file in directoryInfo1.GetFiles())
            {
                List<BookXmlInfo> collection = LogLikeMod.LoadEquipPage(file.FullName, LogLikeMod.ModId);
                list1.AddRange((IEnumerable<BookXmlInfo>)collection);
            }
            Singleton<BookXmlList>.Instance.AddEquipPageByMod(LogLikeMod.ModId, list1);
            foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
            {
                List<BookXmlInfo> list2 = new List<BookXmlInfo>();
                DirectoryInfo directoryInfo2 = new DirectoryInfo(logMod.GetLogDllPath() + "/AddData/EquipPage");
                string uniqueId = logMod.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(directoryInfo2.FullName))
                {
                    foreach (System.IO.FileInfo file in directoryInfo2.GetFiles())
                    {
                        File.ReadAllText(file.FullName);
                        list2.AddRange((IEnumerable<BookXmlInfo>)LogLikeMod.LoadEquipPage(file.FullName, uniqueId));
                    }
                    Singleton<BookXmlList>.Instance.AddEquipPageByMod(uniqueId, list2);
                }
            }
        }

        public static List<BookXmlInfo> LoadEquipPage(string path, string modid)
        {
            List<BookXmlInfo> bookXmlInfoList = new List<BookXmlInfo>();
            try
            {
                string path1 = path;
                if (File.Exists(path1))
                {
                    using (StreamReader streamReader = new StreamReader(path1))
                    {
                        bookXmlInfoList = (new XmlSerializer(typeof(BookXmlRoot)).Deserialize((TextReader)streamReader) as BookXmlRoot).bookXmlList;
                        foreach (BookXmlInfo bookXmlInfo in bookXmlInfoList)
                        {
                            bookXmlInfo.workshopID = modid;
                            LorId.InitializeLorIds<LorIdXml>(bookXmlInfo.EquipEffect._PassiveList, bookXmlInfo.EquipEffect.PassiveList, modid);
                            if (!string.IsNullOrEmpty(bookXmlInfo.skinType))
                            {
                                if (bookXmlInfo.skinType == "UNKNOWN")
                                    bookXmlInfo.skinType = "Lor";
                                else if (bookXmlInfo.skinType == "CUSTOM")
                                    bookXmlInfo.skinType = "Custom";
                                else if (bookXmlInfo.skinType == "LOR")
                                    bookXmlInfo.skinType = "Lor";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log((object)(ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog(ex.Message);
            }
            return bookXmlInfoList;
        }

        public void Patching(
          Harmony harmony,
          MethodBase original,
          HarmonyMethod prefix = null,
          HarmonyMethod postfix = null,
          HarmonyMethod transpiler = null,
          HarmonyMethod finalizer = null,
          HarmonyMethod ilmanipulator = null,
          bool Debug = false)
        {
            Debug = LogLikeMod.Debugging;
            if (Debug)
                Debug.Log(("Patch : " + original.Name));
            harmony.Patch(original, prefix, postfix, transpiler, finalizer, ilmanipulator);
            if (!Debug)
                return;
            Debug.Log(("Patch Successs : " + original.Name));
        }

        public override void OnInitializeMod()
        {
            try
            {
                LogLikeMod.path = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
                LogLikeMod.Debugging = LogLikeMod.path.Contains("LogLikeMod_DebugP");
                base.OnInitializeMod();
                Harmony harmony = new Harmony("abcdcode.LogLikeMOD");
                HookHelper.CreateHook(typeof(StageController), "RoundEndPhase_ChoiceEmotionCard", (object)this, "StageController_RoundEndPhase_ChoiceEmotionCard");
                HookHelper.CreateHook(typeof(StageController), "InitStageByInvitation", (object)this, "StageController_InitStageByInvitation");
                HookHelper.CreateHook(typeof(StageController), "RoundEndPhase_ReturnUnit", (object)this, "RoundEndPhase_ReturnUnit");
                HookHelper.CreateHook(typeof(StageController), (MethodBase)typeof(StageController).GetMethod("CreateLibrarianUnit", AccessTools.all, (System.Reflection.Binder)null, new System.Type[1]
                {
        typeof (SephirahType)
                }, (ParameterModifier[])null), (object)this, "StageController_CreateLibrarianUnit");
                HookHelper.CreateHook(typeof(StageController), "StartBattle", (object)this, "StageController_StartBattle");
                HookHelper.CreateHook(typeof(StageController), "OnEnemyDropBookForAdded", (object)this, "StageController_OnEnemyDropBookForAdded");
                HookHelper.CreateHook(typeof(StageController), "EndBattlePhase", (object)this, "StageController_EndBattlePhase");
                HookHelper.CreateHook(typeof(StageController), "EndBattle", (object)this, "StageController_EndBattle");
                HarmonyMethod postfix1 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("StageController_ClearBattle", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(StageController).GetMethod("ClearBattle", AccessTools.all), postfix: postfix1);
                HookHelper.CreateHook(typeof(StageController), "OnUpdate", (object)this, "StageController_OnUpdate");
                HookHelper.CreateHook(typeof(StageLibraryFloorModel), "OnPickPassiveCard", (object)this, "StageLibraryFloorModel_OnPickPassiveCard");
                HookHelper.CreateHook(typeof(StageLibraryFloorModel), "OnPickEgoCard", (object)this, "StageLibraryFloorModel_OnPickEgoCard");
                HookHelper.CreateHook(typeof(LevelUpUI), "InitBase", (object)this, "LevelUpUI_InitBase");
                HookHelper.CreateHook(typeof(LevelUpUI), "SetEmotionPerDataUI", (object)this, "LevelUpUI_SetEmotionPerDataUI");
                HookHelper.CreateHook(typeof(LevelUpUI), "OnSelectRoutine", (object)this, "LevelUpUI_OnSelectRoutine");
                HookHelper.CreateHook(typeof(DropBookInventoryModel), "GetBookList_invitationBookList", (object)this, "DropBookInventoryModel_GetBookList_invitationBookList");
                HookHelper.CreateHook(typeof(BookInventoryModel), "GetBookList_equip", (object)this, "BookInventoryModel_GetBookList_equip");
                HookHelper.CreateHook(typeof(UIBattleSettingPanel), "SetCurrentSephirahButton", (object)this, "UIBattleSettingPanel_SetCurrentSephirahButton");
                HookHelper.CreateHook(typeof(UILibrarianCharacterListPanel), "InitSephirahSelectionButtonsInBattle", (object)this, "UILibrarianCharacterListPanel_InitSephirahSelectionButtonsInBattle");
                HookHelper.CreateHook(typeof(UILibrarianCharacterListPanel), "SetLibrarianCharacterListPanel_Battle", (object)this, "UILibrarianCharacterListPanel_SetLibrarianCharacterListPanel_Battle");
                HookHelper.CreateHook(typeof(UIInvitationRightMainPanel), "ConfirmSendInvitation", (object)this, "UIInvitationRightMainPanel_ConfirmSendInvitation");
                HookHelper.CreateHook(typeof(UIInvenCardSlot), "SetSlotState", (object)this, "UIInvenCardSlot_SetSlotState");
                HookHelper.CreateHook(typeof(UIInvenCardSlot), "OnClickCardEquipInfoButton", (object)this, "UIInvenCardSlot_OnClickCardEquipInfoButton");
                HookHelper.CreateHook(typeof(UnitDataModel), "AddCardFromInventory", (object)this, "UnitDataModel_AddCardFromInventory");
                HookHelper.CreateHook(typeof(UIInvenCardListScroll), "SetData", (object)this, "UIInvenCardListScroll_SetData");
                HookHelper.CreateHook(typeof(DeckModel), "AddCardFromInventory", (object)this, "DeckModel_AddCardFromInventory");
                HookHelper.CreateHook(typeof(DeckModel), "MoveCardToInventory", (object)this, "DeckModel_MoveCardToInventory");
                HookHelper.CreateHook(typeof(BattleUnitCardsInHandUI), "SetCardsObject", (object)this, "BattleUnitCardsInHandUI_SetCardsObject");
                HookHelper.CreateHook(typeof(BattleDiceCardUI), "SetEgoCardForPopup", (object)this, "BattleDiceCardUI_SetEgoCardForPopup");
                HookHelper.CreateHook(typeof(UIBattleSettingLibrarianInfoPanel), "SetBattleCardSlotState", (object)this, "UIBattleSettingLibrarianInfoPanel_SetBattleCardSlotState");
                HookHelper.CreateHook(typeof(UIBattleSettingLibrarianInfoPanel), "SetEquipPageSlotState", (object)this, "UIBattleSettingLibrarianInfoPanel_SetEquipPageSlotState");
                HookHelper.CreateHook(typeof(UIBattleSettingEditPanel), "Open", (object)this, "UIBattleSettingEditPanel_Open");
                HookHelper.CreateHook(typeof(BattleUnitEmotionDetail), "CreateEmotionCoin", (object)this, "BattleUnitEmotionDetail_CreateEmotionCoin");
                HookHelper.CreateHook(typeof(BattleUnitEmotionDetail), "Reset", (object)this, "BattleUnitEmotionDetail_Reset");
                HookHelper.CreateHook(typeof(WorkshopSkinDataSetter), "LateInit", (object)this, "WorkshopSkinDataSetter_LateInit");
                HookHelper.CreateHook(typeof(BookXmlInfo), "get_DeckId", (object)this, "BookXmlInfo_get_DeckId");
                HookHelper.CreateHook(typeof(BattleSceneRoot), "Update", (object)this, "BattleSceneRoot_Update");
                HookHelper.CreateHook(typeof(UIGetAbnormalityPanel), "PointerClickButton", (object)this, "UIGetAbnormalityPanel_PointerClickButton");
                HookHelper.CreateHook(typeof(AbnormalityCardDescXmlList), "GetAbnormalityCard", (object)this, "AbnormalityCardDescXmlList_GetAbnormalityCard");
                HarmonyMethod postfix2 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIController_CallUIPhase", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UI.UIController).GetMethod("CallUIPhase", AccessTools.all, (System.Reflection.Binder)null, new System.Type[1]
                {
        typeof (UIPhase)
                }, (ParameterModifier[])null), postfix: postfix2);
                HarmonyMethod prefix1 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleAllyCardDetail_ReturnCardToHand", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleAllyCardDetail).GetMethod("ReturnCardToHand", AccessTools.all), prefix1);
                HarmonyMethod prefix2 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleEmotionCoinUI_Init", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleEmotionCoinUI).GetMethod("Init", AccessTools.all), prefix2);
                HarmonyMethod prefix3 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleEmotionInfo_CenterBtn_OnPointerUp", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleEmotionInfo_CenterBtn).GetMethod("OnPointerUp", AccessTools.all), prefix3);
                HarmonyMethod prefix4 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleEmotionRewardInfoUI_SetData", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleEmotionRewardInfoUI).GetMethod("SetData", AccessTools.all), prefix4);
                HarmonyMethod prefix5 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattlePlayingCardDataInUnitModel_OnUseCard", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattlePlayingCardDataInUnitModel).GetMethod("OnUseCard", AccessTools.all), prefix5);
                HarmonyMethod prefix6 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattlePlayingCardSlotDetail_OnApplyCard", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattlePlayingCardSlotDetail).GetMethod("OnApplyCard", AccessTools.all), prefix6);
                HarmonyMethod prefix7 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattlePersonalEgoCardDetail_ReturnCardToHand", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattlePersonalEgoCardDetail).GetMethod("ReturnCardToHand", AccessTools.all), prefix7);
                HarmonyMethod prefix8 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleAllyCardDetail_ReturnCardToHand", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleAllyCardDetail).GetMethod("ReturnCardToHand", AccessTools.all), prefix8);
                HarmonyMethod postfix3 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleDiceCardModel_GetCost", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleDiceCardModel).GetMethod("GetCost", AccessTools.all), postfix: postfix3);
                HarmonyMethod postfix4 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleDiceCardUI_GetClickableState", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleDiceCardUI).GetMethod("GetClickableState", AccessTools.all), postfix: postfix4);
                HarmonyMethod prefix9 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleUnitInfoManagerUI_Initialize", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleUnitInfoManagerUI).GetMethod("Initialize", AccessTools.all), prefix9);
                HarmonyMethod postfix5 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleUnitModel_OnDie", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleUnitModel).GetMethod("OnDie", AccessTools.all), postfix: postfix5);
                HarmonyMethod postfix6 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleUnitPassiveDetail_DmgFactor", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleUnitPassiveDetail).GetMethod("DmgFactor", AccessTools.all), postfix: postfix6);
                HarmonyMethod prefix10 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleUnitModel_BeforeRollDice", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleUnitModel).GetMethod("BeforeRollDice", AccessTools.all), prefix10);
                HarmonyMethod postfix7 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleUnitBuf_Destroy", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleUnitBuf).GetMethod("Destroy", AccessTools.all), postfix: postfix7);
                HarmonyMethod prefix11 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleUnitBuf_burn_OnRoundEnd", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleUnitBuf_burn).GetMethod("OnRoundEnd", AccessTools.all), prefix11);
                HarmonyMethod postfix8 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleUnitBufListDetail_CheckGift", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleUnitBufListDetail).GetMethod("CheckGift", AccessTools.all), postfix: postfix8);
                HarmonyMethod postfix9 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleUnitPassiveDetail_OnDie", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleUnitPassiveDetail).GetMethod("OnDie", AccessTools.all), postfix: postfix9);
                HarmonyMethod postfix10 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BattleUnitPassiveDetail_OnKill", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BattleUnitPassiveDetail).GetMethod("OnKill", AccessTools.all), postfix: postfix10);
                HarmonyMethod postfix11 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BookInventoryModel_GetBookList_PassiveEquip", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BookInventoryModel).GetMethod("GetBookList_PassiveEquip", AccessTools.all), postfix: postfix11);
                HarmonyMethod postfix12 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BookInventoryModel_GetBookListAll", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BookInventoryModel).GetMethod("GetBookListAll", AccessTools.all), postfix: postfix12);
                HarmonyMethod postfix13 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BookInventoryModel_GetBookByInstanceId", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BookInventoryModel).GetMethod("GetBookByInstanceId", AccessTools.all), postfix: postfix13);
                HarmonyMethod postfix14 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BookInventoryModel_GetAllBookByInstanceId", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BookInventoryModel).GetMethod("GetAllBookByInstanceId", AccessTools.all), postfix: postfix14);
                HarmonyMethod postfix15 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BookPassiveInfo_get_desc_postfix", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BookPassiveInfo).GetMethod("get_desc", AccessTools.all), postfix: postfix15);
                HarmonyMethod prefix12 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BookPassiveInfo_get_desc", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BookPassiveInfo).GetMethod("get_desc", AccessTools.all), prefix12);
                HarmonyMethod prefix13 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BookPassiveInfo_get_name", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BookPassiveInfo).GetMethod("get_name", AccessTools.all), prefix13);
                HarmonyMethod prefix14 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("CharacterAppearance_ChangeMotion_prefix", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(CharacterAppearance).GetMethod("ChangeMotion", AccessTools.all), prefix14);
                HarmonyMethod postfix16 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("CharacterAppearance_ChangeMotion", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(CharacterAppearance).GetMethod("ChangeMotion", AccessTools.all), postfix: postfix16);
                HarmonyMethod postfix17 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("CustomizingCardArtworkLoader_GetSpecificArtworkSprite", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(CustomizingCardArtworkLoader).GetMethod("GetSpecificArtworkSprite", AccessTools.all), postfix: postfix17);
                HarmonyMethod prefix15 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("EmotionEgoXmlInfo_get_CardId", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(EmotionEgoXmlInfo).GetMethod("get_CardId", AccessTools.all), prefix15);
                HarmonyMethod postfix18 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("EmotionPassiveCardUI_SetSprites", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(EmotionPassiveCardUI).GetMethod("SetSprites", AccessTools.all), postfix: postfix18);
                HarmonyMethod postfix19 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("EmotionPassiveCardUI_Init", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(EmotionPassiveCardUI).GetMethod("Init", AccessTools.all), postfix: postfix19);
                HarmonyMethod prefix16 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("ItemXmlDataList_GetCardItem", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(ItemXmlDataList).GetMethod("GetCardItem", AccessTools.all, (System.Reflection.Binder)null, new System.Type[2]
                {
        typeof (LorId),
        typeof (bool)
                }, (ParameterModifier[])null), prefix16);
                HarmonyMethod postfix20 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("LevelUpUI_OnClickTargetUnit", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(LevelUpUI).GetMethod("OnClickTargetUnit", AccessTools.all), postfix: postfix20);
                HarmonyMethod postfix21 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("LocalizedTextLoader_LoadOthers", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(LocalizedTextLoader).GetMethod("LoadOthers", AccessTools.all), postfix: postfix21);
                HarmonyMethod prefix17 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("SpecialCardListModel_ReturnCardToHand", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(SpecialCardListModel).GetMethod("ReturnCardToHand", AccessTools.all), prefix17);
                HarmonyMethod postfix22 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("StageController_ApplyLibrarianCardPhase", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(StageController).GetMethod("ApplyLibrarianCardPhase", AccessTools.all), postfix: postfix22);
                HarmonyMethod postfix23 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("StageController_OnFixedUpdateLate", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(StageController).GetMethod("OnFixedUpdateLate", AccessTools.all), postfix: postfix23);
                HarmonyMethod postfix24 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("StageController_ActivateStartBattleEffectPhase", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(StageController).GetMethod("ActivateStartBattleEffectPhase", AccessTools.all), postfix: postfix24);
                HarmonyMethod postfix25 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("StageController_GameOver", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(StageController).GetMethod("GameOver", AccessTools.all), postfix: postfix25);
                HarmonyMethod prefix18 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("StageController_RoundStartPhase_System", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(StageController).GetMethod("RoundStartPhase_System", AccessTools.all), prefix18);
                HarmonyMethod prefix19 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("StageController_CheckStoryAfterBattle", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(StageController).GetMethod("CheckStoryAfterBattle", AccessTools.all), prefix19);
                HarmonyMethod postfix26 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("StageModel_GetFrontAvailableFloor", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(StageModel).GetMethod("GetFrontAvailableFloor", AccessTools.all), postfix: postfix26);
                HarmonyMethod postfix27 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("StageWaveModel_Init", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(StageWaveModel).GetMethod("Init", AccessTools.all), postfix: postfix27);
                HarmonyMethod postfix28 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("StageWaveModel_GetUnitBattleDataListByFormation", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(StageWaveModel).GetMethod("GetUnitBattleDataListByFormation", AccessTools.all), postfix: postfix28);
                HarmonyMethod prefix20 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("TextDataModel_GetText", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(TextDataModel).GetMethod("GetText", AccessTools.all), prefix20);
                HarmonyMethod postfix29 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("TimeManager_UpdateTime", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(TimeManager).GetMethod("UpdateTime", AccessTools.all), postfix: postfix29);
                HarmonyMethod postfix30 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIBattleSettingEditPanel_Close", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIBattleSettingEditPanel).GetMethod("Close", AccessTools.all), postfix: postfix30);
                HarmonyMethod prefix21 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIBattleSettingEditPanel_SetBUttonState", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIBattleSettingEditPanel).GetMethod("SetBUttonState", AccessTools.all), prefix21);
                HarmonyMethod prefix22 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIBattleSettingPanel_OnClickBackButton", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIBattleSettingPanel).GetMethod("OnClickBackButton", AccessTools.all), prefix22);
                HarmonyMethod prefix23 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIBattleSettingWaveList_SetData", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIBattleSettingWaveList).GetMethod("SetData", AccessTools.all), prefix23);
                HarmonyMethod postfix31 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIColorManager_GetSephirahColor", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIColorManager).GetMethod("GetSephirahColor", AccessTools.all), postfix: postfix31);
                HarmonyMethod postfix32 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIColorManager_GetSephirahGlowColor", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIColorManager).GetMethod("GetSephirahGlowColor", AccessTools.all), postfix: postfix32);
                HarmonyMethod postfix33 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIController_Awake", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UI.UIController).GetMethod("Awake", AccessTools.all), postfix: postfix33);
                HarmonyMethod postfix34 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UICharacterStatInfoPanel_SetData", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UICharacterStatInfoPanel).GetMethod("SetData", AccessTools.all, (System.Reflection.Binder)null, new System.Type[1]
                {
        typeof (UnitDataModel)
                }, (ParameterModifier[])null), postfix: postfix34);
                HarmonyMethod postfix35 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIEmotionPassiveCardInven_SetSprites", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIEmotionPassiveCardInven).GetMethod("SetSprites", AccessTools.all), postfix: postfix35);
                HarmonyMethod postfix36 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIInvitationRightMainPanel_OpenInit", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIInvitationRightMainPanel).GetMethod("OpenInit", AccessTools.all), postfix: postfix36);
                HarmonyMethod postfix37 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UILibrarianEquipInfoSlot_SetData", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UILibrarianEquipInfoSlot).GetMethod("SetData", AccessTools.all), postfix: postfix37);
                HarmonyMethod postfix38 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIManualContentPanel_SetData", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIManualContentPanel).GetMethod("SetData", AccessTools.all), postfix: postfix38);
                HarmonyMethod postfix39 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIManualScreenPage_LoadContent", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIManualScreenPage).GetMethod("LoadContent", AccessTools.all), postfix: postfix39);
                HarmonyMethod postfix40 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIOptionWindow_Open", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIOptionWindow).GetMethod("Open", AccessTools.all), postfix: postfix40);
                HarmonyMethod postfix41 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIPassiveSuccessionPopup_InitReservedData", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIPassiveSuccessionPopup).GetMethod("InitReservedData", AccessTools.all), postfix: postfix41);
                HarmonyMethod prefix24 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIPassiveSuccessionSlot_SetDataModel", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIPassiveSuccessionSlot).GetMethod("SetDataModel", AccessTools.all), prefix24);
                HarmonyMethod prefix25 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIPopupWindowManager_Update", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIPopupWindowManager).GetMethod("Update", AccessTools.all), prefix25);
                HarmonyMethod prefix26 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UISpriteDataManager_GetStoryIcon", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UISpriteDataManager).GetMethod("GetStoryIcon", AccessTools.all), prefix26);
                HarmonyMethod postfix42 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UIBattleSettingLibrarianInfoPanel_SetData", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UIBattleSettingLibrarianInfoPanel).GetMethod("SetData", AccessTools.all), postfix: postfix42);
                HarmonyMethod prefix27 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UnitDataModel_EquipBookForUI", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UnitDataModel).GetMethod("EquipBookForUI", AccessTools.all), prefix27);
                HarmonyMethod prefix28 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("UICharacterListPanel_RefreshBattleUnitDataModel", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(UICharacterListPanel).GetMethod("RefreshBattleUnitDataModel", AccessTools.all), prefix28);
                HarmonyMethod postfix43 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BookModel_GetThumbSprite", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BookModel).GetMethod("GetThumbSprite", AccessTools.all), postfix: postfix43);
                HarmonyMethod postfix44 = new HarmonyMethod(typeof(LogLikeMod).GetMethod("BookModel_GetMaxPassiveCost", AccessTools.all));
                this.Patching(harmony, (MethodBase)typeof(BookModel).GetMethod("GetMaxPassiveCost", AccessTools.all), postfix: postfix44);
                LogLikeMod.LogModAssemblys = new List<Assembly>();
                LogLikeMod.LoadSpineAssets();
                foreach (ModContentInfo logMod in LogLikeMod.GetLogMods())
                    LogLikeMod.ModLoader.OnInitializeMod(logMod.GetAssemPath(), logMod.invInfo.workshopInfo.uniqueId);
                LogLikeMod.ArtWorks = new LogLikeMod.CacheDic<string, Sprite>(new LogLikeMod.CacheDic<string, Sprite>.getdele(LogLikeMod.GetArtWorks));
                LogLikeMod.LogUIObjs = new LogLikeMod.CacheDic<int, GameObject>(new LogLikeMod.CacheDic<int, GameObject>.getdele(LogLikeMod.GetLogUIObj));
                this.LoadStages();
                this.LoadMysteryInfos();
                this.LoadRewardPassiveInfos();
                this.LoadDropValues();
                this.LoadStoryPath();
                LogLikeMod.LoadStageInfos();
                LogLikeMod.LoadEnemyUnitInfos();
                LogLikeMod.LoadEquipPages();
                LogLikeMod.LoadCardDropTables();
                LogLikeMod.LoadDropBooks();
                LogLikeMod.LoadCardInfos();
                LogLikeMod.LoadDecks();
                LogLikeMod.LoadPassives();
                LogLikeMod.LoadTextData(TextDataModel.CurrentLanguage);
                LogLikeMod.spinemotions = new Dictionary<string, Dictionary<ActionDetail, Dictionary<GameObject, SkeletonAnimation>>>();
                FormationXmlRoot formationXmlRoot;
                using (StringReader stringReader = new StringReader(File.ReadAllText(LogLikeMod.path + "/AddData/FormationInfo/FormationInfo.txt")))
                    formationXmlRoot = (FormationXmlRoot)new XmlSerializer(typeof(FormationXmlRoot)).Deserialize((TextReader)stringReader);
                ((List<FormationXmlInfo>)typeof(FormationXmlList).GetField("_list", AccessTools.all).GetValue((object)Singleton<FormationXmlList>.Instance)).AddRange((IEnumerable<FormationXmlInfo>)formationXmlRoot.list);
                LogLikeMod.CheckExceptionModList = new List<string>();
                LogLikeMod.PreLoader preLoader = GlobalGameManager.Instance.gameObject.AddComponent<LogLikeMod.PreLoader>();
                preLoader.StartArtWorkPreLoad();
                preLoader.StartAssetBundlePreload();
                preLoader.StartUpgradeInfoPreload();
                preLoader.StartCreatureTabPreload();
                preLoader.StartSoundPreload();
            }
            catch (Exception ex)
            {
                Debug.Log((object)(ex.Message + Environment.NewLine + ex.StackTrace));
                Singleton<ModContentManager>.Instance.AddErrorLog($"LogLikeMod Init error : {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        public static bool ItemXmlDataList_GetCardItem(
          ItemXmlDataList __instance,
          LorId id,
          bool errNull = false)
        {
            UpgradeMetadata metadata;
            if (LogLikeMod.CheckStage() && UpgradeMetadata.UnpackPid(id.packageId, out metadata))
                Singleton<LogCardUpgradeManager>.Instance.GetUpgradeCard(new LorId(metadata.actualPid, id.id), metadata.index, metadata.count);
            return true;
        }

        public static void UIInvitationRightMainPanel_OpenInit(UIInvitationRightMainPanel __instance)
        {
            if ((UnityEngine.Object)LogLikeMod.LogOpenButton == (UnityEngine.Object)null)
            {
                LogLikeMod.LogOpenButton = ModdingUtils.CreateButton(__instance.transform, "LogLikeModIcon", new Vector2(1f, 1f), new Vector2(-70f, 350f), new Vector2(100f, 100f));
                LogLikeMod.LogOpenButton.gameObject.AddComponent<FrameDummy>();
                Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
                buttonClickedEvent.AddListener((UnityAction)(() =>
                {
                    List<string> ExceptModNames;
                    if (LogLikeMod.CheckExceptionModExist(out ExceptModNames))
                    {
                        string text = TextDataModel.GetText("ui_ExceptionWithLog") + Environment.NewLine;
                        foreach (string str in ExceptModNames)
                            text = $"{text}-{str}{Environment.NewLine}";
                        UIAlarmPopup.instance.SetAlarmText(text);
                    }
                    else
                    {
                        bool flag = true;
                        __instance.SetCustomInvToggle(true);
                        foreach (UIInvitationBookSlot invitationbookSlot in __instance.invitationbookSlots)
                        {
                            if (flag)
                                invitationbookSlot.ApplySlotid(new LorId(LogLikeMod.ModId, -853), true);
                            else
                                invitationbookSlot.SetEmptySlot();
                            flag = false;
                        }
                        __instance.ConfirmSendInvitation();
                    }
                }));
                LogLikeMod.LogOpenButton.onClick = buttonClickedEvent;
            }
            if ((UnityEngine.Object)LogLikeMod.LogContinueButton == (UnityEngine.Object)null)
            {
                LogLikeMod.LogContinueButton = ModdingUtils.CreateButton(__instance.transform, "LogLikeModIcon_Continue", new Vector2(1f, 1f), new Vector2(-70f, 250f), new Vector2(100f, 100f));
                LogLikeMod.LogContinueButton.gameObject.AddComponent<FrameDummy>();
                Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
                buttonClickedEvent.AddListener((UnityAction)(() =>
                {
                    List<string> ExceptModNames;
                    if (LogLikeMod.CheckExceptionModExist(out ExceptModNames))
                    {
                        string text = TextDataModel.GetText("ui_ExceptionWithLog") + Environment.NewLine;
                        foreach (string str in ExceptModNames)
                            text = $"{text}-{str}{Environment.NewLine}";
                        UIAlarmPopup.instance.SetAlarmText(text);
                    }
                    else
                    {
                        bool flag = true;
                        __instance.SetCustomInvToggle(true);
                        foreach (UIInvitationBookSlot invitationbookSlot in __instance.invitationbookSlots)
                        {
                            if (flag)
                                invitationbookSlot.ApplySlotid(new LorId(LogLikeMod.ModId, -855), true);
                            else
                                invitationbookSlot.SetEmptySlot();
                            flag = false;
                        }
                        __instance.ConfirmSendInvitation();
                    }
                }));
                LogLikeMod.LogContinueButton.onClick = buttonClickedEvent;
            }
            LogLikeMod.LogContinueButton.gameObject.SetActive(LoguePlayDataSaver.CheckPlayerData());
        }

        public static void UIEmotionPassiveCardInven_SetSprites(
          UIEmotionPassiveCardInven __instance,
          MentalState state)
        {
            if (!LogLikeMod.CheckStage())
                return;
            LogLikeMod.GetFieldValue<Image>((object)__instance, "_artwork").sprite = LogLikeMod.ArtWorks[__instance.Card.Artwork];
        }

        public static bool TextDataModel_GetText(string id, ref string __result, params object[] args)
        {
            string text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText(id, args);
            if (!(text != string.Empty))
                return true;
            __result = text;
            return false;
        }

        public static bool UISpriteDataManager_GetStoryIcon(
          string story,
          ref UIIconManager.IconSet __result)
        {
            if (!LogLikeMod.CheckStage() || !story.Contains("<LogLike>"))
                return true;
            string key = story.Remove(0, 9);
            Sprite artWork = LogLikeMod.ArtWorks[key];
            if ((UnityEngine.Object)artWork != (UnityEngine.Object)null)
                __result = new UIIconManager.IconSet()
                {
                    icon = artWork,
                    iconGlow = artWork
                };
            return false;
        }

        public static bool UIPopupWindowManager_Update()
        {
            return !LogLikeMod.CheckStage() || UI.UIController.Instance.CurrentUIPhase != UIPhase.BattleSetting;
        }

        public static void UIPassiveSuccessionPopup_InitReservedData()
        {
            if (!LogLikeMod.CheckStage())
                return;
            foreach (BookModel bookModel in LogueBookModels.booklist)
            {
                bookModel.InitReservedDataForPassiveSuccession();
                foreach (PassiveModel passiveModel in bookModel.GetPassiveModelList())
                    passiveModel.InitReservedData();
            }
            foreach (UnitDataModel unitDataModel in LogueBookModels.playerModel)
            {
                BookModel bookItem = unitDataModel.bookItem;
                bookItem.InitReservedDataForPassiveSuccession();
                foreach (PassiveModel passiveModel in bookItem.GetPassiveModelList())
                    passiveModel.InitReservedData();
            }
        }

        public static bool UIPassiveSuccessionSlot_SetDataModel(
          UIPassiveSuccessionSlot __instance,
          PassiveModel passive,
          ref bool __result)
        {
            if (passive == null)
            {
                __result = false;
                return false;
            }
            if (passive.reservedData == null)
            {
                __result = false;
                return false;
            }
            if (!(passive.reservedData.currentpassive.id == new LorId(LogLikeMod.ModId, 1)))
                return true;
            __result = false;
            return false;
        }

        public static void UIBattleSettingLibrarianInfoPanel_SetData(
          UIBattleSettingLibrarianInfoPanel __instance,
          UnitDataModel data)
        {
            __instance.PassiveListSelectable.SubmitEvent.RemoveAllListeners();
            if (!LogLikeMod.CheckStage() || !LogueBookModels.playerModel.Contains(data))
                return;
            __instance.PassiveListSelectable.SubmitEvent.AddListener((UnityAction<BaseEventData>)(e => UIPassiveSuccessionPopup.Instance.SetData(data, (UIPassiveSuccessionPopup.ApplyEvent)(() =>
            {
                __instance.passiveSlotsPanel.SetStatsDataInEquipBook(data.bookItem);
                (UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel).EditPanel.EquipPagePanel.ChangeEquipBook((UnitDataModel)null);
                UIControlManager.Instance.SelectSelectableForcely(__instance.PassiveListSelectable);
                LoguePlayDataSaver.SavePlayData_Menu();
            }))));
        }

        public static void StageModel_GetFrontAvailableFloor(
          StageModel __instance,
          ref StageLibraryFloorModel __result)
        {
            if (!LogLikeMod.CheckStage() || __instance.floorList.Find((Predicate<StageLibraryFloorModel>)(x => x.IsUnavailable())) == null)
                return;
            __result = (StageLibraryFloorModel)null;
        }

        public static void UIColorManager_GetSephirahColor(SephirahType sephirah, ref Color __result)
        {
            //if (!LogLikeMod.CheckStage() || sephirah != SephirahType.None)
            // ;
        }

        public static void UIColorManager_GetSephirahGlowColor(SephirahType sephirah, ref Color __result)
        {
            //if (!LogLikeMod.CheckStage() || sephirah != SephirahType.None)
            //  ;
        }

        public static void UICharacterStatInfoPanel_SetData(
          UICharacterStatInfoPanel __instance,
          UnitDataModel data)
        {
            if (LogueBookModels.playerModel == null || !LogueBookModels.playerModel.Contains(data) || LogueBookModels.playersstatadders[data].Count <= 0)
                return;
            BookXmlInfo bookXmlInfo = LogueBookModels.CurPlayerEquipInfo(data);
            int hp = bookXmlInfo.EquipEffect.Hp;
            hp.Log("chp : " + hp.ToString());
            hp.Log("mhp : " + data.MaxHp.ToString());
            int num1;
            if (data.MaxHp > hp)
            {
                TextMeshProUGUI hpText = __instance.hpText;
                string str1 = hp.ToString();
                num1 = data.MaxHp - hp;
                string str2 = num1.ToString();
                string str3 = $"{str1} <color=#22FFE4>+ {str2}</color>";
                hpText.text = str3;
            }
            else if (data.MaxHp < hp)
            {
                TextMeshProUGUI hpText = __instance.hpText;
                string str4 = hp.ToString();
                num1 = hp - data.MaxHp;
                string str5 = num1.ToString();
                string str6 = $"{str4} <color=red>- {str5}</color>";
                hpText.text = str6;
            }
            int num2 = bookXmlInfo.EquipEffect.Break;
            if (data.Break > num2)
            {
                TextMeshProUGUI breakText = __instance.breakText;
                string str7 = num2.ToString();
                num1 = data.Break - num2;
                string str8 = num1.ToString();
                string str9 = $"{str7} <color=#22FFE4>+ {str8}</color>";
                breakText.text = str9;
            }
            else if (data.Break < num2)
            {
                TextMeshProUGUI breakText = __instance.breakText;
                string str10 = num2.ToString();
                num1 = num2 - data.Break;
                string str11 = num1.ToString();
                string str12 = $"{str10} <color=red>- {str11}</color>";
                breakText.text = str12;
            }
            AtkResist sresist1 = data.bookItem.equipeffect.SResist;
            AtkResist sresist2 = bookXmlInfo.EquipEffect.SResist;
            if (sresist1 > sresist2)
                __instance.resistSlash.text = $"<color=#22FFE4>{data.bookItem.GetResistHP_Text(BehaviourDetail.Slash)}</color>";
            else if (sresist1 < sresist2)
                __instance.resistSlash.text = $"<color=red>{data.bookItem.GetResistHP_Text(BehaviourDetail.Slash)}</color>";
            AtkResist presist1 = data.bookItem.equipeffect.PResist;
            AtkResist presist2 = bookXmlInfo.EquipEffect.PResist;
            if (presist1 > presist2)
                __instance.resistSlash.text = $"<color=#22FFE4>{data.bookItem.GetResistHP_Text(BehaviourDetail.Penetrate)}</color>";
            else if (presist1 < presist2)
                __instance.resistSlash.text = $"<color=red>{data.bookItem.GetResistHP_Text(BehaviourDetail.Penetrate)}</color>";
            AtkResist hresist1 = data.bookItem.equipeffect.HResist;
            AtkResist hresist2 = bookXmlInfo.EquipEffect.HResist;
            if (hresist1 > hresist2)
                __instance.resistSlash.text = $"<color=#22FFE4>{data.bookItem.GetResistHP_Text(BehaviourDetail.Hit)}</color>";
            else if (hresist1 < hresist2)
                __instance.resistSlash.text = $"<color=red>{data.bookItem.GetResistHP_Text(BehaviourDetail.Hit)}</color>";
            AtkResist sbResist1 = data.bookItem.equipeffect.SBResist;
            AtkResist sbResist2 = bookXmlInfo.EquipEffect.SBResist;
            if (sbResist1 > sbResist2)
                __instance.resistSlash.text = $"<color=#22FFE4>{data.bookItem.GetResistBP_Text(BehaviourDetail.Slash)}</color>";
            else if (sbResist1 < sbResist2)
                __instance.resistSlash.text = $"<color=red>{data.bookItem.GetResistBP_Text(BehaviourDetail.Slash)}</color>";
            AtkResist pbResist1 = data.bookItem.equipeffect.PBResist;
            AtkResist pbResist2 = bookXmlInfo.EquipEffect.PBResist;
            if (pbResist1 > pbResist2)
                __instance.resistSlash.text = $"<color=#22FFE4>{data.bookItem.GetResistBP_Text(BehaviourDetail.Penetrate)}</color>";
            else if (pbResist1 < pbResist2)
                __instance.resistSlash.text = $"<color=red>{data.bookItem.GetResistBP_Text(BehaviourDetail.Penetrate)}</color>";
            AtkResist hbResist1 = data.bookItem.equipeffect.HBResist;
            AtkResist hbResist2 = bookXmlInfo.EquipEffect.HBResist;
            if (hbResist1 > hbResist2)
                __instance.resistSlash.text = $"<color=#22FFE4>{data.bookItem.GetResistBP_Text(BehaviourDetail.Hit)}</color>";
            else if (hbResist1 < hbResist2)
                __instance.resistSlash.text = $"<color=red>{data.bookItem.GetResistBP_Text(BehaviourDetail.Hit)}</color>";
        }

        public static void BookModel_GetMaxPassiveCost(ref int __result)
        {
            if (!LogLikeMod.CheckStage())
                return;
            int num = 6 + Singleton<GlobalLogueEffectManager>.Instance.ChangeSuccCostValue();
            if (num < 0)
                num = 0;
            __result = num;
        }

        public static void BookModel_GetThumbSprite(BookModel __instance, ref Sprite __result)
        {
            if (!LogLikeMod.CheckStage() || __instance.ClassInfo.CharacterSkin == null)
                return;
            if (!__instance.ClassInfo.CharacterSkin.Any<string>())
                return;
            try
            {
                if (__instance.ClassInfo.skinType == "Lor")
                {
                    BookXmlInfo bookXmlInfo = Singleton<BookXmlList>.Instance.GetList().Find((Predicate<BookXmlInfo>)(x => x.CharacterSkin[0] == __instance.ClassInfo.CharacterSkin[0] && !x.id.IsWorkshop()));
                    __result = UnityEngine.Resources.Load<Sprite>("Sprites/Books/Thumb/" + bookXmlInfo.id.id.ToString());
                }
                else
                {
                    if (!(__instance.ClassInfo.skinType == "CUSTOM") || !__instance.ClassInfo.CharacterSkin[0].Contains("<LogLike>"))
                        return;
                    string key = __instance.ClassInfo.CharacterSkin[0].Remove(0, 9);
                    if (!LogLikeMod.ArtWorks.ContainsKey(key))
                        return;
                    __result = LogLikeMod.ArtWorks[key];
                }
            }
            catch (Exception ex)
            {
                Debug.Log((object)("Failed to load thumbnail: " + (object)ex));
                __result = UnityEngine.Resources.Load<Sprite>("Sprites/Books/Thumb/1");
            }
        }

        public static bool UICharacterListPanel_RefreshBattleUnitDataModel(
          UICharacterListPanel __instance,
          UnitDataModel data)
        {
            if (!LogLikeMod.CheckStage())
                return true;
            __instance.Log("Refrash Character start");
            UnitBattleDataModel battledata = LogueBookModels.playerBattleModel.Find((Predicate<UnitBattleDataModel>)(x => x.unitData == data));
            if (battledata != null)
            {
                UICharacterSlot uiCharacterSlot = LogLikeMod.GetFieldValue<UICharacterList>((object)__instance, "CharacterList").slotList.Find((Predicate<UICharacterSlot>)(x => x.unitBattleData == battledata));
                if ((UnityEngine.Object)uiCharacterSlot != (UnityEngine.Object)null && uiCharacterSlot.unitBattleData != null)
                {
                    uiCharacterSlot.ReloadHpBattleSettingSlot();
                    __instance.Log("Refrash Character success");
                }
            }
            return false;
        }

        public static bool UnitDataModel_EquipBookForUI(
          UnitDataModel __instance,
          BookModel newBook,
          ref bool __result,
          bool isEnemySetting = false,
          bool force = false)
        {
            if (!LogLikeMod.CheckStage() || isEnemySetting)
                return true;
            int num = LogueBookModels.playerBattleModel.IndexOf(LogueBookModels.playerBattleModel.Find((Predicate<UnitBattleDataModel>)(x => x.unitData == __instance)));
            if (newBook != null)
            {
                __instance.Log("newBook not null");
                LogueBookModels.EquipNewPage(LogueBookModels.playerBattleModel.Find((Predicate<UnitBattleDataModel>)(x => x.unitData == __instance)), newBook.ClassInfo);
                LogueBookModels.RemoveEquip(__instance);
                newBook.SetOwner(__instance);
                __result = true;
                return false;
            }
            __instance.Log("newBook null");
            LogueBookModels.EquipNewPage(LogueBookModels.playerBattleModel.Find((Predicate<UnitBattleDataModel>)(x => x.unitData == __instance)), LogueBookModels.BaseXmlInfo);
            if (num != 0)
            {
                __instance.bookItem.ClassInfo.CharacterSkin[0] = "KetherLibrarian";
                typeof(BookModel).GetField("_selectedOriginalSkin", AccessTools.all).SetValue((object)__instance.bookItem, (object)__instance.bookItem.ClassInfo.CharacterSkin[0]);
                typeof(BookModel).GetField("_characterSkin", AccessTools.all).SetValue((object)__instance.bookItem, (object)__instance.bookItem.ClassInfo.CharacterSkin[0]);
            }
            LogueBookModels.RemoveEquip(__instance);
            __result = true;
            return false;
        }

        public static void BattleDiceCardUI_GetClickableState(
          BattleDiceCardUI __instance,
          ref BattleDiceCardUI.ClickableState __result)
        {
            BattleUnitModel owner = __instance.CardModel.owner;
            if (owner == null || PassiveAbility_ShopPassiveMook9.HasPassive(owner) == null || __result != BattleDiceCardUI.ClickableState.NotEnoughCost)
                return;
            __result = BattleDiceCardUI.ClickableState.CanClick;
        }

        public static void BattleDiceCardModel_GetCost(BattleDiceCardModel __instance, ref int __result)
        {
        }

        public static bool SpecialCardListModel_ReturnCardToHand(
          SpecialCardListModel __instance,
          BattleUnitModel unit,
          BattleDiceCardModel appliedCard)
        {
            List<BattleDiceCardModel> fieldValue1 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInUse", (object)__instance);
            List<BattleDiceCardModel> fieldValue2 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInReserved", (object)__instance);
            List<BattleDiceCardModel> fieldValue3 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInHand", (object)__instance);
            PassiveAbility_ShopPassiveMook9 shopPassiveMook9 = PassiveAbility_ShopPassiveMook9.HasPassive(unit);
            if (shopPassiveMook9 == null || !shopPassiveMook9.cards.ContainsKey(appliedCard))
                return true;
            unit.cardSlotDetail.ReserveCost(-(appliedCard.GetCost() - shopPassiveMook9.cards[appliedCard]));
            fieldValue1.Remove(appliedCard);
            fieldValue2.Remove(appliedCard);
            fieldValue3.Add(appliedCard);
            shopPassiveMook9.cards.Remove(appliedCard);
            return false;
        }

        public static bool BattlePersonalEgoCardDetail_ReturnCardToHand(
          BattlePersonalEgoCardDetail __instance,
          BattleDiceCardModel appliedCard)
        {
            BattleUnitModel fieldValue1 = ModdingUtils.GetFieldValue<BattleUnitModel>("_self", (object)__instance);
            List<BattleDiceCardModel> fieldValue2 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInUse", (object)__instance);
            List<BattleDiceCardModel> fieldValue3 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInReserved", (object)__instance);
            List<BattleDiceCardModel> fieldValue4 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInHand", (object)__instance);
            PassiveAbility_ShopPassiveMook9 shopPassiveMook9 = PassiveAbility_ShopPassiveMook9.HasPassive(fieldValue1);
            if (shopPassiveMook9 == null || !shopPassiveMook9.cards.ContainsKey(appliedCard))
                return true;
            fieldValue1.cardSlotDetail.ReserveCost(-(appliedCard.GetCost() - shopPassiveMook9.cards[appliedCard]));
            fieldValue2.Remove(appliedCard);
            fieldValue3.Remove(appliedCard);
            fieldValue4.Add(appliedCard);
            shopPassiveMook9.cards.Remove(appliedCard);
            return false;
        }

        public static bool BattleAllyCardDetail_ReturnCardToHand(
          BattleAllyCardDetail __instance,
          BattleDiceCardModel appliedCard)
        {
            BattleUnitModel fieldValue1 = ModdingUtils.GetFieldValue<BattleUnitModel>("_self", (object)__instance);
            List<BattleDiceCardModel> fieldValue2 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInUse", (object)__instance);
            List<BattleDiceCardModel> fieldValue3 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInReserved", (object)__instance);
            List<BattleDiceCardModel> fieldValue4 = ModdingUtils.GetFieldValue<List<BattleDiceCardModel>>("_cardInHand", (object)__instance);
            PassiveAbility_ShopPassiveMook9 shopPassiveMook9 = PassiveAbility_ShopPassiveMook9.HasPassive(fieldValue1);
            if (shopPassiveMook9 == null || !shopPassiveMook9.cards.ContainsKey(appliedCard))
                return true;
            fieldValue1.cardSlotDetail.ReserveCost(-(appliedCard.GetCost() - shopPassiveMook9.cards[appliedCard]));
            fieldValue2.Remove(appliedCard);
            fieldValue3.Remove(appliedCard);
            fieldValue4.Add(appliedCard);
            shopPassiveMook9.cards.Remove(appliedCard);
            return false;
        }

        public static bool BattlePlayingCardSlotDetail_OnApplyCard(
          BattlePlayingCardSlotDetail __instance,
          BattleDiceCardModel card,
          ref bool __result)
        {
            PassiveAbility_ShopPassiveMook9 shopPassiveMook9 = PassiveAbility_ShopPassiveMook9.HasPassive(ModdingUtils.GetFieldValue<BattleUnitModel>("_self", (object)__instance));
            if (shopPassiveMook9 == null)
                return true;
            int cost = card.GetCost();
            if (__instance.ReservedPlayPoint + cost > __instance.GetMaxPlayPoint())
            {
                int num1 = __instance.GetMaxPlayPoint() - __instance.ReservedPlayPoint;
                int num2 = cost - num1;
                shopPassiveMook9.cards[card] = num2;
                __result = __instance.ReserveCost(num1);
                return false;
            }
            card.costSpended = false;
            __result = __instance.ReserveCost(cost);
            return false;
        }

        public static void BattleUnitBuf_Destroy(BattleUnitBuf __instance)
        {
            if (!LogLikeMod.CheckStage())
                return;
            BattleUnitModel fieldValue = ModdingUtils.GetFieldValue<BattleUnitModel>("_owner", (object)__instance);
            if (!(__instance is BattleUnitBuf_burn) || fieldValue.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_ShopPassiveStigma5)) == null)
                return;
            (fieldValue.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_ShopPassiveStigma5)) as PassiveAbility_ShopPassiveStigma5).Recovering();
        }

        public static bool BattleUnitBuf_burn_OnRoundEnd(BattleUnitBuf_burn __instance)
        {
            if (!LogLikeMod.CheckStage())
                return true;
            BattleUnitModel fieldValue = ModdingUtils.GetFieldValue<BattleUnitModel>("_owner", (object)__instance);
            if (fieldValue.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_ShopPassiveStigma5)) != null)
            {
                PassiveAbility_ShopPassiveStigma5 shopPassiveStigma5 = fieldValue.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_ShopPassiveStigma5)) as PassiveAbility_ShopPassiveStigma5;
                if (shopPassiveStigma5.stack < __instance.stack)
                    shopPassiveStigma5.stack = __instance.stack;
            }
            if (Singleton<GlobalLogueEffectManager>.Instance.GetEffectList().Find((Predicate<GlobalLogueEffectBase>)(x => x is PickUpModel_ShopGoodStigma1.Stigma1Effect)) == null)
                return true;
            if (!fieldValue.IsImmune(__instance.bufType))
            {
                int v = __instance.stack - fieldValue.bufListDetail.GetActivatedBufList().FindAll((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_burnDown)).Count * 10;
                if (v < 0)
                    v = 0;
                fieldValue.TakeDamage(v, DamageType.Buf, keyword: __instance.bufType);
                __instance.GetType().GetMethod("PrintEffect", AccessTools.all).Invoke((object)__instance, (object[])null);
                if (fieldValue.bufListDetail.GetActivatedBuf(KeywordBuf.BurnBreak) != null)
                    fieldValue.TakeBreakDamage(v / 2, DamageType.Buf, keyword: __instance.bufType);
                if (fieldValue.faction == Faction.Enemy && fieldValue.IsDead())
                    Singleton<StageController>.Instance.GetStageModel().AddBurnKillCount();
            }
            __instance.stack = __instance.stack * 3 / 4;
            if (__instance.stack <= 0)
                __instance.Destroy();
            return false;
        }

        public static void BattleUnitPassiveDetail_OnKill(
          BattleUnitPassiveDetail __instance,
          BattleUnitModel target)
        {
            Singleton<GlobalLogueEffectManager>.Instance.OnKillUnit(ModdingUtils.GetFieldValue<BattleUnitModel>("_self", (object)__instance), target);
        }

        public static void BattleUnitPassiveDetail_OnDie(BattleUnitPassiveDetail __instance)
        {
            Singleton<GlobalLogueEffectManager>.Instance.OnDieUnit(ModdingUtils.GetFieldValue<BattleUnitModel>("_self", (object)__instance));
        }

        public static void BattleUnitBufListDetail_ChangeDiceResult(
          BattleUnitBufListDetail __instance,
          BattleDiceBehavior behavior,
          ref int diceResult)
        {
            if (!LogLikeMod.CheckStage())
                return;
            Singleton<GlobalLogueEffectManager>.Instance.ChangeDiceResult(behavior, ref diceResult);
        }

        public static void BattleUnitBufListDetail_CheckGift(
          BattleUnitBufListDetail __instance,
          KeywordBuf bufType,
          int stack,
          BattleUnitModel actor)
        {
            if (bufType != KeywordBuf.Bleeding || actor.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_ShopPassiveUnion2)) == null)
                return;
            ModdingUtils.GetFieldValue<BattleUnitModel>("_self", (object)__instance).TakeDamage(stack);
        }

        public static void BattleUnitPassiveDetail_DmgFactor(
          BattleUnitPassiveDetail __instance,
          ref float __result,
          int dmg,
          DamageType type = DamageType.ETC,
          KeywordBuf keyword = KeywordBuf.None)
        {
            __result = Singleton<GlobalLogueEffectManager>.Instance.DmgFactor(ModdingUtils.GetFieldValue<BattleUnitModel>("_self", (object)__instance), dmg, type, keyword);
        }

        public static bool BattleUnitModel_BeforeRollDice(
          BattleUnitModel __instance,
          BattleDiceBehavior behavior)
        {
            Singleton<GlobalLogueEffectManager>.Instance.BeforeRollDice(behavior);
            return true;
        }

        public static bool BattlePlayingCardDataInUnitModel_OnUseCard(
          BattlePlayingCardDataInUnitModel __instance)
        {
            Singleton<GlobalLogueEffectManager>.Instance.OnUseCard(__instance);
            return true;
        }

        public static bool BookPassiveInfo_get_name(BookPassiveInfo __instance, ref string __result)
        {
            string name = Singleton<PassiveDescXmlList>.Instance.GetName(__instance.passive.id);
            if (!(name != string.Empty))
                return true;
            __result = name;
            return false;
        }

        public static bool BookPassiveInfo_get_desc(BookPassiveInfo __instance, ref string __result)
        {
            string desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(__instance.passive.id);
            if (!(desc != string.Empty))
                return true;
            __result = desc;
            return false;
        }

        public static void LocalizedTextLoader_LoadOthers(string language)
        {
            LogLikeMod.LoadTextData(language);
        }

        public static bool BattleEmotionRewardInfoUI_SetData(BattleEmotionRewardInfoUI __instance)
        {
            List<BattleEmotionRewardSlotUI> fieldValue = LogLikeMod.GetFieldValue<List<BattleEmotionRewardSlotUI>>((object)__instance, "slots");
            if (fieldValue.Count < 10)
            {
                while (fieldValue.Count < 10)
                {
                    BattleEmotionRewardSlotUI emotionRewardSlotUi = LogLikeMod.LogBattleEmotionRewardSlotUI.BattleEmotionRewardSlotUI_Copying(fieldValue[0]);
                    fieldValue.Add(emotionRewardSlotUi);
                }
            }
            return true;
        }

        public static bool BattleEmotionCoinUI_Init(BattleEmotionCoinUI __instance)
        {
            if (__instance.enermy.Length < 10)
            {
                List<Vector2> vector2List = new List<Vector2>()
      {
        new Vector2(121f, 410f),
        new Vector2(121f, 480f),
        new Vector2(121f, 550f),
        new Vector2(121f, 620f),
        new Vector2(121f, 690f)
      };
                int index = 0;
                while (__instance.enermy.Length < 10)
                {
                    BattleEmotionCoinUI.BattleEmotionCoinData battleEmotionCoinData = __instance.enermy[0];
                    battleEmotionCoinData.target = UnityEngine.Object.Instantiate<RectTransform>(battleEmotionCoinData.target, battleEmotionCoinData.target.parent);
                    battleEmotionCoinData.target.localPosition = (Vector3)vector2List[index];
                    __instance.enermy = __instance.enermy.AddToArray<BattleEmotionCoinUI.BattleEmotionCoinData>(battleEmotionCoinData);
                    ++index;
                }
            }
            return true;
        }

        public static void StageWaveModel_GetUnitBattleDataListByFormation(
          StageWaveModel __instance,
          ref List<UnitBattleDataModel> __result)
        {
            List<UnitBattleDataModel> unitBattleDataModelList = new List<UnitBattleDataModel>();
            for (int i = 0; i < 10; ++i)
            {
                int formationIndex = __instance.GetFormationIndex(i);
                if (formationIndex < __instance.UnitList.Count)
                    unitBattleDataModelList.Add(__instance.UnitList[formationIndex]);
            }
            __result = unitBattleDataModelList;
        }

        public static void StageWaveModel_Init(StageWaveModel __instance)
        {
            List<int> fieldValue = ModdingUtils.GetFieldValue<List<int>>("_formationIndex", (object)__instance);
            fieldValue.Add(5);
            fieldValue.Add(6);
            fieldValue.Add(7);
            fieldValue.Add(8);
            fieldValue.Add(9);
        }

        public static bool BattleUnitInfoManagerUI_Initialize(BattleUnitInfoManagerUI __instance)
        {
            if (__instance.enemyProfileArray.Length < 10)
            {
                List<Vector2> vector2List = new List<Vector2>()
      {
        new Vector2(121f, 410f),
        new Vector2(121f, 480f),
        new Vector2(121f, 550f),
        new Vector2(121f, 620f),
        new Vector2(121f, 690f)
      };
                BattleCharacterProfileUI enemyProfile = __instance.enemyProfileArray[0];
                int index = 0;
                while (__instance.enemyProfileArray.Length < 10)
                {
                    __instance.enemyProfileArray = __instance.enemyProfileArray.AddToArray<BattleCharacterProfileUI>(LogLikeMod.LogBattleCharacterProfileUI.BattleUnitInfoManagerUI_Copying(enemyProfile));
                    __instance.enemyProfileArray[__instance.enemyProfileArray.Length - 1].gameObject.transform.localPosition = (Vector3)vector2List[index];
                    __instance.enemyProfileArray[__instance.enemyProfileArray.Length - 1].gameObject.SetActive(false);
                    ++index;
                }
            }
            return true;
        }

        public static void UIManualScreenPage_LoadContent(UIManualScreenPage __instance)
        {
            TutorialManager.TutoInfo logTuto = Singleton<TutorialManager>.Instance.FindLogTuto(__instance);
            if (logTuto == null)
                return;
            __instance.img_screenShot.sprite = LogLikeMod.ArtWorks[logTuto.ArtWork];
        }

        public static void UIManualContentPanel_SetData(UIManualContentPanel __instance)
        {
            if (Singleton<TutorialManager>.Instance.Inited)
                return;
            Singleton<TutorialManager>.Instance.Init(__instance);
        }

        public static bool BattleEmotionInfo_CenterBtn_OnPointerUp()
        {
            return !LogLikeMod.CheckStage() || Singleton<MysteryManager>.Instance.curMystery == null && (Singleton<MysteryManager>.Instance.interruptMysterys == null || Singleton<MysteryManager>.Instance.interruptMysterys.Count <= 0) && Singleton<ShopManager>.Instance.curshop == null && !LogLikeMod.PauseBool;
        }

        public static void TimeManager_UpdateTime()
        {
        }

        public static void UIController_CallUIPhase(UIController __instance, UIPhase phase)
        {
            if (phase != UIPhase.BattleSetting || MysteryBase.curinfo == null)
                return;
            MysteryBase.LoadGetAbnomalityPanel(MysteryBase.curinfo.abnormal, MysteryBase.curinfo.level);
            MysteryBase.curinfo = (MysteryBase.MysteryAbnormalInfo)null;
        }

        public static void UIController_Awake()
        {
            if (!((UnityEngine.Object)LogLikeMod.UILogCardSlot.Original == (UnityEngine.Object)null))
                return;
            LogLikeMod.UILogCardSlot.Original = LogLikeMod.UILogCardSlot.SlotCopying();
        }

        public static void InitUIBattleSettingWaveSlot(
          UIBattleSettingWaveSlot slot,
          UIBattleSettingWaveList list)
        {
            FieldInfo field1 = slot.GetType().GetField("panel", AccessTools.all);
            FieldInfo field2 = slot.GetType().GetField("rect", AccessTools.all);
            FieldInfo field3 = slot.GetType().GetField("img_circle", AccessTools.all);
            FieldInfo field4 = slot.GetType().GetField("img_circleglow", AccessTools.all);
            FieldInfo field5 = slot.GetType().GetField("img_Icon", AccessTools.all);
            FieldInfo field6 = slot.GetType().GetField("img_IconGlow", AccessTools.all);
            FieldInfo field7 = slot.GetType().GetField("hsv_Icon", AccessTools.all);
            FieldInfo field8 = slot.GetType().GetField("hsv_IconGlow", AccessTools.all);
            FieldInfo field9 = slot.GetType().GetField("hsv_Circle", AccessTools.all);
            FieldInfo field10 = slot.GetType().GetField("hsv_CircleGlow", AccessTools.all);
            FieldInfo field11 = slot.GetType().GetField("txt_Alarm", AccessTools.all);
            FieldInfo field12 = slot.GetType().GetField("materialsetter_txtAlarm", AccessTools.all);
            FieldInfo field13 = slot.GetType().GetField("arrow", AccessTools.all);
            FieldInfo field14 = slot.GetType().GetField("defeatColor", AccessTools.all);
            FieldInfo field15 = slot.GetType().GetField("anim", AccessTools.all);
            FieldInfo field16 = slot.GetType().GetField("cg", AccessTools.all);
            field1.SetValue((object)slot, (object)list);
            RectTransform transform = slot.transform as RectTransform;
            field2.SetValue((object)slot, (object)transform);
            field3.SetValue((object)slot, (object)slot.gameObject.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Image>());
            field4.SetValue((object)slot, (object)slot.gameObject.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>());
            field5.SetValue((object)slot, (object)slot.gameObject.transform.GetChild(1).GetChild(3).gameObject.GetComponent<Image>());
            field6.SetValue((object)slot, (object)slot.gameObject.transform.GetChild(1).GetChild(2).gameObject.GetComponent<Image>());
            field7.SetValue((object)slot, (object)slot.gameObject.transform.GetChild(1).GetChild(3).gameObject.GetComponent<_2dxFX_HSV>());
            field8.SetValue((object)slot, (object)slot.gameObject.transform.GetChild(1).GetChild(2).gameObject.GetComponent<_2dxFX_HSV>());
            field9.SetValue((object)slot, (object)slot.gameObject.transform.GetChild(1).GetChild(1).gameObject.GetComponent<_2dxFX_HSV>());
            field10.SetValue((object)slot, (object)slot.gameObject.transform.GetChild(1).GetChild(0).gameObject.GetComponent<_2dxFX_HSV>());
            field11.SetValue((object)slot, (object)slot.gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>());
            field12.SetValue((object)slot, (object)slot.gameObject.transform.GetChild(2).gameObject.GetComponent<TextMeshProMaterialSetter>());
            field13.SetValue((object)slot, (object)slot.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>());
            Color color = new Color(0.454902f, 0.1098039f, 0.0f, 1f);
            field14.SetValue((object)slot, (object)color);
            field15.SetValue((object)slot, (object)slot.gameObject.GetComponent<Animator>());
            field16.SetValue((object)slot, (object)slot.gameObject.GetComponent<CanvasGroup>());
            slot.transform.localPosition = (Vector3)new Vector2(120f, 0.0f);
            slot.gameObject.SetActive(false);
        }

        public static void InitUIBattleSettingWaveSlots(
          List<UIBattleSettingWaveSlot> slots,
          UIBattleSettingWaveList __instance)
        {
            float num = 5f / (float)slots.Count;
            for (int index = 0; index < slots.Count; ++index)
                slots[index].gameObject.transform.localScale = new Vector3(1f, 1f);
        }

        public static bool UIBattleSettingWaveList_SetData(
          UIBattleSettingWaveList __instance,
          StageModel stage)
        {
            try
            {
                if ((UnityEngine.Object)__instance.gameObject.GetComponent<ScrollRect>() == (UnityEngine.Object)null)
                {
                    List<Transform> transformList = new List<Transform>();
                    Texture2D texture2D = new Texture2D(2, 2);
                    texture2D.LoadImage(File.ReadAllBytes(Application.dataPath + "/Managed/Image/Mask.png"));
                    Sprite sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
                    Image image = __instance.gameObject.AddComponent<Image>();
                    image.sprite = sprite;
                    image.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                    (__instance.transform as RectTransform).sizeDelta = new Vector2(400f, (float)((double)stage.waveList.Count * 250.0 / 3.0));
                    ScrollRect scrollRect = __instance.gameObject.AddComponent<ScrollRect>();
                    scrollRect.scrollSensitivity = 15f;
                    scrollRect.content = __instance.transform as RectTransform;
                    scrollRect.horizontal = false;
                    scrollRect.movementType = ScrollRect.MovementType.Elastic;
                    scrollRect.elasticity = 0.1f;
                }
                if (stage.waveList.Count > __instance.waveSlots.Count)
                {
                    int num = stage.waveList.Count - __instance.waveSlots.Count;
                    for (int index = 0; index < num; ++index)
                    {
                        UIBattleSettingWaveSlot slot = UnityEngine.Object.Instantiate<UIBattleSettingWaveSlot>(__instance.waveSlots[0], __instance.waveSlots[0].transform.parent);
                        LogLikeMod.InitUIBattleSettingWaveSlot(slot, __instance);
                        List<UIBattleSettingWaveSlot> battleSettingWaveSlotList = new List<UIBattleSettingWaveSlot>();
                        battleSettingWaveSlotList.Add(slot);
                        battleSettingWaveSlotList.AddRange((IEnumerable<UIBattleSettingWaveSlot>)__instance.waveSlots);
                        __instance.waveSlots = battleSettingWaveSlotList;
                    }
                }
                if (stage.waveList.Count < __instance.waveSlots.Count)
                {
                    int num = __instance.waveSlots.Count - stage.waveList.Count;
                    for (int index = 0; index < num && __instance.waveSlots.Count != 5; ++index)
                    {
                        UIBattleSettingWaveSlot waveSlot = __instance.waveSlots[__instance.waveSlots.Count - 1];
                        __instance.waveSlots.Remove(waveSlot);
                        UnityEngine.Object.DestroyImmediate((UnityEngine.Object)waveSlot);
                    }
                }
                LogLikeMod.InitUIBattleSettingWaveSlots(__instance.waveSlots, __instance);
                foreach (Component waveSlot in __instance.waveSlots)
                    waveSlot.gameObject.SetActive(false);
                for (int index = 0; index < stage.waveList.Count; ++index)
                {
                    UIBattleSettingWaveSlot waveSlot = __instance.waveSlots[index];
                    waveSlot.SetData(stage.waveList[index]);
                    waveSlot.gameObject.SetActive(true);
                    if (stage.waveList[index].IsUnavailable())
                        waveSlot.SetDefeat();
                    if (index == stage.waveList.Count - 1)
                        waveSlot.ActivateArrow(false);
                }
                int index1 = Singleton<StageController>.Instance.CurrentWave - 1;
                if (index1 < 0 || __instance.waveSlots.Count <= index1)
                    Debug.LogError((object)"Index Error");
                else
                    __instance.waveSlots[index1].SetHighlighted();
            }
            catch
            {
            }
          (__instance.transform as RectTransform).sizeDelta = new Vector2(400f, (float)((double)stage.waveList.Count * 250.0 / 3.0));
            return false;
        }

        public static bool CharacterAppearance_ChangeMotion_prefix(ActionDetail detail)
        {
            LogLikeMod.LastDetail = detail;
            return true;
        }

        public static void CharacterAppearance_ChangeMotion(
          CharacterAppearance __instance,
          ActionDetail detail)
        {
            if (LogLikeMod.Temp)
            {
                for (int index = 0; index < SingletonBehavior<UICharacterRenderer>.Instance.cameraList.Count; ++index)
                {
                    Camera camera = SingletonBehavior<UICharacterRenderer>.Instance.cameraList[index];
                    camera.cullingMask = -1;
                    if ((UnityEngine.Object)camera.gameObject.GetComponent("CameraRender") == (UnityEngine.Object)null)
                        camera.gameObject.AddComponent<CameraRender>().index = index;
                }
                LogLikeMod.Temp = false;
            }
            WorkshopSkinDataSetter setter = __instance.GetComponent<WorkshopSkinDataSetter>();
            if ((UnityEngine.Object)setter == (UnityEngine.Object)null)
                return;
            WorkshopSkinData workshopSkinData = Singleton<CustomizingBookSkinLoader>.Instance.GetWorkshopBookSkinData(LogLikeMod.ModId).Find((Predicate<WorkshopSkinData>)(x => x.dic == setter.dic));
            if (workshopSkinData == null)
                return;
            string dataName = workshopSkinData.dataName;
            if (dataName == string.Empty)
                return;
            SpineStandingData spineStandingData = (SpineStandingData)null;
            if (LogLikeMod.spinedatas.TryGetValue(dataName, out spineStandingData))
            {
                if (spineStandingData.AnimDic.ContainsKey(LogLikeMod.LastDetail))
                    detail = LogLikeMod.LastDetail;
                if (spineStandingData.AnimDic.ContainsKey(detail))
                {
                    if (!LogLikeMod.spinemotions.ContainsKey(dataName))
                        LogLikeMod.spinemotions.Add(dataName, new Dictionary<ActionDetail, Dictionary<GameObject, SkeletonAnimation>>());
                    if (!LogLikeMod.spinemotions[dataName].ContainsKey(detail))
                        LogLikeMod.spinemotions[dataName].Add(detail, new Dictionary<GameObject, SkeletonAnimation>());
                    if (!LogLikeMod.spinemotions[dataName][detail].ContainsKey(__instance.gameObject))
                    {
                        SkeletonAnimation skeletonAnimation = SkeletonRenderer.NewSpineGameObject<SkeletonAnimation>(spineStandingData.asset);
                        __instance.AddChild(skeletonAnimation.gameObject);
                        skeletonAnimation.gameObject.transform.localScale = new Vector3(1f, 1f);
                        LogLikeMod.spinemotions[dataName][detail].Add(__instance.gameObject, skeletonAnimation);
                    }
                    LogLikeMod.spinemotions[dataName][detail][__instance.gameObject].gameObject.SetActive(true);
                    LogLikeMod.spinemotions[dataName][detail][__instance.gameObject].gameObject.transform.localPosition = new Vector3(0.0f, 0.0f);
                    LogLikeMod.spinemotions[dataName][detail][__instance.gameObject].gameObject.transform.localScale = spineStandingData.AnimScale[detail];
                    LogLikeMod.spinemotions[dataName][detail][__instance.gameObject].state.SetAnimation(0, spineStandingData.AnimDic[detail], spineStandingData.AnimLoop[detail]);
                    LogLikeMod.spinemotions[dataName][detail][__instance.gameObject].state.TimeScale = spineStandingData.AnimSpeed[detail];
                    if (UIPanel.Controller.GetUIPanel(UIPanelType.CharacterList).IsActivated || UIPanel.Controller.GetUIPanel(UIPanelType.CharacterList_Right).IsActivated)
                        LogLikeMod.spinemotions[dataName][detail][__instance.gameObject].gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 10f);
                    //else if (detail == ActionDetail.Standing)
                    //  ;
                }
            }
            if (!LogLikeMod.spinemotions.ContainsKey(dataName))
                return;
            foreach (KeyValuePair<ActionDetail, Dictionary<GameObject, SkeletonAnimation>> keyValuePair in LogLikeMod.spinemotions[dataName])
            {
                if (keyValuePair.Key != detail && keyValuePair.Value.ContainsKey(__instance.gameObject))
                    keyValuePair.Value[__instance.gameObject].gameObject.SetActive(false);
            }
        }

        public static void UILibrarianEquipInfoSlot_SetData(
          UILibrarianEquipInfoSlot __instance,
          BookPassiveInfo passive)
        {
            if (!(passive.passive.id == new LorId(LogLikeMod.ModId, 1)))
                return;
            __instance.txt_cost.text = "";
        }

        public static void BattleUnitModel_OnDie(BattleUnitModel __instance)
        {
            if (__instance.faction != Faction.Enemy || __instance.UnitData.unitData.ExpDrop <= 0)
                return;
            PassiveAbility_MoneyCheck.AddMoney(__instance.UnitData.unitData.ExpDrop);
        }

        public static void BookPassiveInfo_get_desc_postfix(
          BookPassiveInfo __instance,
          ref string __result)
        {
            if (__instance.passive == null || !(__instance.passive.id == new LorId(LogLikeMod.ModId, 1)))
                return;
            __result = PassiveAbility_MoneyCheck.GetMoney().ToString();
        }

        public static void CustomizingCardArtworkLoader_GetSpecificArtworkSprite(
          ref Sprite __result,
          string id,
          string name)
        {
            if (!LogLikeMod.CheckStage() || !LogLikeMod.ArtWorks.ContainsKey(name))
                return;
            __result = LogLikeMod.ArtWorks[name];
        }

        public static void BattleDiceCardUI_SetCard(
          BattleDiceCardUI __instance,
          BattleDiceCardModel cardModel)
        {
            if (!LogLikeMod.CheckStage() || !LogLikeMod.ArtWorks.ContainsKey(cardModel.GetArtworkSrc()))
                return;
            __instance.img_artwork.sprite = LogLikeMod.ArtWorks[cardModel.GetArtworkSrc()];
        }

        public static void UIOriginCardSlot_SetData(
          UIOriginCardSlot __instance,
          DiceCardItemModel cardmodel)
        {
            if (!LogLikeMod.CheckStage())
                return;
            Image image = (Image)typeof(UIOriginCardSlot).GetField("img_Artwork", AccessTools.all).GetValue((object)__instance);
            bool flag = cardmodel.GetID().packageId == LogLikeMod.ModId;
            if (LogLikeMod.ArtWorks.ContainsKey(cardmodel.GetArtworkSrc()) & flag)
            {
                image.sprite = LogLikeMod.ArtWorks[cardmodel.GetArtworkSrc()];
            }
            else
            {
                if (!LogLikeMod.ModdedArtWorks.ContainsKey((cardmodel.GetID().packageId, cardmodel.GetArtworkSrc())) || flag)
                    return;
                image.sprite = LogLikeMod.ModdedArtWorks[(cardmodel.GetID().packageId, cardmodel.GetArtworkSrc())];
            }
        }

        public static void EmotionPassiveCardUI_Init(EmotionPassiveCardUI __instance)
        {
            if (!LogLikeMod.CheckStage())
                return;
            bool fieldValue = LogLikeMod.GetFieldValue<bool>((object)__instance, "_isForceOpen");
            if ((UnityEngine.Object)LogLikeMod.ChangeEmotinCardBtn == (UnityEngine.Object)null)
            {
                LogLikeMod.ChangeEmotinCardBtn = ModdingUtils.CreateButton(SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.selectedEmotionCardBg.transform.parent.parent, "AbCardSelection_Skip", new Vector2(1f, 1f), new Vector2(0.0f, -480f));
                LogLikeMod.ChangeEmotinCardBtn.onClick.AddListener((UnityAction)(() => LogLikeMod.ChangeEPCUTransform(__instance)));
                LogLikeMod.ChangeEmotinCardBtn.gameObject.AddComponent<FrameDummy>();
                ModdingUtils.CreateText_TMP(LogLikeMod.ChangeEmotinCardBtn.transform, new Vector2(-30f, 0.0f), 40, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP).text = TextDataModel.GetText("ui_EmotionPositionChange");
            }
            LogLikeMod.ChangeEmotinCardBtn.gameObject.SetActive(fieldValue);
        }

        public static void ChangeEPCUTransform(EmotionPassiveCardUI __instance)
        {
            LevelUpUI uiLevelup = SingletonBehavior<BattleManagerUI>.Instance.ui_levelup;
            if ((double)uiLevelup.selectedEmotionCard.transform.localPosition.x > 0.0)
            {
                uiLevelup.selectedEmotionCard.transform.localPosition = new Vector3(-410f, -510f);
                uiLevelup.selectedEmotionCardBg.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                uiLevelup.selectedEmotionCard.transform.localPosition = new Vector3(410f, -510f);
                uiLevelup.selectedEmotionCardBg.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }

        public static void EmotionPassiveCardUI_SetSprites(EmotionPassiveCardUI __instance)
        {
            if (!LogLikeMod.CheckStage())
                return;
            Image image = (Image)typeof(EmotionPassiveCardUI).GetField("_artwork", AccessTools.all).GetValue((object)__instance);
            EmotionCardXmlInfo emotionCardXmlInfo = (EmotionCardXmlInfo)typeof(EmotionPassiveCardUI).GetField("_card", AccessTools.all).GetValue((object)__instance);
            if (LogLikeMod.ArtWorks.ContainsKey(emotionCardXmlInfo.Artwork))
                image.sprite = LogLikeMod.ArtWorks[emotionCardXmlInfo.Artwork];
        }

        public static bool EmotionEgoXmlInfo_get_CardId(EmotionEgoXmlInfo __instance, ref LorId __result)
        {
            if (LogLikeMod.RewardCardDic_Dummy == null)
                return true;
            foreach (KeyValuePair<string, List<EmotionEgoXmlInfo>> keyValuePair in LogLikeMod.RewardCardDic_Dummy)
            {
                if (keyValuePair.Value.Contains(__instance))
                {
                    __result = new LorId(keyValuePair.Key, __instance._CardId);
                    return false;
                }
            }
            return true;
        }

        public static void UIOptionWindow_Open(UIOptionWindow __instance)
        {
            if (!((UnityEngine.Object)LogLikeMod.DefFont == (UnityEngine.Object)null))
                return;
            LogLikeMod.DefFont = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
            LogLikeMod.DefFontColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
            LogLikeMod.DefFont_TMP = __instance.displayDropdown.itemText.font;
        }

        public AbnormalityCard AbnormalityCardDescXmlList_GetAbnormalityCard(
          Func<AbnormalityCardDescXmlList, string, AbnormalityCard> orig,
          AbnormalityCardDescXmlList self,
          string cardID)
        {
            if (!LogLikeMod.CheckStage())
                return orig(self, cardID);
            Dictionary<string, AbnormalityCard> dictionary = (Dictionary<string, AbnormalityCard>)typeof(AbnormalityCardDescXmlList).GetField("_dictionary", AccessTools.all).GetValue((object)self);
            AbnormalityCard abnormalityCard;
            if (dictionary.ContainsKey(cardID))
            {
                abnormalityCard = dictionary[cardID];
            }
            else
            {
                abnormalityCard = new AbnormalityCard()
                {
                    id = cardID,
                    abnormalityName = "Not found",
                    cardName = "Not found",
                    abilityDesc = "Not found",
                    flavorText = "Not found",
                    dialogues = (List<AbnormalityCardDialog>)null
                };
                dictionary.Add(cardID, abnormalityCard);
            }
            return abnormalityCard;
        }

        public void UIGetAbnormalityPanel_PointerClickButton(
          Action<UIGetAbnormalityPanel> orig,
          UIGetAbnormalityPanel self)
        {
            if (LogLikeMod.CheckStage() || LogLikeMod.CheckStage(true))
                self.Close();
            else
                orig(self);
        }

        public void BattleSceneRoot_Update(Action<BattleSceneRoot> orig, BattleSceneRoot self)
        {
            if (LogLikeMod.CheckStage() && (Singleton<MysteryManager>.Instance.curMystery != null || Singleton<MysteryManager>.Instance.interruptMysterys != null && Singleton<MysteryManager>.Instance.interruptMysterys.Count > 0 || Singleton<ShopManager>.Instance.curshop != null || LogLikeMod.PauseBool))
                return;
            orig(self);
        }

        public LorId BookXmlInfo_get_DeckId(Func<BookXmlInfo, LorId> orig, BookXmlInfo self)
        {
            if (self.id == new LorId(LogLikeMod.ModId, -854) && RMRCore.CurrentGamemode.ReplaceBaseDeck)
            {
                return RMRCore.CurrentGamemode.BaseDeckReplacement;
            }
            return orig(self);


        }

        public void WorkshopSkinDataSetter_LateInit(
          Action<WorkshopSkinDataSetter> orig,
          WorkshopSkinDataSetter self)
        {
            orig(self);
        }

        public void BattleUnitEmotionDetail_Reset(
          Action<BattleUnitEmotionDetail> orig,
          BattleUnitEmotionDetail self)
        {
            try
            {
                orig(self);
            }
            catch
            {
            }
        }

        public int BattleUnitEmotionDetail_CreateEmotionCoin(
          Func<BattleUnitEmotionDetail, EmotionCoinType, int, int> orig,
          BattleUnitEmotionDetail self,
          EmotionCoinType coinType,
          int count = 1)
        {
            if (!LogLikeMod.CheckStage(true))
                return orig(self, coinType, count);
            self.SetMaxEmotionLevel(Math.Min((int)(LogLikeMod.curchaptergrade + 1), 5));
            BattleUnitModel battleUnitModel = (BattleUnitModel)typeof(BattleUnitEmotionDetail).GetField("_self", AccessTools.all).GetValue((object)self);
            if (battleUnitModel.faction == Faction.Player && battleUnitModel.UnitData.unitData.gender == Gender.Creature)
                return 0;
            MethodInfo method = typeof(BattleUnitEmotionDetail).GetMethod("GetEmotionCoinAdder", AccessTools.all);
            count += (int)method.Invoke((object)self, new object[1]
            {
      (object) count
            });
            if (battleUnitModel.faction == Faction.Player)
                battleUnitModel.personalEgoDetail.AddEgoCoolTime(count);
            if (self.EmotionLevel >= self.MaximumEmotionLevel)
                return 0;
            List<EmotionCoin> emotionCoinList = (List<EmotionCoin>)typeof(BattleUnitEmotionDetail).GetField("_emotionCoins", AccessTools.all).GetValue((object)self);
            for (int index = 0; index < count; ++index)
            {
                if (emotionCoinList.Count < self.MaximumCoinNumber)
                {
                    //if (!self.OnGetEmotionCoin(coinType))
                    //  ;
                    EmotionCoin uninitializedObject = (EmotionCoin)FormatterServices.GetUninitializedObject(typeof(EmotionCoin));
                    typeof(EmotionCoin).GetField("_coinType", AccessTools.all).SetValue((object)uninitializedObject, (object)coinType);
                    emotionCoinList.Add(uninitializedObject);
                    self.totalEmotionCoins.Add(uninitializedObject);
                }
            }
            return count;
        }

        public static bool UIBattleSettingEditPanel_SetBUttonState(
          UIBattleSettingEditPanel __instance,
          UIBattleSettingEditTap state)
        {
            if (!LogLikeMod.CheckStage())
            {
                Singleton<GlobalLogueInventoryPanel>.Instance.SetActive(false);
                return true;
            }
            Button fieldValue1 = LogLikeMod.GetFieldValue<Button>((object)__instance, "button_EquipPage");
            Button fieldValue2 = LogLikeMod.GetFieldValue<Button>((object)__instance, "button_BattleCard");
            Image fieldValue3 = LogLikeMod.GetFieldValue<Image>((object)__instance, "img_equippageFrame");
            Image fieldValue4 = LogLikeMod.GetFieldValue<Image>((object)__instance, "img_battlecardFrame");
            UISettingEquipPageInvenPanel fieldValue5 = LogLikeMod.GetFieldValue<UISettingEquipPageInvenPanel>((object)__instance, "_equipPagePanel");
            UISettingCardInvenPanel fieldValue6 = LogLikeMod.GetFieldValue<UISettingCardInvenPanel>((object)__instance, "_battleCardPanel");
            RectTransform fieldValue7 = LogLikeMod.GetFieldValue<RectTransform>((object)__instance, "rect_LeftBg");
            switch (state)
            {
                case (UIBattleSettingEditTap)2:
                    fieldValue7.localPosition = (Vector3)new Vector2(0.0f, 0.0f);
                    ColorBlock colors1 = fieldValue1.colors;
                    ColorBlock colors2 = fieldValue2.colors;
                    colors1.normalColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    colors2.normalColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    fieldValue1.colors = colors1;
                    fieldValue2.colors = colors2;
                    fieldValue3.enabled = false;
                    fieldValue4.enabled = false;
                    fieldValue5.SetActivePanel(false);
                    fieldValue6.SetActivePanel(false);
                    LogLikeMod.InvenBtnFrame.enabled = true;
                    LogLikeMod.CreatureBtnFrame.enabled = false;
                    LogLikeMod.CraftBtnFrame.enabled = false;
                    Singleton<GlobalLogueInventoryPanel>.Instance.SetActive(true);
                    Singleton<LogCreatureTabPanel>.Instance.SetActive(false);
                    Singleton<LogCraftPanel>.Instance.SetActive(false);
                    return false;
                case (UIBattleSettingEditTap)3:
                    fieldValue7.localPosition = (Vector3)new Vector2(0.0f, 0.0f);
                    ColorBlock colors3 = fieldValue1.colors;
                    ColorBlock colors4 = fieldValue2.colors;
                    colors3.normalColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    colors4.normalColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    fieldValue1.colors = colors3;
                    fieldValue2.colors = colors4;
                    fieldValue3.enabled = false;
                    fieldValue4.enabled = false;
                    fieldValue5.SetActivePanel(false);
                    fieldValue6.SetActivePanel(false);
                    LogLikeMod.InvenBtnFrame.enabled = false;
                    LogLikeMod.CreatureBtnFrame.enabled = true;
                    LogLikeMod.CraftBtnFrame.enabled = false;
                    Singleton<GlobalLogueInventoryPanel>.Instance.SetActive(false);
                    Singleton<LogCreatureTabPanel>.Instance.SetActive(true);
                    Singleton<LogCraftPanel>.Instance.SetActive(false);
                    return false;
                case (UIBattleSettingEditTap)4:
                    fieldValue7.localPosition = (Vector3)new Vector2(0.0f, 0.0f);
                    ColorBlock colors5 = fieldValue1.colors;
                    ColorBlock colors6 = fieldValue2.colors;
                    colors5.normalColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    colors6.normalColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    fieldValue1.colors = colors5;
                    fieldValue2.colors = colors6;
                    fieldValue3.enabled = false;
                    fieldValue4.enabled = false;
                    fieldValue5.SetActivePanel(false);
                    fieldValue6.SetActivePanel(false);
                    LogLikeMod.InvenBtnFrame.enabled = false;
                    LogLikeMod.CreatureBtnFrame.enabled = false;
                    LogLikeMod.CraftBtnFrame.enabled = true;
                    Singleton<GlobalLogueInventoryPanel>.Instance.SetActive(false);
                    Singleton<LogCreatureTabPanel>.Instance.SetActive(false);
                    Singleton<LogCraftPanel>.Instance.SetActive(true);
                    return false;
                default:
                    Singleton<GlobalLogueInventoryPanel>.Instance.SetActive(false);
                    Singleton<LogCreatureTabPanel>.Instance.SetActive(false);
                    Singleton<LogCraftPanel>.Instance.SetActive(false);
                    LogLikeMod.InvenBtnFrame.enabled = false;
                    LogLikeMod.CreatureBtnFrame.enabled = false;
                    LogLikeMod.CraftBtnFrame.enabled = false;
                    return true;
            }
        }

        public static void OnClickCraftTab(UIBattleSettingEditPanel __instance)
        {
            UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click);
            __instance.SetBUttonState((UIBattleSettingEditTap)4);
        }

        public static void OnClickCreatureTab(UIBattleSettingEditPanel __instance)
        {
            UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click);
            __instance.SetBUttonState((UIBattleSettingEditTap)3);
        }

        public static void OnClickInventory(UIBattleSettingEditPanel __instance)
        {
            UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click);
            __instance.SetBUttonState((UIBattleSettingEditTap)2);
        }

        public static void UIBattleSettingEditPanel_Close()
        {
            if (!LogLikeMod.CheckStage())
                return;
            LoguePlayDataSaver.SavePlayData_Menu();
        }

        public void UIBattleSettingEditPanel_Open(
          Action<UIBattleSettingEditPanel, UIBattleSettingEditTap> orig,
          UIBattleSettingEditPanel self,
          UIBattleSettingEditTap state)
        {
            if (LogLikeMod.CheckStage())
            {
                if ((UnityEngine.Object)LogLikeMod.InvenBtn == (UnityEngine.Object)null)
                {
                    Button fieldValue = LogLikeMod.GetFieldValue<Button>((object)self, "button_BattleCard");
                    LogLikeMod.InvenBtn = UnityEngine.Object.Instantiate<Button>(fieldValue, fieldValue.transform.parent);
                    LogLikeMod.InvenBtn.transform.localPosition = fieldValue.transform.localPosition + new Vector3(200f, 0.0f);
                    Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
                    buttonClickedEvent.AddListener((UnityAction)(() => LogLikeMod.OnClickInventory(self)));
                    LogLikeMod.InvenBtn.onClick = buttonClickedEvent;
                    UITextDataLoader component = LogLikeMod.InvenBtn.transform.GetChild(1).gameObject.GetComponent<UITextDataLoader>();
                    component.key = "ui_Inventory";
                    component.SetText();
                    LogLikeMod.InvenBtnFrame = LogLikeMod.InvenBtn.transform.GetChild(0).gameObject.GetComponent<Image>();
                    LogLikeMod.InvenBtnFrame.enabled = false;
                }
                if ((UnityEngine.Object)LogLikeMod.CreatureBtn == (UnityEngine.Object)null)
                {
                    Button fieldValue = LogLikeMod.GetFieldValue<Button>((object)self, "button_BattleCard");
                    LogLikeMod.CreatureBtn = UnityEngine.Object.Instantiate<Button>(fieldValue, fieldValue.transform.parent);
                    LogLikeMod.CreatureBtn.transform.localPosition = fieldValue.transform.localPosition + new Vector3(400f, 0.0f);
                    Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
                    buttonClickedEvent.AddListener((UnityAction)(() => LogLikeMod.OnClickCreatureTab(self)));
                    LogLikeMod.CreatureBtn.onClick = buttonClickedEvent;
                    UITextDataLoader component = LogLikeMod.CreatureBtn.transform.GetChild(1).gameObject.GetComponent<UITextDataLoader>();
                    component.key = "ui_CreatureTab";
                    component.SetText();
                    LogLikeMod.CreatureBtnFrame = LogLikeMod.CreatureBtn.transform.GetChild(0).gameObject.GetComponent<Image>();
                    LogLikeMod.CreatureBtnFrame.enabled = false;
                }
                if ((UnityEngine.Object)LogLikeMod.CraftBtn == (UnityEngine.Object)null)
                {
                    Button fieldValue = LogLikeMod.GetFieldValue<Button>((object)self, "button_BattleCard");
                    LogLikeMod.CraftBtn = UnityEngine.Object.Instantiate<Button>(fieldValue, fieldValue.transform.parent);
                    LogLikeMod.CraftBtn.transform.localPosition = fieldValue.transform.localPosition + new Vector3(600f, 0.0f);
                    Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
                    buttonClickedEvent.AddListener((UnityAction)(() => LogLikeMod.OnClickCraftTab(self)));
                    LogLikeMod.CraftBtn.onClick = buttonClickedEvent;
                    UITextDataLoader component = LogLikeMod.CraftBtn.transform.GetChild(1).gameObject.GetComponent<UITextDataLoader>();
                    component.key = "ui_CraftTab";
                    component.SetText();
                    LogLikeMod.CraftBtnFrame = LogLikeMod.CraftBtn.transform.GetChild(0).gameObject.GetComponent<Image>();
                    LogLikeMod.CraftBtnFrame.enabled = false;
                }
                LogLikeMod.InvenBtn.gameObject.SetActive(true);
                LogLikeMod.CreatureBtn.gameObject.SetActive(true);
                LogLikeMod.CraftBtn.gameObject.SetActive(true);
                Singleton<GlobalLogueInventoryPanel>.Instance.SetActive(false);
                Singleton<LogCreatureTabPanel>.Instance.SetActive(false);
                Singleton<LogCraftPanel>.Instance.SetActive(false);
                Image image = (Image)typeof(UIBattleSettingEditPanel).GetField("img_BlockBackGroundBg", AccessTools.all).GetValue((object)self);
                self.SetBUttonState(state);
                image.raycastTarget = true;
                self.SetActivePanel(true);
            }
            else
            {
                LogLikeMod.InvenBtn.gameObject.SetActive(false);
                LogLikeMod.CreatureBtn.gameObject.SetActive(false);
                LogLikeMod.CraftBtn.gameObject.SetActive(false);
                orig(self, state);
            }
        }

        public void UIBattleSettingLibrarianInfoPanel_SetBattleCardSlotState(
          Action<UIBattleSettingLibrarianInfoPanel> orig,
          UIBattleSettingLibrarianInfoPanel self)
        {
            orig(self);
            if (!LogLikeMod.CheckStage())
                return;
            Color uiColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
            typeof(UIBattleSettingLibrarianInfoPanel).GetField("isBattlePageLock", AccessTools.all).SetValue((object)self, (object)false);
            self.SetBattlePageSlotColor(uiColor);
        }

        public void UIBattleSettingLibrarianInfoPanel_SetEquipPageSlotState(
          Action<UIBattleSettingLibrarianInfoPanel> orig,
          UIBattleSettingLibrarianInfoPanel self)
        {
            orig(self);
            if (!LogLikeMod.CheckStage())
                return;
            Color uiColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
            typeof(UIBattleSettingLibrarianInfoPanel).GetField("isEquipPageLock", AccessTools.all).SetValue((object)self, (object)false);
            self.SetEquipPageSlotColor(uiColor);
        }

        public void BattleDiceCardUI_ShowDetail(Action<BattleDiceCardUI> orig, BattleDiceCardUI self)
        {
            orig(self);
            if (!LogLikeMod.CheckStage(true))
                return;
            self.KeywordListUI.Activate();
        }

        public void BattleDiceCardUI_SetEgoCardForPopup(
          Action<BattleDiceCardUI, EmotionEgoXmlInfo> orig,
          BattleDiceCardUI self,
          EmotionEgoXmlInfo egoxmlinfo)
        {
            orig(self, egoxmlinfo);
            if (!LogLikeMod.CheckStage(true))
                return;
            foreach (GameObject gameObject in (GameObject[])typeof(BattleDiceCardUI).GetField("ob_NormalFrames", AccessTools.all).GetValue((object)self))
                gameObject.SetActive(true);
            foreach (GameObject gameObject in (GameObject[])typeof(BattleDiceCardUI).GetField("ob_EgoFrames", AccessTools.all).GetValue((object)self))
                gameObject.SetActive(false);
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(egoxmlinfo.CardId);
            FieldInfo field1 = typeof(BattleDiceCardUI).GetField("colorFrame", AccessTools.all);
            FieldInfo field2 = typeof(BattleDiceCardUI).GetField("colorLineardodge", AccessTools.all);
            field1.SetValue((object)self, (object)UIColorManager.Manager.GetCardRarityColor(cardItem.Rarity));
            field2.SetValue((object)self, (object)UIColorManager.Manager.GetCardRarityLinearColor(cardItem.Rarity));
            typeof(BattleDiceCardUI).GetMethod("SetRangeIconHsv", AccessTools.all).Invoke((object)self, new object[1]
            {
      (object) UIColorManager.Manager.CardRangeHsvValue[(int) cardItem.Rarity]
            });
            typeof(BattleDiceCardUI).GetMethod("SetFrameColor", AccessTools.all).Invoke((object)self, new object[1]
            {
      field1.GetValue((object) self)
            });
            self.SetLinearDodgeColor(true);
        }

        public void BattleUnitCardsInHandUI_SetCardsObject(
          Action<BattleUnitCardsInHandUI, BattleUnitModel, bool> orig,
          BattleUnitCardsInHandUI self,
          BattleUnitModel unitModel,
          bool isClicked = true)
        {
            if (LogLikeMod.CheckStage(true))
            {
                GameObject gameObject = (GameObject)typeof(BattleUnitCardsInHandUI).GetField("_rootObj", AccessTools.all).GetValue((object)self);
                Toggle toggle = (Toggle)typeof(BattleUnitCardsInHandUI).GetField("toggle_ShowEgo", AccessTools.all).GetValue((object)self);
                FieldInfo field1 = typeof(BattleUnitCardsInHandUI).GetField("isOverOnEgoToggle", AccessTools.all);
                FieldInfo field2 = typeof(BattleUnitCardsInHandUI).GetField("_selectedUnit", AccessTools.all);
                FieldInfo field3 = typeof(BattleUnitCardsInHandUI).GetField("_hOveredUnit", AccessTools.all);
                FieldInfo field4 = typeof(BattleUnitCardsInHandUI).GetField("_handState", AccessTools.all);
                gameObject.SetActive(true);
                field1.SetValue((object)self, (object)false);
                if (isClicked)
                    field2.SetValue((object)self, (object)unitModel);
                else
                    field3.SetValue((object)self, (object)unitModel);
                BattleUnitCardsInHandUI.EgoToggleState egoToggleState = BattleUnitCardsInHandUI.EgoToggleState.Hide;
                if (unitModel.personalEgoDetail.ExistsCard())
                    egoToggleState = toggle.isOn ? BattleUnitCardsInHandUI.EgoToggleState.On : BattleUnitCardsInHandUI.EgoToggleState.Off;
                else
                    field4.SetValue((object)self, (object)BattleUnitCardsInHandUI.HandState.BattleCard);
                if (!PlatformManager.Instance.AchievementUnlocked(AchievementEnum.ONCE_COPY) && unitModel.allyCardDetail.Exsist6CardsInHand_andCopy())
                    PlatformManager.Instance.UnlockAchievement(AchievementEnum.ONCE_COPY);
                typeof(BattleUnitCardsInHandUI).GetMethod("SetEgoToggleState", AccessTools.all).Invoke((object)self, new object[1]
                {
        (object) egoToggleState
                });
            }
            else
                orig(self, unitModel, isClicked);
        }

        public bool DeckModel_MoveCardToInventory(
          Func<DeckModel, LorId, bool> orig,
          DeckModel self,
          LorId cardId)
        {
            if (!LogLikeMod.CheckStage())
                return orig(self, cardId);
            if (!((List<DiceCardXmlInfo>)typeof(DeckModel).GetField("_deck", AccessTools.all).GetValue((object)self)).Remove(ItemXmlDataList.instance.GetCardItem(cardId)))
                return false;
            LogueBookModels.AddCard(cardId);
            return true;
        }

        public CardEquipState DeckModel_AddCardFromInventory(
          Func<DeckModel, LorId, CardEquipState> orig,
          DeckModel self,
          LorId cardId)
        {
            CardEquipState cardEquipState;
            if (LogLikeMod.CheckStage())
            {
                List<DiceCardXmlInfo> diceCardXmlInfoList = (List<DiceCardXmlInfo>)typeof(DeckModel).GetField("_deck", AccessTools.all).GetValue((object)self);
                if (diceCardXmlInfoList.Count >= 9)
                {
                    cardEquipState = CardEquipState.FullOfDeck;
                }
                else
                {
                    DiceCardXmlInfo card = ItemXmlDataList.instance.GetCardItem(cardId);
                    DiceCardSelfAbilityBase diceCardSelfAbility = Singleton<AssemblyManager>.Instance.CreateInstance_DiceCardSelfAbility(card.Script);
                    CardEquipState state;
                    if (diceCardSelfAbility != null && diceCardSelfAbility is LogDiceCardSelfAbility && !(diceCardSelfAbility as LogDiceCardSelfAbility).CanAddDeck(self, out state))
                        return state;
                    if (diceCardXmlInfoList.FindAll((Predicate<DiceCardXmlInfo>)(x => x.id.GetOriginalId() == card.id.GetOriginalId())).Count >= card.Limit)
                        cardEquipState = CardEquipState.OverCardLimit;
                    else if (!LogueBookModels.RemoveCard(card.id))
                    {
                        cardEquipState = CardEquipState.LackOfCards;
                    }
                    else
                    {
                        Singleton<LibraryQuestManager>.Instance.OnEditCard();
                        diceCardXmlInfoList.Add(card);
                        cardEquipState = CardEquipState.Equippable;
                    }
                }
            }
            else
                cardEquipState = orig(self, cardId);
            return cardEquipState;
        }

        public void UIInvenCardListScroll_SetData(
          Action<UIInvenCardListScroll, List<DiceCardItemModel>, UnitDataModel> orig,
          UIInvenCardListScroll self,
          List<DiceCardItemModel> cards,
          UnitDataModel unitData)
        {
            if (LogLikeMod.CheckStage())
                cards = LogueBookModels.GetCardListForInven();
            orig(self, cards, unitData);
        }

        public CardEquipState UnitDataModel_AddCardFromInventory(
          Func<UnitDataModel, LorId, CardEquipState> orig,
          UnitDataModel self,
          LorId cardId)
        {
            if (!LogLikeMod.CheckStage())
                return orig(self, cardId);
            return ItemXmlDataList.instance.GetCardItem(cardId) == null ? CardEquipState.ERROR : self.bookItem.AddCardFromInventoryToCurrentDeck(cardId);
        }

        public void UIInvenCardSlot_OnClickCardEquipInfoButton(
          Action<UIInvenCardSlot> orig,
          UIInvenCardSlot self)
        {
            if (LogLikeMod.CheckStage())
                return;
            orig(self);
        }

        public void UIInvenCardSlot_SetSlotState(Action<UIInvenCardSlot> orig, UIInvenCardSlot self)
        {
            if (LogLikeMod.CheckStage())
            {
                TextMeshProUGUI textMeshProUgui = (TextMeshProUGUI)typeof(UIInvenCardSlot).GetField("txt_deckLimit", AccessTools.all).GetValue((object)self);
                GameObject gameObject = (GameObject)typeof(UIInvenCardSlot).GetField("deckLimitRoot", AccessTools.all).GetValue((object)self);
                DiceCardItemModel _cardModel = (DiceCardItemModel)typeof(UIOriginCardSlot).GetField("_cardModel", AccessTools.all).GetValue((object)self);
                FieldInfo field = typeof(UIInvenCardSlot).GetField("slotState", AccessTools.all);
                field.SetValue((object)self, (object)UIINVENCARD_STATE.None);
                if (_cardModel.num <= 0)
                    field.SetValue((object)self, (object)UIINVENCARD_STATE.NumberZero);
                if (UI.UIController.Instance.CurrentUnit.GetDeckAll().FindAll((Predicate<DiceCardXmlInfo>)(x => x.id.GetOriginalId() == _cardModel.GetID().GetOriginalId())).Count >= _cardModel.GetLimit())
                    field.SetValue((object)self, (object)UIINVENCARD_STATE.LimitedDeck);
                UnitDataModel currentUnit = UI.UIController.Instance.CurrentUnit;
                if (currentUnit != null)
                {
                    BookModel bookItem = currentUnit.bookItem;
                    List<DiceCardXmlInfo> onlyCards = bookItem.GetOnlyCards();
                    if (_cardModel.ClassInfo.optionList.Contains(CardOption.OnlyPage))
                    {
                        if (!onlyCards.Exists((Predicate<DiceCardXmlInfo>)(y => y.id.GetOriginalId() == _cardModel.GetID().GetOriginalId())))
                            field.SetValue((object)self, (object)UIINVENCARD_STATE.OnlyPage);
                    }
                    else if (bookItem.ClassInfo.RangeType == EquipRangeType.Melee && _cardModel.GetSpec().Ranged == CardRange.Far)
                        field.SetValue((object)self, (object)UIINVENCARD_STATE.RangeCard);
                    else if (bookItem.ClassInfo.RangeType == EquipRangeType.Range && _cardModel.GetSpec().Ranged == CardRange.Near)
                        field.SetValue((object)self, (object)UIINVENCARD_STATE.MeleeCard);
                }
                gameObject.gameObject.SetActive((UIINVENCARD_STATE)field.GetValue((object)self) > UIINVENCARD_STATE.None);
                self.SetGrayScale((UIINVENCARD_STATE)field.GetValue((object)self) > UIINVENCARD_STATE.None);
                switch ((UIINVENCARD_STATE)field.GetValue((object)self))
                {
                    case UIINVENCARD_STATE.LimitedDeck:
                        textMeshProUgui.text = TextDataModel.GetText("ui_card_equipstate_overcardlimit");
                        break;
                    case UIINVENCARD_STATE.LimitedFloor:
                        textMeshProUgui.text = TextDataModel.GetText("ui_card_equipstate_overfloorlimit");
                        break;
                    case UIINVENCARD_STATE.NumberZero:
                        textMeshProUgui.text = TextDataModel.GetText("ui_card_equipstate_lackofcards");
                        break;
                    case UIINVENCARD_STATE.OnlyPage:
                        textMeshProUgui.text = TextDataModel.GetText("ui_card_equipstate_onlypagelimit");
                        break;
                    case UIINVENCARD_STATE.RangeCard:
                        textMeshProUgui.text = TextDataModel.GetText("ui_card_equipstate_fartypelimit");
                        break;
                    case UIINVENCARD_STATE.MeleeCard:
                        textMeshProUgui.text = TextDataModel.GetText("ui_card_equipstate_neartypelimit");
                        break;
                }
                self.RefreshNumbersData();
            }
            else
                orig(self);
        }

        public void UIInvitationRightMainPanel_ConfirmSendInvitation(
          Action<UIInvitationRightMainPanel> orig,
          UIInvitationRightMainPanel self)
        {
            StageClassInfo bookRecipe = self.GetBookRecipe();
            StagesXmlList.Instance.RestoreToDefault();
            RewardPassivesList.Instance.RestoreToDefault();
            MysteryXmlList.Instance.RestoreToDefault();

            RMRCore.CurrentGamemode = null;

            LorId invitation = bookRecipe.id;
            bool succes = false;

            if (invitation == new LorId(RMRCore.packageId, -855))
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(invitation, true);
                RMRCore.CurrentGamemode.FilterContent();

                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadPlayData();
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                this.Log("CONTINUED ROGUELIKE RUN! " + RMRCore.CurrentGamemode.SaveDataString);
                return;
            }
            else if (bookRecipe.id == new LorId(LogLikeMod.ModId, -2854))
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(new LorId(RMRCore.packageId, -854), false);
                RMRCore.CurrentGamemode.FilterContent();
                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadChDebugData(ChapterGrade.Grade2);
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter1());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter2());
                LoguePlayDataSaver.RemovePlayerData();
            }
            else if (bookRecipe.id == new LorId(LogLikeMod.ModId, -3854))
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(new LorId(RMRCore.packageId, -854), false);
                RMRCore.CurrentGamemode.FilterContent();
                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadChDebugData(ChapterGrade.Grade3);
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter1());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter2());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter3());
                LoguePlayDataSaver.RemovePlayerData();
            }
            else if (bookRecipe.id == new LorId(LogLikeMod.ModId, -4854))
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(new LorId(RMRCore.packageId, -854), false);
                RMRCore.CurrentGamemode.FilterContent();
                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadChDebugData(ChapterGrade.Grade4);
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter1());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter2());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter3());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter4());
                LoguePlayDataSaver.RemovePlayerData();
            }
            else if (bookRecipe.id == new LorId(LogLikeMod.ModId, -5854))
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(new LorId(RMRCore.packageId, -854), false);
                RMRCore.CurrentGamemode.FilterContent();
                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadChDebugData(ChapterGrade.Grade5);
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter1());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter2());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter3());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter4());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter5());
                LoguePlayDataSaver.RemovePlayerData();
            }
            else if (bookRecipe.id == new LorId(LogLikeMod.ModId, -6854))
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(new LorId(RMRCore.packageId, -854), false);
                RMRCore.CurrentGamemode.FilterContent();
                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadChDebugData(ChapterGrade.Grade6);
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter1());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter2());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter3());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter4());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter5());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter6());
                LoguePlayDataSaver.RemovePlayerData();
            }
            else if (bookRecipe.id == new LorId(LogLikeMod.ModId, -7854))
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(new LorId(RMRCore.packageId, -854), false);
                RMRCore.CurrentGamemode.FilterContent();
                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadChDebugData(ChapterGrade.Grade7);
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter1());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter2());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter3());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter4());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter5());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter6());
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects((GlobalLogueEffectBase)new CraftEquipChapter7());
                LoguePlayDataSaver.RemovePlayerData();
            }
            
            try
            {
                succes = RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(invitation, false);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            if (RMRCore.CurrentGamemode == null)
                RMRCore.CurrentGamemode = new RoguelikeGamemode_RMR_Default();

            if (succes)
            {
                RMRCore.CurrentGamemode.FilterContent();
                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                bookRecipe.mapInfo.Clear();
                LogueBookModels.CreatePlayer();
                orig(self);
                LogueBookModels.CreatePlayerBattle();
                LoguePlayDataSaver.RemovePlayerData();
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                this.Log("NEW RUN! " + RMRCore.CurrentGamemode.SaveDataString);
            }
            else
            {
                orig(self);
                this.Log("REGULAR RECEPTION!");
            }
        }

        public void UILibrarianCharacterListPanel_SetLibrarianCharacterListPanel_Battle(
          Action<UILibrarianCharacterListPanel> orig,
          UILibrarianCharacterListPanel self)
        {
            if (LogLikeMod.CheckStage())
            {
                UICharacterList uiCharacterList = (UICharacterList)typeof(UILibrarianCharacterListPanel).GetField("CharacterList", AccessTools.all).GetValue((object)self);
                List<UnitBattleDataModel> playerBattleModel = LogueBookModels.playerBattleModel;
                self.SetCharacterRenderer(playerBattleModel, false);
                uiCharacterList.InitUnitListFromBattleData(playerBattleModel);
                typeof(UILibrarianCharacterListPanel).GetMethod("UpdateFrameToSephirah", AccessTools.all).Invoke((object)self, new object[1]
                {
        (object) SephirahType.None
                });
            }
            else
                orig(self);
        }

        public void UILibrarianCharacterListPanel_InitSephirahSelectionButtonsInBattle(
          Action<UILibrarianCharacterListPanel, List<StageLibraryFloorModel>> orig,
          UILibrarianCharacterListPanel self,
          List<StageLibraryFloorModel> floors)
        {
            if (LogLikeMod.CheckStage())
            {
                foreach (UISephirahSelectionButton sephirahSelectionButton in (List<UISephirahSelectionButton>)typeof(UILibrarianCharacterListPanel).GetField("SephirahSelectionButtons", AccessTools.all).GetValue((object)self))
                {
                    sephirahSelectionButton.Deactivate();
                    sephirahSelectionButton.SetLock();
                }
            }
            else
                orig(self, floors);
        }

        public static bool UIBattleSettingPanel_OnClickBackButton()
        {
            return !LogLikeMod.CheckStage() || !UIPassiveSuccessionPopup.Instance.isActiveAndEnabled;
        }

        public void UIBattleSettingPanel_SetCurrentSephirahButton(
          Action<UIBattleSettingPanel> orig,
          UIBattleSettingPanel self)
        {
            orig(self);
            if (!LogLikeMod.CheckStage())
                return;
            foreach (UISephirahButton uiSephirahButton in (List<UISephirahButton>)typeof(UIBattleSettingPanel).GetField("SephirahButtons", AccessTools.all).GetValue((object)self))
                uiSephirahButton.SetButtonState(UISephirahButton.ButtonState.Close);
        }

        public static void BookInventoryModel_GetAllBookByInstanceId(
          ref BookModel __result,
          int bookInstanceId)
        {
            if (!LogLikeMod.CheckStage())
                return;
            BookModel bookModel = LogueBookModels.booklist.Find((Predicate<BookModel>)(x => x.instanceId == bookInstanceId));
            if (bookModel != null)
            {
                __result = bookModel;
            }
            else
            {
                BookModel bookItem = LogueBookModels.playerModel.Find((Predicate<UnitDataModel>)(x => x.bookItem.instanceId == bookInstanceId)).bookItem;
                if (bookItem == null)
                    return;
                __result = bookItem;
            }
        }

        public static void BookInventoryModel_GetBookByInstanceId(
          ref BookModel __result,
          int bookInstanceId)
        {
            if (!LogLikeMod.CheckStage())
                return;
            BookModel bookModel = LogueBookModels.booklist.Find((Predicate<BookModel>)(x => x.instanceId == bookInstanceId));
            if (bookModel != null)
            {
                __result = bookModel;
            }
            else
            {
                BookModel bookItem = LogueBookModels.playerModel.Find((Predicate<UnitDataModel>)(x => x.bookItem.instanceId == bookInstanceId)).bookItem;
                if (bookItem == null)
                    return;
                __result = bookItem;
            }
        }

        public static void BookInventoryModel_GetBookListAll(ref List<BookModel> __result)
        {
            if (!LogLikeMod.CheckStage())
                return;
            List<BookModel> bookModelList = new List<BookModel>();
            foreach (UnitDataModel unitDataModel in LogueBookModels.playerModel)
                bookModelList.Add(unitDataModel.defaultBook);
            bookModelList.AddRange((IEnumerable<BookModel>)LogueBookModels.booklist);
            __result = bookModelList;
        }

        public static void BookInventoryModel_GetBookList_PassiveEquip(
          ref List<BookModel> __result,
          BookModel booktobeEquiped)
        {
            if (!LogLikeMod.CheckStage())
                return;
            List<BookModel> bookModelList1 = new List<BookModel>();
            List<BookModel> bookModelList2 = new List<BookModel>();
            bookModelList2.AddRange((IEnumerable<BookModel>)LogueBookModels.booklist);
            foreach (BookModel bookModel in bookModelList2)
            {
                if (bookModel.owner == null && bookModel.GetPassiveInfoList().Count != 0)
                    bookModelList1.Add(bookModel);
            }
            __result = bookModelList1;
        }

        public List<BookModel> BookInventoryModel_GetBookList_equip(
          Func<BookInventoryModel, List<BookModel>> orig,
          BookInventoryModel self)
        {
            return LogLikeMod.CheckStage() ? LogueBookModels.booklist : orig(self);
        }

        public List<LorId> DropBookInventoryModel_GetBookList_invitationBookList(
          Func<DropBookInventoryModel, List<LorId>> orig,
          DropBookInventoryModel self)
        {
            List<LorId> invitationBookList = orig(self);
            invitationBookList.Add(new LorId(LogLikeMod.ModId, -854));
            if (LoguePlayDataSaver.CheckPlayerData())
                invitationBookList.Add(new LorId(LogLikeMod.ModId, -855));
            invitationBookList.Add(new LorId(LogLikeMod.ModId, -2854));
            invitationBookList.Add(new LorId(LogLikeMod.ModId, -3854));
            invitationBookList.Add(new LorId(LogLikeMod.ModId, -4854));
            invitationBookList.Add(new LorId(LogLikeMod.ModId, -5854));
            return invitationBookList;
        }

        public static IEnumerator DisableRoutine(LevelUpUI self)
        {
            self.cardHidingGroup.alpha = 0.0f;
            float elapsed = 0.0f;
            while ((double)elapsed < 1.0)
            {
                elapsed += TimeManager.GetUIDeltaTime() * 2f;
                self.cardSelectionGroup.alpha = 1f - elapsed;
                yield return (object)null;
            }
            self.SetRootCanvas(false);
            for (int i = 0; i < self.candidates.Length; ++i)
                self.candidates[i].gameObject.SetActive(false);
            for (int j = 0; j < self.egoSlotList.Length; ++j)
                self.egoSlotList[j].gameObject.SetActive(false);
        }

        public static IEnumerator TranslateRoutine(bool hide, LevelUpUI self)
        {
            float translateDelay = (float)typeof(LevelUpUI).GetField("translateDelay", AccessTools.all).GetValue((object)self);
            float translateSpeed = (float)typeof(LevelUpUI).GetField("translateSpeed", AccessTools.all).GetValue((object)self);
            float elapsed = 0.0f;
            Vector2 v = new Vector2(-2027f, -213f);
            Vector2 vector2 = new Vector2(751f, 79f);
            if (hide)
            {
                self.showTranslator.anchoredPosition = Vector2.zero;
                self.cardSelectionGroup.interactable = false;
            }
            else
                self.showTranslator.anchoredPosition = v;
            yield return (object)YieldCache.WaitForSeconds(translateDelay);
            if (hide)
            {
                while ((double)elapsed < 1.0)
                {
                    elapsed += Time.deltaTime * translateSpeed;
                    self.showTranslator.anchoredPosition = Vector2.Lerp(Vector2.zero, v, elapsed * elapsed);
                    yield return (object)YieldCache.waitFrame;
                }
            }
            else
            {
                while ((double)elapsed < 1.0)
                {
                    elapsed += Time.deltaTime * translateSpeed;
                    self.showTranslator.anchoredPosition = Vector2.Lerp(v, Vector2.zero, elapsed);
                    yield return (object)YieldCache.waitFrame;
                }
                self.cardSelectionGroup.interactable = true;
            }
        }

        public static void LevelUpUI_OnClickTargetUnit()
        {
            if (!LogLikeMod.CheckStage())
                return;
            foreach (BattleUnitModel battleUnitModel in (IEnumerable<BattleUnitModel>)BattleObjectManager.instance.GetList())
            {
                if (battleUnitModel.view.abCardSelector.isInitialized)
                    battleUnitModel.view.abCardSelector.TurnOffUI();
            }
        }

        public IEnumerator LevelUpUI_OnSelectRoutine(Func<LevelUpUI, IEnumerator> orig, LevelUpUI self)
        {
            if (LogLikeMod.CheckStage())
            {
                FieldInfo _needUnitSelection = typeof(LevelUpUI).GetField("_needUnitSelection", AccessTools.all);
                FieldInfo _selectedCard = typeof(LevelUpUI).GetField("_selectedCard", AccessTools.all);
                self.cardSelectionGroup.interactable = false;
                yield return (object)YieldCache.WaitForSeconds(0.1f);
                if ((bool)_needUnitSelection.GetValue((object)self))
                {
                    EmotionCardXmlInfo einfo = _selectedCard.GetValue((object)self) as EmotionCardXmlInfo;
                    LorId id = new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(einfo), einfo.id);
                    List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetList(Faction.Player);
                    RewardPassiveInfo pinfo = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(id);
                    PickUpModelBase pickupmodel = (PickUpModelBase)null;
                    if (pinfo != null)
                        pickupmodel = LogLikeMod.FindPickUp(pinfo.script);
                    if (aliveList != null)
                    {
                        if (pickupmodel != null)
                        {
                            if (pickupmodel.GetPickupTarget() != null)
                            {
                                aliveList = pickupmodel.GetPickupTarget();
                                goto label_11;
                            }
                            EmotionCardXmlInfo info = _selectedCard.GetValue((object)self) as EmotionCardXmlInfo;
                            pickupmodel.id = new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(info), info.id);
                            aliveList.RemoveAll((Predicate<BattleUnitModel>)(x => !pickupmodel.IsCanPickUp(x.UnitData.unitData)));
                            info = (EmotionCardXmlInfo)null;
                        }
                        else
                            aliveList.RemoveAll((Predicate<BattleUnitModel>)(x => x.IsDead()));
                    }
                label_11:
                    if (aliveList.Count > 0)
                    {
                        if (_selectedCard.GetValue((object)self) != null)
                            self.selectedEmotionCard.Init((EmotionCardXmlInfo)_selectedCard.GetValue((object)self), true);
                        if (Singleton<StageController>.Instance.AllyFormationDirection == Direction.LEFT)
                        {
                            self.selectedEmotionCard.transform.localPosition = new Vector3(410f, -510f);
                            self.selectedEmotionCardBg.transform.localScale = new Vector3(-1f, 1f, 1f);
                        }
                        else
                        {
                            self.selectedEmotionCard.transform.localPosition = new Vector3(-410f, -510f);
                            self.selectedEmotionCardBg.transform.localScale = new Vector3(1f, 1f, 1f);
                        }
                        List<UICustomSelectable_autofind> list = new List<UICustomSelectable_autofind>();
                        foreach (BattleUnitModel battleUnitModel in aliveList)
                        {
                            battleUnitModel.view.abCardSelector.Init(battleUnitModel, Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.emotionLevel);
                            list.Add(battleUnitModel.view.abCardSelector.selectable);
                        }
                        foreach (UICustomSelectable_autofind uicustomSelectable_autofind in list)
                            uicustomSelectable_autofind.SetActivatedCharacterSelectables(list);
                        if (list.Count > 0)
                            BattleUIInputController.Instance.SelectSelectableForcely((UICustomSelectable)list[0]);
                        self.OnSelectHide();
                        list = (List<UICustomSelectable_autofind>)null;
                    }
                    else
                    {
                        self.StartCoroutine(LogLikeMod.DisableRoutine(self));
                        self.StartCoroutine(LogLikeMod.TranslateRoutine(true, self));
                    }
                    einfo = (EmotionCardXmlInfo)null;
                    id = (LorId)null;
                    aliveList = (List<BattleUnitModel>)null;
                    pinfo = (RewardPassiveInfo)null;
                }
                else
                {
                    self.StartCoroutine(LogLikeMod.DisableRoutine(self));
                    self.StartCoroutine(LogLikeMod.TranslateRoutine(true, self));
                }
            }
            else
                yield return (object)orig(self);
        }

        public void LevelUpUI_SetEmotionPerDataUI(
          Action<LevelUpUI, float, float> orig,
          LevelUpUI self,
          float positivevalue,
          float negativevalue)
        {
            try
            {
                if (LogLikeMod.CheckStage(true))
                {
                    TextMeshProUGUI textMeshProUgui1 = (TextMeshProUGUI)typeof(LevelUpUI).GetField("txt_PositiveValueText", AccessTools.all).GetValue((object)self);
                    TextMeshProUGUI textMeshProUgui2 = (TextMeshProUGUI)typeof(LevelUpUI).GetField("txt_NegativeValueText", AccessTools.all).GetValue((object)self);
                    RectTransform rectTransform = (RectTransform)typeof(LevelUpUI).GetField("rect_BarContent", AccessTools.all).GetValue((object)self);
                    textMeshProUgui1.text = "0";
                    textMeshProUgui2.text = "0";
                    rectTransform.anchoredPosition = Vector2.zero;
                    rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
                    return;
                }
            }
            catch
            {
                orig(self, positivevalue, negativevalue);
            }
            orig(self, positivevalue, negativevalue);
        }

        public void LevelUpUI_InitBase(
          Action<LevelUpUI, int, bool> orig,
          LevelUpUI self,
          int selectedCount,
          bool isEgo = false)
        {
            orig(self, selectedCount, isEgo);
            if (LogLikeMod.CheckStage(true))
            {
                TextMeshProUGUI textMeshProUgui1 = (TextMeshProUGUI)typeof(LevelUpUI).GetField("txt_SelectDesc", AccessTools.all).GetValue((object)self);
                TextMeshProUGUI textMeshProUgui2 = (TextMeshProUGUI)typeof(LevelUpUI).GetField("txt_BtnSelectDesc", AccessTools.all).GetValue((object)self);
                if ((UnityEngine.Object)LogLikeMod.skipPanel == (UnityEngine.Object)null)
                {
                    Button button = ModdingUtils.CreateButton(self.selectablePanel.transform.parent, "AbCardSelection_Skip", new Vector2(1f, 1f), new Vector2(650f, 385f));
                    Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
                    buttonClickedEvent.AddListener((UnityAction)(() =>
                    {
                        if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.CardReward)
                        {
                            List<DiceCardXmlInfo> cardlist = new List<DiceCardXmlInfo>();
                            foreach (BattleDiceCardUI egoSlot in self.egoSlotList)
                                cardlist.Add(egoSlot.CardModel.XmlData);
                            Singleton<GlobalLogueEffectManager>.Instance.OnSkipCardRewardChoose(cardlist);
                            LogLikeMod.rewards.RemoveAt(0);
                        }
                        if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.PassiveReward)
                            LogLikeMod.rewards_passive.RemoveAt(0);
                        self.StartCoroutine((IEnumerator)typeof(LevelUpUI).GetMethod("DisableRoutine", AccessTools.all).Invoke((object)self, (object[])null));
                        self.StartCoroutine((IEnumerator)typeof(LevelUpUI).GetMethod("TranslateRoutine", AccessTools.all).Invoke((object)self, new object[1]
            {
            (object) true
                      }));
                    }));
                    button.onClick = buttonClickedEvent;
                    LogLikeMod.skipPanel = button;
                    LogLikeMod.skipPanelText = ModdingUtils.CreateText_TMP(button.transform, new Vector2(-10f, 0.0f), (int)textMeshProUgui2.fontSize, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, textMeshProUgui2.color, textMeshProUgui2.font);
                }
                if ((UnityEngine.Object)LogLikeMod.StageRemainText == (UnityEngine.Object)null)
                {
                    UILogCustomSelectable BackGround = ModdingUtils.CreateLogSelectable(self.selectablePanel.transform.parent, "AbCardSelection_Skip", new Vector2(1f, 1f), new Vector2(0.0f, 500f));
                    BackGround.SelectEvent = new UnityEventBasedata();
                    BackGround.SelectEvent.AddListener((UnityAction<BaseEventData>)(e =>
                    {
                        string name = TextDataModel.GetText("ui_stageremain") + LogueBookModels.RemainStageList[LogLikeMod.curchaptergrade].Count.ToString();
                        Dictionary<string, int> dictionary = new Dictionary<string, int>();
                        foreach (LogueStageInfo info in LogueBookModels.RemainStageList[LogLikeMod.curchaptergrade])
                        {
                            LogueBookModels.CreateStageDesc(info);
                            AbnormalityCard abnormalityCard = Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(LogLikeMod.GetRegisteredPickUpXml(info).Name);
                            if (!dictionary.ContainsKey(abnormalityCard.cardName))
                                dictionary[abnormalityCard.cardName] = 1;
                            else
                                ++dictionary[abnormalityCard.cardName];
                        }
                        string desc = string.Empty;
                        foreach (KeyValuePair<string, int> keyValuePair in dictionary)
                            desc = desc + keyValuePair.Key + keyValuePair.Value.ToString() + Environment.NewLine;
                        SingletonBehavior<UIBattleOverlayManager>.Instance.EnableBufOverlay(name, desc, (Sprite)null, BackGround.gameObject);
                    }));
                    BackGround.DeselectEvent = new UnityEventBasedata();
                    BackGround.DeselectEvent.AddListener((UnityAction<BaseEventData>)(e => SingletonBehavior<UIBattleOverlayManager>.Instance.DisableOverlay()));
                    TextMeshProUGUI textTmp = ModdingUtils.CreateText_TMP(BackGround.transform, new Vector2(-10f, 0.0f), (int)textMeshProUgui2.fontSize, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.Midline, textMeshProUgui2.color, textMeshProUgui2.font);
                    LogLikeMod.StageRemainPanel = BackGround;
                    LogLikeMod.StageRemainText = textTmp;
                }
                int num;
                switch (RewardingModel.rewardFlag)
                {
                    case RewardingModel.RewardFlag.CardReward:
                        num = 1;
                        break;
                    case RewardingModel.RewardFlag.PassiveReward:
                        num = LogLikeMod.rewards_passive[0].rewards[0].rewardtype != RewardType.Creature ? 1 : 0;
                        break;
                    default:
                        num = 0;
                        break;
                }
                if (num != 0)
                {
                    LogLikeMod.skipPanel.gameObject.SetActive(true);
                    LogLikeMod.skipPanelText.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("ui_selectskip");
                }
                else
                    LogLikeMod.skipPanel.gameObject.SetActive(false);
                if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.NextStageChoose)
                {
                    LogLikeMod.StageRemainPanel.gameObject.SetActive(true);
                    TextMeshProUGUI stageRemainText1 = LogLikeMod.StageRemainText;
                    string text1 = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("ui_stageremain");
                    int count = LogueBookModels.RemainStageList[LogLikeMod.curchaptergrade].Count;
                    string str1 = count.ToString();
                    string str2 = text1 + str1;
                    stageRemainText1.text = str2;
                    if (LogLikeMod.curstagetype == StageType.Boss)
                    {
                        TextMeshProUGUI stageRemainText2 = LogLikeMod.StageRemainText;
                        string text2 = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("ui_stageremain");
                        count = LogueBookModels.RemainStageList[LogLikeMod.curchaptergrade + 1].Count;
                        string str3 = count.ToString();
                        string str4 = text2 + str3;
                        stageRemainText2.text = str4;
                    }
                }
                else
                    LogLikeMod.StageRemainPanel.gameObject.SetActive(false);
                if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.CardReward)
                {
                    textMeshProUgui1.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("BattleEnd_CardReward");
                    textMeshProUgui2.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("BattleEnd_CardReward");
                    if (!Singleton<TutorialManager>.Instance.IsSeeTuto("tutorial_BattlePage1_1"))
                        Singleton<TutorialManager>.Instance.LoadTuto("tutorial_BattlePage1_1");
                }
                if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.PassiveReward)
                {
                    textMeshProUgui1.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("BattleEnd_PassiveReward");
                    textMeshProUgui2.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("BattleEnd_PassiveReward");
                    if (!Singleton<TutorialManager>.Instance.IsSeeTuto("tutorial_EquipPage1_1new") && Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(self.candidates[0].Card), self.candidates[0].Card.id)).rewardtype == RewardType.EquipPage)
                        Singleton<TutorialManager>.Instance.LoadTuto("tutorial_EquipPage1_1new");
                    if (!Singleton<TutorialManager>.Instance.IsSeeTuto("tutorial_EmotionPage1_1") && Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(self.candidates[0].Card), self.candidates[0].Card.id)).rewardtype == RewardType.Creature)
                        Singleton<TutorialManager>.Instance.LoadTuto("tutorial_EmotionPage1_1");
                }
                if (RewardingModel.rewardFlag != RewardingModel.RewardFlag.NextStageChoose)
                    return;
                textMeshProUgui1.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("BattleEnd_NextStage");
                textMeshProUgui2.text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText("BattleEnd_NextStage");
            }
            else if ((UnityEngine.Object)LogLikeMod.skipPanel != (UnityEngine.Object)null)
                LogLikeMod.skipPanel.gameObject.SetActive(false);
        }

        public bool RoundEndPhase_ReturnUnit(
          Func<StageController, float, bool> orig,
          StageController self,
          float deltaTime)
        {
            return orig(self, deltaTime);
        }

        public void StageLibraryFloorModel_OnPickEgoCard(
          Action<StageLibraryFloorModel, EmotionEgoXmlInfo> orig,
          StageLibraryFloorModel self,
          EmotionEgoXmlInfo egoCard)
        {
            if (LogLikeMod.CheckStage(true))
            {
                List<DiceCardXmlInfo> cardlist = new List<DiceCardXmlInfo>();
                foreach (BattleDiceCardUI egoSlot in SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.egoSlotList)
                    cardlist.Add(egoSlot.CardModel.XmlData);
                Singleton<GlobalLogueEffectManager>.Instance.OnPickCardReward(cardlist, ItemXmlDataList.instance.GetCardItem(egoCard.CardId));
                LogueBookModels.AddCard(egoCard.CardId);
                LogLikeMod.rewards.RemoveAt(0);
            }
            else
                orig(self, egoCard);
        }

        public void StageLibraryFloorModel_OnPickPassiveCard(
          Action<StageLibraryFloorModel, EmotionCardXmlInfo, BattleUnitModel> orig,
          StageLibraryFloorModel self,
          EmotionCardXmlInfo card,
          BattleUnitModel target = null)
        {
            if (RewardingModel.rewardFlag == RewardingModel.RewardFlag.NextStageChoose && Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Stage(card), card.id)) != null)
                LogLikeMod.SetNextStage(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Stage(card), card.id), Singleton<StagesXmlList>.Instance.GetStageInfo(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Stage(card), card.id)).type);
            else if (Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(card), card.id)) != null)
            {
                PickUpModelBase pickUp = LogLikeMod.FindPickUp(card.Script[0]);
                pickUp.rewardinfo = Singleton<RewardPassivesList>.Instance.GetPassiveInfo(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(card), card.id));
                pickUp.id = new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(card), card.id);
                if (pickUp != null)
                {
                    if (card.TargetType == EmotionTargetType.All || card.TargetType == EmotionTargetType.AllIncludingEnemy)
                    {
                        foreach (BattleUnitModel model in BattleObjectManager.instance.GetList(Faction.Player))
                        {
                            pickUp.OnPickUp(model);
                            if (LogueBookModels.playersPick.ContainsKey(model.UnitData.unitData))
                                LogueBookModels.playersPick[model.UnitData.unitData].Add(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(card), card.id));
                        }
                        if (card.TargetType == EmotionTargetType.AllIncludingEnemy)
                        {
                            foreach (BattleUnitModel model in BattleObjectManager.instance.GetList(Faction.Enemy))
                                pickUp.OnPickUp(model);
                        }
                    }
                    if (target != null)
                    {
                        pickUp.OnPickUp(target);
                        if (LogueBookModels.playersPick.ContainsKey(target.UnitData.unitData))
                            LogueBookModels.playersPick[target.UnitData.unitData].Add(new LorId(LogLikeMod.GetPickUpXmlWorkShopId_Passive(card), card.id));
                    }
                    pickUp.OnPickUp();
                    Singleton<LogueSaveManager>.Instance.AddToObtainCount((object)pickUp);
                }
                switch (RewardingModel.rewardFlag)
                {
                    case RewardingModel.RewardFlag.PassiveReward:
                        LogLikeMod.rewards_passive.RemoveAt(0);
                        break;
                    case RewardingModel.RewardFlag.RewardInStage:
                        LogLikeMod.rewards_InStage.RemoveAt(0);
                        if (Singleton<ShopManager>.Instance.curshop != null && LogLikeMod.rewards_InStage.Count == 0)
                            Singleton<ShopManager>.Instance.curshop.HideShop();
                        if (Singleton<MysteryManager>.Instance.curMystery == null || !(Singleton<MysteryManager>.Instance.curMystery is MysteryModel_Rest))
                            break;
                        (Singleton<MysteryManager>.Instance.curMystery as MysteryModel_Rest).HideRest();
                        break;
                }
            }
            else
                orig(self, card, target);
        }

        public static bool StageController_CheckStoryAfterBattle(StageController __instance)
        {
            return !LogLikeMod.CheckStage() || LogLikeMod.curChStageStep != 0 || true;
        }

        public static bool StageController_RoundStartPhase_System(StageController __instance)
        {
            if (!LogLikeMod.GetFieldValue<bool>((object)__instance, "_bCalledRoundStart_system") && LogLikeMod.CheckStage())
                Singleton<GlobalLogueEffectManager>.Instance.OnRoundStart(__instance);
            return true;
        }

        public static void StageController_GameOver(bool iswin, bool isbackbutton = false)
        {
        }

        public static bool StageController_StartParrying(
          StageController __instance,
          BattlePlayingCardDataInUnitModel cardA,
          BattlePlayingCardDataInUnitModel cardB)
        {
            if (cardA.owner.passiveDetail.HasPassive<PassiveAbility_ShopPassiveMook4>() && cardB is BattleKeepedCardDataInUnitModel)
            {
                LogLikeMod.SetStagePhase(__instance, StageController.StagePhase.ExecuteOneSideAction);
                cardA.owner.turnState = BattleUnitTurnState.DOING_ACTION;
                cardA.target.turnState = BattleUnitTurnState.DOING_ACTION;
                Singleton<BattleOneSidePlayManager>.Instance.StartOneSidePlay(cardA);
                return false;
            }
            if (!cardB.owner.passiveDetail.HasPassive<PassiveAbility_ShopPassiveMook4>() || !(cardA is BattleKeepedCardDataInUnitModel))
                return true;
            LogLikeMod.SetStagePhase(__instance, StageController.StagePhase.ExecuteOneSideAction);
            cardB.owner.turnState = BattleUnitTurnState.DOING_ACTION;
            cardB.target.turnState = BattleUnitTurnState.DOING_ACTION;
            Singleton<BattleOneSidePlayManager>.Instance.StartOneSidePlay(cardB);
            return false;
        }

        public void StageController_InitStageByInvitation(
          Action<StageController, StageClassInfo, List<LorId>> orig,
          StageController self,
          StageClassInfo stage,
          List<LorId> books = null)
        {
            orig(self, stage, books);
        }

        public void StageController_OnEnemyDropBookForAdded(
          Action<StageController, DropBookDataForAddedReward> orig,
          StageController self,
          DropBookDataForAddedReward data)
        {
            if (LogLikeMod.CheckStage(true))
            {
                DropBookXmlInfo data1 = Singleton<DropBookXmlList>.Instance.GetData(data.id);
                LogLikeMod.rewards.Add(data1);
            }
            else
                orig(self, data);
        }

        public void StageController_EndBattle(Action<StageController> orig, StageController self)
        {
            if (LogLikeMod.CheckStage(true) && self.Phase != StageController.StagePhase.EndBattle)
            {
                LogLikeMod.EndBattle = false;
                LogLikeMod.SetStagePhase(self, StageController.StagePhase.EndBattle);
            }
            else
                orig(self);
        }

        public static void StageController_ActivateStartBattleEffectPhase(StageController __instance)
        {
            foreach (BattlePlayingCardDataInUnitModel card in ModdingUtils.GetFieldValue<List<BattlePlayingCardDataInUnitModel>>("_allCardList", (object)__instance))
                Singleton<GlobalLogueEffectManager>.Instance.OnStartBattle_AfterCardSet(card);
        }

        public static void StageController_OnFixedUpdateLate()
        {
            if (LogLikeMod.CheckStage())
                LogLikeMod.PauseBool = RewardingModel.RewardInStage();
            else
                LogLikeMod.PauseBool = false;
        }

        public static void StageController_ApplyLibrarianCardPhase()
        {
        }

        public void StageController_OnUpdate(
          Action<StageController, float> orig,
          StageController self,
          float deltatTime)
        {
            orig(self, deltatTime);
        }

        public static void StageController_ClearBattle(StageController __instance)
        {
            if (!LogLikeMod.CheckStage(true) || Environment.StackTrace.Contains("UIBgScreenChangeAnim"))
                return;
            if (!LogLikeMod.purpleexcept)
            {
                if (LogLikeMod.AddPlayer)
                    LogueBookModels.AddSubPlayer();
                if (LogLikeMod.RecoverPlayers)
                {
                    foreach (UnitBattleDataModel unitBattleDataModel in LogueBookModels.playerBattleModel)
                    {
                        unitBattleDataModel.isDead = false;
                        unitBattleDataModel.Refreshhp();
                        unitBattleDataModel.emotionDetail.Reset();
                    }
                }
            }
            LogLikeMod.AddPlayer = false;
            LogLikeMod.RecoverPlayers = false;
            if (!(LogLikeMod.curstageid != (LorId)null))
                return;
            LoguePlayDataSaver.SavePlayData();
            LoguePlayDataSaver.RemoveFlashData();
            StageModel stageModel = __instance.GetStageModel();
            if ((stageModel.GetFrontAvailableWave() == null ? 1 : (stageModel.GetFrontAvailableFloor() == null ? 1 : 0)) != 0)
                LoguePlayDataSaver.RemovePlayerData();
            Singleton<StagesXmlList>.Instance.RestoreToDefault();
            Singleton<RewardPassivesList>.Instance.RestoreToDefault();
            Singleton<MysteryXmlList>.Instance.RestoreToDefault();
            Singleton<CardDropValueList>.Instance.RestoreToDefault();
        }

        public void StageController_EndBattlePhase(
          Action<StageController, float> orig,
          StageController self,
          float deltaTime)
        {
            if (LogLikeMod.CheckStage(true))
            {
                if (!RewardingModel.RewardClearStage(self))
                    return;
                if (!LogLikeMod.EndBattle)
                {
                    self.EndBattle();
                    Singleton<GlobalLogueEffectManager>.Instance.OnEndBattle();
                    LogLikeMod.EndBattle = true;
                    foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList(Faction.Player))
                    {
                        battleUnitModel.emotionDetail.SetEmotionLevel(0);
                        battleUnitModel.emotionDetail.PassiveList.Clear();
                    }
                }
                orig(self, deltaTime);
            }
            else
                orig(self, deltaTime);
        }

        public static void ResetNextStage()
        {
            LogLikeMod.nextlist = new List<EmotionCardXmlInfo>();
            if (LogLikeMod.curstagetype == StageType.Boss)
            {
                if (LogLikeMod.curchaptergrade != ChapterGrade.Grade6 && LogueBookModels.RemainStageList.ContainsKey(LogLikeMod.curchaptergrade + 1))
                    LogLikeMod.nextlist = LogueBookModels.GetNextList(LogLikeMod.curchaptergrade + 1, true);
                else
                    LogLikeMod.nextlist.Clear();
                LogLikeMod.curChStageStep = 0;
            }
            else
                LogLikeMod.nextlist = LogueBookModels.GetNextList(LogLikeMod.curchaptergrade, LogLikeMod.curstagetype == StageType.Start);
        }

        public void StageController_StartBattle(Action<StageController> orig, StageController self)
        {
            if (LogLikeMod.CheckStage())
            {
                LogLikeMod.BattleMoneyUI.Active();
                LogueBookModels.selectedEmotion = new List<RewardPassiveInfo>();
                LogueBookModels.EmotionCardList = new List<RewardPassiveInfo>();
                foreach (KeyValuePair<int, int> keyValuePair in LogueBookModels.EmotionSelectDic)
                    LogueBookModels.EmotionCardList.AddRange((IEnumerable<RewardPassiveInfo>)Singleton<LogCreatureTabPanel>.Instance.GetCreaturePickUpByIndex(keyValuePair.Key, keyValuePair.Value));
                LogLikeMod.curemotion = 0;
                LogLikeMod.purpleexcept = false;
                LogLikeMod.rewards_InStage = new List<RewardInfo>();
                LogLikeMod.rewards = new List<DropBookXmlInfo>();
                LogLikeMod.rewards_passive = new List<RewardInfo>();
                LogLikeMod.rewardsMystery = new List<LorId>();
                //if (!LogLikeMod.Debugging)
                //  ;
                LogLikeMod.PlayerEquipOrders = new List<EquipChangeOrder>();
                if (LogLikeMod.curstagetype == StageType.Normal)
                {
                    if (LogLikeMod.NormalRewardCool <= 0)
                    {
                        LogLikeMod.rewards_passive.Add(RewardInfo.GetCurChapterCommonReward(LogLikeMod.curchaptergrade));
                        LogLikeMod.NormalRewardCool = 0;
                    }
                    else
                        --LogLikeMod.NormalRewardCool;
                }
                if (LogLikeMod.curstagetype == StageType.Elite)
                    LogLikeMod.rewards_passive.Add(RewardInfo.GetCurChapterEliteReward(LogLikeMod.curchaptergrade));
                if (LogLikeMod.curChStageStep != 0 && LogLikeMod.curstagetype == StageType.Boss && LogueBookModels.RemainStageList.ContainsKey(LogLikeMod.curchaptergrade + 1))
                {
                    RewardInfo chapterBossReward = RewardInfo.GetCurChapterBossReward(LogLikeMod.curchaptergrade);
                    if (chapterBossReward != null)
                        LogLikeMod.rewards_passive.Add(chapterBossReward);
                }
                LogLikeMod.ResetNextStage();
                if (LogLikeMod.curstagetype == StageType.Boss)
                {
                    if (LogLikeMod.curchaptergrade != ChapterGrade.Grade6 && LogueBookModels.RemainStageList.ContainsKey(LogLikeMod.curchaptergrade + 1))
                        LogLikeMod.nextlist = LogueBookModels.GetNextList(LogLikeMod.curchaptergrade + 1, true);
                    else
                        LogLikeMod.nextlist.Clear();
                }
                else
                    LogLikeMod.nextlist = LogueBookModels.GetNextList(LogLikeMod.curchaptergrade, LogLikeMod.curstagetype == StageType.Start);
                Singleton<GlobalLogueEffectManager>.Instance.OnStartBattle();
                Singleton<GlobalLogueEffectManager>.Instance.UpdateSprites();
            }
            else
                LogLikeMod.ResetUIs();
            orig(self);
            if (!LogLikeMod.CheckStage())
                return;
            Singleton<GlobalLogueEffectManager>.Instance.OnStartBattleAfter();
        }

        public void StageController_CreateLibrarianUnit(
          Action<StageController, SephirahType> orig,
          StageController self,
          SephirahType sephirah)
        {
            if (LogLikeMod.CheckStage(true))
            {
                BattleTeamModel battleTeamModel = (BattleTeamModel)typeof(StageController).GetField("_librarianTeam", AccessTools.all).GetValue((object)self);
                int num = 0;
                foreach (UnitBattleDataModel unitBattleData in LogueBookModels.playerBattleModel)
                {
                    StageLibraryFloorModel floor = self.GetStageModel().GetFloor(sephirah);
                    UnitDataModel unitData = unitBattleData.unitData;
                    BattleUnitModel defaultUnit = BattleObjectManager.CreateDefaultUnit(Faction.Player);
                    defaultUnit.index = num;
                    defaultUnit.grade = unitData.grade;
                    defaultUnit.formation = floor.GetFormationPosition(defaultUnit.index);
                    defaultUnit.SetUnitData(unitBattleData);
                    defaultUnit.OnCreated();
                    battleTeamModel.AddUnit(defaultUnit);
                    BattleObjectManager.instance.RegisterUnit(defaultUnit);
                    defaultUnit.passiveDetail.OnUnitCreated();
                    defaultUnit.SetDeadSceneBlock(true);
                    ++num;
                    Singleton<GlobalLogueEffectManager>.Instance.OnCreateLibrarian(defaultUnit);
                }
                Singleton<GlobalLogueEffectManager>.Instance.OnCreateLibrarians();
            }
            else
                orig(self, sephirah);
        }

        public bool StageController_RoundEndPhase_ChoiceEmotionCard(
          Func<StageController, bool> orig,
          StageController self)
        {
            if (!LogLikeMod.CheckStage(true))
                return orig(self);
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (!alive.view.FormationReturned)
                    return false;
            }
            return RewardingModel.EmotionChoice();
        }

        public static PickUpModelBase FindPickUp(string script)
        {
            if (LogLikeMod.FindPickUpCache == null)
                LogLikeMod.FindPickUpCache = new Dictionary<string, System.Type>();
            if (LogLikeMod.FindPickUpCache.ContainsKey(script))
                return Activator.CreateInstance(LogLikeMod.FindPickUpCache[script]) as PickUpModelBase;
            foreach (Assembly assem in LogLikeMod.GetAssemList())
            {
                foreach (System.Type type in assem.GetTypes())
                {
                    if (type.Name == "PickUpModel_" + script.Trim())
                    {
                        LogLikeMod.FindPickUpCache[script] = type;
                        return Activator.CreateInstance(type) as PickUpModelBase;
                    }
                }
            }
            return (PickUpModelBase)null;
        }

        public class PreLoader : MonoBehaviour
        {
            public bool LoadingArtWork;
            public Stack<System.Type> UpgradeInfos;
            public Stack<AssetBundle> AssetBundles;
            public Stack<KeyValuePair<string, string>> NameAndPathDic;

            public void StartSoundPreload()
            {
                this.StartCoroutine(Singleton<LogSoundEffectManager>.Instance.PreloadAudioClip());
            }

            public void StartCreatureTabPreload()
            {
                this.StartCoroutine(Singleton<LogCreatureTabPanel>.Instance.PreloadImages());
            }

            public void StartUpgradeInfoPreload()
            {
            }

            public void GetArtWorks(DirectoryInfo dir)
            {
                if (dir.GetDirectories().Length != 0)
                {
                    foreach (DirectoryInfo directory in dir.GetDirectories())
                        this.GetArtWorks(directory);
                }
                foreach (System.IO.FileInfo file in dir.GetFiles())
                    this.NameAndPathDic.Push(new KeyValuePair<string, string>(file.FullName, Path.GetFileNameWithoutExtension(file.FullName)));
            }

            public void StartArtWorkPreLoad()
            {
                DirectoryInfo dir = new DirectoryInfo(LogLikeMod.path + "/ArtWork");
                this.NameAndPathDic = new Stack<KeyValuePair<string, string>>();
                this.GetArtWorks(dir);
                this.Log($"Detect {this.NameAndPathDic.Count.ToString()} ArtWorks");
                this.Log("Start PreLoad ArtWork : " + DateTime.Now.ToString());
                this.StartCoroutine(this.ArtWorkPreLoading());
                DirectoryInfo directoryInfo = new DirectoryInfo(LogLikeMod.path + "/AssetBundle");
            }

            public void GetAssetBundles(DirectoryInfo dir)
            {
                if (dir.GetDirectories().Length != 0)
                {
                    foreach (DirectoryInfo directory in dir.GetDirectories())
                        this.GetAssetBundles(directory);
                }
                foreach (System.IO.FileInfo file in dir.GetFiles())
                {
                    AssetBundle assetBundle = AssetBundle.LoadFromFile(file.FullName);
                    assetBundle.name = Path.GetFileNameWithoutExtension(file.FullName);
                    this.AssetBundles.Push(assetBundle);
                }
            }

            public void StartAssetBundlePreload()
            {
                this.AssetBundles = new Stack<AssetBundle>();
                DirectoryInfo dir = new DirectoryInfo(LogLikeMod.path + "/AssetBundle");
                this.Log("Start Load AssetBundle : " + DateTime.Now.ToString());
                this.GetAssetBundles(dir);
                Singleton<LogAssetBundleManager>.Instance.SetBundles(this.AssetBundles.ToList<AssetBundle>());
                this.Log("End Load AssetBundle : " + DateTime.Now.ToString());
                this.StartCoroutine(this.AssetBundlePreLoading());
            }

            public IEnumerator AssetBundlePreLoading()
            {
                DateTime now = DateTime.Now;
                this.Log("Start PreLoad AssetBundle : " + now.ToString());
                while (this.AssetBundles.Count > 0)
                {
                    AssetBundle bundle = this.AssetBundles.Pop();
                    string[] strings = bundle.GetAllAssetNames();
                    string[] strArray = strings;
                    for (int index = 0; index < strArray.Length; ++index)
                    {
                        string name = strArray[index];
                        GameObject Gobj = bundle.LoadAsset<GameObject>(name);
                        if ((UnityEngine.Object)Gobj != (UnityEngine.Object)null)
                        {
                            LogAssetBundleManager.GameObjectBundleCache cache = new LogAssetBundleManager.GameObjectBundleCache()
                            {
                                BundleName = bundle.name,
                                objname = name,
                                obj = Gobj
                            };
                            Singleton<LogAssetBundleManager>.Instance.GObjList.Add(cache);
                            cache = (LogAssetBundleManager.GameObjectBundleCache)null;
                        }
                        yield return (object)new WaitForEndOfFrame();
                        Gobj = (GameObject)null;
                        name = (string)null;
                    }
                    strArray = (string[])null;
                    bundle = (AssetBundle)null;
                    strings = (string[])null;
                }
                now = DateTime.Now;
                this.Log("End PreLoad AssetBundle : " + now.ToString());
            }

            public IEnumerator ArtWorkPreLoading()
            {
                while (this.NameAndPathDic.Count > 0)
                {
                    KeyValuePair<string, string> nam = this.NameAndPathDic.Pop();
                    Texture2D texture2D = new Texture2D(2, 2);
                    byte[] bytes = File.ReadAllBytes(nam.Key);
                    texture2D.LoadImage(bytes);
                    Sprite value = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.0f, 0.0f), 100f, 0U, SpriteMeshType.FullRect);
                    LogLikeMod.ArtWorks.dic[nam.Key] = value;
                    yield return (object)new WaitForEndOfFrame();
                    nam = new KeyValuePair<string, string>();
                    texture2D = (Texture2D)null;
                    bytes = (byte[])null;
                    value = (Sprite)null;
                }
                this.Log("End PreLoad ArtWork : " + DateTime.Now.ToString());
            }
        }

        public class UIActiveChecker : MonoBehaviour
        {
            public void Update()
            {
                int childCount = this.gameObject.transform.childCount;
                this.gameObject.GetComponent<GraphicRaycaster>();
                for (int index = 0; index < childCount; ++index)
                {
                    if (this.gameObject.transform.GetChild(index).gameObject.activeSelf)
                    {
                        this.gameObject.SetActive(true);
                        return;
                    }
                }
                this.gameObject.SetActive(false);
            }
        }

        public class LoadList
        {
            [XmlElement("Dll")]
            public List<string> ReadDll;
        }

        public class ModLoader
        {
            public static void OnInitializeMod(string path, string modid)
            {
                LogLikeMod.LoadList loadList = (LogLikeMod.LoadList)null;
                if (!File.Exists(path + "/AssemList.xml"))
                    return;
                using (StreamReader streamReader = new StreamReader(path + "/AssemList.xml"))
                    loadList = new XmlSerializer(typeof(LogLikeMod.LoadList)).Deserialize((TextReader)streamReader) as LogLikeMod.LoadList;
                List<string> stringList = new List<string>();
                foreach (string str in loadList.ReadDll)
                {
                    if (File.Exists($"{path}/Roguedlls/{str}.dll"))
                        stringList.Add($"{path}/Roguedlls/{str}.dll");
                }
                Dictionary<string, BattleCardAbilityDesc> abilityText;
                LogLikeMod.ModLoader.LoadAllAssembly(modid + "<Loader>", stringList.ToArray(), out abilityText);
                Singleton<BattleCardAbilityDescXmlList>.Instance.AddByMode(modid, abilityText);
            }

            public static void LoadTypesFromAssembly(Assembly assembly)
            {
                foreach (System.Type type in assembly.GetTypes())
                {
                    string name = type.Name;
                    if (type.IsSubclassOf(typeof(DiceCardSelfAbilityBase)) && name.StartsWith("DiceCardSelfAbility_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>((object)Singleton<AssemblyManager>.Instance, "_diceCardSelfAbilityDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
            (object) name.Substring("DiceCardSelfAbility_".Length),
            (object) type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(DiceCardAbilityBase)) && name.StartsWith("DiceCardAbility_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>((object)Singleton<AssemblyManager>.Instance, "_diceCardAbilityDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
            (object) name.Substring("DiceCardAbility_".Length),
            (object) type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(BehaviourActionBase)) && name.StartsWith("BehaviourAction_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>((object)Singleton<AssemblyManager>.Instance, "_behaviourActionDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
            (object) name.Substring("BehaviourAction_".Length),
            (object) type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(PassiveAbilityBase)) && name.StartsWith("PassiveAbility_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>((object)Singleton<AssemblyManager>.Instance, "_passiveAbilityDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
            (object) name.Substring("PassiveAbility_".Length),
            (object) type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(DiceCardPriorityBase)) && name.StartsWith("DiceCardPriority_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>((object)Singleton<AssemblyManager>.Instance, "_diceCardPriorityDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
            (object) name.Substring("DiceCardPriority_".Length),
            (object) type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(EnemyUnitAggroSetter)) && name.StartsWith("EnemyUnitAggroSetter_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>((object)Singleton<AssemblyManager>.Instance, "_enemyUnitAggroSetterDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
            (object) name.Substring("EnemyUnitAggroSetter_".Length),
            (object) type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(EnemyTeamStageManager)) && name.StartsWith("EnemyTeamStageManager_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>((object)Singleton<AssemblyManager>.Instance, "_enemyTeamStageManagerDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
            (object) name.Substring("EnemyTeamStageManager_".Length),
            (object) type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(EnemyUnitTargetSetter)) && name.StartsWith("EnemyUnitTargetSetter_"))
                    {
                        object fieldValue = LogLikeMod.GetFieldValue<object>((object)Singleton<AssemblyManager>.Instance, "_enemyUnitTargetSetterDict");
                        fieldValue.GetType().GetMethod("Add", AccessTools.all).Invoke(fieldValue, new object[2]
                        {
            (object) name.Substring("EnemyUnitTargetSetter_".Length),
            (object) type
                        });
                    }
                    else if (type.IsSubclassOf(typeof(ModInitializer)))
                        (Activator.CreateInstance(type) as ModInitializer).OnInitializeMod();
                }
            }

            public static void LoadAllAssembly(
              string uid,
              string[] filenames,
              out Dictionary<string, BattleCardAbilityDesc> abilityText)
            {
                Dictionary<string, List<Assembly>> fieldValue = LogLikeMod.GetFieldValue<Dictionary<string, List<Assembly>>>((object)Singleton<AssemblyManager>.Instance, "_assemblyDict");
                abilityText = new Dictionary<string, BattleCardAbilityDesc>();
                List<Assembly> collection = new List<Assembly>();
                for (int index = 0; index < filenames.Length; ++index)
                {
                    string filename = filenames[index];
                    try
                    {
                        List<Assembly> assemblyList = new List<Assembly>((IEnumerable<Assembly>)AppDomain.CurrentDomain.GetAssemblies());
                        Assembly assembly = Assembly.LoadFile(filename);
                        if (assembly != (Assembly)null)
                        {
                            if (!assemblyList.Contains(assembly))
                            {
                                foreach (System.Type type in assembly.GetTypes())
                                {
                                    string key = type.Name;
                                    bool flag = false;
                                    if (type.IsSubclassOf(typeof(DiceCardAbilityBase)))
                                    {
                                        if (key.StartsWith("DiceCardAbility_"))
                                            key = key.Substring("DiceCardAbility_".Length);
                                        flag = true;
                                    }
                                    if (type.IsSubclassOf(typeof(DiceCardSelfAbilityBase)))
                                    {
                                        if (key.StartsWith("DiceCardSelfAbility_"))
                                            key = key.Substring("DiceCardSelfAbility_".Length);
                                        flag = true;
                                    }
                                    if (flag)
                                    {
                                        FieldInfo field = type.GetField("Desc");
                                        if (field != (FieldInfo)null)
                                        {
                                            string str = field.GetValue((object)null) as string;
                                            if (!string.IsNullOrEmpty(str) && !abilityText.ContainsKey(key))
                                                abilityText.Add(key, new BattleCardAbilityDesc()
                                                {
                                                    id = key,
                                                    desc = new List<string>() { str }
                                                });
                                        }
                                    }
                                }
                                collection.Add(assembly);
                                LogLikeMod.LogModAssemblys.Add(assembly);
                            }
                        }
                        else
                            typeof(LogLikeMod.ModLoader).Log("load fail : " + filename);
                    }
                    catch (Exception ex)
                    {
                        typeof(LogLikeMod.ModLoader).LogError(ex);
                    }
                }
                foreach (Assembly assembly in collection)
                    LogLikeMod.ModLoader.LoadTypesFromAssembly(assembly);
                if (collection.Count <= 0)
                    return;
                if (!fieldValue.ContainsKey(uid))
                    fieldValue.Add(uid, collection);
                else
                    fieldValue[uid].AddRange((IEnumerable<Assembly>)collection);
            }
        }

        public class CacheDic<Tkey, TValue>
        {
            public LogLikeMod.CacheDic<Tkey, TValue>.getdele del;
            public Dictionary<Tkey, TValue> dic;

            public bool ContainsKey(Tkey key)
            {
                if (this.dic.ContainsKey(key))
                    return true;
                TValue obj = this.del(key);
                if ((object)obj == null)
                    return false;
                this.dic[key] = obj;
                return true;
            }

            public CacheDic(LogLikeMod.CacheDic<Tkey, TValue>.getdele del)
            {
                this.dic = new Dictionary<Tkey, TValue>();
                this.del = del;
            }

            public TValue this[Tkey key]
            {
                get
                {
                    if (this.dic.ContainsKey(key))
                    {
                        if ((object)this.dic[key] is GameObject)
                            ((object)this.dic[key] as GameObject).SetActive(true);
                        return this.dic[key];
                    }
                    this.dic[key] = this.del(key);
                    return this.dic[key];
                }
                set => this.dic[key] = value;
            }

            public void PreLoading(Tkey key) => this.dic[key] = this.del(key);

            public delegate TValue getdele(Tkey key);
        }

        public class LogUIBookSlot : MonoBehaviour
        {
            public CanvasGroup cg;
            public UICustomSelectable selectable;
            public Image Frame;
            public Image FrameGlow;
            public Image Icon;
            public Image IconGlow;
            public TextMeshProUGUI BookName;
            public int originSiblingIdx = -1;
            public int stringAscendidx = -1;
            public LorId _bookId = LorId.None;
            public DropBookXmlInfo bookInfo;
            public bool isDisabled;
            public GameObject bookNumRoot;
            public Image bookNumBg;
            public TextMeshProUGUI txt_bookNum;
            public GameObject ob_tutorialhighlight;

            public static LogLikeMod.LogUIBookSlot SlotCopying()
            {
                UIInvitationDropBookSlot bookSlot = (UI.UIController.Instance.GetUIPanel(UIPanelType.Invitation) as UIInvitationPanel).InvCenterBookListPanel.BookSlotList[0] as UIInvitationDropBookSlot;
                LogLikeMod.LogUIBookSlot original = bookSlot.gameObject.AddComponent<LogLikeMod.LogUIBookSlot>();
                original.cg = (CanvasGroup)typeof(UIBookSlot).GetField("cg", AccessTools.all).GetValue((object)bookSlot);
                original.selectable = (UICustomSelectable)typeof(UIBookSlot).GetField("selectable", AccessTools.all).GetValue((object)bookSlot);
                original.Frame = (Image)typeof(UIBookSlot).GetField("Frame", AccessTools.all).GetValue((object)bookSlot);
                original.FrameGlow = (Image)typeof(UIBookSlot).GetField("FrameGlow", AccessTools.all).GetValue((object)bookSlot);
                original.Icon = (Image)typeof(UIBookSlot).GetField("Icon", AccessTools.all).GetValue((object)bookSlot);
                original.IconGlow = (Image)typeof(UIBookSlot).GetField("IconGlow", AccessTools.all).GetValue((object)bookSlot);
                original.BookName = (TextMeshProUGUI)typeof(UIBookSlot).GetField("BookName", AccessTools.all).GetValue((object)bookSlot);
                original.bookNumRoot = (GameObject)typeof(UIInvitationDropBookSlot).GetField("bookNumRoot", AccessTools.all).GetValue((object)bookSlot);
                original.bookNumBg = (Image)typeof(UIInvitationDropBookSlot).GetField("bookNumBg", AccessTools.all).GetValue((object)bookSlot);
                original.txt_bookNum = (TextMeshProUGUI)typeof(UIInvitationDropBookSlot).GetField("txt_bookNum", AccessTools.all).GetValue((object)bookSlot);
                original.ob_tutorialhighlight = (GameObject)typeof(UIInvitationDropBookSlot).GetField("ob_tutorialhighlight", AccessTools.all).GetValue((object)bookSlot);
                LogLikeMod.LogUIBookSlot logUiBookSlot = UnityEngine.Object.Instantiate<LogLikeMod.LogUIBookSlot>(original);
                UnityEngine.Object.Destroy((UnityEngine.Object)logUiBookSlot.gameObject.GetComponent<UIInvitationDropBookSlot>());
                UnityEngine.Object.Destroy((UnityEngine.Object)original);
                logUiBookSlot.selectable.SubmitEvent.RemoveAllListeners();
                logUiBookSlot.selectable.SubmitEvent.AddListener(new UnityAction<BaseEventData>(logUiBookSlot.OnPointerClick));
                logUiBookSlot.selectable.SelectEvent.RemoveAllListeners();
                logUiBookSlot.selectable.SelectEvent.AddListener(new UnityAction<BaseEventData>(logUiBookSlot.OnPointerEnter));
                logUiBookSlot.selectable.DeselectEvent.RemoveAllListeners();
                logUiBookSlot.selectable.DeselectEvent.AddListener(new UnityAction<BaseEventData>(logUiBookSlot.OnPointerExit));
                logUiBookSlot.selectable.XEvent.RemoveAllListeners();
                return logUiBookSlot;
            }

            public void Initialized()
            {
                this._bookId = LorId.None;
                this.bookInfo = (DropBookXmlInfo)null;
                this.isDisabled = false;
            }

            public void SetData_DropBook(LorId bookId, int value)
            {
                if (!this.gameObject.activeSelf)
                    this.gameObject.SetActive(true);
                this.isDisabled = false;
                this._bookId = bookId;
                this.bookInfo = Singleton<DropBookXmlList>.Instance.GetData(this._bookId);
                if (this.bookInfo == null)
                {
                    this.SetActivatedSlot(false);
                    Debug.LogError((object)"dropbook Info null Error");
                }
                else
                {
                    this.SetActivatedSlot(true);
                    this.BookName.text = this.bookInfo.Name;
                    if ((UnityEngine.Object)this.Icon != (UnityEngine.Object)null)
                        this.Icon.sprite = this.bookInfo.bookIcon;
                    if ((UnityEngine.Object)this.IconGlow != (UnityEngine.Object)null)
                        this.IconGlow.sprite = this.bookInfo.bookIconGlow;
                    this.SetHighlighted(false);
                    if (!((UnityEngine.Object)this.bookNumRoot != (UnityEngine.Object)null))
                        return;
                    if (!this.bookNumRoot.activeSelf)
                        this.bookNumRoot.SetActive(true);
                    this.txt_bookNum.text = "x" + value.ToString();
                }
            }

            public void SetEmptyViewSlot()
            {
                this.BookName.text = TextDataModel.GetText("ui_book_bookname_emptybook");
                if ((UnityEngine.Object)this.Icon != (UnityEngine.Object)null)
                {
                    Image icon = this.Icon;
                    UISpriteDataManager instance = UISpriteDataManager.instance;
                    icon.sprite = (UnityEngine.Object)instance != (UnityEngine.Object)null ? instance.GetStoryIcon("None").icon : (Sprite)null;
                }
                if ((UnityEngine.Object)this.IconGlow != (UnityEngine.Object)null)
                {
                    Image iconGlow = this.IconGlow;
                    UISpriteDataManager instance = UISpriteDataManager.instance;
                    iconGlow.sprite = (UnityEngine.Object)instance != (UnityEngine.Object)null ? instance.GetStoryIcon("None").iconGlow : (Sprite)null;
                }
                this.SetDisabled(true);
                GameObject bookNumRoot = this.bookNumRoot;
                if ((UnityEngine.Object)bookNumRoot != (UnityEngine.Object)null)
                    bookNumRoot.SetActive(false);
                if (!this.ob_tutorialhighlight.activeSelf)
                    return;
                this.ob_tutorialhighlight.SetActive(false);
            }

            public void SetAleadyHighlighted()
            {
                this.SetGlowColor(UIColorManager.Manager.GetUIColor(UIColor.Highlighted));
                this.SetColor(UIColorManager.Manager.GetUIColor(UIColor.Default));
                if (!this.ob_tutorialhighlight.activeSelf)
                    return;
                this.ob_tutorialhighlight.SetActive(false);
            }

            public void SetHighlighted(bool on)
            {
                Color c = on ? (this.isDisabled ? UIColorManager.Manager.GetUIColor(UIColor.Disabled) : UIColorManager.Manager.GetUIColor(UIColor.Highlighted)) : UIColorManager.Manager.GetUIColor(UIColor.Default);
                this.SetColor(c);
                this.SetGlowColor(c);
                if (!on || !this.ob_tutorialhighlight.activeSelf)
                    return;
                this.ob_tutorialhighlight.SetActive(false);
            }

            public void SetColor(Color c)
            {
                this.Frame.color = c;
                this.BookName.color = c;
                this.Icon.color = Color.white;
                this.bookNumBg.color = c;
                this.txt_bookNum.color = c;
            }

            public void OnPointerEnter(BaseEventData eventData)
            {
                if (this.isDisabled)
                    return;
                UISoundManager.instance.PlayEffectSound(UISoundType.Ui_BookOver);
                this.SetHighlighted(true);
            }

            public void OnPointerExit(BaseEventData eventData)
            {
                if (this.isDisabled)
                    return;
                this.SetHighlighted(false);
            }

            public void OnPointerClick(BaseEventData eventData)
            {
                if (this.isDisabled)
                    UISoundManager.instance.PlayEffectSound(UISoundType.Card_Lock);
                else
                    UISoundManager.instance.PlayEffectSound(UISoundType.Ui_EnemyButton);
            }

            public void OnScroll(BaseEventData eventData)
            {
            }

            public void OnXEvent()
            {
            }

            public void OnCancel()
            {
            }

            public DropBookXmlInfo DropBookInfo => this.bookInfo;

            public LorId BookId => this._bookId;

            public void SetActivatedSlot(bool on)
            {
                if (on)
                {
                    this.cg.alpha = 1f;
                    this.cg.interactable = true;
                    this.cg.blocksRaycasts = true;
                    if (!((UnityEngine.Object)this.selectable != (UnityEngine.Object)null))
                        return;
                    this.selectable.interactable = true;
                }
                else
                {
                    this.cg.alpha = 0.0f;
                    this.cg.interactable = false;
                    this.cg.blocksRaycasts = false;
                    if ((UnityEngine.Object)this.selectable != (UnityEngine.Object)null)
                        this.selectable.interactable = false;
                    this._bookId = LorId.None;
                    this.bookInfo = (DropBookXmlInfo)null;
                }
            }

            public bool GetActiveState() => (double)this.cg.alpha == 1.0;

            public void SetGlowColor(Color c)
            {
                this.FrameGlow.color = c;
                if ((UnityEngine.Object)this.IconGlow != (UnityEngine.Object)null)
                    this.IconGlow.color = c;
                TextMeshProMaterialSetter component = this.BookName.GetComponent<TextMeshProMaterialSetter>();
                if ((UnityEngine.Object)component != (UnityEngine.Object)null)
                    component.underlayColor = c;
                component.enabled = false;
                component.enabled = true;
            }

            public void SetDisabled(bool on)
            {
                this.isDisabled = on;
                if (this.isDisabled)
                {
                    this.SetColor(UIColorManager.Manager.GetUIColor(UIColor.Disabled));
                    this.SetGlowColor(UIColorManager.Manager.GetUIColor(UIColor.Disabled));
                    this.Icon.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                }
                else
                {
                    this.SetColor(UIColorManager.Manager.GetUIColor(UIColor.Default));
                    this.SetGlowColor(UIColorManager.Manager.GetUIColor(UIColor.Default));
                }
            }
        }

        public class LogUISettingInvenEquipPageSlot : UIOriginEquipPageSlot
        {
            [Header("---Child---")]
            [SerializeField]
            public UISettingEquipPageScrollList listRoot;
            [Header("Equip Character Sprite")]
            [SerializeField]
            public Image img_equipFrame;
            [SerializeField]
            public GameObject ob_equipRoot;
            [SerializeField]
            public CanvasGroup cg_equiproot;
            [SerializeField]
            public FaceEditor faceEditor;
            [Header("Operating Panel")]
            [SerializeField]
            public GameObject ob_OperatingPanel;
            [SerializeField]
            public CanvasGroup cg_operatingPanel;
            [Header("BookMark Button")]
            [SerializeField]
            public UICustomGraphicObject button_BookMark;
            [SerializeField]
            public TextMeshProUGUI txt_bookmarkButton;
            [SerializeField]
            public Image img_bookmarkbuttonIcon;
            [Header("Equip Button")]
            [SerializeField]
            public UICustomGraphicObject button_Equip;
            [SerializeField]
            public TextMeshProUGUI txt_equipButton;
            [SerializeField]
            public Image img_equipbuttonIcon;
            [SerializeField]
            public UICustomGraphicObject button_EmptyDeck;
            [Header("Block Frame")]
            [SerializeField]
            public GameObject ob_blockFrame;
            [SerializeField]
            public Image img_SepIcon;
            public bool isBlock;

            public override void Initialized()
            {
                base.Initialized();
                this.SetActiveOperatinPanel(false);
                if (!((UnityEngine.Object)this.selectable == (UnityEngine.Object)null))
                    return;
                this.selectable = this.GetComponent<UICustomSelectable>();
            }

            public override void SetActiveSlot(bool on)
            {
                base.SetActiveSlot(on);
                if (on)
                    return;
                this.SetActiveOperatinPanel(false);
            }

            public override void SetData(BookModel book)
            {
                base.SetData(book);
                this.SetActiveSlot(true);
                this.cg_equiproot.alpha = 0.0f;
                if (this._bookDataModel.owner != null)
                {
                    if (this._bookDataModel != UI.UIController.Instance.CurrentUnit.bookItem)
                    {
                        if ((UnityEngine.Object)this.img_equipFrame != (UnityEngine.Object)null && (UnityEngine.Object)this.faceEditor != (UnityEngine.Object)null)
                        {
                            if (book.owner.isSephirah)
                                this.faceEditor.InitBySephirah(book.owner.defaultBook.GetBookClassInfoId());
                            else
                                this.faceEditor.Init(book.owner.customizeData);
                            this.cg_equiproot.alpha = 1f;
                            this.SetColorFrame(UIEquipPageSlotState.OtherEquiped);
                        }
                    }
                    else
                        this.SetColorFrame(UIEquipPageSlotState.None);
                }
                else if (!this._bookDataModel.CanEquipBookByGivePassive())
                    this.SetColorFrame(UIEquipPageSlotState.SuccessionMatter);
                this.SetActiveOperatinPanel(false);
                this.SetOperatingPanel();
                if ((UnityEngine.Object)this.ob_blockFrame != (UnityEngine.Object)null)
                {
                    if (this.ob_blockFrame.activeSelf)
                        this.ob_blockFrame.gameObject.SetActive(false);
                    this.isBlock = false;
                }
                if (LibraryModel.Instance.PlayHistory.Start_TheBlueReverberationPrimaryBattle != 1 || !((UnityEngine.Object)this.ob_blockFrame != (UnityEngine.Object)null))
                    return;
                SephirahType index = this._bookDataModel.IsCanUsingEquipPageWhenBlueReverberation();
                if (index == SephirahType.None)
                {
                    if (this.ob_blockFrame.activeSelf)
                        this.ob_blockFrame.gameObject.SetActive(false);
                }
                else
                {
                    if (!this.ob_blockFrame.activeSelf)
                        this.ob_blockFrame.gameObject.SetActive(true);
                    this.img_SepIcon.sprite = UISpriteDataManager.instance._floorIconSet[(int)index].icon;
                    this.isBlock = true;
                }
            }

            public override void SetActiveOperatinPanel(bool on)
            {
                if (!((UnityEngine.Object)this.ob_OperatingPanel != (UnityEngine.Object)null))
                    return;
                if (!this.ob_OperatingPanel.gameObject.activeSelf)
                    this.ob_OperatingPanel.gameObject.SetActive(true);
                if ((UnityEngine.Object)this.cg_operatingPanel == (UnityEngine.Object)null)
                    return;
                this.cg_operatingPanel.alpha = on ? 1f : 0.0f;
                this.cg_operatingPanel.blocksRaycasts = on;
                this.cg_operatingPanel.interactable = on;
            }

            public void SetOperatingPanel()
            {
                if ((UnityEngine.Object)this.ob_OperatingPanel == (UnityEngine.Object)null)
                    return;
                if (this._bookDataModel == null)
                {
                    this.SetActiveOperatinPanel(false);
                }
                else
                {
                    this.SetActiveOperatinPanel(true);
                    this.button_Equip.interactable = this._bookDataModel.CanEquipBookByGivePassive();
                    string id = this._bookDataModel.owner == null ? "ui_bookinventory_equipbook" : "ui_book_bookname_unequip";
                    this.img_equipbuttonIcon.sprite = this._bookDataModel.owner == null ? UISpriteDataManager.instance.EquipIcon[0] : UISpriteDataManager.instance.EquipIcon[1];
                    this.img_equipbuttonIcon.rectTransform.anchoredPosition = this._bookDataModel.owner == null ? new Vector2(0.0f, -5f) : Vector2.zero;
                    this.SetActiveOperatinPanel(false);
                    if (!this.button_EmptyDeck.gameObject.activeSelf)
                        this.button_EmptyDeck.gameObject.SetActive(true);
                    this.button_EmptyDeck.interactable = !this._bookDataModel.IsEmptyDeckAll();
                    if (this._bookDataModel.owner != null)
                    {
                        if (Singleton<StageController>.Instance.GetStageModel().IsUsedSephirah(this._bookDataModel.owner.OwnerSephirah))
                        {
                            id = "ui_cannotbemodifiedequippage";
                            this.button_Equip.interactable = false;
                            this.button_EmptyDeck.interactable = false;
                        }
                    }
                    else
                    {
                        UnitDataModel currentUnit = UI.UIController.Instance.CurrentUnit;
                        if (currentUnit != null)
                        {
                            if (this._bookDataModel.ClassInfo.id == 250022)
                            {
                                id = currentUnit.IsGebura() ? "ui_bookinventory_equipbook" : "ui_equippage_notequip";
                                this.button_Equip.interactable = currentUnit.IsGebura();
                            }
                            else if (currentUnit.IsChangeItemLock())
                            {
                                id = "ui_equippage_notequip";
                                this.button_Equip.interactable = false;
                            }
                        }
                    }
                    if (LibraryModel.Instance.PlayHistory.Start_TheBlueReverberationPrimaryBattle == 1 && LibraryModel.Instance.IsClearTheBlueReverberationPrimary(UI.UIController.Instance.CurrentUnit.OwnerSephirah))
                    {
                        id = "ui_equippage_notequip";
                        this.button_Equip.interactable = false;
                    }
                    this.img_equipbuttonIcon.color = this.button_Equip.interactable ? Color.white : UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                    this.txt_equipButton.text = TextDataModel.GetText(id);
                    if (!this.button_BookMark.gameObject.activeSelf)
                        this.button_BookMark.gameObject.SetActive(true);
                    this.txt_bookmarkButton.text = TextDataModel.GetText(this._bookDataModel.GetBookMarkState() ? "ui_equippageinventory_releasebookmark" : "ui_equippageinventory_addbookmark");
                    this.img_bookmarkbuttonIcon.color = this._bookDataModel.GetBookMarkState() ? UIColorManager.Manager.GetUIColor(UIColor.Highlighted) : UIColorManager.Manager.GetUIColor(UIColor.Default);
                }
            }

            public override void SetEmptySlot()
            {
                this.SetActiveOperatinPanel(false);
                this.cg_equiproot.alpha = 0.0f;
                base.SetEmptySlot();
                if (!((UnityEngine.Object)this.ob_blockFrame != (UnityEngine.Object)null))
                    return;
                if (this.ob_blockFrame.activeSelf)
                    this.ob_blockFrame.gameObject.SetActive(false);
                this.selectable.interactable = true;
            }

            public void OnClickEquipButton()
            {
                if (!this._bookDataModel.CanEquipBookByGivePassive())
                    Debug.LogError((object)"귀속된 책장입니다.");
                else if (this._bookDataModel.owner != null && Singleton<StageController>.Instance.GetStageModel().IsUsedSephirah(this._bookDataModel.owner.OwnerSephirah))
                {
                    Debug.LogError((object)"중고층에서 장착중인 책장입니다.");
                }
                else
                {
                    if (this._bookDataModel.owner == null)
                    {
                        UnitDataModel currentUnit = UI.UIController.Instance.CurrentUnit;
                        BookModel bookItem = currentUnit.bookItem;
                        BookModel bookDataModel = this._bookDataModel;
                        if (bookDataModel.ClassInfo.canNotEquip || !bookDataModel.CanEquipBookByGivePassive())
                        {
                            Debug.LogError((object)"장착 불가 책장");
                            return;
                        }
                        bool flag;
                        if (bookDataModel == bookItem)
                        {
                            flag = UI.UIController.Instance.CurrentUnit.EquipBookForUI((BookModel)null);
                        }
                        else
                        {
                            flag = UI.UIController.Instance.CurrentUnit.EquipBookForUI(bookDataModel);
                            this.GetEquipInvenPanel().isSaveCheck = false;
                            currentUnit.appearanceType = Gender.N;
                        }
                        if (flag)
                            UIAlarmPopup.instance.SetAlarmText_unused(UIAlarmType.DeckSizeReduced);
                        UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click);
                        currentUnit.appearanceType = Gender.N;
                        this.SetOperatingPanel();
                        SingletonBehavior<UICharacterRenderer>.Instance.ReloadCharacter(UI.UIController.Instance.CurrentUnit);
                        this.GetEquipInvenPanel().ChangeEquipBook(UI.UIController.Instance.CurrentUnit);
                    }
                    else
                    {
                        UnitDataModel owner = this._bookDataModel.owner;
                        owner.EquipBookForUI((BookModel)null);
                        UISoundManager.instance.PlayEffectSound(UISoundType.Ui_Click);
                        this.GetEquipInvenPanel().EquipLeftPanel.EquipPageList.ReleaseSelectedSlot(this._bookDataModel);
                        owner.appearanceType = Gender.N;
                        this.SetOperatingPanel();
                        this.GetEquipInvenPanel().isSaveCheck = false;
                        SingletonBehavior<UICharacterRenderer>.Instance.ReloadCharacter(owner);
                        this.GetEquipInvenPanel().ChangeEquipBook(owner);
                    }
                    if (this.selectable.interactable)
                    {
                        UIControlManager.Instance.SelectSelectableForcely(this.selectable);
                    }
                    else
                    {
                        UICustomSelectable component = this.selectable.FindSelectableOnLeft().GetComponent<UICustomSelectable>();
                        if ((UnityEngine.Object)component == (UnityEngine.Object)null)
                            UIControlManager.Instance.SelectSelectableForcely(this.selectable);
                        else
                            UIControlManager.Instance.SelectSelectableForcely(component);
                    }
                }
            }

            public void OnClickEmptyDeckButton()
            {
                if (this._bookDataModel == null)
                    return;
                if (this._bookDataModel.owner != null && Singleton<StageController>.Instance.GetStageModel().IsUsedSephirah(this._bookDataModel.owner.OwnerSephirah))
                {
                    Debug.LogError((object)"중고층에서 장착중인 책장입니다.");
                }
                else
                {
                    this._bookDataModel.EmptyDeckToInventoryAll();
                    this.GetEquipInvenPanel().isSaveCheck = false;
                    this.GetEquipInvenPanel().UpdateRightPanel();
                    this.GetEquipInvenPanel().ReleaseSelectedSlot();
                    this.SetOperatingPanel();
                    if (this.selectable.interactable)
                    {
                        UIControlManager.Instance.SelectSelectableForcely(this.selectable);
                    }
                    else
                    {
                        UICustomSelectable component = this.selectable.FindSelectableOnLeft().GetComponent<UICustomSelectable>();
                        if ((UnityEngine.Object)component == (UnityEngine.Object)null)
                            UIControlManager.Instance.SelectSelectableForcely(this.selectable);
                        else
                            UIControlManager.Instance.SelectSelectableForcely(component);
                    }
                }
            }

            public void OnClickBookMarkButton()
            {
                if (this._bookDataModel == null)
                    return;
                this._bookDataModel.ResisterBookMark(!this._bookDataModel.GetBookMarkState());
                this.GetEquipInvenPanel().EquipLeftPanel.EquipPageList.ReleaseSelectedSlot(this._bookDataModel);
                this.GetEquipInvenPanel().isSaveCheck = false;
                this.GetEquipInvenPanel().UpdateLeftPanel();
                this.GetEquipInvenPanel().UpdateCenterPanel();
                this.GetEquipInvenPanel().ReleaseSelectedSlot();
                if (this.selectable.interactable)
                {
                    UIControlManager.Instance.SelectSelectableForcely(this.selectable);
                }
                else
                {
                    UICustomSelectable component = this.selectable.FindSelectableOnLeft().GetComponent<UICustomSelectable>();
                    if ((UnityEngine.Object)component == (UnityEngine.Object)null)
                        UIControlManager.Instance.SelectSelectableForcely(this.selectable);
                    else
                        UIControlManager.Instance.SelectSelectableForcely(component);
                }
            }

            public void OnPointerEnter(BaseEventData eventData)
            {
                if (this.isEmptyBook)
                    return;
                UISoundManager.instance.PlayEffectSound(UISoundType.Card_Over);
                if (UIControlManager.GetInpuTypeOf(eventData) == UI.InputType.ControllerInput)
                {
                    this.listRoot.SetSaveFirstChild((UIOriginEquipPageSlot)this);
                    this.listRoot.CheckSelectSlotMove((UIOriginEquipPageSlot)this);
                    this.SetHighlighted(true, this.listRoot.CurrentSelectedBook == this._bookDataModel, true);
                }
                else
                    this.SetHighlighted(true, this.listRoot.CurrentSelectedBook == this._bookDataModel);
                if (!((UnityEngine.Object)this.GetEquipInvenPanel().CurrentSelectedSlot == (UnityEngine.Object)null))
                    return;
                this.GetEquipInvenPanel().currentOverSlot = (UIOriginEquipPageSlot)this;
                this.GetEquipInvenPanel().ShowPreviewPanel((UIOriginEquipPageSlot)this);
            }

            public void OnPointerExit(BaseEventData eventData)
            {
                if ((UnityEngine.Object)this.GetEquipInvenPanel().CurrentSelectedSlot == (UnityEngine.Object)null)
                {
                    this.GetEquipInvenPanel().currentOverSlot = (UIOriginEquipPageSlot)null;
                    this.GetEquipInvenPanel().HidePreviewPanel();
                }
                if (this.isEmptyBook)
                    return;
                if (this.listRoot.CurrentSelectedBook == this._bookDataModel)
                    this.SetOffPadSelectHighlighted();
                else
                    this.SetHighlighted(false);
            }

            public void OnPointerClick(BaseEventData data)
            {
                if (this.isEmptyBook || this.isBlock || UIControlManager.GetInpuTypeOf(data) == UI.InputType.RightClick)
                    return;
                this.GetEquipInvenPanel().OnClickSlot((UIOriginEquipPageSlot)this);
                UIControlManager.Instance.SelectSelectableForcely(this.button_Equip.interactable ? this.button_Equip.selectable : this.button_BookMark.selectable);
            }

            public void OnCancelOperating()
            {
                this.SetActiveOperatinPanel(false);
                UIControlManager.Instance.SelectSelectableForcely(this.selectable);
                this.GetEquipInvenPanel().ReleaseSelectedSlotNotHidePreview();
            }

            public void OnScroll(BaseEventData eventData)
            {
                this.listRoot.OnScroll(eventData as PointerEventData);
            }

            public void OnXEvent() => this.listRoot.ChangeSelectableToFilter();

            public void OnYEvent()
            {
                this.GetEquipInvenPanel().SwitchPreviewVisible((UIOriginEquipPageSlot)this);
            }

            public UISettingEquipPageInvenPanel GetEquipInvenPanel()
            {
                return (UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel).EditPanel.EquipPagePanel;
            }
        }

        public class LogBattleEmotionRewardSlotUI : MonoBehaviour
        {
            public BattleEmotionRewardInfoUI panel;
            public RectTransform rect;
            public RectTransform rect_frame;
            public RectTransform rect_bg;
            public Image img_emotionlevel;
            public TextMeshProUGUI txt_Name;
            public List<TextMeshProUGUI> rewardtexts;

            public static BattleEmotionRewardSlotUI BattleEmotionRewardSlotUI_Copying(
              BattleEmotionRewardSlotUI baseobj)
            {
                LogLikeMod.LogBattleEmotionRewardSlotUI original = baseobj.gameObject.AddComponent<LogLikeMod.LogBattleEmotionRewardSlotUI>();
                original.panel = LogLikeMod.GetFieldValue<BattleEmotionRewardInfoUI>((object)baseobj, "panel");
                original.rect = LogLikeMod.GetFieldValue<RectTransform>((object)baseobj, "rect");
                original.rect_frame = LogLikeMod.GetFieldValue<RectTransform>((object)baseobj, "rect_frame");
                original.rect_bg = LogLikeMod.GetFieldValue<RectTransform>((object)baseobj, "rect_bg");
                original.img_emotionlevel = LogLikeMod.GetFieldValue<Image>((object)baseobj, "img_emotionlevel");
                original.txt_Name = LogLikeMod.GetFieldValue<TextMeshProUGUI>((object)baseobj, "txt_Name");
                original.rewardtexts = LogLikeMod.GetFieldValue<List<TextMeshProUGUI>>((object)baseobj, "rewardtexts");
                LogLikeMod.LogBattleEmotionRewardSlotUI emotionRewardSlotUi1 = UnityEngine.Object.Instantiate<LogLikeMod.LogBattleEmotionRewardSlotUI>(original, original.transform.parent);
                BattleEmotionRewardSlotUI emotionRewardSlotUi2 = (UnityEngine.Object)emotionRewardSlotUi1.GetComponent<BattleEmotionRewardSlotUI>() == (UnityEngine.Object)null ? emotionRewardSlotUi1.gameObject.AddComponent<BattleEmotionRewardSlotUI>() : emotionRewardSlotUi1.GetComponent<BattleEmotionRewardSlotUI>();
                LogLikeMod.SetFieldValue((object)emotionRewardSlotUi2, "panel", (object)emotionRewardSlotUi1.panel);
                LogLikeMod.SetFieldValue((object)emotionRewardSlotUi2, "rect", (object)emotionRewardSlotUi1.rect);
                LogLikeMod.SetFieldValue((object)emotionRewardSlotUi2, "rect_frame", (object)emotionRewardSlotUi1.rect_frame);
                LogLikeMod.SetFieldValue((object)emotionRewardSlotUi2, "rect_bg", (object)emotionRewardSlotUi1.rect_bg);
                LogLikeMod.SetFieldValue((object)emotionRewardSlotUi2, "img_emotionlevel", (object)emotionRewardSlotUi1.img_emotionlevel);
                LogLikeMod.SetFieldValue((object)emotionRewardSlotUi2, "txt_Name", (object)emotionRewardSlotUi1.txt_Name);
                LogLikeMod.SetFieldValue((object)emotionRewardSlotUi2, "rewardtexts", (object)emotionRewardSlotUi1.rewardtexts);
                UnityEngine.Object.Destroy((UnityEngine.Object)original);
                UnityEngine.Object.Destroy((UnityEngine.Object)emotionRewardSlotUi1);
                return emotionRewardSlotUi2;
            }
        }

        public class LogBattleCharacterProfileUI : MonoBehaviour
        {
            public Transform uiRoot;
            public Image img_bg;
            public BattleUnitProfileInfoUI_EmotionLvTooltip _emotionLvTooltip;
            public RawImage rawImg_portrait;
            public TextMeshProUGUI txt_unitName;
            public GameObject go_hpValueLayout;
            public GameObject go_emotionLVPivot;
            public BattleCharacterProfileUI.HpBar hpBar;
            public BattleCharacterProfileUI.HpBar img_damagedHp;
            public BattleCharacterProfileUI.HpBar img_healedHp;
            public Text txt_hp;
            public BattleCharacterProfileUI_CoinManager coinUI;
            public BattleCharacterProfileEmotionUI emotionUI;
            public Color _colorDialogBG_New;
            public CanvasGroup _battleDialogCanvasGroup;
            public TextMeshProUGUI _battleDialog;
            public Image _battleDialogBG;
            public Image _battleDialogNewBG;
            public Image _battleDialogChildImg;
            public Image _battleDialogLinearDodge;
            public bool isLeft = true;

            public static BattleCharacterProfileUI BattleUnitInfoManagerUI_Copying(
              BattleCharacterProfileUI baseobj)
            {
                LogLikeMod.LogBattleCharacterProfileUI original = baseobj.gameObject.AddComponent<LogLikeMod.LogBattleCharacterProfileUI>();
                original.uiRoot = LogLikeMod.GetFieldValue<Transform>((object)baseobj, "uiRoot");
                original.img_bg = LogLikeMod.GetFieldValue<Image>((object)baseobj, "img_bg");
                original._emotionLvTooltip = LogLikeMod.GetFieldValue<BattleUnitProfileInfoUI_EmotionLvTooltip>((object)baseobj, "_emotionLvTooltip");
                original.rawImg_portrait = LogLikeMod.GetFieldValue<RawImage>((object)baseobj, "rawImg_portrait");
                original.txt_unitName = LogLikeMod.GetFieldValue<TextMeshProUGUI>((object)baseobj, "txt_unitName");
                original.go_hpValueLayout = LogLikeMod.GetFieldValue<GameObject>((object)baseobj, "go_hpValueLayout");
                original.go_emotionLVPivot = LogLikeMod.GetFieldValue<GameObject>((object)baseobj, "go_emotionLVPivot");
                original.hpBar = LogLikeMod.GetFieldValue<BattleCharacterProfileUI.HpBar>((object)baseobj, "hpBar");
                original.img_damagedHp = LogLikeMod.GetFieldValue<BattleCharacterProfileUI.HpBar>((object)baseobj, "img_damagedHp");
                original.img_healedHp = LogLikeMod.GetFieldValue<BattleCharacterProfileUI.HpBar>((object)baseobj, "img_healedHp");
                original.txt_hp = LogLikeMod.GetFieldValue<Text>((object)baseobj, "txt_hp");
                original.coinUI = LogLikeMod.GetFieldValue<BattleCharacterProfileUI_CoinManager>((object)baseobj, "coinUI");
                original.emotionUI = LogLikeMod.GetFieldValue<BattleCharacterProfileEmotionUI>((object)baseobj, "emotionUI");
                original._colorDialogBG_New = LogLikeMod.GetFieldValue<Color>((object)baseobj, "_colorDialogBG_New");
                original._battleDialogCanvasGroup = LogLikeMod.GetFieldValue<CanvasGroup>((object)baseobj, "_battleDialogCanvasGroup");
                original._battleDialog = LogLikeMod.GetFieldValue<TextMeshProUGUI>((object)baseobj, "_battleDialog");
                original._battleDialogBG = LogLikeMod.GetFieldValue<Image>((object)baseobj, "_battleDialogBG");
                original._battleDialogNewBG = LogLikeMod.GetFieldValue<Image>((object)baseobj, "_battleDialogNewBG");
                original._battleDialogChildImg = LogLikeMod.GetFieldValue<Image>((object)baseobj, "_battleDialogChildImg");
                original._battleDialogLinearDodge = LogLikeMod.GetFieldValue<Image>((object)baseobj, "_battleDialogLinearDodge");
                original.isLeft = true;
                LogLikeMod.LogBattleCharacterProfileUI characterProfileUi1 = UnityEngine.Object.Instantiate<LogLikeMod.LogBattleCharacterProfileUI>(original, original.transform.parent);
                BattleCharacterProfileUI characterProfileUi2 = (UnityEngine.Object)characterProfileUi1.GetComponent<BattleCharacterProfileUI>() == (UnityEngine.Object)null ? characterProfileUi1.gameObject.AddComponent<BattleCharacterProfileUI>() : characterProfileUi1.GetComponent<BattleCharacterProfileUI>();
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "uiRoot", (object)characterProfileUi1.uiRoot);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "img_bg", (object)characterProfileUi1.img_bg);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "_emotionLvTooltip", (object)characterProfileUi1._emotionLvTooltip);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "rawImg_portrait", (object)characterProfileUi1.rawImg_portrait);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "txt_unitName", (object)characterProfileUi1.txt_unitName);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "go_hpValueLayout", (object)characterProfileUi1.go_hpValueLayout);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "go_emotionLVPivot", (object)characterProfileUi1.go_emotionLVPivot);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "hpBar", (object)characterProfileUi1.hpBar);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "img_damagedHp", (object)characterProfileUi1.img_damagedHp);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "img_healedHp", (object)characterProfileUi1.img_healedHp);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "txt_hp", (object)characterProfileUi1.txt_hp);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "coinUI", (object)characterProfileUi1.coinUI);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "emotionUI", (object)characterProfileUi1.emotionUI);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "_colorDialogBG_New", (object)characterProfileUi1._colorDialogBG_New);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "_battleDialogCanvasGroup", (object)characterProfileUi1._battleDialogCanvasGroup);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "_battleDialog", (object)characterProfileUi1._battleDialog);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "_battleDialogBG", (object)characterProfileUi1._battleDialogBG);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "_battleDialogNewBG", (object)characterProfileUi1._battleDialogNewBG);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "_battleDialogChildImg", (object)characterProfileUi1._battleDialogChildImg);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "_battleDialogLinearDodge", (object)characterProfileUi1._battleDialogLinearDodge);
                LogLikeMod.SetFieldValue((object)characterProfileUi2, "isLeft", (object)characterProfileUi1.isLeft);
                UnityEngine.Object.Destroy((UnityEngine.Object)original);
                UnityEngine.Object.Destroy((UnityEngine.Object)characterProfileUi1);
                return characterProfileUi2;
            }
        }

        public class UILogBattleDiceCardUI : MonoBehaviour
        {
            public static LogLikeMod.UILogBattleDiceCardUI _instance;
            public RectTransform vibeRect;
            public Sprite[] costNumberSprite;
            public Sprite costNumberSprite_1;
            public Sprite costNumberSprite_2;
            public Sprite costNumberSprite_3;
            public Sprite costNumberSprite_4;
            public Sprite costNumberSprite_5;
            public Sprite costNumberSprite_6;
            public Sprite costNumberSprite_7;
            public Sprite costNumberSprite_8;
            public Sprite costNumberSprite_9;
            public Sprite costNumberSprite_10;
            public Color[] refColors_Cost;
            public Color refColors_Cost_1;
            public Color refColors_Cost_2;
            public Color refColors_Cost_3;
            public TextMeshProUGUI txt_cardName;
            public Image[] img_Frames;
            public Image img_Frames_1;
            public Image img_Frames_2;
            public Image img_Frames_3;
            public Image img_Frames_4;
            public Image img_Frames_5;
            public Image[] img_linearDodges;
            public Image img_linearDodges_1;
            public Image img_linearDodges_2;
            public Image img_linearDodges_3;
            public Image img_linearDodges_4;
            public Image img_linearDodges_5;
            public Image img_linearDodges_6;
            public GameObject selfAbilityArea;
            public TextMeshProUGUI txt_selfAbility;
            public List<BattleDiceCard_BehaviourDescUI> ui_behaviourDescList;
            public BattleDiceCard_BehaviourDescUI ui_behaviourDescList_1;
            public BattleDiceCard_BehaviourDescUI ui_behaviourDescList_2;
            public BattleDiceCard_BehaviourDescUI ui_behaviourDescList_3;
            public BattleDiceCard_BehaviourDescUI ui_behaviourDescList_4;
            public BattleDiceCard_BehaviourDescUI ui_behaviourDescList_5;
            public List<Image> img_behaviourDetatilList;
            public Image img_behaviourDetatilList_1;
            public Image img_behaviourDetatilList_2;
            public Image img_behaviourDetatilList_3;
            public Image img_behaviourDetatilList_4;
            public Image img_behaviourDetatilList_5;
            public Animator anim;
            public Image img_artwork;
            public Image img_icon;
            public RefineHsv hsv_rangeIcon;
            public RefineHsv hsv_Cost;
            public float _glowElapsedTime;
            public BattleDiceCardBufUI[] bufIconListUI;
            public BattleDiceCardBufUI bufIconListUI_1;
            public BattleDiceCardBufUI bufIconListUI_2;
            public BattleDiceCardBufUI bufIconListUI_3;
            public KeywordListUI KeywordListUI;
            public List<RefineHsv> hsv_behaviourIcons;
            public RefineHsv hsv_behaviourIcons_1;
            public RefineHsv hsv_behaviourIcons_2;
            public RefineHsv hsv_behaviourIcons_3;
            public RefineHsv hsv_behaviourIcons_4;
            public RefineHsv hsv_behaviourIcons_5;
            public List<TextMeshProUGUI> txt_Resist;
            public TextMeshProUGUI txt_Resist_1;
            public TextMeshProUGUI txt_Resist_2;
            public TextMeshProUGUI txt_Resist_3;
            public TextMeshProUGUI txt_Resist_4;
            public TextMeshProUGUI txt_Resist_5;
            public List<TextMeshProUGUI> txt_bpResist;
            public TextMeshProUGUI txt_bpResist_1;
            public TextMeshProUGUI txt_bpResist_2;
            public TextMeshProUGUI txt_bpResist_3;
            public TextMeshProUGUI txt_bpResist_4;
            public TextMeshProUGUI txt_bpResist_5;
            public GameObject[] ob_NormalFrames;
            public GameObject ob_NormalFrames_1;
            public GameObject ob_NormalFrames_2;
            public GameObject[] ob_EgoFrames;
            public GameObject ob_EgoFrames_1;
            public GameObject ob_EgoFrames_2;
            public UICustomSelectable selectable;
            public bool isProfileCard;
            public bool isEmotionSelectedPopup;
            public Canvas _parentCanvas;
            public Transform mouseTransform;
            public int _defaultIdx;
            public BattleDiceCardModel _cardModel;
            public bool _bClicked;
            public bool _bAvailable;
            public bool _bFirstClicked;
            public bool _bEntered;
            public BattleUnitTargetArrowUI arrow;
            public Color colorFrame;
            public Color colorLineardodge;
            public Color colorLineardodge_deactive;
            public Vector3 scaleOrigin;
            public NumbersData costNumbers;
            public int _cost;
            public int _originCost;
            public bool _editor;
            public bool isRunningVibeCard;
            public float vibeCounter;
            public Vector2 rangeIconOriginPos = new Vector2(305f, 189f);
            public Vector2 rangeIconEgoPos = new Vector2(335f, 189f);
            public EmotionEgoXmlInfo egoxmldata;
            public GameObject ob_EgoCoolTime;
            public RectTransform rect_Gauge;
            public Image img_Bg;
            public Image img_BgGlow;
            public RefineHsv hsv_bgGlow;
            public Animator anim_gaugebgglow;
            public Graphic[] graphics_EgoLockFrames;
            public Graphic graphics_EgoLockFrames_1;
            public RefineHsv[] hsv_EgoLockFrames;
            public RefineHsv hsv_EgoLockFrames_1;
            public bool isEgoCoolTimeLock;
            public float gaugeLength = 950f;

            public static LogLikeMod.UILogBattleDiceCardUI Instance
            {
                get
                {
                    if ((UnityEngine.Object)LogLikeMod.UILogBattleDiceCardUI._instance == (UnityEngine.Object)null)
                        LogLikeMod.UILogBattleDiceCardUI._instance = LogLikeMod.UILogBattleDiceCardUI.SlotCopying();
                    return LogLikeMod.UILogBattleDiceCardUI._instance;
                }
                set => LogLikeMod.UILogBattleDiceCardUI._instance = value;
            }

            public static LogLikeMod.UILogBattleDiceCardUI SlotCopying()
            {
                BattleDiceCardUI battleDiceCardUi1 = (BattleDiceCardUI)typeof(BattleUnitInformationUI).GetField("previewCardUI", AccessTools.all).GetValue((object)SingletonBehavior<BattleManagerUI>.Instance.ui_unitInformationPlayer);
                LogLikeMod.UILogBattleDiceCardUI original = battleDiceCardUi1.gameObject.AddComponent<LogLikeMod.UILogBattleDiceCardUI>();
                original.vibeRect = (RectTransform)typeof(BattleDiceCardUI).GetField("vibeRect", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.costNumberSprite = (Sprite[])typeof(BattleDiceCardUI).GetField("costNumberSprite", AccessTools.all).GetValue((object)battleDiceCardUi1);
                for (int index = 0; index < 10; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("costNumberSprite_" + (index + 1).ToString()).SetValue((object)original, (object)original.costNumberSprite[index]);
                original.refColors_Cost = (Color[])typeof(BattleDiceCardUI).GetField("refColors_Cost", AccessTools.all).GetValue((object)battleDiceCardUi1);
                for (int index = 0; index < 3; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("refColors_Cost_" + (index + 1).ToString()).SetValue((object)original, (object)original.refColors_Cost[index]);
                original.txt_cardName = (TextMeshProUGUI)typeof(BattleDiceCardUI).GetField("txt_cardName", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.img_Frames = (Image[])typeof(BattleDiceCardUI).GetField("img_Frames", AccessTools.all).GetValue((object)battleDiceCardUi1);
                for (int index = 0; index < 5; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("img_Frames_" + (index + 1).ToString()).SetValue((object)original, (object)original.img_Frames[index]);
                original.img_linearDodges = (Image[])typeof(BattleDiceCardUI).GetField("img_linearDodges", AccessTools.all).GetValue((object)battleDiceCardUi1);
                for (int index = 0; index < 6; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("img_linearDodges_" + (index + 1).ToString()).SetValue((object)original, (object)original.img_linearDodges[index]);
                original.selfAbilityArea = (GameObject)typeof(BattleDiceCardUI).GetField("selfAbilityArea", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.txt_selfAbility = (TextMeshProUGUI)typeof(BattleDiceCardUI).GetField("txt_selfAbility", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.ui_behaviourDescList = (List<BattleDiceCard_BehaviourDescUI>)typeof(BattleDiceCardUI).GetField("ui_behaviourDescList", AccessTools.all).GetValue((object)battleDiceCardUi1);
                for (int index = 0; index < 5; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("ui_behaviourDescList_" + (index + 1).ToString()).SetValue((object)original, (object)original.ui_behaviourDescList[index]);
                original.img_behaviourDetatilList = (List<Image>)typeof(BattleDiceCardUI).GetField("img_behaviourDetatilList", AccessTools.all).GetValue((object)battleDiceCardUi1);
                for (int index = 0; index < 5; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("img_behaviourDetatilList_" + (index + 1).ToString()).SetValue((object)original, (object)original.img_behaviourDetatilList[index]);
                original.anim = (Animator)typeof(BattleDiceCardUI).GetField("anim", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.img_artwork = (Image)typeof(BattleDiceCardUI).GetField("img_artwork", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.img_icon = (Image)typeof(BattleDiceCardUI).GetField("img_icon", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.hsv_rangeIcon = (RefineHsv)typeof(BattleDiceCardUI).GetField("hsv_rangeIcon", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.hsv_Cost = (RefineHsv)typeof(BattleDiceCardUI).GetField("hsv_Cost", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original._glowElapsedTime = (float)typeof(BattleDiceCardUI).GetField("_glowElapsedTime", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.bufIconListUI = (BattleDiceCardBufUI[])typeof(BattleDiceCardUI).GetField("bufIconListUI", AccessTools.all).GetValue((object)battleDiceCardUi1);
                for (int index = 0; index < 3; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("bufIconListUI_" + (index + 1).ToString()).SetValue((object)original, (object)original.bufIconListUI[index]);
                original.KeywordListUI = (KeywordListUI)typeof(BattleDiceCardUI).GetField("KeywordListUI", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.hsv_behaviourIcons = (List<RefineHsv>)typeof(BattleDiceCardUI).GetField("hsv_behaviourIcons", AccessTools.all).GetValue((object)battleDiceCardUi1);
                for (int index = 0; index < 5; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("hsv_behaviourIcons_" + (index + 1).ToString()).SetValue((object)original, (object)original.hsv_behaviourIcons[index]);
                original.txt_Resist = (List<TextMeshProUGUI>)typeof(BattleDiceCardUI).GetField("txt_Resist", AccessTools.all).GetValue((object)battleDiceCardUi1);
                for (int index = 0; index < 5; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("txt_Resist_" + (index + 1).ToString()).SetValue((object)original, (object)original.txt_Resist[index]);
                original.txt_bpResist = (List<TextMeshProUGUI>)typeof(BattleDiceCardUI).GetField("txt_bpResist", AccessTools.all).GetValue((object)battleDiceCardUi1);
                for (int index = 0; index < 5; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("txt_bpResist_" + (index + 1).ToString()).SetValue((object)original, (object)original.txt_bpResist[index]);
                original.ob_NormalFrames = (GameObject[])typeof(BattleDiceCardUI).GetField("ob_NormalFrames", AccessTools.all).GetValue((object)battleDiceCardUi1);
                for (int index = 0; index < 2; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("ob_NormalFrames_" + (index + 1).ToString()).SetValue((object)original, (object)original.ob_NormalFrames[index]);
                original.ob_EgoFrames = (GameObject[])typeof(BattleDiceCardUI).GetField("ob_EgoFrames", AccessTools.all).GetValue((object)battleDiceCardUi1);
                for (int index = 0; index < 2; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("ob_EgoFrames_" + (index + 1).ToString()).SetValue((object)original, (object)original.ob_EgoFrames[index]);
                original.selectable = (UICustomSelectable)typeof(BattleDiceCardUI).GetField("selectable", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.isProfileCard = (bool)typeof(BattleDiceCardUI).GetField("isProfileCard", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.isEmotionSelectedPopup = (bool)typeof(BattleDiceCardUI).GetField("isEmotionSelectedPopup", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.mouseTransform = (Transform)typeof(BattleDiceCardUI).GetField("mouseTransform", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original._defaultIdx = (int)typeof(BattleDiceCardUI).GetField("_defaultIdx", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original._cardModel = (BattleDiceCardModel)typeof(BattleDiceCardUI).GetField("_cardModel", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original._bClicked = (bool)typeof(BattleDiceCardUI).GetField("_bClicked", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original._bAvailable = (bool)typeof(BattleDiceCardUI).GetField("_bAvailable", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original._bFirstClicked = (bool)typeof(BattleDiceCardUI).GetField("_bFirstClicked", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original._bEntered = (bool)typeof(BattleDiceCardUI).GetField("_bEntered", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.arrow = (BattleUnitTargetArrowUI)null;
                original.colorFrame = (Color)typeof(BattleDiceCardUI).GetField("colorFrame", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.colorLineardodge = (Color)typeof(BattleDiceCardUI).GetField("colorLineardodge", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.colorLineardodge_deactive = (Color)typeof(BattleDiceCardUI).GetField("colorLineardodge_deactive", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.scaleOrigin = (Vector3)typeof(BattleDiceCardUI).GetField("scaleOrigin", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.costNumbers = (NumbersData)typeof(BattleDiceCardUI).GetField("costNumbers", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original._cost = (int)typeof(BattleDiceCardUI).GetField("_cost", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original._originCost = (int)typeof(BattleDiceCardUI).GetField("_originCost", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original._editor = (bool)typeof(BattleDiceCardUI).GetField("_editor", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.isRunningVibeCard = (bool)typeof(BattleDiceCardUI).GetField("isRunningVibeCard", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.vibeCounter = (float)typeof(BattleDiceCardUI).GetField("vibeCounter", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.rangeIconOriginPos = (Vector2)typeof(BattleDiceCardUI).GetField("rangeIconOriginPos", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.rangeIconEgoPos = (Vector2)typeof(BattleDiceCardUI).GetField("rangeIconEgoPos", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.egoxmldata = (EmotionEgoXmlInfo)typeof(BattleDiceCardUI).GetField("egoxmldata", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.ob_EgoCoolTime = (GameObject)typeof(BattleDiceCardUI).GetField("ob_EgoCoolTime", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.rect_Gauge = (RectTransform)typeof(BattleDiceCardUI).GetField("rect_Gauge", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.img_Bg = (Image)typeof(BattleDiceCardUI).GetField("img_Bg", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.img_BgGlow = (Image)typeof(BattleDiceCardUI).GetField("img_BgGlow", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.hsv_bgGlow = (RefineHsv)typeof(BattleDiceCardUI).GetField("hsv_bgGlow", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.anim_gaugebgglow = (Animator)typeof(BattleDiceCardUI).GetField("anim_gaugebgglow", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.graphics_EgoLockFrames = (Graphic[])typeof(BattleDiceCardUI).GetField("graphics_EgoLockFrames", AccessTools.all).GetValue((object)battleDiceCardUi1);
                for (int index = 0; index < 1; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("graphics_EgoLockFrames_" + (index + 1).ToString()).SetValue((object)original, (object)original.graphics_EgoLockFrames[index]);
                original.hsv_EgoLockFrames = (RefineHsv[])typeof(BattleDiceCardUI).GetField("hsv_EgoLockFrames", AccessTools.all).GetValue((object)battleDiceCardUi1);
                for (int index = 0; index < 1; ++index)
                    typeof(LogLikeMod.UILogBattleDiceCardUI).GetField("hsv_EgoLockFrames_" + (index + 1).ToString()).SetValue((object)original, (object)original.hsv_EgoLockFrames[index]);
                original.isEgoCoolTimeLock = (bool)typeof(BattleDiceCardUI).GetField("isEgoCoolTimeLock", AccessTools.all).GetValue((object)battleDiceCardUi1);
                original.gaugeLength = (float)typeof(BattleDiceCardUI).GetField("gaugeLength", AccessTools.all).GetValue((object)battleDiceCardUi1);
                LogLikeMod.UILogBattleDiceCardUI battleDiceCardUi2 = UnityEngine.Object.Instantiate<LogLikeMod.UILogBattleDiceCardUI>(original, SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.transform);
                int num;
                for (int index1 = 0; index1 < 10; ++index1)
                {
                    Sprite[] costNumberSprite = battleDiceCardUi2.costNumberSprite;
                    int index2 = index1;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index1 + 1;
                    string name = "costNumberSprite_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    Sprite sprite = (Sprite)(type.GetField(name)).GetValue((object)battleDiceCardUi2);
                    costNumberSprite[index2] = sprite;
                }
                for (int index3 = 0; index3 < 3; ++index3)
                {
                    Color[] refColorsCost = battleDiceCardUi2.refColors_Cost;
                    int index4 = index3;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index3 + 1;
                    string name = "refColors_Cost_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    Color color = (Color)(type.GetField(name)).GetValue((object)battleDiceCardUi2);
                    refColorsCost[index4] = color;
                }
                for (int index5 = 0; index5 < 5; ++index5)
                {
                    Image[] imgFrames = battleDiceCardUi2.img_Frames;
                    int index6 = index5;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index5 + 1;
                    string name = "img_Frames_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    Image image = (Image)(type.GetField(name)).GetValue((object)battleDiceCardUi2);
                    imgFrames[index6] = image;
                }
                for (int index7 = 0; index7 < 6; ++index7)
                {
                    Image[] imgLinearDodges = battleDiceCardUi2.img_linearDodges;
                    int index8 = index7;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index7 + 1;
                    string name = "img_linearDodges_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    Image image = (Image)(type.GetField(name)).GetValue((object)battleDiceCardUi2);
                    imgLinearDodges[index8] = image;
                }
                for (int index9 = 0; index9 < 5; ++index9)
                {
                    List<BattleDiceCard_BehaviourDescUI> behaviourDescList = battleDiceCardUi2.ui_behaviourDescList;
                    int index10 = index9;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index9 + 1;
                    string name = "ui_behaviourDescList_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    BattleDiceCard_BehaviourDescUI cardBehaviourDescUi = (BattleDiceCard_BehaviourDescUI)(type.GetField(name)).GetValue((object)battleDiceCardUi2);
                    behaviourDescList[index10] = cardBehaviourDescUi;
                }
                for (int index11 = 0; index11 < 5; ++index11)
                {
                    List<Image> behaviourDetatilList = battleDiceCardUi2.img_behaviourDetatilList;
                    int index12 = index11;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index11 + 1;
                    string name = "img_behaviourDetatilList_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    Image image = (Image)(type.GetField(name)).GetValue((object)battleDiceCardUi2);
                    behaviourDetatilList[index12] = image;
                }
                for (int index13 = 0; index13 < 3; ++index13)
                {
                    BattleDiceCardBufUI[] bufIconListUi = battleDiceCardUi2.bufIconListUI;
                    int index14 = index13;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index13 + 1;
                    string name = "bufIconListUI_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    BattleDiceCardBufUI battleDiceCardBufUi = (BattleDiceCardBufUI)(type.GetField(name)).GetValue((object)battleDiceCardUi2);
                    bufIconListUi[index14] = battleDiceCardBufUi;
                }
                for (int index15 = 0; index15 < 5; ++index15)
                {
                    List<RefineHsv> hsvBehaviourIcons = battleDiceCardUi2.hsv_behaviourIcons;
                    int index16 = index15;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index15 + 1;
                    string name = "hsv_behaviourIcons_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    RefineHsv refineHsv = (RefineHsv)(type.GetField(name)).GetValue((object)battleDiceCardUi2);
                    hsvBehaviourIcons[index16] = refineHsv;
                }
                for (int index17 = 0; index17 < 5; ++index17)
                {
                    List<TextMeshProUGUI> txtResist = battleDiceCardUi2.txt_Resist;
                    int index18 = index17;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index17 + 1;
                    string name = "txt_Resist_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    TextMeshProUGUI textMeshProUgui = (TextMeshProUGUI)(type.GetField(name)).GetValue((object)battleDiceCardUi2);
                    txtResist[index18] = textMeshProUgui;
                }
                for (int index19 = 0; index19 < 5; ++index19)
                {
                    List<TextMeshProUGUI> txtBpResist = battleDiceCardUi2.txt_bpResist;
                    int index20 = index19;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index19 + 1;
                    string name = "txt_bpResist_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    TextMeshProUGUI textMeshProUgui = (TextMeshProUGUI)(type.GetField(name)).GetValue((object)battleDiceCardUi2);
                    txtBpResist[index20] = textMeshProUgui;
                }
                for (int index21 = 0; index21 < 2; ++index21)
                {
                    GameObject[] obNormalFrames = battleDiceCardUi2.ob_NormalFrames;
                    int index22 = index21;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index21 + 1;
                    string name = "ob_NormalFrames_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    GameObject gameObject = (GameObject)(type.GetField(name)).GetValue((object)battleDiceCardUi2);
                    obNormalFrames[index22] = gameObject;
                }
                for (int index23 = 0; index23 < 2; ++index23)
                {
                    GameObject[] obEgoFrames = battleDiceCardUi2.ob_EgoFrames;
                    int index24 = index23;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index23 + 1;
                    string name = "ob_EgoFrames_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    GameObject gameObject = (GameObject)(type.GetField(name)).GetValue((object)battleDiceCardUi2);
                    obEgoFrames[index24] = gameObject;
                }
                for (int index25 = 0; index25 < 1; ++index25)
                {
                    Graphic[] graphicsEgoLockFrames = battleDiceCardUi2.graphics_EgoLockFrames;
                    int index26 = index25;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index25 + 1;
                    string name = "graphics_EgoLockFrames_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    Graphic graphic = (Graphic)(type.GetField(name)).GetValue((object)battleDiceCardUi2);
                    graphicsEgoLockFrames[index26] = graphic;
                }
                for (int index27 = 0; index27 < 1; ++index27)
                {
                    RefineHsv[] hsvEgoLockFrames = battleDiceCardUi2.hsv_EgoLockFrames;
                    int index28 = index27;
                    System.Type type = typeof(LogLikeMod.UILogBattleDiceCardUI);
                    num = index27 + 1;
                    string name = "hsv_EgoLockFrames_" + num.ToString();
                    // ISSUE: explicit non-virtual call
                    RefineHsv refineHsv = (RefineHsv)(type.GetField(name)).GetValue((object)battleDiceCardUi2);
                    hsvEgoLockFrames[index28] = refineHsv;
                }
                UnityEngine.Object.Destroy((UnityEngine.Object)original);
                UnityEngine.Object.Destroy((UnityEngine.Object)battleDiceCardUi2.gameObject.GetComponent<BattleDiceCardUI>());
                battleDiceCardUi2.gameObject.AddComponent<FrameDummy>();
                return battleDiceCardUi2;
            }

            public void EnableAddedIcons()
            {
                List<BattleDiceCardBuf> bufList = this._cardModel.GetBufList();
                int index1 = 0;
                foreach (BattleDiceCardBuf cardBuf in bufList)
                {
                    if (index1 < this.bufIconListUI.Length)
                    {
                        if (!((UnityEngine.Object)cardBuf.GetBufIcon() == (UnityEngine.Object)null))
                        {
                            this.bufIconListUI[index1].SetBufIcon(cardBuf);
                            this.bufIconListUI[index1].SetEnable(true);
                            ++index1;
                        }
                    }
                    else
                        break;
                }
                for (int index2 = index1; index2 < this.bufIconListUI.Length; ++index2)
                    this.bufIconListUI[index2].SetEnable(false);
            }

            public void SetEgoFrameLockColor(bool on)
            {
                Color color = on ? UIColorManager.Manager.GetUIColor(UIColor.Disabled) : Color.white;
                foreach (Graphic graphicsEgoLockFrame in this.graphics_EgoLockFrames)
                {
                    if (!((UnityEngine.Object)graphicsEgoLockFrame == (UnityEngine.Object)null))
                        graphicsEgoLockFrame.color = color;
                }
                float num = on ? 0.0f : 1f;
                foreach (RefineHsv hsvEgoLockFrame in this.hsv_EgoLockFrames)
                {
                    hsvEgoLockFrame._Saturation = num;
                    hsvEgoLockFrame.CallUpdate();
                }
                foreach (RefineHsv hsvBehaviourIcon in this.hsv_behaviourIcons)
                {
                    hsvBehaviourIcon._Saturation = num;
                    hsvBehaviourIcon.CallUpdate();
                }
            }

            public void SetEgoLock()
            {
                this.isEgoCoolTimeLock = true;
                Color uiColor = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                this.SetFrameColor(uiColor);
                this.SetLinearDodgeColor(uiColor);
                this.SetEgoFrameLockColor(true);
                this.SetRangeIconHsv(new Vector3(0.0f, 0.0f, 1f));
            }

            public void SetEgoCoolTimeGauge()
            {
                if ((UnityEngine.Object)this.ob_EgoCoolTime == (UnityEngine.Object)null)
                    return;
                if (!this._cardModel.XmlData.IsEgo())
                {
                    this.ob_EgoCoolTime.SetActive(false);
                    this.SetEgoFrameLockColor(false);
                }
                else if ((this.isProfileCard || this.isEmotionSelectedPopup) && (UnityEngine.Object)this.ob_EgoCoolTime != (UnityEngine.Object)null && this.ob_EgoCoolTime.activeSelf)
                {
                    this.ob_EgoCoolTime.gameObject.SetActive(false);
                }
                else
                {
                    this.ob_EgoCoolTime.gameObject.SetActive(true);
                    float num = 0.0f;
                    if ((double)this._cardModel.MaxCooltimeValue != 0.0)
                        num = this._cardModel.CurrentCooltimeValue / this._cardModel.MaxCooltimeValue;
                    if ((double)num >= 1.0)
                    {
                        this.ob_EgoCoolTime.gameObject.SetActive(false);
                        this.SetEgoFrameLockColor(false);
                    }
                    else
                    {
                        this.rect_Gauge.anchoredPosition = new Vector2(this.gaugeLength * num, 0.0f);
                        this.hsv_bgGlow.CallUpdate();
                        if ((double)num < 0.699999988079071)
                        {
                            Color color = this.img_BgGlow.color;
                            color.a = 1f;
                            this.img_BgGlow.color = color;
                            this.anim_gaugebgglow.enabled = false;
                            this.hsv_bgGlow._ValueBrightness = 0.3f;
                            Color color2 = Color.white;
                            color2.a = 0.3f;
                            this.img_Bg.color = color2;
                        }
                        else
                        {
                            this.hsv_bgGlow._ValueBrightness = num * 1.2f;
                            this.anim_gaugebgglow.enabled = true;
                            Color color3 = Color.white;
                            color3.a = 0.7f;
                            this.img_Bg.color = color3;
                        }
                        this.SetEgoLock();
                    }
                }
            }

            public void SetRangeIconHsv(Vector3 hsvvalue)
            {
                this.img_icon.color = Color.white;
                if ((UnityEngine.Object)this.hsv_rangeIcon == (UnityEngine.Object)null)
                    this.hsv_rangeIcon = this.img_icon.GetComponent<RefineHsv>();
                if ((UnityEngine.Object)this.hsv_rangeIcon == (UnityEngine.Object)null)
                {
                    Debug.LogError((object)"Hsv Not Reference");
                }
                else
                {
                    this.hsv_rangeIcon._HueShift = hsvvalue.x;
                    this.hsv_rangeIcon._Saturation = hsvvalue.y;
                    this.hsv_rangeIcon._ValueBrightness = hsvvalue.z;
                    this.hsv_rangeIcon.CallUpdate();
                }
            }

            public void SetLinearDodgeColor(Color c)
            {
                for (int index = 0; index < this.img_linearDodges.Length; ++index)
                    this.img_linearDodges[index].color = c;
            }

            public void SetFrameColor(Color c)
            {
                for (int index = 0; index < this.img_Frames.Length; ++index)
                    this.img_Frames[index].color = c;
                if (this._cost == this._originCost)
                {
                    this.costNumbers.SetContentColor(c);
                    this.hsv_Cost._HueShift = 0.0f;
                    this.hsv_Cost.CallUpdate();
                }
                else if (this._cost < this._originCost)
                {
                    this.costNumbers.SetContentColor(Color.white);
                    this.hsv_Cost._HueShift = 0.0f;
                    this.hsv_Cost.CallUpdate();
                }
                else
                {
                    if (this._cost <= this._originCost)
                        return;
                    this.costNumbers.SetContentColor(Color.white);
                    this.hsv_Cost._HueShift = 150f;
                    this.hsv_Cost.CallUpdate();
                }
            }

            public void SetDefaultPreviewResistText()
            {
                foreach (TMP_Text tmpText in this.txt_Resist)
                    tmpText.text = "";
                foreach (TMP_Text tmpText in this.txt_bpResist)
                    tmpText.text = "";
                foreach (RefineHsv hsvBehaviourIcon in this.hsv_behaviourIcons)
                {
                    hsvBehaviourIcon._Saturation = 1f;
                    hsvBehaviourIcon.CallUpdate();
                }
            }

            public void SetCard(BattleDiceCardModel cardModel)
            {
                bool __state = false;
                if (cardModel != this._cardModel)
                {
                    __state = true;
                }
                BCEVLoglikeExtensions.ConfigureBattleCard(this, cardModel.GetBehaviourList().Count, cardModel != this._cardModel);
                this.egoxmldata = (EmotionEgoXmlInfo)null;
                this._cardModel = cardModel;
                bool flag = this._cardModel.XmlData.IsEgo();
                foreach (GameObject obNormalFrame in this.ob_NormalFrames)
                    obNormalFrame.SetActive(!flag);
                foreach (GameObject obEgoFrame in this.ob_EgoFrames)
                    obEgoFrame.SetActive(flag);
                if (LorId.IsModId(cardModel.XmlData.workshopID))
                    this.txt_cardName.text = cardModel.XmlData.workshopName;
                else
                    this.txt_cardName.text = Singleton<BattleCardDescXmlList>.Instance.GetCardName(cardModel.GetTextId());
                this.SetDefaultPreviewResistText();
                this._cost = this._cardModel.GetCost();
                this._originCost = this._cardModel.GetOriginCost();
                Sprite[] sp = this.costNumberSprite;
                if ((this._cost < this._originCost ? 1 : (this._cost > this._originCost ? 1 : 0)) != 0)
                    sp = UISpriteDataManager.instance.CardCostAddGlow;
                this.costNumbers.SetOneValue(this._cardModel.GetCost(), sp);
                this.img_icon.sprite = UISpriteDataManager.instance.GetRangeIconSprite(cardModel.GetSpec().Ranged);
                this.img_icon.rectTransform.anchoredPosition = flag ? this.rangeIconEgoPos : this.rangeIconOriginPos;
                List<DiceBehaviour> behaviourList = cardModel.GetBehaviourList();
                int b = 4 - behaviourList.Count;
                string text = Singleton<BattleCardDescXmlList>.Instance.GetAbilityDesc(cardModel.GetID());
                if (string.IsNullOrEmpty(text))
                {
                    List<string> abilityDesc = Singleton<BattleCardAbilityDescXmlList>.Instance.GetAbilityDesc(cardModel.XmlData);
                    if (abilityDesc.Count > 0)
                        text = string.Join("\n", (IEnumerable<string>)abilityDesc);
                }
                else
                {
                    string abilityDescString = Singleton<BattleCardAbilityDescXmlList>.Instance.GetDefaultAbilityDescString(cardModel.XmlData);
                    if (!string.IsNullOrEmpty(abilityDescString))
                        text = $"{abilityDescString}\n{text}";
                }
                if (!string.IsNullOrEmpty(text))
                {
                    this.selfAbilityArea.SetActive(true);
                    this.txt_selfAbility.text = TextUtil.TransformConditionKeyword(text);
                    float preferredHeight = this.txt_selfAbility.preferredHeight;
                    int num = Mathf.Min((double)preferredHeight >= 260.0 ? ((double)preferredHeight >= 480.0 ? ((double)preferredHeight >= 700.0 ? 3 : 2) : 1) : 0, b);
                    RectTransform component = this.selfAbilityArea.GetComponent<RectTransform>();
                    if ((UnityEngine.Object)component != (UnityEngine.Object)null)
                    {
                        switch (num)
                        {
                            case 1:
                                component.sizeDelta = new Vector2(component.sizeDelta.x, 440f);
                                break;
                            case 2:
                                component.sizeDelta = new Vector2(component.sizeDelta.x, 660f);
                                break;
                            case 3:
                                component.sizeDelta = new Vector2(component.sizeDelta.x, 880f);
                                break;
                            default:
                                component.sizeDelta = new Vector2(component.sizeDelta.x, 220f);
                                break;
                        }
                    }
                }
                else
                    this.selfAbilityArea.SetActive(false);
                for (int index = 0; index < behaviourList.Count; ++index)
                {
                    this.ui_behaviourDescList[index].SetBehaviourInfo(behaviourList[index], cardModel.GetID(), cardModel.GetBehaviourList());
                    this.ui_behaviourDescList[index].gameObject.SetActive(true);
                    Sprite sprite = behaviourList[index].Type == BehaviourType.Standby ? UISpriteDataManager.instance.CardStandbyBehaviourDetailIcons[(int)behaviourList[index].Detail] : UISpriteDataManager.instance._cardBehaviourDetailIcons[(int)behaviourList[index].Detail];
                    this.img_behaviourDetatilList[index].sprite = sprite;
                    this.img_behaviourDetatilList[index].gameObject.SetActive(true);
                }
                for (int count = behaviourList.Count; count < this.ui_behaviourDescList.Count; ++count)
                {
                    this.ui_behaviourDescList[count].gameObject.SetActive(false);
                    this.img_behaviourDetatilList[count].gameObject.SetActive(false);
                }
                this.colorFrame = flag ? UIColorManager.Manager.CardEgoCostColor : UIColorManager.Manager.GetCardRarityColor(cardModel.GetRarity());
                this.colorLineardodge = flag ? UIColorManager.Manager.CardEgoLinearColor : UIColorManager.Manager.GetCardRarityLinearColor(cardModel.GetRarity());
                this.colorLineardodge_deactive = this.colorLineardodge;
                this.colorLineardodge_deactive.a = 1f;
                this.hsv_Cost._ValueBrightness = flag ? 1.5f : 1f;
                this.hsv_Cost.CallUpdate();
                this.SetFrameColor(this.colorFrame);
                this.SetLinearDodgeColor(this.colorLineardodge);
                this.SetRangeIconHsv(flag ? UIColorManager.Manager.CardRangeHsvValue[5] : UIColorManager.Manager.CardRangeHsvValue[(int)this._cardModel.GetRarity()]);
                this.KeywordListUI.Activate();
                this.KeywordListUI.Init(this._cardModel.XmlData, this._cardModel.GetBehaviourList());
                if (this._editor)
                    return;
                Sprite sprite1 = !LorId.IsModId(cardModel.XmlData.workshopID) ? Singleton<AssetBundleManagerRemake>.Instance.LoadCardSprite(cardModel.GetArtworkSrc()) : Singleton<CustomizingCardArtworkLoader>.Instance.GetSpecificArtworkSprite(cardModel.XmlData.workshopID, cardModel.GetArtworkSrc());
                if ((UnityEngine.Object)sprite1 != (UnityEngine.Object)null)
                    this.img_artwork.sprite = sprite1;
                else
                    Debug.Log((object)"Can't find sprite");
                this.isEgoCoolTimeLock = false;
                this.SetEgoCoolTimeGauge();
                this.EnableAddedIcons();
                BCEVLoglikeExtensions.ForceUpdateLeftBehaviourMaterial(this);
                if (cardModel != null && this.isProfileCard)
                {
                    Vector3 localPosition = this.KeywordListUI.transform.localPosition;
                    if (this.transform.position.x < 0f)
                    {
                        localPosition.x = -2200f;
                    }
                    else
                    {
                        localPosition.x = 900f;
                    }
                    this.KeywordListUI.transform.localPosition = localPosition;
                    this.KeywordListUI.Activate();
                    this.KeywordListUI.Init(cardModel.XmlData, cardModel.XmlData.DiceBehaviourList);
                }
                if (__state)
                {
                    LayoutRebuilder.MarkLayoutForRebuild(this.transform as RectTransform);
                }
                if (!this.name.Contains("PreviewCard"))
                {
                    BCEVLoglikeExtensions.SetBattleCardPreview(this, cardModel, new BattleDiceCardUI.Option[] { });
                }
            }

        }

        public class LogLikeBattleDiceCardPreviewUI : MonoBehaviour
        {
            public static LogLikeBattleDiceCardPreviewUI GetOrCreateUI(UILogBattleDiceCardUI mainUI, bool createIfNull)
            {
                LogLikeBattleDiceCardPreviewUI battleDiceCardPreviewUI = mainUI.GetComponent<LogLikeBattleDiceCardPreviewUI>();
                if (battleDiceCardPreviewUI)
                {
                    return battleDiceCardPreviewUI;
                }
                if (!createIfNull)
                {
                    return null;
                }
                battleDiceCardPreviewUI = mainUI.gameObject.AddComponent<LogLikeBattleDiceCardPreviewUI>();
                GameObject gameObject = new GameObject("[Rect]PreviewCardRoot");
                battleDiceCardPreviewUI.previewRoot = gameObject;
                RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
                rectTransform.SetParent(mainUI.transform);
                rectTransform.localScale = Vector3.one * 0.95f;
                rectTransform.localRotation = Quaternion.identity;
                rectTransform.localPosition = new Vector3(0f, 1500f, 0f);
                UILogBattleDiceCardUI battleDiceCardUI = UnityEngine.Object.Instantiate<UILogBattleDiceCardUI>(mainUI, rectTransform);
                battleDiceCardPreviewUI.cardObject = battleDiceCardUI;
                RectTransform rectTransform2 = battleDiceCardUI.transform as RectTransform;
                rectTransform2.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform2.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform2.localPosition = new Vector2(0f, -50f);
                rectTransform2.localScale = (mainUI.isProfileCard ? Vector3.one : (Vector3.one * 0.8f));
                battleDiceCardUI.scaleOrigin = rectTransform2.localScale;
                rectTransform2.localRotation = Quaternion.identity;
                battleDiceCardUI.name = "[Rect]PreviewCardBattle";
                GameObject gameObject2 = new GameObject("[Rect]PreviewSelectArrows");
                battleDiceCardPreviewUI.arrowsRoot = gameObject2;
                RectTransform rectTransform3 = gameObject2.AddComponent<RectTransform>();
                rectTransform3.SetParent(rectTransform);
                rectTransform3.pivot = new Vector2(0.5f, 0.65f);
                rectTransform3.localRotation = Quaternion.identity;
                rectTransform3.localPosition = new Vector3(0f, 750f, 0f);
                rectTransform3.localScale = new Vector3(5f, 5f, 1f);
                rectTransform3.sizeDelta = new Vector2(50f, 40f);
                gameObject2.AddComponent<Image>().sprite = CardViewInternals.GetArrowsBgSprite();
                GameObject gameObject3 = new GameObject("[Image]Left");
                RectTransform rectTransform4 = gameObject3.AddComponent<RectTransform>();
                rectTransform4.SetParent(rectTransform3);
                rectTransform4.localScale = Vector3.one;
                rectTransform4.localRotation = Quaternion.Euler(0f, 0f, -90f);
                rectTransform4.sizeDelta = new Vector2(40f, 30f);
                rectTransform4.anchoredPosition3D = new Vector3(-10f, 0f, 0f);
                Image image = gameObject3.AddComponent<Image>();
                image.sprite = CardViewInternals.GetArrowSprite();
                battleDiceCardPreviewUI.prevArrow = image;
                GameObject gameObject4 = new GameObject("[Image]Right");
                RectTransform rectTransform5 = gameObject4.AddComponent<RectTransform>();
                rectTransform5.SetParent(rectTransform3);
                rectTransform5.localScale = Vector3.one;
                rectTransform5.localRotation = Quaternion.Euler(0f, 0f, 90f);
                rectTransform5.sizeDelta = new Vector2(40f, 30f);
                rectTransform5.anchoredPosition3D = new Vector3(10f, 0f, 0f);
                Image image2 = gameObject4.AddComponent<Image>();
                image2.sprite = CardViewInternals.GetArrowSprite();
                battleDiceCardPreviewUI.nextArrow = image2;
                Graphic[] componentsInChildren = gameObject.GetComponentsInChildren<Graphic>(true);
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].raycastTarget = false;
                }
                battleDiceCardPreviewUI.Awake();
                return battleDiceCardPreviewUI;
            }

            public void SetCardList(List<BattleDiceCardModel> previewList, BattleDiceCardUI.Option[] options)
            {
                if (previewList == null || previewList.Count == 0)
                {
                    this.previewRoot.SetActive(false);
                    this.cardList = null;
                    return;
                }
                this.cardList = previewList;
                this.curIndex = 0;
                this.curOptions = options;
                if (this.cardList.Count == 1)
                {
                    this.arrowsRoot.SetActive(false);
                }
                else
                {
                    this.arrowsRoot.SetActive(true);
                    this.prevArrow.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                    this.nextArrow.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
                }
                if (this.cardObject.isProfileCard)
                {
                    this.SetCard(previewList[0], options);
                    return;
                }
                this.previewRoot.SetActive(false);
            }
            public void NextCard()
            {
                if (this.cardList == null)
                {
                    return;
                }
                if (this.curIndex >= this.cardList.Count - 1)
                {
                    return;
                }
                this.curIndex++;
                this.SetCard(this.cardList[this.curIndex], this.curOptions);
                this.prevArrow.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
                if (this.curIndex == this.cardList.Count - 1)
                {
                    this.nextArrow.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                }
            }
            public void PrevCard()
            {
                if (this.cardList == null)
                {
                    return;
                }
                if (this.curIndex <= 0)
                {
                    return;
                }
                this.curIndex--;
                this.SetCard(this.cardList[this.curIndex], this.curOptions);
                this.nextArrow.color = UIColorManager.Manager.GetUIColor(UIColor.Default);
                if (this.curIndex == 0)
                {
                    this.prevArrow.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                }
            }

            public void ShowCard()
            {
                if (this.cardList == null || this.curIndex < 0 || this.curIndex >= this.cardList.Count)
                {
                    return;
                }
                this.SetCard(this.cardList[this.curIndex], this.curOptions);
            }

            public void HideCard()
            {
                this.SetCard(null, null);
            }
            public void SetCard(BattleDiceCardModel card, BattleDiceCardUI.Option[] options)
            {
                if (card == null)
                {
                    this.previewRoot.SetActive(false);
                    return;
                }
                this.previewRoot.SetActive(true);
                this.cardObject.SetCard(card);
            }
            public void Awake()
            {
                if (this.cardObject)
                {
                    foreach (object obj in this.cardObject.transform)
                    {
                        Transform transform = (Transform)obj;
                        if (transform.name.Contains("PreviewCard"))
                        {
                            UnityEngine.Object.Destroy(transform.gameObject);
                        }
                    }
                    BattleDiceCardPreviewUI component = this.cardObject.GetComponent<BattleDiceCardPreviewUI>();
                    if (component)
                    {
                        UnityEngine.Object.Destroy(component);
                    }
                }
            }

            public void Update()
            {
                if (this.previewRoot.activeSelf)
                {
                    if (this.cardObject.isProfileCard)
                    {
                        if (this.previewRoot.transform.localPosition.y > 0f)
                        {
                            Vector3 localPosition = this.previewRoot.transform.localPosition;
                            localPosition.y = -localPosition.y;
                            this.previewRoot.transform.localPosition = localPosition;
                        }
                    }
                    else if (this.previewRoot.transform.localPosition.y < 0f)
                    {
                        Vector3 localPosition2 = this.previewRoot.transform.localPosition;
                        localPosition2.y = -localPosition2.y;
                        this.previewRoot.transform.localPosition = localPosition2;
                    }
                    if (Input.GetKeyDown(CardViewInternals.NextCardKey))
                    {
                        this.NextCard();
                        this.cooldown = CardViewInternals.CardChangeHoldDelay;
                        return;
                    }
                    if (Input.GetKeyDown(CardViewInternals.PrevCardKey))
                    {
                        this.PrevCard();
                        this.cooldown = CardViewInternals.CardChangeHoldDelay;
                        return;
                    }
                    bool key = Input.GetKey(CardViewInternals.NextCardKey);
                    bool key2 = Input.GetKey(CardViewInternals.PrevCardKey);
                    if (!key && !key2)
                    {
                        this.cooldown = 0f;
                        return;
                    }
                    this.cooldown -= Time.deltaTime;
                    if (this.cooldown > 0f)
                    {
                        return;
                    }
                    if (key && key2)
                    {
                        return;
                    }
                    this.cooldown = CardViewInternals.CardChangeRepeatHoldDelay;
                    if (key)
                    {
                        this.NextCard();
                        return;
                    }
                    this.PrevCard();
                }
            }

            [SerializeField]
            public UILogBattleDiceCardUI cardObject;

            [SerializeField]
            public Image nextArrow;

            [SerializeField]
            public Image prevArrow;

            [SerializeField]
            public GameObject arrowsRoot;

            [SerializeField]
            public GameObject previewRoot;

            public List<BattleDiceCardModel> cardList;

            public int curIndex;

            public BattleDiceCardUI.Option[] curOptions;

            public float cooldown;
        }



        /// <summary>
        /// This is quite literally just a bunch of code adapted from BattleCardEnhancedView<br></br> 
        /// made to work with abcdcode's custom card UI class.
        /// </summary>
        public static class BCEVLoglikeExtensions
        {
            public static void ConfigureBattleCard(UILogBattleDiceCardUI __instance, int cardCount, bool resetScrolls)
            {
                UICardSlotScroller uicardSlotScroller = __instance.GetComponent<UICardSlotScroller>();
                CardViewPatches.ConfigureKeywordList(__instance.KeywordListUI, __instance.gameObject, ref uicardSlotScroller, true);
                RectTransform rectTransform = __instance.img_behaviourDetatilList[0].transform.parent as RectTransform;
                RectTransform rectTransform2 = rectTransform.parent as RectTransform;
                if (!rectTransform2.GetComponent<ScrollRect>())
                {
                    Vector2 anchoredPosition = rectTransform.anchoredPosition;
                    anchoredPosition.x = 0f;
                    if (__instance.isProfileCard)
                    {
                        if (anchoredPosition.y < -400f)
                        {
                            anchoredPosition.y = -400f;
                        }
                        foreach (TextMeshProUGUI textMeshProUGUI in __instance.txt_Resist)
                        {
                            textMeshProUGUI.transform.localPosition = new Vector2(-10f, -130f);
                        }
                        foreach (TextMeshProUGUI textMeshProUGUI2 in __instance.txt_bpResist)
                        {
                            textMeshProUGUI2.transform.localPosition = new Vector2(-20f, -220f);
                        }
                    }
                    rectTransform.anchoredPosition = anchoredPosition;
                    GameObject gameObject = new GameObject("[RectMask]BehaviourDetailList");
                    RectTransform rectTransform3 = gameObject.AddComponent<RectTransform>();
                    rectTransform3.SetParent(rectTransform2);
                    rectTransform3.SetSiblingIndex(rectTransform.GetSiblingIndex());
                    rectTransform3.localScale = Vector3.one;
                    rectTransform3.localRotation = Quaternion.identity;
                    rectTransform3.anchoredPosition3D = new Vector3(0f, -500f, 0f);
                    rectTransform3.sizeDelta = new Vector2(870f, 1000f);
                    gameObject.AddComponent<Image>().raycastTarget = false;
                    gameObject.AddComponent<Mask>().showMaskGraphic = false;
                    rectTransform.SetParent(rectTransform3, true);
                    rectTransform2.localScale = Vector3.one;
                    GameObject gameObject2 = new GameObject("[ScrollRect]BehaviourDetailList");
                    RectTransform rectTransform4 = gameObject2.AddComponent<RectTransform>();
                    rectTransform4.SetParent(rectTransform3);
                    rectTransform4.localRotation = rectTransform.localRotation;
                    rectTransform4.localScale = Vector3.one;
                    rectTransform4.pivot = rectTransform.pivot;
                    rectTransform4.anchorMin = rectTransform.anchorMin;
                    rectTransform4.anchorMax = rectTransform.anchorMax;
                    rectTransform4.anchoredPosition3D = rectTransform.anchoredPosition3D;
                    rectTransform4.sizeDelta = new Vector2(925f, rectTransform.sizeDelta.y);
                    rectTransform.SetParent(rectTransform4, true);
                    rectTransform2.localScale = Vector3.one;
                    rectTransform.pivot = new Vector2(0f, 0.5f);
                    if (!rectTransform.GetComponent<ContentSizeFitter>())
                    {
                        ContentSizeFitter contentSizeFitter = rectTransform.gameObject.AddComponent<ContentSizeFitter>();
                        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
                        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                    }
                    ScrollRect scrollRect = gameObject2.AddComponent<ScrollRect>();
                    scrollRect.vertical = false;
                    scrollRect.horizontal = true;
                    scrollRect.content = rectTransform;
                    scrollRect.scrollSensitivity = 250f;
                    scrollRect.movementType = ScrollRect.MovementType.Clamped;
                    if (!uicardSlotScroller)
                    {
                        uicardSlotScroller = __instance.gameObject.AddComponent<UICardSlotScroller>();
                    }
                    ScrollRectHandler scrollRectHandler = gameObject2.AddComponent<ScrollRectHandler>();
                    scrollRectHandler.scrollRect = scrollRect;
                    scrollRectHandler.axis = RectTransform.Axis.Horizontal;
                    uicardSlotScroller.scrollHandlers.Add(scrollRectHandler);
                }
                RectTransform rectTransform5 = __instance.ui_behaviourDescList[0].transform.parent as RectTransform;
                RectTransform rectTransform6 = rectTransform5.parent as RectTransform;
                if (!rectTransform6.GetComponent<ScrollRect>())
                {
                    float num = CardViewInternals.Config.CardAbilityMaxFontSize * 10f;
                    float num2 = CardViewInternals.Config.CardAbilityMinFontSize * 10f;
                    float num3 = num2 * (float)CardViewInternals.Config.CardAbilityMaxBaseLines;
                    List<AbilityDescSizeController> list = null;
                    if (!CardViewInternals.Config.DisableDiceLayoutChange)
                    {
                        list = new List<AbilityDescSizeController>();
                        foreach (BattleDiceCard_BehaviourDescUI battleDiceCard_BehaviourDescUI in __instance.ui_behaviourDescList)
                        {
                            RectTransform rectTransform7 = battleDiceCard_BehaviourDescUI.transform as RectTransform;
                            rectTransform7.pivot = new Vector2(0.5f, 1f);
                            TextMeshProUGUI txt_range = battleDiceCard_BehaviourDescUI.txt_range;
                            RectTransform rectTransform8 = txt_range.transform as RectTransform;
                            if (rectTransform8.parent != rectTransform7)
                            {
                                rectTransform8.SetParent(rectTransform7, true);
                            }
                            if (rectTransform8.anchorMin.y == 0.5f && rectTransform8.anchorMax.y == 0.5f)
                            {
                                Vector2 anchoredPosition2 = rectTransform8.anchoredPosition;
                                anchoredPosition2.y = 0f;
                                rectTransform8.anchoredPosition = anchoredPosition2;
                            }
                            RectTransform rectTransform9 = battleDiceCard_BehaviourDescUI.img_detail.transform as RectTransform;
                            while (rectTransform9 && rectTransform9 != rectTransform7)
                            {
                                if (rectTransform9.anchorMin.y == 0.5f && rectTransform9.anchorMax.y == 0.5f)
                                {
                                    Vector2 anchoredPosition3 = rectTransform9.anchoredPosition;
                                    anchoredPosition3.y = 0f;
                                    rectTransform9.anchoredPosition = anchoredPosition3;
                                }
                                rectTransform9 = (rectTransform9.parent as RectTransform);
                            }
                            rectTransform7.sizeDelta = new Vector2(rectTransform7.sizeDelta.x, num3);
                            foreach (object obj in rectTransform7)
                            {
                                RectTransform rectTransform10 = ((Transform)obj) as RectTransform;
                                if (rectTransform10 != null && rectTransform10.anchorMin.y == 0.5f && rectTransform10.anchorMax.y == 0.5f)
                                {
                                    Vector3 localPosition = rectTransform10.localPosition;
                                    rectTransform10.anchorMin = new Vector2(rectTransform10.anchorMin.x, 1f);
                                    rectTransform10.anchorMax = new Vector2(rectTransform10.anchorMax.x, 1f);
                                    rectTransform10.localPosition = localPosition;
                                }
                            }
                            txt_range.alignment = TextAlignmentOptions.Left;
                            txt_range.fontSizeMax = 100f;
                            txt_range.enableWordWrapping = true;
                            rectTransform8.anchorMin = new Vector2(0f, 1f);
                            rectTransform8.offsetMin = new Vector2(150f, -num3);
                            rectTransform8.anchorMax = new Vector2(1f, 1f);
                            rectTransform8.offsetMax = new Vector2(0f, 0f);
                            TextMeshProUGUI txt_ability = battleDiceCard_BehaviourDescUI.txt_ability;
                            RectTransform rectTransform11 = txt_ability.transform as RectTransform;
                            txt_ability.fontSizeMax = num;
                            txt_ability.fontSizeMin = num2;
                            txt_ability.alignment = TextAlignmentOptions.Left;
                            rectTransform11.transform.localScale = Vector3.one;
                            rectTransform11.pivot = new Vector2(0.5f, 1f);
                            rectTransform11.anchorMin = new Vector2(0f, 1f);
                            rectTransform11.offsetMin = new Vector2(300f, -(num3 + num2 * 0.1f));
                            rectTransform11.anchorMax = new Vector2(1f, 1f);
                            rectTransform11.offsetMax = new Vector2(0f, num2 * 0.1f);
                            GameObject gameObject3 = new GameObject("[Text]Behaviour_Ability_Overflow");
                            RectTransform rectTransform12 = gameObject3.AddComponent<RectTransform>();
                            rectTransform12.SetParent(rectTransform8);
                            rectTransform12.localScale = Vector3.one;
                            rectTransform12.localRotation = Quaternion.identity;
                            rectTransform12.pivot = new Vector2(0.5f, 1f);
                            rectTransform12.anchorMin = new Vector2(0f, 0f);
                            rectTransform12.offsetMin = new Vector2(-50f, -50f);
                            rectTransform12.anchorMax = new Vector2(1f, 0f);
                            rectTransform12.offsetMax = new Vector2(0f, 0f);
                            TextMeshProUGUI textMeshProUGUI3 = gameObject3.AddComponent<TextMeshProUGUI>();
                            textMeshProUGUI3.fontSize = txt_ability.fontSizeMin;
                            if (txt_ability.font)
                            {
                                textMeshProUGUI3.font = txt_ability.font;
                            }
                            textMeshProUGUI3.color = txt_ability.color;
                            textMeshProUGUI3.overflowMode = TextOverflowModes.Overflow;
                            textMeshProUGUI3.alignment = TextAlignmentOptions.Left;
                            textMeshProUGUI3.ignoreRectMaskCulling = true;
                            txt_ability.overflowMode = TextOverflowModes.Linked;
                            txt_ability.linkedTextComponent = textMeshProUGUI3;
                            AbilityDescSizeController abilityDescSizeController = battleDiceCard_BehaviourDescUI.gameObject.AddComponent<AbilityDescSizeController>();
                            abilityDescSizeController.rect = rectTransform7;
                            abilityDescSizeController.baselineText = txt_range;
                            abilityDescSizeController.abilityTextFixed = txt_ability;
                            abilityDescSizeController.abilityTextFlex = textMeshProUGUI3;
                            abilityDescSizeController.abilityTextMinHeight = 75f;
                            abilityDescSizeController.fixedAbilityTextHSpacing = 20f;
                            abilityDescSizeController.flexCheck = new Vector2(-50f, -50f);
                            list.Add(abilityDescSizeController);
                        }
                        RectTransform rectTransform13 = __instance.selfAbilityArea.transform as RectTransform;
                        RectTransform rectTransform14 = __instance.txt_selfAbility.transform as RectTransform;
                        rectTransform13.pivot = new Vector2(0.5f, 1f);
                        rectTransform14.anchorMin = new Vector2(rectTransform14.anchorMin.x, 1f);
                        rectTransform14.anchorMax = new Vector2(rectTransform14.anchorMax.x, 1f);
                        rectTransform14.pivot = new Vector2(rectTransform14.pivot.x, 1f);
                        rectTransform14.anchoredPosition = new Vector2(rectTransform14.anchoredPosition.x, 0f);
                        rectTransform14.offsetMin = new Vector2(rectTransform14.offsetMin.x - 100f, rectTransform14.offsetMin.y);
                        __instance.txt_selfAbility.fontSizeMax = num;
                        __instance.txt_selfAbility.fontSizeMin = num2;
                        __instance.txt_selfAbility.overflowMode = TextOverflowModes.Overflow;
                        AbilityDescSizeController abilityDescSizeController2 = __instance.selfAbilityArea.gameObject.AddComponent<AbilityDescSizeController>();
                        abilityDescSizeController2.rect = rectTransform13;
                        abilityDescSizeController2.abilityTextFlex = __instance.txt_selfAbility;
                        abilityDescSizeController2.abilityTextMinHeight = num;
                        list.Add(abilityDescSizeController2);
                    }
                    GameObject gameObject4 = new GameObject("[ScrollRect]BehaviourList");
                    RectTransform rectTransform15 = gameObject4.AddComponent<RectTransform>();
                    rectTransform15.SetParent(rectTransform6);
                    rectTransform15.localScale = rectTransform5.localScale;
                    rectTransform15.localRotation = rectTransform5.localRotation;
                    rectTransform15.anchorMin = rectTransform5.anchorMin;
                    rectTransform15.anchorMax = rectTransform5.anchorMax;
                    VerticalLayoutGroup component = rectTransform5.GetComponent<VerticalLayoutGroup>();
                    int top = component.padding.top;
                    component.padding.top = 30;
                    component.padding.bottom = 50;
                    if (component.spacing < 15f)
                    {
                        component.spacing = 15f;
                    }
                    Vector2 offsetMin = rectTransform5.offsetMin;
                    Vector2 offsetMax = rectTransform5.offsetMax;
                    offsetMax.y -= (float)top;
                    RectTransform rectTransform16 = rectTransform15;
                    RectTransform rectTransform17 = rectTransform5;
                    Vector2 vector = new Vector2(0.5f, 1f);
                    rectTransform17.pivot = vector;
                    rectTransform16.pivot = vector;
                    RectTransform rectTransform18 = rectTransform15;
                    vector = (rectTransform5.offsetMin = offsetMin);
                    rectTransform18.offsetMin = vector;
                    RectTransform rectTransform19 = rectTransform15;
                    vector = (rectTransform5.offsetMax = offsetMax);
                    rectTransform19.offsetMax = vector;
                    rectTransform5.sizeDelta = new Vector2(rectTransform5.sizeDelta.x, 1340f);
                    rectTransform15.sizeDelta = new Vector2(900f, 1340f);
                    rectTransform5.gameObject.AddComponent<LayoutElement>().minHeight = 1340f;
                    rectTransform5.SetParent(rectTransform15, true);
                    rectTransform5.localScale = Vector3.one;
                    rectTransform5.anchoredPosition3D = Vector3.zero;
                    if (list != null)
                    {
                        AbilityDescListSizeFitter abilityDescListSizeFitter = rectTransform5.gameObject.AddComponent<AbilityDescListSizeFitter>();
                        abilityDescListSizeFitter.subControllers = list;
                        using (List<AbilityDescSizeController>.Enumerator enumerator4 = list.GetEnumerator())
                        {
                            while (enumerator4.MoveNext())
                            {
                                AbilityDescSizeController abilityDescSizeController3 = enumerator4.Current;
                                abilityDescSizeController3.mainController = abilityDescListSizeFitter;
                            }
                            goto IL_B4B;
                        }
                    }
                    ContentSizeFitter contentSizeFitter2 = rectTransform5.gameObject.AddComponent<ContentSizeFitter>();
                    contentSizeFitter2.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                    contentSizeFitter2.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                IL_B4B:
                    ScrollRect scrollRect2 = gameObject4.AddComponent<ScrollRect>();
                    scrollRect2.horizontal = false;
                    scrollRect2.vertical = true;
                    scrollRect2.content = rectTransform5;
                    scrollRect2.scrollSensitivity = 250f;
                    scrollRect2.movementType = ScrollRect.MovementType.Clamped;
                    gameObject4.AddComponent<Image>().raycastTarget = false;
                    gameObject4.AddComponent<Mask>().showMaskGraphic = false;
                    if (!uicardSlotScroller)
                    {
                        uicardSlotScroller = __instance.gameObject.AddComponent<UICardSlotScroller>();
                    }
                    ScrollRectHandler scrollRectHandler2 = gameObject4.AddComponent<ScrollRectHandler>();
                    scrollRectHandler2.scrollRect = scrollRect2;
                    uicardSlotScroller.scrollHandlers.Add(scrollRectHandler2);
                }
                int count = __instance.img_behaviourDetatilList.Count;
                if (count < cardCount)
                {
                    Image image = __instance.img_behaviourDetatilList[0];
                    for (int i = count; i < cardCount; i++)
                    {
                        Image image2 = UnityEngine.Object.Instantiate<Image>(image, image.transform.parent);
                        image2.name = string.Format("[Image]Behaviour_Detail{0} (1)", i + 1);
                        __instance.img_behaviourDetatilList.Add(image2);
                        __instance.hsv_behaviourIcons.Add(image2.GetComponent<RefineHsv>());
                        TextMeshProUGUI[] componentsInChildren = image2.GetComponentsInChildren<TextMeshProUGUI>(true);
                        if (__instance.isProfileCard)
                        {
                            componentsInChildren[0].transform.localPosition = new Vector2(-10f, -130f);
                            componentsInChildren[1].transform.localPosition = new Vector2(-20f, -220f);
                        }
                        __instance.txt_Resist.Add(componentsInChildren[0]);
                        __instance.txt_bpResist.Add(componentsInChildren[1]);
                    }
                }
                int count2 = __instance.ui_behaviourDescList.Count;
                if (count2 < cardCount)
                {
                    BattleDiceCard_BehaviourDescUI battleDiceCard_BehaviourDescUI2 = __instance.ui_behaviourDescList[0];
                    for (int j = count2; j < cardCount; j++)
                    {
                        BattleDiceCard_BehaviourDescUI battleDiceCard_BehaviourDescUI3 = UnityEngine.Object.Instantiate<BattleDiceCard_BehaviourDescUI>(battleDiceCard_BehaviourDescUI2, battleDiceCard_BehaviourDescUI2.transform.parent);
                        battleDiceCard_BehaviourDescUI3.name = string.Format("[Image]Behaviour_Baseline ({0})", j);
                        __instance.ui_behaviourDescList.Add(battleDiceCard_BehaviourDescUI3);
                    }
                }
                if (uicardSlotScroller)
                {
                    if (__instance.isProfileCard)
                    {
                        uicardSlotScroller.isFocused = true;
                    }
                    if (resetScrolls)
                    {
                        if (uicardSlotScroller.scrollHandlers.Exists((ScrollRectHandler h) => h.axis == RectTransform.Axis.Horizontal))
                        {
                            if (cardCount <= 5)
                            {
                                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                            }
                            else
                            {
                                rectTransform.pivot = new Vector2(0f, 0.5f);
                            }
                            rectTransform.anchoredPosition = Vector2.zero;
                        }
                        uicardSlotScroller.ResetScrolls();
                    }
                }
            }

            public static void SetBattleCardPreview(UILogBattleDiceCardUI mainUI, BattleDiceCardModel card, BattleDiceCardUI.Option[] options)
            {
                LogLikeBattleDiceCardPreviewUI orCreateUI;
                if (card != null)
                {
                    List<BattleDiceCardModel> battleCardPreviewList = CardViewInternals.GetBattleCardPreviewList(card, options);
                    if (battleCardPreviewList.Count > 0)
                    {
                        orCreateUI = LogLikeBattleDiceCardPreviewUI.GetOrCreateUI(mainUI, true);
                        orCreateUI.SetCardList(battleCardPreviewList, options);
                        return;
                    }
                }
                orCreateUI = LogLikeBattleDiceCardPreviewUI.GetOrCreateUI(mainUI, false);
                if (orCreateUI)
                {
                    orCreateUI.SetCardList(null, null);
                }
            }
            public static void ForceUpdateLeftBehaviourMaterial(UILogBattleDiceCardUI cardUI)
            {
                GameObject gameObject = cardUI.hsv_behaviourIcons[0].transform.parent.gameObject;
                gameObject.SetActive(false);
                gameObject.SetActive(true);
            }
        }

        public class UILogDetailCardSlot : MonoBehaviour
        {
            public static LogLikeMod.UILogDetailCardSlot Instance;
            public RectTransform Pivot;
            public CanvasGroup cg;
            public GameObject ob_NormalFrame;
            public Image[] img_Frames;
            public Image img_Frames_1;
            public Image img_Frames_2;
            public Image[] img_linearDodge;
            public Image img_linearDodge_1;
            public Image img_linearDodge_2;
            public Image[] img_BehaviourIcons;
            public Image img_BehaviourIcons_1;
            public Image img_BehaviourIcons_2;
            public Image img_BehaviourIcons_3;
            public Image img_BehaviourIcons_4;
            public Image img_BehaviourIcons_5;
            public _2dxFX_GrayScale[] gs_BehaviourIcons;
            public _2dxFX_GrayScale gs_BehaviourIcons_1;
            public _2dxFX_GrayScale gs_BehaviourIcons_2;
            public _2dxFX_GrayScale gs_BehaviourIcons_3;
            public _2dxFX_GrayScale gs_BehaviourIcons_4;
            public _2dxFX_GrayScale gs_BehaviourIcons_5;
            public Image img_RangeIcon;
            public NumbersData costNumbers;
            public TextMeshProUGUI txt_cardName;
            public Image img_Artwork;
            public RefineHsv hsv_rangeIcon;
            public UICustomSelectable selectable;
            public DiceCardItemModel _cardModel;
            public Color colorFrame;
            public Color colorLineardodge;
            public int originSiblingIdx = -1;
            public GameObject ob_selfAbility;
            public TextMeshProUGUI txt_selfAbility;
            public List<UIDetailCardDescSlot> rightDescSlotList;
            public UIDetailCardDescSlot rightDescSlotList_1;
            public UIDetailCardDescSlot rightDescSlotList_2;
            public UIDetailCardDescSlot rightDescSlotList_3;
            public UIDetailCardDescSlot rightDescSlotList_4;
            public UIDetailCardDescSlot rightDescSlotList_5;
            public KeywordListUI keywordListUI_R;
            public KeywordListUI keywordListUI_L;
            public bool OnKeyword = true;

            public static LogLikeMod.UILogDetailCardSlot SlotCopying()
            {
                UIInvenCardListScroll invenCardList = (UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel).EditPanel.BattleCardPanel.InvenCardList;
                UIDetailCardSlot uiDetailCardSlot = (UIDetailCardSlot)typeof(UIInvenCardListScroll).GetField("detailSlot", AccessTools.all).GetValue((object)invenCardList);
                LogLikeMod.UILogDetailCardSlot original = uiDetailCardSlot.gameObject.AddComponent<LogLikeMod.UILogDetailCardSlot>();
                original.Pivot = (RectTransform)typeof(UIOriginCardSlot).GetField("Pivot", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.cg = (CanvasGroup)typeof(UIOriginCardSlot).GetField("cg", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.ob_NormalFrame = (GameObject)typeof(UIOriginCardSlot).GetField("ob_NormalFrame", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.img_Frames = (Image[])typeof(UIOriginCardSlot).GetField("img_Frames", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.img_Frames_1 = original.img_Frames[0];
                original.img_Frames_2 = original.img_Frames[1];
                original.img_linearDodge = (Image[])typeof(UIOriginCardSlot).GetField("img_linearDodge", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.img_linearDodge_1 = original.img_linearDodge[0];
                original.img_linearDodge_2 = original.img_linearDodge[1];
                original.img_BehaviourIcons = (Image[])typeof(UIOriginCardSlot).GetField("img_BehaviourIcons", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.img_BehaviourIcons_1 = original.img_BehaviourIcons[0];
                original.img_BehaviourIcons_2 = original.img_BehaviourIcons[1];
                original.img_BehaviourIcons_3 = original.img_BehaviourIcons[2];
                original.img_BehaviourIcons_4 = original.img_BehaviourIcons[3];
                original.img_BehaviourIcons_5 = original.img_BehaviourIcons[4];
                original.gs_BehaviourIcons = (_2dxFX_GrayScale[])typeof(UIOriginCardSlot).GetField("gs_BehaviourIcons", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.gs_BehaviourIcons_1 = original.gs_BehaviourIcons[0];
                original.gs_BehaviourIcons_2 = original.gs_BehaviourIcons[1];
                original.gs_BehaviourIcons_3 = original.gs_BehaviourIcons[2];
                original.gs_BehaviourIcons_4 = original.gs_BehaviourIcons[3];
                original.gs_BehaviourIcons_5 = original.gs_BehaviourIcons[4];
                original.img_RangeIcon = (Image)typeof(UIOriginCardSlot).GetField("img_RangeIcon", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.costNumbers = (NumbersData)typeof(UIOriginCardSlot).GetField("costNumbers", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.txt_cardName = (TextMeshProUGUI)typeof(UIOriginCardSlot).GetField("txt_cardName", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.img_Artwork = (Image)typeof(UIOriginCardSlot).GetField("img_Artwork", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.hsv_rangeIcon = (RefineHsv)typeof(UIOriginCardSlot).GetField("hsv_rangeIcon", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.selectable = (UICustomSelectable)typeof(UIOriginCardSlot).GetField("selectable", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original._cardModel = (DiceCardItemModel)typeof(UIOriginCardSlot).GetField("_cardModel", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.colorFrame = (Color)typeof(UIOriginCardSlot).GetField("colorFrame", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.colorLineardodge = (Color)typeof(UIOriginCardSlot).GetField("colorLineardodge", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.originSiblingIdx = (int)typeof(UIOriginCardSlot).GetField("originSiblingIdx", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.ob_selfAbility = (GameObject)typeof(UIDetailCardSlot).GetField("ob_selfAbility", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.txt_selfAbility = (TextMeshProUGUI)typeof(UIDetailCardSlot).GetField("txt_selfAbility", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.rightDescSlotList = (List<UIDetailCardDescSlot>)typeof(UIDetailCardSlot).GetField("rightDescSlotList", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.rightDescSlotList_1 = original.rightDescSlotList[0];
                original.rightDescSlotList_2 = original.rightDescSlotList[1];
                original.rightDescSlotList_3 = original.rightDescSlotList[2];
                original.rightDescSlotList_4 = original.rightDescSlotList[3];
                original.rightDescSlotList_5 = original.rightDescSlotList[4];
                original.keywordListUI_R = (KeywordListUI)typeof(UIDetailCardSlot).GetField("keywordListUI_R", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.keywordListUI_L = (KeywordListUI)typeof(UIDetailCardSlot).GetField("keywordListUI_L", AccessTools.all).GetValue((object)uiDetailCardSlot);
                original.OnKeyword = (bool)typeof(UIDetailCardSlot).GetField("OnKeyword", AccessTools.all).GetValue((object)uiDetailCardSlot);
                LogLikeMod.UILogDetailCardSlot logDetailCardSlot = UnityEngine.Object.Instantiate<LogLikeMod.UILogDetailCardSlot>(original, SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.transform);
                logDetailCardSlot.img_Frames[0] = logDetailCardSlot.img_Frames_1;
                logDetailCardSlot.img_Frames[1] = logDetailCardSlot.img_Frames_2;
                logDetailCardSlot.img_linearDodge[0] = logDetailCardSlot.img_linearDodge_1;
                logDetailCardSlot.img_linearDodge[1] = logDetailCardSlot.img_linearDodge_2;
                logDetailCardSlot.img_BehaviourIcons[0] = logDetailCardSlot.img_BehaviourIcons_1;
                logDetailCardSlot.img_BehaviourIcons[1] = logDetailCardSlot.img_BehaviourIcons_2;
                logDetailCardSlot.img_BehaviourIcons[2] = logDetailCardSlot.img_BehaviourIcons_3;
                logDetailCardSlot.img_BehaviourIcons[3] = logDetailCardSlot.img_BehaviourIcons_4;
                logDetailCardSlot.img_BehaviourIcons[4] = logDetailCardSlot.img_BehaviourIcons_5;
                logDetailCardSlot.gs_BehaviourIcons[0] = logDetailCardSlot.gs_BehaviourIcons_1;
                logDetailCardSlot.gs_BehaviourIcons[1] = logDetailCardSlot.gs_BehaviourIcons_2;
                logDetailCardSlot.gs_BehaviourIcons[2] = logDetailCardSlot.gs_BehaviourIcons_3;
                logDetailCardSlot.gs_BehaviourIcons[3] = logDetailCardSlot.gs_BehaviourIcons_4;
                logDetailCardSlot.gs_BehaviourIcons[4] = logDetailCardSlot.gs_BehaviourIcons_5;
                logDetailCardSlot.rightDescSlotList[0] = logDetailCardSlot.rightDescSlotList_1;
                logDetailCardSlot.rightDescSlotList[1] = logDetailCardSlot.rightDescSlotList_2;
                logDetailCardSlot.rightDescSlotList[2] = logDetailCardSlot.rightDescSlotList_3;
                logDetailCardSlot.rightDescSlotList[3] = logDetailCardSlot.rightDescSlotList_4;
                logDetailCardSlot.rightDescSlotList[4] = logDetailCardSlot.rightDescSlotList_5;
                UnityEngine.Object.Destroy((UnityEngine.Object)original);
                logDetailCardSlot.gameObject.AddComponent<FrameDummy>();
                logDetailCardSlot.transform.GetChild(0).transform.localPosition = new Vector3(0.0f, 0.0f);
                return logDetailCardSlot;
            }

            public void SetLinearDodgeColor(Color c)
            {
                for (int index = 0; index < this.img_linearDodge.Length; ++index)
                {
                    this.img_linearDodge[index].color = c;
                    if (!((UnityEngine.Object)this.img_linearDodge[index] == (UnityEngine.Object)null))
                        this.img_linearDodge[index].color = c;
                }
            }

            public void SetFrameColor(Color c)
            {
                for (int index = 0; index < this.img_Frames.Length; ++index)
                {
                    this.img_Frames[index].color = c;
                    if (!((UnityEngine.Object)this.img_Frames[index] == (UnityEngine.Object)null))
                        this.img_Frames[index].color = c;
                }
            }

            public void SetRangeIconHsv(Vector3 hsvvalue)
            {
                this.img_RangeIcon.color = Color.white;
                if ((UnityEngine.Object)this.hsv_rangeIcon == (UnityEngine.Object)null)
                    this.hsv_rangeIcon = this.img_RangeIcon.GetComponent<RefineHsv>();
                if ((UnityEngine.Object)this.hsv_rangeIcon == (UnityEngine.Object)null)
                {
                    Debug.LogError((object)"Hsv Not Reference");
                }
                else
                {
                    this.hsv_rangeIcon._HueShift = hsvvalue.x;
                    this.hsv_rangeIcon._Saturation = hsvvalue.y;
                    this.hsv_rangeIcon._ValueBrightness = hsvvalue.z;
                    this.hsv_rangeIcon.CallUpdate();
                    this.hsv_rangeIcon.enabled = false;
                    this.hsv_rangeIcon.enabled = true;
                }
            }

            public void BSetData(DiceCardItemModel cardmodel)
            {
                this._cardModel = cardmodel;
                if (this._cardModel == null)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    if (!this.ob_NormalFrame.activeSelf)
                        this.ob_NormalFrame.gameObject.SetActive(true);
                    if (LorId.IsModId(this._cardModel.ClassInfo.workshopID))
                        this.txt_cardName.text = this._cardModel.GetName();
                    else
                        this.txt_cardName.text = Singleton<BattleCardDescXmlList>.Instance.GetCardName(this._cardModel.GetTextId());
                    this.costNumbers.SetOneValue(this._cardModel.GetSpec().Cost, UISpriteDataManager.instance._cardCostNumberSprites);
                    this.img_RangeIcon.sprite = UISpriteDataManager.instance.GetRangeIconSprite(this._cardModel.GetSpec().Ranged);
                    this.SetRangeIconHsv(UIColorManager.Manager.CardRangeHsvValue[(int)this._cardModel.GetRarity()]);
                    this.colorFrame = UIColorManager.Manager.GetCardRarityColor(this._cardModel.GetRarity());
                    this.colorLineardodge = UIColorManager.Manager.GetCardRarityLinearColor(this._cardModel.GetRarity());
                    this.SetFrameColor(this.colorFrame);
                    this.SetLinearDodgeColor(this.colorLineardodge);
                    this.costNumbers.SetContentColor(this.colorFrame);
                    List<DiceBehaviour> behaviourList = this._cardModel.GetBehaviourList();
                    for (int index = 0; index < this.img_BehaviourIcons.Length; ++index)
                    {
                        if (index < behaviourList.Count)
                        {
                            Sprite sprite = behaviourList[index].Type == BehaviourType.Standby ? UISpriteDataManager.instance.CardStandbyBehaviourDetailIcons[(int)behaviourList[index].Detail] : UISpriteDataManager.instance._cardBehaviourDetailIcons[(int)behaviourList[index].Detail];
                            this.img_BehaviourIcons[index].sprite = sprite;
                            this.img_BehaviourIcons[index].gameObject.SetActive(true);
                        }
                        else
                            this.img_BehaviourIcons[index].gameObject.SetActive(false);
                    }
                    Sprite sprite1;
                    if (LorId.IsModId(this._cardModel.ClassInfo.workshopID))
                    {
                        sprite1 = Singleton<CustomizingCardArtworkLoader>.Instance.GetSpecificArtworkSprite(this._cardModel.ClassInfo.workshopID, this._cardModel.GetArtworkSrc());
                        if ((UnityEngine.Object)sprite1 == (UnityEngine.Object)null)
                            sprite1 = Singleton<AssetBundleManagerRemake>.Instance.LoadCardSprite(this._cardModel.GetArtworkSrc());
                    }
                    else
                        sprite1 = Singleton<AssetBundleManagerRemake>.Instance.LoadCardSprite(this._cardModel.GetArtworkSrc());
                    if ((UnityEngine.Object)sprite1 != (UnityEngine.Object)null)
                        this.img_Artwork.sprite = sprite1;
                    else
                        Debug.Log((object)"Can't find sprite");
                    this.gameObject.SetActive(true);
                }
            }

            public void SetData(DiceCardItemModel cardmodel)
            {
                this.BSetData(cardmodel);
                if (this._cardModel == null)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    List<DiceBehaviour> behaviourList = this._cardModel.GetBehaviourList();
                    int b = 4 - behaviourList.Count;
                    if ((UnityEngine.Object)this.ob_selfAbility != (UnityEngine.Object)null)
                    {
                        string text = Singleton<BattleCardDescXmlList>.Instance.GetAbilityDesc(this._cardModel.GetID());
                        if (string.IsNullOrEmpty(text))
                        {
                            List<string> abilityDesc = Singleton<BattleCardAbilityDescXmlList>.Instance.GetAbilityDesc(this._cardModel.ClassInfo);
                            if (abilityDesc.Count > 0)
                                text = string.Join("\n", (IEnumerable<string>)abilityDesc);
                        }
                        else
                        {
                            string abilityDescString = Singleton<BattleCardAbilityDescXmlList>.Instance.GetDefaultAbilityDescString(this._cardModel.ClassInfo);
                            if (!string.IsNullOrEmpty(abilityDescString))
                                text = $"{abilityDescString}\n{text}";
                        }
                        if (!string.IsNullOrEmpty(text))
                        {
                            this.ob_selfAbility.SetActive(true);
                            this.txt_selfAbility.text = TextUtil.TransformConditionKeyword(text);
                            float preferredHeight = this.txt_selfAbility.preferredHeight;
                            int num = Mathf.Min((double)preferredHeight >= 26.0 ? ((double)preferredHeight >= 48.0 ? ((double)preferredHeight >= 70.0 ? 3 : 2) : 1) : 0, b);
                            RectTransform component = this.ob_selfAbility.GetComponent<RectTransform>();
                            if ((UnityEngine.Object)component != (UnityEngine.Object)null)
                            {
                                switch (num)
                                {
                                    case 1:
                                        component.sizeDelta = new Vector2(component.sizeDelta.x, 44f);
                                        break;
                                    case 2:
                                        component.sizeDelta = new Vector2(component.sizeDelta.x, 66f);
                                        break;
                                    case 3:
                                        component.sizeDelta = new Vector2(component.sizeDelta.x, 88f);
                                        break;
                                    default:
                                        component.sizeDelta = new Vector2(component.sizeDelta.x, 22f);
                                        break;
                                }
                            }
                        }
                        else
                            this.ob_selfAbility.gameObject.SetActive(false);
                    }
                    for (int index = 0; index < this.rightDescSlotList.Count; ++index)
                    {
                        if (index >= behaviourList.Count)
                        {
                            this.rightDescSlotList[index].gameObject.SetActive(false);
                        }
                        else
                        {
                            this.rightDescSlotList[index].SetBehaviourInfo(behaviourList[index], this._cardModel.GetID(), this._cardModel.GetBehaviourList());
                            this.rightDescSlotList[index].gameObject.SetActive(true);
                        }
                    }
                    if (this.OnKeyword)
                    {
                        if ((double)this.keywordListUI_R.GetComponent<RectTransform>().position.x / (double)Screen.width > 0.75)
                        {
                            if ((UnityEngine.Object)this.keywordListUI_L != (UnityEngine.Object)null)
                            {
                                this.keywordListUI_L.Init(this._cardModel.ClassInfo, this._cardModel.GetBehaviourList());
                                this.keywordListUI_L.Activate();
                            }
                            if (!((UnityEngine.Object)this.keywordListUI_R != (UnityEngine.Object)null))
                                return;
                            this.keywordListUI_R.Deactivate();
                        }
                        else
                        {
                            this.keywordListUI_R.Init(this._cardModel.ClassInfo, this._cardModel.GetBehaviourList());
                            this.keywordListUI_R.Activate();
                            if (!((UnityEngine.Object)this.keywordListUI_L != (UnityEngine.Object)null))
                                return;
                            this.keywordListUI_L.Deactivate();
                        }
                    }
                    else
                    {
                        this.keywordListUI_R.Deactivate();
                        this.keywordListUI_L.Deactivate();
                    }
                }
            }
        }

        public class UILogCardSlot : MonoBehaviour
        {
            public static LogLikeMod.UILogCardSlot Original;
            public RectTransform Pivot;
            public CanvasGroup cg;
            public GameObject ob_NormalFrame;
            public Image[] img_Frames;
            public Image img_Frames_1;
            public Image img_Frames_2;
            public Image[] img_linearDodge;
            public Image img_linearDodge_1;
            public Image img_linearDodge_2;
            public Image[] img_BehaviourIcons;
            public Image img_BehaviourIcons_1;
            public Image img_BehaviourIcons_2;
            public Image img_BehaviourIcons_3;
            public Image img_BehaviourIcons_4;
            public Image img_BehaviourIcons_5;
            public _2dxFX_GrayScale[] gs_BehaviourIcons;
            public _2dxFX_GrayScale gs_BehaviourIcons_1;
            public _2dxFX_GrayScale gs_BehaviourIcons_2;
            public _2dxFX_GrayScale gs_BehaviourIcons_3;
            public _2dxFX_GrayScale gs_BehaviourIcons_4;
            public _2dxFX_GrayScale gs_BehaviourIcons_5;
            public Image img_RangeIcon;
            public NumbersData costNumbers;
            public TextMeshProUGUI txt_cardName;
            public Image img_Artwork;
            public RefineHsv hsv_rangeIcon;
            public UICustomSelectable selectable;
            public DiceCardItemModel _cardModel;
            public Color colorFrame;
            public Color colorLineardodge;
            public int originSiblingIdx = -1;
            public UIInvenCardListScroll listPanel;
            public Image img_cardNumberBg;
            public TextMeshProUGUI txt_cardNumbers;
            public GameObject deckLimitRoot;
            public TextMeshProUGUI txt_deckLimit;
            public UICustomGraphicObject EquipInfoButton;
            public Animator EquipInfoButtonAnim;
            public CanvasGroup cg_LeftPanel;
            public CanvasGroup cg_EmptyFrameRoot;
            public UIINVENCARD_STATE slotState;
            public bool isEmpty;

            public static LogLikeMod.UILogCardSlot SlotCopyingByOrig()
            {
                LogLikeMod.UILogCardSlot uiLogCardSlot = UnityEngine.Object.Instantiate<LogLikeMod.UILogCardSlot>(LogLikeMod.UILogCardSlot.Original);
                uiLogCardSlot.img_Frames[0] = uiLogCardSlot.img_Frames_1;
                uiLogCardSlot.img_Frames[1] = uiLogCardSlot.img_Frames_2;
                uiLogCardSlot.img_linearDodge[0] = uiLogCardSlot.img_linearDodge_1;
                uiLogCardSlot.img_linearDodge[1] = uiLogCardSlot.img_linearDodge_2;
                uiLogCardSlot.img_BehaviourIcons[0] = uiLogCardSlot.img_BehaviourIcons_1;
                uiLogCardSlot.img_BehaviourIcons[1] = uiLogCardSlot.img_BehaviourIcons_2;
                uiLogCardSlot.img_BehaviourIcons[2] = uiLogCardSlot.img_BehaviourIcons_3;
                uiLogCardSlot.img_BehaviourIcons[3] = uiLogCardSlot.img_BehaviourIcons_4;
                uiLogCardSlot.img_BehaviourIcons[4] = uiLogCardSlot.img_BehaviourIcons_5;
                uiLogCardSlot.gs_BehaviourIcons[0] = uiLogCardSlot.gs_BehaviourIcons_1;
                uiLogCardSlot.gs_BehaviourIcons[1] = uiLogCardSlot.gs_BehaviourIcons_2;
                uiLogCardSlot.gs_BehaviourIcons[2] = uiLogCardSlot.gs_BehaviourIcons_3;
                uiLogCardSlot.gs_BehaviourIcons[3] = uiLogCardSlot.gs_BehaviourIcons_4;
                uiLogCardSlot.gs_BehaviourIcons[4] = uiLogCardSlot.gs_BehaviourIcons_5;
                uiLogCardSlot.selectable.SubmitEvent.RemoveAllListeners();
                uiLogCardSlot.selectable.SubmitEvent.AddListener(new UnityAction<BaseEventData>(uiLogCardSlot.OnPointerClick));
                uiLogCardSlot.selectable.SelectEvent.RemoveAllListeners();
                uiLogCardSlot.selectable.SelectEvent.AddListener(new UnityAction<BaseEventData>(uiLogCardSlot.OnPointerEnter));
                uiLogCardSlot.selectable.DeselectEvent.RemoveAllListeners();
                uiLogCardSlot.selectable.DeselectEvent.AddListener(new UnityAction<BaseEventData>(uiLogCardSlot.OnPointerExit));
                uiLogCardSlot.selectable.XEvent.RemoveAllListeners();
                return uiLogCardSlot;
            }

            public static LogLikeMod.UILogCardSlot SlotCopying()
            {
                UIInvenCardListScroll invenCardList = (UI.UIController.Instance.GetUIPanel(UIPanelType.BattleSetting) as UIBattleSettingPanel).EditPanel.BattleCardPanel.InvenCardList;
                UIOriginCardSlot componentsInChild = ((Component)typeof(UIOriginCardList).GetField("transform_CardListRoot", AccessTools.all).GetValue((object)invenCardList)).GetComponentsInChildren<UIOriginCardSlot>(true)[0];
                LogLikeMod.UILogCardSlot original = componentsInChild.gameObject.AddComponent<LogLikeMod.UILogCardSlot>();
                original.Pivot = (RectTransform)typeof(UIOriginCardSlot).GetField("Pivot", AccessTools.all).GetValue((object)componentsInChild);
                original.cg = (CanvasGroup)typeof(UIOriginCardSlot).GetField("cg", AccessTools.all).GetValue((object)componentsInChild);
                original.ob_NormalFrame = (GameObject)typeof(UIOriginCardSlot).GetField("ob_NormalFrame", AccessTools.all).GetValue((object)componentsInChild);
                original.img_Frames = (Image[])typeof(UIOriginCardSlot).GetField("img_Frames", AccessTools.all).GetValue((object)componentsInChild);
                original.img_Frames_1 = original.img_Frames[0];
                original.img_Frames_2 = original.img_Frames[1];
                original.img_linearDodge = (Image[])typeof(UIOriginCardSlot).GetField("img_linearDodge", AccessTools.all).GetValue((object)componentsInChild);
                original.img_linearDodge_1 = original.img_linearDodge[0];
                original.img_linearDodge_2 = original.img_linearDodge[1];
                original.img_BehaviourIcons = (Image[])typeof(UIOriginCardSlot).GetField("img_BehaviourIcons", AccessTools.all).GetValue((object)componentsInChild);
                original.img_BehaviourIcons_1 = original.img_BehaviourIcons[0];
                original.img_BehaviourIcons_2 = original.img_BehaviourIcons[1];
                original.img_BehaviourIcons_3 = original.img_BehaviourIcons[2];
                original.img_BehaviourIcons_4 = original.img_BehaviourIcons[3];
                original.img_BehaviourIcons_5 = original.img_BehaviourIcons[4];
                original.gs_BehaviourIcons = (_2dxFX_GrayScale[])typeof(UIOriginCardSlot).GetField("gs_BehaviourIcons", AccessTools.all).GetValue((object)componentsInChild);
                original.gs_BehaviourIcons_1 = original.gs_BehaviourIcons[0];
                original.gs_BehaviourIcons_2 = original.gs_BehaviourIcons[1];
                original.gs_BehaviourIcons_3 = original.gs_BehaviourIcons[2];
                original.gs_BehaviourIcons_4 = original.gs_BehaviourIcons[3];
                original.gs_BehaviourIcons_5 = original.gs_BehaviourIcons[4];
                original.img_RangeIcon = (Image)typeof(UIOriginCardSlot).GetField("img_RangeIcon", AccessTools.all).GetValue((object)componentsInChild);
                original.costNumbers = (NumbersData)typeof(UIOriginCardSlot).GetField("costNumbers", AccessTools.all).GetValue((object)componentsInChild);
                original.txt_cardName = (TextMeshProUGUI)typeof(UIOriginCardSlot).GetField("txt_cardName", AccessTools.all).GetValue((object)componentsInChild);
                original.img_Artwork = (Image)typeof(UIOriginCardSlot).GetField("img_Artwork", AccessTools.all).GetValue((object)componentsInChild);
                original.hsv_rangeIcon = (RefineHsv)typeof(UIOriginCardSlot).GetField("hsv_rangeIcon", AccessTools.all).GetValue((object)componentsInChild);
                original.selectable = (UICustomSelectable)typeof(UIOriginCardSlot).GetField("selectable", AccessTools.all).GetValue((object)componentsInChild);
                original._cardModel = (DiceCardItemModel)typeof(UIOriginCardSlot).GetField("_cardModel", AccessTools.all).GetValue((object)componentsInChild);
                original.colorFrame = (Color)typeof(UIOriginCardSlot).GetField("colorFrame", AccessTools.all).GetValue((object)componentsInChild);
                original.colorLineardodge = (Color)typeof(UIOriginCardSlot).GetField("colorLineardodge", AccessTools.all).GetValue((object)componentsInChild);
                original.originSiblingIdx = (int)typeof(UIOriginCardSlot).GetField("originSiblingIdx", AccessTools.all).GetValue((object)componentsInChild);
                original.listPanel = (UIInvenCardListScroll)typeof(UIInvenCardSlot).GetField("listPanel", AccessTools.all).GetValue((object)componentsInChild);
                original.img_cardNumberBg = (Image)typeof(UIInvenCardSlot).GetField("img_cardNumberBg", AccessTools.all).GetValue((object)componentsInChild);
                original.txt_cardNumbers = (TextMeshProUGUI)typeof(UIInvenCardSlot).GetField("txt_cardNumbers", AccessTools.all).GetValue((object)componentsInChild);
                original.deckLimitRoot = (GameObject)typeof(UIInvenCardSlot).GetField("deckLimitRoot", AccessTools.all).GetValue((object)componentsInChild);
                original.txt_deckLimit = (TextMeshProUGUI)typeof(UIInvenCardSlot).GetField("txt_deckLimit", AccessTools.all).GetValue((object)componentsInChild);
                original.EquipInfoButton = (UICustomGraphicObject)typeof(UIInvenCardSlot).GetField("EquipInfoButton", AccessTools.all).GetValue((object)componentsInChild);
                original.EquipInfoButtonAnim = (Animator)typeof(UIInvenCardSlot).GetField("EquipInfoButtonAnim", AccessTools.all).GetValue((object)componentsInChild);
                original.cg_LeftPanel = (CanvasGroup)typeof(UIInvenCardSlot).GetField("cg_LeftPanel", AccessTools.all).GetValue((object)componentsInChild);
                original.cg_EmptyFrameRoot = (CanvasGroup)typeof(UIInvenCardSlot).GetField("cg_EmptyFrameRoot", AccessTools.all).GetValue((object)componentsInChild);
                original.slotState = (UIINVENCARD_STATE)typeof(UIInvenCardSlot).GetField("slotState", AccessTools.all).GetValue((object)componentsInChild);
                original.isEmpty = (bool)typeof(UIInvenCardSlot).GetField("isEmpty", AccessTools.all).GetValue((object)componentsInChild);
                LogLikeMod.UILogCardSlot uiLogCardSlot = UnityEngine.Object.Instantiate<LogLikeMod.UILogCardSlot>(original);
                uiLogCardSlot.img_Frames[0] = uiLogCardSlot.img_Frames_1;
                uiLogCardSlot.img_Frames[1] = uiLogCardSlot.img_Frames_2;
                uiLogCardSlot.img_linearDodge[0] = uiLogCardSlot.img_linearDodge_1;
                uiLogCardSlot.img_linearDodge[1] = uiLogCardSlot.img_linearDodge_2;
                uiLogCardSlot.img_BehaviourIcons[0] = uiLogCardSlot.img_BehaviourIcons_1;
                uiLogCardSlot.img_BehaviourIcons[1] = uiLogCardSlot.img_BehaviourIcons_2;
                uiLogCardSlot.img_BehaviourIcons[2] = uiLogCardSlot.img_BehaviourIcons_3;
                uiLogCardSlot.img_BehaviourIcons[3] = uiLogCardSlot.img_BehaviourIcons_4;
                uiLogCardSlot.img_BehaviourIcons[4] = uiLogCardSlot.img_BehaviourIcons_5;
                uiLogCardSlot.gs_BehaviourIcons[0] = uiLogCardSlot.gs_BehaviourIcons_1;
                uiLogCardSlot.gs_BehaviourIcons[1] = uiLogCardSlot.gs_BehaviourIcons_2;
                uiLogCardSlot.gs_BehaviourIcons[2] = uiLogCardSlot.gs_BehaviourIcons_3;
                uiLogCardSlot.gs_BehaviourIcons[3] = uiLogCardSlot.gs_BehaviourIcons_4;
                uiLogCardSlot.gs_BehaviourIcons[4] = uiLogCardSlot.gs_BehaviourIcons_5;
                UnityEngine.Object.Destroy((UnityEngine.Object)original);
                typeof(UIInvenCardSlot).GetField("EquipInfoButton", AccessTools.all).SetValue((object)uiLogCardSlot.gameObject.GetComponent<UIInvenCardSlot>(), (object)null);
                typeof(UIInvenCardSlot).GetField("EquipInfoButtonAnim", AccessTools.all).SetValue((object)uiLogCardSlot.gameObject.GetComponent<UIInvenCardSlot>(), (object)null);
                UnityEngine.Object.Destroy((UnityEngine.Object)uiLogCardSlot.gameObject.GetComponent<UIInvenCardSlot>());
                uiLogCardSlot.selectable.SubmitEvent.RemoveAllListeners();
                uiLogCardSlot.selectable.SubmitEvent.AddListener(new UnityAction<BaseEventData>(uiLogCardSlot.OnPointerClick));
                uiLogCardSlot.selectable.SelectEvent.RemoveAllListeners();
                uiLogCardSlot.selectable.SelectEvent.AddListener(new UnityAction<BaseEventData>(uiLogCardSlot.OnPointerEnter));
                uiLogCardSlot.selectable.DeselectEvent.RemoveAllListeners();
                uiLogCardSlot.selectable.DeselectEvent.AddListener(new UnityAction<BaseEventData>(uiLogCardSlot.OnPointerExit));
                uiLogCardSlot.selectable.XEvent.RemoveAllListeners();
                UnityEngine.Object.Destroy((UnityEngine.Object)uiLogCardSlot.EquipInfoButton.gameObject);
                return uiLogCardSlot;
            }

            public void SetCgLeftPanel(bool on)
            {
                if ((UnityEngine.Object)this.cg_LeftPanel == (UnityEngine.Object)null)
                    return;
                this.cg_LeftPanel.alpha = on ? 1f : 0.0f;
                this.cg_LeftPanel.blocksRaycasts = on;
                this.cg_LeftPanel.interactable = on;
            }

            public void OnPointerEnter(BaseEventData eventData)
            {
                LogLikeMod.UILogBattleDiceCardUI.Instance.transform.SetParent(this.transform);
                LogLikeMod.UILogBattleDiceCardUI.Instance.gameObject.SetActive(true);
                LogLikeMod.UILogBattleDiceCardUI.Instance.SetCard(BattleDiceCardModel.CreatePlayingCard(this._cardModel.ClassInfo));
                LogLikeMod.UILogBattleDiceCardUI.Instance.transform.localPosition = new Vector3(250f, -100f, -1f);
                LogLikeMod.UILogBattleDiceCardUI.Instance.transform.localScale = new Vector3(0.2f, 0.2f);
            }

            public void OnPointerExit(BaseEventData eventData)
            {
                if (!((UnityEngine.Object)LogLikeMod.UILogBattleDiceCardUI.Instance != (UnityEngine.Object)null))
                    return;
                LogLikeMod.UILogBattleDiceCardUI.Instance.gameObject.SetActive(false);
            }

            public void OnPointerClick(BaseEventData eventData)
            {
            }

            public void SetLinearDodgeColor(Color c)
            {
                for (int index = 0; index < this.img_linearDodge.Length; ++index)
                {
                    if (!((UnityEngine.Object)this.img_linearDodge[index] == (UnityEngine.Object)null))
                        this.img_linearDodge[index].color = c;
                }
            }

            public void SetFrameColor(Color c)
            {
                for (int index = 0; index < this.img_Frames.Length; ++index)
                {
                    if (!((UnityEngine.Object)this.img_Frames[index] == (UnityEngine.Object)null))
                        this.img_Frames[index].color = c;
                }
            }

            public void SetRangeIconHsv(Vector3 hsvvalue)
            {
                this.img_RangeIcon.color = Color.white;
                if ((UnityEngine.Object)this.hsv_rangeIcon == (UnityEngine.Object)null)
                    this.hsv_rangeIcon = this.img_RangeIcon.GetComponent<RefineHsv>();
                if ((UnityEngine.Object)this.hsv_rangeIcon == (UnityEngine.Object)null)
                {
                    Debug.LogError((object)"Hsv Not Reference");
                }
                else
                {
                    this.hsv_rangeIcon._HueShift = hsvvalue.x;
                    this.hsv_rangeIcon._Saturation = hsvvalue.y;
                    this.hsv_rangeIcon._ValueBrightness = hsvvalue.z;
                    this.hsv_rangeIcon.CallUpdate();
                    this.hsv_rangeIcon.enabled = false;
                    this.hsv_rangeIcon.enabled = true;
                }
            }

            public void RefreshNumbersData() => this.txt_cardNumbers.text = "";

            public void SetSlotState()
            {
                this.slotState = UIINVENCARD_STATE.None;
                this.deckLimitRoot.gameObject.SetActive(false);
                this.RefreshNumbersData();
            }

            public void SetData(DiceCardItemModel cardmodel)
            {
                this._cardModel = cardmodel;
                if (this._cardModel == null)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    if (!this.ob_NormalFrame.activeSelf)
                        this.ob_NormalFrame.gameObject.SetActive(true);
                    if (LorId.IsModId(this._cardModel.ClassInfo.workshopID))
                        this.txt_cardName.text = this._cardModel.GetName();
                    else
                        this.txt_cardName.text = Singleton<BattleCardDescXmlList>.Instance.GetCardName(this._cardModel.GetTextId());
                    this.costNumbers.SetOneValue(this._cardModel.GetSpec().Cost, UISpriteDataManager.instance._cardCostNumberSprites);
                    this.img_RangeIcon.sprite = UISpriteDataManager.instance.GetRangeIconSprite(this._cardModel.GetSpec().Ranged);
                    this.SetRangeIconHsv(UIColorManager.Manager.CardRangeHsvValue[(int)this._cardModel.GetRarity()]);
                    this.colorFrame = UIColorManager.Manager.GetCardRarityColor(this._cardModel.GetRarity());
                    this.colorLineardodge = UIColorManager.Manager.GetCardRarityLinearColor(this._cardModel.GetRarity());
                    this.SetFrameColor(this.colorFrame);
                    this.SetLinearDodgeColor(this.colorLineardodge);
                    this.costNumbers.SetContentColor(this.colorFrame);
                    List<DiceBehaviour> behaviourList = this._cardModel.GetBehaviourList();
                    for (int index = 0; index < this.img_BehaviourIcons.Length; ++index)
                    {
                        if (index < behaviourList.Count)
                        {
                            Sprite sprite = behaviourList[index].Type == BehaviourType.Standby ? UISpriteDataManager.instance.CardStandbyBehaviourDetailIcons[(int)behaviourList[index].Detail] : UISpriteDataManager.instance._cardBehaviourDetailIcons[(int)behaviourList[index].Detail];
                            this.img_BehaviourIcons[index].sprite = sprite;
                            this.img_BehaviourIcons[index].gameObject.SetActive(true);
                        }
                        else
                            this.img_BehaviourIcons[index].gameObject.SetActive(false);
                    }
                    Sprite sprite1;
                    if (LorId.IsModId(this._cardModel.ClassInfo.workshopID))
                    {
                        sprite1 = Singleton<CustomizingCardArtworkLoader>.Instance.GetSpecificArtworkSprite(this._cardModel.ClassInfo.workshopID, this._cardModel.GetArtworkSrc());
                        if ((UnityEngine.Object)sprite1 == (UnityEngine.Object)null)
                            sprite1 = Singleton<AssetBundleManagerRemake>.Instance.LoadCardSprite(this._cardModel.GetArtworkSrc());
                    }
                    else
                        sprite1 = Singleton<AssetBundleManagerRemake>.Instance.LoadCardSprite(this._cardModel.GetArtworkSrc());
                    if ((UnityEngine.Object)sprite1 != (UnityEngine.Object)null)
                        this.img_Artwork.sprite = sprite1;
                    else
                        Debug.Log((object)"Can't find sprite");
                    this.gameObject.SetActive(true);
                    if (cardmodel == null)
                        return;
                    this.SetSlotState();
                    this.isEmpty = false;
                    if ((UnityEngine.Object)this.cg_EmptyFrameRoot != (UnityEngine.Object)null)
                        this.cg_EmptyFrameRoot.alpha = 0.0f;
                    this.SetCgLeftPanel(true);
                }
            }
        }

        public class BattleMoneyUI
        {
            public static Dictionary<string, GameObject> obj_dic;

            public static void Create()
            {
                if ((UnityEngine.Object)LogLikeMod.DefFont == (UnityEngine.Object)null)
                {
                    LogLikeMod.DefFont = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
                    LogLikeMod.DefFontColor = UIColorManager.Manager.GetUIColor(UIColor.Default);
                    LogLikeMod.DefFont_TMP = (SingletonBehavior<UIPopupWindowManager>.Instance.popupPanels[1] as UIOptionWindow).displayDropdown.itemText.font;
                }
                LogLikeMod.BattleMoneyUI.obj_dic = new Dictionary<string, GameObject>();
                Image image = ModdingUtils.CreateImage(LogLikeMod.LogUIObjs[100].transform, "MoneyIcon", new Vector2(1f, 1f), new Vector2(610f, 510f));
                TextMeshProUGUI textTmp = ModdingUtils.CreateText_TMP(image.transform, new Vector2(40f, 0.0f), 30, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAlignmentOptions.MidlineRight, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
                textTmp.rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
                textTmp.rectTransform.sizeDelta = new Vector2(200f, 50f);
                textTmp.text = PassiveAbility_MoneyCheck.GetMoney().ToString();
                textTmp.overflowMode = TextOverflowModes.Overflow;
                textTmp.autoSizeTextContainer = true;
                LogLikeMod.BattleMoneyUI.obj_dic.Add("MoneyIcon", image.gameObject);
                LogLikeMod.BattleMoneyUI.obj_dic.Add("Money", textTmp.gameObject);
            }

            public static void Active()
            {
                if (LogLikeMod.BattleMoneyUI.obj_dic == null)
                    LogLikeMod.BattleMoneyUI.Create();
                foreach (GameObject gameObject in LogLikeMod.BattleMoneyUI.obj_dic.Values)
                    gameObject.SetActive(true);
            }

            public static void DeActive()
            {
                if (LogLikeMod.BattleMoneyUI.obj_dic == null)
                    return;
                foreach (GameObject gameObject in LogLikeMod.BattleMoneyUI.obj_dic.Values)
                    gameObject.SetActive(false);
            }

            public static void UpdateMoney()
            {
                if (LogLikeMod.BattleMoneyUI.obj_dic == null)
                    return;
                LogLikeMod.BattleMoneyUI.obj_dic["Money"].GetComponent<TextMeshProUGUI>().text = PassiveAbility_MoneyCheck.GetMoney().ToString();
            }
        }
    }
}
