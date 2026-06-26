using System;
using UnityEditor;
using UnityEngine;

public class ScoreManager
{
    public ScoreData ScoreData;
    public GardenAnimalsData GardenAnimals;
    public ScoreCalculation ScoreCalculate;
    public ScoreModifier ScoreModifier;

    public ScoreManager(ScoreData scoreData, ScoreCalculation scoreCalculate, ScoreModifier scoreModifier, GardenAnimalsData gardenAnimals)
    {
        if (scoreData == null) Debug.LogError("ScoreData is null. Please ensure it is properly initialized before creating ScoreManager.");
        else if (scoreCalculate == null) Debug.LogError("ScoreCalculation is null. Please ensure it is properly initialized before creating ScoreManager.");
        else if (scoreModifier == null) Debug.LogError("ScoreModifier is null. Please ensure it is properly initialized before creating ScoreManager.");
        else if (gardenAnimals == null) Debug.LogError("GardenAnimalsData is null. Please ensure it is properly initialized before creating ScoreManager.");
        else
        {
            this.ScoreData = scoreData;
            this.ScoreCalculate = scoreCalculate;
            this.ScoreModifier = scoreModifier;
            this.GardenAnimals = gardenAnimals;
        }
    }

    //P1
    /// <summary>
    /// Calculates the first pillar. 
    /// </summary>
    /// <returns>Rounded grade (max 2 decimals) for the amount of water that can be found in the soil</returns>
    public float SoilWater()
    {
        float surfaceAreaWaterValue = this.ScoreCalculate.HardeningScore * ScoreModifier.HardeningScoreModifier +
                         this.ScoreCalculate.PermeabilityScore * ScoreModifier.PermeabilityScoreModifier +
                         this.ScoreCalculate.NotHardenedWithoutPlantsScore * ScoreModifier.NotHardenedWithoutPlantsModifier +
                         this.ScoreCalculate.SmallGreenScore * ScoreModifier.SmallGreenModifier +
                         ScoreData.Grass * ScoreModifier.GrassModifier +
                         this.ScoreCalculate.ShrubberyScore * ScoreModifier.ShrubberyModifier +
                         ScoreData.BigTree * ScoreModifier.BigTreeModifier;

        float soilWaterValue = (float)Math.Round(surfaceAreaWaterValue / this.ScoreCalculate.TotalSurfaceArea, 2);
        
        return scoreBoundaryCheck(soilWaterValue);
    }

    //P2
    /// <summary>
    /// Second pillar. Calculates the healthy soil score based on the fertilizer score and a modifier.
    /// </summary>
    /// <param name="fertilizerType">Enum variable that needs to be given as first parameter.
    /// <br>Example: scoreManager.HealthySoil(FertilizerType.Organic, GreenWasteLeftInGarden.Half);</br></param>
    /// <param name="greenWasteLeftInGarden">Enum variable of the green waste that gets left behind in the garden.
    /// <br>Example: scoreManager.HealthySoil(FertilizerType.Organic, GreenWasteLeftInGarden.Half);</br>
    /// </param>
    /// <returns>Rounded grade (max 2 decimals) for how healthy the soil is</returns>
    public float HealthySoil(FertilizerType fertilizerType, GreenWasteLeftInGarden greenWasteLeftInGarden)
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

        if (soilValue == -1)
        {
            Debug.LogError($"Invalid combination of FertilizerType: {fertilizerType} and GreenWasteLeftInGarden: {greenWasteLeftInGarden}. Please check the input values.");
        }

        float healthySoilValue = (float)Math.Round(soilValue * ScoreModifier.Modifier, 2);

        return scoreBoundaryCheck(healthySoilValue);
    }

    //P3
    /// <summary>
    /// Third pillar. Calculates the healthyness above the soil.
    /// </summary>
    /// <returns>Rounded grade (max 2 decimals) about how much life there is above the soil.</returns>
    public float LifeAboveTheSoil()
    {
        if(GardenAnimals == null)
        {
            Debug.LogError("GardenAnimalsData is null. Please ensure it is properly initialized before calling LifeAboveTheSoil().");
            return -1f; // Return a default value or handle the error as needed
        }

        float beesButterflyScore = GardenAnimals.SpottedBeesAndButterflies ? ScoreModifier.BeesAndButterfliesModifier * ScoreModifier.Modifier : 0;
        float birdsScore = GardenAnimals.SpottedBirds ? ScoreModifier.BirdsModifier * ScoreModifier.Modifier : 0;
        float spiderScore = GardenAnimals.SpottedSpiders ? ScoreModifier.SpiderModifier * ScoreModifier.Modifier : 0;
        float otherAnimalsScore = GardenAnimals.SpottedOtherAnimals ? ScoreModifier.OtherAnimalsModifier * ScoreModifier.Modifier : 0;

        float lifeAboveSoilValue = (float)Math.Round(beesButterflyScore + birdsScore + spiderScore + otherAnimalsScore, 2);

        return scoreBoundaryCheck(lifeAboveSoilValue);
    }

    //P4
    /// <summary>
    /// 
    /// </summary>
    /// <param name="plantAmount">The amount of plant species that can be found in the users garden</param>
    /// <returns>Rounded grade (max 2 decimals) about how much diversity there is in terms of plants</returns>
    public float PlantDiversity(int plantAmount)
    {
        int plantDiversityModifier = ScoreCalculate.GetPlantDiversityModifier(plantAmount);

        float smallGreenSurfaceRatio = ScoreCalculate.SmallGreenScore / ScoreCalculate.TotalSurfaceArea;
        float grassSurfaceRatio = ScoreData.Grass / ScoreCalculate.TotalSurfaceArea;
        float shrubberySurfaceRatio = ScoreCalculate.ShrubberyScore / ScoreCalculate.TotalSurfaceArea;
        float bigTreeSurfaceRatio = ScoreData.BigTree / ScoreCalculate.TotalSurfaceArea;

        float smallGreenValue = plantDiversityModifier * smallGreenSurfaceRatio * ScoreModifier.diversityFlowersModifier;
        float grassValue = plantDiversityModifier * grassSurfaceRatio * ScoreModifier.diversityGrassModifier;
        float shrubberyValue = plantDiversityModifier * shrubberySurfaceRatio * ScoreModifier.diversityShrubModifier;
        float bigTreeValue = plantDiversityModifier * bigTreeSurfaceRatio * ScoreModifier.diversityBigTreeModifier;

        float plantDiversityValue = (float)Math.Round(smallGreenValue + grassValue + shrubberyValue + bigTreeValue, 2);

        return scoreBoundaryCheck(plantDiversityValue);
    }

    /// <summary>
    /// Checks if score is within the boundaries of 0 and 10. If not, it will return the closest boundary value.
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    private float scoreBoundaryCheck(float score)
    {
        if (score < 0)
        {
            return 0;
        }
        else if (score > 10)
        {
            return 10;
        }
        else
        {
            return score;
        }
    }

}
