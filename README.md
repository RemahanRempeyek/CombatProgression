# Combat Progression

A Stardew Valley SMAPI mod that adds combat-based character progression.

## Features

- Increases Max Health based on Combat Level.
- Adds Defense based on Combat Level.
- Configurable through Generic Mod Config Menu (GMCM).

## Requirements

- Stardew Valley 1.6.15+
- SMAPI 4.5.2+
- Generic Mod Config Menu

## Configuration

By default, CombatProgression uses the following cumulative bonuses:

Combat Level| Max Health Bonus| Defense Bonus
1| +20| +1
2| +35| +2
3| +50| +3
4| +70| +4
5| +100| +5
6| +140| +6
7| +200| +7
8| +280| +8
9| +380| +9
10| +500| +10

These values are cumulative bonuses for the current Combat Level. They are calculated from the player's True Base HP and do not stack the previous level's bonus again.

All values can be customized through Generic Mod Config Menu.

## Development

This repository contains the source code of the mod.

Stardew Valley and SMAPI DLL files are not included in this repository.
