using HarmonyLib;
using MiraAPI.GameOptions;
using TownOfUs.Options;

namespace TownOfUs.Patches.Options;

[HarmonyPatch]
public static class ReportRangePatch
{
    private static float _vanillaDistance;

    public static float VanillaDistance => _vanillaDistance > 0f ? _vanillaDistance : PlayerControl.LocalPlayer.MaxReportDistance;

    private static float RangeMultiplier => (ReportReach)OptionGroupSingleton<VanillaTweakOptions>.Instance.ReportRange.Value switch
    {
        ReportReach.Short => 0.35f,
        ReportReach.Medium => 0.65f,
        _ => 1f
    };

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    [HarmonyPrefix]
    public static void SetReportRange(PlayerControl __instance)
    {
        if (!__instance.AmOwner)
        {
            return;
        }

        if (_vanillaDistance <= 0f)
        {
            _vanillaDistance = __instance.MaxReportDistance;
        }

        __instance.MaxReportDistance = _vanillaDistance * RangeMultiplier;
    }
}
