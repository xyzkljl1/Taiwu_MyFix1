using TaiwuModdingLib.Core.Plugin;
using HarmonyLib;
using GameData.Domains;
using System.Collections.Generic;
using GameData.Domains.Taiwu;
using GameData.Domains.CombatSkill;
using GameData.Common;
using System;
using GameData.Utilities;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace MyFix1
{
    // 对应v0.0.17
    //修复鼠标移动到读书图标上时，读书策略ID越界的问题
    //修复方法：GetCurrentReadingPage不会返回大于5的数值
    [PluginConfig("MyFix1", "xyzkljl1", "1.0.2")]
    public class BackendFix1 : TaiwuRemakePlugin
    {
        public static bool On;
        Harmony harmony;
        public override void Dispose()
        {
            if(harmony != null)
                harmony.UnpatchSelf();

        }
        public override void Initialize()
        {
            harmony = Harmony.CreateAndPatchAll(typeof(BackendFix1));
        }
        public override void OnModSettingUpdate()
        {
            //游戏目录/Logs/GameData*
            DomainManager.Mod.GetSetting(ModIdStr, "BackendFix1On", ref On);
            AdaptableLog.Info(String.Format("Load Setting, BackendFix1 {0}", On?"开启":"关闭"));
        }
    }
}
