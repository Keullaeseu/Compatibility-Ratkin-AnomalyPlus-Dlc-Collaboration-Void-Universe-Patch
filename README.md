# Compatibility Ratkin Anomaly+ and Void Universe Patch

A RimWorld compatibility patch for [Ratkin Anomaly+](https://steamcommunity.com/sharedfiles/filedetails/?id=3293914637) and [Dlc collaboration - Void universe](https://steamcommunity.com/sharedfiles/filedetails/?id=3587277884).

This mod is designed to fix the research-tab crash that happens when playing with [Ratkin Anomaly+](https://steamcommunity.com/sharedfiles/filedetails/?id=3293914637), and to give both custom research tabs (Ratkin's `RA_Tab` and Void Universe's `UV_Anomaly`) the full vanilla Anomaly-style research UI.

## Features

- Fixes the `InvalidProgram` crash in Ratkin Anomaly+ (`Background_PatchMain` transpiler on `MainTabWindow_Research.DrawRightRect`).
- Extends vanilla Anomaly research-tab handling (split Basic/Advanced background, extra view width, anomaly project panel) to the Ratkin tab (`RA_Tab`) and the Void Universe tab (`UV_Anomaly`).
- Stays safe alongside Anomalies Expected, which patches the same research methods first: its checks are detected and left as-is.

## Requirements

- RimWorld
- [Harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077)
- Anomaly DLC
- [Ratkin Anomaly+](https://steamcommunity.com/sharedfiles/filedetails/?id=3293914637)
- [Dlc collaboration - Void universe](https://steamcommunity.com/sharedfiles/filedetails/?id=3587277884)

## Installation

### Steam Workshop

Subscribe to the required mods and add them to your RimWorld mod list in the following order:

1. Harmony
2. Core
3. Royalty, Ideology, Biotech, and Anomaly
4. [Ratkin Anomaly+](https://steamcommunity.com/sharedfiles/filedetails/?id=3293914637)
5. [Dlc collaboration - Void universe](https://steamcommunity.com/sharedfiles/filedetails/?id=3587277884)
6. [Compatibility Ratkin Anomaly+ and Void Universe Patch](https://github.com/Keullaeseu/Compatibility-Ratkin-AnomalyPlus-Dlc-Collaboration-Void-Universe-Patch/releases/latest)

The patch should load after both [Ratkin Anomaly+](https://steamcommunity.com/sharedfiles/filedetails/?id=3293914637) and [Dlc collaboration - Void universe](https://steamcommunity.com/sharedfiles/filedetails/?id=3587277884).

### Manual Installation

1. Download the latest release from the [**Releases**](https://github.com/Keullaeseu/Compatibility-Ratkin-AnomalyPlus-Dlc-Collaboration-Void-Universe-Patch/releases/latest) section.
2. Extract the mod folder into your RimWorld `Mods` directory.
3. Enable the required mods in RimWorld.
4. Use the recommended load order listed above.

## Compatibility

This patch is intended to provide compatibility between [Ratkin Anomaly+](https://steamcommunity.com/sharedfiles/filedetails/?id=3293914637) and [Dlc collaboration - Void universe](https://steamcommunity.com/sharedfiles/filedetails/?id=3587277884).

It does not replace:

- [Ratkin Anomaly+](https://steamcommunity.com/sharedfiles/filedetails/?id=3293914637)
- [Dlc collaboration - Void universe](https://steamcommunity.com/sharedfiles/filedetails/?id=3587277884)

## Known Limitations

- Compatibility may be affected by future RimWorld updates.
- Compatibility may be affected by future updates to Ratkin Anomaly+ or Dlc collaboration - Void universe.

## Credits

- [Ratkin Anomaly+ on Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3293914637)
- [Dlc collaboration - Void universe on Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3587277884)
- [Compatibility Ratkin Anomaly+ and Void Universe Patch](https://github.com/Keullaeseu)
