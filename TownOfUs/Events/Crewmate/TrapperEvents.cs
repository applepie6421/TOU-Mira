using HarmonyLib;
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

public static class TrapperEvents
{
    public static int ActiveTrapTaskCount;

    [RegisterEvent]
    public static void CompleteTaskEvent(CompleteTaskEvent @event)
    {
        var opt = OptionGroupSingleton<TrapperOptions>.Instance;
        var button = CustomButtonSingleton<TrapperTrapButton>.Instance;
        if (@event.Player.AmOwner)
        {
            ++ActiveTrapTaskCount;
            if (@event.Player.Data.Role is not TrapperRole || opt.TrapsRemoveOnNewRound)
            {
                return;
            }

            if (button.LimitedUses &&
                opt.TrapsPerTasks != 0 && opt.TrapsPerTasks <= ActiveTrapTaskCount)
            {
                ++button.UsesLeft;
                ++button.ExtraUses;
                button.SetUses(button.UsesLeft);
                ActiveTrapTaskCount = 0;
            }
        }
    }

    [RegisterEvent]
    public static void StartMeetingEventHandler(StartMeetingEvent _)
    {
        CustomRoleUtils.GetActiveRolesOfType<TrapperRole>().Do(x => x.Report());
    }

    [RegisterEvent]
    public static void RoundStartEventHandler(RoundStartEvent @event)
    {
        if (@event.TriggeredByIntro)
        {
            ActiveTrapTaskCount = 0;

            var trapButton = CustomButtonSingleton<TrapperTrapButton>.Instance;
            trapButton.ExtraUses = 0;
            trapButton.SetUses((int)OptionGroupSingleton<TrapperOptions>.Instance.MaxTraps);
            if (!trapButton.LimitedUses)
            {
                trapButton.Button?.usesRemainingText.gameObject.SetActive(false);
                trapButton.Button?.usesRemainingSprite.gameObject.SetActive(false);
            }
            else
            {
                trapButton.Button?.usesRemainingText.gameObject.SetActive(true);
                trapButton.Button?.usesRemainingSprite.gameObject.SetActive(true);
            }
        }

        if (OptionGroupSingleton<TrapperOptions>.Instance.TrapsRemoveOnNewRound)
        {
            CustomRoleUtils.GetActiveRolesOfType<TrapperRole>().Do(x => x.Clear());

            if (PlayerControl.LocalPlayer.Data.Role is TrapperRole)
            {
                CustomButtonSingleton<TrapperTrapButton>.Instance.SetUses((int)OptionGroupSingleton<TrapperOptions>.Instance.MaxTraps);
            }
        }
    }
}