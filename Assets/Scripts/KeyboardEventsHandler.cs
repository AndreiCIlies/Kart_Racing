using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyboardEventsHandler : MonoBehaviour
{
	private List<float> keyPressTimes;

	public void Start()
	{
		keyPressTimes = Enumerable.Repeat(0.0f, Enum.GetValues(typeof(KeyCode)).Length).ToList();
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			KeyCode keyCode = KeyCode.R;
			if (IsDoubleKeyPress(keyCode))
			{
				ReloadScene();
			}
			keyPressTimes[(int)keyCode] = Time.time;
		}
	}

	private void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private bool IsDoubleKeyPress(KeyCode keyCode)
	{
		return Time.time <= keyPressTimes[(int)keyCode] + Const.DOUBLE_KEY_PRESS_DELTA_TIME;
	}
}