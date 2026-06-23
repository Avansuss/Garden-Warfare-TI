using JetBrains.Annotations;
using UnityEngine;

public class ScoreData
{
    //P1
    // Surface areas in m2
    // Hardening
    public float AreaPond { get; set; } = -1f;
    public float AreaSwimmingPool { get; set; } = -1f;
    public float AreaPavement { get; set; } = -1f;

    // Permeability
    public float Gravel { get; set; } = -1f;
    public float PermeableTiles { get; set; } = -1f;

    // Not hardened without plants
    public float RootBarrierFabric { get; set; } = -1f;
    public float ArtificialGrass { get; set; } = -1f;
    public float Trampoline { get; set;  } = -1f;
    public float PlayGround { get; set; } = -1f;

    // Small green
    public float Flowers { get; set; } = -1f;
    public float TreeBark { get; set; } = -1f;
    public float VegetableGarden { get; set; } = -1f;

    // Shrubbery
    public float Shrub { get; set; } = -1f;
    public float Hedge { get; set; } = -1f;
    public float PickingGarden { get; set; } = -1f;

    // Grass and big trees are scored separately, as they have a higher score modifier than the other categories
    public float Grass { get; set; } = -1f;
    public float BigTree { get; set; } = -1f;
}

//P3
public class GardenAnimalsData
{
    public bool SpottedBeesAndButterflies { get; set; } = false;
    public bool SpottedBirds { get; set; } = false;
    public bool SpottedSpiders { get; set; } = false;
    public bool SpottedOtherAnimals { get; set; } = false;
}





