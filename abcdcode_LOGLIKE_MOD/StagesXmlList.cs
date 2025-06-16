// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.StagesXmlList
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using System.Linq;


namespace abcdcode_LOGLIKE_MOD
{

    public class StagesXmlList : Singleton<StagesXmlList>
    {
        public List<StagesXmlInfo> infos;
        private StagesXmlInfo[] _allInfos;

        public void Init(List<StagesXmlInfo> info)
        {
            this.infos = info;
            foreach (StagesXmlInfo stagesXmlInfo in info)
            {
                foreach (LogueStageInfo stage in stagesXmlInfo.Stages)
                    LogLikeMod.RegisterPickUpXml(stage);
            }
            this._allInfos = new StagesXmlInfo[info.Count];
            info.CopyTo(this._allInfos);
        }

        public List<LogueStageInfo> GetChapterData(ChapterGrade chapter, bool ExceptAllGrade = false)
        {
            List<LogueStageInfo> chapterData = new List<LogueStageInfo>();
            foreach (StagesXmlInfo stagesXmlInfo in this.infos.FindAll((Predicate<StagesXmlInfo>)(x =>
            {
                if (x.chapter == chapter)
                    return true;
                return !ExceptAllGrade && x.chapter == ChapterGrade.GradeAll;
            })))
                chapterData.AddRange((IEnumerable<LogueStageInfo>)stagesXmlInfo.Stages);
            return chapterData;
        }

        public LogueStageInfo GetStageInfo(LorId stageid)
        {
            foreach (StagesXmlInfo info in this.infos)
            {
                foreach (LogueStageInfo stage in info.Stages)
                {
                    if (stage.Id == stageid)
                        return stage.Copy();
                }
            }
            return (LogueStageInfo)null;
        }

        public void RestoreToDefault()
        {
            this.infos.Clear();
            this.infos.AddRange((IEnumerable<StagesXmlInfo>)((IEnumerable<StagesXmlInfo>)this._allInfos).ToList<StagesXmlInfo>());
        }
    }
}
