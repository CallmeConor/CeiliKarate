using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatCounter : MonoBehaviour
{
	private Beat beatToHit;
	Collider2D beatCollider;

	void Awake()
	{
		beatCollider = GetComponent<Collider2D>();
	}

	void Update()
	{
		bool validKeyPress = false;
		Vector2 moveDir = Vector2.zero;
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
		{
			moveDir = Vector2.up;
			validKeyPress = true;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
		{
			moveDir = Vector2.down;
			validKeyPress = true;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
		{
			moveDir = Vector2.right;
			validKeyPress = true;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
		{
			moveDir = Vector2.left;
			validKeyPress = true;
		}

		if (beatToHit && validKeyPress)
		{
			float camX = Camera.main.transform.position.x;
			float hitX = beatCollider.bounds.ClosestPoint(beatToHit.transform.position).x;
			int fameBonusVal = 0;
			if (hitX < -0.2f + camX)
			{
				Debug.Log("LATE");
				fameBonusVal = -20;
			}
			else if(hitX > 0.2f + camX)
			{
				Debug.Log("EARLY");
				fameBonusVal = -10;
			}
			else if (hitX < -0.02f + camX || hitX > 0.02f + camX)
			{
				Debug.Log("GOOD");
				fameBonusVal = 5;
			}
			else
			{
				Debug.Log("PERFECT");
				fameBonusVal = 10;
			}
			Destroy(beatToHit.gameObject);
			Player.Instance.MovePlayer(moveDir, fameBonusVal);
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Beat>() && !beatToHit)
		{
			beatToHit = collision.GetComponent<Beat>();
		}
		else
		{
			Debug.Log("Something went wrong");
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<Beat>() == beatToHit)
		{
			beatToHit = null;
		}
		else
		{
			Debug.Log("Beat wasn't our current beat to hit");
		}
	}
}
