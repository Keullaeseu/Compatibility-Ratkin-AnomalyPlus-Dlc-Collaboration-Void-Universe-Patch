using Verse;

namespace CompatibilityRatkinAnomalyPlusDlcCollaborationVoidUniversePatch.Source;

/// <summary>
///     Mod entry point. The constructor runs before any
///     <c>[StaticConstructorOnStartup]</c> type (including Ratkin's
///     <c>HarmonyInit</c>), so Ratkin's broken transpilers are neutralized here,
///     while the actual replacement patches are applied later by
///     <see cref="CompatibilityStartup" />.
/// </summary>
public class CompatMod : Mod
{
    public CompatMod(ModContentPack contentPack)
        : base(contentPack)
    {
        RatkinTranspilerNeutralizer.Neutralize();
    }
}