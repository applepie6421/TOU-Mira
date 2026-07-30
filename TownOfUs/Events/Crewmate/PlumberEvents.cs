using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.Events.Vanilla.Usables;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Roles;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles.Crewmate;

namespace TownOfUs.Events.Crewmate;

public static class PlumberEvents
{
    public static int ActiveBarricadeTaskCount;

    [RegisterEvent]
    public static void RoundStartEventHandler(RoundStartEvent @event)
    {
        if (@event.TriggeredByIntro)
        {
            PlumberRole.ClearAll();

            ActiveBarricadeTaskCount = 0;

            var blockButton = CustomButtonSingleton<PlumberBlockButton>.Instance;
            blockButton.ExtraUses = 0;
            blockButton.SetUses((int)OptionGroupSingleton<PlumberOptions>.Instance.MaxBarricades);
            if (!blockButton.LimitedUses)
            {
                blockButton.Button?.usesRemainingText.gameObject.SetActive(false);
                blockButton.Button?.usesRemainingSprite.gameObject.SetActive(false);
            }
            else
            {
                blockButton.Button?.usesRemainingText.gameObject.SetActive(true);
                blockButton.Button?.usesRemainingSprite.gameObject.SetActive(true);
            }
        }
    }

    [RegisterEvent]
    public static void CompleteTaskEvent(CompleteTaskEvent @event)
    {
        var opt = OptionGroupSingleton<PlumberOptions>.Instance;
        var button = CustomButtonSingleton<PlumberBlockButton>.Instance;
        if (@event.Player.AmOwner)
        {
            ++ActiveBarricadeTaskCount;
            if (@event.Player.Data.Role is not PlumberRole)
            {
                return;
            }

            if (button.LimitedUses &&
                opt.BarricadesPerTasks != 0 && opt.BarricadesPerTasks <= ActiveBarricadeTaskCount)
            {
                ++button.UsesLeft;
                ++button.ExtraUses;
                button.SetUses(button.UsesLeft);
                ActiveBarricadeTaskCount = 0;
            }
        }
    }

    [RegisterEvent]
    public static void PlayerCanUseEventHandler(PlayerCanUseEvent @event)
    {
        if (!@event.IsVent)
        {
            return;
        }

        var vent = @event.Usable.TryCast<Vent>();

        if (vent == null)
        {
            return;
        }

        if (PlumberRole.VentsBlocked.ContainsKey(vent.Id) || PlumberRole.VentFlushSet.Contains(vent.Id))
        {
            @event.Cancel();
        }
    }

    [RegisterEvent]
    public static void EjectionEventHandler(EjectionEvent _)
    {
        if ((int)OptionGroupSingleton<PlumberOptions>.Instance.BarricadeRoundDuration > 0)
        {
            var unblockedVents = new HashSet<int>();
            foreach (var (ventId, rounds) in PlumberRole.VentsBlocked)
            {
                if (rounds == 1)
                {
                    unblockedVents.Add(ventId);
                    PlumberRole.Barricades.Remove(ventId, out var barricade);
                    UnityEngine.Object.Destroy(barricade);
                    continue;
                }

                PlumberRole.VentsBlocked[ventId] -= 1;
            }

            foreach (var vent in unblockedVents)
            {
                PlumberRole.VentsBlocked.Remove(vent);
            }

            PlumberRole.VentFlushSet.Clear();
        }

        foreach (var plumber in CustomRoleUtils.GetActiveRolesOfType<PlumberRole>())
        {
            plumber.SetupBarricades();
        }
    }
}