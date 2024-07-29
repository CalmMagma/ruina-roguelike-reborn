using System;
using System.Collections;
using System.Collections.Generic;
using LOR_DiceSystem;
using UnityEngine.UI;
using TMPro;
using UI;
using UnityEngine;
using abcdcode_LOGLIKE_MOD;
using HarmonyLib;
using System.Reflection;
using UnityEngine.EventSystems;
using System.Linq;
using BattleCharacterProfile;
using GameSave;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Mod;
using System.Xml;
using System.Xml.Serialization;
using LOR_XML;
using System.Reflection.Emit;
using CustomMapUtility;
using Workshop;
using Sound;
using UnityEngine.SceneManagement;

namespace RogueLike_Mod_Reborn
{
    public class RMRCore : ModInitializer
    {
        public static LorId[] booksToAddToInventory = new LorId[]
        {
            new LorId(RMRCore.packageId, -853),
        };
          
        public static bool provideAdditionalLogging = true;
        public static Dictionary<Assembly, string> ClassIds = new Dictionary<Assembly, string>();
        public static RoguelikeGamemodeBase CurrentGamemode;
        public const string packageId = "abcdcodecalmmagma.LogueLikeReborn";
        public static CustomMapHandler RMRMapHandler;

        public override void OnInitializeMod()
        {
            base.OnInitializeMod();

            if (!File.Exists(LogueSaveManager.Saveroot + "/RMR_Config.xml"))
            {
               using (var file = File.Create(LogueSaveManager.Saveroot + "/RMR_Config.xml")) {
                    new XmlSerializer(typeof(RMRConfigRoot)).Serialize(file, new RMRConfigRoot());
               }
            }
            using (var file = File.OpenRead(LogueSaveManager.Saveroot + "/RMR_Config.xml"))
            {
                RMRConfigRoot config = (RMRConfigRoot)(new XmlSerializer(typeof(RMRConfigRoot)).Deserialize(file));
                RMRCore.provideAdditionalLogging = config.EnableAdditionalLogging;
                GlobalLogueItemCatalogPanel.Instance.debugMode = config.ShowAllItemCatalog;
            }

            Harmony.CreateAndPatchAll(typeof(RMR_Patches), packageId);
            if (!Directory.Exists(LogueSaveManager.Saveroot))
                Directory.CreateDirectory(LogueSaveManager.Saveroot);
            if (!File.Exists(LogueSaveManager.Saveroot + "/RMR_ItemCatalog"))
            {
                SaveData data = new SaveData(SaveDataType.Dictionary);
                using (FileStream fileStream = File.Create(LogueSaveManager.Saveroot + "/RMR_ItemCatalog"))
                {
                    new BinaryFormatter().Serialize(fileStream, data.GetSerializedData());
                }
            }

            RMRCore.RMRMapHandler = CustomMapHandler.GetCMU(packageId);

            foreach (Assembly ass in LogLikeMod.GetAssemList().Distinct())
            {
                if (ass != null)
                {
                    var package = ModContentManager.Instance.GetAllMods().Find(x => ass.CodeBase.Contains(x.GetAssemPath()) || ass.EscapedCodeBase.Contains(x.GetAssemPath()) || ass.Location.Contains(x.GetAssemPath()));
                    if (package != null)
                    {
                        ClassIds[ass] = package.invInfo.workshopInfo.uniqueId;
                    }
                }
            }
            ClassIds[this.GetType().Assembly] = RMRCore.packageId;
            ClassIds[typeof(LogLikeMod).Assembly] = RMRCore.packageId;

            LogLikeMod.ModdedArtWorks = new LogLikeMod.CacheDic<(string, string), Sprite>(new LogLikeMod.CacheDic<(string, string), Sprite>.getdele(LoadSatelliteArtwork));
            LogueEffectXmlList.Instance.Init(TextDataModel.CurrentLanguage);
            LoadSatelliteBattleTexts(TextDataModel.CurrentLanguage);
            LoadSatelliteBattleDialog(TextDataModel.CurrentLanguage);
            RogueMysteryXmlList.Instance.Init(TextDataModel.CurrentLanguage);
            SceneManager.sceneLoaded += FindGamemodes;
            CurrentGamemode = new RoguelikeGamemode_RMR_Default();
        }

        // This essentially guarantees that the gamemodes are only collected far after all mods are loaded
        private void FindGamemodes(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Stage_Hod_New")
            {
                Singleton<RoguelikeGamemodeController>.Instance.AddGamemodesToList();
                SceneManager.sceneLoaded -= FindGamemodes;
            }
        }

        public static void LoadSatelliteBattleDialog(string language)
        {
            string str = Path.Combine("Localize", language);
            string ogpath = Path.Combine(ModContentManager.Instance.GetModPath(RMRCore.packageId), "Assemblies", "dlls");

            if (Directory.Exists(Path.Combine(ogpath, str, "BattleDialogs")))
            {
                BattleDialogXmlList.Instance._dictionary["Workshop"].characterList.RemoveAll(x => x.id.packageId == RMRCore.packageId);
                DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(ogpath, str, "BattleDialogs"));
                foreach (System.IO.FileInfo fileinfo in directoryInfo.GetFiles())
                {
                    try
                    {
                        var root = new XmlSerializer(typeof(BattleDialogRoot)).Deserialize(fileinfo.OpenRead()) as BattleDialogRoot;
                        BattleDialogXmlList.Instance.AddDialogByMod(root.characterList);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error while trying to load XML file " + fileinfo.Name + ": " + e);
                    }
                }

            }

