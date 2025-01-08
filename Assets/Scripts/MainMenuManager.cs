using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
	[SerializeField] private GameObject mainMenuPanel;
	[SerializeField] private GameObject controlsPanel;
	[SerializeField] private GameObject authorsPanel;

	public void ShowControlsPanel()
	{
		mainMenuPanel.SetActive(false);
		controlsPanel.SetActive(true);
		authorsPanel.SetActive(false);
	}

	public void ShowMainMenuPanel()
	{
		controlsPanel.SetActive(false);
		mainMenuPanel.SetActive(true);
		authorsPanel.SetActive(false);
	}

	public void ShowAuthorsPanel()
	{
		mainMenuPanel.SetActive(false);
		controlsPanel.SetActive(false);
		authorsPanel.SetActive(true);
	}

	public void StartGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("RaceScene");
	}
}