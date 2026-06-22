using Grid;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //references to the UI elements
    //https://www.flaticon.com/free-icon/paint-bucket_483918
    //https://www.flaticon.com/free-icon/download_724933
    //https://www.flaticon.com/free-icon/paint-brush_587377
    //triangle https://www.flaticon.com/free-icon/bleach_481099
    //square https://www.flaticon.com/free-icon/stop_545666


    public int chosenId = 0;

    [SerializeField]
    private MouseController mouseController;

    public void SelectDrawingType(int drawingID)
    {
        switch(drawingID)
        {
            case 0:
                Debug.Log("Selected square draw tile type");
                break;
            case 1:
                Debug.Log("Selected triangle draw tile type");
                break;
        }

        mouseController.SetDrawingType(drawingID);
    }

    public void SelectTileType(int textureID)
    {
        chosenId = textureID;

        switch (textureID)
        {
            case 2:
                Debug.Log("Selected grass tile type");
                break;
            case 3:
                Debug.Log("Selected rocky tile type");
                break;
            case 4:
                Debug.Log("Selected water tile type");
                break;
        }
        
        mouseController.SetTileType(textureID);
    }
}
