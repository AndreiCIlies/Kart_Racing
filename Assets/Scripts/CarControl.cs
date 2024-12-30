using Unity.VisualScripting;
using UnityEngine;

public class CarControl : MonoBehaviour
{
	[SerializeField] private float motorTorque = 20000;
	[SerializeField] private float brakeTorque = 200000;
	[SerializeField] private float maxSpeed = 50;
	[SerializeField] private float steeringRange = 30;
	[SerializeField] private float steeringRangeAtMaxSpeed = 5;

	private WheelControl[] wheels;
	private Rigidbody rigidBody;

	// Start is called before the first frame update
	public void Start()
	{
		rigidBody = GetComponent<Rigidbody>();

		// Find all child GameObjects that have the WheelControl script attached
		wheels = GetComponentsInChildren<WheelControl>();
	}

	// Update is called once per frame
	public void Update()
	{
		float vInput = Input.GetAxis("Vertical");
		float hInput = Input.GetAxis("Horizontal");

		// Calculate current speed in relation to the forward direction of the car
		// (this returns a negative number when traveling backwards)
		float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);


		// Calculate how close the car is to top speed
		// as a number from zero to one
		float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

		// Use that to calculate how much torque is available 
		// (zero torque at top speed)
		float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

		// �and to calculate how much to steer 
		// (the car steers more gently at top speed)
		float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

		// Check whether the user input is in the same direction 
		// as the car's velocity
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
				// If the user is trying to go in the opposite direction
				// apply brakes to all wheels
				wheel.wheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
				wheel.wheelCollider.motorTorque = 0;
			}
		}
	}
}
