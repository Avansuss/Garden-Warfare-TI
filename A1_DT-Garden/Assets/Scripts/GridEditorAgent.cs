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
    private const int MAXSTEPS = 5000;
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
        var section = (Section)actions.DiscreteActions[3];
        
        var coord = new Coordinate(posX, posY, section);
        
        // Per-step punishment
        AddReward(-0.01f);

        manager.GetGrid().TryGetValue(coord, out var tileBeforeChange);
        var wasCorrect = tileBeforeChange == GridTileType.Grass;
        
        // If the tile placement is correct or not
        if (manager.SetTile(coord, type, true, true))
        {
            var isCorrect = type == GridTileType.Grass;
            
            if(!wasCorrect && isCorrect) AddReward(30);
            else if (wasCorrect && isCorrect) AddReward(-1);
            else if (!isCorrect && wasCorrect) AddReward(-5);
            else AddReward(-2); 
        }
        else
        {
            AddReward(-5);
        }
        
        // Read the number of some tiles in the grid
        var summary = manager.GetGridSummary();

        // If the whole grid is filled
        if (summary[GridTileType.Empty] == 0)
        {
            if (summary[GridTileType.Grass] == manager.Size.x * manager.Size.y * 4) AddReward(100);
            AddReward(50);
            EndEpisode();
        }
        
        if(step >= MAXSTEPS) EndEpisode();
    }
}
