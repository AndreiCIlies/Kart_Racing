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
	[SerializeField] private Vector3 thirdOffset = new(0, 2, -4);
	[SerializeField] private float thirdSmoothFactor = 0.8f;

	private CameraMode cameraMode;

	public void Start()
	{
		Assert.IsNotNull(kartTransform, $"Transform (target) not found on {name}");

		cameraMode = CameraMode.ThirdPerson;
	}

	public void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.F1))
		{
			cameraMode = CameraMode.FirstPerson;
		}
		else if (Input.GetKey(KeyCode.F2))
		{
			cameraMode = CameraMode.ThirdPerson;
		}

		if (cameraMode == CameraMode.FirstPerson)
		{
			Vector3 desiredPosition = kartTransform.TransformPoint(firstOffset);
			transform.position = desiredPosition;
			transform.LookAt(kartTransform.TransformPoint(firstLookAtPosition));
		}
		else if (cameraMode == CameraMode.ThirdPerson)
		{
			Vector3 desiredPosition = kartTransform.TransformPoint(thirdOffset);
			transform.position = Vector3.Lerp(transform.position, desiredPosition, thirdSmoothFactor);
			transform.LookAt(kartTransform.position);
		}
	}
}