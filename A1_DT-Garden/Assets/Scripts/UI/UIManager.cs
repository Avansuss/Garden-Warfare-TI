using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //references to the UI elements
    //https://www.flaticon.com/free-icon/paint-bucket_483918
    //https://www.flaticon.com/free-icon/download_724933

    public int chosenId = 0;

    [SerializeField]
    private MouseController mouseController;

    public void SelectDrawingType()
    {

    }

    public void SelectTileType(int btnTextureID)
    {
        chosenId = btnTextureID;

        switch (btnTextureID)
        {
            case 2:
                Debug.Log("Selected grass tile type");
                break;
            case 3:
                Debug.Log("Selected rocky tile type");
                break;
        }
        
        mouseController.SetTileType(btnTextureID);
    }
}
