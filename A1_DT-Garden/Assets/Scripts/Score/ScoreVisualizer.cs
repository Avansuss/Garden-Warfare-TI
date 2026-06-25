using UnityEngine;

public class ScoreVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject[] scoreGraphs;
    [SerializeField] private GameObject pnlParent;

    public void VisualizeScore(float[] calculatedPillars)
    {
        for (int i = 0; i < calculatedPillars.Length; i++)
        {
            float pnlHeightModifier = calculatedPillars[i] * 0.1f * 0.8f;
            scoreGraphs[i].GetComponent<RectTransform>().localScale = new Vector3(1, pnlHeightModifier, 1);
        }
    }
}
