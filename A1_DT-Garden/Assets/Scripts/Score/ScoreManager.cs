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

}
