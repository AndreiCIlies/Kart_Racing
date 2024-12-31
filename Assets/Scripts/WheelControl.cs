using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class WheelControl : MonoBehaviour
{
	[SerializeField] private Transform wheelTransform;
	public bool steerable;
	public bool motorized;
	[SerializeField] private Vector3 additionalRotationEuler;
	[SerializeField] private bool debug = false;

	[HideInInspector] public WheelCollider wheelCollider;

	private Quaternion additionalRotation;

	public void Start()
	{
		wheelCollider = GetComponent<WheelCollider>();
		Assert.IsNotNull(wheelCollider, "WheelCollider not found on " + name);

		additionalRotation = Quaternion.Euler(additionalRotationEuler);
	}

	public void Update()
	{
		wheelCollider.GetWorldPose(out _, out Quaternion rotation);

		wheelTransform.rotation = rotation * additionalRotation;

		if (debug)
		{
			Debug.Log(wheelTransform.rotation.eulerAngles);
		}
	}
}
