using Grid;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class GridEditorAgent : Agent
{
    public GridManager manager;

    private int step;
    private const int MAXSTEPS = 1000;
    
    private void Start()
    {
        step = 0;
    }

    public override void OnEpisodeBegin()
    {
        manager.ResetGrid();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var gridData = manager.GetGrid();
        foreach (var gridDate in gridData)
        {
            sensor.AddObservation(gridDate.Key.Position.x);
            sensor.AddObservation(gridDate.Key.Position.y);
            sensor.AddObservation((int)gridDate.Value);
        }
        sensor.AddObservation(manager.Size.x);
        sensor.AddObservation(manager.Size.y);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        step++;
        var posX = actions.ContinuousActions[0];
        var posy = actions.ContinuousActions[0];
        GridTileType type = (GridTileType)actions.DiscreteActions[0];

        var coord = new Coordinate(posX, posX, Section.Full);

        if (manager.SetTile(coord, type, true, true))
        {
            if(type == GridTileType.Grass) SetReward(1);
            else SetReward(-1);
        }
        else
        {
            SetReward(-5);
        }
        
        if(step > MAXSTEPS) EndEpisode();
    }
}