            foreach (ModContentInfo modContentInfo in LogLikeMod.GetLogMods())
            {
                string uniqueId = modContentInfo.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(Path.Combine(modContentInfo.GetLogDllPath(), str, "BattleDialogs")))
                {
                    BattleDialogXmlList.Instance._dictionary["Workshop"].characterList.RemoveAll(x => x.id.packageId == uniqueId);
                    DirectoryInfo directoryInfo2 = new DirectoryInfo(Path.Combine(modContentInfo.GetLogDllPath(), str, "BattleDialogs"));
                    foreach (System.IO.FileInfo fileinfo2 in directoryInfo2.GetFiles())
                    {
                        try
                        {
                            var root = new XmlSerializer(typeof(BattleDialogRoot)).Deserialize(fileinfo2.OpenRead()) as BattleDialogRoot;
                            BattleDialogXmlList.Instance.AddDialogByMod(root.characterList);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Error while trying to load XML file " + fileinfo2.Name + ": " + e);
                        }
                    }
                }

            }

        }

        public static void LoadSatelliteBattleTexts(string language)
        {
            string str = Path.Combine("Localize", language);
            string ogpath = Path.Combine(ModContentManager.Instance.GetModPath(RMRCore.packageId), "Assemblies", "dlls");

            if (Directory.Exists(Path.Combine(ogpath, str, "EffectTexts")))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(ogpath, str, "EffectTexts"));
                foreach (System.IO.FileInfo fileinfo in directoryInfo.GetFiles())
                {
                    try
                    {
                        var root = new XmlSerializer(typeof(BattleEffectTextRoot)).Deserialize(fileinfo.OpenRead()) as BattleEffectTextRoot;
                        foreach (var info in root.effectTextList)
                        {
                            if (!BattleEffectTextsXmlList.Instance._dictionary.ContainsKey(info.ID))
                                BattleEffectTextsXmlList.Instance._dictionary.Add(info.ID, info);
                            else
                                BattleEffectTextsXmlList.Instance._dictionary[info.ID] = info;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error while trying to load XML file " + fileinfo.Name + ": " + e);
                    }
                }

            }

            foreach (ModContentInfo modContentInfo in LogLikeMod.GetLogMods())
            {
                string uniqueId = modContentInfo.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(Path.Combine(modContentInfo.GetLogDllPath(), str, "EffectTexts")))
                {
                    DirectoryInfo directoryInfo2 = new DirectoryInfo(Path.Combine(modContentInfo.GetLogDllPath(), str, "EffectTexts"));
                    foreach (System.IO.FileInfo fileinfo2 in directoryInfo2.GetFiles())
                    {
                        try
                        {
                            var root = new XmlSerializer(typeof(BattleEffectTextRoot)).Deserialize(fileinfo2.OpenRead()) as BattleEffectTextRoot;
                            foreach (var info in root.effectTextList)
                            {
                                if (!BattleEffectTextsXmlList.Instance._dictionary.ContainsKey(info.ID))
                                    BattleEffectTextsXmlList.Instance._dictionary.Add(info.ID, info);
                                else
                                    BattleEffectTextsXmlList.Instance._dictionary[info.ID] = info;
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Error while trying to load XML file " + fileinfo2.Name + ": " + e);
                        }
                    }
                }

            }

        }

        public static Sprite LoadSatelliteArtwork((string, string) ids)
        {
            try
            {
                if (ids.Item1 == RMRCore.packageId)
                {
                    if (provideAdditionalLogging && !Environment.StackTrace.Contains("abcdcode_LOGLIKE_MOD.LogLikeMod+CacheDic`2[Tkey,TValue].ContainsKey (Tkey key)")) Debug.Log("Unnecessary custom artwork call for vanilla artwork! Coming from " + Environment.StackTrace);
                    if (LogLikeMod.ArtWorks.ContainsKey(ids.Item2))
                        return LogLikeMod.ArtWorks[ids.Item2];
                    else if (provideAdditionalLogging) Debug.Log("Failed to obtain sprite altogether!! REAL SHIT!!!");
                }
                else
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(ModContentManager.Instance.GetModPath(ids.Item1) + "/Resource/CustomArtwork");
                    if (directoryInfo.GetDirectories().Length != 0)
                    {
                        foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                        {
                            Sprite artWorks = LogLikeMod.GetArtWorks(dir, ids.Item2);
                            if (artWorks != null)
                            {
                                return artWorks;
                            }
                        }
                    }
                    foreach (System.IO.FileInfo fileInfo in directoryInfo.GetFiles())
                    {
                        if (Path.GetFileNameWithoutExtension(fileInfo.FullName) == ids.Item2)
                        {
                            Texture2D texture2D = new Texture2D(2, 2);
                            texture2D.LoadImage(File.ReadAllBytes(fileInfo.FullName));
                            return Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0f, 0f), 100f, 0U, SpriteMeshType.FullRect);
                        }
                    }
                }
            } catch (Exception e)
            {
                if (provideAdditionalLogging) Debug.Log("Failed to obtain custom artwork: " + e);
            }
            return null;
        }

        #region literally do not use this ever
        /*
        public static string GetPackageIdFromAssembly(Assembly ass)
        {
            if (ass == typeof(RMRCore).Assembly || ass == typeof(LogLikeMod).Assembly)
                return RMRCore.packageId;
            string ass2 = ass.Location;
            var mod = LogLikeMod.GetLogMods().Find(x => ass2.Contains(x.GetLogDllPath()));
            if (mod != null)
                return mod.invInfo.workshopInfo.uniqueId;
            return null;
        }
        */
        #endregion
    }

    #region TECHNICAL INFRASTRUCTURE

    public static class RMRMysteryExtensions
    {
        public static void DisableButton(this MysteryBase mystery, int button)
        {
            string btn = button.ToString();
            mystery.FrameObj["choice_btn" + btn].GetComponent<Image>().sprite = LogLikeMod.ArtWorks["disabledButton"];
            mystery.FrameObj["Desc" + btn].GetComponent<TextMeshProUGUI>().text = TextDataModel.GetText("Mystery_RequireCondition") + mystery.FrameObj["Desc" + btn].GetComponent<TextMeshProUGUI>().text;
        }

        public static void ShowOverlayOverButton(this MysteryBase mystery, BattleUnitBuf buf, int button)
        {
            SingletonBehavior<UIBattleOverlayManager>.Instance.EnableBufOverlay(buf.bufActivatedName, buf.bufActivatedText, buf.GetBufIcon(), mystery.FrameObj["choice_btn" + button.ToString()]);
        }

        public static void ShowOverlayOverButton(this MysteryBase mystery, GlobalLogueEffectBase item, int button)
        {
            SingletonBehavior<UIBattleOverlayManager>.Instance.EnableBufOverlay(item.GetEffectName(), item.GetEffectDesc(), item.GetSprite(), mystery.FrameObj["choice_btn" + button.ToString()]);
        }

        public static void ShowOverlayOverButton(this MysteryBase mystery, PickUpModelBase item, int button)
        {
            SingletonBehavior<UIBattleOverlayManager>.Instance.EnableBufOverlay(item.Name, item.Desc, LogLikeMod.ModdedArtWorks.ContainsKey((RMRCore.ClassIds[item.GetType().Assembly], item.ArtWork)) ? LogLikeMod.ModdedArtWorks[(RMRCore.ClassIds[item.GetType().Assembly], item.ArtWork)] : LogLikeMod.ArtWorks[item.ArtWork], mystery.FrameObj["choice_btn" + button.ToString()]);
        }

        public static void CloseOverlayOverButton(this MysteryBase mystery)
        {
            SingletonBehavior<UIBattleOverlayManager>.Instance.DisableOverlay();
        }
    }

    public static class RMRUtilityExtensions
    {
        /// <summary>
        /// Plays a sound. Useful in conjunction with CustomMapUtility.
        /// </summary>
        public static void PlaySound(this AudioClip audio, Transform transform, float VolumnControl = 1.5f, bool loop = false)
        {
            BattleEffectSound battleEffectSound = UnityEngine.Object.Instantiate<BattleEffectSound>(SingletonBehavior<BattleSoundManager>.Instance.effectSoundPrefab, transform);
            float volume = 1f;
            bool flag = SingletonBehavior<BattleSoundManager>.Instance != null;
            bool flag2 = flag;
            if (flag2)
            {
                volume = SingletonBehavior<BattleSoundManager>.Instance.VolumeFX * VolumnControl;
            }
            battleEffectSound.Init(audio, volume, loop);
        }

        /// <summary>
        /// Set <u><see cref="BattleUnitView.deadEvent"/></u> to this in order to make an unit explode on death.
        /// </summary>
        public static void ExplodeOnDeath(BattleUnitView view)
        {
            if (view.model.UnitData.floorBattleData.param3 != 113413411)
            {
                SoundEffectPlayer soundEffectPlayer = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/MatchGirl_Explosion");
                if (soundEffectPlayer != null)
                {
                    soundEffectPlayer.SetGlobalPosition(view.WorldPosition);
                }
                var effect = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("1/MatchGirl_Footfall", 1f, view, null, 2f);
                effect.transform.localScale *= 3.5f;
                effect.AttachEffectLayer();
                view.model.UnitData.floorBattleData.param3 = 113413411;
                view.StartDeadEffect(false);
                view.model._deadSceneBlock = true;
            }
        }

        /// <summary>
        /// Returns a random element from a given list.
        /// </summary>
        public static T SelectOneRandom<T>(this IList<T> list)
        {
            return list[Singleton<System.Random>.Instance.Next(list.Count)];
        }

        /// <summary>
        /// Returns several random elements from a given list.
        /// </summary>
        /// <param name="count">The number of random elements to be returned</param>
        /// <returns>A new list containing randomly selected elements from the original list. The same element may be picked multiple times.</returns>
        public static List<T> SelectManyRandom<T>(this IList<T> list, int count)
        {
            List<T> list2 = new List<T>();
            for (int x = 0; x < count; x++)
            {
                list2.Append(list[Singleton<System.Random>.Instance.Next(list.Count)]);
            }
            return list2;
        }

        /// <summary>
        /// Sorts a list of <see cref="BattleDiceCardModel"/>s (Combat Pages) by their current cost, in crescent order.
        /// </summary>
        /// <param name="list">The list to be sorted.</param>
        /// <returns></returns>
        public static void SortByCost(this List<BattleDiceCardModel> list)
        {
            list.Sort((BattleDiceCardModel x, BattleDiceCardModel y) => x.CurCost - y.CurCost);
        }

        /// <summary>
        /// Shuffles a list.
        /// </summary>
        public static List<T> Shuffle<T>(this IList<T> list)
        {
            return list.OrderBy(x => Singleton<System.Random>.Instance.Next()).ToList();
        }

        /// <summary>
        /// Creates a new list from an existing one, sorts it, then returns it.
        /// </summary>
        public static IList<T> SortReturn<T> (this IList<T> list, Comparison<T> comparer)
        {
            var list2 = list.ToList();
            list2.Sort(comparer);
            return list2;
        }

        /// <summary>
        /// Checks to see if a card or one of its dice posses a given keyword.
        /// </summary>
        /// <param name="card">The card to be tested.</param>
        /// <param name="keyword">The keyword to test the card for.</param>
        /// <returns>Returns <see langword="true"/> if the <paramref name="card"/> or any of its dice have the given <paramref name="keyword"/>. Returns <see langword="false"/> otherwise.</returns>
        public static bool CheckForKeyword(this BattleDiceCardModel card, string keyword)
        {
            if (card != null)
            {
                DiceCardXmlInfo xmlData = card.XmlData;
                if (xmlData == null)
                {
                    return false;
                }
                if (xmlData.Keywords.Contains(keyword))
                {
                    return true;
                }
                List<string> abilityKeywords = Singleton<BattleCardAbilityDescXmlList>.Instance.GetAbilityKeywords(xmlData);
                for (int i = 0; i < abilityKeywords.Count; i++)
                {
                    if (abilityKeywords[i] == keyword)
                    {
                        return true;
                    }
                }
                foreach (DiceBehaviour diceBehaviour in card.GetBehaviourList())
                {
                    List<string> abilityKeywords_byScript = Singleton<BattleCardAbilityDescXmlList>.Instance.GetAbilityKeywords_byScript(diceBehaviour.Script);
                    for (int j = 0; j < abilityKeywords_byScript.Count; j++)
                    {
                        if (abilityKeywords_byScript[j] == keyword)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            return false;
        }

        public static void RemoveAllAltMotion(this CharacterAppearance charAppearance)
        {
            charAppearance.RemoveAltMotion(ActionDetail.Default);
            charAppearance.RemoveAltMotion(ActionDetail.Damaged);
            charAppearance.RemoveAltMotion(ActionDetail.Standing);
            charAppearance.RemoveAltMotion(ActionDetail.Guard);
            charAppearance.RemoveAltMotion(ActionDetail.Aim);
            charAppearance.RemoveAltMotion(ActionDetail.Hit);
            charAppearance.RemoveAltMotion(ActionDetail.Slash);
            charAppearance.RemoveAltMotion(ActionDetail.Penetrate);
            charAppearance.RemoveAltMotion(ActionDetail.Move);
            charAppearance.RemoveAltMotion(ActionDetail.Fire);
            charAppearance.RemoveAltMotion(ActionDetail.Evade);
        }

        public static BattleUnitModel GetPatron(this BattleObjectManager manager)
        {
            return manager.GetAliveList(Faction.Player).SortReturn((BattleUnitModel x, BattleUnitModel y) => x.index - y.index)[0];
        }

        /// <summary>
        /// Replaces color shorthands from the XMLs.
        /// <br></br>[green] - #33DD11
        /// <br></br>[red] - #DD3311
        /// </summary>
        public static string ReplaceColorShorthands(this string input)
        {
            string colored = input;
            foreach (var values in colorShorthands)
            {
                colored = colored.Replace(values.Key, values.Value);
            }
            return colored;
        }

        public static Dictionary<string, string> colorShorthands = new Dictionary<string, string>
        {
            {"[green]", "<color=#33DD11>"},
            {"[red]", "<color=#DD3311>"}
        };

        /*
        
        */
    }

    [Serializable]
    public class RMRConfigRoot
    {
        [XmlElement("ShowAdditionalLogging")]
        public bool EnableAdditionalLogging = false;

        [XmlElement("ShowAllItemCatalog")]
        public bool ShowAllItemCatalog = false;
    }

    #region UNIT UTIL
    /// <summary>
    /// A simplified BattleUnitModel object for more easily cloning BattleUnitModels.
    /// </summary>
    public class UnitModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Pos { get; set; }

        public SephirahType Sephirah { get; set; }

        public bool LockedEmotion { get; set; }

        public int MaxEmotionLevel { get; set; } = 0;

        public int EmotionLevel { get; set; }

        public bool AddEmotionPassive { get; set; } = true;

        public bool OnWaveStart { get; set; }

        public XmlVector2 CustomPos { get; set; }
    }

    /// <summary>
    /// Initially taken from CWR (with permission from undefined) before being improved upon 
    /// <br></br>by fixing some UI bugs and other unrelated jank and by adding some shortcuts for
    /// <br></br>storing BattleUnitModels easily.
    /// </summary>
    public static class UnitUtil
    {
        public enum UnitSpawnMethod
        {
            Default,
            Summoned = 7,
            Cloned = 8
        }

        /// <summary>
        /// Copies a <see cref="BattleUnitModel"/> into a simplified <see cref="UnitModel"/> for cloning purposes.
        /// <br></br>Useful for adding controllable units with <see cref="AddModdedUnitPlayerSide"/>.
        /// </summary>
        public static UnitModel Copy(this BattleUnitModel model) => new UnitModel()
        {
            Id = model.id,
            Name = model.UnitData.unitData.name,
            Pos = model.index,
            Sephirah = model.UnitData.unitData.OwnerSephirah,
            LockedEmotion = false,
            MaxEmotionLevel = model.emotionDetail.MaximumEmotionLevel,
            EmotionLevel = model.emotionDetail.EmotionLevel,
            OnWaveStart = true,
            CustomPos = new XmlVector2 { x = model.formation.Pos.x, y = model.formation.Pos.y }
        };

        /// <summary>
        /// Clones an existing BattleUnitModel and summons them.
        /// </summary>
        /// <returns>The cloned <see cref="BattleUnitModel"/>.</returns>
        public static BattleUnitModel CopyModdedUnit(this StageController instance, Faction faction, BattleUnitModel cloner, int index = -1, int height = -1, XmlVector2 position = null)
        {
            UnitBattleDataModel unitBattleDataModel = new UnitBattleDataModel(instance.GetStageModel(), cloner.UnitData.unitData);
            if (faction > Faction.Enemy)
            {
                FieldInfo fieldInfo = AccessTools.Field(typeof(UnitDataModel), "_ownerSephirah");
                fieldInfo.SetValue(unitBattleDataModel.unitData, instance.CurrentFloor);
            }
            if (height != -1)
            {
                unitBattleDataModel.unitData.customizeData.height = height;
            }
            BattleUnitModel battleUnitModel = BattleObjectManager.CreateDefaultUnit(faction);
            UnitDataModel unitData = unitBattleDataModel.unitData;
            if (index < 0)
            {
                IEnumerable<int> source = from y in BattleObjectManager.instance.GetAliveList(faction)
                                          select y.index;
                int num = 0;
                while (index < 0)
                {
                    if (!source.Contains(num))
                    {
                        index = num;
                        break;
                    }
                    num++;
                }
            }
            battleUnitModel.index = index;
            battleUnitModel.grade = unitData.grade;
            if (faction == Faction.Enemy)
            {
                StageWaveModel currentWaveModel = instance.GetCurrentWaveModel();
                if (position != null)
                {
                    battleUnitModel.formation = new FormationPosition(new FormationPositionXmlData
                    {
                        vector = position
                    });
                }
                else
                {
                    int num2 = Mathf.Min(battleUnitModel.index, currentWaveModel.GetFormation().PostionList.Count - 1);
                    if (num2 < battleUnitModel.index)
                    {
                        Debug.Log("UnitUtil: Index higher than available formation positions, summoning at highest value possible");
                    }
                    battleUnitModel.formation = new FormationPosition(new FormationPositionXmlData
                    {
                        vector = new XmlVector2
                        {
                            x = currentWaveModel.GetFormationPosition(num2).Pos.x,
                            y = currentWaveModel.GetFormationPosition(num2).Pos.y
                        }
                    });
                }
            }
            else
            {
                StageLibraryFloorModel floor = instance.GetStageModel().GetFloor(instance.CurrentFloor);
                if (position != null)
                {
                    battleUnitModel.formation = new FormationPosition(new FormationPositionXmlData
                    {
                        vector = position
                    });
                }
                else
                {
                    int num3 = Mathf.Min(battleUnitModel.index, floor.GetFormation().PostionList.Count - 1);
                    if (num3 < battleUnitModel.index)
                    {
                        Debug.Log("AftermathCollection: Index higher than available formation positions, summoning at highest value possible");
                    }
                    battleUnitModel.formation = new FormationPosition(new FormationPositionXmlData
                    {
                        vector = new XmlVector2
                        {
                            x = floor.GetFormationPosition(num3).Pos.x,
                            y = floor.GetFormationPosition(num3).Pos.y
                        }
                    });
                }
            }
            BattleUnitModel result;
            if (unitBattleDataModel.isDead)
            {
                result = battleUnitModel;
            }
            else
            {
                battleUnitModel.SetUnitData(unitBattleDataModel);
                battleUnitModel.OnCreated();
                BattleObjectManager.instance.RegisterUnit(battleUnitModel);
                battleUnitModel.passiveDetail.OnUnitCreated();
                UnitUtil.AddEmotionPassives(battleUnitModel);
                battleUnitModel.cardSlotDetail.RecoverPlayPoint(battleUnitModel.cardSlotDetail.GetMaxPlayPoint());
                battleUnitModel.OnWaveStart();
                UnitUtil.LevelUpEmotion(battleUnitModel, 0);
                UnitUtil.InitializeCombatUI(battleUnitModel);
                battleUnitModel.history.data2 = (int)UnitSpawnMethod.Cloned;
                result = battleUnitModel;
            }
            return result;
        }

        /// <summary>
        /// Summons a new unit.
        /// </summary>
        /// <returns>The summoned <see cref="BattleUnitModel"/>.</returns>
        public static BattleUnitModel AddModdedUnit(this StageController instance, Faction faction, LorId enemyUnitId, int index = -1, int height = -1, XmlVector2 position = null)
        {
            UnitBattleDataModel unitBattleDataModel = UnitBattleDataModel.CreateUnitBattleDataByEnemyUnitId(instance.GetStageModel(), enemyUnitId);
            if (faction > Faction.Enemy)
            {
                FieldInfo fieldInfo = AccessTools.Field(typeof(UnitDataModel), "_ownerSephirah");
                fieldInfo.SetValue(unitBattleDataModel.unitData, instance.CurrentFloor);
            }
            if (height != -1)
            {
                unitBattleDataModel.unitData.customizeData.height = height;
            }
            BattleObjectManager.instance.UnregisterUnitByIndex(faction, index);
            BattleUnitModel battleUnitModel = BattleObjectManager.CreateDefaultUnit(faction);
            UnitDataModel unitData = unitBattleDataModel.unitData;
            if (index < 0)
            {
                IEnumerable<int> source = from y in BattleObjectManager.instance.GetAliveList(faction)
                                          select y.index;
                int num = 0;
                while (index < 0)
                {
                    if (!source.Contains(num))
                    {
                        index = num;
                        break;
                    }
                    num++;
                }
            }
            battleUnitModel.index = index;
            battleUnitModel.grade = unitData.grade;
            if (faction == Faction.Enemy)
            {
                StageWaveModel currentWaveModel = instance.GetCurrentWaveModel();
                if (position != null)
                {
                    battleUnitModel.formation = new FormationPosition(new FormationPositionXmlData
                    {
                        vector = position
                    });
                }
                else
                {
                    int num2 = Mathf.Min(battleUnitModel.index, currentWaveModel.GetFormation().PostionList.Count - 1);
                    if (num2 < battleUnitModel.index)
                    {
                        Debug.Log("UnitUtil: Index higher than available formation positions, summoning at highest value possible");
                    }
                    battleUnitModel.formation = new FormationPosition(new FormationPositionXmlData
                    {
                        vector = new XmlVector2
                        {
                            x = currentWaveModel.GetFormationPosition(num2).Pos.x,
                            y = currentWaveModel.GetFormationPosition(num2).Pos.y
                        }
                    });
                }
            }
            else
            {
                StageLibraryFloorModel floor = instance.GetStageModel().GetFloor(instance.CurrentFloor);
                if (position != null)
                {
                    battleUnitModel.formation = new FormationPosition(new FormationPositionXmlData
                    {
                        vector = position
                    });
                }
                else
                {
                    int num3 = Mathf.Min(battleUnitModel.index, floor.GetFormation().PostionList.Count - 1);
                    if (num3 < battleUnitModel.index)
                    {
                        Debug.Log("UnitUtil: Index higher than available formation positions, summoning at highest value possible");
                    }
                    battleUnitModel.formation = new FormationPosition(new FormationPositionXmlData
                    {
                        vector = new XmlVector2
                        {
                            x = floor.GetFormationPosition(num3).Pos.x,
                            y = floor.GetFormationPosition(num3).Pos.y
                        }
                    });
                }
            }
            BattleUnitModel result;
            if (unitBattleDataModel.isDead)
            {
                result = battleUnitModel;
            }
            else
            {
                battleUnitModel.SetUnitData(unitBattleDataModel);
                battleUnitModel.OnCreated();
                BattleObjectManager.instance.RegisterUnit(battleUnitModel);
                battleUnitModel.passiveDetail.OnUnitCreated();
                UnitUtil.AddEmotionPassives(battleUnitModel);
                battleUnitModel.cardSlotDetail.RecoverPlayPoint(battleUnitModel.cardSlotDetail.GetMaxPlayPoint());
                battleUnitModel.OnWaveStart();
                battleUnitModel.allyCardDetail.DrawCards(battleUnitModel.UnitData.unitData.GetStartDraw());
                UnitUtil.LevelUpEmotion(battleUnitModel, 0);
                UnitUtil.InitializeCombatUI(battleUnitModel);
                battleUnitModel.history.data2 = (int)UnitSpawnMethod.Summoned;
                result = battleUnitModel;
            }
            return result;
        }

        /// <summary>
        /// Summons an unit that can be controlled by the player.
        /// </summary>
        /// <returns>The summoned <see cref="BattleUnitModel"/>.</returns>
        public static BattleUnitModel AddModdedUnitPlayerSide(this StageController instance, UnitModel unit, string packageId, bool playerSide = true)
        {
            StageLibraryFloorModel floor = instance.GetStageModel().GetFloor(instance.CurrentFloor);
            UnitDataModel unitDataModel = new UnitDataModel(new LorId(packageId, unit.Id), playerSide ? floor.Sephirah : SephirahType.None, false);
            unitDataModel.SetCustomName(unit.Name);
            BattleUnitModel battleUnitModel = BattleObjectManager.CreateDefaultUnit(playerSide ? Faction.Player : Faction.Enemy);
            battleUnitModel.index = unit.Pos;
            battleUnitModel.grade = unitDataModel.grade;
            battleUnitModel.formation = ((unit.CustomPos != null) ? new FormationPosition(new FormationPositionXmlData
            {
                vector = unit.CustomPos
            }) : floor.GetFormationPosition(battleUnitModel.index));
            UnitBattleDataModel unitBattleDataModel = new UnitBattleDataModel(instance.GetStageModel(), unitDataModel);
            unitBattleDataModel.Init();
            battleUnitModel.SetUnitData(unitBattleDataModel);
            battleUnitModel.OnCreated();
            BattleObjectManager.instance.RegisterUnit(battleUnitModel);
            battleUnitModel.passiveDetail.OnUnitCreated();
            UnitUtil.LevelUpEmotion(battleUnitModel, unit.EmotionLevel);
            if (unit.LockedEmotion)
            {
                battleUnitModel.emotionDetail.SetMaxEmotionLevel(unit.MaxEmotionLevel);
            }
            battleUnitModel.allyCardDetail.DrawCards(battleUnitModel.UnitData.unitData.GetStartDraw());
            battleUnitModel.cardSlotDetail.RecoverPlayPoint(battleUnitModel.cardSlotDetail.GetMaxPlayPoint());
            if (unit.AddEmotionPassive)
            {
                UnitUtil.AddEmotionPassives(battleUnitModel);
            }
            battleUnitModel.OnWaveStart();
            UnitUtil.InitializeCombatUI(battleUnitModel);
            battleUnitModel.history.data2 = (int)UnitSpawnMethod.Summoned;
            return battleUnitModel;
        }

        /// <summary>
        /// Refreshes the UI after summoning <see cref="BattleUnitModel"/>s.
        /// <br></br>(If you're summoning just one or several units at once, call this after you're done summoning units to update the UI.)
        /// </summary>
        /// <param name="forceReturn">Forces the units to return to their assigned position in the formation.</param>
        public static void RefreshCombatUI(bool forceReturn = false)
        {
            foreach (ValueTuple<BattleUnitModel, int> valueTuple in BattleObjectManager.instance.GetList().Select((BattleUnitModel value, int i) => new ValueTuple<BattleUnitModel, int>(value, i)))
            {
                SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(valueTuple.Item1.UnitData.unitData, valueTuple.Item2, true, false);
                if (forceReturn)
                {
                    valueTuple.Item1.moveDetail.ReturnToFormationByBlink(true);
                }
            }
            try
            {
                BattleObjectManager.instance.InitUI();
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        /// <summary>
        /// Levels up an unit's emotion level.
        /// </summary>
        /// <param name="unit">Which unit should level up.</param>
        /// <param name="value">How much they should level up.</param>
        public static void LevelUpEmotion(BattleUnitModel unit, int value)
        {
            for (int i = 0; i < value; i++)
            {
                unit.emotionDetail.LevelUp_Forcely(1);
                unit.emotionDetail.CheckLevelUp();
            }
            Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.UpdateCoin();
        }

        /// <summary>
        /// Stores a single unit in Battle Unit Storage.
        /// </summary>
        /// <param name="unit">The unit to be stored.</param>
        /// <param name="packageId">The packageId of the mod.</param>
        public static void StoreBattleUnitModel(BattleUnitModel unit, string packageId)
        {
            Singleton<StageController>.Instance.GetStageModel().GetStageStorageData<List<BattleUnitModel>>(packageId + "_BattleUnitModelStorage", out var output);
            if (output != null && output.Count > 0)
            {
                output.Add(unit);
                Singleton<StageController>.Instance.GetStageModel().SetStageStorgeData(packageId + "_BattleUnitModelStorage", output);
            }
            else
            {
                var list = new List<BattleUnitModel> { unit };
                Singleton<StageController>.Instance.GetStageModel().SetStageStorgeData(packageId + "_BattleUnitModelStorage", list);
            }

        }

        /// <summary>
        /// Stores multiple units in Battle Unit Storage.
        /// </summary>
        /// <param name="units">The units to be stored.</param>
        /// <param name="packageId">The packageId of the mod.</param>
        public static void StoreBattleUnitModel(List<BattleUnitModel> units, string packageId)
        {
            Singleton<StageController>.Instance.GetStageModel().GetStageStorageData<List<BattleUnitModel>>(packageId + "_BattleUnitModelStorage", out var output);
            if (output != null && output.Count > 0)
            {
                output.AddRange(units);
                Singleton<StageController>.Instance.GetStageModel().SetStageStorgeData(packageId + "_BattleUnitModelStorage", output);
            }
            else return;
        }

        /// <param name="packageId">The packageId of the mod.</param>
        /// <returns>A list of <see cref="BattleUnitModel"/>s containing the stored units in Battle Unit Storage.</returns>
        public static List<BattleUnitModel> GetStoredUnitModels(string packageId)
        {
            Singleton<StageController>.Instance.GetStageModel().GetStageStorageData<List<BattleUnitModel>>(packageId + "_BattleUnitModelStorage", out var output);
            if (output != null && output.Count > 0) return output;
            return null;
        }

        /// <summary>
        /// Clears out Battle Unit Storage.
        /// </summary>
        /// <param name="packageId">The packageId of the mod.</param>
        public static void ClearBattleUnitStorage(string packageId)
        {
            Singleton<StageController>.Instance.GetStageModel().SetStageStorgeData(packageId + "_BattleUnitModelStorage", null);
        }

        /// <summary>
        /// Returns the method through which an unit was summoned which can range between <see cref="UnitSpawnMethod.Default"/>, <see cref="UnitSpawnMethod.Summoned"/> or <see cref="UnitSpawnMethod.Cloned"/>.
        /// </summary>
        /// <param name="unit">The unit to check.</param>
        /// <returns>A <see cref="UnitSpawnMethod"/> containing the spawn method of how the unit was spawned.<b><br></br>Only works with units summoned by <see cref="UnitUtil"/>. Any other units will return <see cref="UnitSpawnMethod.Default"/>.</b></returns>
        public static UnitSpawnMethod GetUnitSpawnMethod(BattleUnitModel unit)
        {
            return (UnitSpawnMethod)unit.history.data2;
        }

        private static void InitializeCombatUI(BattleUnitModel battleUnitModel)
        {
            try
            {
                BattleCharacterProfileUI battleUI = SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.GetProfileUI(battleUnitModel);
                if (battleUI == null)
                {
                    battleUI = new BattleCharacterProfileUI();
                    battleUI.gameObject.SetActive(true);
                    battleUI.Initialize();
                    battleUI.SetUnitModel(battleUnitModel);
                    if (battleUnitModel.faction == Faction.Player)
                    {
                        SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.allyarray[battleUnitModel.index] = battleUI;
                    }
                    else
                    {
                        SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.enemyarray[battleUnitModel.index] = battleUI;
                    }

                }
            }
            catch (Exception e)
            {
                if (e.Message == "")
                    Debug.Log("UnitUtil: successfully summoned " + battleUnitModel.UnitData.unitData.name + " at index " + battleUnitModel.index);
                else
                    Debug.Log("UnitUtil - failed to initialize UI of summoned unit: " + e);
            }
        }

        private static void AddEmotionPassives(BattleUnitModel unit)
        {
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(Faction.Player);
            if (aliveList.Any<BattleUnitModel>())
            {
                foreach (BattleEmotionCardModel battleEmotionCardModel in from x in aliveList.FirstOrDefault<BattleUnitModel>().emotionDetail.PassiveList
                                                                          where x.XmlInfo.TargetType == EmotionTargetType.AllIncludingEnemy || x.XmlInfo.TargetType == EmotionTargetType.All
                                                                          select x)
                {
                    bool flag2 = unit.faction != Faction.Enemy || battleEmotionCardModel.XmlInfo.TargetType > EmotionTargetType.All;
                    if (flag2)
                    {
                        unit.emotionDetail.ApplyEmotionCard(battleEmotionCardModel.XmlInfo, false);
                    }
                }
            }
        }
    }
    #endregion

    #region ITEM LOCALIZATION

    public class PickUpRebornModel : PickUpModelBase
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
        /// Do <b>NOT</b> forget to inherit this constructor on your derived <see cref="PickUpRebornModel"/>.
        /// <code>
        /// // You can do it like this:
        /// public class PickUpModel_MyCoolItem : PickUpRebornModel
        /// {
        ///     public PickUpModel_MyCoolItem() : base() // do not forget this : base() part
        ///     {
        ///         this.id = new LorId(myModInitializer.packageId, 1984);
        ///         this.rewardinfo = RewardPassivesList.Instance.GetPassiveInfo(new LorId(myModInitializer.packageId, 1984));
        ///     }
        /// }</code>
        /// </summary>
        public PickUpRebornModel()
        {
            var info = LogueEffectXmlList.Instance.GetEffectInfo(KeywordId, RMRCore.ClassIds[this.GetType().Assembly]);
            if (info != null)
            {
                this.Name = info.Name;
                this.Desc = info.Desc;
                this.FlaverText = info.FlavorText;
                this.ArtWork = KeywordIconId == null ? KeywordId : KeywordIconId;
            }
        }
    }

    [HideFromItemCatalog]
    public class GlobalRebornEffectBase : GlobalLogueEffectBase
    {
        /// <value>
        /// Override this with the ID provided within the effect's respective localization XML.
        /// </value>
        public virtual string KeywordId { get; }

        /// <value>
        /// Override this with the filename of the effect's icon. Defaults to <see cref="KeywordId"/> if not provided.
        /// </value>
        public virtual string KeywordIconId { get; }

        public override string GetEffectDesc()
        {
            LogueEffectXmlInfo info;
            try
            {
                info = LogueEffectXmlList.Instance.GetEffectInfo(KeywordId, RMRCore.ClassIds[this.GetType().Assembly], this.GetStack());
            }
            catch
            {
                info = null;
            }
            return info == null ? "" : info.Desc + "\n\n" + info.FlavorText;
        }

        public virtual string GetCredenzaEntry()
        {
            LogueEffectXmlInfo info;
            try
            {
                info = LogueEffectXmlList.Instance.GetEffectInfo(KeywordId, RMRCore.ClassIds[this.GetType().Assembly], this.GetStack());
            }
            catch
            {
                info = null;
            }
            return info == null ? "" : info.CatalogDesc;
        }

        public override string GetEffectName()
        {
            LogueEffectXmlInfo info;
            try
            {
                info = LogueEffectXmlList.Instance.GetEffectInfo(KeywordId, RMRCore.ClassIds[this.GetType().Assembly], this.GetStack());
            }
            catch
            {
                info = null;
            }
            return info == null ? "" : info.Name;
        }

        public override Sprite GetSprite()
        {
            Sprite sprite;
            string id;
            try
            {
                id = RMRCore.ClassIds[this.GetType().Assembly];
            }
            catch
            {
                return null;
            }
            try
            {
                if (id == RMRCore.packageId)
                    sprite = LogLikeMod.ArtWorks[KeywordIconId == null ? KeywordId : KeywordIconId];
                else
                    sprite = LogLikeMod.ModdedArtWorks[(id, KeywordIconId == null ? KeywordId : KeywordIconId)];
            }
            catch
            {
                return null;
            }
            return sprite;
        }
    }
    #endregion

    #region MYSTERY EVENT LOC.
    public class RogueMysteryXmlList : Singleton<RogueMysteryXmlList>
    {
        public void Init(string language)
        {
            string str = Path.Combine("Localize", language);
            string ogpath = Path.Combine(ModContentManager.Instance.GetModPath(RMRCore.packageId), "Assemblies", "dlls");

            if (Directory.Exists(Path.Combine(ogpath, str, "MysteryEvents")))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(ogpath, str, "MysteryEvents"));
                foreach (System.IO.FileInfo fileinfo in directoryInfo.GetFiles())
                {
                    try
                    {
                        var root = new XmlSerializer(typeof(RogueMysteryXmlRoot)).Deserialize(fileinfo.OpenRead()) as RogueMysteryXmlRoot;
                        foreach (var info in root.RogueMysteryList)
                        {
                            var lorid = new LorId(RMRCore.packageId, info.ID);
                            if (!mysteryDict.ContainsKey(lorid))
                                this.mysteryDict.Add(lorid, info);
                            else
                                this.mysteryDict[lorid] = info;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error while trying to load XML file " + fileinfo.Name + ": " + e);
                    }
                }

            }

            foreach (ModContentInfo modContentInfo in LogLikeMod.GetLogMods())
            {
                string uniqueId = modContentInfo.invInfo.workshopInfo.uniqueId;
                if (Directory.Exists(Path.Combine(modContentInfo.GetLogDllPath(), str, "MysteryEvents")))
                {
                    DirectoryInfo directoryInfo2 = new DirectoryInfo(Path.Combine(modContentInfo.GetLogDllPath(), str, "MysteryEvents"));
                    foreach (System.IO.FileInfo fileinfo2 in directoryInfo2.GetFiles())
                    {
                        try
                        {
                            var root = new XmlSerializer(typeof(RogueMysteryXmlRoot)).Deserialize(fileinfo2.OpenRead()) as RogueMysteryXmlRoot;
                            foreach (var info in root.RogueMysteryList)
                            {
                                var lorid = new LorId(uniqueId, info.ID);
                                if (!mysteryDict.ContainsKey(lorid))
                                    this.mysteryDict.Add(lorid, info);
                                else
                                    this.mysteryDict[lorid] = info;
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Error while trying to load XML file " + fileinfo2.Name + ": " + e);
                        }
                    }
                }

            }
        }

        public RogueMysteryXmlInfo GetLocalizedMystery(LorId id)
        {
            if (mysteryDict.ContainsKey(id))
                return mysteryDict[id];
            return null;
        }

        public Dictionary<LorId, RogueMysteryXmlInfo> mysteryDict = new Dictionary<LorId, RogueMysteryXmlInfo>();
    }

    public class RogueMysteryXmlRoot
    {
        [XmlElement("Mystery")]
        public List<RogueMysteryXmlInfo> RogueMysteryList = new List<RogueMysteryXmlInfo>();
    }

    public class RogueMysteryXmlInfo
    {
        public RogueMysteryXmlInfoFrame GetFrameById(int id)
        {
            return FrameList.Find(x => x.ID == id);
        }

        [XmlElement("Frame")]
        public List<RogueMysteryXmlInfoFrame> FrameList = new List<RogueMysteryXmlInfoFrame>();

        [XmlAttribute]
        public int ID = -1;

        [XmlElement]
        public string Title = "";
    }

    public class RogueMysteryXmlInfoFrame
    {
        public RogueMysteryXmlInfoFrameChoice GetChoiceById(int id)
        {
            return ChoiceList.Find(x => x.ID == id);
        }

        [XmlIgnore]
        public string Dialogs
        {
            get
            {
                string text = "";
                if (Dialog.Count > 0)
                {
                    foreach (var dia in Dialog)
                    {
                        text += dia.ReplaceColorShorthands() + Environment.NewLine;
                    }
                }
                return text;
            }
        }

        [XmlAttribute]
        public int ID = -1;

        [XmlAttribute]
        public FrameType FrameType = FrameType.DEFAULT;

        [XmlElement]
        public List<string> Dialog = new List<string>();

        [XmlElement("Choice")]
        public List<RogueMysteryXmlInfoFrameChoice> ChoiceList = new List<RogueMysteryXmlInfoFrameChoice>();
    }

    public class RogueMysteryXmlInfoFrameChoice
    {
        [XmlAttribute]
        public int ID = -1;

        [XmlElement]
        public string Desc = "";
    }
    #endregion

    #region GAMEMODE MANAGEMENT

    public class RoguelikeGamemodeController : Singleton<RoguelikeGamemodeController>
    {
        public void AddGamemodesToList()
        {
            Assembly[] assemblies = LogLikeMod.GetAssemList().Distinct().ToArray();
            for (int b = 0; b < assemblies.Length; b++)
            {
                try
                {
                    TypeInfo[] effects = assemblies[b].DefinedTypes.ToList().FindAll(x => x.IsSubclassOf(typeof(RoguelikeGamemodeBase))).ToArray();
                    for (int i = 0; i < effects.Length; i++)
                    {
                        if (effects[i] != null)
                            gamemodeList.Add(Activator.CreateInstance(effects[i].AsType()) as RoguelikeGamemodeBase);
                    }
                    gamemodeList.Add(new RoguelikeGamemode_RMR_Modded());
                }
                catch
                { }
            }
        }

        // Handles save file creation, deletion and initialization
        public void LoadGamemodeByStageRecipe(LorId stageRecipe, bool isContinue)
        {
            this.isContinue = false;
            if (isContinue)
            {
                this.isContinue = true;
                LoadGamemodeByName(Singleton<LogueSaveManager>.Instance.LoadData("Lastest").GetString("CurrentGamemode"), true);
                return;
            }
            var gamemode = gamemodeList.Find(x => stageRecipe == x.StageStart);
            if (gamemode == null) return;
            else if (!File.Exists(Path.Combine(LogueSaveManager.Saveroot, gamemode.SaveDataString)))
            {
                SaveData data = new SaveData(SaveDataType.Dictionary);
                using (FileStream fileStream = File.Create(Path.Combine(LogueSaveManager.Saveroot, gamemode.SaveDataString)))
                {
                    new BinaryFormatter().Serialize(fileStream, data.GetSerializedData());
                }
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RogueLikeGamemodeManager());
            }
            else
            {
                LogueSaveManager.Instance.RemoveData(gamemode.SaveDataString);
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RogueLikeGamemodeManager());
            }
            RMRCore.CurrentGamemode = gamemode;
        }

        // Handles save file creation, deletion and initialization
        public void LoadGamemodeByName(string name, bool isContinue)
        {
            var gamemode = gamemodeList.Find(x => x.GetType().Name == name);
            if (gamemode == null)
            {
                Debug.Log("LoadGamemodeByName failed to load gamemode, real shit??");
                return;
            }
            else if (isContinue)
                this.InitializeGamemode(gamemode);
            else if (!File.Exists(Path.Combine(LogueSaveManager.Saveroot, gamemode.SaveDataString)))
            {
                SaveData data = new SaveData(SaveDataType.Dictionary);
                using (FileStream fileStream = File.Create(Path.Combine(LogueSaveManager.Saveroot, gamemode.SaveDataString)))
                {
                    new BinaryFormatter().Serialize(fileStream, data.GetSerializedData());
                }
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RogueLikeGamemodeManager());
            }
            else
            {
                LogueSaveManager.Instance.RemoveData(gamemode.SaveDataString);
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new RogueLikeGamemodeManager());
            }
            RMRCore.CurrentGamemode = gamemode;
        }

        public void SaveCurrentGamemodeData()
        {
            Singleton<LogueSaveManager>.Instance.SaveData(RMRCore.CurrentGamemode.GetSaveData(), RMRCore.CurrentGamemode.SaveDataString);
        }

        public void SaveCurrentGamemodeName(SaveData data)
        {
            data.SetData("CurrentGamemode", new SaveData(RMRCore.CurrentGamemode.GetType().Name));
        }

        public void InitializeGamemode(RoguelikeGamemodeBase gamemode)
        {
            SaveData data = Singleton<LogueSaveManager>.Instance.LoadData(gamemode.SaveDataString);
            gamemode.LoadFromSaveData(data);
        }

        public bool isContinue = false;

        public List<RoguelikeGamemodeBase> gamemodeList = new List<RoguelikeGamemodeBase>();

    }

    /// <summary>
    /// Determines the origin of the content.
    /// </summary>
    public enum ContentScope
    {
        /// <summary>
        /// All content will be loaded.
        /// </summary>
        ALL,
        /// <summary>
        /// Only Loglike/RMR content will be loaded.
        /// </summary>
        ONLY_LOGLIKE,
        /// <summary>
        /// Only satellite mod content will be loaded.
        /// </summary>
        ONLY_MODDED,
        /// <summary>
        /// Only satellite mod content from a certain package id will be loaded.<br></br>
        /// PackageId must be specified by overriding <see cref="RoguelikeGamemodeBase.GetContentScopePackageId"/>.
        /// </summary>
        ONLY_PACKAGEID
    }

    public class RoguelikeGamemodeBase
    {
        /// <summary>
        /// Controls how to filter content when initializing the gamemode.
        /// <br></br>Do not mess with this unless you know what you are doing.
        /// </summary>
        public virtual void FilterContent()
        {
            switch (this.GetMysteryScope())
            {
                case ContentScope.ALL:
                    break;
                case ContentScope.ONLY_LOGLIKE:
                    MysteryXmlList.Instance.info.RemoveAll(x => x.StageId.packageId != RMRCore.packageId);
                    break;
                case ContentScope.ONLY_MODDED:
                    MysteryXmlList.Instance.info.RemoveAll(x => x.StageId.packageId == RMRCore.packageId);
                    break;
                case ContentScope.ONLY_PACKAGEID:
                    MysteryXmlList.Instance.info.RemoveAll(x => x.StageId.packageId != this.GetContentScopePackageId);
                    break;
            }
            switch (this.GetStagesScope())
            {
                case ContentScope.ALL:
                    break;
                case ContentScope.ONLY_LOGLIKE:
                    foreach (var stage in StagesXmlList.Instance.infos)
                        stage.Stages.RemoveAll(x => x.Id.packageId != RMRCore.packageId);
                    break;
                case ContentScope.ONLY_MODDED:
                    foreach (var stage in StagesXmlList.Instance.infos)
                        stage.Stages.RemoveAll(x => x.Id.packageId == RMRCore.packageId);
                    break;
                case ContentScope.ONLY_PACKAGEID:
                    foreach (var stage in StagesXmlList.Instance.infos)
                        stage.Stages.RemoveAll(x => x.Id.packageId != this.GetContentScopePackageId);
                    break;
            }
            switch (this.GetRewardsScope())
            {
                case ContentScope.ALL:
                    break;
                case ContentScope.ONLY_LOGLIKE:
                    RewardPassivesList.Instance.infos.RemoveAll(x => x.Id.packageId != RMRCore.packageId);
                    break;
                case ContentScope.ONLY_MODDED:
                    RewardPassivesList.Instance.infos.RemoveAll(x => x.Id.packageId == RMRCore.packageId);
                    break;
                case ContentScope.ONLY_PACKAGEID:
                    RewardPassivesList.Instance.infos.RemoveAll(x => x.Id.packageId != this.GetContentScopePackageId);
                    break;
            }
        }
        /// <summary>
        /// This determines the name of the savefile containing gamemode-specific data.
        /// </summary>
        public virtual string SaveDataString => "RMR_GMSaveData_" + this.GetType().Name;

        /// <summary>
        /// The LorId of the reception used to start a new run in this gamemode.
        /// </summary>
        public virtual LorId StageStart { get; }

        /// <summary>
        /// Use this to <b>load</b> whatever persistent data from save data.
        /// </summary>
        public virtual void LoadFromSaveData(SaveData data)
        {
            
        }
        
        /// <summary>
        /// Use this to <b>save</b> whatever persistent data to the player's save data.
        /// </summary>
        public virtual SaveData GetSaveData()
        {
            SaveData saveData = new SaveData();
            saveData.AddData("GamemodeName", new SaveData(this.GetType().Name));
            return saveData;
        }

        /// <summary>
        /// Runs when initializing a roguelike reception, BEFORE creating the player.<br></br>
        /// Hooked up to <see cref="LogLikeMod.UIInvitationRightMainPanel_ConfirmSendInvitation"/>.<br></br>
        /// Runs immediately <b>before</b> StageClassInfo.mapInfo.clear().
        /// </summary>
        public virtual void BeforeInitializeGamemode()
        {
            // done
        }

        /// <summary>
        /// Runs when initializing a roguelike reception, AFTER creating the player.<br></br>
        /// Hooked up to <see cref="LogLikeMod.UIInvitationRightMainPanel_ConfirmSendInvitation"/>.<br></br>
        /// Runs immediately <b>prior</b> to all return calls.
        /// </summary>
        public virtual void AfterInitializeGamemode()
        {
            // done
        }

        /// <summary>
        /// Runs at the start of the Act of the first encounter.<br></br>
        /// Feel free to insert your initial event call here via <see cref="Singleton&lt;MysteryManager&gt;.Instance.StartMystery(LorId mysteryId)"/>.
        /// </summary>
        public virtual void OnWaveStartInitialEvent()
        {
            // done
        }

        /// <summary>
        /// Runs whenever the player enters a shop.
        /// </summary>
        public virtual void OnEnterShop(ShopBase shop)
        {
            // done
        }

        /// <summary>
        /// Runs whenever the player exits a shop.
        /// </summary>
        public virtual void OnExitShop(ShopBase shop)
        {
            // done
        }

        /*
        /// <summary>
        /// Runs whenever a new Librarian is added to the floor.<br></br>
        /// <b>Returning <see langword="false"/> prevents the Librarian from being added.</b>
        /// </summary>
        public virtual bool OnAddLibrarian(UnitDataModel model)
        {
            return true; //done 
        }
        */

        /// <summary>
        /// Determines from which mods RMR should take Mystery Events from.
        /// </summary>
        public virtual ContentScope GetMysteryScope()
        {
            return ContentScope.ALL;
        }

        /// <summary>
        /// Determines from which mods RMR should take encounters from.
        /// </summary>
        public virtual ContentScope GetStagesScope()
        {
            return ContentScope.ALL;
        }

        /// <summary>
        /// Determines from which mods RMR should take rewards from.
        /// </summary>
        public virtual ContentScope GetRewardsScope()
        {
            return ContentScope.ALL;
        }

        /// <summary>
        /// The packageId to be used if any <see cref="ContentScope"/> methods are set to <see cref="ContentScope.ONLY_PACKAGEID"/>.<br></br>
        /// Defaults to <b>this assembly's</b> current packageId. Do not ask how that is done. It simply is.
        /// </summary>
        public virtual string GetContentScopePackageId => RMRCore.ClassIds[this.GetType().Assembly];
    }

    public class RoguelikeGamemode_RMR_Default : RoguelikeGamemodeBase
    {
        public override LorId StageStart => new LorId(LogLikeMod.ModId, -854);

        public override ContentScope GetRewardsScope()
        {
            return ContentScope.ONLY_LOGLIKE;
        }

        public override ContentScope GetMysteryScope()
        {
            return ContentScope.ONLY_LOGLIKE;
        }

        public override ContentScope GetStagesScope()
        {
            return ContentScope.ONLY_LOGLIKE;
        }

        public override void AfterInitializeGamemode()
        {
            if (!RoguelikeGamemodeController.Instance.isContinue)
            {
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter1());
                Singleton<LogStoryPathList>.Instance.LoadStoryFile(new LorId(LogLikeMod.ModId, 1), null, true);
            }
        }

        public override void OnWaveStartInitialEvent()
        {
            Singleton<MysteryManager>.Instance.StartMystery(Singleton<MysteryXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, -1)));
        }

        public override string GetContentScopePackageId => RMRCore.packageId;
    }

    public class RoguelikeGamemode_RMR_Modded : RoguelikeGamemodeBase
    {
        public override LorId StageStart => new LorId(LogLikeMod.ModId, -853);

        public override ContentScope GetRewardsScope()
        {
            return ContentScope.ALL;
        }

        public override ContentScope GetMysteryScope()
        {
            return ContentScope.ALL;
        }

        public override ContentScope GetStagesScope()
        {
            return ContentScope.ALL;
        }

        public override void AfterInitializeGamemode()
        {
            if (!RoguelikeGamemodeController.Instance.isContinue)
            {
                Singleton<GlobalLogueEffectManager>.Instance.AddEffects(new CraftEquipChapter1());
                Singleton<LogStoryPathList>.Instance.LoadStoryFile(new LorId(LogLikeMod.ModId, 1), null, true);
            }
        }

        public override void OnWaveStartInitialEvent()
        {
            Singleton<MysteryManager>.Instance.StartMystery(Singleton<MysteryXmlList>.Instance.GetData(new LorId(LogLikeMod.ModId, -100)));
        }

        public override string GetContentScopePackageId => RMRCore.packageId;
    }

    public class RogueLikeGamemodeManager : GlobalLogueEffectBase
    {
        public RoguelikeGamemodeBase currentGamemode => RMRCore.CurrentGamemode;

        public override void OnEnterShop(ShopBase shop)
        {
            currentGamemode.OnEnterShop(shop);
        }

        public override void OnLeaveShop(ShopBase shop)
        {
            currentGamemode.OnExitShop(shop);
        }

        /*
        public override void OnAddSubPlayer(UnitDataModel model)
        {
            if (!currentGamemode.OnAddLibrarian(model))
            {
                LogueBookModels.playerModel.Remove(model);
                LogueBookModels.playerBattleModel.RemoveAll(x => x.unitData == model);
                LogueBookModels.playersPick.Remove(model);
                LogueBookModels.playersperpassives.Remove(model);
                LogueBookModels.playersstatadders.Remove(model);
                throw new Exception("New Librarian removed (working as intended)");
            }
        }
        */
    }

        #endregion

    #endregion


    #region UI INFRASTRUCTURE

    /// <summary>
    /// If attached to a GlobalLogueEffect, the effect will not be shown in the Item Catalog.
    /// </summary>
    public class HideFromItemCatalog : Attribute
    {

    }

    /// <summary>
    /// If attached to a GlobalLogueEffect, the effect will show in the Item Catalog if the player has obtained it before.
    /// </summary>
    public class SecretInItemCatalog : Attribute
    {

    }

    public class GlobalLogueItemCatalogPanel : Singleton<GlobalLogueItemCatalogPanel>
    {
        public bool IsItemShown(GlobalLogueEffectBase item)
        {
            Type type = item.GetType();
            if (type.GetCustomAttribute<HideFromItemCatalog>(false) != null)
                return false;
            else if (item.GetType().GetCustomAttribute<SecretInItemCatalog>(false) != null)
                return item.HasBeenObtained();
            return true;
        }

        public bool IsItemShown(PickUpModelBase item)
        {
            Type type = item.GetType();
            if (type.GetCustomAttribute<HideFromItemCatalog>(false) != null)
                return false;
            else if (item.GetType().GetCustomAttribute<SecretInItemCatalog>(false) != null)
                return item.HasBeenObtained();
            return true;
        }

        public void GetLogUIObj()
        {
            UIStoryArchivesPanel goG = UI.UIController.Instance.GetUIPanel(UIPanelType.Story) as UIStoryArchivesPanel;
            UICustomTabButton gameObject2 = UnityEngine.Object.Instantiate(goG.tabcontroller.CustomTabs[2], goG.tabcontroller.TabsRoot.transform);
            UIBookStoryPanel gameObject3 = UnityEngine.Object.Instantiate(goG.bookStoryPanel, goG.ActiveControl.transform);
            goG.tabcontroller.CustomTabs = goG.tabcontroller.CustomTabs.Append(gameObject2).ToArray();
            gameObject2.Init(4, goG.tabcontroller);
            LayoutRebuilder.MarkLayoutForRebuild(goG.tabcontroller.GetComponentInChildren<HorizontalLayoutGroup>().transform as RectTransform);
            gameObject2.gameObject.SetActive(true);
            button = gameObject2;
            button.GetComponentInChildren<UITextDataLoader>().key = "ui_RMR_ItemCatalog";
            gameObject3.Initialize();
            gameObject3.gameObject.SetActive(true);
            gameObject3.transform.SetAsFirstSibling();
            root = gameObject3;
        }

        public void Activate()
        {
            this.activated = true;
            LogLikeMod.itemCatalogActive = true;
            root.canvasGroup.alpha = 1f;
            root.canvasGroup.interactable = true;
            root.canvasGroup.blocksRaycasts = true;
            this.UpdateSprites();
        }

        public void Deactivate()
        {
            this.activated = false;
            LogLikeMod.itemCatalogActive = false;
            root.canvasGroup.alpha = 0f;
            root.canvasGroup.interactable = false;
            root.canvasGroup.blocksRaycasts = false;
        }

        public void Init()
        {
            this.sprites = new List<LogueEffectImage_ItemCatalog>();
            List<GlobalLogueEffectBase> gamer = new List<GlobalLogueEffectBase>();
            Assembly[] assemblies = LogLikeMod.GetAssemList().Distinct().ToArray();
            for (int b = 0; b < assemblies.Length; b++)
            {
                try
                {
                    TypeInfo[] effects = assemblies[b].DefinedTypes.ToList().FindAll(x => x.IsSubclassOf(typeof(GlobalLogueEffectBase)) || x.IsSubclassOf(typeof(OnceEffect)) || x.IsSubclassOf(typeof(GlobalRebornEffectBase))).ToArray();
                    for (int i = 0; i < effects.Length; i++)
                    {
                        if (effects[i] != null && effects[i].GetCustomAttribute(typeof(HideFromItemCatalog), false) == null) 
                            gamer.Add(Activator.CreateInstance(effects[i].AsType()) as GlobalLogueEffectBase);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Failed to add list of types to sprites: " + e);
                }
            }
            this.effects = gamer.FindAll(x => x != null && x.GetSprite() != null && IsItemShown(x)).ToArray();

            Image image = ModdingUtils.CreateImage(this.root.transform, "RogueLikeRebornIconAlt", new Vector2(1.2f, 1.2f), new Vector2(0f, 0f));
            image.transform.localPosition = new Vector3(525f, -25f, 0f);
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a / 5);

            this.pageText = ModdingUtils.CreateText_TMP(this.root.transform, new Vector2(-1285f, 250f), 64, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0f, 0f), TextAlignmentOptions.BottomRight, LogLikeMod.DefFontColor, LogLikeMod.DefFont_TMP);
            this.pageText.gameObject.SetActive(true);

            Image image3 = ModdingUtils.CreateImage(this.root.transform, "ItemCatalogForwardButton", new Vector2(1f, 1f), new Vector2(0f, 0f), new Vector2(65f, 65f));
            this.pageClickForward = image3.gameObject.AddComponent<UIPageSwitchButton>();
            this.pageClickForward.Init(true);
            this.pageClickForward.transform.localPosition = new Vector3(-265f, -245f); 
            this.pageClickForward.gameObject.SetActive(true);

            Image image4 = ModdingUtils.CreateImage(this.root.transform, "ItemCatalogBackButton", new Vector2(1f, 1f), new Vector2(0f, 0f), new Vector2(65f, 65f));
            this.pageClickBackwards = image4.gameObject.AddComponent<UIPageSwitchButton>();
            this.pageClickBackwards.Init(false);
            this.pageClickBackwards.transform.localPosition = new Vector3(-515f, -245f);
            this.pageClickBackwards.gameObject.SetActive(true);
            
            for (int i = 0; i < rowCount; i++) // 6 rows
            {
                for (int j = 0; j < columnCount; j++) // 9 entries each
                {
                    Image image2 = ModdingUtils.CreateImage(this.root.transform, (Sprite)null, new Vector2(1f, 1f), new Vector2((float)(-785 + j * 100), (float)(350 - i * 100)), new Vector2(85f, 85f));
                    LogueEffectImage_ItemCatalog item = image2.gameObject.AddComponent<LogueEffectImage_ItemCatalog>();
                    this.sprites.Add(item);
                    item.gameObject.SetActive(true);
                }
            }
        }

        public void UpdateSprites()
        {
            foreach (LogueEffectImage_ItemCatalog logueEffectImage_Inventory in this.sprites)
            {
                if (logueEffectImage_Inventory != null)
                    logueEffectImage_Inventory.gameObject.SetActive(false);
            }

            int index = 0;
            for (int i = currentPage * SpriteCount; i < effects.Length; i++)
            {
                this.sprites[index].Init(effects[i]);
                index++;
                if (index >= SpriteCount)
                    break;
            }

            this.pageText.text = string.Concat(currentPage + 1, " / ", pageCount + 1);
        }

        public bool debugMode = false;

        public bool activated = false;

        public int currentPage = 0;

        public int pageCount {
            get
            {
                return effects.Length % SpriteCount == 0 ? effects.Length / SpriteCount - 1 : effects.Length / SpriteCount;
            }
        }

        public int rowCount = 6;

        public int columnCount = 9;

        public int SpriteCount => rowCount * columnCount;

        public UIPageSwitchButton pageClickBackwards;

        public UIPageSwitchButton pageClickForward;

        public TextMeshProUGUI pageText;

        public UICustomTabButton button;

        public UIBookStoryPanel root;

        public GlobalLogueEffectBase[] effects;

        public List<LogueEffectImage_ItemCatalog> sprites;
    }

    public class UIPageSwitchButton : MonoBehaviour
    {
        public void Init(bool forward)
        {
            this.forward = forward;
            this.image = gameObject.GetComponent<Image>();
            if (this.selectable == null)
            {
                this.selectable = base.gameObject.AddComponent<UILogCustomSelectable>();
                this.selectable.targetGraphic = this.image;


                this.selectable.SelectEvent = new UnityEventBasedata();
                this.selectable.SelectEvent.AddListener(delegate (BaseEventData e)
                {
                    this.OnEnterImage();
                });
                this.selectable.onClick = new Button.ButtonClickedEvent();
                this.selectable.onClick.AddListener(delegate
                {
                    this.OnPointerClick();
                });
                this.selectable.DeselectEvent = new UnityEventBasedata();
                this.selectable.DeselectEvent.AddListener(delegate (BaseEventData e)
                {
                    this.OnExitImage();
                });
            }

            this.gameObject.SetActive(true);
        }

        public void OnPointerClick()
        {
            int curPage = Singleton<GlobalLogueItemCatalogPanel>.Instance.currentPage;
            if (forward)
                Singleton<GlobalLogueItemCatalogPanel>.Instance.currentPage = Math.Min(curPage + 1, Singleton<GlobalLogueItemCatalogPanel>.Instance.pageCount);
            else
                Singleton<GlobalLogueItemCatalogPanel>.Instance.currentPage = Math.Max(curPage - 1, 0);
            if (curPage != Singleton<GlobalLogueItemCatalogPanel>.Instance.currentPage)
            {
                Singleton<GlobalLogueItemCatalogPanel>.Instance.root.SetItemRightPanel(null);
                Singleton<GlobalLogueItemCatalogPanel>.Instance.UpdateSprites();
            }
        }

        public void OnEnterImage()
        {
            this.image.color = new Color(0f, 0.6f, 1f);
        }

        public void OnExitImage()
        {
            this.image.color = new Color(1f, 1f, 1f);
        }

        public bool forward;

        public UILogCustomSelectable selectable;

        public Image image;
    }

    public class LogueEffectImage_ItemCatalog : MonoBehaviour
    {
        public void Init(GlobalLogueEffectBase effect)
        {
            if (effect == null)
            {
                base.gameObject.SetActive(false);
            }
            else
            {
                this.effect = effect;
                this.image = base.gameObject.GetComponent<Image>();
                this.image.sprite = LogLikeMod.ArtWorks["ItemCatalogRounded"];
                if (effect.GetSprite() == null)
                {
                    this.Log("effect is null");
                    base.gameObject.SetActive(false);
                }
                else
                {
                    if (this.baseimage == null)
                    {
                        this.baseimage = ModdingUtils.CreateImage(base.transform, "EmptyIcon", new Vector2(1f, 1f), new Vector2(0f, 0f), new Vector2(70f, 70f));
                    }
                    this.sprite = effect.GetSprite();
                    this.baseimage.sprite = this.sprite;
                    this.baseimage.color = this.isObtained ? new Color(1f, 1f, 1f) : Color.black;
                    if (this.selectable == null)
                    {
                        this.selectable = base.gameObject.AddComponent<UILogCustomSelectable>();
                        this.selectable.targetGraphic = this.image;
                        this.selectable.SelectEvent = new UnityEventBasedata();
                        this.selectable.SelectEvent.AddListener(delegate (BaseEventData e)
                        {
                            this.OnEnterImage();
                        });
                        this.selectable.onClick = new Button.ButtonClickedEvent();
                        this.selectable.onClick.AddListener(delegate
                        {
                            this.OnPointerClick();
                        });
                        this.selectable.DeselectEvent = new UnityEventBasedata();
                        this.selectable.DeselectEvent.AddListener(delegate (BaseEventData e)
                        {
                            this.OnExitImage();
                        });
                    }
                    this.gameObject.SetActive(true);
                    this.update = false;
                    if (SingletonBehavior<UIMainOverlayManager>.Instance != null)
                    {
                        SingletonBehavior<UIMainOverlayManager>.Instance.Close();
                    }
                }
            }
        }

        public void OnPointerClick()
        {
            Singleton<GlobalLogueItemCatalogPanel>.Instance.root.SetItemRightPanel(this);
        }

        public void OnEnterImage()
        {
            if (!string.IsNullOrEmpty(this.effect.GetEffectDesc()))
            {
                SingletonBehavior<UIMainOverlayManager>.Instance.SetTooltip(
                    this.isObtained ? this.effect.GetEffectName() : TextDataModel.GetText("ui_RMR_ItemNotObtained_Name"), 
                    this.isObtained ? this.effect.GetEffectDesc() + "\n\n" + TextDataModel.GetText("ui_RMR_ItemObtainCount", this.effect.GetItemObtainCount()) : TextDataModel.GetText("ui_RMR_ItemNotObtained_Desc"), 
                    base.transform as RectTransform, 
                    UIToolTipPanelType.OnlyContent
                );
                this.image.color = new Color(0f, 0.6f, 1f);
                this.update = true;
            }
        }

        public void OnExitImage()
        {
            SingletonBehavior<UIMainOverlayManager>.Instance.Close();
            this.image.color = new Color(1f, 1f, 1f);
            this.update = false;
        }

        public bool update;

        public bool isObtained
        {
            get
            {
                return effect.HasBeenObtained() || Singleton<GlobalLogueItemCatalogPanel>.Instance.debugMode;
            }
        }

        public GlobalLogueEffectBase effect;

        public UILogCustomSelectable selectable;

        public Sprite sprite;

        public Image image;

        public Image baseimage;
    }

    public static class UIItemCatalogPanel
    {

        public static string GetItemCredenzaEntry(this GlobalLogueEffectBase item)
        {
            if (item.GetType().IsSubclassOf(typeof(GlobalRebornEffectBase)))
                return ((GlobalRebornEffectBase)item).GetCredenzaEntry();
            if (LogueEffectXmlList.Instance.TryGetVanillaEffectInfo(item, out LogueEffectXmlInfo loc, item.GetStack()))
                return loc.CatalogDesc;
            return item.GetEffectDesc();
        }


        public static string GetItemCredenzaEntry(this PickUpModelBase item)
        {
            if (item.GetType().IsSubclassOf(typeof(PickUpRebornModel)))
                return ((PickUpRebornModel)item).;
            if (LogueEffectXmlList.Instance.TryGetVanillaEffectInfo(item, out LogueEffectXmlInfo loc, item.GetStack()))
                return loc.CatalogDesc;
            return item.GetEffectDesc();
        }


        public static string GetItemKeywordId(this GlobalLogueEffectBase item)
        {
            if (item.GetType().IsSubclassOf(typeof(GlobalRebornEffectBase)))
                return ((GlobalRebornEffectBase)item).KeywordId;
            if (LogueEffectXmlList.Instance.TryGetVanillaEffectInfo(item, out LogueEffectXmlInfo loc, item.GetStack()))
                return loc.Id;
            return "NO_KEYWORD_GIVEN";
        }

        public static int GetItemObtainCount(this GlobalLogueEffectBase item)
        {
            try { 
                int count = Singleton<LogueSaveManager>.Instance.LoadData("RMR_ItemCatalog").GetInt("ObtainCount_" + item.GetType().Name);
                return count;
            } catch
            { }
            return 0;
        }

        public static bool HasBeenObtained(this GlobalLogueEffectBase item)
        {
            try
            {
                bool obtain = Singleton<LogueSaveManager>.Instance.LoadData("RMR_ItemCatalog").GetInt("ObtainCount_" + item.GetType().Name) > 0;
                return obtain;
            } catch
            { }
            return false;
        }

        public static void SetItemRightPanel(this UIBookStoryPanel panel, LogueEffectImage_ItemCatalog item)
        {
            if (item == null)
            {
                panel.equipPageName.text = "";
                panel.equipPageStory.text = "";
                panel.portrait.enabled = false;
                panel.SelectablePanel_Text.ChildSelectable.interactable = false;
                return;
            }
            if (Singleton<GlobalLogueItemCatalogPanel>.Instance.debugMode)
            {  
                panel.equipPageStory.text = "--- DEBUGGING INFO ---\n" + 
                    "Class name: " + item.effect.GetType().Name + "\n" +
                    "Full class path: " + item.effect.GetType().FullName + "\n" + 
                    "KeywordId: " + item.effect.GetItemKeywordId() + "\n" +
                    "PackageId/AssemblyId: " + RMRCore.ClassIds[item.effect.GetType().Assembly] + 
                    "\n--- DEBUGGING INFO ---\n\n" +
                    item.effect.GetItemCredenzaEntry();
            }
            else panel.equipPageStory.text = item.isObtained ? item.effect.GetItemCredenzaEntry() : TextDataModel.GetText("ui_RMR_ItemNotObtained_Credenza");
            panel.equipPageName.text = item.isObtained ? item.effect.GetEffectName() : TextDataModel.GetText("ui_RMR_ItemNotObtained_Name");
            
            panel.portrait.sprite = item.isObtained ? item.sprite : LogLikeMod.ArtWorks["ItemNotFoundIcon"];
            panel.portrait.enabled = true;
            LayoutRebuilder.ForceRebuildLayoutImmediate(panel.equipPageStory.GetComponent<RectTransform>());
            panel.scrollbar.scrollbar.Select();
            panel.scrollbar.scrollbar.value = 1f;
        }

        public static void Initialize(this UIBookStoryPanel panel)
        {
            panel.SetData();
            panel.equipPageName.text = "";
            panel.equipPageStory.text = "";
            if (panel.portrait.GetComponentInParent<Mask>() is var mask && mask != null)
            {
                mask.gameObject.transform.localPosition = new Vector3(170f, 370f, 0f);
                mask.gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            }
            
            panel.portrait.transform.localPosition = new Vector3(0f, 0f, 0f);
            panel.portrait.transform.Rotate(337.5f, 0f, 0f);
            panel.portrait.transform.localScale = new Vector3(0.48f, 0.48f, 1f);
            panel.portrait.enabled = false;
            panel.SelectablePanel_Text.ChildSelectable.interactable = false;

            UnityEngine.Object.Destroy(panel.bookListDownButton.gameObject);
            UnityEngine.Object.Destroy(panel.bookListUpButton.gameObject);
            UnityEngine.Object.Destroy(panel.BookListLayout.gameObject);
            for (int i = 0; i < panel.bookSlots.Count; i++)
                if (panel.bookSlots[i] != null)
                    UnityEngine.Object.Destroy(panel.bookSlots[i].gameObject);
            for (int i = 0; i < panel.bookStoryChapterSlots.Count; i++)
                if (panel.bookStoryChapterSlots[i] != null)
                    UnityEngine.Object.Destroy(panel.bookStoryChapterSlots[i].gameObject);
            for (int i = 0; i < panel.downSelectableObjects.Count; i++)
                if (panel.downSelectableObjects[i] != null)
                    UnityEngine.Object.Destroy(panel.downSelectableObjects[i]);
            UnityEngine.Object.Destroy(panel.episodeListScroll.gameObject);
            for (int i = 0; i < panel.upSelectableObjects.Count; i++)
                if (panel.upSelectableObjects[i] != null)
                    UnityEngine.Object.Destroy(panel.upSelectableObjects[i]);
            UnityEngine.Object.Destroy(panel.equipPageSlot.rect);
            UnityEngine.Object.Destroy(panel.equipPageSlot.gameObject);
            UnityEngine.Object.Destroy(panel.SelectablePanel_books.gameObject);
            UnityEngine.Object.Destroy(panel.SelectablePanel_epis.gameObject);
            UnityEngine.Object.Destroy(panel.slotsLayoutGroup.gameObject);
            UnityEngine.Object.Destroy(panel.selectedEpisodeIcon.gameObject);
            UnityEngine.Object.Destroy(panel.selectedEpisodeIconGlow.gameObject);
            UnityEngine.Object.Destroy(panel.selectedEpisodeText.gameObject);
            UnityEngine.Object.Destroy(panel.selectedEpisodeTitleRect.gameObject);
            UnityEngine.Object.Destroy(panel.nullEpisodeRect);
            UnityEngine.Object.Destroy(panel.slotsLayoutGroup.gameObject);
        }

        public static void SetTooltip(this UIMainOverlayManager __instance, string name, string content, RectTransform rectTransform, UIToolTipPanelType panelType = UIToolTipPanelType.Normal)
        {
            __instance.Open();
            __instance.tooltipName.text = name;
            __instance.tooltipName.rectTransform.sizeDelta = new Vector2(__instance.tooltipName.rectTransform.sizeDelta.x, 20f);
            Camera camera = null;
            if (rectTransform != null)
            {
                Graphic componentInChildren = rectTransform.GetComponentInChildren<Graphic>();
                bool flag2 = componentInChildren != null && componentInChildren.canvas.renderMode == RenderMode.ScreenSpaceCamera;
                if (flag2)
                {
                    camera = Camera.main;
                }
            }
            __instance.tooltipDesc.text = content;
            __instance.SetTooltipOverlayBoxSize(panelType);
            __instance.SetTooltipOverlayBoxPosition(camera, rectTransform);
        }
    }

    #endregion


    #region HARMONY PATCHES
    [HarmonyPatch]
    public class RMR_Patches
    {
        #region POSTFIXES
        // Responsible for making the Item Catalog Credenza tab activate and deactivate other panels
        [HarmonyPostfix, HarmonyPatch(typeof(UIStoryArchivesPanel), nameof(UIStoryArchivesPanel.TabControllerUpdated))]
        static void OpenItemCatalogTab_Post(UIStoryArchivesPanel __instance)
        {
            if (__instance.tabcontroller.GetCurrentIndex() == 4)
            {
                __instance.sephirahStoryPanel.Deactivate();
                __instance.battleStoryPanel.Deactivate();
                __instance.bookStoryPanel.Deactivate();
                __instance.creatureRebattlePanel.Deactivate();
                Singleton<GlobalLogueItemCatalogPanel>.Instance.Activate();
            } else
            {
                Singleton<GlobalLogueItemCatalogPanel>.Instance.Deactivate();
            }
        }

        // Patch that initializes the Item Catalog panel; runs after the credenza is done initializing
        [HarmonyPostfix, HarmonyPatch(typeof(UIStoryArchivesPanel), nameof(UIStoryArchivesPanel.InitData))]
        static void InitItemCatalogTab_Post(UIStoryArchivesPanel __instance)
        {
            if (Singleton<GlobalLogueItemCatalogPanel>.Instance.root == null)
            {
                Singleton<GlobalLogueItemCatalogPanel>.Instance.GetLogUIObj();
                Singleton<GlobalLogueItemCatalogPanel>.Instance.Init();
            }
            
            if (__instance.tabcontroller.GetCurrentIndex() != 4)
                Singleton<GlobalLogueItemCatalogPanel>.Instance.Deactivate();

            __instance.tabcontroller.TabsRoot.transform.localPosition = new Vector3(-290f, 3.17f, 0f);
            __instance.tabcontroller.CustomTabs[0].transform.localPosition = new Vector3(325f, 35f, 0f);
            __instance.tabcontroller.CustomTabs[1].transform.localPosition = new Vector3(570f, 35f, 0f);
            __instance.tabcontroller.CustomTabs[2].transform.localPosition = new Vector3(720f, 35f, 0f);
            __instance.tabcontroller.CustomTabs[4].transform.localPosition = new Vector3(900f, 35f, 0f);
            Singleton<GlobalLogueItemCatalogPanel>.Instance.button.TabName.text = TextDataModel.GetText("ui_RMR_ItemCatalog");
        }

        // Patch that adds books into the book list
        [HarmonyPostfix, HarmonyPatch(typeof(DropBookInventoryModel), "GetBookList_invitationBookList")]
        static List<LorId> AddInvitationBooks(List<LorId> result)
        {
            result.AddRange(RMRCore.booksToAddToInventory);
            return result;
        }
        #endregion

        #region TRANSPILERS
        /// <summary>
        /// Cyam's front sprite patch, for forcing front sprites to show above everything else<br></br>
        /// Apparently less "forceful" than Hat's patch due to not messing with the layers<br></br>
        /// // CalmMagma: Also, apparently all of this is just to change ONE FUCKING BOOLEAN FROM <see langword="false"/> TO <see langword="true"/>? What the fuck?
        /// </summary>
        [HarmonyPatch(typeof(SdCharacterUtil), nameof(SdCharacterUtil.CreateSkin))]
        [HarmonyPatch(typeof(UICharacterRenderer), nameof(UICharacterRenderer.SetCharacter))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> SdCharacterUtil_CreateSkin_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilgen)
        {
            var setDataMethod = AccessTools.Method(typeof(WorkshopSkinDataSetter), nameof(WorkshopSkinDataSetter.SetData), new Type[] { typeof(WorkshopSkinData) });
            bool obtainedFlag = false;
            LocalBuilder local = null;
            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].Is(OpCodes.Callvirt, setDataMethod))
                {
                    if (!obtainedFlag)
                    {
                        int j;
                        for (j = i + 1; j < codes.Count; j++)
                        {
                            if (codes[j].Branches(out Label? _))
                            {
                                j = codes.Count;
                            }
                            else
                            {
                                if (codes[j].IsStloc())
                                {
                                    local = codes[j].operand as LocalBuilder;
                                    if (local != null && local.LocalType == typeof(bool))
                                    {
                                        obtainedFlag = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (j == codes.Count)
                        {
                            Debug.Log("Failed to obtain LateInit flag for CreateSkin");
                        }
                    }
                    else
                    {
                        codes.InsertRange(i + 1, new CodeInstruction[]
                        {
                            new CodeInstruction(OpCodes.Ldc_I4_1),
                            new CodeInstruction(OpCodes.Stloc_S, local)
                        });
                        break;
                    }
                }
            }
            return codes;
        }

        /// <summary>
        /// Patch that localizes modded BattleUnitBufs
        /// </summary>
        [HarmonyTranspiler, HarmonyPatch(typeof(BattleUnitBuf), nameof(BattleUnitBuf.Init))]
        static IEnumerable<CodeInstruction> InitBattleUnitBuf_Trans(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var x in instructions)
            {
                yield return x;
                if (x.opcode == OpCodes.Stfld)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0); // loads current instance of BattleUnitBuf onto stack
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RMR_Patches), nameof(RMR_Patches.InitBattleUnitBuf_Infix)));
                }
            }
        }
        static void InitBattleUnitBuf_Infix(BattleUnitBuf buf)
        {
            try
            {
                if (buf == null) return;
                string keyword = buf.keywordIconId == null ? buf.keywordId : buf.keywordIconId;
                if (string.IsNullOrEmpty(keyword) || !RMRCore.ClassIds.ContainsKey(buf.GetType().Assembly)) return;
                if (RMRCore.ClassIds[buf.GetType().Assembly] == RMRCore.packageId && LogLikeMod.ArtWorks.ContainsKey(keyword))
                {
                    Sprite sprite = LogLikeMod.ArtWorks[keyword];
                    if (sprite != null) buf._bufIcon = sprite;
                }
                else if (LogLikeMod.ModdedArtWorks.ContainsKey((RMRCore.ClassIds[buf.GetType().Assembly], keyword)))
                {
                    Sprite sprite = LogLikeMod.ModdedArtWorks[(RMRCore.ClassIds[buf.GetType().Assembly], keyword)];
                    if (sprite != null) buf._bufIcon = sprite;
                }
            }
            catch (Exception e)
            {
                Debug.Log("Unable to set buf icon: " + e);
            }
        }

        /// <summary>
        /// Patch to localize mystery event frames/dialogs
        /// </summary>
        [HarmonyTranspiler, HarmonyPatch(typeof(MysteryBase), nameof(MysteryBase.GetCurFrameDia))]
        static IEnumerable<CodeInstruction> GetLocalizedFrameDialog(IEnumerable<CodeInstruction> instructions)
        {

            foreach (var x in instructions)
            {
                if (x.opcode == OpCodes.Stloc_S)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0); // loads current instance of MysteryBase onto stack
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RMR_Patches), nameof(RMR_Patches.LocalizeFrameDialog_Infix)));
                    yield return new CodeInstruction(OpCodes.Stloc_0);
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                }
                yield return x;
            }
        }
        static string LocalizeFrameDialog_Infix(string text, MysteryBase mystery)
        {
            RogueMysteryXmlInfo loc = RogueMysteryXmlList.Instance.GetLocalizedMystery(mystery.xmlinfo.StageId);
            try
            {
                if (loc != null)
                {
                    text = loc.GetFrameById(mystery.curFrame.FrameID).Dialogs; 
                }
            } catch (Exception e) { Debug.Log("Failed to localize frame dialog: " + e); }
            return text;
        }

        /// <summary>
        /// Patch to localize mystery event title
        /// </summary>
        [HarmonyTranspiler, HarmonyPatch(typeof(MysteryBase), nameof(MysteryBase.GetCurFrameTitle))]
        static IEnumerable<CodeInstruction> GetLocalizedFrameTitle(IEnumerable<CodeInstruction> instructions)
        {
            yield return new CodeInstruction(OpCodes.Ldarg_0); // loads current instance of MysteryBase onto stack
            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RMR_Patches), nameof(RMR_Patches.LocalizeFrameTitle_Infix)));
            yield return new CodeInstruction(OpCodes.Ret);
        }
        static string LocalizeFrameTitle_Infix(MysteryBase mystery)
        {
            string text = "";
            RogueMysteryXmlInfo loc = RogueMysteryXmlList.Instance.GetLocalizedMystery(mystery.xmlinfo.StageId);
            try
            {
                if (loc != null)
                    text = loc.Title;
                 else
                    text = abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText(mystery.xmlinfo.Title, Array.Empty<object>());
            } catch (Exception e) { Debug.Log("Failed to localize frame title: " + e); }
            return text.ReplaceColorShorthands();
        }

        /// <summary>
        /// Patch to localize mystery event choices
        /// </summary>
        [HarmonyTranspiler, HarmonyPatch(typeof(MysteryBase), nameof(MysteryBase.SwapFrame))]
        static IEnumerable<CodeInstruction> GetLocalizedChoices(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var x in instructions)
            {
                yield return x;
                if (x.opcode == OpCodes.Call && x.Calls(typeof(abcdcode_LOGLIKE_MOD_Extension.TextDataModel).GetMethod(nameof(abcdcode_LOGLIKE_MOD_Extension.TextDataModel.GetText))))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0); // loads current instance of MysteryBase onto stack
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 5);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RMR_Patches), nameof(RMR_Patches.LocalizeChoices_Infix)));
                }
            }
        }
        static string LocalizeChoices_Infix(string text, MysteryBase mystery, List<MysteryChoiceInfo>.Enumerator enumerator)
        {
            RogueMysteryXmlInfo loc = RogueMysteryXmlList.Instance.GetLocalizedMystery(mystery.xmlinfo.StageId);
            if (loc != null)
            {
                text = loc.GetFrameById(mystery.curFrame.FrameID).GetChoiceById(enumerator.Current.ChoiceID).Desc;
            }
            return text.ReplaceColorShorthands();
        }

        /// <summary>
        /// Patch to add in keypage thumbnails automatically (if they exist)<br></br>
        /// TO USE: Simply name the skin name and the skin folder the same thing.<br></br>
        /// Then put a 256x512 image named "Thumb.png" inside the skin's respective ClothCustom folder. That's it.
        /// </summary>
        [HarmonyTranspiler, HarmonyPatch(typeof(BookModel), nameof(BookModel.GetThumbSprite))]
        static IEnumerable<CodeInstruction> BookModel_GetThumbSprite_Trans(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var x in instructions)
            {
                yield return x;
                if (x.opcode == OpCodes.Callvirt && x.Calls(typeof(ClothCustomizeData).GetMethod("get_" + nameof(ClothCustomizeData.sprite))))
                {
                    // patch occurs right after Sprite getter; stack currently has the Default sprite at the top
                    yield return new CodeInstruction(OpCodes.Ldarg_0); // loads current instance of BookModel onto stack
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RMR_Patches), nameof(BookModel_GetThumbSprite_Method)));
                }
            }
        }
        static Sprite BookModel_GetThumbSprite_Method(Sprite sprite, BookModel book)
        {
            try
            {
                Texture2D texture2D = new Texture2D(4, 4);
                string path = Path.Combine(ModContentManager.Instance.GetModPath(book.BookId.packageId), "Resource", "CharacterSkin", book._characterSkin, "ClothCustom", "Thumb.png");
                if (File.Exists(path))
                {
                    texture2D.LoadImage(File.ReadAllBytes(path));
                    sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
                }
            } catch (Exception e)
            {
                Debug.Log("Unable to set keypage thumbnail: " + e);
            }
            return sprite;
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(BookXmlInfo), nameof(BookXmlInfo.GetThumbSprite))]
        static IEnumerable<CodeInstruction> BookXmlInfo_GetThumbSprite_Trans(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var x in instructions)
            {
                yield return x;
                if (x.opcode == OpCodes.Callvirt && x.Calls(typeof(ClothCustomizeData).GetMethod("get_" + nameof(ClothCustomizeData.sprite))))
                {
                    // patch occurs right after Sprite getter; stack currently has the Default sprite at the top
                    yield return new CodeInstruction(OpCodes.Ldarg_0); // loads current instance of BookXmlInfo onto stack
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RMR_Patches), nameof(BookXmlInfo_GetThumbSprite_Method)));
                }
            }
        }
        static Sprite BookXmlInfo_GetThumbSprite_Method(Sprite sprite, BookXmlInfo book)
        {
            try
            {
                Texture2D texture2D = new Texture2D(4, 4);
                string path = Path.Combine(ModContentManager.Instance.GetModPath(book.id.packageId), "Resource", "CharacterSkin", book.GetCharacterSkin(), "ClothCustom", "Thumb.png");
                if (File.Exists(path))
                {
                    texture2D.LoadImage(File.ReadAllBytes(path));
                    sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
                }
            }
            catch (Exception e)
            {
                Debug.Log("Unable to set keypage thumbnail: " + e);
            }
            return sprite;
        }

        /// <summary>
        /// Relocalize localization extensions/classes whenever the OG mod does so
        /// </summary>
        [HarmonyTranspiler, HarmonyPatch(typeof(LogLikeMod), nameof(LogLikeMod.LoadTextData))]
        static IEnumerable<CodeInstruction> LogLikeMod_LoadTextData_Trans(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var x in instructions)
            {
                yield return x;
                if (x.Is(OpCodes.Stsfld, AccessTools.Field(typeof(abcdcode_LOGLIKE_MOD_Extension.TextDataModel), nameof(abcdcode_LOGLIKE_MOD_Extension.TextDataModel._isLoaded))))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0); // loads current language given by static method onto stack
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RMR_Patches), nameof(LogLikeMod_LoadTextData_Infix)));
                }
            }
        }
        static void LogLikeMod_LoadTextData_Infix(string language)
        {
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
        }

        /// <summary>
        /// Patch to deal with saving gamemode data
        /// </summary>
        [HarmonyTranspiler, HarmonyPatch(typeof(LoguePlayDataSaver), nameof(LoguePlayDataSaver.SavePlayData))]
        static IEnumerable<CodeInstruction> SaveGamemodeData(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var x in instructions)
            {
                if (x.opcode == OpCodes.Call && x.Calls(AccessTools.Method(typeof(LogueSaveManager), "get_Instance")))
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RoguelikeGamemodeController), "get_Instance"));
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RoguelikeGamemodeController), nameof(RoguelikeGamemodeController.SaveCurrentGamemodeName)));
                }
                yield return x;
            }
        }

        /// <summary>
        /// Patch to deal with patching gamemodes on invitation send
        /// </summary>
        [HarmonyTranspiler, HarmonyPatch(typeof(LogLikeMod), nameof(LogLikeMod.UIInvitationRightMainPanel_ConfirmSendInvitation))]
        static IEnumerable<CodeInstruction> DetectAndPatchGamemodes(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> ins = new List<CodeInstruction>();
            foreach (CodeInstruction x in instructions)
            {

                if (x.opcode == OpCodes.Newobj)
                {
                    ins.RemoveAt(ins.Count - 1);
                    ins.RemoveAt(ins.Count - 1);
                    ins.RemoveAt(ins.Count - 1);
                    ins.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    ins.Add(new CodeInstruction(OpCodes.Ldarg_2));
                    ins.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RMR_Patches), nameof(RMR_Patches.DetectAndPatchGamemodesInfix))));
                    ins.Add(new CodeInstruction(OpCodes.Ret));
                } else
                {
                    ins.Add(x);
                }
            }
            return ins;
        }
        static void DetectAndPatchGamemodesInfix(StageClassInfo inv, Action<UIInvitationRightMainPanel> orig, UIInvitationRightMainPanel self)
        {
            StagesXmlList.Instance.RestoreToDefault();
            RewardPassivesList.Instance.RestoreToDefault();
            MysteryXmlList.Instance.RestoreToDefault();
            LorId invitation = inv.id;
            bool succes = false;
            bool isContinue = invitation == new LorId(RMRCore.packageId, -855);
            if (isContinue)
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(invitation, true);
                RMRCore.CurrentGamemode.FilterContent();

                RMRCore.CurrentGamemode.BeforeInitializeGamemode();
                inv.mapInfo.Clear();
                orig(self);
                LoguePlayDataSaver.LoadPlayData();
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                Debug.Log("IT IS GOING INTO THE CONTINUE PATH! " + RMRCore.CurrentGamemode.SaveDataString);
                return;
            }
            try
            {
                RoguelikeGamemodeController.Instance.LoadGamemodeByStageRecipe(invitation, false);
                succes = true;
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
                inv.mapInfo.Clear();
                LogueBookModels.CreatePlayer();
                orig(self);
                LogueBookModels.CreatePlayerBattle();
                LoguePlayDataSaver.RemovePlayerData();
                RMRCore.CurrentGamemode.AfterInitializeGamemode();
                Debug.Log("NEW RUN! " + RMRCore.CurrentGamemode.SaveDataString);
            }
            else
                orig(self);
            Debug.Log("REGULAR RECEPTION!");
        }
    

        /// <summary>
        /// Patches RoguelikeGamemodeBase.OnWaveStartInitialEvent into the initial event passive
        /// </summary>
        [HarmonyTranspiler, HarmonyPatch(typeof(PassiveAbility_ChStart), nameof(PassiveAbility_ChStart.OnWaveStart))]
        static IEnumerable<CodeInstruction> InitialEventPatch(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeInstruction[] {
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PassiveAbilityBase), nameof(PassiveAbilityBase.OnWaveStart))),
                new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(RMRCore), nameof(RMRCore.CurrentGamemode))),
                new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(RoguelikeGamemodeBase), nameof(RoguelikeGamemodeBase.OnWaveStartInitialEvent))),
                new CodeInstruction(OpCodes.Ret)
            };
        }

        /// <summary>
        /// Disables logging depending on <see cref="RMRCore.provideAdditionalLogging"/>
        /// </summary>
        [HarmonyTranspiler, HarmonyPatch(typeof(ExtensionUtils), nameof(ExtensionUtils.Log)), HarmonyPatch(typeof(ExtensionUtils), nameof(ExtensionUtils.LogError))]
        static IEnumerable<CodeInstruction> ToggleLogging(IEnumerable<CodeInstruction> instructions)
        {
            if (RMRCore.provideAdditionalLogging)
                return instructions;
            return new CodeInstruction[] {
                new CodeInstruction(OpCodes.Ret)
            };
        }

        /// <summary>
        /// Patch to deal with checking if current reception is roguelike
        /// </summary>
        [HarmonyTranspiler, HarmonyPatch(typeof(LogLikeMod), nameof(LogLikeMod.CheckStage))]
        static IEnumerable<CodeInstruction> CheckStageGamemodePatch(IEnumerable<CodeInstruction> instructions)
        {
            bool doonce = true;
            List<CodeInstruction> ins = new List<CodeInstruction>();
            foreach (CodeInstruction x in instructions)
            {
                if (doonce && x.opcode == OpCodes.Newobj)
                {
                    doonce = false;
                    ins.RemoveAt(ins.Count - 1);
                    ins.RemoveAt(ins.Count - 1);
                    ins.Add(new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(RMRCore), nameof(RMRCore.CurrentGamemode))));
                    ins.Add(new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(RoguelikeGamemodeBase), "get_StageStart")));
                } else ins.Add(x);
            }
            return ins;
        }


        /// <summary>
        /// Patch to deal with checking if current reception is roguelike
        /// </summary>
        [HarmonyTranspiler, HarmonyPatch(typeof(LogLikeMod), nameof(LogLikeMod.SetNextStage))]
        static IEnumerable<CodeInstruction> CheckSetNextStagePatch(IEnumerable<CodeInstruction> instructions)
        {
            bool doonce1 = true;
            bool doonce2 = true;
            List<CodeInstruction> ins = new List<CodeInstruction>();
            foreach (CodeInstruction x in instructions)
            {
                if (doonce2 && x.opcode == OpCodes.Stloc_0)
                {
                    doonce2 = true;
                    ins.Add(x);
                    ins.Add(new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(RMRCore), nameof(RMRCore.CurrentGamemode))));
                    ins.Add(new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(RoguelikeGamemodeBase), "get_StageStart")));
                    ins.Add(new CodeInstruction(OpCodes.Stloc_1));
                }
                else if (doonce1 && x.opcode == OpCodes.Newobj)
                {
                    doonce1 = false;
                    ins.RemoveAt(ins.Count - 1);
                    ins.RemoveAt(ins.Count - 1);
                    ins.Add(new CodeInstruction(OpCodes.Ldloc_1));
                }
                else ins.Add(x);
            }
            return ins;
        }

        #endregion

        #region FINALIZERS

        /// <summary>
        /// Makes BookModel.SortPassive stop bitching
        /// </summary>
        [HarmonyPatch(typeof(BookModel), nameof(BookModel.SortPassive))]
        [HarmonyFinalizer]
        static Exception BookModel_SortPassive_Finalizer(Exception __exception)
        {
            return __exception is ArgumentOutOfRangeException ? null : __exception;
        }


        /// <summary>
        /// Cyam's front sprite patch- this is to prevent the game from shitting itself
        /// </summary>
        [HarmonyPatch(typeof(Workshop.WorkshopSkinDataSetter), nameof(Workshop.WorkshopSkinDataSetter.LateInit))]
        [HarmonyFinalizer]
        static Exception WorkshopSkinDataSetter_LateInit_Finalizer(Exception __exception)
        {
            return __exception is NullReferenceException ? null : __exception;
        }


        #endregion
    }
    #endregion
}
