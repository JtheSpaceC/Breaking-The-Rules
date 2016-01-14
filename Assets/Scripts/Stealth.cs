using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Stealth : MonoBehaviour {

	public Vector2 shootpoint;
	public Vector2 hitPoint;

	public Path path;
	Seeker seeker;

	void Awake()
	{
		seeker = GetComponent<Seeker> ();
	}
	
	public float CalculateDistance(Vector2 a, Vector2 b)
	{
		path = seeker.StartPath (a, b);
		List<Vector3> vPath = path.vectorPath;
		float totalDistance = 0;
		
		Vector2 current = a;
		
		//Iterate through vPath and find the distance between the nodes
		for (int i = 0; i < vPath.Count; i++)
		{
			totalDistance += ((Vector2)vPath[i] - current).magnitude;
			current = vPath[i];
		}
		
		totalDistance += (b - current).magnitude;
		path.Reset() ;

		return totalDistance;
	}
}
