using UnityEngine;

public class CheckerboardGenerator : MonoBehaviour
{
	[SerializeField] private Material whiteMaterial;
	[SerializeField] private Material blackMaterial;
	[SerializeField] private float tileSize = 1;

	void Start()
	{
		GenerateCheckerboard();
	}

	void GenerateCheckerboard()
	{
		float planeWidth = transform.localScale.x * 10;
		float planeHeight = transform.localScale.z * 10;

		int tilesX = Mathf.FloorToInt(planeWidth / tileSize);
		int tilesZ = Mathf.FloorToInt(planeHeight / tileSize);

		float offsetX = -planeWidth / 2 + tileSize / 2;
		float offsetZ = -planeHeight / 2 + tileSize / 2;

		for (int x = 0; x < tilesX; x++)
		{
			for (int z = 0; z < tilesZ; z++)
			{
				float posX = x * tileSize + offsetX;
				float posZ = z * tileSize + offsetZ;

				GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Quad);
				tile.transform.position = new Vector3(posX, gameObject.transform.position.z + 0.01f, posZ);
				tile.transform.localScale = new Vector3(tileSize, tileSize, 1);
				tile.transform.rotation = Quaternion.Euler(90, 0, 0);

				if ((x + z) % 2 == 0)
				{
					tile.GetComponent<Renderer>().material = whiteMaterial;
				}
				else
				{
					tile.GetComponent<Renderer>().material = blackMaterial;
				}

				tile.transform.parent = transform;
			}
		}
	}
}
