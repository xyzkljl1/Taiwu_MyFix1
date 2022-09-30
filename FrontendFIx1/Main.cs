using TaiwuModdingLib.Core.Plugin;
using HarmonyLib;
using GameData.Domains.SpecialEffect;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using GameData.Domains;
using System.Reflection;
using UICommon.Character;
using CharacterDataMonitor;
using Config;
using System.IO;
using UICommon.Character.Elements;
using UnityEngine;

namespace EffectInfo
{
    [PluginConfig("Myfix", "xyzkljl1", "1.0.2")]
    public class FrontendFix1 : TaiwuRemakePlugin
    {
        public static bool On = true;
        Harmony harmony;
        public override void Dispose()
        {
            if (harmony != null)
                harmony.UnpatchSelf();
        }
        public override void Initialize()
        {
            harmony = Harmony.CreateAndPatchAll(typeof(FrontendFix1));
        }
        public override void OnModSettingUpdate()
        {
            ModManager.GetSetting(ModIdStr, "FrontendFix1On", ref On);
        }
        /*
         * 进行了多次Add而没有对应的remove，导致对象销毁后还有一些事件触发了空对象的调用
         * 会创建或删除事件的函数只有SetVisible和OnDisable,改成clear
         */

        [HarmonyPostfix, HarmonyPatch(typeof(CharacterAttributeDataView),
              "SetVisible")]
        public static void SetVisiblePatch()
        {
            if(On)
                GEvent.ClearEvent(UiEvents.OnEatItemSend);
        }
        [HarmonyPostfix, HarmonyPatch(typeof(CharacterAttributeDataView),"OnDisable")]
        public static void OnDisablePatch()
        {
            if (On)
                GEvent.ClearEvent(UiEvents.OnEatItemSend);
        }
    }
}
