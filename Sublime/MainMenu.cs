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
    public static GTA.Menu MainMenu;

    private void SublimeMainMenu()
    {
        List<IMenuItem> mainMenuItems = new List<IMenuItem>();

        var buttonPlayer = new MenuButton("Player");
        buttonPlayer.Activated += (sender, args) => SublimePlayerMenu();
        mainMenuItems.Add(buttonPlayer);

        var buttonWeapon = new MenuButton("Weapon");
        buttonWeapon.Activated += (sender, args) => SublimeWeaponMenu();
        mainMenuItems.Add(buttonWeapon);

        var buttonVehicle = new MenuButton("Vehicle");
        buttonVehicle.Activated += (sender, args) => SublimeVehicleMenu();
        mainMenuItems.Add(buttonVehicle);

        MainMenu = new GTA.Menu("Sublime Mod", mainMenuItems.ToArray());
        DrawMenu(MainMenu);
    }
}

