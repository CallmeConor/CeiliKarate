using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public TextMeshProUGUI proceedText;
	private Slider fameBonusSlider;
	public int FameBonus { get { return (fameBonusSlider) ? (int)fameBonusSlider.value : 0; } }
	public static Player Instance;
	public DancePartner dancePartner;

	Vector2 expectedDir = Vector2.zero;

	public Zone zone;

	public bool canProgress;
	public bool fightingBoss;

	void Awake()
	{
		Instance = this;
		fameBonusSlider = FindObjectOfType<Slider>();
		canProgress = false;
		fightingBoss = false;
	}

	public void SetPlayerZone(Zone inZone)
	{
		CanProgress(false);
		zone = inZone;
		var pos = Camera.main.transform.position;
		pos.x = zone.transform.position.x;
		Camera.main.transform.position = pos;

		Beat[] beatsOnScreen = GameObject.FindObjectsOfType<Beat>();
		for(int i = 0; i < beatsOnScreen.Length; i++)
		{
			var beatPos = beatsOnScreen[i].transform.position;
			beatPos.x += 10.8f;
			beatsOnScreen[i].transform.position = beatPos;
		}
		beatsOnScreen = null;
	}

	public void LoseFame()
	{
		Mathf.Clamp(fameBonusSlider.value -= 10f, 0, 100);
	}

	public void CanProgress(bool inBool)
	{
		canProgress = inBool;
		proceedText.gameObject.SetActive(canProgress);
	}

	public void MovePlayer(Vector2 moveDir, int fameSliderValue)
	{
		if (dancePartner && Vector2.Distance(moveDir, expectedDir) == 0f)
		{
			Vector3 tempPos = transform.position;
			transform.position = dancePartner.transform.position;
			dancePartner.transform.position = tempPos;
			dancePartner.IncreaseSatisfaction();
			if (!dancePartner.IsDanceable) { dancePartner = null; }
			return;
		}

		Vector3 pos = transform.position;
		moveDir *= 0.5f;

		if (pos.x + moveDir.x < zone.GetMinX()
			|| (pos.x + moveDir.x > zone.GetMaxX() && !canProgress)
			|| pos.y + moveDir.y > 2.5f
			|| pos.y + moveDir.y < -0.5f)
		{
			return;
		}

		pos += new Vector3(moveDir.x, moveDir.y, 0f);
		transform.position = pos;
		Mathf.Clamp(fameBonusSlider.value += fameSliderValue, 0, 100);
	}

	public void TryGiveDancePartner(DancePartner inDancePartner, DancePartnerDetector.DetectorSide side)
	{
		if(!dancePartner && inDancePartner.IsDanceable)
		{
			dancePartner = inDancePartner;

			if(side == DancePartnerDetector.DetectorSide.Right)
			{
				expectedDir = Vector2.right;
			}
			else
			{
				expectedDir = Vector2.left;
			}
		}
	}

	public void TryRemoveDancePartner(DancePartner inDancePartner)
	{
		if (dancePartner && dancePartner == inDancePartner)
		{
			dancePartner = null;
		}
	}

}
