using UnityEngine;
using UnityEngine.Assertions;

public class KartRaceTimer : MonoBehaviour
{
	[SerializeField] private Collider finishLineCollider;
	[SerializeField] private RaceTimer raceTimer;

	private KartControl kartControl;

	private bool hasFinished = false;

	private void Start()
	{
		kartControl = GetComponent<KartControl>();

		Assert.IsNotNull(kartControl, $"KartControl not found on {name}");
		Assert.IsNotNull(finishLineCollider, $"Finish line collider is not found on {name}");

		// RaceTimer.OnRaceStart += kartControl.StartRace;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (hasFinished)
		{
			return;
		}

		if (other == finishLineCollider)
		{
			hasFinished = true; // Mark the kart as finished
			raceTimer.StopRaceTimer(true); // Call RaceTimer to display results for the player
			Debug.Log("You finished the race!");
		}
	}

	private void OnDestroy()
	{
		// RaceTimer.OnRaceStart -= kartControl.StartRace;
	}
}
