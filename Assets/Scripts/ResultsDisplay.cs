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
        resultsText = new TimerText(resultsTextUI);
        resultsText.ConfigureText(30, TextAnchor.UpperLeft);
        resultsText.SetActive(false);
    }

    public void AddResult(string playerName, string time)
    {
        results.Add($"{playerName}: {time}");
        UpdateResultsText();
    }

    public void AddText(string text)
    {
        results.Add(text);
        UpdateResultsText();
    }

    private void UpdateResultsText()
    {
        string resultsString = string.Join("\n", results);
        resultsText.SetText(resultsString);
        resultsText.SetActive(true);
    }
}