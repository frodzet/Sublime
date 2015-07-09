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

class WeaponFunctions : Script
{
    public static bool HasWeaponInfiniteAmmo { get; set; }
    public static bool IsWeaponRapidFireEnabled { get; set; }

    public WeaponFunctions()
    {
        Tick += OnTick;
        Game.Player.Character.Weapons.Current.InfiniteAmmoClip = false;
    }
    private void OnTick(object sender, EventArgs e)
    {
        if (HasWeaponInfiniteAmmo)
            Game.Player.Character.Weapons.Current.InfiniteAmmoClip = true;


    }

    internal static void GiveAllWeapons()
    {
        foreach (WeaponHash weaponHash in Enum.GetValues(typeof(WeaponHash)))
        {
            bool hasPedGotWeapon = Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, Game.Player.Character, (int) weaponHash, false);

            if (!hasPedGotWeapon)
            {
                Game.Player.Character.Weapons.Give(weaponHash, 9999, false, false);
            }
            else
            {
                Function.Call(Hash.SET_PED_AMMO, Game.Player.Character, (int) weaponHash, 9999);
            }
        }
        
        Sublime.DisplayMessage("All weapons received");
    }

    internal static void ToggleInfiniteAmmo()
    {
        HasWeaponInfiniteAmmo = !HasWeaponInfiniteAmmo;

        if (!HasWeaponInfiniteAmmo)
            Game.Player.Character.Weapons.Current.InfiniteAmmoClip = false;

        Sublime.DisplayMessage("Infinite Ammo", HasWeaponInfiniteAmmo);
    }


}

