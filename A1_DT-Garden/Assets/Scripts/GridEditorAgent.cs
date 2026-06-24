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
    private const int MAXSTEPS = 300;
    
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
        foreach (var gridDate in gridData)
        {
            // tile xy pos
            sensor.AddObservation(gridDate.Key.Position.x);
            sensor.AddObservation(gridDate.Key.Position.y);
            
            // Tile type
            sensor.AddObservation((int)gridDate.Value);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        step++;
        var posX = actions.DiscreteActions[0];
        var posY = actions.DiscreteActions[1];
        GridTileType type = (GridTileType)actions.DiscreteActions[2];

        posX = math.clamp(posX, 0, manager.Size.x);
        posY = math.clamp(posY, 0, manager.Size.y);
        
        var coord = new Coordinate(posX, posY, Section.Full);

        if (manager.SetTile(coord, type, true, true))
        {
            if(type == GridTileType.Grass) SetReward(100);
            else SetReward(-1); 
        }
        else
        {
            SetReward(-5);
        }
        
        if(step > MAXSTEPS) EndEpisode();
    }
}
