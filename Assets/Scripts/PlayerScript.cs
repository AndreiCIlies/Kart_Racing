using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	[SerializeField] private float speed = 5f;

	void Update()
	{
		Vector2 inputVector = GameInput.Instance.GetInputVectorNormalized();
		transform.position += Time.deltaTime * speed * new Vector3(inputVector.x, 0, inputVector.y);
	}
}
