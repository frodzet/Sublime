using GTA;
using GTA.Native;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

partial class Sublime
{
    public static GTA.Menu PlayerMenu;
    public static GTA.Menu TeleportMenu;

    private void SublimePlayerMenu()
    {
        List<IMenuItem> playerMenuItems = new List<IMenuItem>();

        var buttonTeleport = new MenuButton("Teleport Menu");
        buttonTeleport.Activated += (sender, args) => SublimeTeleportMenu();
        playerMenuItems.Add(buttonTeleport);

        var buttonFixPlayer = new MenuButton("Fix Player");
        buttonFixPlayer.Activated += (sender, args) => PlayerFunctions.FixPlayer();
        playerMenuItems.Add(buttonFixPlayer);

        var buttonSetMoney = new MenuButton("Set Money");
        buttonSetMoney.Activated += (sender, args) => PlayerFunctions.SetMoney();
        playerMenuItems.Add(buttonSetMoney);

        var buttonAddMoney = new MenuButton("Add Money");
        buttonAddMoney.Activated += (sender, args) => PlayerFunctions.AddMoney();
        playerMenuItems.Add(buttonAddMoney);

        var numericWantedLevel = new MenuNumericScroller("Set Wanted Level", "", 0.0f, 5.0f, 1.0f, PlayerFunctions.WantedLevel);
        numericWantedLevel.Changed += (sender, args) => PlayerFunctions.SetWantedLevel(numericWantedLevel);
        playerMenuItems.Add(numericWantedLevel);

        var toggleNeverWanted = new MenuToggle("Never Wanted", "", PlayerFunctions.IsPlayerNeverWanted);
        toggleNeverWanted.Changed += (sender, args) => PlayerFunctions.ToggleNeverWanted();
        playerMenuItems.Add(toggleNeverWanted);

        var toggleInvincible = new MenuToggle("Invincible", "", PlayerFunctions.IsPlayerInvincible);
        toggleInvincible.Changed += (sender, args) => PlayerFunctions.ToggleInvincibility();
        playerMenuItems.Add(toggleInvincible);

        PlayerMenu = new GTA.Menu("Player Options", playerMenuItems.ToArray());
        DrawMenu(PlayerMenu);
    }

    private void SublimeTeleportMenu()
    {
        List<IMenuItem> teleportMenuItems = new List<IMenuItem>();

        var buttonTeleportToMarker = new MenuButton("Marker");
        buttonTeleportToMarker.Activated += (sender, args) => PlayerFunctions.TeleportToMarker();
        teleportMenuItems.Add(buttonTeleportToMarker);

        Dictionary<string, Vector3> teleportDict = new Dictionary<string, Vector3>()
        {
            {"Michael's House", new Vector3(-852.4f, 160.0f, 65.6f)},
            {"Franklin's House", new Vector3(7.9f, 548.1f, 175.5f)},
            {"Franklin's Aunt", new Vector3(-14.8f, -1454.5f, 30.5f)},
            {"Trevor's Trailer", new Vector3(1985.7f, 3812.2f, 32.2f)},
            {"Airport Entrance", new Vector3(-1034.6f, -2733.6f, 13.8f)},
            {"Airport Field", new Vector3(-1336.0f, -3044.0f, 13.9f)},
            {"Elysian Island", new Vector3(338.2f, -2715.9f, 38.5f)},
            {"Jetsam", new Vector3(760.4f, -2943.2f, 5.8f)},
            {"Strip Club", new Vector3(127.4f, -1307.7f, 29.2f)},
            {"Elburro Heights", new Vector3(1384.0f, -2057.1f, 52.0f)},
            {"Ferris Wheel", new Vector3(-1670.7f, -1125.0f, 13.0f)},
            {"Chumash", new Vector3(-3192.6f, 1100.0f, 20.2f)},
            {"Windfarm", new Vector3(2354.0f, 1830.3f, 101.1f)},
            {"Military Base", new Vector3(-2047.4f, 3132.1f, 32.8f)},
            {"McKenzie Airfield", new Vector3(2121.7f, 4796.3f, 41.1f)},
            {"Desert Airfield", new Vector3(1747.0f, 3273.7f, 41.1f)},
            {"Chilliad", new Vector3(425.4f, 5614.3f, 766.5f)},
        };

        foreach (KeyValuePair<string, Vector3> location in teleportDict)
        {
            var buttonTeleportToLocation = new MenuButton(location.Key);
            buttonTeleportToLocation.Activated += (sender, args) => PlayerFunctions.TeleportToLocation(location);
            teleportMenuItems.Add(buttonTeleportToLocation);
        }

        TeleportMenu = new GTA.Menu("Teleport Options", teleportMenuItems.ToArray());
        DrawMenu(TeleportMenu);
    }
}

