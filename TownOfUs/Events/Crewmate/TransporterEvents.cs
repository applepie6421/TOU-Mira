using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.Events.Vanilla.Usables;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Events.Crewmate;

public static class TransporterEvents
{
    public static int ActiveTransportTaskCount;

    [RegisterEvent]
    public static void RoundStartEventHandler(RoundStartEvent @event)
    {
        if (!@event.TriggeredByIntro)
        {
            return;
        }

        ActiveTransportTaskCount = 0;

        var transportButton = CustomButtonSingleton<TransporterTransportButton>.Instance;
        transportButton.ExtraUses = 0;
        transportButton.SetUses((int)OptionGroupSingleton<TransporterOptions>.Instance.MaxNumTransports);
        if (!transportButton.LimitedUses)
        {
            transportButton.Button?.usesRemainingText.gameObject.SetActive(false);
            transportButton.Button?.usesRemainingSprite.gameObject.SetActive(false);
        }
        else
        {
            transportButton.Button?.usesRemainingText.gameObject.SetActive(true);
            transportButton.Button?.usesRemainingSprite.gameObject.SetActive(true);
        }
    }

    [RegisterEvent]
    public static void CompleteTaskEvent(CompleteTaskEvent @event)
    {
        var opt = OptionGroupSingleton<TransporterOptions>.Instance;
        var button = CustomButtonSingleton<TransporterTransportButton>.Instance;
        if (@event.Player.AmOwner)
        {
            ++ActiveTransportTaskCount;
            if (@event.Player.Data.Role is not TransporterRole)
            {
                return;
            }

            if (button.LimitedUses &&
                opt.TransportsPerTasks != 0 && opt.TransportsPerTasks <= ActiveTransportTaskCount)
            {
                ++button.UsesLeft;
                ++button.ExtraUses;
                button.SetUses(button.UsesLeft);
                ActiveTransportTaskCount = 0;
            }
        }
    }

    [RegisterEvent]
    public static void PlayerCanUseEventHandler(PlayerCanUseEvent @event)
    {
        if (OptionGroupSingleton<TransporterOptions>.Instance.CanUseVitals)
        {
            return;
        }

        if (!PlayerControl.LocalPlayer ||
            !PlayerControl.LocalPlayer.Data ||
            PlayerControl.LocalPlayer.Data.Role is not TransporterRole)
        {
            return;
        }

        var console = @event.Usable.TryCast<SystemConsole>();

        if (console == null)
            // Not a SystemConsole, return
        {
            return;
        }

        if (console.MinigamePrefab.TryCast<VitalsMinigame>())
        {
            @event.Cancel();
        }
    }
}