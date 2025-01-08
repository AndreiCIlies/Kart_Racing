using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System;

public class CheckpointManager : MonoBehaviour
{
	private List<Checkpoint> checkpoints;
	private int lastCheckpointIndex = -1;

	private void Start()
	{
		checkpoints = new List<Checkpoint>(GetComponentsInChildren<Checkpoint>());

		Assert.IsNotNull(checkpoints, $"Checkpoints not found on {name}");
		Assert.IsTrue(checkpoints.Count == 19, $"Checkpoint count doesn't match with the expected value on {name}. Expected {19} but found {checkpoints.Count}");
	}

	public bool SetNextCheckpoint(int index)
	{
		if (lastCheckpointIndex + 1 == index)
		{
			lastCheckpointIndex = index;
			return true;
		}
		else if (lastCheckpointIndex == index)
		{
			Debug.Log($"Player respawned at checkpoint {index}");
			return true;
		}
		else
		{
			Debug.Log($"Checkpoint {index} is not the next checkpoint after checkpoint {lastCheckpointIndex}");
			return false;
		}
	}
	public Tuple<Vector3, Quaternion> GetRespawnPositionAndRotation()
	{
		if (lastCheckpointIndex == -1)
		{
			Debug.Log("No checkpoint reached yet");
			return null;
		}

		return new Tuple<Vector3, Quaternion>(
			checkpoints[lastCheckpointIndex].transform.position + new Vector3(0, 1, 0),
			checkpoints[lastCheckpointIndex].transform.rotation * Quaternion.Euler(0, -90, 0));
	}
}
