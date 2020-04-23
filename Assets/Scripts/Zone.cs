using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Zone : MonoBehaviour
{
	BoxCollider2D zoneCollider;

	public List<DancePartner> dancePartners;

	Player player;

	void Awake()
	{
		zoneCollider = GetComponent<BoxCollider2D>();
		dancePartners = new List<DancePartner>();

		InvokeRepeating("CheckAllDancersSatisfied", 2.0f, 0.2f);
	}

	void CheckAllDancersSatisfied()
	{
		if (player)
		{
			bool allSatisfied = true;
			DancePartner satisfied = null;
			foreach (DancePartner dancePartner in dancePartners)
			{
				if (dancePartner.IsSatisfied)
				{
					satisfied = dancePartner;
				}
				else if (allSatisfied)
				{
					allSatisfied = false;
				}
			}
			dancePartners.Remove(satisfied);
			if (allSatisfied)
			{
				player.CanProgress(true);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!player)
		{
			player = other.GetComponent<Player>();
			if (player)
			{
				player.SetPlayerZone(this);
			}
		}
		DancePartner dancePartner = other.GetComponent<DancePartner>();
		if (dancePartner)
		{
			dancePartners.Add(dancePartner);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (player)
		{
			var playerExit = other.GetComponent<Player>();
			if(playerExit)
			{
				player = null;
			}
		}
	}

	public float GetMinX()
	{
		return zoneCollider.bounds.min.x;
	}

	public float GetMaxX()
	{
		return zoneCollider.bounds.max.x;
	}
}