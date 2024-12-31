using UnityEngine;
using UnityEngine.Assertions;

public class CarFollow : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private Vector3 offset = new(0, 5, -10);
	[SerializeField] private float smoothFactor = 0.1f;

	public void Start()
	{
		Assert.IsNotNull(target, "The CarFollow component needs a target.");
	}

	public void FixedUpdate()
	{
		Vector3 desiredPosition = target.TransformPoint(offset);

		transform.position = Vector3.Lerp(desiredPosition, transform.position, smoothFactor);
		transform.LookAt(target);
	}
}