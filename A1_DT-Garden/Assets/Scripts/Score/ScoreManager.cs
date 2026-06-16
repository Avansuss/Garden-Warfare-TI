using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //P1
    public float SoilWater()
    {
        float surfaceAreaWaterValue = ScoreCalculate.HardeningScore * ScoreModifier.HardeningScoreModifier +
                         ScoreCalculate.PermeabilityScore * ScoreModifier.PermeabilityScoreModifier +
                         ScoreCalculate.NotHardenedWithoutPlantsScore * ScoreModifier.NotHardenedWithoutPlantsModifier +
                         ScoreCalculate.SmallGreenScore * ScoreModifier.SmallGreenModifier +
                         Score.Grass * ScoreModifier.GrassModifier +
                         ScoreCalculate.ShrubberyScore * ScoreModifier.ShrubberyModifier +
                         Score.BigTree * ScoreModifier.BigTreeModifier;

        return (float)Math.Round(ScoreCalculate.TotalSurfaceArea / surfaceAreaWaterValue, 2);

    }

    //P2
    /// <summary>
    /// Calculates the healthy soil score based on the fertilizer score and a modifier.
    /// </summary>
    /// <param name="fertilizerScore">Enum of fertiziler type and capacity converted to int <br>for example (int)Fertilizer.Artificial.Half.</br></param>
    /// <returns></returns>
    public float HealthySoil(int fertilizerScore)
    {
        return fertilizerScore / ScoreModifier.Modifier;
    }

    //P3
    public float AnimalFriendliness()
    {
        float beesButterflyScore = GardenAnimals.SpottedBeesAndButterflies ? ScoreModifier.BeesAndButterfliesModifier * ScoreModifier.Modifier : 0;
        float birdsScore = GardenAnimals.SpottedBirds ? ScoreModifier.BirdsModifier * ScoreModifier.Modifier : 0;
        float spiderScore = GardenAnimals.SpottedSpiders ? ScoreModifier.SpiderModifier * ScoreModifier.Modifier : 0;
        float otherAnimalsScore = GardenAnimals.SpottedOtherAnimals ? ScoreModifier.OtherAnimalsModifier * ScoreModifier.Modifier : 0;

        return beesButterflyScore + birdsScore + spiderScore + otherAnimalsScore;
    }

    //P4
    public float PlantDiversity(int plantAmount)
    {
        int plantDiversityModifier = ScoreCalculate.GetPlantDiversityModifier(plantAmount);

        float smallGreenSurfaceRatio = ScoreCalculate.SmallGreenScore / ScoreCalculate.TotalSurfaceArea;
        float grassSurfaceRatio = Score.Grass / ScoreCalculate.TotalSurfaceArea;
        float shrubberySurfaceRatio = ScoreCalculate.ShrubberyScore / ScoreCalculate.TotalSurfaceArea;
        float bigTreeSurfaceRatio = Score.BigTree / ScoreCalculate.TotalSurfaceArea;

        float smallGreenValue = plantDiversityModifier * smallGreenSurfaceRatio * ScoreModifier.diversityFlowersModifier;
        float grassValue = plantDiversityModifier * grassSurfaceRatio * ScoreModifier.diversityGrassModifier;
        float shrubberyValue = plantDiversityModifier * shrubberySurfaceRatio * ScoreModifier.diversityShrubModifier;
        float bigTreeValue = plantDiversityModifier * bigTreeSurfaceRatio * ScoreModifier.diversityBigTreeModifier;

        return smallGreenValue + grassValue + shrubberyValue + bigTreeValue;
    }

}
