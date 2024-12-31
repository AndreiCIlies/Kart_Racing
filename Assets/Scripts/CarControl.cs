using UnityEngine;
using UnityEngine.Assertions;

public class CarControl : MonoBehaviour
{
	[SerializeField] private float motorTorque = 500;
	[SerializeField] private float brakeTorque = 2000;
	[SerializeField] private float maxSpeed = 50;
	[SerializeField] private float steeringRange = 30;
	[SerializeField] private float steeringRangeAtMaxSpeed = 5;

	private WheelControl[] wheels;
	private Rigidbody rigidBody;

	public void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
		wheels = GetComponentsInChildren<WheelControl>();
	}

	public void Update()
	{
		// Input.GetAxis() smooths out the input values for keyboard input
		float vInput = Input.GetAxis("Vertical");
		float hInput = Input.GetAxis("Horizontal");

		// Calculate current speed in relation to the forward direction of the car
		// (this returns a negative number when traveling backwards)
		float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);

		// Calculate how close the car is to top speed as a number from zero to one
		float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

		// Calculate how much torque is available (zero torque at top speed)
		float currentMotorTorque = MotorTorqueFn(speedFactor);

		// Calculate how much to steer (the car steers more gently at top speed)
		float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

		// Check whether the user input is in the same as the car's velocity
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

		return Mathf.Lerp(motorTorque, 0, speedFactor);
		// return motorTorque / 10;
	}
}
