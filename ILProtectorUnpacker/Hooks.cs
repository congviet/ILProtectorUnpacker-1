﻿using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace ILProtectorUnpacker
{
    public class Hooks
    {
        private static readonly Harmony Harmony = new Harmony("(Washo/Watasho)(1337)");
        public static MethodBase MethodBase;

        public static void ApplyHook()
        {
            var runtimeType = typeof(Delegate).Assembly.GetType("System.RuntimeType");
            var getMethod = runtimeType.GetMethods((BindingFlags) (-1)).First(m =>
                m.Name == "GetMethodBase" && m.GetParameters().Length == 2 &&
                m.GetParameters()[0].ParameterType == runtimeType &&
                m.GetParameters()[1].ParameterType.Name == "IRuntimeMethodInfo");
            Harmony.Patch(getMethod, null, new HarmonyMethod(typeof(Hooks).GetMethod("Postfix")));
        }

        public static void Postfix(ref MethodBase __result)
        {
            if (__result.Name == "InvokeMethod")
                __result = MethodBase;
        }
    }
}