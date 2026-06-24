using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ScoreTesting
{
    private ScoreData _scoreData;
    private GardenAnimalsData _gardenAnimalsData;
    private ScoreCalculation _scoreCalculate;
    private ScoreModifier _scoreModifier;
    private ScoreManager _scoreManager;

    [SetUp]
    public void SetUp()
    {
        _scoreData = new ScoreData();
        _gardenAnimalsData = new GardenAnimalsData();

        setupScoreData(_scoreData);
        setUpGardenAnimalsData(_gardenAnimalsData);

        _scoreCalculate = new ScoreCalculation(_scoreData);
        _scoreModifier = new ScoreModifier(_scoreCalculate);
        _scoreManager = new ScoreManager(_scoreData, _scoreCalculate, _scoreModifier, _gardenAnimalsData);
    }

    [Test]
    public void TestSoilWater()
    {
        Assert.AreEqual(6.6f, _scoreManager.SoilWater());
    }

    [Test]
    public void TestHealthySoil()
    {
        Assert.AreEqual(6.47f, _scoreManager.HealthySoil(FertilizerType.Organic, GreenWasteLeftInGarden.Half));
    }

    [Test]
    public void TestLifeAboveTheSoil()
    {
        Assert.AreEqual(7.19f, _scoreManager.LifeAboveTheSoil());
    }

    [Test]
    public void TestPlantDiversity()
    {
        Assert.AreEqual(9.5f, _scoreManager.PlantDiversity(11));
    }

    [Test]
    public void TestCanAllScoresBeFilled()
    {
        Assert.IsTrue(_scoreCalculate.AllScoresFilled);
    }

    private void setupScoreData(ScoreData scoreData)
    {
        scoreData.AreaPond = 1f;
        scoreData.AreaSwimmingPool = 2f;
        scoreData.AreaTiles = 3f;

        scoreData.PermeableTiles = 3f;
        scoreData.Gravel = 4f;

        scoreData.RootBarrierFabric = 5f;
        scoreData.ArtificialGrass = 6f;
        scoreData.Trampoline = 7f;
        scoreData.PlayGround = 8f;

        scoreData.Flowers = 9f;
        scoreData.WoodChips = 10f;
        scoreData.VegetableGarden = 11f;

        scoreData.Grass = 12f;

        scoreData.Hedge = 13f;
        scoreData.Shrub = 14f;
        scoreData.PickingGarden = 15f;

        scoreData.BigTree = 16f;
    }

    private void setUpGardenAnimalsData(GardenAnimalsData gardenAnimalsData)
    {
        gardenAnimalsData.SpottedBeesAndButterflies = true;
        gardenAnimalsData.SpottedBirds = true;
        gardenAnimalsData.SpottedSpiders = true;
        gardenAnimalsData.SpottedOtherAnimals = true;
    }
}
