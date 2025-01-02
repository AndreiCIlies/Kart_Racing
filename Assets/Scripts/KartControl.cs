using UnityEngine;
using UnityEngine.Assertions;

public class KartControl : MonoBehaviour
{
	[SerializeField] private float maxSpeedKPH = 120;
	[SerializeField] private float steeringRange = 25;
	[SerializeField] private float steeringRangeAtMaxSpeed = 2;
	[SerializeField] private float maxMotorTorque = 150;
	[SerializeField] private float brakeTorque = 2000;

	private WheelControl[] wheels;
	private Rigidbody rigidBody;
	private float maxSpeedMPS;

	public void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
		wheels = GetComponentsInChildren<WheelControl>();

		Assert.IsNotNull(rigidBody, $"Rigidbody not found on {name}");
		Assert.IsNotNull(wheels, $"WheelControl not found on {name}");
		Assert.IsTrue(wheels.Length == 4, $"WheelControls not of length 4 on {name}. Found {wheels.Length} WheelControls instead");

		maxSpeedMPS = maxSpeedKPH * Const.KPH_TO_MPS;
	}

	public void Update()
	{
		// Input.GetAxis() smooths out the input values for keyboard input
		float vInput = Input.GetAxis("Vertical");
		float hInput = Input.GetAxis("Horizontal");

		// Calculate current speed in relation to the forward direction of the kart
		// (this returns a negative number when traveling backwards)
		float forwardSpeed = rigidBody.linearVelocity.magnitude;

		// Calculate how close the kart is to top speed as a number from zero to one
		float speedFactor = Mathf.InverseLerp(0, maxSpeedMPS, forwardSpeed);

		// Calculate how much torque is available (zero torque at top speed)
		float currentMotorTorque = MotorTorqueFn(speedFactor);

		// Calculate how much to steer (the kart steers more gently at top speed)
		float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

		// Check whether the user input is in the same as the kart's velocity
		bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

		foreach (var wheel in wheels)
		{
			// Apply steering to Wheel colliders that have "Steerable" enabled
			if (wheel.steerable)
			{
				wheel.wheelCollider.steerAngle = hInput * currentSteerRange;
			}

			if (isAccelerating)
			{
				// Apply torque to Wheel colliders that have "Motorized" enabled
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
	}

	private float MotorTorqueFn(float speedFactor)
	{
		Assert.IsTrue(speedFactor >= 0 && speedFactor <= 1, "Speed factor must be between 0 and 1");

		return Mathf.Lerp(maxMotorTorque, 0, speedFactor);
	}
}
