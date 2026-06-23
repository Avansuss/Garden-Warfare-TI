using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ScoreTesting
{
    private ScoreManager scoreManager;

    [Test]
    public void TestSoilWater()
    {
        setValues();
        scoreManager = new ScoreManager();

        Assert.AreEqual(6.6f, scoreManager.SoilWater());
    }

    [Test]
    public void TestHealthySoil()
    {
        setValues();
        scoreManager = new ScoreManager();
        Assert.AreEqual(6.47f, scoreManager.HealthySoil(FertilizerType.Organic, GreenWasteLeftInGarden.Half));
    }

    [Test]
    public void TestLifeAboveTheSoil()
    {
        setValues();
        scoreManager = new ScoreManager();
        Assert.AreEqual(7.19f, scoreManager.LifeAboveTheSoil());
    }

    [Test]
    public void TestPlantDiversity()
    {
        setValues();
        scoreManager = new ScoreManager();
        Assert.AreEqual(9.5f, scoreManager.PlantDiversity(11));
    }

    [Test]
    public void TestCanAllScoresBeFilled()
    {
        setValues();
        scoreManager = new ScoreManager();
        Assert.IsTrue(ScoreCalculate.AllScoresFilled);
    }

    private void setValues()
    {
        Score.AreaPond = 1f;
        Score.AreaSwimmingPool = 2f;
        Score.AreaPavement = 3f;

        Score.PermeableTiles = 3f;
        Score.Gravel = 4f;

        Score.RootBarrierFabric = 5f;
        Score.ArtificialGrass = 6f;
        Score.Trampoline = 7f;
        Score.PlayGround = 8f;

        Score.Flowers = 9f;
        Score.TreeBark = 10f;
        Score.VegetableGarden = 11f;

        Score.Grass = 12f;

        Score.Hedge = 13f;
        Score.Shrub = 14f;
        Score.PickingGarden = 15f;

        Score.BigTree = 16f;

        GardenAnimals.SpottedBeesAndButterflies = true;
        GardenAnimals.SpottedBirds = true;
        GardenAnimals.SpottedSpiders = true;
        GardenAnimals.SpottedOtherAnimals = true;
    }
}
