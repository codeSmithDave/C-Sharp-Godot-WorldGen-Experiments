using Godot;
using System;

public partial class ResourceCharacter : Resource
{
    private ResourceStats resStats;
    private ResourceCultivate resCultivation;
    public ResourceCultivate ResCultivation { get; private set; }
    public ResourceStats ResStats { get; private set; }
//     public ResourceStats ResStats
//     {
//             get{ return resStats; }
//             set{ resStats = value; }
//     }
//     public ResourceCultivate ResCultivation
//     {
//             get{ return resCultivation; }
//             set{ resCultivation = value; }
//     }

    public ResourceCharacter()
    {
        ResStats = resStats = new ResourceStats();
        ResCultivation = resCultivation = new ResourceCultivate(ResourceCultivate.MainLevel.QiCondensation, ResourceCultivate.SubLevel.Early);
        // resCultivation.MainLevels - WORKS
        // GD.Print("", ResourceCultivate.MainLevel.QiCondensation);
        // GD.Print("experience = ", resCultivation.);
    }
}
