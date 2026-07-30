using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class TransporterOptions : AbstractRoleOptionGroup<TransporterRole>
{
    public override string GroupName => TouLocale.Get("TouRoleTransporter", "Transporter");

    [ModdedNumberOption("TouOptionTransporterTransportCooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float TransporterCooldown { get; set; } = 25f;

    public ModdedNumberOption MaxNumTransports { get; } = new("TouOptionTransporterMaxUses", 5f, -1f, 15f, 1f, "0", "∞", MiraNumberSuffixes.None, "0");

    public ModdedNumberOption TransportsPerTasks { get; } = new("TouOptionTransporterTransportsPerTasks", 2f, 0f, 15f, 1f, "Off", "#", MiraNumberSuffixes.None, "0")
    {
        Visible = () => OptionGroupSingleton<TransporterOptions>.Instance.MaxNumTransports != -1
    };

    [ModdedToggleOption("TouOptionTransporterMoveWithMenu")]
    public bool MoveWithMenu { get; set; } = true;

    [ModdedToggleOption("TouOptionTransporterCanUseVitals")]
    public bool CanUseVitals { get; set; } = true;
}