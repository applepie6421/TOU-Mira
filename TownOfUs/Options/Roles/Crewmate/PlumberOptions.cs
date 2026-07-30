using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Options.Roles.Crewmate;

public sealed class PlumberOptions : AbstractRoleOptionGroup<PlumberRole>
{
    public override string GroupName => TouLocale.Get("TouRolePlumber", "Plumber");

    [ModdedNumberOption("TouOptionPlumberFlushCooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds, "0.0")]
    public float FlushCooldown { get; set; } = 25f;

    [ModdedNumberOption("TouOptionPlumberFlushDuration", 1f, 20f, 1f, MiraNumberSuffixes.Seconds, "0.0")]
    public float FlushDuration { get; set; } = 3f;

    [ModdedNumberOption("TouOptionPlumberBlockCooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds, "0.0")]
    public float BlockCooldown { get; set; } = 25f;

    public ModdedNumberOption MaxBarricades { get; } = new("TouOptionPlumberMaxNumberOfBarricades", 3f, -1f, 15f, 1f, "0", "∞", MiraNumberSuffixes.None, "0");

    [ModdedNumberOption("TouOptionPlumberAmountOfRoundsBarricadesLast", 0f, 15f, 1f, MiraNumberSuffixes.None, "0", true)]
    public float BarricadeRoundDuration { get; set; } = 2f;

    public ModdedNumberOption BarricadesPerTasks { get; } = new("TouOptionPlumberBarricadesPerTasks", 2f, 0f, 15f, 1f, "Off", "#", MiraNumberSuffixes.None, "0")
    {
        Visible = () => OptionGroupSingleton<PlumberOptions>.Instance.MaxBarricades != -1
    };
}