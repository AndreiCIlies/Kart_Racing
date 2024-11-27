using UnityEngine;

public class CarController : MonoBehaviour
{
	public enum DriveMode
	{
		Front,
		Rear,
		All
	};

	[SerializeField] private float idealRPM = 500f;
	[SerializeField] private float maxRPM = 1000f;

	[SerializeField] private Transform centerOfGravity;

	[SerializeField] private WheelCollider wheelFR;
	[SerializeField] private WheelCollider wheelFL;
	[SerializeField] private WheelCollider wheelRR;
	[SerializeField] private WheelCollider wheelRL;

	[SerializeField] private float turnRadius = 6f;
	[SerializeField] private float torque = 25f;
	[SerializeField] private float brakeTorque = 100f;

	[SerializeField] private float AntiRoll = 20000.0f;

	[SerializeField] private DriveMode driveMode = DriveMode.Rear;

	private Rigidbody rigidBody;

	// public Text speedText;


	private void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		Vector2 input = GameInput.Instance.GetInputVectorNormalized() / Time.deltaTime;

		/*
		if (speedText != null)
		{
			speedText.text = "Speed: " + Speed().ToString("f0") + " km/h";
		}
		*/

		Debug.Log("Speed: " + CalculateSpeed().ToString("f0") + "km/h    RPM: " + GetRpm());

		float scaledTorque = input.y > 0 ? input.y * torque : 0;

		if (wheelRL.rpm < idealRPM)
		{
			scaledTorque = Mathf.Lerp(scaledTorque / 10f, scaledTorque, wheelRL.rpm / idealRPM);
		}
		else
		{
			scaledTorque = Mathf.Lerp(scaledTorque, 0, (wheelRL.rpm - idealRPM) / (maxRPM - idealRPM));
		}

		DoRollBar(wheelFR, wheelFL);
		DoRollBar(wheelRR, wheelRL);

		wheelFR.steerAngle = input.x * turnRadius;
		wheelFL.steerAngle = input.x * turnRadius;

		wheelFR.motorTorque = driveMode == DriveMode.Rear ? 0 : scaledTorque;
		wheelFL.motorTorque = driveMode == DriveMode.Rear ? 0 : scaledTorque;
		wheelRR.motorTorque = driveMode == DriveMode.Front ? 0 : scaledTorque;
		wheelRL.motorTorque = driveMode == DriveMode.Front ? 0 : scaledTorque;

		if (input.y < 0)
		{
			wheelFR.brakeTorque = brakeTorque;
			wheelFL.brakeTorque = brakeTorque;
			wheelRR.brakeTorque = brakeTorque;
			wheelRL.brakeTorque = brakeTorque;
		}
		else
		{
			wheelFR.brakeTorque = 0;
			wheelFL.brakeTorque = 0;
			wheelRR.brakeTorque = 0;
			wheelRL.brakeTorque = 0;
		}
	}

	void DoRollBar(WheelCollider WheelL, WheelCollider WheelR)
	{
		float travelL = 1.0f;
		float travelR = 1.0f;
		WheelHit hit;

		bool groundedL = WheelL.GetGroundHit(out hit);
		if (groundedL)
		{
			float localY = -WheelL.transform.InverseTransformPoint(hit.point).y;
			travelL = (localY - WheelL.radius) / WheelL.suspensionDistance;
		}

		bool groundedR = WheelR.GetGroundHit(out hit);
		if (groundedR)
		{
			float localY = -WheelR.transform.InverseTransformPoint(hit.point).y;
			travelR = (localY - WheelR.radius) / WheelR.suspensionDistance;
		}

		float antiRollForce = (travelL - travelR) * AntiRoll;

		if (groundedL)
		{
			rigidBody.AddForceAtPosition(WheelL.transform.up * -antiRollForce, WheelL.transform.position);
		}
		if (groundedR)
		{
			rigidBody.AddForceAtPosition(WheelR.transform.up * antiRollForce, WheelR.transform.position);
		}
	}

	public float CalculateSpeed()
	{
		return wheelRR.radius * Mathf.PI * wheelRR.rpm * 60f / 1000f;
	}

	public float GetRpm()
	{
		return wheelRR.rpm;
	}
}
