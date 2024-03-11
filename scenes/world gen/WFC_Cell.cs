using Godot;
using System.Collections.Generic;

public class WFC_Cell
{
	public List<string> PossibleTypes { get; set; }
	public bool IsCollapsed { get; set; }
	public Vector2I Position { get; set; }

	public WFC_Cell(List<string> possibleTypes, Vector2I position)
	{
		PossibleTypes = new List<string>(possibleTypes);
		Position = position;
		IsCollapsed = false;
	}

	public override string ToString()
	{
		return $"Cell at {Position}, Collapsed: {IsCollapsed}, Types: {string.Join(", ", PossibleTypes)}";
	}
}
