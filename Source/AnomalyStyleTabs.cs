using RimWorld;

namespace CompatibilityRatkinAnomalyPlusDlcCollaborationVoidUniversePatch.Source;

/// <summary>
///     Identifies research tabs that use the vanilla Anomaly-style research UI
///     (split Basic/Advanced background, extra view width, anomaly project panel).
///     Compared by defName so no hard reference to the other mods' assemblies is
///     needed at runtime and missing tabs simply never match.
/// </summary>
public static class AnomalyStyleTabs
{
    public const string VanillaAnomalyTabDefName = "Anomaly";
    public const string RatkinTabDefName = "RA_Tab";
    public const string VoidTabDefName = "UV_Anomaly";

    public static bool IsAnomalyStyleTab(ResearchTabDef tab)
    {
        if (tab == null)
            return false;
        var defName = tab.defName;
        return defName == VanillaAnomalyTabDefName
               || defName == RatkinTabDefName
               || defName == VoidTabDefName;
    }
}