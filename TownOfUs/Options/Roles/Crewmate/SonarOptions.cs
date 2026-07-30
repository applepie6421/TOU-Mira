using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class SonarOptions : AbstractRoleOptionGroup<SonarRole>
{
    public override string GroupName => TouLocale.Get("TouRoleSonar", "Sonar");

    [ModdedNumberOption("TouOptionSonarTrackCooldown", 1f, 30f, 1f, MiraNumberSuffixes.Seconds)]
    public float TrackCooldown { get; set; } = 20f;

    public ModdedNumberOption MaxTracks { get; } = new("TouOptionSonarMaxNumberOfTracks", 5f, -1f, 15f, 1f, "0", "∞", MiraNumberSuffixes.None, "0");

    [ModdedNumberOption("TouOptionSonarArrowUpdateInterval", 0f, 15f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float UpdateInterval { get; set; } = 5f;

    [ModdedToggleOption("TouOptionSonarArrowsMakeSoundOnDeath")]
    public bool SoundOnDeactivate { get; set; } = true;

    [ModdedToggleOption("TouOptionSonarArrowsResetAfterEachRound")]
    public bool ResetOnNewRound { get; set; } = true;

    public ModdedNumberOption TracksPerTasks { get; } = new("TouOptionSonarTracksPerTasks", 0f, 0f, 15f, 1f, "Off", "#", MiraNumberSuffixes.None, "0")
    {
        Visible = () => OptionGroupSingleton<SonarOptions>.Instance.MaxTracks != -1 &&
                        !OptionGroupSingleton<SonarOptions>.Instance.ResetOnNewRound
    };
}