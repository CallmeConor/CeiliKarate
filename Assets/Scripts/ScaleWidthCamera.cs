using UnityEngine;

// Unite 2014 - 2D Best Practices In Unity

[ExecuteInEditMode]
public class ScaleWidthCamera : MonoBehaviour
{
	public int targetWidth = 540;
	public float pixelsToUnits = 10;

	private void Update()
	{
		int height = Mathf.RoundToInt(targetWidth / (float)Screen.width * Screen.height);

		Camera.main.orthographicSize = height / pixelsToUnits / 2;
	}
}
