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
    public static bool HasWeaponFireBullets { get; set; }
    public static bool HasWeaponExplosiveBullets { get; set; }
    public static bool IsExplosiveMeleeEnabled { get; set; }
    public static bool IsOneHitKillEnabled { get; set; }
    public static bool IsGravityGunEnabled { get; set; }
    public static Entity GetAimedEntity { get; set; }

    List<WeaponHash> weaponHashList = new List<WeaponHash>()
    {
        WeaponHash.BZGas, WeaponHash.FireExtinguisher, WeaponHash.Firework,
        WeaponHash.Grenade, WeaponHash.GrenadeLauncher, WeaponHash.GrenadeLauncherSmoke,
        WeaponHash.HomingLauncher, WeaponHash.Molotov, WeaponHash.ProximityMine,
        WeaponHash.RPG, WeaponHash.SmokeGrenade, WeaponHash.StickyBomb
    };

    public WeaponFunctions()
    {
        Tick += OnTick;
        Game.Player.Character.Weapons.Current.InfiniteAmmoClip = false;
        Function.Call(Hash.SET_PLAYER_WEAPON_DAMAGE_MODIFIER, Game.Player, 1.0f);
        Function.Call(Hash.SET_PLAYER_MELEE_WEAPON_DAMAGE_MODIFIER, Game.Player, 1.0f);
    }
    private void OnTick(object sender, EventArgs e)
    {
        if (HasWeaponInfiniteAmmo)
        {
            Weapon playerWeapon = Game.Player.Character.Weapons.Current;

            playerWeapon.InfiniteAmmoClip = true;
            if (playerWeapon.AmmoInClip != playerWeapon.MaxAmmoInClip)
            {
                playerWeapon.AmmoInClip = playerWeapon.MaxAmmoInClip;
            }
        }

        if (HasWeaponFireBullets)
            Function.Call(Hash.SET_FIRE_AMMO_THIS_FRAME, Game.Player);

        if (HasWeaponExplosiveBullets)
            Function.Call(Hash.SET_EXPLOSIVE_AMMO_THIS_FRAME, Game.Player);

        if (IsExplosiveMeleeEnabled)
            Function.Call(Hash.SET_EXPLOSIVE_MELEE_THIS_FRAME, Game.Player);

        // Revisit Gravity Gun function - needs better logic.
        if (IsGravityGunEnabled)
        {
            WeaponHash currentWeapon = (WeaponHash) Game.Player.Character.Weapons.Current.Hash;
            Entity entity = Game.Player.GetTargetedEntity();

            if (Game.Player.Character.IsShooting && !weaponHashList.Contains(currentWeapon))
            {
                GetAimedEntity = entity;
                if (entity is Ped)
                {
                    Ped ped = (Ped) entity;
                    if (ped.IsInVehicle())
                    {
                        Function.Call(Hash.SET_VEHICLE_GRAVITY, ped.CurrentVehicle, 0);
                        ped.CurrentVehicle.ApplyForceRelative(new Vector3(0.0f, 0.0f, 1.5f));
                    }
                    else
                    {
                        Function.Call(Hash.SET_ENTITY_HAS_GRAVITY, ped, false);
                        ped.ApplyForceRelative(new Vector3(0.0f, 0.0f, 1.5f));
                    }
                }

                if (entity is Vehicle)
                {
                    Vehicle vehicle = (Vehicle) entity;
                    Function.Call(Hash.SET_VEHICLE_GRAVITY, vehicle, 0);
                    vehicle.ApplyForceRelative(new Vector3(0.0f, 0.0f, 1.5f));
                }
            }
        }
    }

    // Weapon Component Related
    internal static void ChangeWeaponComponent(WeaponHash weaponHash, KeyValuePair<string, string> weaponComponent)
    {
        int weaponComponentHash = Function.Call<int>(Hash.GET_HASH_KEY, weaponComponent.Key);
        bool hasWeaponGotComponent = Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON_COMPONENT, Game.Player.Character, (int) weaponHash, weaponComponentHash);

        if (!hasWeaponGotComponent)
        {
            Function.Call(Hash.GIVE_WEAPON_COMPONENT_TO_PED, Game.Player.Character, (int) weaponHash, weaponComponentHash);
        }
        else
        {
            Function.Call(Hash.REMOVE_WEAPON_COMPONENT_FROM_PED, Game.Player.Character, (int) weaponHash, weaponComponentHash);
        }
    }
    internal static void ChangeWeaponTint(WeaponHash weaponHash, int tintIndex)
    {
        Function.Call(Hash.SET_PED_WEAPON_TINT_INDEX, Game.Player.Character, (int) weaponHash, tintIndex);
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

        Sublime.DisplayMessage("Infinite Ammunition", HasWeaponInfiniteAmmo);
    }

    internal static void ToggleFireBullets()
    {
        HasWeaponFireBullets = !HasWeaponFireBullets;

        Sublime.DisplayMessage("Fire Bullets", HasWeaponFireBullets);
    }

    internal static void ToggleExplosiveBullets()
    {
        HasWeaponExplosiveBullets = !HasWeaponExplosiveBullets;

        Sublime.DisplayMessage("Explosive Bullets", HasWeaponExplosiveBullets);
    }

    internal static void ToggleExplosiveMelee()
    {
        IsExplosiveMeleeEnabled = !IsExplosiveMeleeEnabled;

        Sublime.DisplayMessage("Explosive Melee", IsExplosiveMeleeEnabled);
    }

    internal static void ToggleOneHitKill()
    {
        IsOneHitKillEnabled = !IsOneHitKillEnabled;

        if (IsOneHitKillEnabled)
        {
                Function.Call(Hash.SET_PLAYER_WEAPON_DAMAGE_MODIFIER, Game.Player, 1000.0f);
                Function.Call(Hash.SET_PLAYER_MELEE_WEAPON_DAMAGE_MODIFIER, Game.Player, 1000.0f);
        }
        else
        {
            Function.Call(Hash.SET_PLAYER_WEAPON_DAMAGE_MODIFIER, Game.Player, 1.0f);
            Function.Call(Hash.SET_PLAYER_MELEE_WEAPON_DAMAGE_MODIFIER, Game.Player, 1.0f);
        }

        Sublime.DisplayMessage("One-Hit Kill", IsOneHitKillEnabled);
    }

    internal static void ToggleGravityGun()
    {
        IsGravityGunEnabled = !IsGravityGunEnabled;

        Sublime.DisplayMessage("Gravity Gun", IsGravityGunEnabled);
    }
}

