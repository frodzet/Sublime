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
    public static GTA.Menu VehicleMenu;

    private void SublimeVehicleMenu()
    {
        List<IMenuItem> vehicleMenuItems = new List<IMenuItem>();

        var buttonSpawnVehicleMenu = new MenuButton("Vehicle Spawner Menu");
        vehicleMenuItems.Add(buttonSpawnVehicleMenu);

        var buttonFixVehicle = new MenuButton("Fix Vehicle");
        buttonFixVehicle.Activated += (sender, args) => VehicleFunctions.FixVehicle();
        vehicleMenuItems.Add(buttonFixVehicle);

        var toggleIndestructible = new MenuToggle("Indestructible", "", VehicleFunctions.IsVehicleIndestructible);
        toggleIndestructible.Changed += (sender, args) => VehicleFunctions.ToggleVehicleIndestructible();
        vehicleMenuItems.Add(toggleIndestructible);

        var toggleSeatBelt = new MenuToggle("Seat Belt", "", VehicleFunctions.IsSeatBeltEnabled);
        toggleSeatBelt.Changed += (sender, args) => VehicleFunctions.ToggleSeatBelt();
        vehicleMenuItems.Add(toggleSeatBelt);

        var toggleLockDoors = new MenuToggle("Lock Doors", "", VehicleFunctions.IsLockedDoorsEnabled);
        toggleLockDoors.Changed += (sender, args) => VehicleFunctions.ToggleLockedDoors();
        vehicleMenuItems.Add(toggleLockDoors);

        var toggleNeverFallOffBike = new MenuToggle("Never Fall-Off Bike", "", VehicleFunctions.IsNeverFallOffBikeEnabled);
        toggleNeverFallOffBike.Changed += (sender, args) => VehicleFunctions.ToggleNeverFallOffBike();
        vehicleMenuItems.Add(toggleNeverFallOffBike);

        var toggleSpeedBoost = new MenuToggle("Speed Boost", "", VehicleFunctions.IsSpeedBoostEnabled);
        toggleSpeedBoost.Changed += (sender, args) => VehicleFunctions.ToggleSpeedBoost();
        vehicleMenuItems.Add(toggleSpeedBoost);

        var toggleVehicleJump = new MenuToggle("Vehicle Jump", "", VehicleFunctions.CanVehiclesJump);
        toggleVehicleJump.Changed += (sender, args) => VehicleFunctions.ToggleVehiclesCanJump();
        vehicleMenuItems.Add(toggleVehicleJump);

        var enumVehicleWeaponAssets = new MenuEnumScroller("Vehicle Weapon", "", VehicleFunctions.VehicleWeaponAssetsDict.Keys.ToArray(), VehicleFunctions.VehicleWeaponAssetIndex);
        enumVehicleWeaponAssets.Changed += (sender, args) => VehicleFunctions.VehicleWeaponAssetIndex = enumVehicleWeaponAssets.Index;
        vehicleMenuItems.Add(enumVehicleWeaponAssets);

        var toggleVehicleWeapons = new MenuToggle("Vehicle Weapons", "", VehicleFunctions.IsVehicleWeaponsEnabled);
        toggleVehicleWeapons.Changed += (sender, args) => VehicleFunctions.ToggleVehicleWeapons();
        vehicleMenuItems.Add(toggleVehicleWeapons);

        VehicleMenu = new GTA.Menu("Vehicle Options", vehicleMenuItems.ToArray());
        DrawMenu(VehicleMenu);
    }
}
