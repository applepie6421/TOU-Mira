using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Roles.Neutral;
using UnityEngine;

namespace TownOfUs.Modifiers.Neutral;

public sealed class VampireBittenModifier(byte sireId) : BaseModifier
{
    public override string ModifierName => "Bitten";
    public override bool HideOnUi => true;
    public byte SireId { get; } = sireId;

    private bool _waiting;

    public override void FixedUpdate()
    {
        if (!Player.AmOwner || Player.HasDied() || !OptionGroupSingleton<VampireOptions>.Instance.EldestVampireOnly)
        {
            return;
        }

        if (!VampireRole.IsEldest(Player))
        {
            _waiting = true;
            return;
        }

        if (!_waiting)
        {
            return;
        }

        _waiting = false;

        var sire = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(x => x.PlayerId == SireId);

        if (sire == null)
        {
            return;
        }

        var text = MiraLocaleManager.Get("TownOfUsMira.Role.VampireEldestNotif").Replace("<player>", sire.Data.PlayerName);
        var notif = Helpers.CreateAndShowNotification($"<b>{text}</b>", Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Vampire.LoadAsset());
        notif.AdjustNotification();
    }
}
