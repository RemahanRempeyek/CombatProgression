using System;
using System.Linq;
using System.Reflection;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buffs;
using StardewValley.GameData.Buffs;
using GenericModConfigMenu;

namespace CombatProgression;

public class ModEntry : Mod
{
    private const string ModDataKey = "YolBukik.CombatProgression";
    private const string BaseHealthKey = "BaseMaxHealth";
    private const string LevelKey = "AppliedCombatLevel";

    private const string DefenseBuffId =
        "YolBukik.CombatProgression.Defense";

    private ModConfig Config = new();

    public override void Entry(IModHelper helper)
    {
        Monitor.Log(
            "CombatProgression loaded successfully!",
            LogLevel.Info
        );

        Config = helper.ReadConfig<ModConfig>();

        helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        helper.Events.GameLoop.DayStarted += OnDayStarted;
        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
    }

    private void OnGameLaunched(
        object? sender,
        GameLaunchedEventArgs e)
    {
        var gmcm =
            Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>(
                "spacechase0.GenericModConfigMenu"
            );

        if (gmcm is null)
            return;

        gmcm.Register(
            ModManifest,

            reset: () =>
            {
                Config = new ModConfig();
            },

            save: () =>
            {
                Helper.WriteConfig(Config);

                if (Context.IsWorldReady)
                    ApplyProgression();
            }
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => "Max HP per Combat Level",
            tooltip: () =>
                "Bonus Max HP yang didapat setiap Combat Level.",
            getValue: () =>
                Config.MaxHealthPercentPerLevel,
            setValue: value =>
                Config.MaxHealthPercentPerLevel = value,
            min: 0,
            max: 10,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => "Defense per Combat Level",
            tooltip: () =>
                "Bonus Defense berdasarkan Combat Level.",
            getValue: () =>
                Config.DefensePercentPerLevel,
            setValue: value =>
                Config.DefensePercentPerLevel = value,
            min: 0,
            max: 20,
            interval: 1
        );
    }

    private void OnSaveLoaded(
        object? sender,
        SaveLoadedEventArgs e)
    {
        ApplyProgression();
    }

    private void OnDayStarted(
        object? sender,
        DayStartedEventArgs e)
    {
        ApplyProgression();
    }

    private void ApplyProgression()
    {
        Farmer player = Game1.player;

        string baseHealthKey =
            $"{ModDataKey}/{BaseHealthKey}";

        string levelKey =
            $"{ModDataKey}/{LevelKey}";

        int combatLevel = player.CombatLevel;

        int previousLevel = combatLevel;

        if (player.modData.TryGetValue(
            levelKey,
            out string? savedLevel)
            && int.TryParse(
                savedLevel,
                out int parsedLevel))
        {
            previousLevel = parsedLevel;
        }

        // ==================================================
        // MAX HEALTH
        // ==================================================

        int baseMaxHealth;

        if (player.modData.TryGetValue(
            baseHealthKey,
            out string? storedHealth)
            && int.TryParse(
                storedHealth,
                out int savedHealth))
        {
            baseMaxHealth = savedHealth;
        }
        else
        {
            baseMaxHealth = player.maxHealth;
        }

        if (combatLevel != previousLevel)
        {
            double oldHealthPercent =
                previousLevel *
                (Config.MaxHealthPercentPerLevel / 100.0);

            int oldBonusHealth =
                (int)Math.Round(
                    baseMaxHealth * oldHealthPercent
                );

            baseMaxHealth =
                player.maxHealth - oldBonusHealth;
        }

        double healthPercent =
            combatLevel *
            (Config.MaxHealthPercentPerLevel / 100.0);

        int bonusHealth =
            (int)Math.Round(
                baseMaxHealth * healthPercent
            );

        player.maxHealth =
            baseMaxHealth + bonusHealth;

        if (player.health > player.maxHealth)
            player.health = player.maxHealth;

        player.modData[baseHealthKey] =
            baseMaxHealth.ToString();

        // ==================================================
        // DEFENSE
        // ==================================================

        ApplyDefenseBuff(player, combatLevel);

        player.modData[levelKey] =
            combatLevel.ToString();

        Monitor.Log(
            $"Combat Level {combatLevel}: " +
            $"Max HP bonus = {bonusHealth}, " +
            $"Defense bonus = {GetDefenseBonus(combatLevel)}",
            LogLevel.Trace
        );
    }

    private void ApplyDefenseBuff(
        Farmer player,
        int combatLevel)
    {
        int defenseBonus =
            GetDefenseBonus(combatLevel);

        if (defenseBonus <= 0)
            return;

        var data = new BuffAttributesData
        {
            Defense = defenseBonus
        };

        var effects = new BuffEffects();
        effects.Add(data);

        Type buffType = typeof(Buff);

        ConstructorInfo? constructor =
            buffType
                .GetConstructors(
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance)
                .FirstOrDefault();

        if (constructor is null)
        {
            Monitor.Log(
                "Buff constructor tidak ditemukan.",
                LogLevel.Error
            );

            return;
        }

        object?[] args =
        {
            DefenseBuffId,
            "Combat Progression",
            $"Defense +{defenseBonus}",
            -1,
            null,
            0,
            effects,
            false,
            "Combat Progression",
            "Combat Progression"
        };

        try
        {
            Buff? buff =
                constructor.Invoke(args) as Buff;

            if (buff is null)
            {
                Monitor.Log(
                    "Gagal membuat Defense Buff.",
                    LogLevel.Error
                );

                return;
            }

            player.buffs.Apply(buff);
        }
        catch (Exception ex)
        {
            Monitor.Log(
                $"Gagal menerapkan Defense Buff: {ex}",
                LogLevel.Error
            );
        }
    }

    private int GetDefenseBonus(int combatLevel)
    {
        if (combatLevel <= 0)
            return 0;

        double bonus =
            combatLevel *
            (Config.DefensePercentPerLevel / 100.0);

        return (int)Math.Round(
            Farmer.defender * bonus
        );
    }
}
