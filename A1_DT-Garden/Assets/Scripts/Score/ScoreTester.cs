using JetBrains.Annotations;
using UnityEngine;

public class ScoreTester : MonoBehaviour
{
    [Header("Score values")]
    [SerializeField] private float soilWaterScore;
    [SerializeField] private float healthySoilScore;
    [SerializeField] private float animalFriendlinessScore;
    [SerializeField] private float plantDiversityScore;

    [Header("References")]
    [SerializeField] private float hardeningScore;
    [SerializeField] private float permeabilityScore;
    [SerializeField] private float notHardenedWithoutPlantsScore;
    [SerializeField] private float smallGreenScore;
    [SerializeField] private float shrubberyScore;
    [SerializeField] private float totalSurfaceArea;
    [SerializeField] private float scoreModifierData;
    [SerializeField] private float vegetationAmount;

    private ScoreData scoreData = new ScoreData();
    private GardenAnimalsData gardenAnimalsData = new GardenAnimalsData();
    //private ScoreVisualizer scoreVisualizer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //scoreVisualizer = GetComponent<ScoreVisualizer>();

        scoreData = setValues();
        gardenAnimalsData = SetGardenAnimals();

        ScoreCalculation scoreCalculate = new ScoreCalculation(scoreData);
        ScoreModifier scoreModifier = new ScoreModifier(scoreCalculate);
        ScoreManager scoreManager = new ScoreManager(scoreData, scoreCalculate, scoreModifier, gardenAnimalsData);

        showScores(scoreCalculate, scoreModifier);
        
        showValues(scoreCalculate, scoreManager);
    }

    private ScoreData setValues()
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

        return scoreData;
    }

    private GardenAnimalsData SetGardenAnimals()
    {
        gardenAnimalsData.SpottedBeesAndButterflies = true;
        gardenAnimalsData.SpottedBirds = true;
        gardenAnimalsData.SpottedSpiders = true;
        gardenAnimalsData.SpottedOtherAnimals = true;

        return gardenAnimalsData;
    }

    private void showScores(ScoreCalculation scoreCalculate, ScoreModifier scoreModifier)
    {
        this.hardeningScore = scoreCalculate.HardeningScore;
        this.permeabilityScore = scoreCalculate.PermeabilityScore;
        this.notHardenedWithoutPlantsScore = scoreCalculate.NotHardenedWithoutPlantsScore;
        this.smallGreenScore = scoreCalculate.SmallGreenScore;
        this.shrubberyScore = scoreCalculate.ShrubberyScore;
        this.totalSurfaceArea = scoreCalculate.TotalSurfaceArea;
        this.scoreModifierData = scoreModifier.Modifier;
        this.vegetationAmount = scoreCalculate.VegetationAmount;
    }

    private void showValues(ScoreCalculation scoreCalculation, ScoreManager scoreManager)
    {
        if (scoreCalculation.AllScoresFilled)
        {
            soilWaterScore = scoreManager.SoilWater();
            healthySoilScore = scoreManager.HealthySoil(FertilizerType.Organic, GreenWasteLeftInGarden.Half);
            animalFriendlinessScore = scoreManager.LifeAboveTheSoil();
            plantDiversityScore = scoreManager.PlantDiversity(13);

            //float[] calculatedValues = new float[4] { soilWaterScore, healthySoilScore, animalFriendlinessScore, plantDiversityScore };

            //scoreVisualizer.VisualizeScore(calculatedValues);
        }
    }
}
