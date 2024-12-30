using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class WheelControl : MonoBehaviour
{
	[SerializeField] private Transform wheelTransform;
	public bool steerable;
	public bool motorized;

	[HideInInspector] public WheelCollider wheelCollider;

	public void Start()
	{
		wheelCollider = GetComponent<WheelCollider>();
		Assert.IsNotNull(wheelCollider, "WheelCollider not found on " + name);
	}

	public void Update()
	{
		wheelCollider.GetWorldPose(out _, out Quaternion rotation);

		wheelTransform.rotation = rotation * Quaternion.Euler(0, 0, 90);
	}
}
