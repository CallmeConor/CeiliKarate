using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance;

	public TextMeshProUGUI moneyText;
	public Zone[] zones;

	public int playerMoney = 0;

	public class DanceContribution
	{
		public float secondsToPay;
		public int rateToPay;
	}
	public List<DanceContribution> danceContributions;

	void Start()
	{
		Instance = this;
		AddMoney(0);
		danceContributions = new List<DanceContribution>();

		InvokeRepeating("UpdatePlayerMoney", 2.0f, BeatManager.Instance.SongTempo * 0.25f);
	}

	void UpdatePlayerMoney()
	{
		if (!Player.Instance.fightingBoss)
		{
			List<DanceContribution> emptyContributions = new List<DanceContribution>();
			foreach (DanceContribution contribution in danceContributions)
			{
				contribution.secondsToPay -= BeatManager.Instance.SongTempo * 0.25f;
				if(contribution.secondsToPay < 0)
				{
					emptyContributions.Add(contribution);
				}
				AddMoney(Mathf.CeilToInt(contribution.rateToPay * (Player.Instance.FameBonus * 0.01f)));
			}
			foreach(DanceContribution empty in emptyContributions)
			{
				danceContributions.Remove(empty);
			}
		}
	}

	void AddMoney(int amount)
	{
		playerMoney += amount;
		moneyText.text = "€" + playerMoney;
	}

	public void AddToMoney(DanceContribution danceContribution)
	{
		Debug.Log("New contribution: " + danceContribution.rateToPay + ", " + danceContribution.secondsToPay);
		danceContributions.Add(danceContribution);
	}
}