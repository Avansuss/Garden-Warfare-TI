using Grid;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    private Camera cam;
    private Mouse mouse;

    public GridManager manager;
    private GameObject currentTileObj;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
        mouse = Mouse.current;
        SetcurrentTile(GridTileType.Empty);
        
        cam.transform.position = new Vector3(manager.Size.x / 2f, 10, manager.Size.y / 2f);;
    }

    // Update is called once per frame
    void Update()
    {
        var coordinate = GetGridSnappedMousePos();
        
        // Don't update the position outside of the bounds
        if (coordinate.Position.x > 0 && coordinate.Position.x < manager.Size.x &&
            coordinate.Position.z > 0 && coordinate.Position.z < manager.Size.y)
        {
            currentTileObj.transform.eulerAngles = new Vector3(0, coordinate.GetAngle() + 90, 0);
            coordinate.Position.y = 1.001f;
            currentTileObj.transform.position = coordinate.Position;
        }
    }

    private void SetcurrentTile(GridTileType type)
    { 
        Destroy(currentTileObj); 
        currentTileObj = Instantiate(manager.TileTypeToObject(type), transform, true);
    }
    
    private Coordinate GetGridSnappedMousePos(bool getFullTile = false)
    {
        var cursorPos = cam.ScreenToWorldPoint(mouse.position.ReadValue());
        
        cursorPos.y = 0;

        // Get the decimals of the cursor position, centered aroudn the middle
        var decX = cursorPos.x % 1 - 0.5f;
        var decXAbs = math.abs(decX);
        var decZ = cursorPos.z % 1 - 0.5f;
        var decZAbs = math.abs(decZ);

        // Snap to middle of grid
        cursorPos.x = math.floor(cursorPos.x) + 0.5f;
        cursorPos.z = math.floor(cursorPos.z) + 0.5f;

        // Calculate what triangle the mouse is in
        Coordinate coordinate = new(cursorPos.x, cursorPos.z);
        if (getFullTile)
        {
            coordinate.Section = Section.Full;
        }
        else
        {
            if (decZAbs > decXAbs)
            {
                coordinate.Section = decZ < 0 ? Section.North : Section.South;
            }
            else
            {
                coordinate.Section = decX < 0 ? Section.East : Section.West; 
            }
        }
        return coordinate;  
    }
}
