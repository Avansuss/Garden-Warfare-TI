using Grid;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DrawingTypes;

public class UIManager : MonoBehaviour
{
    //references to the UI elements
    //https://www.flaticon.com/free-icon/paint-bucket_483918
    //https://www.flaticon.com/free-icon/download_724933
    //https://www.flaticon.com/free-icon/paint-brush_587377
    //triangle https://www.flaticon.com/free-icon/bleach_481099
    //square https://www.flaticon.com/free-icon/stop_545666

    private GridTileType chosenGridTileType = GridTileType.Inactive;
    private DrawingType chosenDrawingType = DrawingType.Triangle;

    [SerializeField]
    private MouseController mouseController;

    public void SelectDrawingType(int drawingID)
    {
        chosenDrawingType = (DrawingType)drawingID;

        switch (chosenDrawingType)
        {
            case DrawingType.Square:
                Debug.Log("Selected square draw tile type");
                break;
            case DrawingType.Triangle:
                Debug.Log("Selected triangle draw tile type");
                break;
        }

        mouseController.SetDrawingType(chosenDrawingType);
    }

    public void SelectTileType(int textureID)
    {
        chosenGridTileType = (GridTileType)textureID;
                
        mouseController.SetTileType(chosenGridTileType);
    }
}
