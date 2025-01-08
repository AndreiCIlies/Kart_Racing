using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class CheckpointManager : MonoBehaviour
{
	public List<Collider> checkpoints { get; private set; }

	private void Start()
	{
		checkpoints = new List<Collider>(GetComponentsInChildren<Collider>());
		Assert.IsNotNull(checkpoints, $"Checkpoints not found on {name}");
		Assert.IsFalse(checkpoints.Count == 0, $"No checkpoints found on {name}");
	}

	private void Update()
	{

	}
}
