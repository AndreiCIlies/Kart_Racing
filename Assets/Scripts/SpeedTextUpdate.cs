using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class SpeedTextUpdate : MonoBehaviour
{

	[SerializeField] private Rigidbody kartRigidBody;

	private TextMeshProUGUI textMesh;

	public void Start()
	{
		textMesh = GetComponent<TextMeshProUGUI>();

		Assert.IsNotNull(textMesh, $"TextMesh not found on {name}");
		Assert.IsNotNull(kartRigidBody, $"KartControl not found on {name}");
	}

	public void Update()
	{
		int speed = Mathf.RoundToInt(kartRigidBody.linearVelocity.magnitude * Const.MPS_TO_KPH);
		textMesh.text = $"{speed} km/h";
	}
}