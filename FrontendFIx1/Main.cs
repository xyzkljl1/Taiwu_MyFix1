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
using GameData.Domains.Building;

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
            ModManager.GetSetting(ModIdStr, "FrontOn", ref On);
        }
        public static FieldType GetValue<FieldType>(object instance, string field_name, BindingFlags flags)
        {
            Type type = instance.GetType();
            FieldInfo field_info = type.GetField(field_name, flags);
            return (FieldType)field_info.GetValue(instance);
        }
        public static FieldType GetPrivateValue<FieldType>(object instance, string field_name)
        {
            return GetValue<FieldType>(instance, field_name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        }

    }
}
