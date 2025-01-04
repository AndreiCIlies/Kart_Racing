using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ResultsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text resultsTextUI;

    private List<string> results = new List<string>(); 
    private TimerText resultsText;

    private void Start()
    {
        RectTransform rectTransform = resultsTextUI.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f); 
            rectTransform.anchoredPosition = new Vector2(0f, -10f); 
        }

        resultsText = new TimerText(resultsTextUI);
        resultsText.ConfigureText(30, TextAnchor.UpperCenter, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -10f));
        resultsText.SetActive(false);
    }

    public void AddResult(string playerName, string time)
    {
        results.Add($"{playerName}: {time}");

        UpdateResultsText();
    }

    private void UpdateResultsText()
    {
        string resultsString = string.Join("\n", results);

        resultsText.SetText(resultsString);
        resultsText.SetActive(true);
    }
}
