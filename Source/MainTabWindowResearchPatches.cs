using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace CompatibilityRatkinAnomalyPlusDlcCollaborationVoidUniversePatch.Source;

/// <summary>
///     Robust replacements for Ratkin Anomaly+'s index-based transpilers on
///     <see cref="MainTabWindow_Research" /> (Background_PatchMain/Sub/Project/ProjectSub).
///     Ratkin searched the IL by hardcoded offsets (e.g. list[index + 33]) and
///     rewrote single-pop branches as double-pop short branches, which breaks with
///     InvalidProgram as soon as the target method differs from vanilla. In practice
///     this happens when Anomalies Expected loads first: its MTWR_ViewSize_Transpiler
///     (Harmony ID rimworld.mrhydralisk.AnomaliesExpectedPatch) already rewrites the
///     vanilla "tab == Anomaly" comparison into a call to its CurrentResearchTabAnomaly
///     helper (true for Anomaly and for every tab with minMonolithLevelVisible greater
///     than zero, which covers both RA_Tab and UV_Anomaly). Ratkin then replaces that
///     single-pop branch with a double-pop branch over a one-value stack.
///     These replacements instead find the vanilla comparison by pattern
///     (ldsfld ResearchTabDefOf::Anomaly followed by beq/bne) and widen it
///     to Anomaly + RA_Tab + UV_Anomaly, reusing the existing branch target and
///     long-form branches so offsets can never overflow. When another mod already
///     owns the check, the instructions are returned unchanged.
/// </summary>
public static class MainTabWindowResearchPatches
{
    internal static List<CodeInstruction> GeneralizeAnomalyChecks(
        IEnumerable<CodeInstruction> instructions, string methodName, int expectedCount)
    {
        var codes = new List<CodeInstruction>(instructions);
        var tabCheckMethod = AccessTools.Method(
            typeof(AnomalyStyleTabs), nameof(AnomalyStyleTabs.IsAnomalyStyleTab));

        var generalizedCount = 0;
        for (var i = 0; i < codes.Count - 1; i++)
        {
            if (!IsAnomalyTabLoad(codes[i]))
                continue;

            var branch = codes[i + 1].opcode;
            if (branch == OpCodes.Beq || branch == OpCodes.Beq_S)
            {
                // (tab, Anomaly) beq TARGET  ->  (tab) call IsAnomalyStyleTab brtrue TARGET
                codes[i].opcode = OpCodes.Call;
                codes[i].operand = tabCheckMethod;
                codes[i + 1].opcode = OpCodes.Brtrue;
                generalizedCount++;
            }
            else if (branch == OpCodes.Bne_Un || branch == OpCodes.Bne_Un_S)
            {
                // (tab, Anomaly) bne TARGET  ->  (tab) call IsAnomalyStyleTab brfalse TARGET
                codes[i].opcode = OpCodes.Call;
                codes[i].operand = tabCheckMethod;
                codes[i + 1].opcode = OpCodes.Brfalse;
                generalizedCount++;
            }
            // Any other use of the Anomaly field is left untouched.
        }

        if (generalizedCount == expectedCount)
            Log.Message(
                $"[compat] {methodName}: generalized {generalizedCount} Anomaly check(s) to RA_Tab + UV_Anomaly.");
        else if (generalizedCount == 0 && UsesAnomalyStyleHelperCall(codes))
            Log.Message($"[compat] {methodName}: Anomaly check already handled by another mod; leaving as-is.");
        else if (generalizedCount == 0)
            Log.Warning($"[compat] {methodName}: no vanilla Anomaly check found; leaving as-is.");
        else
            Log.Error(
                $"[compat] {methodName}: expected {expectedCount} Anomaly check(s), generalized {generalizedCount}. Game update?");

        return codes;
    }

    private static bool IsAnomalyTabLoad(CodeInstruction code)
    {
        return code.opcode == OpCodes.Ldsfld
               && code.operand is FieldInfo field
               && field.Name == nameof(ResearchTabDefOf.Anomaly)
               && field.FieldType == typeof(ResearchTabDef);
    }

    private static bool UsesAnomalyStyleHelperCall(List<CodeInstruction> codes)
    {
        foreach (var code in codes)
            if (code.opcode == OpCodes.Call
                && code.operand is MethodInfo calledMethod
                && calledMethod.Name == "CurrentResearchTabAnomaly")
                return true;

        return false;
    }
}

/// <summary>Background split + TeachOpportunity for RA_Tab / UV_Anomaly.</summary>
[HarmonyPatch(typeof(MainTabWindow_Research), "DrawRightRect", typeof(Rect), typeof(float))]
public static class Patch_DrawRightRect
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return MainTabWindowResearchPatches.GeneralizeAnomalyChecks(
            instructions, "DrawRightRect", 1);
    }
}

/// <summary>Extra view width (+14) for RA_Tab / UV_Anomaly.</summary>
[HarmonyPatch(typeof(MainTabWindow_Research), "ViewSize", typeof(ResearchTabDef))]
public static class Patch_ViewSize
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return MainTabWindowResearchPatches.GeneralizeAnomalyChecks(
            instructions, "ViewSize", 1);
    }
}

/// <summary>Anomaly-style project panel for RA_Tab / UV_Anomaly (both checks).</summary>
[HarmonyPatch(typeof(MainTabWindow_Research), "DrawProjectInfo", typeof(Rect))]
public static class Patch_DrawProjectInfo
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return MainTabWindowResearchPatches.GeneralizeAnomalyChecks(
            instructions, "DrawProjectInfo", 2);
    }
}