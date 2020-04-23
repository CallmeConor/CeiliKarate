using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatManager : MonoBehaviour
{
	public static BeatManager Instance;
	public float SongTempo = 120f;
	public float songWarmupDelay = 1f;

	public Color[] enemyColours;
	private bool songWarmpupOver = false;
	public Beat BeatRef;

	void Awake()
	{
		Instance = this;
		SongTempo = SongTempo / 60f;
		songWarmupDelay = SongTempo;
	}

	void Start()
	{
		InvokeRepeating("SpawnNewBeat", songWarmupDelay, SongTempo * 0.25f);
	}

	void SpawnNewBeat()
	{
		if (!songWarmpupOver) { songWarmpupOver = true; }

		Instantiate(BeatRef, (BeatRef.transform.position + (Camera.main.gameObject.transform.position.x * Vector3.right) - Vector3.left * SongTempo * Time.deltaTime), Quaternion.identity);
	}
}
