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

partial class Sublime : Script
{
    public static Entity GetEntity
    {
        get
        {
            Ped playerPed = Game.Player.Character;
            Entity entity = playerPed;
            if (playerPed.IsInVehicle())
                entity = playerPed.CurrentVehicle;

            return entity;
        }
    }

    public Sublime()
    {
        KeyUp += OnKeyUp;
        FillWeaponComponents();
    }
    private void OnKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F9)
        {
            if (View.ActiveMenus == 0)
            {
                SublimeMainMenu();
            }
            else
            {
                View.CloseAllMenus();
            }
        }

        if (e.KeyCode == Keys.Back)
        {
            View.HandleBack();
        }

        if (e.KeyCode == Keys.Insert)
        {
            UI.Notify("Initialized default settings.");
        }

        //if (e.KeyCode == Keys.J)
        //{
        //    float x = Game.Player.Character.Position.X;
        //    float y = Game.Player.Character.Position.Y;
        //    float z = Game.Player.Character.Position.Z;
        //    string GetZoneName = Function.Call<string>(Hash.GET_NAME_OF_ZONE, x, y, z);
        //    Logger.Log(string.Format("Zone: {0}\t{1}", GetZoneName, Game.Player.Character.Position.ToString()));
        //    UI.Notify("Log Updated");
        //}
    }

    private void DrawMenu(GTA.Menu name)
    {
        // Header Layout
        name.HeaderHeight += 2;
        name.HeaderFont = GTA.Font.Pricedown;
        name.HeaderColor = System.Drawing.Color.FromArgb(233, 0, 16, 99);

        // Items Layout
        name.ItemTextCentered = false;
        name.ItemHeight -= 7;
        name.ItemTextScale = 0.32f;
        name.SelectedItemColor = System.Drawing.Color.FromArgb(112, 0, 16, 115);
        name.UnselectedItemColor = System.Drawing.Color.FromArgb(112, 0, 16, 0);
        name.SelectedTextColor = System.Drawing.Color.White;
        name.UnselectedTextColor = System.Drawing.Color.AntiqueWhite;

        // Footer Layout
        name.HasFooter = false;
        name.FooterHeight -= 32;
        name.FooterFont = GTA.Font.Pricedown;
        name.FooterColor = System.Drawing.Color.FromArgb(233, 0, 16, 99);
        name.FooterTextColor = System.Drawing.Color.White;

        View.MenuPosition = new System.Drawing.Point(5, 5);
        View.AddMenu(name);
    }

    public static void DisplayMessage(string msg, bool state)
    {
        string stateMsg;
        if (state)
            stateMsg = " Activated";
        else
            stateMsg = " Deactivated";

        UI.Notify(msg + ":" + stateMsg);
    }
    public static void DisplayMessage(string msg)
    {
        UI.Notify(msg);
    }
}

