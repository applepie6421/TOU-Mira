using MiraAPI.Utilities;
using Reactor.Utilities;
using UnityEngine;

namespace TownOfUs.Modifiers.Neutral;

public sealed class AmnesiacArrowModifier(DeadBody deadBody, Color color) : ArrowDeadBodyModifier(deadBody, color, 0)
{
    public override string ModifierName => $"Amnesiac  Arrow";

    public override void OnActivate()
    {
        base.OnActivate();

        Coroutines.Start(MiscUtils.CoFlash(color));
    }
}