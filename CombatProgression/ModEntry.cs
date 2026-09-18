using System;
using System.Linq;
using System.Reflection;
using GenericModConfigMenu;
using StardewModdingAPI;
using StardewModdingAPI.Enums;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buffs;
using StardewValley.GameData.Buffs;

namespace CombatProgression;

public sealed class ModEntry : Mod
{
    private const string ModDataKey =
        "YolBukik.CombatProgression";

    private const string BaseHealthKey =
        ModDataKey + "/BaseMaxHealth";

    private const string LevelKey =
        ModDataKey + "/AppliedCombatLevel";

    private const string DefenseBuffId =
        ModDataKey + ".Defense";

    private ModConfig Config = new();

    public override void Entry(IModHelper helper)
    {
        Config = helper.ReadConfig<ModConfig>();

        I18n.Init(helper.Translation);

        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        helper.Events.GameLoop.DayStarted += OnDayStarted;
        helper.Events.Player.LevelChanged += OnLevelChanged;
    }

    private void OnGameLaunched(
        object? sender,
        GameLaunchedEventArgs e)
    {
        var gmcm =
            Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>(
                "spacechase0.GenericModConfigMenu"
            );

        if (gmcm == null)
        {
            Monitor.Log(
                "Generic Mod Config Menu was not found.",
                LogLevel.Warn
            );

            return;
        }

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

        gmcm.AddSectionTitle(
            ModManifest,
            text: () =>
                I18n.Get("config.max_health_section")
        );

        gmcm.AddParagraph(
            ModManifest,
            text: () =>
                I18n.Get("config.max_health_description")
        );

        AddHealthOptions(gmcm);

        gmcm.AddSectionTitle(
            ModManifest,
            text: () =>
                I18n.Get("config.defense_section")
        );

        gmcm.AddParagraph(
            ModManifest,
            text: () =>
                I18n.Get("config.defense_description")
        );

        AddDefenseOptions(gmcm);
    }

