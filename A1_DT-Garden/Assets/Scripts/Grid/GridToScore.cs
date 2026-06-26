using Grid;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GridToScore
{
    ScoreData scoreData;
    GardenAnimalsData gardenAnimalsData;
    ScoreCalculation scoreCalculate;
    ScoreModifier scoreModifier;
    ScoreManager scoreManager;
    ScoreVisualizer scoreVisualizer;
    FertilizerType fertilizerType;
    GreenWasteLeftInGarden greenWasteLeftInGarden;


    public GridToScore()
    {
        scoreData = new();
        gardenAnimalsData = new();

        // find UImanager
        GameObject uiManagerObject = GameObject.Find("UIManager");
        
        scoreVisualizer = uiManagerObject?.GetComponent<ScoreVisualizer>();
    }

    public GridScorePillars CalculateScore(Dictionary<Coordinate, GridTile> drawnTiles, int plantAmount, bool[] questionsAnimal, byte[] answersDropdown, bool doLogging=true)
    {
        setSpottedGardenAnimals(questionsAnimal, doLogging);
        setSoilHandlingValues(answersDropdown);
        setScores(drawnTiles);

        //Debug.Log($"Amount of ponds: {scoreData.AreaPond}");

        scoreCalculate = new ScoreCalculation(scoreData);
        scoreModifier = new ScoreModifier(scoreCalculate);
        scoreManager = new ScoreManager(scoreData, scoreCalculate, scoreModifier, gardenAnimalsData);

        var gridScore = new GridScorePillars()
        {
            SoilWater = scoreManager.SoilWater(),
            HealthySoil = scoreManager.HealthySoil(fertilizerType, greenWasteLeftInGarden),
            LifeAboveSoil = scoreManager.LifeAboveTheSoil(),
            PlantDiversity = scoreManager.PlantDiversity(plantAmount)
        };
        
        //NaN check
        if (float.IsNaN(gridScore.SoilWater)) gridScore.SoilWater = 0;
        if (float.IsNaN(gridScore.HealthySoil)) gridScore.HealthySoil = 0;
        if (float.IsNaN(gridScore.LifeAboveSoil)) gridScore.LifeAboveSoil = 0;
        if (float.IsNaN(gridScore.PlantDiversity)) gridScore.PlantDiversity = 0;
        
        if(doLogging)
        {
            Debug.Log($"Soil Water: {gridScore.SoilWater}");
            Debug.Log($"Healthy Soil: {gridScore.HealthySoil}"); //fix amount of fertilizer and green waste in garden
            Debug.Log($"Life Above The Soil: {gridScore.LifeAboveSoil}");
            Debug.Log($"Plant Diversity: {gridScore.PlantDiversity}"); //fix amount of plants in garden
            Debug.Log("Plant Amount: " + plantAmount);
        }
        
        float[] calculatedPillars = new float[4] { gridScore.SoilWater, gridScore.HealthySoil, gridScore.LifeAboveSoil, gridScore.PlantDiversity } ;

        scoreVisualizer?.VisualizeScore(calculatedPillars);
        return gridScore;
    }

    private void setScores(Dictionary<Coordinate, GridTile> drawnTiles)
    {
        int maxEnumValue = System.Enum.GetValues(typeof(GridTileType)).Cast<int>().Max();
        int[] tileCounts = new int[maxEnumValue + 1];

        foreach (var tile in drawnTiles.Values)
        {
            int index = (int)tile.TileType;
            if (index >= 0 && index < tileCounts.Length)
            {
                tileCounts[index]++;
            }
        }

        scoreData.Grass = tileCounts[(int)GridTileType.Grass];
        scoreData.AreaPond = tileCounts[(int)GridTileType.Pond];
        scoreData.AreaSwimmingPool = tileCounts[(int)GridTileType.SwimmingPool];
        scoreData.AreaTiles = tileCounts[(int)GridTileType.StoneTiles];
        scoreData.PermeableTiles = tileCounts[(int)GridTileType.PermeableTiles];
        scoreData.Gravel = tileCounts[(int)GridTileType.Gravel];
        scoreData.RootBarrierFabric = tileCounts[(int)GridTileType.RootFabric];
        scoreData.ArtificialGrass = tileCounts[(int)GridTileType.ArtificialGrass];
        scoreData.Trampoline = tileCounts[(int)GridTileType.Trampoline];
        scoreData.PlayGround = tileCounts[(int)GridTileType.Playground];
        scoreData.PlayGround += tileCounts[(int)GridTileType.PlaygroundSand];
        scoreData.Flowers = tileCounts[(int)GridTileType.Flowers];
        scoreData.WoodChips = tileCounts[(int)GridTileType.WoodChips];
        scoreData.VegetableGarden = tileCounts[(int)GridTileType.VegetableGarden];
        scoreData.Hedge = tileCounts[(int)GridTileType.Hedge];
        scoreData.Shrub = tileCounts[(int)GridTileType.Bush];
        scoreData.PickingGarden = tileCounts[(int)GridTileType.PickingGarden];
        scoreData.BigTree = tileCounts[(int)GridTileType.Tree];
    }

    private void setSpottedGardenAnimals(bool[] userGardenAnimals, bool doLogging=true)
    {
        foreach (var animal in userGardenAnimals)
        {
           if(doLogging) Debug.Log("User Garden Animals: " + animal);
        }

        gardenAnimalsData.SpottedBeesAndButterflies = userGardenAnimals[0];
        gardenAnimalsData.SpottedBirds = userGardenAnimals[1];
        gardenAnimalsData.SpottedSpiders = userGardenAnimals[2];
        gardenAnimalsData.SpottedOtherAnimals = userGardenAnimals[3];
    }

    private void setSoilHandlingValues(byte[] answersDropdown)
    {
        fertilizerType = (FertilizerType)answersDropdown[0];
        greenWasteLeftInGarden = (GreenWasteLeftInGarden)answersDropdown[1];
    }
}