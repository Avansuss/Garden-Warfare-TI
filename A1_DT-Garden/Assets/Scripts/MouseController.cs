using System;
using System.Collections.Generic;
using DrawingTypes;
using Grid;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    public GridManager manager;
    public bool CanDraw;
    private Mouse mouse;
    private Camera cam;
    private GameObject currentTileObj;
    private GridTileType currentTileType;
    private DrawingType drawingType = DrawingType.Triangle;
    private Vector2 initCamPos;
    private Vector3 dragOrigin;

    private GameObject[] squareTile = new GameObject[4];

    private bool blockClick = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
        mouse = Mouse.current;
        if(CanDraw) SetcurrentTile(GridTileType.Grass);

        if (cam && manager)
        {
            cam.transform.position = new Vector3(manager.Size.x / 2f, 10, manager.Size.y / 2f);;
        }
        else if (!manager)
        {
            Debug.LogWarning("gridmanager has not been set on mouse controller!");
        }
        else
        {
            Debug.LogWarning("main camera has not been set on mouse controller (somehow)");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Camera zoom
        if (math.abs(mouse.scroll.value.y) > 0)
        {
            ZoomOrthoToMouse(mouse.scroll.value.y);
        }
        
        // Camera move
        if (mouse.middleButton.wasPressedThisFrame)
        {
            dragOrigin = ScreenToWorld(mouse.position.value);
        }
        if (mouse.middleButton.isPressed)
        {
            Vector3 current = ScreenToWorld(mouse.position.value);
            Vector3 delta = dragOrigin - current;
            cam.transform.position += delta;
            // Recalculate so next frame's delta is relative, not cumulative
            dragOrigin = ScreenToWorld(mouse.position.value);
        }
        else
        {
            // No mouse controls if its outside of the game window
            Vector2 view = cam.ScreenToViewportPoint( mouse.position.value );
            bool isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;
            if (!isOutside && CanDraw)
            {
                if (blockClick) return;

                Coordinate coordinate = new Coordinate();
                switch (drawingType)
                {
                    case DrawingType.Square:
                        coordinate = GetGridSnappedMousePos(true);
                        break;
                    case DrawingType.Triangle:
                        coordinate = GetGridSnappedMousePos(false);
                        break;

                }
                //Debug.Log($"Mouse position: {coordinate.Position.x}, {coordinate.Position.z} | Section: {coordinate.Section}");

                // Don't update the position outside of the bounds
                if (WithinBounds(coordinate.Position))
                {
                    TileFollowCursor(coordinate);
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
                    // Make the mouse coord relative
                    var relativeCoord = manager.transform.position;
                    coordinate.Position -= relativeCoord;
                    manager.SetTile(coordinate, currentTileType, redraw: true);
                }
                else if (mouse.rightButton.isPressed)
                {
                    // Make the mouse coord relative
                    var relativeCoord = manager.transform.position;
                    coordinate.Position -= relativeCoord;
                    manager.SetTile(coordinate, GridTileType.Empty, redraw: true);
                }
            }
        }
    }

    private void TileFollowCursor(Coordinate coordinate)
    {
        switch (drawingType)
        {
            case DrawingType.Square:
                for (int i = 0; i < 4; i++)
                {
                    squareTile[i].transform.eulerAngles = new Vector3(0, i * 90, 0);
                    coordinate.Position.y = 1.001f;
                    squareTile[i].transform.position = coordinate.Position;
                }
                break;
            case DrawingType.Triangle:
                currentTileObj.transform.eulerAngles = new Vector3(0, coordinate.GetAngle(), 0);
                coordinate.Position.y = 1.001f;
                currentTileObj.transform.position = coordinate.Position;
                break;
        }
    }

    /// <summary>
    /// Prevents drawing tiles when the mouse is over a UI element
    /// </summary>
    /// <param name="isBlocked"></param>
    public void BlockClick(bool isBlocked)
    {
        this.blockClick = isBlocked;
    }

    public void SetTileType(GridTileType chosenTileType)
    {
        this.currentTileType = chosenTileType;
        SetCursor(chosenTileType);
    }

    public void SetDrawingType(DrawingType chosenDrawingType)
    {
        // 0 = single square, 1 = single triangle
        drawingType = chosenDrawingType;
        SetCursor(currentTileType);
    }

    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        // For your isometric-style ortho camera on Y axis,
        // use a raycast against the Y=0 plane
        Ray ray = cam.ScreenPointToRay(new Vector3(screenPos.x, screenPos.y, 0));
        float t = -ray.origin.y / ray.direction.y;
        return ray.origin + ray.direction * t;
    }
    
    private void ZoomOrthoToMouse(float amount)
    {
        // Calculate how much we will have to move towards the zoomTowards position
        float multiplier = (1.0f / cam.orthographicSize * amount);

        // Move camera
        transform.position += (cam.ScreenToWorldPoint(mouse.position.value) - transform.position) * multiplier; 

        // Zoom camera
        cam.orthographicSize -= amount;

        // Limit zoom
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1, 100);
    }

    private bool WithinBounds(Vector3 position)
    {
        return position.x > 0 && position.x < manager.Size.x &&
               position.z > 0 && position.z < manager.Size.y;
    }

    /// <summary>
    /// Doesnt change the current tile type, only visually change the cursor
    /// </summary>
    /// <param name="type">The new visual tile type</param>
    private void SetCursor(GridTileType type)
    {
        Destroy(currentTileObj);

        switch (drawingType)
        {
            case DrawingType.Triangle:
                currentTileObj = Instantiate(manager.TileTypeToObject(type), manager.transform, true);
                currentTileObj.transform.position = GetGridSnappedMousePos(false).Position;
                currentTileObj.transform.eulerAngles = new Vector3(0, GetGridSnappedMousePos(false).GetAngle(), 0);
                break;
            case DrawingType.Square:
                for(int i = 0; i < 4; i++)
                {
                    Destroy(squareTile[i]);
                    squareTile[i] = Instantiate(manager.TileTypeToObject(type), manager.transform, true);
                    squareTile[i].transform.position = GetGridSnappedMousePos(true).Position;
                    squareTile[i].transform.eulerAngles = new Vector3(0, GetGridSnappedMousePos(true).GetAngle(), 0);
                }
                break;
        }
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

        // Get the decimals of the cursor position, centered around the middle
        var decX = cursorPos.x % 1 - 0.5f;
        var decXAbs = math.abs(decX);
        var decZ = cursorPos.z % 1 - 0.5f;
        var decZAbs = math.abs(decZ);

        // Snap to middle of grid
        cursorPos.x = math.floor(cursorPos.x) + 0.5f;
        cursorPos.z = math.floor(cursorPos.z) + 0.5f;

        // Calculate what triangle the mouse is in
        //Coordinate coordinate = new(cursorPos.x, cursorPos.z);
        Section targetedSection = Section.Full;

        if(!getFullTile)
        {   
            if (decZAbs > decXAbs)
            {
                targetedSection = decZ < 0 ? Section.South: Section.North;
            }
            else
            {
                targetedSection = decX < 0 ? Section.East : Section.West; 
            }
        }
        //return coordinate;
        return new Coordinate(cursorPos.x, cursorPos.z, targetedSection);
    }
}
