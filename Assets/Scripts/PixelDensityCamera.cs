using UnityEngine;

// Unite 2014 - 2D Best Practices In Unity

[ExecuteInEditMode]
public class PixelDensityCamera : MonoBehaviour
{
	public float pixelsToUnits = 10;

	void Update()
	{
		Camera.main.orthographicSize = Screen.height / pixelsToUnits / 2;
	}
}
