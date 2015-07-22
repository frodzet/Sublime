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
    public static GTA.Menu VehicleModCategoriesMenu;

    private void SublimeVehicleModCategoriesMenu()
    {
        List<IMenuItem> vehicleModCategoriesList = new List<IMenuItem>();

        Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
        Function.Call(Hash.SET_VEHICLE_MOD_KIT, currentVehicle, 0);

        foreach (VehicleMod vehicleMod in Enum.GetValues(typeof(VehicleMod)))
        {
            int vehicleModCount = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, currentVehicle, (int) vehicleMod);

            if (vehicleModCount != 0)
            {
                if (vehicleMod != VehicleMod.BackWheels && vehicleMod != VehicleMod.FrontWheels)
                {
                    var buttonModCategory = new MenuButton(vehicleMod.ToString());
                    //buttonModCategory.Activated += (sender, args) => SublimeVehicleModTypeMenu(currentVehicle);
                    vehicleModCategoriesList.Add(buttonModCategory); 
                }
            }          
        }

        var buttonRespray = new MenuButton("Respray");
        vehicleModCategoriesList.Add(buttonRespray);

        var buttonLights = new MenuButton("Lights");
        vehicleModCategoriesList.Add(buttonLights);

        var buttonPlates = new MenuButton("Plates");
        vehicleModCategoriesList.Add(buttonPlates);

        var buttonWindows = new MenuButton("Windows");
        vehicleModCategoriesList.Add(buttonWindows);

        var buttonWheels = new MenuButton("Wheels");
        buttonWheels.Activated += (sender, args) => SublimeVehicleModWheelsMenu(currentVehicle);
        vehicleModCategoriesList.Add(buttonWheels);

        var toggleTurbo = new MenuToggle("Turbo", "", true);
        vehicleModCategoriesList.Add(toggleTurbo);

        VehicleModCategoriesMenu = new GTA.Menu("Mod Categories", vehicleModCategoriesList.OrderBy(item => item.Caption).ToArray());
        DrawMenu(VehicleModCategoriesMenu);
    }

    private void SublimeVehicleModWheelsMenu(Vehicle currentVehicle)
    {
        List<IMenuItem> wheelCategoriesList = new List<IMenuItem>();

        var buttonWheelType = new MenuButton("Wheel Type");
        buttonWheelType.Activated += (sender, args) => SublimeVehicleModWheelTypesMenu(currentVehicle);
        wheelCategoriesList.Add(buttonWheelType);

        var vehicleModWheelsMenu = new GTA.Menu("Wheels", wheelCategoriesList.ToArray());
        DrawMenu(vehicleModWheelsMenu);
    }

    private void SublimeVehicleModWheelTypesMenu(Vehicle currentVehicle)
    {
        List<MenuButton> wheelTypesList = new List<MenuButton>();
        foreach (VehicleWheelType wheelType in Enum.GetValues(typeof(VehicleWheelType)))
        {
            var buttonWheels = new MenuButton(wheelType.ToString());
            buttonWheels.Activated += (sender, args) =>
            {
                List<MenuButton> wheelsList = new List<MenuButton>();

                Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, currentVehicle, (int) wheelType);

                int numFrontWheels = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, currentVehicle, (int) VehicleMod.FrontWheels);

                for (int wheelIndex = 0; wheelIndex < numFrontWheels; wheelIndex++)
                {
                    string getModLocalizationText = Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, currentVehicle, (int) VehicleMod.FrontWheels, wheelIndex);
                    string getModDisplayText = Function.Call<string>(Hash._GET_LABEL_TEXT, getModLocalizationText);

                    int value = wheelIndex;

                    var buttonSetVehicleWheel = new MenuButton(getModDisplayText);
                    buttonSetVehicleWheel.Description = string.Format("Wheel {0} / {1}", wheelIndex + 1, numFrontWheels);
                    buttonSetVehicleWheel.Activated += (subSender, subArgs) =>
                    {
                        currentVehicle.SetMod(VehicleMod.FrontWheels, value, false);
                    };
                    wheelsList.Add(buttonSetVehicleWheel);
                }

                ListMenu vehicleWheelsMenu = new ListMenu(buttonWheels.Caption, wheelsList.ToArray(), 20);
                DrawMenu(vehicleWheelsMenu, true);
            };
            wheelTypesList.Add(buttonWheels);
        }

        var vehicleModWheelTypesMenu = new GTA.Menu("Wheel Types", wheelTypesList.ToArray());
        DrawMenu(vehicleModWheelTypesMenu);
    }


}

