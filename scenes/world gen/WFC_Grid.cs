using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class WFC_Grid : TileMap
{
	private Vector2I gridSize = new Vector2I(150, 80);
	private List<string> allPossibleTypes = new List<string> { "grass", "sand", "water" };
	private Dictionary<string, Vector2I> typeToTileCoords = new Dictionary<string, Vector2I>
	{
		{"grass", new Vector2I(2, 2)},
		{"water", new Vector2I(3, 1)},
		{"sand", new Vector2I(1, 3)},
		{"snow", new Vector2I(0, 0)},
	};
	private Dictionary<string, Vector2I> typeOfTreeTiles = new Dictionary<string, Vector2I>
	{
		{"snowy_tree", new Vector2I(2, 3)}
	};
	private WFC_Cell[,] grid;
	private Random random = new Random();

	// noise used to generate world sections
	//used to generate ocean and continent landmass
	// private FastNoiseLite oceanAltitude;
	// //used to generate biomes based on air moisture
	// private FastNoiseLite moisture;
	// //used to generate biomes based on temperature
	// private FastNoiseLite temperature;
	// //used to generate clumps of objects (like forests)
	// private FastNoiseLite objectClumps;
	private Noise_Map noiseMap;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		noiseMap = new Noise_Map();
		GenerateWorld();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void GenerateWorld()
	{
		// InitializeNoise();
		InitializeGrid();
		PerformWaveFunctionCollapse();
		OutputToTileMap();
	}

	// private void InitializeNoise()
	// {
	// 	oceanAltitude = new FastNoiseLite();
	// 	oceanAltitude.Seed = 1337; //(int)DateTime.Now.Ticks;//rand number
	// 	oceanAltitude.Frequency = 0.033f;
	// 	oceanAltitude.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
	// 	oceanAltitude.FractalType = FastNoiseLite.FractalTypeEnum.None;

	// 	moisture = new FastNoiseLite();
	// 	moisture.Seed = (int)DateTime.Now.Ticks;//rand number
	// 	moisture.Frequency = 0.033f;
	// 	moisture.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
	// 	moisture.FractalType = FastNoiseLite.FractalTypeEnum.None;

	// 	temperature = new FastNoiseLite();
	// 	temperature.Seed = (int)DateTime.Now.Ticks;//rand number
	// 	temperature.Frequency = 0.033f;
	// 	temperature.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
	// 	temperature.FractalType = FastNoiseLite.FractalTypeEnum.None;

	// 	objectClumps = new FastNoiseLite();
	// 	objectClumps.Seed = (int)DateTime.Now.Ticks;//rand number
	// 	objectClumps.Frequency = 0.033f;
	// 	objectClumps.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
	// 	objectClumps.FractalType = FastNoiseLite.FractalTypeEnum.None;
	// }

	private void InitializeGrid()
	{
		grid = new WFC_Cell[(int)gridSize.X, (int)gridSize.Y];
		for (int x = 0; x < gridSize.X; x++)
		{
			for (int y = 0; y < gridSize.Y; y++)
			{
				float oceanAlt = noiseMap.GetOceanAltitude(x, y);//oceanAltitude.GetNoise2D(x, y);
				float moist = noiseMap.GetMoisture(x, y);//moisture.GetNoise2D(x, y);
				float temp = noiseMap.GetTemperature(x, y);//temperature.GetNoise2D(x, y);
				float objectClump = noiseMap.GetObjectClumps(x, y);//for now, to be used to generate forests

				List<string> possibleTypes = GetInitialPossibleTypes(oceanAlt, moist, temp, objectClump);
				grid[x, y] = new WFC_Cell(possibleTypes, new Vector2I(x, y));
			}
		}
	}

	//look at setting down tiles based on noise levels
	private List<string> GetInitialPossibleTypes(float oceanAlt, float moist, float temp, float objectClump)
	{
		// oceans
		if (oceanAlt < 0) return new List<string> { "water" };
		// beach between oceans / lakes and other biomes
		else if (oceanAlt < 0.3) return new List<string> { "sand" };
		else{
			// frozen biome
			string currentBiome = getBiome(moist, temp);

			if(currentBiome == "snow"){

				return new List<string> { "snow" };
			}
			else return new List<string> { "grass" };
		}
	}

	private string getBiome(float moist, float temp)
	{
		string currentBiome = "";

		// frozen biome
		if(moist < 0 && temp < 0){
			currentBiome = "snow";
		}
		else currentBiome = "grass";

		return currentBiome;
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

	private bool PropagateConstraints(WFC_Cell cell)
	{
		// Define the adjacency rules
		Dictionary<string, List<string>> adjacencyRules = new Dictionary<string, List<string>>
		{
			{"water", new List<string> {"water", "sand"}}, // Water can only be next to sand or water
			{"sand", new List<string> {"sand", "water", "grass"}}, // Sand can be next to sand, water, or grass
			{"grass", new List<string> {"grass", "sand"}}, // Grass can only be next to grass or sand
			{"snow", new List<string> {"snow", "grass"}}, // Snow can only be next to grass
		};

		var neighbors = GetNeighbors(cell.Position);
		foreach (var neighbor in neighbors)
		{
			if (!neighbor.IsCollapsed)
			{
				var allowedNeighborTypes = adjacencyRules[cell.PossibleTypes[0]];
				var newPossibleTypes = neighbor.PossibleTypes.Intersect(allowedNeighborTypes).ToList();
				if (newPossibleTypes.Count == 0)
				{
					return false; // Contradiction found
				}
				neighbor.PossibleTypes = newPossibleTypes;
			}
		}
		return true; // No contradiction found
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

	private bool PerformWaveFunctionCollapse()
	{
		while (true)
		{
			WFC_Cell cell = SelectCellWithLeastEntropy();
			if (cell == null) return true; // All cells are collapsed

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
						SetCell(0, cellCoord, 0, tileCoords);

						//with this code, the trees will 100% show up on each snow tile; use the noiseMap objectClumps noise to create more natural (hopefully) forests
						if(tileType == "snow" && noiseMap.GetObjectClumps(x, y) < -0.5)
						{
							SetCell(1, cellCoord, 1, typeOfTreeTiles["snowy_tree"]);
						}
					}
				}
			}
		}
	}
}
