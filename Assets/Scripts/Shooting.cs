using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class Shooting : MonoBehaviour {

	AudioSource audioSource;
	LineRenderer gunLine;
	Soldier soldierScript;
	Stealth stealthScript;

	[Tooltip("Set True if this is the player's gun")] 
	public bool playerControlled = false;
	public bool hasLaser = false;

	public float firingRPM = 200;
	public float damagePerShot = 30;
	float nextFire;

	public float accuracy = 50f;
	public float weaponNoiseTravelDist = 20f;
	float circleSize = 1;
	Vector2 mousePos;

	public LayerMask shootableLayer;
	public LayerMask blockingLayer; //this should just be set to Obstacle (full cover blocking objects like walls)


	float timer = 0;
	float effectsDisplayTime;    

	[HideInInspector] public Transform targetCircle;
	public bool showTargetCircle = true;
	[HideInInspector]public float speedModifier = 1;

	[HideInInspector] public Vector2 shootPoint;

	[Header("Reload Stuff")]
	bool reloading = false;
	public Slider reloadSlider;
	float reloadStartTime;
	public float reloadTime = 1;
	public Text ammoText;
	public int ammoGun = 30;
	[HideInInspector] public int ammoReserve;
	public AudioClip reloadNoise;
	public AudioClip ammoOutNoise;
	AudioClip shootNoise;


	void Awake()
	{
		if (playerControlled) 
		{
		} 
		else 
		{
			soldierScript = GetComponentInParent<Soldier>();
		}
		audioSource = GetComponent<AudioSource> ();
		gunLine = GetComponent<LineRenderer> ();
		targetCircle = GameObject.Find("circle (outline)").transform;

		timer = 60/firingRPM;
		if (timer > 0.2f)
			effectsDisplayTime = 0.2f;
		else
			effectsDisplayTime = timer-0.1f;

		if(!hasLaser)
		{
			try{	transform.parent.FindChild("Laser").gameObject.SetActive(false);}catch{}
		}

		stealthScript = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Stealth> ();
		shootNoise = audioSource.clip;
	}

	void Start()
	{
		reloadTime = RPGelements.rpgElements.reloadTime;

		if(playerControlled)
		{
			accuracy = RPGelements.rpgElements.accuracyStat;
			damagePerShot = RPGelements.rpgElements.damage;
			ammoReserve = RPGelements.rpgElements.endingAmmo - ammoGun;

			if(reloadSlider != null)
			{
				reloadSlider.gameObject.SetActive(false);
			}
			UpdateAmmoText();
		}
	}

	public void UpdateAmmoText()
	{
		ammoText.text = ammoGun + " || " + ammoReserve;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if(playerControlled)
		{
			if(Input.GetMouseButton(0) && timer >= 60/firingRPM && ammoGun > 0 && !reloading)
			{
				Fire();
			}
			else if(Input.GetMouseButton(0) && timer >= 60/firingRPM && ammoGun <= 0 && !reloading)
			{
				audioSource.clip = ammoOutNoise;
				if(audioSource.isPlaying == false)
					audioSource.Play();
			}

			if(Input.GetKeyDown(KeyCode.R) && ammoGun < 30 && ammoReserve >0 && !reloading)
			{
				StartCoroutine(Reload());
			}

			else if(reloading)
			{
				reloadSlider.value = (Time.time - reloadStartTime) / reloadTime;
			}
		}
		else
		{
			if(timer >= nextFire)
			{
				RaycastHit2D hit = Physics2D.Raycast(transform.position, soldierScript.targetToShoot.position - transform.position, 
				                                     Vector2.Distance(transform.position, soldierScript.targetToShoot.position), blockingLayer);
				if(hit.collider == null) 
				{
					Fire();
				}
			}
		}
	}// end of Update


	void LateUpdate()
	{
		if (playerControlled) 
		{
			//determine the size of the target brackets
			mousePos = (Vector2)(Camera.main.ScreenToWorldPoint (Input.mousePosition));
			float distToMouse = Vector2.Distance (transform.position, mousePos);
			circleSize = distToMouse / 18 * 100 / accuracy * speedModifier; //TODO: put accuracy stats here.

			if (showTargetCircle)
				AdjustCircleScale ();
		}
		else
		{
			float distToTarget = Vector2.Distance(transform.position, soldierScript.targetToShoot.position);
			circleSize = distToTarget / 18 * 100 /accuracy * speedModifier; //TODO: put accuracy stats here.
		}
	}

	IEnumerator Reload()
	{
		reloading = true;
		reloadSlider.gameObject.SetActive(true);
		reloadStartTime = Time.time;

		AudioSource.PlayClipAtPoint(reloadNoise, transform.position);

		yield return new WaitForSeconds(reloadTime);

		int ammoRefill = 30 - ammoGun;
		if(ammoRefill > ammoReserve)
		{
			ammoRefill = ammoReserve;
		}
		ammoGun = ammoGun + ammoRefill;
		ammoReserve -= ammoRefill;
		audioSource.clip = shootNoise;
		UpdateAmmoText();
		reloadSlider.value = 0;
		reloadSlider.gameObject.SetActive(false);
		reloading = false;
	}

	void Fire()
	{
		//determine where the shot will go. Accuracy, cover, etc


		Vector3 pos1 = Vector2.zero; //for drawing where the bullet hits

		
		//then shoot near the mouse position randomly based on accuracy. The accuracy is inside a circle that grows with distance

		if (playerControlled) 
		{
			shootPoint = mousePos + Random.insideUnitCircle * circleSize;
		}
		else
		{
			//shootPoint as defined in Soldier script.
			shootPoint = (Vector2)soldierScript.targetToShoot.position + Random.insideUnitCircle * circleSize;
		}

		// Check if shootpoint doesn't hit "obstacle" before hitting its point. Only for Player. AI has already done this in Update
		if(playerControlled)
		{
			RaycastHit2D hit1 = Physics2D.Raycast(transform.position, shootPoint - (Vector2)transform.position, 
		                                     Vector2.Distance(transform.position, shootPoint), blockingLayer);

			if(hit1.collider != null && hit1.collider.tag != "Soldier" && hit1.collider.tag != "Player")
				//then we must have hit a wall, and set the shot to go there, and move on
			{
				pos1 = (Vector3)hit1.point;
			}
			else if(hit1.collider != null && (hit1.collider.tag == "Soldier" || hit1.collider.tag == "Player"))
			{
				pos1 = (Vector3)hit1.point;
				Health enemyHealth = hit1.collider.GetComponent <Health> ();
				
				// If the Health component exists...
				if(enemyHealth != null)
				{
					//return if it's us
					if(enemyHealth.gameObject == this.transform.parent.transform.parent.gameObject)
					{
						return;
					}
					// ... the enemy should take damage.
					enemyHealth.YouveBeenHit (damagePerShot, false, this.transform.parent.transform.parent.gameObject);
				}
				else{Debug.Log("ERROR: shot something that doesn't have a Health script");}
			}
			else //otherwise we check what we might have hit besides a wall/door
			{
				pos1 = shootPoint;
			}

			Instantiate(_MANAGER.instance.bulletHitParticlesPrefab, (Vector3)pos1, Quaternion.identity);

			ammoGun --;
			UpdateAmmoText();		}

		else //if not player controlled, so, AI
		{
			try{
			if(Physics2D.OverlapPoint(shootPoint, shootableLayer).GetComponent<Collider2D>().tag == "Player" ||
			   Physics2D.OverlapPoint(shootPoint, shootableLayer).GetComponent<Collider2D>().tag == "Soldier")
			{
				Health enemyHealth = Physics2D.OverlapPoint(shootPoint, shootableLayer).GetComponent<Collider2D>().GetComponent <Health> ();
				
				// If the Health component exists...
				if(enemyHealth != null)
				{
					//return if it's us
					if(enemyHealth.gameObject == this.transform.parent.transform.parent.gameObject)
					{
						return;
					}
					// ... the enemy should take damage.
					enemyHealth.YouveBeenHit (damagePerShot * Random.Range(0.3f, 1.5f), false, this.transform.parent.transform.parent.gameObject);
				}
				
				pos1 = shootPoint;
			}
			}
			catch{pos1 = shootPoint;}
		}

		//effects
		audioSource.Play();
		
		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);
		gunLine.SetPosition (1, pos1);
		
		Invoke ("DisableEffects", effectsDisplayTime);

		//reset timer
		timer = 0;

		if (!playerControlled) 
		{
			float[] acceptableNums = new float[]{1,1,1,4,7}; //this is only used by the NPCs
			nextFire = (60 / firingRPM) * acceptableNums [Random.Range (1, 5)]; //this is only used by the NPCs
			RPGelements.rpgElements.shotsFired++;
		}
		else if(playerControlled)
		{
			//check if we were heard firing
			WasIHeard(transform.position, pos1);
		}
	}

	public void DisableEffects ()
	{
		gunLine.enabled = false;
	}

	void AdjustCircleScale()
	{		
		targetCircle.localScale = new Vector2 ( 2*circleSize, 2*circleSize); //*2 because circleSize is actually radius, not diameter
	}

	void OnDisable()
	{
		if (playerControlled) {
			try{targetCircle.gameObject.SetActive (false);}catch{}
		}
	}

	void WasIHeard(Vector2 firingPoint, Vector2 hitPoint)
	{
		Collider2D[] enemies = Physics2D.OverlapCircleAll (transform.position, 17, LayerMask.GetMask("Enemy"));

		foreach(Collider2D enemy in enemies)
		{
			Soldier script = enemy.GetComponent<Soldier>();

			if(script.state == Soldier.stateMachine.Patroling || script.state == Soldier.stateMachine.Guarding)
			{
				if(stealthScript.CalculateDistance(enemy.transform.position, firingPoint) <= weaponNoiseTravelDist)
				{
					WasIHeardPt2(script);
					//print ("heard gunshot " + stealthScript.CalculateDistance(enemy.transform.position, firingPoint));
				}
				else if(stealthScript.CalculateDistance(enemy.transform.position, hitPoint) <= weaponNoiseTravelDist)
				{
					WasIHeardPt2(script);
					//print ("heard BULLET " + stealthScript.CalculateDistance(enemy.transform.position, hitPoint));
				}
			}
		}
	}
	void WasIHeardPt2(Soldier script)
	{
		float[] weights = new float[] {1,0,0};
		script.targetToShoot = transform.parent.transform.parent;
		script.state = script.ReturnANewState(script.combatStates, weights);
		//TODO: change to checking/searching?
	}
}//Mono
