using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
	private Renderer beatRenderer;
	bool hasBeenOnCamera = false;

	void Start()
	{
		beatRenderer = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		if (!VisibleToMainCamera() && hasBeenOnCamera)
		{
			Destroy(gameObject);
		}
		else if(VisibleToMainCamera() && !hasBeenOnCamera)
		{
			hasBeenOnCamera = true;
		}

		transform.localPosition += Vector3.left * BeatManager.Instance.SongTempo * Time.deltaTime;
	}

	bool VisibleToMainCamera()
	{
		Plane[] mainCamFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		return GeometryUtility.TestPlanesAABB(mainCamFrustumPlanes, beatRenderer.bounds);
	}
}
