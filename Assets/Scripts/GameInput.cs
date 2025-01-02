using UnityEngine;

public class GameInput : MonoBehaviour
{
	public static GameInput Instance { get; private set; }

	private InputSystem_Actions inputActions;

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("Multiple instances of GameInput found!");
			return;
		}
		Instance = this;

		inputActions = new InputSystem_Actions();
		inputActions.Player.Enable();
	}

	public Vector2 GetInputVectorNormalized()
	{
		return inputActions.Player.Move.ReadValue<Vector2>();
	}
}
