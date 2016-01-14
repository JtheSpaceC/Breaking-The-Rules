using UnityEngine;
using System.Collections;

public class InCoverDetection : MonoBehaviour {

	public bool someoneIsHeadedHere = false;
	public bool isOccupied = false;
	public float correctInterval = 3f;

	void Start()
	{
		InvokeRepeating ("RefreshCoverBoolsAndNearbyColliders", correctInterval, (Random.Range(0.95f, 1.05f)* 3));
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Soldier")
		{
			other.SendMessage ("YouAreInCover");	
			isOccupied = true;
		}
		else
		{
			isOccupied = false;
			Debug.Log ("ELSE happened");
		}
	}


	void OnTriggerExit2D(Collider2D other)
	{
		if(other.tag == "Soldier")
		{
			other.SendMessage ("YouAreNOTInCover");	
			//RefreshCoverBoolsAndNearbyColliders();
		}
	}

	void RefreshCoverBoolsAndNearbyColliders()
	{
		LayerMask soldiersMask = LayerMask.GetMask("Enemy","Ally");

		Collider2D[] nearbySoldiers = Physics2D.OverlapCircleAll (transform.position, 1, soldiersMask);

		if(nearbySoldiers.Length >= 1)
		{
			isOccupied = true;
		}
		else
		{
			isOccupied = false;
		}

		
	}
}
