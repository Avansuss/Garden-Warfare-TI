using JetBrains.Annotations;
using UnityEngine;

public static class Score
{
    //P1
    // Surface areas in m2
    // Hardening
    public static float AreaPond { get; set; } = -1f;
    public static float AreaSwimmingPool { get; set; } = -1f;
    public static float AreaPavement { get; set; } = -1f;

    // Permeability
    public static float Gravel { get; set; } = -1f;
    public static float PermeableTiles { get; set; } = -1f;

    // Not hardened without plants
    public static float RootBarrierFabric { get; set; } = -1f;
    public static float ArtificialGrass { get; set; } = -1f;
    public static float Trampoline { get; set;  } = -1f;
    public static float PlayGround { get; set; } = -1f;

    // Small green
    public static float Flowers { get; set; } = -1f;
    public static float TreeBark { get; set; } = -1f;
    public static float VegetableGarden { get; set; } = -1f;

    // Shrubbery
    public static float Shrub { get; set; } = -1f;
    public static float Hedge { get; set; } = -1f;
    public static float PickingGarden { get; set; } = -1f;

    // Grass and big trees are scored separately, as they have a higher score modifier than the other categories
    public static float Grass { get; set; } = -1f;
    public static float BigTree { get; set; } = -1f;
}

public static class ScoreCalculate
{
    //input validation
    public static bool AllScoresFilled => Score.AreaPond >= 0 && Score.AreaSwimmingPool >= 0 && Score.AreaPavement >= 0 &&
                              Score.Gravel >= 0 && Score.PermeableTiles >= 0 &&
                              Score.RootBarrierFabric >= 0 && Score.ArtificialGrass >= 0 && Score.Trampoline >= 0 && Score.PlayGround >= 0 &&
                              Score.Flowers >= 0 && Score.TreeBark >= 0 && Score.VegetableGarden >= 0 &&
                              Score.Shrub >= 0 && Score.Hedge >= 0 && Score.PickingGarden >= 0 &&
                              Score.Grass >= 0 && Score.BigTree >= 0;

    //P1
    public static float HardeningScore => Score.AreaPond + Score.AreaSwimmingPool + Score.AreaPavement;
    public static float PermeabilityScore => Score.Gravel + Score.PermeableTiles;
    public static float NotHardenedWithoutPlantsScore => Score.RootBarrierFabric + Score.ArtificialGrass + Score.Trampoline + Score.PlayGround;
    public static float SmallGreenScore => Score.Flowers + Score.TreeBark + Score.VegetableGarden;
    public static float ShrubberyScore => Score.Shrub + Score.Hedge + Score.PickingGarden;
    public static float TotalSurfaceArea => HardeningScore + PermeabilityScore + NotHardenedWithoutPlantsScore + SmallGreenScore + ShrubberyScore + Score.Grass + Score.BigTree;

    //P2
    public static float VegetationAmount => SmallGreenScore + ShrubberyScore + Score.Grass + Score.BigTree;

    //P4
    public static int GetPlantDiversityModifier(int amount)
    {
        int returnAmount;

        switch (amount)
        {
            case <= 0:
                returnAmount = 0;
                break;
            case >= 1 and <= 3:
                returnAmount = 2;
                break;
            case >= 4 and <= 10:
                returnAmount = 5;
                break;
            case >= 11 and <= 25:
                returnAmount = 8;
                break;
            case >= 26:
                returnAmount = 12;
                break;
        }

        return returnAmount;
    }
}

public static class ScoreModifier
{
    //P1
    public readonly static float HardeningScoreModifier = 0f;
    public readonly static float PermeabilityScoreModifier = 2f;
    public readonly static float NotHardenedWithoutPlantsModifier = 3f;
    public readonly static float SmallGreenModifier = 3.5f;
    public readonly static float GrassModifier = 5f;
    public readonly static float ShrubberyModifier = 10f;
    public readonly static float BigTreeModifier = 15f;

    //P2 and P3
    public readonly static float Modifier = ScoreCalculate.VegetationAmount / ScoreCalculate.TotalSurfaceArea;

    //P3
    public readonly static float BeesAndButterfliesModifier = 2.5f;
    public readonly static float BirdsModifier = 2.5f;
    public readonly static float SpiderModifier = 2.5f;
    public readonly static float OtherAnimalsModifier = 2.5f;

    //P4
    public readonly static float diversityFlowersModifier = 1;
    public readonly static float diversityGrassModifier = 0.25f;
    public readonly static float diversityShrubModifier = 2;
    public readonly static float diversityBigTreeModifier = 3;
}

//P2
public static class Fertilizer
{
    public enum None
    {
        None = 7,
        Half = 2,
        Full = 0
    }

    public enum Artificial
    {
        None = 5,
        Half = 4,
        Full = 3
    }

    public enum Organic
    {
        None = 10,
        Half = 9,
        Full = 8
    }
}

//P3
public static class GardenAnimals
{
    public static bool SpottedBeesAndButterflies { get; set; } = false;
    public static bool SpottedBirds { get; set; } = false;
    public static bool SpottedSpiders { get; set; } = false;    
    public static bool SpottedOtherAnimals { get; set; } = false;
}



