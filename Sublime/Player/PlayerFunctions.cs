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

class PlayerFunctions : Script
{
    public static int WantedLevel { get { return Game.Player.WantedLevel; } set { Game.Player.WantedLevel = value; } }
    public static bool IsPlayerNeverWanted { get; set; }

    public PlayerFunctions()
    {
        Tick += OnTick;
    }
    private void OnTick(object sender, EventArgs e)
    {
        if (IsPlayerNeverWanted)
        {
            WantedLevel = 0;
            ((MenuNumericScroller) Sublime.PlayerMenu.Items[4]).TimesIncremented = 0;
        }
    }

    // Teleport Related
    internal static void TeleportToLocation(KeyValuePair<string, Vector3> location)
    {
        Sublime.GetEntity.Position = location.Value;
        Sublime.DisplayMessage(string.Format("Teleport to {0} succeeded", location.Key));
    }
    internal static void TeleportToMarker()
    {
        Entity e = Sublime.GetEntity;

        float[] groundHeight = new float[]
            {
                100.0f, 150.0f, 50.0f, 0.0f, 200.0f, 250.0f, 300.0f, 350.0f, 400.0f,
                450.0f, 500.0f, 550.0f, 600.0f, 650.0f, 700.0f, 750.0f, 800.0f
            };

        unsafe
        {
            Blip[] activeBlips = World.GetActiveBlips();
            Blip blip = activeBlips.FirstOrDefault(b => b.Type == 4);
            if (blip != null)
            {
                e.Position = blip.Position;
                foreach (float height in groundHeight)
                {
                    float zCoord = 0;
                    e.Position = new Vector3(e.Position.X, e.Position.Y, height);
                    Wait(100);
                    if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, e.Position.X,
                        e.Position.Y, height, &zCoord))
                    {
                        e.Position = new Vector3(e.Position.X, e.Position.Y, zCoord + 3);
                        break;
                    }
                }
                UI.Notify("Teleport succeeded");
            }
            else
            {
                UI.Notify("Marker not found.");
            }
        }
    }

    internal static void FixPlayer()
    {
        Game.Player.Character.Health = Game.Player.Character.MaxHealth;
        Game.Player.Character.Armor = Function.Call<int>(Hash.GET_PLAYER_MAX_ARMOUR, Game.Player);
        Function.Call(Hash._RECHARGE_SPECIAL_ABILITY, Game.Player, true);
        Function.Call(Hash.RESET_PLAYER_STAMINA, Game.Player);

        Sublime.DisplayMessage("Player health, armor, stamina and special ability restored");
    }

    internal static void SetMoney()
    {
        string userCashInput = Game.GetUserInput(12);
        int result;
        bool success = int.TryParse(userCashInput, out result);

        if (success)
        {
            Game.Player.Money = result;
            Sublime.DisplayMessage(string.Format(CultureInfo.GetCultureInfo(1033), "Player money set to: {0:C}", result));
            return;
        }
        else if (userCashInput != string.Empty)
        {
            Sublime.DisplayMessage("Could not parse. Please enter a valid integer number.");
            return;
        }
    }
    internal static void AddMoney()
    {
        Game.Player.Money += 1000000;
        Sublime.DisplayMessage(string.Format(CultureInfo.GetCultureInfo(1033), "{0:C} added to player", 1000000));
    }

    internal static void SetWantedLevel(MenuNumericScroller sender)
    {
        WantedLevel = (int) sender.Value;
    }
    internal static void ToggleNeverWanted()
    {
        if (!IsPlayerNeverWanted)
        {
            IsPlayerNeverWanted = true;
        }
        else
        {
            IsPlayerNeverWanted = false;
        }

        Sublime.DisplayMessage("Never Wanted", IsPlayerNeverWanted);
    }
}

