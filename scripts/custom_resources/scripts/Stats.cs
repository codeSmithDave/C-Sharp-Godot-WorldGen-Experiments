using Godot;

[GlobalClass, Icon("res://Stats/StatsIcon.svg")]
public partial class Stats : Resource
{
	[Export]
	public int Strength { get; set; }

	[Export]
	public int Defense { get; set; }

	[Export]
	public int Speed { get; set; }
}
