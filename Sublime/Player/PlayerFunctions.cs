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
    public static bool IsPlayerInvincible { get; set; }
    public static bool CanPlayerSuperJump { get; set; }
    public static bool CanPlayerFastRun { get; set; }
    public static bool CanPlayerFastSwim { get; set; }
    public static bool IsPlayerNoiseless { get; set; }
    public static bool HasPlayerUnlimitedStamina { get; set; }
    public static bool HasPlayerUnlimitedBreath { get; set; }
    public static float BreathValue = Function.Call<float>(Hash.GET_PLAYER_UNDERWATER_TIME_REMAINING, Game.Player);
    public static bool HasPlayerUnlimitedAbility { get; set; }
    public static bool IsAbilityMeterFull { get { return Function.Call<bool>(Hash.IS_SPECIAL_ABILITY_METER_FULL, Game.Player); } }

    public PlayerFunctions()
    {
        Tick += OnTick;
        Game.Player.Character.IsInvincible = false;
        Function.Call(Hash._SET_MOVE_SPEED_MULTIPLIER, Game.Player, 1.0f);
        Function.Call(Hash._SET_SWIM_SPEED_MULTIPLIER, Game.Player, 1.0f);
        unsafe
        {
            float defaultBreathValue = BreathValue;
            Function.Call(Hash.SET_PED_MAX_TIME_UNDERWATER, Game.Player.Character, &defaultBreathValue);
        }
    }
    private void OnTick(object sender, EventArgs e)
    {
        if (IsPlayerNeverWanted)
        {
            WantedLevel = 0;
            ((MenuNumericScroller) Sublime.PlayerMenu.Items[4]).TimesIncremented = 0;
        }

        if (IsPlayerInvincible)
            Game.Player.Character.IsInvincible = true;

        if (CanPlayerSuperJump)
            Function.Call(Hash.SET_SUPER_JUMP_THIS_FRAME, Game.Player);

        if (IsPlayerNoiseless)
            Function.Call(Hash.SET_PLAYER_NOISE_MULTIPLIER, Game.Player, 0.0f);

        if (HasPlayerUnlimitedStamina)
            Function.Call(Hash.RESET_PLAYER_STAMINA, Game.Player);

        if (HasPlayerUnlimitedBreath)
        {
            if (Game.Player.Character.IsSwimmingUnderWater)
            {
                Function.Call(Hash.SET_PED_MAX_TIME_UNDERWATER, Game.Player.Character, BreathValue++);
            }
        }

        if (HasPlayerUnlimitedAbility)
        {
            if (!IsAbilityMeterFull)
            {
                Function.Call(Hash._RECHARGE_SPECIAL_ABILITY, Game.Player, true);
            }
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

    internal static void ToggleInvincibility()
    {
        if (!IsPlayerInvincible)
        {
            IsPlayerInvincible = true;
        }
        else
        {
            IsPlayerInvincible = false;
            Game.Player.Character.IsInvincible = false;
        }

        Sublime.DisplayMessage("Invincibility", IsPlayerInvincible);
    }

    internal static void ToggleSuperJump()
    {
        if (!CanPlayerSuperJump)
        {
            CanPlayerSuperJump = true;
        }
        else
        {
            CanPlayerSuperJump = false;
        }

        Sublime.DisplayMessage("Super Jump", CanPlayerSuperJump);
    }

    internal static void ToggleFastRun()
    {
        if (!CanPlayerFastRun)
        {
            CanPlayerFastRun = true;
            Function.Call(Hash._SET_MOVE_SPEED_MULTIPLIER, Game.Player, 1.49f);
        }
        else
        {
            CanPlayerFastRun = false;
            Function.Call(Hash._SET_MOVE_SPEED_MULTIPLIER, Game.Player, 1.0f);
        }

        Sublime.DisplayMessage("Fast Run", CanPlayerFastRun);
    }

    internal static void ToggleFastSwim()
    {
        if (!CanPlayerFastSwim)
        {
            CanPlayerFastSwim = true;
            Function.Call(Hash._SET_SWIM_SPEED_MULTIPLIER, Game.Player, 1.49f);
        }
        else
        {
            CanPlayerFastSwim = false;
            Function.Call(Hash._SET_SWIM_SPEED_MULTIPLIER, Game.Player, 1.0f);
        }

        Sublime.DisplayMessage("Fast Swim", CanPlayerFastSwim);
    }

    internal static void ToggleNoNoise()
    {
        if (!IsPlayerNoiseless)
        {
            IsPlayerNoiseless = true;
        }
        else
        {
            IsPlayerNoiseless = false;
        }

        Sublime.DisplayMessage("No Noise", IsPlayerNoiseless);
    }

    internal static void ToggleUnlimitedStamina()
    {
        if (!HasPlayerUnlimitedStamina)
        {
            HasPlayerUnlimitedStamina = true;
        }
        else
        {
            HasPlayerUnlimitedStamina = false;
        }

        Sublime.DisplayMessage("Unlimited Stamina", HasPlayerUnlimitedStamina);
    }

    internal static void ToggleUnlimitedBreath()
    {
        if (!HasPlayerUnlimitedBreath)
        {
            HasPlayerUnlimitedBreath = true;
        }
        else
        {
            HasPlayerUnlimitedBreath = false;
            unsafe
            {
                float originalBreathValue = BreathValue;
                Function.Call(Hash.SET_PED_MAX_TIME_UNDERWATER, Game.Player.Character, &originalBreathValue);
            }
        }

        Sublime.DisplayMessage("Unlimited Breath", HasPlayerUnlimitedBreath);
    }

    internal static void ToggleUnlimitedAbility()
    {
        if (!HasPlayerUnlimitedAbility)
        {
            HasPlayerUnlimitedAbility = true;
        }
        else
        {
            HasPlayerUnlimitedAbility = false;
        }

        Sublime.DisplayMessage("Unlimited Special Ability", HasPlayerUnlimitedAbility);
    }
}

