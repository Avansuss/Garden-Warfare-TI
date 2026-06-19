using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //references to the UI elements
    //https://www.flaticon.com/free-icon/paint-bucket_483918
    //https://www.flaticon.com/free-icon/download_724933

    public List<Button> drawingTypeButtons;
    public int chosenId = 0;

    public void SelectDrawingType()
    {

    }

    public void SelectTileType(int btnTextureID)
    {
        switch(btnTextureID)
        {
            case 2:
                chosenId = btnTextureID;
                Debug.Log("Selected grass tile type");
                break;

        }
    }
}
