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
using System.Reflection;
using GameData.Domains.Item;
using System.Reflection.Emit;
using GameData.Domains.Building;

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
        public static ReturnType CallPrivateMethod<ReturnType>(object instance, string method_name, object[] paras)
        {
            Type type = instance.GetType();
            MethodInfo method_info = type.GetMethod(method_name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var para_infos = method_info.GetParameters();
            if (paras.Length != paras.Length)
            {
                AdaptableLog.Info($":{method_name}");
                return (ReturnType)new object();
            }
            for (int i = 0; i < para_infos.Length; i++)
                if (para_infos[i].ParameterType != paras[i].GetType())
                {
                    AdaptableLog.Info($":{method_name} {para_infos[i].Name}");
                    return (ReturnType)new object();
                }
            return (ReturnType)method_info.Invoke(instance, paras);
        }

        //修复练功房无效bug
        //采取一个偷懒的方法，由于CalcReferenceBooksBonusSpeedPercent没有其它调用，并且buildingBonusSpeed + refBookBonusSpeed是相加
        //直接把练功房效果加在CalcReferenceBooksBonusSpeedPercent的返回值上
        [HarmonyPostfix, HarmonyPatch(typeof(TaiwuDomain), "CalcReferenceBooksBonusSpeedPercent")]
        public static void CalcReferenceBooksBonusSpeedPercentsPatch(TaiwuDomain __instance,ref int __result,SkillBook book)
        {
            if (!On)
                return;
            if(book.IsCombatSkillBook())
            {
                SpecifyBuildingEffect buildingEffect = DomainManager.Building.GetSpecifyBuildingEffect(__instance.GetTaiwu().GetLocation());
                if(buildingEffect != null)
                    __result+= buildingEffect.AddReadingCombatSkillBookEfficiency;//和技艺书不一样，功法是无视类型的加成
            }
        }
        //修复读书效率显示
        //弱智官方5、6不分写反了
        [HarmonyPostfix, HarmonyPatch(typeof(TaiwuDomain), "GetCurrReadingEfficiency")]
        public static void GetCurrReadingEfficiencyPatch(TaiwuDomain __instance,ref short __result, DataContext context)
        {
            if (!On)
                return ;
            var _readingBooks = GetPrivateValue<Dictionary<ItemKey, ReadingBookStrategies>>(__instance, "_readingBooks");
            var _curReadingBook=__instance.GetCurReadingBook();
            ReadingBookStrategies strategies = _readingBooks[_curReadingBook];
            GameData.Domains.Item.SkillBook book = DomainManager.Item.GetElement_SkillBooks(_curReadingBook.Id);
            short skillTemplateId = -1;
            byte readingPage = 0;
            if (book.IsCombatSkillBook())
            {
                skillTemplateId = book.GetCombatSkillTemplateId();
                TaiwuCombatSkill combatSkill = CallPrivateMethod<TaiwuCombatSkill>(__instance, "GetTaiwuCombatSkill", new object[] { skillTemplateId });
                readingPage = __instance.GetCurrentReadingPage(book, strategies, combatSkill);
                if (readingPage == 5)
                {
                    int readingSpeed = __instance.GetBaseReadingSpeed(readingPage) * __instance.GetReadingSpeedBonus(readingPage) / 100;
                    __result = (short)readingSpeed;
                }
            }
            else
            {
                skillTemplateId = book.GetLifeSkillTemplateId();
                TaiwuLifeSkill lifeSkill = CallPrivateMethod<TaiwuLifeSkill>(__instance, "GetTaiwuLifeSkill", new object[] { skillTemplateId });
                readingPage = __instance.GetCurrentReadingPage(book, strategies, lifeSkill);
                if (readingPage >= 5)
                {
                    __result = 0;
                }
            }
            return;
        }
        public override void OnModSettingUpdate()
        {
            //游戏目录/Logs/GameData*
            DomainManager.Mod.GetSetting(ModIdStr, "On", ref On);
            AdaptableLog.Info(String.Format("Load Setting, BackendFix1 {0}", On?"开启":"关闭"));
        }
    }
}
