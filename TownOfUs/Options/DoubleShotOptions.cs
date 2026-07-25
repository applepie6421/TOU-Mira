using MiraAPI.GameOptions;
using MiraAPI.GameOptions.OptionTypes;
using TownOfUs.Modifiers.Game;
using UnityEngine;

namespace TownOfUs.Options;

public sealed class DoubleShotOptions : AbstractTouModifierOptionGroup<DoubleShotModifier>
{
    public override string GroupName => TouLocale.Get("TouModifierDoubleShot", "Double Shot");
    public override Color GroupColor => TownOfUsColors.DoubleShot;
    public override uint GroupPriority => 8;
    public override Func<bool> GroupVisible => () => OptionGroupSingleton<RoleOptions>.Instance.IsClassicRoleAssignment;

    public ModdedToggleOption PreventGuessingAfterMisguess { get; } =
        new("Temporarily Prevent Guessing After A Misguess", false);
}
