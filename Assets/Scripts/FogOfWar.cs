using UnityEngine;
using System.Collections;

public class FogOfWar : MonoBehaviour {

	public SpriteRenderer childRenderer;
	public bool canTurnOffFog = true;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			childRenderer.enabled = false;
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
		childRenderer.enabled = false;
	}
}
