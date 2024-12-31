using UnityEngine;
using UnityEngine.Assertions;

public class WheelSkid : MonoBehaviour
{
	[SerializeField] private WheelCollider wheelCollider;
	[SerializeField] private float slipThreshold = 1.0f;

	private TrailRenderer trailRenderer;

	public void Start()
	{
		trailRenderer = GetComponent<TrailRenderer>();

		Assert.IsNotNull(trailRenderer, "No component of type TrailRenderer found for WheelSkid.");
		Assert.IsNotNull(wheelCollider, "Wheel Collider is not assigned.");

		trailRenderer.emitting = false;
	}

	public void Update()
	{
		trailRenderer.emitting = IsWheelSliding();
	}

	private bool IsWheelSliding()
	{
		if (wheelCollider.GetGroundHit(out WheelHit wheelHit))
		{
			if (Mathf.Abs(wheelHit.forwardSlip) > slipThreshold)
			{
				return true;
			}
			if (Mathf.Abs(wheelHit.sidewaysSlip) > slipThreshold)
			{
				return true;
			}
		}

		return false;
	}
}