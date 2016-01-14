using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class coverManager : MonoBehaviour {
	
	TeamAI oppositionAIScript;
	
	public static coverManager instance;
	public List<Transform> allCover;
	public List<Transform> allUnoccupiedCoverPoints;
	
	
	void Awake()
	{
		instance = this;
		
		GameObject[] allCoverGameObjects = GameObject.FindGameObjectsWithTag ("Cover");
		foreach (GameObject cover in allCoverGameObjects)
		{
			allCover.Add(cover.transform);
		}
	}
	
	public Vector2 ClosestCoverPoint (GameObject theCaller, bool mustAdvance)
	{
		if (allCover.Count == 0)
			return theCaller.transform.position;
		
		bool isAlly = false;
		
		if(mustAdvance && theCaller.layer == LayerMask.NameToLayer("Ally"))
		{
			isAlly = true;
		}
		else if(mustAdvance && theCaller.layer == LayerMask.NameToLayer("Enemy"))
		{
			isAlly = false;
		}
		
		float closestYet = Mathf.Infinity;
		Transform closestCover = null;

		//if that failed try just for somewhere ahead of me
		if (closestCover == null)
		{
			foreach(Transform coverPoint in allCover)
			{ 
				if(mustAdvance && isAlly && coverPoint.position.x > (theCaller.transform.position.x - 2f))
				{
					continue;
				}
				else if(mustAdvance && !isAlly && coverPoint.position.x < (theCaller.transform.position.x + 2f))
				{
					continue;
				}
				
				InCoverDetection coverScript = coverPoint.GetComponentInChildren<InCoverDetection>();
				if(!coverScript.someoneIsHeadedHere && !coverScript.isOccupied)
				{
					if(Vector2.Distance(theCaller.transform.position, coverPoint.position) < closestYet)
					{
						closestCover = coverPoint;
						closestYet = Vector2.Distance(theCaller.transform.position, coverPoint.position);
					}
				}
			}
		}
		if (closestCover == null)
		{
			return theCaller.transform.position;
		}
		
		//if there is a point to go to, we pick the point on that cover that is opposite the enemy's avg position
		if(!isAlly)
		{
			oppositionAIScript = GameObject.Find("Ally COMMANDER").GetComponent<TeamAI>();
		}
		else if(isAlly)
		{
			oppositionAIScript = GameObject.Find("Enemy COMMANDER").GetComponent<TeamAI>();
		}
		
		Vector2 oppositionAvgPoint = oppositionAIScript.AveragePositionOfTeam ();
		
		Vector2 dir = ((Vector2)closestCover.position - oppositionAvgPoint).normalized;
		
		closestCover.GetComponentInChildren<InCoverDetection> ().someoneIsHeadedHere = true;
		
		if(theCaller.GetComponent<Soldier> ().coverImHeadedTo != null)
			theCaller.GetComponent<Soldier> ().coverImHeadedTo.GetComponentInChildren<InCoverDetection>().someoneIsHeadedHere = false;
		
		theCaller.GetComponent<Soldier> ().coverImHeadedTo = closestCover;
		Vector2 coverPointOnClosestCover = (Vector2)closestCover.position + (dir*1.1f);
		return coverPointOnClosestCover;
	}
}
