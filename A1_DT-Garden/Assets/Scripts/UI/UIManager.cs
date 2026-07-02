using DrawingTypes;
using Grid;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject pnlQuestions;
    [SerializeField] private Toggle[] questionsToggle;
    [SerializeField] private TMP_Dropdown[] questionsDropdowns;
    [SerializeField] private TMP_Text lblExplanation;
    [SerializeField] private Camera topDownCamera;
    [SerializeField] private Camera threeDimensionalCamera;
    [SerializeField] private GameObject pnlSecondCamera;

    private bool[] questionsAnimals = new bool[4];
    private byte[] answersDropdown = new byte[2];

    public void Start()
    {
        //inpfText = inpfPlantAmount.GetComponent<TMP_Text>();
        inpfPlantAmount?.onValueChanged.AddListener(delegate { OnTextChanged(); });
    }

    /// <summary>
    /// If the input field text sees something that isn't a number, it will remove it and only keep the numbers in the input field.
    /// </summary>
    public void OnTextChanged()
    {
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
        for(int i = 0; i < questionsToggle.Length; i++)
        {
            questionsAnimals[i] = questionsToggle[i].isOn;
        }

        for(int i = 0; i < questionsDropdowns.Length; i++)
        {
            answersDropdown[i] = (byte)questionsDropdowns[i].value;
        }

        gridManager.PassGridTilesToScore(plantAmount, questionsAnimals, answersDropdown);
    }

    public void OpenQuestions()
    {
        pnlQuestions.SetActive(true);
    }

    public void CloseQuestions()
    {
        pnlQuestions.SetActive(false);
    }

    public void ShowPillarOnEnter(int pillarIndex)
    {
        string pillar = "";

        switch (pillarIndex)
        {
            case 1:
                pillar = "Water doorloop";
                break;
            case 2:
                pillar = "Gezondheid van het bodemleven";
                break;
            case 3:
                pillar = "Diervriendelijkheid van de tuin";
                break;
            case 4:
                pillar = "Diversiteit van planten";
                break;
        }

        string pillarInfo = $"Pijler {pillarIndex}: {pillar}";
        lblExplanation.text = pillarInfo;
    }

    public void HidePillarOnExit()
    {
        lblExplanation.text = "";
    }

    public void SwitchActiveCamera()
    {
        mouseController.ResetCamera();
    }


}
