using HarmonyLib;
using UnityEngine.Events;

namespace TownOfUs.Patches.Misc;

[HarmonyPatch(typeof(Vent), nameof(Vent.SetButtons))]
public static class VentHoverPatch
{
    private static readonly HashSet<int> Wired = [];
    private static bool _warned;

    public static void Clear()
    {
        Wired.Clear();
        _warned = false;
    }

    [HarmonyPostfix]
    public static void Postfix(Vent __instance, [HarmonyArgument(0)] bool enabled)
    {
        if (!enabled || __instance.Buttons == null)
        {
            return;
        }

        foreach (var button in __instance.Buttons)
        {
            // SetButtons runs every time a vent is approached, so each arrow is wired once
            if (button == null || !Wired.Add(button.GetInstanceID()))
            {
                continue;
            }

            AddHover(button);
        }
    }

    private static void AddHover(ButtonBehavior button)
    {
        var highlight = button.transform.FindChild("ButtonHighlight");

        if (highlight == null)
        {
            Warn($"{button.name} has no ButtonHighlight child");
            return;
        }

        var passive = button.GetComponent<PassiveButton>();

        if (passive == null)
        {
            Warn($"{button.name} has no PassiveButton");
            return;
        }

        passive.OnMouseOver.AddListener((UnityAction)(() => highlight.gameObject.SetActive(true)));
        passive.OnMouseOut.AddListener((UnityAction)(() => highlight.gameObject.SetActive(false)));
    }

    private static void Warn(string message)
    {
        if (_warned)
        {
            return;
        }

        _warned = true;
        Error($"VentHoverPatch: {message}");
    }
}
