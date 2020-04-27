using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using System.Collections;

public class DancePartner : MonoBehaviour
{
	public ProperTwelve ProperTwelve;
	private bool danceable = true;
	public bool IsDanceable { get { return danceable; } }

	private float satisfaction = 0;
	public bool IsSatisfied { get { return satisfaction == 100; } }

	private TextMeshProUGUI textmesh;
	private ParticleSystem danceParticleSystem;
	private EmissionModule emissionModule;
	private SpriteRenderer spriteRenderer;
	public enum EnemyType
	{
		OldMan,
		Thrower,
		Looper,
		Boss
	}
	public EnemyType enemyType;

	private Color satisfiedColour;

	int bossStage = 0;

	public Transform[] walkPoints;

	void Awake()
	{
		if (!spriteRenderer)
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
		if(!danceParticleSystem)
		{
			danceParticleSystem = GetComponentInChildren<ParticleSystem>();
			emissionModule = danceParticleSystem.emission;
		}
		textmesh = GetComponentInChildren<TextMeshProUGUI>();
	}

	void Start()
	{
		ColorUtility.TryParseHtmlString("#6EFF4C", out satisfiedColour);
		spriteRenderer.color = BeatManager.Instance.enemyColours[(int)enemyType];
		
		if(enemyType == EnemyType.Thrower)
		{
			InvokeRepeating("ThrowProperTwelve", 0.0f, BeatManager.Instance.SongTempo * 0.5f);
		}
		if(enemyType == EnemyType.Looper)
		{
			StartCoroutine("MoveThroughWalkPoints");
			InvokeRepeating("LooperCooldown", 0.0f, BeatManager.Instance.SongTempo * 2f);
		}
		else if(enemyType == EnemyType.Boss)
		{
			danceable = false;
			InvokeRepeating("ThrowProperTwelveBoss", 0.0f, BeatManager.Instance.SongTempo * 0.5f);
			InvokeRepeating("BossCooldown", 0.0f, BeatManager.Instance.SongTempo * 2f);
		}
	}

	void LooperCooldown()
	{
		if (!danceable)
		{
			danceable = true;
			StopCoroutine("MoveThroughWalkPoints");
		}
		else
		{
			danceable = false;
			StartCoroutine("MoveThroughWalkPoints");
		}
	}

	void BossCooldown()
	{
		if (Player.Instance.fightingBoss)
		{
			if (bossStage > 3)
			{
				bossStage = 0;
			}

			if (bossStage == 0)
			{
				danceable = false;
				//Throw
			}
			else if (bossStage == 1)
			{
				//Throw + Move
				StartCoroutine("MoveThroughWalkPoints");
			}
			else if(bossStage == 2)
			{
				// Nothing
				danceable = true;
				StopCoroutine("MoveThroughWalkPoints");
			}
			else
			{
				danceable = true;
				StopCoroutine("MoveThroughWalkPoints");
				//Stop and let player get chance
			}

			bossStage++;
		}
	}

	IEnumerator MoveThroughWalkPoints()
	{
		int destInt = 0;
		Transform startMarker = transform;
		Transform endMarker = walkPoints[destInt];
		float speed = BeatManager.Instance.SongTempo * 4f;
		float startTime= Time.time;
		float journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
		float fractionOfJourney = 0f;

		while (fractionOfJourney < 1f)
		{
			// Distance moved equals elapsed time times speed..
			float distCovered = (Time.time - startTime) * speed;

			// Fraction of journey completed equals current distance divided by total distance.
			fractionOfJourney = distCovered / journeyLength;

			// Set our position as a fraction of the distance between the markers.
			transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);

			if(fractionOfJourney >= 1f)
			{
				destInt++;
				if(destInt > 3)
				{
					destInt = 0;
				}
				endMarker = walkPoints[destInt];
				startTime = Time.time;
				journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
				fractionOfJourney = 0f;
			}

			yield return null;
		}
		yield return null;

	}

	void ThrowProperTwelveBoss()
	{
		if (!danceable)
		{
			ProperTwelve pt = Instantiate(ProperTwelve, transform.position, Quaternion.identity);
			ProperTwelve pt1 = Instantiate(ProperTwelve, transform.position, Quaternion.identity);
			ProperTwelve pt2 = Instantiate(ProperTwelve, transform.position, Quaternion.identity);
			ProperTwelve pt3 = Instantiate(ProperTwelve, transform.position, Quaternion.identity);

			pt1.SetDir(Vector3.right);
			pt2.SetDir(Vector3.up);
			pt3.SetDir(Vector3.down);
		}
	}

	void ThrowProperTwelve()
	{
		if (Player.Instance.dancePartner != this)
		{
			Instantiate(ProperTwelve, transform.position, Quaternion.identity);
		}
	}

	public void IncreaseSatisfaction()
	{
		satisfaction += 12.5f;
		textmesh.text = "satisfaction\n" + satisfaction;
		if (satisfaction >= 100)
		{
			CancelInvoke("ThrowProperTwelve");
			danceable = false;
			spriteRenderer.color = satisfiedColour;
			LevelManager.DanceContribution contribution = new LevelManager.DanceContribution();
			switch (enemyType)
			{
				case EnemyType.OldMan:
					contribution.secondsToPay = 5f;
					contribution.rateToPay = 10000;
					break;
				case EnemyType.Thrower:
					contribution.secondsToPay = 5f;
					contribution.rateToPay = 15000;
					break;
				case EnemyType.Looper:
					contribution.secondsToPay = 5f;
					contribution.rateToPay = 20000;
					break;
				case EnemyType.Boss:
					contribution.secondsToPay = 20f;
					contribution.rateToPay = 1000000;
					break;
				default:
					contribution.secondsToPay = 10f;
					contribution.rateToPay = 4000;
					break;
			}
			LevelManager.Instance.AddToMoney(contribution);
		}
		if(danceParticleSystem && !danceParticleSystem.isPlaying)
		{
			danceParticleSystem.Play();
		}
		emissionModule.rateOverTimeMultiplier = satisfaction * 0.1f;
	}
}