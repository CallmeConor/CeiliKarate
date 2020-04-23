using UnityEngine;

public class DancePartnerDetector : MonoBehaviour
{
	public enum DetectorSide
	{
		Right,
		Left
	}
	public DetectorSide side;

	void Awake()
	{
		if(name.Contains("Right"))
		{
			side = DetectorSide.Right;
		}
		else
		{
			side = DetectorSide.Left;
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.GetComponent<DancePartner>())
		{
			Player.Instance.TryGiveDancePartner(collision.GetComponent<DancePartner>(), side);
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<DancePartner>())
		{
			Player.Instance.TryRemoveDancePartner(collision.GetComponent<DancePartner>());
		}
	}
}
