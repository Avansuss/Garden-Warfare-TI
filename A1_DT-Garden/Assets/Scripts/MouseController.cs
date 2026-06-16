using System;
using Grid;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    private Camera cam;
    private Mouse mouse;

    public GridManager manager;
    private GameObject currentTileObj;
    private GridTileType currentTileType;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
        mouse = Mouse.current;
        SetcurrentTile(GridTileType.Grass);
        
        cam.transform.position = new Vector3(manager.Size.x / 2f, 10, manager.Size.y / 2f);;
    }

    // Update is called once per frame
    void Update()
    {
        // Camera zoom
        if (math.abs(mouse.scroll.value.y) > 0)
        {
            // Calculate how much we will have to move towards the zoomTowards position
            float multiplier = (1.0f / cam.orthographicSize * mouse.scroll.value.y);

            // Move camera
            transform.position += (cam.ScreenToWorldPoint(mouse.position.value) - transform.position) * multiplier; 

            // Zoom camera
            cam.orthographicSize -= mouse.scroll.value.y;

            // Limit zoom
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 0, 100);
        }
        
        
        var coordinate = GetGridSnappedMousePos();
        // Don't update the position outside of the bounds
        if (coordinate.Position.x > 0 && coordinate.Position.x < manager.Size.x &&
            coordinate.Position.z > 0 && coordinate.Position.z < manager.Size.y)
        {
            currentTileObj.transform.eulerAngles = new Vector3(0, coordinate.GetAngle(), 0);
            coordinate.Position.y = 1.001f;
            currentTileObj.transform.position = coordinate.Position;
        }
        if (mouse.leftButton.wasPressedThisFrame)
        {
            SetcurrentTile(currentTileType);
        }
        else if (mouse.rightButton.wasPressedThisFrame)
        {
            SetCursor(GridTileType.Empty);
        }

        if (mouse.leftButton.isPressed)
        {
            manager.SetTile(coordinate, currentTileType, redraw: true);
        }
        else if (mouse.rightButton.isPressed)
        {
            manager.SetTile(coordinate, GridTileType.Empty, redraw: true);
        }
    }

    /// <summary>
    /// Doesnt change the current tile type, only visually change the cursor
    /// </summary>
    /// <param name="type">The new visual tile type</param>
    private void SetCursor(GridTileType type)
    {
        Destroy(currentTileObj);
        currentTileObj = Instantiate(manager.TileTypeToObject(type), transform, true);
        currentTileObj.transform.position = GetGridSnappedMousePos().Position;
    }
    
    /// <summary>
    /// Change the current tile and cursor to a different type
    /// </summary>
    /// <param name="type">The new current tile</param>
    private void SetcurrentTile(GridTileType type)
    { 
        currentTileType = type;
        SetCursor(type);
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
