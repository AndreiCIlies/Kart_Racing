using UnityEngine;
using TMPro;
using System.Collections;

public class RaceTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownTextUI;
    [SerializeField] private TMP_Text timerTextUI;
    [SerializeField] private ResultsDisplay resultsDisplay;

    private TimerText countdownText;
    private TimerText timerText;

    private float raceTime = 0f;
    private bool isRunning = false;

    private float playerFinishTime = -1f;
    private float aiFinishTime = -1f;

    public static event System.Action OnRaceStart;

    private void Start()
    {
        countdownText = new TimerText(countdownTextUI);
        timerText = new TimerText(timerTextUI);

        countdownText.ConfigureText(100, TextAnchor.MiddleCenter);
        timerText.ConfigureText(50, TextAnchor.UpperRight);

        timerText.SetActive(false);
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        int countdown = 3;

        while (countdown > 0)
        {
            countdownText.SetText(countdown.ToString());
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.SetText("GO!");
        yield return new WaitForSeconds(1f);
        countdownText.SetActive(false);

        StartRaceTimer();
    }

    private void StartRaceTimer()
    {
        isRunning = true;
        timerText.SetActive(true);

        OnRaceStart?.Invoke();
    }

    private void Update()
    {
        if (isRunning)
        {
            raceTime += Time.deltaTime;
            timerText.SetText(FormatTime(raceTime));
        }
    }

    public void StopRaceTimer(bool isPlayer)
    {
        if (isPlayer && playerFinishTime < 0f)
        {
            playerFinishTime = raceTime;
            Debug.Log($"You finished with time: {FormatTime(playerFinishTime)}");
        }
        else if (!isPlayer && aiFinishTime < 0f)
        {
            aiFinishTime = raceTime;
            Debug.Log($"AI finished with time: {FormatTime(aiFinishTime)}");
        }

        // Check if both have finished
        if (playerFinishTime > 0f && aiFinishTime > 0f)
        {
            isRunning = false;

            if (resultsDisplay != null)
            {
                resultsDisplay.AddText("Results:\n");
                resultsDisplay.AddResult("You", FormatTime(playerFinishTime));
                resultsDisplay.AddResult("AI", FormatTime(aiFinishTime));

                string winner = playerFinishTime < aiFinishTime ? "You" : "AI";
                resultsDisplay.AddText($"\n{winner} won!");
            }
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);
        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}