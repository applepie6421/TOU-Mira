using HarmonyLib;
using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Events.Crewmate;

public static class LookoutEvents
{
    public static int ActiveWatchTaskCount;

    [RegisterEvent]
    public static void RoundStartEventHandler(RoundStartEvent @event)
    {
        if (!@event.TriggeredByIntro)
        {
            return;
        }

        ActiveWatchTaskCount = 0;

        var watchButton = CustomButtonSingleton<WatchButton>.Instance;
        watchButton.ExtraUses = 0;
        watchButton.SetUses((int)OptionGroupSingleton<LookoutOptions>.Instance.MaxWatches);
        if (!watchButton.LimitedUses)
        {
            watchButton.Button?.usesRemainingText.gameObject.SetActive(false);
            watchButton.Button?.usesRemainingSprite.gameObject.SetActive(false);
        }
        else
        {
            watchButton.Button?.usesRemainingText.gameObject.SetActive(true);
            watchButton.Button?.usesRemainingSprite.gameObject.SetActive(true);
        }
    }

    [RegisterEvent]
    public static void CompleteTaskEvent(CompleteTaskEvent @event)
    {
        var opt = OptionGroupSingleton<LookoutOptions>.Instance;
        var button = CustomButtonSingleton<WatchButton>.Instance;
        if (@event.Player.AmOwner)
        {
            ++ActiveWatchTaskCount;
            if (@event.Player.Data.Role is not LookoutRole || opt.LoResetOnNewRound)
            {
                return;
            }

            if (button.LimitedUses &&
                opt.WatchesPerTasks != 0 && opt.WatchesPerTasks <= ActiveWatchTaskCount)
            {
                ++button.UsesLeft;
                ++button.ExtraUses;
                button.SetUses(button.UsesLeft);
                ActiveWatchTaskCount = 0;
            }
        }
    }

    [RegisterEvent]
    public static void MiraButtonClickEventHandler(MiraButtonClickEvent @event)
    {
        // Warning("Lookout click event!");
        var button = @event.Button as CustomActionButton<PlayerControl>;
        var source = PlayerControl.LocalPlayer;
        var target = button?.Target;

        if (target == null || button == null || !button.CanClick())
        {
            return;
        }

        CheckForLookoutWatched(source, target);
    }

    [RegisterEvent]
    public static void AfterMurderEventHandler(AfterMurderEvent @event)
    {
        var victim = @event.Target;
        var source = @event.Source;

        CheckForLookoutWatched(source, victim);
    }

    [RegisterEvent]
    public static void EjectionEventEventHandler(EjectionEvent _)
    {
        if (!OptionGroupSingleton<LookoutOptions>.Instance.LoResetOnNewRound)
        {
            return;
        }

        ModifierUtils.GetPlayersWithModifier<LookoutWatchedModifier>()
            .Do(x => x.RemoveModifier<LookoutWatchedModifier>());

        var button = CustomButtonSingleton<WatchButton>.Instance;
        button.SetUses((int)OptionGroupSingleton<LookoutOptions>.Instance.MaxWatches);
    }

    public static void CheckForLookoutWatched(PlayerControl source, PlayerControl target)
    {
        if (MeetingHud.Instance || ExileController.Instance)
        {
            return;
        }

        if (!target.HasModifier<LookoutWatchedModifier>() || !(TutorialManager.InstanceExists || source.AmOwner) || source.HasModifier<IndirectAttackerModifier>() && !OptionGroupSingleton<LookoutOptions>.Instance.LookoutSeesIndirectAttacks.Value)
        {
            return;
        }

        LookoutRole.RpcSeePlayer(source, target);
    }
}