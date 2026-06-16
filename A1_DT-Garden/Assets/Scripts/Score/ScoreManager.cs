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

        return (float)Math.Round(surfaceAreaWaterValue / ScoreCalculate.TotalSurfaceArea, 2);
    }

    //P2
    /// <summary>
    /// Calculates the healthy soil score based on the fertilizer score and a modifier.
    /// </summary>
    /// <param name="fertilizerScore">Enum of fertiziler type and capacity converted to int <br>for example (int)Fertilizer.Artificial.Half.</br></param>
    /// <returns></returns>
    public float HealthySoil(FertilizerType fertilizerType , GreenWasteLeftInGarden greenWasteLeftInGarden)
    {
        int soilValue = (fertilizerType, greenWasteLeftInGarden) switch
        {
            // FertilizerType.None
            (FertilizerType.None, GreenWasteLeftInGarden.None) => 7,
            (FertilizerType.None, GreenWasteLeftInGarden.Half) => 2,
            (FertilizerType.None, GreenWasteLeftInGarden.Everything) => 0,

            // FertilizerType.Artificial
            (FertilizerType.Artificial, GreenWasteLeftInGarden.None) => 5,
            (FertilizerType.Artificial, GreenWasteLeftInGarden.Half) => 4,
            (FertilizerType.Artificial, GreenWasteLeftInGarden.Everything) => 3,

            // FertilizerType.Organic
            (FertilizerType.Organic, GreenWasteLeftInGarden.None) => 10,
            (FertilizerType.Organic, GreenWasteLeftInGarden.Half) => 9,
            (FertilizerType.Organic, GreenWasteLeftInGarden.Everything) => 8,

            // Default / Fallback case (the underscore acts as a catch-all)
            _ => -1
        };

        if(soilValue == -1)
        {
            Debug.LogError($"Invalid combination of FertilizerType: {fertilizerType} and GreenWasteLeftInGarden: {greenWasteLeftInGarden}. Please check the input values.");
        }

        return (float)Math.Round(soilValue * ScoreModifier.Modifier, 2);
    }


    //public enum Organic
    //{
    //    None = 10,
    //    Half = 9,
    //    Full = 8
    //}

    //P3
    public float AnimalFriendliness()
    {
        float beesButterflyScore = GardenAnimals.SpottedBeesAndButterflies ? ScoreModifier.BeesAndButterfliesModifier * ScoreModifier.Modifier : 0;
        float birdsScore = GardenAnimals.SpottedBirds ? ScoreModifier.BirdsModifier * ScoreModifier.Modifier : 0;
        float spiderScore = GardenAnimals.SpottedSpiders ? ScoreModifier.SpiderModifier * ScoreModifier.Modifier : 0;
        float otherAnimalsScore = GardenAnimals.SpottedOtherAnimals ? ScoreModifier.OtherAnimalsModifier * ScoreModifier.Modifier : 0;

        return (float)Math.Round(beesButterflyScore + birdsScore + spiderScore + otherAnimalsScore, 2);
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

        return (float)Math.Round(smallGreenValue + grassValue + shrubberyValue + bigTreeValue, 2);
    }

}
