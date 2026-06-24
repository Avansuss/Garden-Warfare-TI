using System;
using Grid;
using Unity.Mathematics;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class GridEditorAgent : Agent
{
    public GridManager manager;

    private int step;
    private const int MAXSTEPS = 1200;
    private static readonly Section[] TriangleSections = 
        { Section.North, Section.East, Section.South, Section.West };
    
    private void Start()
    {
        step = 0;
    }

    public override void OnEpisodeBegin()
    {
        step = 0;
        manager.ResetGrid();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var gridData = manager.GetGrid();
        var gridSummary = manager.GetGridSummary();
        
        // How many of each type there are
        sensor.AddObservation(gridSummary[GridTileType.Grass]);
        sensor.AddObservation(gridSummary[GridTileType.Rock]);
        sensor.AddObservation(gridSummary[GridTileType.Water]);
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
                    gridData.TryGetValue(coord, out var type);
                    sensor.AddOneHotObservation((int)type, typesNum);
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
        
        var coord = new Coordinate(posX, posY, Section.Full);
        
        // Per-step punishment
        AddReward(-0.01f);
        
        // If the tile placement is correct or not
        if (manager.SetTile(coord, type, true, true))
        {
            if(type == GridTileType.Grass) AddReward(10);
            else AddReward(-5); 
        }
        else
        {
            AddReward(-5);
        }
        
        // Read the number of some tiles in the grid
        var summary = manager.GetGridSummary();
        AddReward(-.1f * summary[GridTileType.Empty]);

        // If the whole grid is filled
        if (summary[GridTileType.Empty] == 0)
        {
            AddReward(50);
            EndEpisode();
        }
        
        if(step >= MAXSTEPS) EndEpisode();
    }
}
