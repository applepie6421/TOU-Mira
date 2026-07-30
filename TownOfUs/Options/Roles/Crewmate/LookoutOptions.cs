using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class LookoutOptions : AbstractRoleOptionGroup<LookoutRole>
{
    public override string GroupName => TouLocale.Get("TouRoleLookout", "Lookout");

    [ModdedNumberOption("TouOptionLookoutWatchCooldown", 1f, 30f, 1f, MiraNumberSuffixes.Seconds)]
    public float WatchCooldown { get; set; } = 20f;

    public ModdedEnumOption WatchType { get; } = new("Watched Player Feedback Reveals", (int)LookoutView.Players, typeof(LookoutView));

    public ModdedNumberOption MaxWatches { get; } = new("TouOptionLookoutMaxWatches", 5f, -1f, 15f, 1f, "0", "∞", MiraNumberSuffixes.None, "0");

    public ModdedToggleOption LookoutSeesIndirectAttacks { get; } = new("TouOptionLookoutSeesIndirectAttacks", false);

    [ModdedToggleOption("TouOptionLookoutLoResetOnNewRound")]
    public bool LoResetOnNewRound { get; set; } = true;

    public ModdedNumberOption WatchesPerTasks { get; } = new("TouOptionLookoutWatchesPerTasks", 0f, 0f, 15f, 1f, "Off", "#", MiraNumberSuffixes.None, "0")
    {
        Visible = () => OptionGroupSingleton<LookoutOptions>.Instance.MaxWatches != -1 &&
                        !OptionGroupSingleton<LookoutOptions>.Instance.LoResetOnNewRound
    };
}

public enum LookoutView
{
    Roles,
    Players
}