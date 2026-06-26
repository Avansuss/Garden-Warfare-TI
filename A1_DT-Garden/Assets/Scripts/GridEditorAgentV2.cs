using System;
using Grid;
using Unity.Mathematics;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class GridEditorAgentV2 : Agent
{
    public GridManager manager;
    public float SoilWater;
    public float HealthySoil;
    public float LifeAboveSoil;
    public float PlantDiversity;

    private int step;
    private const int MAXSTEPS = 5000;
    private static readonly Section[] TriangleSections = 
        { Section.North, Section.East, Section.South, Section.West };

    // Score calculation stuff
    public int plantAmount;
    public bool[] userGardenAnimals;
    public byte[] answersDropdown;

    private GridToScore _gridToScore;
    
    private void Start()
    {
        _gridToScore = new();
        step = 0;
    }

    public override void OnEpisodeBegin()
    {
        step = 0;
        manager.ResetGrid();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var tiles = manager.GetGrid();
        var gridSummary = manager.GetGridSummary();
        var gridScore = _gridToScore.CalculateScore(tiles, plantAmount, userGardenAnimals, answersDropdown);
        
        // Score matrix observations
        sensor.AddObservation(gridScore.SoilWater);
        sensor.AddObservation(gridScore.HealthySoil);
        sensor.AddObservation(gridScore.LifeAboveSoil);
        sensor.AddObservation(gridScore.PlantDiversity);
        
        
        // How many of each type there are
        sensor.AddObservation(gridSummary[GridTileType.Empty]);

        var typesNum = Enum.GetValues(typeof(GridTileType)).Length;
        // dictionaries can be prone to not being the same order, which is critical for this observation
        for (int x = 0; x < manager.Size.x; x++)
        {
            for (int y = 0; y < manager.Size.y; y++)
            {
                foreach(var section in TriangleSections)
                {
                    var coord = new Coordinate(x, y, section);
                    tiles.TryGetValue(coord, out var type);
                    sensor.AddOneHotObservation((int)type!.TileType, typesNum);
                }
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        step++;
        var posX = actions.DiscreteActions[0];
        var posY = actions.DiscreteActions[1];
        GridTileType type = (GridTileType)(actions.DiscreteActions[2] + 2);
        var section = (Section)actions.DiscreteActions[3];
        
        var coord = new Coordinate(posX, posY, section);
        
        // Per-step punishment
        AddReward(-0.01f);
        
        // If the tile placement is correct or not
        if (!manager.SetTile(coord, type, true, true))
        {
            AddReward(-5);
        }
        
        // 4 pillar score evaulation
        var tiles = manager.GetGrid();
        var gridScore = _gridToScore.CalculateScore(tiles, plantAmount, userGardenAnimals, answersDropdown);

        var fourPillarScore = gridScore.SoilWater *
                              gridScore.HealthySoil *
                              gridScore.LifeAboveSoil *
                              gridScore.PlantDiversity * 0.1f;
        
        //For purely visual reasons
        SoilWater = gridScore.SoilWater;
        HealthySoil = gridScore.HealthySoil;
        LifeAboveSoil = gridScore.LifeAboveSoil;
        PlantDiversity = gridScore.PlantDiversity;
        
        AddReward(fourPillarScore);
        
        // Read the number of some tiles in the grid
        var summary = manager.GetGridSummary();

        // If the whole grid is filled
        if (summary[GridTileType.Empty] == 0)
        {
            AddReward(50);
            EndEpisode();
        }
        
        if(step >= MAXSTEPS) EndEpisode();
    }
}
