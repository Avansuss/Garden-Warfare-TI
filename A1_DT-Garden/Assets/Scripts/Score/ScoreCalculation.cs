using UnityEngine;

public class ScoreCalculation
{
    private readonly ScoreData _score;
    public ScoreCalculation(ScoreData scoreData)
    {
        this._score = scoreData;
    }

    public bool AllScoresFilled => _score.AreaPond >= 0 && _score.AreaSwimmingPool >= 0 && _score.AreaTiles >= 0 &&
                              _score.Gravel >= 0 && _score.PermeableTiles >= 0 &&
                              _score.RootBarrierFabric >= 0 && _score.ArtificialGrass >= 0 && _score.Trampoline >= 0 && _score.PlayGround >= 0 &&
                              _score.Flowers >= 0 && _score.WoodChips >= 0 && _score.VegetableGarden >= 0 &&
                              _score.Shrub >= 0 && _score.Hedge >= 0 && _score.PickingGarden >= 0 &&
                              _score.Grass >= 0 && _score.BigTree >= 0;

    //P1
    public float HardeningScore => _score.AreaPond + _score.AreaSwimmingPool + _score.AreaTiles;
    public float PermeabilityScore => _score.Gravel + _score.PermeableTiles;
    public float NotHardenedWithoutPlantsScore => _score.RootBarrierFabric + _score.ArtificialGrass + _score.Trampoline + _score.PlayGround;
    public float SmallGreenScore => _score.Flowers + _score.WoodChips + _score.VegetableGarden;
    public float ShrubberyScore => _score.Shrub + _score.Hedge + _score.PickingGarden;
    public float TotalSurfaceArea => HardeningScore + PermeabilityScore + NotHardenedWithoutPlantsScore + SmallGreenScore + ShrubberyScore + _score.Grass + _score.BigTree;

    //P2
    public float VegetationAmount => SmallGreenScore + ShrubberyScore + _score.Grass + _score.BigTree;

    //P4
    public int GetPlantDiversityModifier(int amount)
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
