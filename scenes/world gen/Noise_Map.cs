using Godot;
using System;

/// <summary>
/// The Noise_map class encapsulates the creation and management of noise maps for various aspects of terrain and object (forest of trees, etc.) generation.
/// It provides methods to generate coherent noise patterns for simulating different environmental factors such as altitude, moisture, temperature, and objectCLumps.
/// Each noise map uses FastNoiseLite and can be configured with unique seed values and parameters for frequency, noise type, and fractal type.
/// This allows for the generation of diverse and complex landscapes necessary for biome differentiation and other terrain features.
/// </summary>
public partial class Noise_Map : Node
{
	//set up the getter/setters for various types of noise (ocean+landmass, biome, object clumping like forests)
	public FastNoiseLite OceanAltitude { get; private set; }
    public FastNoiseLite Moisture { get; private set; }
    public FastNoiseLite Temperature { get; private set; }
	public FastNoiseLite ObjectClumps { get; private set; }

	public Noise_Map()
    {
        InitializeNoise();
    }

	//go through initialization process; choose noise types, seeds, etc.
    private void InitializeNoise()
	{
		OceanAltitude = new FastNoiseLite();
		OceanAltitude.Seed = 1337; //(int)DateTime.Now.Ticks;//rand number
		OceanAltitude.Frequency = 0.033f;
		OceanAltitude.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
		OceanAltitude.FractalType = FastNoiseLite.FractalTypeEnum.None;

		Moisture = new FastNoiseLite();
		Moisture.Seed = (int)DateTime.Now.Ticks;//rand number
		Moisture.Frequency = 0.033f;
		Moisture.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
		Moisture.FractalType = FastNoiseLite.FractalTypeEnum.None;

		Temperature = new FastNoiseLite();
		Temperature.Seed = (int)DateTime.Now.Ticks;//rand number
		Temperature.Frequency = 0.033f;
		Temperature.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
		Temperature.FractalType = FastNoiseLite.FractalTypeEnum.None;

		ObjectClumps = new FastNoiseLite();
		ObjectClumps.Seed = (int)DateTime.Now.Ticks;//rand number
		ObjectClumps.Frequency = 0.033f;
		ObjectClumps.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
		ObjectClumps.FractalType = FastNoiseLite.FractalTypeEnum.None;
	}

	// Additional methods to get noise values for specific coordinates
    public float GetOceanAltitude(int x, int y) => OceanAltitude.GetNoise2D(x, y);
    public float GetMoisture(int x, int y) => Moisture.GetNoise2D(x, y);
    public float GetTemperature(int x, int y) => Temperature.GetNoise2D(x, y);

	public float GetObjectClumps(int x, int y) => ObjectClumps.GetNoise2D(x, y);
}
