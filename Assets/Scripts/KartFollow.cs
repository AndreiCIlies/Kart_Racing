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
	[SerializeField] private Vector3 firstLookAtOffset = new(0, 0, 4);
	[SerializeField] private Vector3 thirdOffset = new(0, 2, -4);
	[SerializeField] private Vector3 thirdLookAtOffset = new(0, 0, 2);
	[SerializeField] private float thirdSmoothFactor = 0.8f;

	private CameraMode cameraMode;

	public void Start()
	{
		Assert.IsNotNull(kartTransform, $"Transform (target) not found on {name}");

		cameraMode = CameraMode.ThirdPerson;
	}

	public void FixedUpdate()
	{
		bool isLookingAhead = !Input.GetKey(KeyCode.Space);
		Vector3 direction = new Vector3(1, 1, isLookingAhead ? 1 : -1);

		if (Input.GetKey(KeyCode.Alpha1))
		{
			cameraMode = CameraMode.FirstPerson;
		}
		else if (Input.GetKey(KeyCode.Alpha2))
		{
			cameraMode = CameraMode.ThirdPerson;
		}

		if (cameraMode == CameraMode.FirstPerson)
		{
			Vector3 desiredPosition = kartTransform.TransformPoint(Vector3.Scale(firstOffset, direction));
			transform.position = desiredPosition;
			transform.LookAt(kartTransform.TransformPoint(Vector3.Scale(firstLookAtOffset, direction)));
		}
		else if (cameraMode == CameraMode.ThirdPerson)
		{
			Vector3 desiredPosition = kartTransform.TransformPoint(Vector3.Scale(thirdOffset, direction));
			transform.position = Vector3.Lerp(transform.position, desiredPosition, thirdSmoothFactor);
			transform.LookAt(kartTransform.TransformPoint(Vector3.Scale(thirdLookAtOffset, direction)));
		}
	}
}