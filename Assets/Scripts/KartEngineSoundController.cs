using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Assertions;

public class KartEngineSoundController : MonoBehaviour
{
	[SerializeField] private float minPitch = 0.8f;
	[SerializeField] private float maxPitch = 2.5f;
	[SerializeField] private float accelerationRate = 1.05f;
	[SerializeField] private float decelerationRate = 0.965f;

	[SerializeField] private KeyCode forwardKey1 = KeyCode.W;
	[SerializeField] private KeyCode forwardKey2 = KeyCode.UpArrow;

	private AudioSource engineSound;
	private Rigidbody kartRigidbody;

	private float maxSpeedMPS;

	private void Start()
	{
		engineSound = GetComponent<AudioSource>();
		kartRigidbody = GetComponent<Rigidbody>();

		Assert.IsNotNull(engineSound, $"AudioSource not found on {name}");
		Assert.IsNotNull(kartRigidbody, $"Rigidbody not found on {name}");

		if (TryGetComponent<KartControl>(out KartControl kartControl))
		{
			maxSpeedMPS = kartControl.GetMaxSpeedKPH() * Const.KPH_TO_MPS;
		}
		else
		{
			maxSpeedMPS = 120 * Const.KPH_TO_MPS;
		}

		engineSound.loop = true;
		engineSound.Play();
	}

	private void Update()
	{
		bool isAccelerating = Input.GetKey(forwardKey1) || Input.GetKey(forwardKey2);

		float speed = kartRigidbody.linearVelocity.magnitude;

		if (isAccelerating)
		{
			float speedFactor = Mathf.InverseLerp(0, maxSpeedMPS, speed);
			float targetPitch = Mathf.Lerp(minPitch, maxPitch, speedFactor);
			engineSound.pitch = Mathf.Min(targetPitch, engineSound.pitch * accelerationRate);
		}
		else
		{
			engineSound.pitch = Mathf.Max(minPitch, engineSound.pitch * decelerationRate);
		}
	}
}
