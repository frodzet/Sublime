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
    public static GTA.Menu VehicleSpawnerCategoriesMenu;

    private void SublimeVehicleMenu()
    {
        List<IMenuItem> vehicleMenuItems = new List<IMenuItem>();

        var buttonVehicleSpawnerMenu = new MenuButton("Vehicle Spawner Menu");
        buttonVehicleSpawnerMenu.Activated += (sender, args) => SublimeVehicleSpawnerMenu();
        vehicleMenuItems.Add(buttonVehicleSpawnerMenu);

        var toggleWarpInSpawned = new MenuToggle("Warp in Spawned Vehicle", "", VehicleFunctions.IsWarpInSpawnedVehicleEnabled);
        toggleWarpInSpawned.Changed += (sender, args) => VehicleFunctions.ToggleWarpInSpawnedVehicle();
        vehicleMenuItems.Add(toggleWarpInSpawned);

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
        toggleLockDoors.Changed += (sender, args) => VehicleFunctions.ToggleLockDoors();
        vehicleMenuItems.Add(toggleLockDoors);

        var toggleNeverFallOffBike = new MenuToggle("Never Fall-Off Bike", "", VehicleFunctions.IsNeverFallOffBikeEnabled);
        toggleNeverFallOffBike.Changed += (sender, args) => VehicleFunctions.ToggleNeverFallOffBike();
        vehicleMenuItems.Add(toggleNeverFallOffBike);

        var toggleSpeedBoost = new MenuToggle("Speed Boost", "", VehicleFunctions.IsSpeedBoostEnabled);
        toggleSpeedBoost.Changed += (sender, args) => VehicleFunctions.ToggleSpeedBoost();
        vehicleMenuItems.Add(toggleSpeedBoost);

        var toggleVehicleJump = new MenuToggle("Vehicle Jump", "", VehicleFunctions.CanVehiclesJump);
        toggleVehicleJump.Changed += (sender, args) => VehicleFunctions.ToggleVehicleJump();
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

    private void SublimeVehicleSpawnerMenu()
    {
        List<IMenuItem> vehicleSpawnerMenuItems = new List<IMenuItem>();

        Dictionary<string, int> vehicleCategoriesDict = new Dictionary<string, int>()
        {
            {"Compacts", 0}, {"Sedans", 1}, {"SUVs", 2}, {"Coupes", 3}, {"Muscle", 4},
            {"Sports Classic", 5}, {"Sports", 6}, {"Super", 7}, {"Motorcycle", 8},
            {"Off-Road", 9}, {"Industrial", 10}, {"Utility", 11}, {"Vans/Pickups", 12},
            {"Bicycles", 13}, {"Boats", 14}, {"Helicopters", 15}, {"Airplanes", 16},
            {"Service", 17}, {"Emergency", 18}, {"Military", 19}, {"Commercial", 20},
            {"Trains/Containers", 21}
        };

        foreach (KeyValuePair<string, int> vehicleType in vehicleCategoriesDict)
        {
            var buttonVehicleCategoryMenu = new MenuButton(vehicleType.Key);

            // Add vehicle categories
            buttonVehicleCategoryMenu.Activated += (sender, args) =>
            {
                List<IMenuItem> vehicleSpawnerVehicleItems = new List<IMenuItem>();

                // Add vehicles to each category
                foreach (VehicleHash vehicle in Enum.GetValues(typeof(VehicleHash)))
                {
                    int vehicleClass = Function.Call<int>((Hash) 0xDEDF1C8BD47C2200, (int) vehicle);

                    if (vehicleClass == vehicleType.Value)
                    {
                        var buttonSpawnVehicle = new MenuButton(vehicle.ToString());
                        buttonSpawnVehicle.Activated += (subSender, subArgs) => VehicleFunctions.SpawnVehicle(vehicle);
                        vehicleSpawnerVehicleItems.Add(buttonSpawnVehicle);
                    }
                }

                ListMenu VehicleSpawnerVehiclesMenu = new ListMenu(vehicleType.Key, vehicleSpawnerVehicleItems.OrderBy(v => v.Caption).ToArray(), 20);
                DrawMenu(VehicleSpawnerVehiclesMenu);
            };

            vehicleSpawnerMenuItems.Add(buttonVehicleCategoryMenu);
        }

        VehicleSpawnerCategoriesMenu = new GTA.Menu("Vehicle Spawner", vehicleSpawnerMenuItems.ToArray());
        DrawMenu(VehicleSpawnerCategoriesMenu);
    }
}
