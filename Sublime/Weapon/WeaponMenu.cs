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
    public static GTA.Menu WeaponMenu;

    private void SublimeWeaponMenu()
    {
        List<IMenuItem> weaponMenuItems = new List<IMenuItem>();

        var buttonGiveAllWeapons = new MenuButton("Give All Weapons");
        buttonGiveAllWeapons.Activated += (sender, args) => WeaponFunctions.GiveAllWeapons();
        weaponMenuItems.Add(buttonGiveAllWeapons);

        var toggleInfiniteAmmo = new MenuToggle("Infinite Ammo", "", WeaponFunctions.HasWeaponInfiniteAmmo);
        toggleInfiniteAmmo.Changed += (sender, args) => WeaponFunctions.ToggleInfiniteAmmo();
        weaponMenuItems.Add(toggleInfiniteAmmo);


        WeaponMenu = new GTA.Menu("Weapon Options", weaponMenuItems.ToArray());
        DrawMenu(WeaponMenu);
    }
}

