using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

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

	float bossTimeWindow = 2f;

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
		else if(enemyType == EnemyType.Boss)
		{
			danceable = false;
			InvokeRepeating("ThrowProperTwelveBoss", 0.0f, BeatManager.Instance.SongTempo * 0.5f);
		}
	}

	void ThrowProperTwelveBoss()
	{
		ProperTwelve pt = Instantiate(ProperTwelve, transform.position, Quaternion.identity);
		ProperTwelve pt1 = Instantiate(ProperTwelve, transform.position, Quaternion.identity);
		ProperTwelve pt2 = Instantiate(ProperTwelve, transform.position, Quaternion.identity);
		ProperTwelve pt3 = Instantiate(ProperTwelve, transform.position, Quaternion.identity);

		pt1.SetDir(Vector3.right);
		pt2.SetDir(Vector3.up);
		pt3.SetDir(Vector3.down);
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