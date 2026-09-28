using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace CompatibilityRatkinAnomalyPlusDlcCollaborationVoidUniversePatch.Source;

/// <summary>
///     Second stage of the compatibility setup. Must load after Ratkin Anomaly+ and
///     Void Universe (see About.xml loadAfter). Removes any Ratkin research
///     transpiler that still got applied, then applies the pattern-based
///     replacements in <see cref="MainTabWindowResearchPatches" />.
///     The first stage runs earlier from <see cref="CompatMod" /> and neutralizes
///     Ratkin's transpilers before Ratkin's own PatchAll can throw InvalidProgram.
/// </summary>
[StaticConstructorOnStartup]
public static class CompatibilityStartup
{
    public const string HarmonyId = "keullaeseu.compat.ratkinanomalyplus.voiduniverse";

    // Ratkin Anomaly+ Harmony ID from its HarmonyInit ("fxz.ratkinanomaly").
    private const string RatkinHarmonyId = "fxz.ratkinanomaly";

    static CompatibilityStartup()
    {
        try
        {
            var harmony = new Harmony(HarmonyId);
            UnpatchRatkinTranspilers(harmony);
            harmony.PatchAll();
            Log.Message("[Compat] Ratkin Anomaly+ / Void Universe research compatibility applied.");
        }
        catch (Exception exception)
        {
            Log.Error($"[compat] Failed to apply research compatibility: {exception}");
        }
    }

    private static void UnpatchRatkinTranspilers(Harmony harmony)
    {
        MethodBase drawRightRect = AccessTools.Method(
            typeof(MainTabWindow_Research), "DrawRightRect", new[] { typeof(Rect), typeof(float) });
        MethodBase viewSize = AccessTools.Method(
            typeof(MainTabWindow_Research), "ViewSize", new[] { typeof(ResearchTabDef) });
        MethodBase drawProjectInfo = AccessTools.Method(
            typeof(MainTabWindow_Research), "DrawProjectInfo", new[] { typeof(Rect) });

        foreach (var original in new[] { drawRightRect, viewSize, drawProjectInfo })
        {
            if (original == null)
            {
                Log.Error("[Compat] Could not find a MainTabWindow_Research method to unpatch (game update?).");
                continue;
            }

            try
            {
                // Removes Ratkin's transpilers only; no-op when they failed to apply.
                harmony.Unpatch(original, HarmonyPatchType.Transpiler, RatkinHarmonyId);
            }
            catch (Exception exception)
            {
                Log.Error($"[compat] Unpatch failed for {original.Name}: {exception.Message}");
            }
        }
    }
}