    private void AddHealthOptions(
        IGenericModConfigMenuApi gmcm)
    {
        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 1 }
            ),
            tooltip: () => I18n.Get(
                "config.max_health_tooltip",
                new { level = 1 }
            ),
            getValue: () => Config.MaxHealthBonusLevel1,
            setValue: value => Config.MaxHealthBonusLevel1 = value,
            min: 0,
            max: 1000,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 2 }
            ),
            tooltip: () => I18n.Get(
                "config.max_health_tooltip",
                new { level = 2 }
            ),
            getValue: () => Config.MaxHealthBonusLevel2,
            setValue: value => Config.MaxHealthBonusLevel2 = value,
            min: 0,
            max: 1000,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 3 }
            ),
            tooltip: () => I18n.Get(
                "config.max_health_tooltip",
                new { level = 3 }
            ),
            getValue: () => Config.MaxHealthBonusLevel3,
            setValue: value => Config.MaxHealthBonusLevel3 = value,
            min: 0,
            max: 1000,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 4 }
            ),
            tooltip: () => I18n.Get(
                "config.max_health_tooltip",
                new { level = 4 }
            ),
            getValue: () => Config.MaxHealthBonusLevel4,
            setValue: value => Config.MaxHealthBonusLevel4 = value,
            min: 0,
            max: 1000,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 5 }
            ),
            tooltip: () => I18n.Get(
                "config.max_health_tooltip",
                new { level = 5 }
            ),
            getValue: () => Config.MaxHealthBonusLevel5,
            setValue: value => Config.MaxHealthBonusLevel5 = value,
            min: 0,
            max: 1000,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 6 }
            ),
            tooltip: () => I18n.Get(
                "config.max_health_tooltip",
                new { level = 6 }
            ),
            getValue: () => Config.MaxHealthBonusLevel6,
            setValue: value => Config.MaxHealthBonusLevel6 = value,
            min: 0,
            max: 1000,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 7 }
            ),
            tooltip: () => I18n.Get(
                "config.max_health_tooltip",
                new { level = 7 }
            ),
            getValue: () => Config.MaxHealthBonusLevel7,
            setValue: value => Config.MaxHealthBonusLevel7 = value,
            min: 0,
            max: 1000,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 8 }
            ),
            tooltip: () => I18n.Get(
                "config.max_health_tooltip",
                new { level = 8 }
            ),
            getValue: () => Config.MaxHealthBonusLevel8,
            setValue: value => Config.MaxHealthBonusLevel8 = value,
            min: 0,
            max: 1000,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 9 }
            ),
            tooltip: () => I18n.Get(
                "config.max_health_tooltip",
                new { level = 9 }
            ),
            getValue: () => Config.MaxHealthBonusLevel9,
            setValue: value => Config.MaxHealthBonusLevel9 = value,
            min: 0,
            max: 1000,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 10 }
            ),
            tooltip: () => I18n.Get(
                "config.max_health_tooltip",
                new { level = 10 }
            ),
            getValue: () => Config.MaxHealthBonusLevel10,
            setValue: value => Config.MaxHealthBonusLevel10 = value,
            min: 0,
            max: 1000,
            interval: 1
        );
    }

    private void AddDefenseOptions(
        IGenericModConfigMenuApi gmcm)
    {
        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 1 }
            ),
            tooltip: () => I18n.Get(
                "config.defense_tooltip",
                new { level = 1 }
            ),
            getValue: () => Config.DefenseBonusLevel1,
            setValue: value => Config.DefenseBonusLevel1 = value,
            min: 0,
            max: 100,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 2 }
            ),
            tooltip: () => I18n.Get(
                "config.defense_tooltip",
                new { level = 2 }
            ),
            getValue: () => Config.DefenseBonusLevel2,
            setValue: value => Config.DefenseBonusLevel2 = value,
            min: 0,
            max: 100,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 3 }
            ),
            tooltip: () => I18n.Get(
                "config.defense_tooltip",
                new { level = 3 }
            ),
            getValue: () => Config.DefenseBonusLevel3,
            setValue: value => Config.DefenseBonusLevel3 = value,
            min: 0,
            max: 100,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 4 }
            ),
            tooltip: () => I18n.Get(
                "config.defense_tooltip",
                new { level = 4 }
            ),
            getValue: () => Config.DefenseBonusLevel4,
            setValue: value => Config.DefenseBonusLevel4 = value,
            min: 0,
            max: 100,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 5 }
            ),
            tooltip: () => I18n.Get(
                "config.defense_tooltip",
                new { level = 5 }
            ),
            getValue: () => Config.DefenseBonusLevel5,
            setValue: value => Config.DefenseBonusLevel5 = value,
            min: 0,
            max: 100,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 6 }
            ),
            tooltip: () => I18n.Get(
                "config.defense_tooltip",
                new { level = 6 }
            ),
            getValue: () => Config.DefenseBonusLevel6,
            setValue: value => Config.DefenseBonusLevel6 = value,
            min: 0,
            max: 100,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 7 }
            ),
            tooltip: () => I18n.Get(
                "config.defense_tooltip",
                new { level = 7 }
            ),
            getValue: () => Config.DefenseBonusLevel7,
            setValue: value => Config.DefenseBonusLevel7 = value,
            min: 0,
            max: 100,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 8 }
            ),
            tooltip: () => I18n.Get(
                "config.defense_tooltip",
                new { level = 8 }
            ),
            getValue: () => Config.DefenseBonusLevel8,
            setValue: value => Config.DefenseBonusLevel8 = value,
            min: 0,
            max: 100,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 9 }
            ),
            tooltip: () => I18n.Get(
                "config.defense_tooltip",
                new { level = 9 }
            ),
            getValue: () => Config.DefenseBonusLevel9,
            setValue: value => Config.DefenseBonusLevel9 = value,
            min: 0,
            max: 100,
            interval: 1
        );

        gmcm.AddNumberOption(
            ModManifest,
            name: () => I18n.Get(
                "config.combat_level",
                new { level = 10 }
            ),
            tooltip: () => I18n.Get(
                "config.defense_tooltip",
                new { level = 10 }
            ),
            getValue: () => Config.DefenseBonusLevel10,
            setValue: value => Config.DefenseBonusLevel10 = value,
            min: 0,
            max: 100,
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

    private void OnLevelChanged(
        object? sender,
        LevelChangedEventArgs e)
    {
        if (!Context.IsWorldReady)
            return;

        if (e.Skill != SkillType.Combat)
            return;

        Monitor.Log(
            $"Combat Level changed: " +
            $"{e.OldLevel} -> {e.NewLevel}.",
            LogLevel.Info
        );

        ApplyProgression();
    }

    private void ApplyProgression()
    {
        if (!Context.IsWorldReady)
            return;

        Farmer player = Game1.player;

        int combatLevel =
            player.GetUnmodifiedSkillLevel(4);

        int vanillaCombatHealth =
            GetVanillaCombatHealthBonus(
                player,
                combatLevel
            );

        int baseMaxHealth;

        if (player.modData.TryGetValue(
                BaseHealthKey,
                out string? storedBase)
            && int.TryParse(
                storedBase,
                out int parsedBase))
        {
            baseMaxHealth = parsedBase;
        }
        else
        {
            baseMaxHealth =
                Math.Max(
                    1,
                    player.maxHealth -
                    vanillaCombatHealth
                );

            player.modData[BaseHealthKey] =
                baseMaxHealth.ToString();

            Monitor.Log(
                $"Stored base Max HP: {baseMaxHealth}",
                LogLevel.Info
            );
        }

        int combatProgressionHealth =
            GetHealthBonus(combatLevel);

        int expectedMaxHealth =
            baseMaxHealth +
            vanillaCombatHealth +
            combatProgressionHealth;

        if (player.maxHealth != expectedMaxHealth)
        {
            int oldMaxHealth =
                player.maxHealth;

            player.maxHealth =
                expectedMaxHealth;

            Monitor.Log(
                $"Combat Level {combatLevel}: " +
                $"Max HP {oldMaxHealth} -> " +
                $"{expectedMaxHealth} " +
                $"(base {baseMaxHealth}, " +
                $"vanilla combat +{vanillaCombatHealth}, " +
                $"Combat Progression +{combatProgressionHealth}).",
                LogLevel.Info
            );
        }

        ApplyDefenseBuff(
            player,
            combatLevel
        );

        player.modData[LevelKey] =
            combatLevel.ToString();
    }

    private int GetHealthBonus(
        int combatLevel)
    {
        return combatLevel switch
        {
            1 => Config.MaxHealthBonusLevel1,
            2 => Config.MaxHealthBonusLevel2,
            3 => Config.MaxHealthBonusLevel3,
            4 => Config.MaxHealthBonusLevel4,
            5 => Config.MaxHealthBonusLevel5,
            6 => Config.MaxHealthBonusLevel6,
            7 => Config.MaxHealthBonusLevel7,
            8 => Config.MaxHealthBonusLevel8,
            9 => Config.MaxHealthBonusLevel9,
            10 => Config.MaxHealthBonusLevel10,
            _ => 0
        };
    }

    private int GetDefenseBonus(
        int combatLevel)
    {
        return combatLevel switch
        {
            1 => Config.DefenseBonusLevel1,
            2 => Config.DefenseBonusLevel2,
            3 => Config.DefenseBonusLevel3,
            4 => Config.DefenseBonusLevel4,
            5 => Config.DefenseBonusLevel5,
            6 => Config.DefenseBonusLevel6,
            7 => Config.DefenseBonusLevel7,
            8 => Config.DefenseBonusLevel8,
            9 => Config.DefenseBonusLevel9,
            10 => Config.DefenseBonusLevel10,
            _ => 0
        };
    }

    private int GetVanillaCombatHealthBonus(
        Farmer player,
        int combatLevel)
    {
        if (combatLevel <= 0)
            return 0;

        int bonus =
            combatLevel * 5;

        if (player.professions.Contains(Farmer.fighter))
            bonus += 15;

        if (player.professions.Contains(Farmer.defender))
            bonus += 25;

        return bonus;
    }

    private void ApplyDefenseBuff(
        Farmer player,
        int combatLevel)
    {
        if (player.buffs.AppliedBuffs.ContainsKey(
                DefenseBuffId))
        {
            player.buffs.Remove(
                DefenseBuffId
            );
        }

        int defenseBonus =
            GetDefenseBonus(combatLevel);

        if (defenseBonus <= 0)
            return;

        BuffAttributesData attributes =
            new BuffAttributesData
            {
                Defense = defenseBonus
            };

        BuffEffects effects =
            new BuffEffects(attributes);

        ConstructorInfo? constructor =
            typeof(Buff)
                .GetConstructors(
                    BindingFlags.Public |
                    BindingFlags.Instance
                )
                .FirstOrDefault(
                    c => c.GetParameters().Length == 10
                );

        if (constructor == null)
        {
            Monitor.Log(
                "Could not find the Stardew Buff constructor.",
                LogLevel.Error
            );

            return;
        }

        ParameterInfo[] parameters =
            constructor.GetParameters();

        object?[] args =
            new object?[parameters.Length];

        args[0] = DefenseBuffId;
        args[1] = ModManifest.Name;
        args[2] = "Defense";
        args[3] = 0;
        args[4] = null;
        args[5] = 0;
        args[6] = effects;
        args[7] = null;
        args[8] = null;
        args[9] = null;

        Buff buff =
            (Buff)constructor.Invoke(args);

        player.buffs.Apply(buff);
    }
}
