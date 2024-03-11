using Godot;
using System;
using System.Collections.Generic;

public partial class TileMap : Godot.TileMap
{
	private Vector2I gridSize = new Vector2I(100, 50);
	private List<string> allPossibleTypes = new List<string> { "grass", "sand", "water" };
	private Dictionary<string, Vector2I> typeToTileCoords = new Dictionary<string, Vector2I>
	{
		{"grass", new Vector2I(2, 2)},
		{"water", new Vector2I(3, 1)},
		{"sand", new Vector2I(1, 3)}
	};
	private WFC_Cell[,] grid;
	private Random random = new Random();

	public override void _Ready()
	{
		GenerateWorld();
	}

	private void GenerateWorld()
	{
		InitializeGrid();
		PerformWaveFunctionCollapse();
		OutputToTileMap();
	}

	private void InitializeGrid()
	{
		grid = new WFC_Cell[(int)gridSize.X, (int)gridSize.Y];
		for (int x = 0; x < gridSize.X; x++)
		{
			for (int y = 0; y < gridSize.Y; y++)
			{
				var noiseValue = random.NextDouble(); // Placeholder for noise logic
				var possibleTypes = GetInitialPossibleTypes(noiseValue);
				grid[x, y] = new WFC_Cell(possibleTypes, new Vector2I(x, y));
			}
		}
	}

	private List<string> GetInitialPossibleTypes(double noiseValue)
	{
		// Placeholder logic for determining initial possible types
		if (noiseValue < -0.2) return new List<string> { "water" };
		else if (noiseValue < 0.2) return new List<string> { "sand" };
		else return new List<string> { "grass" };
	}

	private WFC_Cell SelectCellWithLeastEntropy()
	{
		int minEntropy = int.MaxValue;
		List<WFC_Cell> cellsWithLeastEntropy = new List<WFC_Cell>();

		for (int x = 0; x < gridSize.X; x++)
		{
			for (int y = 0; y < gridSize.Y; y++)
			{
				WFC_Cell cell = grid[x, y];
				if (!cell.IsCollapsed)
				{
					int entropy = cell.PossibleTypes.Count;
					if (entropy < minEntropy)
					{
						minEntropy = entropy;
						cellsWithLeastEntropy.Clear();
						cellsWithLeastEntropy.Add(cell);
					}
					else if (entropy == minEntropy)
					{
						cellsWithLeastEntropy.Add(cell);
					}
				}
			}
		}

		if (cellsWithLeastEntropy.Count == 0) return null; // All cells are collapsed
		int randomIndex = random.Next(cellsWithLeastEntropy.Count);
		return cellsWithLeastEntropy[randomIndex];
	}

	private void CollapseCell(WFC_Cell cell)
	{
		int randomIndex = random.Next(cell.PossibleTypes.Count);
		string chosenType = cell.PossibleTypes[randomIndex];
		cell.PossibleTypes = new List<string> { chosenType };
		cell.IsCollapsed = true;
	}

	private void PropagateConstraints(WFC_Cell cell)
	{
		// Placeholder for constraint propagation logic
	}

	private List<WFC_Cell> GetNeighbors(Vector2I position)
	{
		List<WFC_Cell> neighbors = new List<WFC_Cell>();
		var directions = new List<Vector2I>
		{
			new Vector2I(0, -1), // North
			new Vector2I(1, 0),  // East
			new Vector2I(0, 1),  // South
			new Vector2I(-1, 0)  // West
		};

		foreach (var direction in directions)
		{
			Vector2I neighborPos = position + direction;
			if (IsValidPosition(neighborPos))
			{
				neighbors.Add(grid[(int)neighborPos.X, (int)neighborPos.Y]);
			}
		}

		return neighbors;
	}

	private bool IsValidPosition(Vector2I position)
	{
		return position.X >= 0 && position.X < gridSize.X && position.Y >= 0 && position.Y < gridSize.Y;
	}

	private void PerformWaveFunctionCollapse()
	{
		while (true)
		{
			WFC_Cell cell = SelectCellWithLeastEntropy();
			if (cell == null) break; // All cells are collapsed

			CollapseCell(cell);
			PropagateConstraints(cell);
		}
	}

	private void OutputToTileMap()
	{
		for (int x = 0; x < gridSize.X; x++)
		{
			for (int y = 0; y < gridSize.Y; y++)
			{
				WFC_Cell cell = grid[x, y];
				if (cell.IsCollapsed && cell.PossibleTypes.Count > 0)
				{
					string tileType = cell.PossibleTypes[0];
					if (typeToTileCoords.TryGetValue(tileType, out Vector2I tileCoords))
					{
						Vector2I cellCoord = new Vector2I(x, y);
						//SetCell((int)tileCoords.X, (int)tileCoords.Y, 2); // Adjust as per your TileMap setup
						SetCell(0, cellCoord, 0, tileCoords); // Adjust as per your TileMap setup
					}
				}
			}
		}
	}
}
