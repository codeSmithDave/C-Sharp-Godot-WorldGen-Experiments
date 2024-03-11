using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class ResourceStats : Resource
{
	// Enumerations to represent different types of stats and modifiers for a resource.
	public enum StatType { Health, Mana, Strength, Intelligence, Agility }
	public enum ModifierType { Temporary, Equipment }

	// A dictionary to hold base values for each stat type.
	private Dictionary<StatType, int> baseStats = new Dictionary<StatType, int>();
	// A dictionary to hold lists of modifiers for each stat type.
	private Dictionary<StatType, List<StatModifier>> modifiers = new Dictionary<StatType, List<StatModifier>>();

	// Constructor for the ResourceStats class.
	// It initializes base stats and their corresponding modifiers for all stat types.
	public ResourceStats()
	{
		foreach (StatType stat in System.Enum.GetValues(typeof(StatType)))
		{
			baseStats[stat] = 0; // Initialize base stats to 0.
			modifiers[stat] = new List<StatModifier>(); // Initialize the modifier list.
		}
	}

	// Sets the base value of a specific stat type.
	public void SetBaseStat(StatType stat, int value)
	{
		baseStats[stat] = value;
	}

	// Retrieves the base value of a specific stat type.
	// If the stat doesn't exist in the dictionary, it returns 0.
	public int GetBaseStat(StatType stat)
	{
		return baseStats.ContainsKey(stat) ? baseStats[stat] : 0;
	}

	// Adds a new modifier to the list of modifiers for a specific stat type.
	public void AddModifier(StatType stat, StatModifier modifier)
	{
		modifiers[stat].Add(modifier);
	}

	// Calculates the final value of a specific stat type.
	// It adds together the base stat value and all modifier values for the stat.
	public int CalculateFinalStat(StatType stat)
	{
		int finalValue = GetBaseStat(stat); // Start with the base stat value.
		foreach (var modifier in modifiers[stat]) // Add all the modifier values.
		{
			finalValue += modifier.Value;
		}
		return finalValue; // Return the final calculated value.
	}

	// Struct to represent a modifier with a type and value.
	public struct StatModifier
	{
		public ModifierType Type; // The type of modifier (e.g., Temporary, Equipment).
		public int Value; // The value of the modifier to be applied to the stat.

		// Constructor for the StatModifier struct.
		// Assigns the type and value to the new modifier instance.
		public StatModifier(ModifierType type, int value)
		{
			Type = type;
			Value = value;
		}
	}

}
