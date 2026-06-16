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
    [SerializeField] private float scoreModifier;
    [SerializeField] private float vegetationAmount;

    private ScoreManager scoreManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreManager = GetComponent<ScoreManager>();

        setValues();
        setLocalVar();
        showValues();
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

    private void setLocalVar()
    {
        this.hardeningScore = ScoreCalculate.HardeningScore;
        this.permeabilityScore = ScoreCalculate.PermeabilityScore;
        this.notHardenedWithoutPlantsScore = ScoreCalculate.NotHardenedWithoutPlantsScore;
        this.smallGreenScore = ScoreCalculate.SmallGreenScore;
        this.shrubberyScore = ScoreCalculate.ShrubberyScore;
        this.totalSurfaceArea = ScoreCalculate.TotalSurfaceArea;
        this.scoreModifier = ScoreModifier.Modifier;
        this.vegetationAmount = ScoreCalculate.VegetationAmount;
    }

    private void showValues()
    {
        if (ScoreCalculate.AllScoresFilled)
        {
            soilWaterScore = scoreManager.SoilWater();
            healthySoilScore = scoreManager.HealthySoil(FertilizerType.Organic, GreenWasteLeftInGarden.Half);
            animalFriendlinessScore = scoreManager.AnimalFriendliness();
            plantDiversityScore = scoreManager.PlantDiversity(13);
        }
    }
}
