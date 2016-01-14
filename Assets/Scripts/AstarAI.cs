using UnityEngine;
using System.Collections;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
public class AstarAI : MonoBehaviour {

	Soldier soldierScript;

	//The point to move to
	public Vector2 targetPosition;
	
	private Seeker seeker;

	//The calculated path
	public Path path;
	
	//The AI's speed per second
	public float speed = 1; //set from Soldier script

	public float updateRate = 2;
	
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 2;
	public float stoppingDistance = 1f;

	bool pathComplete = true; //I'm putting this in myself
	
	//The waypoint we are currently moving towards
	[HideInInspector]public int currentWaypoint = 0;
	

	public void Awake () 
	{
		seeker = GetComponent<Seeker>();
		soldierScript = GetComponent<Soldier> ();
	}

	void Start()
	{
		//InvokeRepeating ("UpdatePath", 1f, updateRate);
	}

	public void UpdatePath()
	{
		if(Vector2.Distance(transform.position, targetPosition) > stoppingDistance && pathComplete)
		{
			try{pathComplete = false;
				Vector3 dir =  (path.vectorPath[currentWaypoint] - transform.position).normalized;
				seeker.StartPath (transform.position + dir, targetPosition, OnPathComplete);
				}
			catch{seeker.StartPath (transform.position, targetPosition, OnPathComplete);}
		}
		else
		{
			soldierScript.moving = false;
			soldierScript.sprinting = false;
		}
	}
	
	public void OnPathComplete (Path p) 
	{
		//Debug.Log ("Yay, we got a path back. Did it have an error? "+p.error);
		if (!p.error) 
		{
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
		}
		pathComplete = true;
	}
	
	public void FixedUpdate () 
	{

		if (path == null) 
		{
			//We have no path to move after yet
			return;
		}
		
		if (currentWaypoint >= path.vectorPath.Count) 
		{
			path = null;
			return;
		}

		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;

		//TIME TO MOVE THE AI
		transform.position += dir;

		//controller.SimpleMove (dir);
		
		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}
} 