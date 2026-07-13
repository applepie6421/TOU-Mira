namespace TownOfUs.Modifiers.Neutral;

public sealed class PestilenceUnstoppableModifier() : InvulnerabilityModifier(true, true, false)
{
    public override string ModifierName => "Pestilence Unstoppable";
}
