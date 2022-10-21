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
using GameData.Domains.Character;
using System.Linq;
using GameData.Domains.Map;
using System.Text.Json;
using System.IO;
using GameData.Domains.World;
using ConchShip.EventConfig.Taiwu;

namespace MyFix1
{
    // 对应v0.0.17
    //修复鼠标移动到读书图标上时，读书策略ID越界的问题
    //修复方法：GetCurrentReadingPage不会返回大于5的数值
    [PluginConfig("MyFix1", "xyzkljl1", "1.0.2")]
    public class BackendFix1 : TaiwuRemakePlugin
    {
        public static bool On;
        static Harmony harmony;
        static Harmony harmony_delay;
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
        public static ReturnType CallPrivateStaticMethod<ReturnType>(object instance, string method_name, object[] paras)
        {
            Type type = instance.GetType();
            MethodInfo method_info = type.GetMethod(method_name, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
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
        //醉了，一万个数值bug，pageId为0的时候是总纲，取pageType低3位为总纲字决，不为0时为第一到第五页，取高5位为正逆练
        //应该只有非总纲时传入direction，但是GetReadingSpeedBonus时写反了
        //万幸的是CalcReadingSpeedSectApprovalFactor只有GetReadingSpeedBonus调用了，而且book总是当前book,因此直接获得正确的direction修改参数
        //还没修，666

        [HarmonyPrefix, HarmonyPatch(typeof(TaiwuDomain), "CalcReadingSpeedSectApprovalFactor")]
        public static void CalcReadingSpeedSectApprovalFactorPatch(TaiwuDomain __instance, sbyte orgTemplateId,ref sbyte combatSkillDirection, sbyte pageId, bool isInBattle)
        {
            if (!On)
                return ;
            if(__instance==null)
                return ;
            //获得正确的direction
            GameData.Domains.Item.SkillBook book = DomainManager.Item.GetElement_SkillBooks(__instance.GetCurReadingBook().Id);
            byte curReadingPage = (byte)(pageId + 1);//传入CalcReadingSpeedSectApprovalFactor时减1了
            //必定是功法书
            byte pageTypes = book.GetPageTypes();
            sbyte direction = SkillBookStateHelper.GetNormalPageType(pageTypes, curReadingPage);
            if (curReadingPage == 0)
                combatSkillDirection = -1;
            else
                combatSkillDirection = direction;
            return ;
        }

       
       
        public override void OnModSettingUpdate()
        {
            //游戏目录/Logs/GameData*
            DomainManager.Mod.GetSetting(ModIdStr, "BackOn", ref On);
            AdaptableLog.Info(String.Format("Load Setting, BackendFix1 {0}", On?"开启":"关闭"));
        }
    }

}
