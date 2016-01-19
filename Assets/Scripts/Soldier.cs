using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Soldier : MonoBehaviour {
	
	AstarAI aStarAI;
	AudioSource audioSource;
	Shooting shootScript;
	GameObject gameManager;

	public bool moving = false;
	public bool sprinting = false;

	[HideInInspector]public TeamAI myTeamAI;

	LayerMask oppositionLayer;
	[HideInInspector] public string oppositionLayerAsString;
	
	public LayerMask masksForFlankSearch;

	public enum whichSide {Enemy, Ally};
	public whichSide Side;
	
	public enum attackingOrDefending {Attacking, Defending};
	[HideInInspector] public attackingOrDefending AorD;
	
	public enum allDefenseRoles {Sentry, Patrol, Guard, Medic, Reinforcement, RnR, Sleeping, None};
	[HideInInspector]public allDefenseRoles dRole;
	
	public enum allOffenseRoles {Officer, Suppression, Stormer, FlankLeft, FlankRight, Sniper, Medic, Demolitions, Radioman, Reinforcement, None}
	[HideInInspector] public allOffenseRoles oRole;

	public enum stateMachine {Patroling, Guarding, Searching, Charging, TakingCover, Flanking, CheckingBody};
	public stateMachine state;	
	public stateMachine[] normalStates; 
	public stateMachine[] curiousStates;
	public stateMachine[] combatStates;
	public stateMachine[] postCombatStates;

	bool switchingStates = true;
	
	public float walkSpeed = 2f;
	public float runSpeed = 4f;
	float speed;

	public float minWaitAtPatrolPointTime = 1;
	public float maxWaitAtPatrolPointTime = 4;
	
	//public float hp = 100f;
	[HideInInspector] public bool inDanger = false;
	[HideInInspector] public bool inCover = false;
	[HideInInspector] public bool underFire = false;
	[HideInInspector] public bool suppressed = false;
	[HideInInspector] public bool firing = true;
	[HideInInspector] public bool iAmCurious = false;

	[Tooltip("How long after seeing an enemy will they check again to confirm sighting")]
	[Range(0.1f, 3)]
	public float reactionTime = 0.5f;

	public float baseAccuracy = 50;
	public Transform targetToShoot;
	
	//public bool hurt = false;
	//public bool badlyWounded = false;
	public bool dead = false;
	
	[HideInInspector] public GameObject myWeapon;
	public bool popAndShootIsRunning = false;
	
	public Vector2 whereTo;
	public Vector2 targetLook;
	[HideInInspector] public Transform coverImHeadedTo;
	GameObject nearestEnemy;

	bool waiting = false;
	float timerA = 0;

	LayerMask suspiciousLayers;
	LayerMask sightBlockLayers;
	BoxCollider2D fovCollider;
	Vector2 lastConfirmedSighting;

	[Header("Sounds")]
	public AudioClip[] curiousSounds;
	public AudioClip[] alertSounds;


	void Awake () 
	{
		aStarAI = GetComponent<AstarAI> ();
		gameManager = GameObject.FindGameObjectWithTag ("GameController");
		audioSource = GetComponent<AudioSource> ();

		if(Side == whichSide.Ally)
		{
			myTeamAI = GameObject.Find("Ally COMMANDER").GetComponent<TeamAI>();
			oppositionLayer = LayerMask.GetMask("Enemy");
			oppositionLayerAsString = "Enemy";
		}
		else if(Side == whichSide.Enemy)
		{
			myTeamAI = GameObject.Find("Enemy COMMANDER").GetComponent<TeamAI>();
			oppositionLayer = LayerMask.GetMask("Ally");
			oppositionLayerAsString = "Ally";
		}

		fovCollider = GetComponentInChildren<BoxCollider2D> ();
		suspiciousLayers = oppositionLayer; //TODO: Add corpses to this
		sightBlockLayers = LayerMask.GetMask ("Obstacle", "Door", "Ally", "Enemy");
		myWeapon = transform.FindChild ("Gun").gameObject;
		shootScript = myWeapon.GetComponentInChildren<Shooting> ();
		speed = walkSpeed;
		aStarAI.speed = speed;

		normalStates = new stateMachine[] {stateMachine.Guarding, stateMachine.Patroling};
		curiousStates = new stateMachine[] {stateMachine.CheckingBody, stateMachine.Searching};
		combatStates = new stateMachine[] {stateMachine.Charging, stateMachine.TakingCover, stateMachine.Flanking};
		postCombatStates = new stateMachine[] {stateMachine.CheckingBody, stateMachine.Searching};
	}


	void Update () 
	{
		//astarAI.targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;

		if(state == stateMachine.Patroling)
		{
			PatrolingState();
		}
		else if(state == stateMachine.Guarding)
		{
			GuardingState();
		}
		else if(state == stateMachine.Charging)
		{
			ChargingState(targetToShoot);
		}
	}

	void FixedUpdate()
	{
		if(!dead)
			LookAt(targetLook);
	}


	public void ReevaluateSituation()
	{
		if (dead)
			return;
		
		targetToShoot = FindNearestEnemy ();
		
		//this line will make the whole team more mobile. More of a charge and less a patient leapfrog
		if ((!moving && targetToShoot != null && Vector2.Distance (transform.position, targetToShoot.position) > (myTeamAI.teamRange + 2))
		    || (!moving && targetToShoot == null))
		{MoveSomeWhereBesidesThisCover (true);}
		
		if(!moving &&/* (AmIFlanked() ||*/ !inCover)//)
		{
			MoveSomeWhereBesidesThisCover(true);
		}
		else if(!moving && inCover)
		{
			if(!popAndShootIsRunning)
			{
				StartCoroutine("PopAndShoot");
			}
		}
	}


	public void MoveSomeWhereBesidesThisCover(bool mustAdvance)
	{
		whereTo = coverManager.instance.ClosestCoverPoint (this.gameObject, mustAdvance);
		
		if(whereTo != (Vector2)transform.position)
		{
			aStarAI.targetPosition = whereTo;
			moving = true;
			StopCoroutine("PopAndShoot");
			popAndShootIsRunning = false;
			
			if(nearestEnemy == null || Vector3.Distance(nearestEnemy.transform.position, transform.position) > Vector3.Distance(whereTo, transform.position))
			{
				int zeroOrOne = Random.Range(0, 2);
				
				if(zeroOrOne == 0)
				{
					myWeapon.SetActive(true);
					firing = true;
					sprinting = false;
				}
				else if (zeroOrOne == 1)
				{
					sprinting = true;
				}
			}
			else //if our enemy is closer than our destination, we'd better be shooting, not sprinting blindly
			{
				myWeapon.SetActive(true);
				firing = true;
				sprinting = false;
			}
			
			
		}
	}


	public void LookAt(Vector3 targetPoint)
	{
		Vector3 dir = targetPoint - transform.position; 
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		
		if(!System.Single.IsNaN(angle))
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, q, (Time.deltaTime * 5));
		}	
	}


	public Transform FindNearestEnemy()
	{
		nearestEnemy = null;
		GameObject[] soldiers = GameObject.FindGameObjectsWithTag ("Soldier");
		float distance = 150;
		
		foreach(GameObject soldier in soldiers)
		{
			float distanceToCurrentSoldier = Vector2.Distance (transform.position, soldier.transform.position);
			Soldier soldierScript = soldier.GetComponent<Soldier>();
			
			if (soldier.layer == oppositionLayer && !soldierScript.dead)
			{
				if(distanceToCurrentSoldier < distance)
				{
					nearestEnemy = soldier;
					distance = distanceToCurrentSoldier;
				}
			}
		}
		if (nearestEnemy != null)
			return nearestEnemy.transform;
		else
			return null;
	}

	Vector2 ChoosePatrolPoint()
	{
		Vector2 point;
		LevelGenerator lg = gameManager.GetComponent<LevelGenerator> ();
		TileRecord randomRecord = lg.goodTileRecords [Random.Range (0, lg.goodTileRecords.Count)];

		if(randomRecord.name == "NSEW XXX" && randomRecord.gridObject.GetComponentInChildren<Elevator>().elevatorHasLeft)
		{
			point = (Vector2)randomRecord.gridObject.transform.position + new Vector2(0, -4.5f);
		}
		else
		point =	randomRecord.gridObject.transform.position;

		return point;
	}

	IEnumerator WaitAtPatrolPoint(float a, float b)
	{
		waiting = true;
		float waitTime = Random.Range (a, b);

		yield return new WaitForSeconds (waitTime);
		whereTo = Vector2.zero;
		waiting = false;

	}



	public stateMachine ReturnANewState(stateMachine [] possibleStates, float[] weights)
	{
		StopCoroutine("CheckAmIAnIdiot");
		iAmCurious = false;

		float totalWeight = 0;
		float weightCheck = 0;

		for(int i = 0; i < weights.Length; i++)
		{
			totalWeight += weights[i];
		}
		float roll = Random.Range (0, totalWeight);

		for(int i = 0; i< possibleStates.Length; i++)
		{
			weightCheck += weights[i];
			if(roll <= weightCheck)
			{
				switchingStates = true;
				CancelInvoke();
				return possibleStates[i];
			}
		}
		switchingStates = true;
		Debug.Log ("SWITCHING STATES function FAILED.");
		return stateMachine.Patroling;
	}

	/// <summary>
	/// below are all the States. Functions they use are above	/// </summary>

	void PatrolingState()
	{

		if(switchingStates)
		{
			speed = walkSpeed;
			aStarAI.speed = speed;
			myWeapon.SetActive(true);
			shootScript.enabled = false;
			whereTo = Vector2.zero;
			StartCoroutine("CheckAmIAnIdiot");

			switchingStates = false;
		}

		if (whereTo == Vector2.zero) 
		{
			whereTo = ChoosePatrolPoint();
			aStarAI.targetPosition = whereTo;
			aStarAI.UpdatePath();
		}
		if(Vector2.Distance(transform.position, whereTo) <= 3.5f)
		{
			if(!waiting)
				StartCoroutine(WaitAtPatrolPoint(minWaitAtPatrolPointTime, maxWaitAtPatrolPointTime));
		}
		try{
		targetLook = (Vector2)aStarAI.path.vectorPath[aStarAI.currentWaypoint];
		}
		catch{targetLook = GameObject.FindGameObjectWithTag("Player").transform.position;}

		//for examining area.
		timerA += Time.deltaTime;
		if(timerA >= reactionTime)
		{
			ExamineArea();
		}
	}


	void GuardingState()
	{
		if(switchingStates)
		{
			speed = 0;
			aStarAI.speed = speed;
			myWeapon.SetActive(true);
			shootScript.enabled = false;
			whereTo = transform.position;
			targetLook = transform.position + transform.up;

			switchingStates = false;
		}
		//TODO: Get them to glance around

		//for examining area.
		timerA += Time.deltaTime;
		if(timerA >= reactionTime)
		{
			ExamineArea();
		}
	}


	void ChargingState(Transform targetToCharge)
	{
		if(switchingStates)
		{
			speed = runSpeed;
			aStarAI.speed = speed;
			InvokeRepeating("RefreshChargingInfo", 0, 1f);
			myWeapon.SetActive(true);
			shootScript.enabled = true;
			shootScript.speedModifier = 2.0f;
			audioSource.clip = alertSounds[Random.Range(0, alertSounds.Length)]; 
			audioSource.Play();
			//AudioSource.PlayClipAtPoint(alertSounds[Random.Range(0, alertSounds.Length)], transform.position, 0.6f);
			aStarAI.targetPosition = targetToCharge.position;
			aStarAI.UpdatePath();

			switchingStates = false;
		}
		targetLook = targetToCharge.position;
	}
	void RefreshChargingInfo()
	{
		aStarAI.targetPosition = targetToShoot.position;
		aStarAI.UpdatePath ();

		if(targetToShoot.GetComponent<Health>().dead == true)
		{
			//TODO: use the Possible States function 
			switchingStates = true;
			CancelInvoke();
			state = stateMachine.Patroling;
		}
	}


	void ExamineArea()
	{
		if (fovCollider.IsTouchingLayers (suspiciousLayers)) 
		{
			Collider2D[] myEnemies = Physics2D.OverlapCircleAll (transform.position, 20f, oppositionLayer);

			if(myEnemies.Length == 0)
			{
				iAmCurious = false;
			}
			else
			{
				foreach (Collider2D enemy in myEnemies) 
				{
					if(TargetInSight(enemy.gameObject)) 
					{
						//opposition is in line of sight, raise the alarm?

						if(!iAmCurious)
						{
							iAmCurious = true;
							audioSource.clip = curiousSounds[Random.Range(0, curiousSounds.Length)]; 
							audioSource.Play();
							//TODO: play Curious animation
						}
						else
						{
							targetToShoot = enemy.transform;

							float[] weights = new float[]{33.33f, 0, 0};

							state = ReturnANewState(combatStates, weights);
						}
					} 
					else 
					{
						//can't directly see enemy within the cone.
						//TODO: Search pattern
					}
				}
			}
		}
		timerA = 0;
	}


	bool TargetInSight(GameObject target)
	{
		Vector2 dir = (target.transform.position - transform.position).normalized;
		float dist = Vector2.Distance (target.transform.position, transform.position) - 0.5f;
		RaycastHit2D hit = Physics2D.Raycast (transform.position + (transform.up * 0.5f), dir, dist, sightBlockLayers);

		if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer(oppositionLayerAsString)) {
			return true;
		} else 
			return false;
	}


	//not used
	public bool IsCurrentStateOnList(stateMachine[] stateGroup)
	{
		foreach(stateMachine sMachine in stateGroup)
		{
			if(sMachine == this.state)
			{
				return true;
			}
		}
		return false;
	}


	IEnumerator CheckAmIAnIdiot() //intended to fix guys walking into boxes and staying there. Haven't been able to fix on A*
	{
		Vector3 oldPos = transform.position;

		yield return new WaitForSeconds(maxWaitAtPatrolPointTime);

		Vector3 newPos = transform.position;

		if(Vector3.Distance(oldPos, newPos) < 0.25f)
		{
			whereTo = ChoosePatrolPoint();
			aStarAI.targetPosition = whereTo;
			aStarAI.UpdatePath();
			Debug.LogError(gameObject.name + " checked if they were an IDIOT, and they were!"); 
		}		
		StartCoroutine("CheckAmIAnIdiot");
	}

}//Mono
