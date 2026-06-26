using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject[] scoreGraphs;
    [SerializeField] private TMP_Text[] lblScores;

    public void VisualizeScore(float[] calculatedPillars)
    {
        for (int i = 0; i < lblScores.Length; i++)
        {
            lblScores[i].text = calculatedPillars[i].ToString("F1");
        }

        for (int i = 0; i < calculatedPillars.Length; i++)
        {
            float pnlHeightModifier = calculatedPillars[i] * 0.1f * 0.8f;
            scoreGraphs[i].GetComponent<RectTransform>().localScale = new Vector3(1, pnlHeightModifier, 1);
        }
    }
}
