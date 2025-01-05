using UnityEngine;
using UnityEngine.Assertions;

public class KartFollow : MonoBehaviour
{
	public enum CameraMode
	{
		FirstPerson,
		ThirdPerson
	}

	[SerializeField] private Transform kartTransform;
	[SerializeField] private Vector3 firstOffset = new(0, 1, 0);
	[SerializeField] private Vector3 firstLookAtPosition = new(0, 0, 4);
	[SerializeField] private float firstSmoothFactor = 0.0f;
	[SerializeField] private Vector3 thirdOffset = new(0, 2, -4);
	[SerializeField] private float thirdSmoothFactor = 0.8f;

	private CameraMode cameraMode;
	private Vector3 currentOffset;
	private float currentSmoothFactor;

	public void Start()
	{
		Assert.IsNotNull(kartTransform, $"Transform (target) not found on {name}");

		SetMode(CameraMode.ThirdPerson);
	}

	public void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.F1) && cameraMode != CameraMode.FirstPerson)
		{
			SetMode(CameraMode.FirstPerson);
		}
		else if (Input.GetKey(KeyCode.F2) && cameraMode != CameraMode.ThirdPerson)
		{
			SetMode(CameraMode.ThirdPerson);
		}

		Vector3 desiredPosition = kartTransform.TransformPoint(currentOffset);

		transform.position = Vector3.Lerp(desiredPosition, transform.position, currentSmoothFactor);
		transform.LookAt(CalculateLookAt());
	}

	private void SetMode(CameraMode mode)
	{
		cameraMode = mode;
		if (mode == CameraMode.FirstPerson)
		{
			currentOffset = firstOffset;
			currentSmoothFactor = firstSmoothFactor;
		}
		else if (mode == CameraMode.ThirdPerson)
		{
			currentOffset = thirdOffset;
			currentSmoothFactor = thirdSmoothFactor;
		}
	}

	private Vector3 CalculateLookAt()
	{
		if (cameraMode == CameraMode.FirstPerson)
		{
			return kartTransform.TransformPoint(firstLookAtPosition);
		}
		else if (cameraMode == CameraMode.ThirdPerson)
		{
			return kartTransform.position;
		}
		else
		{
			return Vector3.zero;
		}
	}
}