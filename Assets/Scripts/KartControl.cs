using UnityEngine;
using UnityEngine.Assertions;

public class KartControl : MonoBehaviour
{
	[SerializeField] private float maxSpeedKPH = 120;
	[SerializeField] private float steeringRange = 25;
	[SerializeField] private float steeringRangeAtMaxSpeed = 2;
	[SerializeField] private float maxMotorTorque = 150;
	[SerializeField] private float brakeTorque = 2000;
	[SerializeField] private RaceTimer raceTimer; // Reference to RaceTimer
	[SerializeField] private Collider finishLine; // Reference to the finish line collider

	private WheelControl[] wheels;
	private Rigidbody rigidBody;
	private float maxSpeedMPS;
	private bool isRaceStarted = false;
	private bool hasFinished = false; // To ensure the results are displayed only once

	public void Start()
	{
		// Initialize required components
		rigidBody = GetComponent<Rigidbody>();
		wheels = GetComponentsInChildren<WheelControl>();

		// Assertions to ensure essential components are assigned
		Assert.IsNotNull(rigidBody, $"Rigidbody not found on {name}");
		Assert.IsNotNull(wheels, $"WheelControl not found on {name}");
		Assert.IsTrue(wheels.Length == 4, $"WheelControls not of length 4 on {name}. Found {wheels.Length} WheelControls instead");
		Assert.IsNotNull(finishLine, $"Finish line collider is not assigned on {name}");

		// Convert speed from KPH to MPS
		maxSpeedMPS = maxSpeedKPH * Const.KPH_TO_MPS;

		// Subscribe to the race start event
		RaceTimer.OnRaceStart += StartRace;

		// Apply brakes initially to keep the kart stationary
		ApplyBrakes();
	}

	public void Update()
	{
		// If the race hasn't started, prevent movement
		if (!isRaceStarted)
		{
			ApplyBrakes();
			return;
		}

		// Get inputs for acceleration and steering
		float vInput = Input.GetAxis("Vertical");
		float hInput = Input.GetAxis("Horizontal");

		// Calculate speed and steering angle
		float forwardSpeed = Vector3.Dot(rigidBody.linearVelocity, transform.forward);
		float speedFactor = Mathf.InverseLerp(0, maxSpeedMPS, Mathf.Abs(forwardSpeed));

		float currentMotorTorque = MotorTorqueFn(speedFactor);
		float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

		// Determine if acceleration direction matches movement direction
		bool isSameDirection = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed) || Mathf.Abs(forwardSpeed) < 0.1f;

		// Update wheel behavior
		foreach (var wheel in wheels)
		{
			if (wheel.steerable)
			{
				wheel.wheelCollider.steerAngle = hInput * currentSteerRange;
			}

			if (Mathf.Abs(vInput) > 0.01f)
			{
				if (isSameDirection)
				{
					if (wheel.motorized)
					{
						wheel.wheelCollider.motorTorque = vInput * currentMotorTorque;
					}
					wheel.wheelCollider.brakeTorque = 0;
				}
				else
				{
					wheel.wheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
					wheel.wheelCollider.motorTorque = 0;
				}
			}
			else
			{
				wheel.wheelCollider.brakeTorque = 0;
				wheel.wheelCollider.motorTorque = 0;
			}
		}
	}

	private void ApplyBrakes()
	{
		// Apply brakes to all wheels
		foreach (var wheel in wheels)
		{
			wheel.wheelCollider.brakeTorque = brakeTorque;
			wheel.wheelCollider.motorTorque = 0;
		}
	}

	private void ReleaseBrakes()
	{
		// Release brakes for all wheels
		foreach (var wheel in wheels)
		{
			wheel.wheelCollider.brakeTorque = 0;
			wheel.wheelCollider.motorTorque = 0;
		}

		// Reset Rigidbody velocity
		rigidBody.linearVelocity = Vector3.zero;
		rigidBody.angularVelocity = Vector3.zero;
	}

	private float MotorTorqueFn(float speedFactor)
	{
		// Calculate motor force based on speed
		Assert.IsTrue(speedFactor >= 0 && speedFactor <= 1, "Speed factor must be between 0 and 1");
		return Mathf.Lerp(maxMotorTorque, 0, speedFactor);
	}

	private void StartRace()
	{
		// Allow kart movement
		isRaceStarted = true;
		ReleaseBrakes();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (hasFinished) return; // Prevent multiple triggers

		if (other == finishLine) // Check if the finish line collider is hit
		{
			hasFinished = true; // Mark the kart as finished
			raceTimer.StopRaceTimer(true); // Call RaceTimer to display results for the player
			Debug.Log("Player finished the race!");
		}
	}

	private void OnDestroy()
	{
		// Unsubscribe from events to prevent errors
		RaceTimer.OnRaceStart -= StartRace;
	}
}
