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
    public static ListMenu WeaponSelectionsMenu;
    public static GTA.Menu WeaponComponentsMenu;

    #region Weapon Component Related Dictionaries and Lists
    Dictionary<string, string> weaponComponentItems = new Dictionary<string, string>()
    {
        {"SWAP_DEFAULT", "SWAP_DEFAULT"}, {"SWAP_MELEE_2H", "SWAP_MELEE_2H"}, {"RELOAD_DEFAULT", "RELOAD_DEFAULT"},
        {"RELOAD_LARGE", "RELOAD_LARGE"}, {"RELOAD_DEFAULT_WITH_EMPTIES", "RELOAD_DEFAULT_WITH_EMPTIES"}, {"RELOAD_LARGE_WITH_EMPTIES", "RELOAD_LARGE_WITH_EMPTIES"},
        {"RELOAD_ONLY_AIM", "RELOAD_ONLY_AIM"}, {"RELOAD_LARGE_ONLY_AIM", "RELOAD_LARGE_ONLY_AIM"}, {"RELOAD_DEFAULT_BOTH_SIDES", "RELOAD_DEFAULT_BOTH_SIDES"},
        {"COMPONENT_AT_RAILCOVER_01", "COMPONENT_AT_RAILCOVER_01"}, {"COMPONENT_AT_AR_AFGRIP", "Grip"}, {"COMPONENT_AT_PI_FLSH", "Flashlight"},
        {"COMPONENT_AT_AR_FLSH", "Flashlight"}, {"POLICE_TORCH_FLASHLIGHT", "POLICE_TORCH_FLASHLIGHT"}, {"COMPONENT_AT_SCOPE_MACRO", "Scope"}, 
        {"COMPONENT_AT_SCOPE_MACRO_02", "Scope"}, {"COMPONENT_AT_SCOPE_SMALL", "Scope"}, {"COMPONENT_AT_SCOPE_SMALL_02", "Scope"}, 
        {"COMPONENT_AT_SCOPE_MEDIUM", "Scope"}, {"COMPONENT_AT_SCOPE_LARGE", "Scope"}, {"COMPONENT_AT_SCOPE_MAX", "Advanced Scope"}, 
        {"COMPONENT_AT_PI_SUPP", "Supressor"}, {"COMPONENT_AT_PI_SUPP_02", "Supressor"}, {"COMPONENT_AT_AR_SUPP", "Supressor"}, 
        {"COMPONENT_AT_AR_SUPP_02", "Supressor"}, {"COMPONENT_AT_SR_SUPP", "Supressor"}, {"COMPONENT_AT_SR_SUPP_02", "Supressor"}, 
        {"COMPONENT_PISTOL_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_PISTOL_CLIP_02", "Extended Clip-Size"}, {"COMPONENT_COMBATPISTOL_CLIP_01", "Normal Clip-Size"}, 
        {"COMPONENT_COMBATPISTOL_CLIP_02", "Extended Clip-Size"}, {"COMPONENT_APPISTOL_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_APPISTOL_CLIP_02", "Extended Clip-Size"}, 
        {"COMPONENT_MICROSMG_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_MICROSMG_CLIP_02", "Extended Clip-Size"}, {"COMPONENT_SMG_CLIP_01", "Normal Clip-Size"}, 
        {"COMPONENT_SMG_CLIP_02", "Extended Clip-Size"}, {"COMPONENT_ASSAULTRIFLE_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_ASSAULTRIFLE_CLIP_02", "Extended Clip-Size"}, 
        {"COMPONENT_CARBINERIFLE_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_CARBINERIFLE_CLIP_02", "Extended Clip-Size"}, {"COMPONENT_ADVANCEDRIFLE_CLIP_01", "Normal Clip-Size"}, 
        {"COMPONENT_ADVANCEDRIFLE_CLIP_02", "Extended Clip-Size"}, {"COMPONENT_MG_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_MG_CLIP_02", "Extended Clip-Size"}, 
        {"COMPONENT_COMBATMG_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_COMBATMG_CLIP_02", "Extended Clip-Size"}, {"COMPONENT_ASSAULTSHOTGUN_CLIP_01", "Normal Clip-Size"}, 
        {"COMPONENT_ASSAULTSHOTGUN_CLIP_02", "Extended Clip-Size"}, {"COMPONENT_PISTOL50_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_PISTOL50_CLIP_02", "Extended Clip-Size"}, 
        {"COMPONENT_ASSAULTSMG_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_ASSAULTSMG_CLIP_02", "Extended Clip-Size"}, {"COMPONENT_HEAVYPISTOL_CLIP_01", "Normal Clip-Size"}, 
        {"COMPONENT_HEAVYPISTOL_CLIP_02", "Extended Clip-Size"}, {"COMPONENT_SPECIALCARBINE_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_SPECIALCARBINE_CLIP_02", "Extended Clip-Siz"}, 
        {"COMPONENT_AT_SCOPE_LARGE_FIXED_ZOOM", "Scope"}, {"COMPONENT_MARKSMANRIFLE_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_MARKSMANRIFLE_CLIP_02", "Extended Clip-Size"}, 
        {"COMPONENT_HEAVYSHOTGUN_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_HEAVYSHOTGUN_CLIP_02", "Extended Clip-Size"}, {"COMPONENT_COMBATPDW_CLIP_01", "Normal Clip-Size"}, 
        {"COMPONENT_COMBATPDW_CLIP_02", "Extended Clip-Size"}, {"COMPONENT_SNSPISTOL_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_SNSPISTOL_CLIP_02", "Extended Clip-Size"}, 
        {"COMPONENT_BULLPUPRIFLE_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_BULLPUPRIFLE_CLIP_02", "Extended Clip-Size"}, {"COMPONENT_VINTAGEPISTOL_CLIP_01", "Normal Clip-Size"}, 
        {"COMPONENT_VINTAGEPISTOL_CLIP_02", "Extended Clip-Size"}, {"COMPONENT_GUSENBERG_CLIP_01", "Normal Clip-Size"}, {"COMPONENT_GUSENBERG_CLIP_02", "Extended Clip-Size"},
    };

    Dictionary<string, int> weaponTintIndexDict = new Dictionary<string, int>()
    {
        {"Normal", 0}, {"Green", 1}, {"Gold", 2}, {"Pink", 3}, {"Army", 4}, {"LSPD", 5},
        {"Orange", 6}, {"Platinum", 7}
    };

    List<WeaponHash> weaponsNoComponentList = new List<WeaponHash>()
    {
        WeaponHash.Bat, WeaponHash.BZGas, WeaponHash.Crowbar, 
        WeaponHash.Dagger, WeaponHash.FireExtinguisher, WeaponHash.Firework,
        WeaponHash.GolfClub, WeaponHash.Grenade, WeaponHash.Hammer,
        WeaponHash.Hatchet, WeaponHash.HomingLauncher, WeaponHash.Knife,
        WeaponHash.Minigun, WeaponHash.Molotov, WeaponHash.Musket,
        WeaponHash.Nightstick, WeaponHash.PetrolCan, WeaponHash.ProximityMine,
        WeaponHash.Railgun, WeaponHash.RPG, WeaponHash.SawnOffShotgun,
        WeaponHash.SmokeGrenade, WeaponHash.Snowball, WeaponHash.StickyBomb,
        WeaponHash.StunGun, WeaponHash.Unarmed
    };
    #endregion

    private void SublimeWeaponMenu()
    {
        List<IMenuItem> weaponMenuItems = new List<IMenuItem>();

        var buttonWeaponComponents = new MenuButton("Weapon Components Menu");
        buttonWeaponComponents.Activated += (sender, args) => SublimeWeaponSelectionMenu();
        weaponMenuItems.Add(buttonWeaponComponents);

        var buttonGiveAllWeapons = new MenuButton("Give All Weapons");
        buttonGiveAllWeapons.Activated += (sender, args) => WeaponFunctions.GiveAllWeapons();
        weaponMenuItems.Add(buttonGiveAllWeapons);

        var toggleInfiniteAmmo = new MenuToggle("Infinite Ammunition", "", WeaponFunctions.HasWeaponInfiniteAmmo);
        toggleInfiniteAmmo.Changed += (sender, args) => WeaponFunctions.ToggleInfiniteAmmo();
        weaponMenuItems.Add(toggleInfiniteAmmo);

        var toggleFireBullets = new MenuToggle("Fire Bullets", "", WeaponFunctions.HasWeaponFireBullets);
        toggleFireBullets.Changed += (sender, args) => WeaponFunctions.ToggleFireBullets();
        weaponMenuItems.Add(toggleFireBullets);

        var toggleExplosiveBullets = new MenuToggle("Explosive Bullets", "", WeaponFunctions.HasWeaponExplosiveBullets);
        toggleExplosiveBullets.Changed += (sender, args) => WeaponFunctions.ToggleExplosiveBullets();
        weaponMenuItems.Add(toggleExplosiveBullets);

        var toggleExplosiveMelee = new MenuToggle("Explosive Melee", "", WeaponFunctions.IsExplosiveMeleeEnabled);
        toggleExplosiveMelee.Changed += (sender, args) => WeaponFunctions.ToggleExplosiveMelee();
        weaponMenuItems.Add(toggleExplosiveMelee);

        var toggleOneHitKill = new MenuToggle("One-Hit Kill", "", WeaponFunctions.IsOneHitKillEnabled);
        toggleOneHitKill.Changed += (sender, args) => WeaponFunctions.ToggleOneHitKill();
        weaponMenuItems.Add(toggleOneHitKill);

        var toggleGravityGun = new MenuToggle("Gravity Gun", "", WeaponFunctions.IsGravityGunEnabled);
        toggleGravityGun.Changed += (sender, args) => WeaponFunctions.ToggleGravityGun();
        weaponMenuItems.Add(toggleGravityGun);

        WeaponMenu = new GTA.Menu("Weapon Options", weaponMenuItems.ToArray());
        DrawMenu(WeaponMenu);
    }

    private void SublimeWeaponSelectionMenu()
    {
        List<MenuButton> weaponButtons = new List<MenuButton>();

        foreach (WeaponHash weaponHash in Enum.GetValues(typeof(WeaponHash)))
        {
            bool hasPedGotWeapon = Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, Game.Player.Character, (int) weaponHash, false);

            if (hasPedGotWeapon && !weaponsNoComponentList.Contains(weaponHash))
            {
                var buttonWeaponComponentMenu = new MenuButton(weaponHash.ToString());
                buttonWeaponComponentMenu.Activated += (sender, args) => SublimeWeaponComponentMenu(buttonWeaponComponentMenu, weaponHash);
                weaponButtons.Add(buttonWeaponComponentMenu);
            }       
        }
        
        WeaponSelectionsMenu = new ListMenu("Weapon Components", weaponButtons.OrderBy(w => w.Caption).ToArray(), 20);
        DrawMenu(WeaponSelectionsMenu);
    }
    private void SublimeWeaponComponentMenu(MenuButton buttonSender, WeaponHash weaponHash)
    {
        List<IMenuItem> componentButtons = new List<IMenuItem>();

        foreach (KeyValuePair<string, string> weaponComponent in weaponComponentItems)
        {
            int weaponComponentHash = Function.Call<int>(Hash.GET_HASH_KEY, weaponComponent.Key);
            bool canWeaponHaveComponent = Function.Call<bool>(Hash._CAN_WEAPON_HAVE_COMPONENT, (int) weaponHash, weaponComponentHash);
            if (canWeaponHaveComponent)
            {
                var componentActivateButton = new MenuButton(weaponComponent.Value);
                componentActivateButton.Activated += (sender, args) => WeaponFunctions.ChangeWeaponComponent(weaponHash, weaponComponent);
                componentButtons.Add(componentActivateButton);
            }
        }

        foreach (KeyValuePair<string, int> tintIndex in weaponTintIndexDict)
        {
            var tintActivateButton = new MenuButton(tintIndex.Key);
            tintActivateButton.Activated += (sender, args) => WeaponFunctions.ChangeWeaponTint(weaponHash, tintIndex.Value);
            componentButtons.Add(tintActivateButton);
        }

        WeaponComponentsMenu = new GTA.Menu(buttonSender.Caption, componentButtons.ToArray());
        DrawMenu(WeaponComponentsMenu);
    }

}

