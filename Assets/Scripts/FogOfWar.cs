using UnityEngine;
using System.Collections;

public class FogOfWar : MonoBehaviour {

	public SpriteRenderer myRenderer;
	public SpriteRenderer childRenderer;
	public bool canTurnOffFog = true;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			TurnOffFog();
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.I) && canTurnOffFog)
		{
			TurnOffFog();
		}
	}

	void CantTurnOffFog()
	{
		canTurnOffFog = false;
	}

	public void TurnOffFog()
	{
		myRenderer.enabled = false;
		childRenderer.enabled = false;
	}
}
