using UnityEngine;

public class ScoreModifier
{
    private ScoreCalculation _scoreCalculation;
    public float Modifier;

    public ScoreModifier(ScoreCalculation scoreCalculation)
    {
        this._scoreCalculation = scoreCalculation;

        this.Modifier = _scoreCalculation.VegetationAmount / _scoreCalculation.TotalSurfaceArea;
    }

    //P1
    public readonly float HardeningScoreModifier = 0f;
    public readonly float PermeabilityScoreModifier = 2f;
    public readonly float NotHardenedWithoutPlantsModifier = 3f;
    public readonly float SmallGreenModifier = 3.5f;
    public readonly float GrassModifier = 5f;
    public readonly float ShrubberyModifier = 10f;
    public readonly float BigTreeModifier = 15f;

    //P3
    public readonly float BeesAndButterfliesModifier = 2.5f;
    public readonly float BirdsModifier = 2.5f;
    public readonly float SpiderModifier = 2.5f;
    public readonly float OtherAnimalsModifier = 2.5f;

    //P4
    public readonly float diversityFlowersModifier = 1;
    public readonly float diversityGrassModifier = 0.25f;
    public readonly float diversityShrubModifier = 2;
    public readonly float diversityBigTreeModifier = 3;

}
