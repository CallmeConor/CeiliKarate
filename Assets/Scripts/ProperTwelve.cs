using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProperTwelve : MonoBehaviour
{
	private Renderer twelveRenderer;

	Vector3 dir = Vector3.left;

	void Start()
	{
		twelveRenderer = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		if (!VisibleToMainCamera())
		{
			Destroy(gameObject);
		}

		transform.localPosition += dir * BeatManager.Instance.SongTempo * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		Player player = collision.GetComponent<Player>();
		if (player)
		{
			player.LoseFame();
			Destroy(this.gameObject);
		}
	}

	bool VisibleToMainCamera()
	{
		Plane[] mainCamFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		return GeometryUtility.TestPlanesAABB(mainCamFrustumPlanes, twelveRenderer.bounds);
	}

	public void SetDir(Vector3 inDir)
	{
		dir = inDir;
	}
}
