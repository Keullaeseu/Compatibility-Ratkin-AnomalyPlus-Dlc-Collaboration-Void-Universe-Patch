using System.Reflection;
using HarmonyLib;
using Verse;

namespace CompatibilityRatkinAnomalyPlusDlcCollaborationVoidUniversePatch.Source;

/// <summary>
///     Disarms Ratkin Anomaly+'s fragile research transpilers before they can run.
///     Must execute before <c>RatkinAnomaly.HarmonyInit</c>'s static constructor, which is
///     why this lives in the mod constructor and not in <see cref="CompatibilityStartup" />:
///     mod constructors run before any <c>[StaticConstructorOnStartup]</c> type.
///     Each targeted Ratkin transpiler is replaced with a pass-through, so Ratkin's
///     own <c>PatchAll</c> succeeds on any IL shape and no InvalidProgram is reported.
///     The equivalent, robust handling is applied later by
///     <see cref="MainTabWindowResearchPatches" />.
/// </summary>
public static class RatkinTranspilerNeutralizer
{
    private static readonly string[] RatkinTranspilerTypeNames =
    {
        "RatkinAnomaly.Background_PatchMain",
        "RatkinAnomaly.Background_PatchSub",
        "RatkinAnomaly.Background_PatchProject",
        "RatkinAnomaly.Background_PatchProjectSub"
    };

    public static void Neutralize()
    {
        var harmony = new Harmony(CompatibilityStartup.HarmonyId);
        var neutralizerPrefix = new HarmonyMethod(
            typeof(RatkinTranspilerNeutralizer), nameof(NeutralizeTranspilerPrefix));

        var neutralizedCount = 0;
        foreach (var typeName in RatkinTranspilerTypeNames)
        {
            var transpilerType = AccessTools.TypeByName(typeName);
            if (transpilerType == null)
                continue;

            MethodBase transpiler = AccessTools.Method(transpilerType, "Transpiler");
            if (transpiler == null)
                continue;

            harmony.Patch(transpiler, neutralizerPrefix);
            neutralizedCount++;
        }

        if (neutralizedCount == 0)
            Log.Warning("[compat] Ratkin research transpilers not found; skipping neutralization.");
        else
            Log.Message($"[compat] Neutralized {neutralizedCount} Ratkin research transpiler(s).");
    }

    /// <summary>
    ///     Pass-through for a Harmony transpiler delegate. Returning false skips Ratkin's
    ///     original transpiler and keeps the incoming instructions unchanged.
    ///     The <c>__result</c> parameter name is mandated by Harmony injection.
    /// </summary>
    public static bool NeutralizeTranspilerPrefix(
        ref IEnumerable<CodeInstruction> __result, IEnumerable<CodeInstruction> instructions)
    {
        __result = instructions;
        return false;
    }
}