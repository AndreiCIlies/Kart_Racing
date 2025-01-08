using UnityEngine;
using UnityEngine.Assertions;

public class Checkpoint : MonoBehaviour
{
	[SerializeField] private CheckpointManager checkpointManager;
	[SerializeField] private int index;

	private void Start()
	{
		Assert.IsNotNull(checkpointManager, $"CheckpointManager not assigned on {name}");
		Assert.IsTrue(name.EndsWith((index + 1).ToString()), $"Checkpoint name {name} doesn't match index {index}");
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log($"Player reached checkpoint {index}");
			checkpointManager.SetNextCheckpoint(index);
		}
	}
}
