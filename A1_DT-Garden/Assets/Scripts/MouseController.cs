using System;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    private Camera cam;
    private Mouse mouse;

    public GridManager manager;
    public GameObject testObj;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
        mouse = Mouse.current;
    }

    // Update is called once per frame
    void Update()
    {
        var coordinate = GetGridSnappedMousePos();
        
        testObj.transform.eulerAngles = new Vector3(0, coordinate.GetAngle() + 90, 0);
        testObj.transform.position = coordinate.Position;
    }

    private Coordinate GetGridSnappedMousePos(bool getFullTile = false)
    {
        var cursorPos = cam.ScreenToWorldPoint(mouse.position.ReadValue());

        // Don't update the position outside of the bounds
        if (cursorPos.x > 0 && cursorPos.x < manager.Size.x && cursorPos.z > 0 && cursorPos.z < manager.Size.y)
        {
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
            Coordinate coordinate = new(cursorPos);
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
                    coordinate.Section = decX < 0 ? Section.West : Section.East; 
                }
            }

            return coordinate;
        }
    }
}
