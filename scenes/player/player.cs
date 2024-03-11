using Godot;
using System;
// using System.Numerics;

public partial class player : CharacterBody2D
{
		// private ResourceCultivate ResCultivate;

	// TEST TEST TEST ResourceCharacter resource
	private ResourceCharacter ResCharacter;

	//get the cultivation class from ResourceCharacter
	// private ResourceCultivate cultivation;
	//get the stats class from the ResourceCharacter
	// private ResourceStats stats;
	
	// @export in C# is: [Export]
	public int speed { get; set; } = 1000;
	// public Cultivation cultivation;
	private double timeSinceLastExpGain = 0f;
	private double expGainInterval = 1.0f; // 1 second interval

	public override void _Ready()
	{	
			// ResCultivate = new ResourceCultivate(ResourceCultivate.MainLevel.QiCondensation, ResourceCultivate.SubLevel.Early);
		// cultivation = new Cultivation(Cultivation.MainLevel.QiCondensation, Cultivation.SubLevel.Early);
		// string displayName = Cultivation.GetEnumDescription(cultivation.CurrentMainLevel); //get the cultivation name / description (used in menus, etc.)


		ResCharacter = new ResourceCharacter();
		//set the player's initial / base stats
		ResCharacter.ResStats.SetBaseStat(ResourceStats.StatType.Health, 100);
		ResCharacter.ResStats.SetBaseStat(ResourceStats.StatType.Mana, 100);
		ResCharacter.ResStats.SetBaseStat(ResourceStats.StatType.Strength, 10);
		ResCharacter.ResStats.SetBaseStat(ResourceStats.StatType.Agility, 10);
		ResCharacter.ResStats.SetBaseStat(ResourceStats.StatType.Intelligence, 10);

		// cultivation = ResCharacter.GetCultivation();
		// stats = ResCharacter.GetStats();
		// GD.Print("", );
		// GD.Print("ResCharacter.ResCultivation.MainLevels = ", ResCharacter.GetCultivation.);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		//every second, add some exp to player
		timeSinceLastExpGain += delta;
		if (timeSinceLastExpGain >= expGainInterval)
		{
			timeSinceLastExpGain = 0f; // Reset the timer
			// cultivation.AddExperience(50); // Increment experience by 1 (or any other amount you prefer)
				// ResCultivate.AddExperience(50);
			ResCharacter.ResCultivation.AddExperience(50);
			// Optional: Update UI or other elements to reflect the new experience value
		}
	}

	public override void _PhysicsProcess(double _delta)
	{
		GetInput();
		MoveAndSlide();
	}

	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
		Velocity = inputDirection * speed;
	}
}
