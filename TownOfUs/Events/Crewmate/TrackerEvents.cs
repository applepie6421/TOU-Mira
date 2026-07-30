using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Roles;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Events.Crewmate;

public static class TrackerEvents
{
    public static int ActiveTrackTaskCount;

    [RegisterEvent]
    public static void RoundStartEventHandler(RoundStartEvent @event)
    {
        if (!@event.TriggeredByIntro)
        {
            return;
        }

        ActiveTrackTaskCount = 0;

        var trackButton = CustomButtonSingleton<SonarTrackButton>.Instance;
        trackButton.ExtraUses = 0;
        trackButton.SetUses((int)OptionGroupSingleton<SonarOptions>.Instance.MaxTracks);
        if (!trackButton.LimitedUses)
        {
            trackButton.Button?.usesRemainingText.gameObject.SetActive(false);
            trackButton.Button?.usesRemainingSprite.gameObject.SetActive(false);
        }
        else
        {
            trackButton.Button?.usesRemainingText.gameObject.SetActive(true);
            trackButton.Button?.usesRemainingSprite.gameObject.SetActive(true);
        }
    }

    [RegisterEvent]
    public static void CompleteTaskEvent(CompleteTaskEvent @event)
    {
        var opt = OptionGroupSingleton<SonarOptions>.Instance;
        var button = CustomButtonSingleton<SonarTrackButton>.Instance;
        if (@event.Player.AmOwner)
        {
            ++ActiveTrackTaskCount;
            if (@event.Player.Data.Role is not SonarRole || opt.ResetOnNewRound)
            {
                return;
            }

            if (button.LimitedUses &&
                opt.TracksPerTasks != 0 && opt.TracksPerTasks <= ActiveTrackTaskCount)
            {
                ++button.UsesLeft;
                ++button.ExtraUses;
                button.SetUses(button.UsesLeft);
                ActiveTrackTaskCount = 0;
            }
        }
    }

    [RegisterEvent]
    public static void StartMeetingEventEventHandler(StartMeetingEvent _)
    {
        if (!OptionGroupSingleton<SonarOptions>.Instance.ResetOnNewRound)
        {
            return;
        }

        foreach (var tracker in CustomRoleUtils.GetActiveRolesOfType<SonarRole>())
        {
            tracker.Clear();
        }

        var button = CustomButtonSingleton<SonarTrackButton>.Instance;
        button.SetUses((int)OptionGroupSingleton<SonarOptions>.Instance.MaxTracks);
    }
}