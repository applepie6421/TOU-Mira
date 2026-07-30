using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class VeteranOptions : AbstractRoleOptionGroup<VeteranRole>
{
    public override string GroupName => TouLocale.Get("TouRoleVeteran", "Veteran");

    [ModdedNumberOption("TouOptionVeteranAlertCooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float AlertCooldown { get; set; } = 25f;

    [ModdedNumberOption("TouOptionVeteranAlertDuration", 5f, 15f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float AlertDuration { get; set; } = 10f;

    public ModdedNumberOption MaxNumAlerts { get; } = new("TouOptionVeteranMaxNumberofAlerts", 5f, -1f, 15f, 1f, "0", "∞", MiraNumberSuffixes.None, "0");

    public ModdedNumberOption AlertsPerTasks { get; } = new("TouOptionVeteranAlertsPerTasks", 3f, 0f, 15f, 1f, "Off", "#", MiraNumberSuffixes.None, "0")
    {
        Visible = () => OptionGroupSingleton<VeteranOptions>.Instance.MaxNumAlerts != -1
    };

    [ModdedToggleOption("TouOptionVeteranCanBeKilledOnAlert")]
    public bool KilledOnAlert { get; set; } = false;

    public ModdedToggleOption KnowWhenAttackedInMeeting { get; } = new("TouOptionVeteranKnowWhenAttackedInMeeting", true)
    {
        Visible = () =>
            !OptionGroupSingleton<VeteranOptions>.Instance.KilledOnAlert
    };
}