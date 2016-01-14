using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamAI : MonoBehaviour {

	[HideInInspector] public List<GameObject> teamPlayers;
	public float teamRange = 12;

	void Awake()
	{
		GetAllMyTeamPlayers ();
	}


	public void GetAllMyTeamPlayers()
	{
		teamPlayers.Clear ();
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag ("Soldier");
		
		foreach(GameObject player in allPlayers)
		{
			if(player.layer == this.gameObject.layer && !player.GetComponent<Soldier>().dead)
			{
				teamPlayers.Add(player);
			}
		}
	}


	public Vector2 AveragePositionOfTeam() //TODO: check how this works for enemies being all over the place
	{
		float avgX = 0;
		float avgY = 0;
		
		if (teamPlayers.Count == 0)
			return transform.position;
		
		foreach(GameObject player in teamPlayers)
		{
			avgX += player.transform.position.x;
			avgY += player.transform.position.y;
		}
		avgX /= teamPlayers.Count;
		avgY /= teamPlayers.Count;
		
		return new Vector2 (avgX, avgY);
	}
}
