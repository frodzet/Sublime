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

class VehicleFunctions : Script
{
    public static bool IsVehicleIndestructible { get; set; }
    public static bool IsSeatBeltEnabled { get; set; }
    public const int PedFlagCanFlyThroughWindscreen = 32;
    public static bool CanFlyThroughWindscreen { get { return Function.Call<bool>(Hash.GET_PED_CONFIG_FLAG, Game.Player.Character, PedFlagCanFlyThroughWindscreen); } }
    public static bool IsLockedDoorsEnabled { get; set; }
    public static bool IsSpeedBoostEnabled { get; set; }
    public static bool IsNeverFallOffBikeEnabled { get; set; }
    public static bool CanVehiclesJump { get; set; }
    public static bool IsVehicleWeaponsEnabled { get; set; }
    public static int VehicleWeaponAssetIndex { get; set; }

    public static Dictionary<string, string> VehicleWeaponAssetsDict = new Dictionary<string, string>()
    {
        {"Rockets", "WEAPON_AIRSTRIKE_ROCKET"}, {"Bullets", "VEHICLE_WEAPON_PLAYER_BULLET"}
    };
    public static string GetVehicleWeaponAsset()
    {
        string weaponAsset = ((MenuEnumScroller) Sublime.VehicleMenu.Items[8]).Value;
        foreach (KeyValuePair<string, string> weapon in VehicleWeaponAssetsDict)
        {
            if (weapon.Key == weaponAsset)
            {
                weaponAsset = weapon.Value;
                break;
            }
        }

        return weaponAsset;
    }

    public VehicleFunctions()
    {
        Tick += OnTick;

        // Onload.
        SetVehicleIndestructible(false);
        Function.Call(Hash.SET_PED_CONFIG_FLAG, Game.Player.Character, PedFlagCanFlyThroughWindscreen, true);
        Game.Player.Character.CanBeDraggedOutOfVehicle = true;
        Function.Call(Hash.SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, Game.Player.Character, 0);
    }
    private void OnTick(object sender, EventArgs e)
    {
        if (IsVehicleIndestructible)
            SetVehicleIndestructible(true);

        if (IsSeatBeltEnabled)
        {
            if (CanFlyThroughWindscreen)
            {
                Function.Call(Hash.SET_PED_CONFIG_FLAG, Game.Player.Character, PedFlagCanFlyThroughWindscreen, false);
            }
        }

        if (IsLockedDoorsEnabled)
            Game.Player.Character.CanBeDraggedOutOfVehicle = false;

        if (IsNeverFallOffBikeEnabled)
            Function.Call(Hash.SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, Game.Player.Character, 1);

        if (IsSpeedBoostEnabled)
        {
            if (Game.IsKeyPressed(Keys.NumPad9))
            {
                Game.Player.Character.CurrentVehicle.Speed += 0.5f;
            }
            if (Game.IsKeyPressed(Keys.NumPad0))
            {
                Game.Player.Character.CurrentVehicle.Speed = 0.0f;
            }
        }

        if (CanVehiclesJump)
        {
            if (Game.IsKeyPressed(Keys.B) && Game.Player.Character.CurrentVehicle.IsOnAllWheels)
            {
                Game.Player.Character.CurrentVehicle.ApplyForceRelative(Game.Player.Character.CurrentVehicle.UpVector * 5.0f);
            }
        }

        if (IsVehicleWeaponsEnabled)
        {
            if (Game.Player.Character.IsInVehicle() && Game.IsKeyPressed(Keys.Add))
            {
                Vehicle vehicle = Game.Player.Character.CurrentVehicle;

                GTA.Model weaponAsset = Function.Call<int>(Hash.GET_HASH_KEY, GetVehicleWeaponAsset());
                if (!Function.Call<bool>(Hash.HAS_WEAPON_ASSET_LOADED, weaponAsset))
                {
                    Function.Call(Hash.REQUEST_WEAPON_ASSET, weaponAsset);
                    while (!Function.Call<bool>(Hash.HAS_WEAPON_ASSET_LOADED, weaponAsset))
                    {
                        Wait(0);
                    }
                }

                Vector3 vehicleDimensions = vehicle.Model.GetDimensions();

                // Left weapon.
                Vector3 leftWeaponFromCoords = vehicle.GetOffsetInWorldCoords(new Vector3(-(vehicleDimensions.X) + 1.25f, vehicleDimensions.Y + 1.25f, 0.3f));
                Vector3 leftWeaponToCoords = vehicle.GetOffsetInWorldCoords(new Vector3(-(vehicleDimensions.X) + 1.25f, vehicleDimensions.Y + 100.0f, 0.3f));

                // Right weapon.
                Vector3 rightWeaponFromCoords = vehicle.GetOffsetInWorldCoords(new Vector3(vehicleDimensions.X - 1.25f, vehicleDimensions.Y + 1.25f, 0.3f));
                Vector3 rightWeaponToCoords = vehicle.GetOffsetInWorldCoords(new Vector3(vehicleDimensions.X + 1.25f, vehicleDimensions.Y + 100.0f, 0.3f));

                World.ShootBullet(leftWeaponFromCoords, leftWeaponToCoords, Game.Player.Character, weaponAsset, 10000);
                World.ShootBullet(rightWeaponFromCoords, rightWeaponToCoords, Game.Player.Character, weaponAsset, 10000);
                weaponAsset.MarkAsNoLongerNeeded();
                Wait(150);
            }
        }

    }

