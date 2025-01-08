using UnityEngine;
using UnityEngine.SceneManagement;

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
		SceneManager.LoadScene("RaceScene");
	}

	public void QuitGame()
	{
		Debug.Log("Quit game");
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}