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

    public float HealthySoil(int fertilizerScore)
    {
        return fertilizerScore / ScoreModifier.Modifier;
    }


}
