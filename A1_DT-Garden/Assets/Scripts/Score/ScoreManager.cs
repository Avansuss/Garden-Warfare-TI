using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public float SoilWater()
    {
        float surfaceAreaWaterValue = ScoreCalculate.HardeningScore * ScoreModifier.HardeningScoreModifier +
                         ScoreCalculate.PermeabilityScore * ScoreModifier.PermeabilityScoreModifier +
                         ScoreCalculate.NotHardenedWithoutPlantsScore * ScoreModifier.NotHardenedWithoutPlantsModifier +
                         ScoreCalculate.SmallGreenScore * ScoreModifier.SmallGreenModifier +
                         Score.Grass * ScoreModifier.GrassModifier +
                         ScoreCalculate.ShrubberyScore * ScoreModifier.ShrubberyModifier +
                         Score.BigTree * ScoreModifier.BigTreeModifier;

        return ScoreCalculate.TotalSurfaceArea / surfaceAreaWaterValue;
    }
}
