using Godot;
using System;
using System.ComponentModel;
using System.Reflection;

[GlobalClass]
public partial class ResourceCultivate : Resource
{
	public enum MainLevel
	{
		[Description("Qi Condensation")]
		QiCondensation,
		[Description("Foundation Establishment")]
		FoundationEstablishment,
		[Description("Golden Core")]
		GoldenCore,
		[Description("Nascent Soul")]
		NascentSoul,
		[Description("Spirit Transformation")]
		SpiritTransformation,
		[Description("Void Transcendence")]
		VoidTranscendence,
		[Description("Immortal Ascension")]
		ImmortalAscension,
		[Description("Immortal King")]
		ImmortalKing,
		[Description("Immortal Emperor")]
		ImmortalEmperor,
		[Description("Immortal Sage")]
		ImmortalSage
	}

	public enum SubLevel
	{
		Early,
		Mid,
		Late
	}

	public MainLevel CurrentMainLevel { get; private set; }
	public SubLevel CurrentSubLevel { get; private set; }
	public int Experience { get; private set; }

	// Constructor
	public ResourceCultivate(MainLevel mainLevel, SubLevel subLevel)
	{
		CurrentMainLevel = mainLevel;
		CurrentSubLevel = subLevel;
		Experience = 0;
	}

	// Add experience and handle level ups
	public void AddExperience(int amount)
	{
		Experience += amount;
		// GD.Print("exp = ", Experience);
		CheckForLevelUp();
		// GD.Print("current exp = ", Experience);
		// GD.Print("CurrentMainLevel = ", GetEnumDescription(CurrentMainLevel));
		// GD.Print("CurrentSubLevel = ", CurrentSubLevel);
	}

	private void CheckForLevelUp()
	{
		// Logic to determine if a level-up should occur
		// Update CurrentMainLevel and CurrentSubLevel accordingly

		// if MAIN levelup, change cultivation level
		// CurrentMainLevel = 

		// if SUB levelup,change sub level

		int requiredToLevelUp = CalculateExperienceForNextLevel();

		// GD.Print("requiredToLevelUp = ", requiredToLevelUp);

		if (Experience >= requiredToLevelUp)
		{
			levelUp();
		}
	}

	private int CalculateExperienceForNextLevel()
	{
		int baseExperience = 100; // Base experience required for the first level
		int levelIndex = (int)CurrentMainLevel; // Get the index of the current level

		return baseExperience * (levelIndex + 1); // Example formula
	}

	private void levelUp() {
		// Increase sub-level within the current main level
		if (CurrentSubLevel < SubLevel.Late) {
			CurrentSubLevel++;
		}
		// If the sub-level is at its max (Late), increase the main level
		else {
			int currentLevelIndex = (int)CurrentMainLevel;
			int nextLevelIndex = currentLevelIndex + 1;

			// Ensure that the next level index is within the bounds of the enum
			if (Enum.IsDefined(typeof(MainLevel), nextLevelIndex)) {
				CurrentMainLevel = (MainLevel)nextLevelIndex;
				CurrentSubLevel = SubLevel.Early;
			} else {
				// Handle max level scenario
			}
		}

		// GD.Print("CurrentMainLevel = ", CurrentMainLevel);
	}

	public static string GetEnumDescription(Enum value)
	{
		FieldInfo fi = value.GetType().GetField(value.ToString());
		DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

		if (attributes != null && attributes.Length > 0)
			return attributes[0].Description;
		else
			return value.ToString();
	}
}