    internal static void FixVehicle()
    {
        Game.Player.Character.CurrentVehicle.Repair();
        Game.Player.Character.CurrentVehicle.EngineRunning = true;

        Sublime.DisplayMessage("Vehicle Fixed");
    }

    internal static void ToggleVehicleIndestructible()
    {
        IsVehicleIndestructible = !IsVehicleIndestructible;

        if (!IsVehicleIndestructible)
        {
            SetVehicleIndestructible(false);
        }

        Sublime.DisplayMessage("Vehicle Indestructible", IsVehicleIndestructible);
    }
    private static void SetVehicleIndestructible(bool value)
    {
        Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
        Vehicle prevVehicle = Game.Player.LastVehicle;
        Vehicle vehicle;

        if (Game.Player.Character.IsInVehicle())
            vehicle = currentVehicle;
        else
            vehicle = prevVehicle;

        if (value)
        {
            Function.Call(Hash.SET_ENTITY_INVINCIBLE, vehicle, true);
            Function.Call(Hash.SET_ENTITY_PROOFS, vehicle, 1, 1, 1, 1, 1, 1, 1, 1);
            Function.Call(Hash.SET_VEHICLE_TYRES_CAN_BURST, vehicle, false);
            Function.Call(Hash.SET_VEHICLE_WHEELS_CAN_BREAK, vehicle, false);
            Function.Call(Hash.SET_VEHICLE_CAN_BE_VISIBLY_DAMAGED, vehicle, false);
            foreach (VehicleDoor door in Enum.GetValues(typeof(VehicleDoor)))
            {
                vehicle.SetDoorBreakable(door, false);
            }
        }
        else
        {
            Function.Call(Hash.SET_ENTITY_INVINCIBLE, vehicle, false);
            Function.Call(Hash.SET_ENTITY_PROOFS, vehicle, 0, 0, 0, 0, 0, 0, 0, 0);
            Function.Call(Hash.SET_VEHICLE_TYRES_CAN_BURST, vehicle, true);
            Function.Call(Hash.SET_VEHICLE_WHEELS_CAN_BREAK, vehicle, true);
            Function.Call(Hash.SET_VEHICLE_CAN_BE_VISIBLY_DAMAGED, vehicle, true);
            foreach (VehicleDoor door in Enum.GetValues(typeof(VehicleDoor)))
            {
                vehicle.SetDoorBreakable(door, true);
            }
        }
    }

    internal static void ToggleSeatBelt()
    {
        IsSeatBeltEnabled = !IsSeatBeltEnabled;

        if (!IsSeatBeltEnabled)
            Function.Call(Hash.SET_PED_CONFIG_FLAG, Game.Player.Character, PedFlagCanFlyThroughWindscreen, true);

        Sublime.DisplayMessage("Seat Belt", IsSeatBeltEnabled);
    }

    internal static void ToggleLockedDoors()
    {
        IsLockedDoorsEnabled = !IsLockedDoorsEnabled;

        if (!IsLockedDoorsEnabled)
            Game.Player.Character.CanBeDraggedOutOfVehicle = true;

        Sublime.DisplayMessage("Locked Doors", IsLockedDoorsEnabled);
    }

    internal static void ToggleNeverFallOffBike()
    {
        IsNeverFallOffBikeEnabled = !IsNeverFallOffBikeEnabled;

        if (!IsNeverFallOffBikeEnabled)
            Function.Call(Hash.SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, Game.Player.Character, 0);

        Sublime.DisplayMessage("Never Fall-Off Bike", IsNeverFallOffBikeEnabled);
    }

    internal static void ToggleSpeedBoost()
    {
        IsSpeedBoostEnabled = !IsSpeedBoostEnabled;

        Sublime.DisplayMessage("Speed Boost", IsSpeedBoostEnabled);
    }

    internal static void ToggleVehiclesCanJump()
    {
        CanVehiclesJump = !CanVehiclesJump;

        Sublime.DisplayMessage("Vehicle Jump", CanVehiclesJump);
    }

    internal static void ToggleVehicleWeapons()
    {
        IsVehicleWeaponsEnabled = !IsVehicleWeaponsEnabled;

        Sublime.DisplayMessage("Vehicle Weapons", IsVehicleWeaponsEnabled);
    }
}

