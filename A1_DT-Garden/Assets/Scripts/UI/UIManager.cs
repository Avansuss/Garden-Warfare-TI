using DrawingTypes;
using Grid;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
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

    [SerializeField] private TMP_InputField inpfPlantAmount;

    private GridTileType chosenGridTileType = GridTileType.Inactive;
    private DrawingType chosenDrawingType = DrawingType.Triangle;
    private int plantAmount = 0;

    [SerializeField] private MouseController mouseController;
    [SerializeField] private GridManager gridManager;

    public void Start()
    {
        //inpfText = inpfPlantAmount.GetComponent<TMP_Text>();
        inpfPlantAmount.onValueChanged.AddListener(delegate { OnTextChanged(); });
    }

    public void OnTextChanged()
    {
        Debug.Log(inpfPlantAmount.text);
        //[^0-9]+

        if (Regex.IsMatch(inpfPlantAmount.text, "[^0-9]+"))
        {
            Debug.Log("Non-numeric characters detected. Removing them.");
            inpfPlantAmount.text = Regex.Replace(inpfPlantAmount.text, "[^0-9]+", "");
        }
        else
        {
            if (int.TryParse(inpfPlantAmount.text, out int plantAmount))
            {
                this.plantAmount = plantAmount;
            }
            else
            {
                Debug.LogWarning("Failed to parse plant amount from input field.");
            }
        }

    }

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

    public void SetTileScore()
    {
        gridManager.PassGridTilesToScore(plantAmount);
    }
}
