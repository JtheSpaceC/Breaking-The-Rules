using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {

	Soldier soldierScript;
	bool playerControlled = false;

	public AudioClip[] hitSounds;
	AudioSource audioSource;

	public float maxHealth = 100;
	[Tooltip("This is set at start to be maxHealth")] public float health;
	public float armour = 0;
	public float xpDefenseBuff = 0;
	public GameObject deathAnimPrefab;

	public bool inCover = false;
	public bool dead = false;
	public bool damaged = false;

	public List<GameObject> myLoot;

	[Header("UI stuff")]
	public Slider healthSlider;
	public Image healthSliderColor;
	public Image healthBackgroundColor;
	public GameObject radarSig;
	public GameObject bloodSplatPrefab;

	// Use this for initialization

	void Start () 
	{
		if (gameObject.tag == "Player") 
		{
			playerControlled = true;
			health = RPGelements.rpgElements.endingHealth;
		}
		if(!playerControlled)
		{
			soldierScript = GetComponent<Soldier>();
		}
		audioSource = GetComponent<AudioSource> ();

		radarSig.SetActive (true);
	}


	void Update()
	{
		if (playerControlled && !dead) {
			healthSlider.value = health/100;
		
			// FOR THE RED FLASH
			// If the player has just been damaged...
			/*if(damaged)
		{
			// ... set the colour of the damageImage to the flash colour.
			damageImage.color = flashColour;
		}
		// Otherwise...
		else
		{
			// ... transition the colour back to clear.
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}*/
		
			// Reset the damaged flag.
			damaged = false;
		
			//FOR COLOUR
			float alpha = healthSliderColor.color.a;
		
			if (health < maxHealth) {
				alpha = 1;
				healthSliderColor.color = Color.green * alpha;
				healthBackgroundColor.enabled = true;
			} else if (health >= maxHealth) {
				health = maxHealth;
				alpha = 0f;
				healthSliderColor.color = Color.green * alpha;
				healthBackgroundColor.enabled = false;
			}
		}
	}
	

	public void YouveBeenHit(float damage, bool bypassCover, GameObject theAttacker)
	{
		if(!dead && (!inCover || bypassCover))
		{
			if(!playerControlled && soldierScript.IsCurrentStateOnList(soldierScript.normalStates))
			{
				//if the enemy is unaware, do double damage
				damage *= 3;
			}
			health -= damage;
			if(playerControlled)
			{
				RPGelements.rpgElements.damageTaken += damage;
			}
			// Array of blood splats and hit sounds
			Vector2 where = transform.position - (theAttacker.transform.position - transform.position).normalized * 0.5f;
			Instantiate(bloodSplatPrefab, where, transform.rotation);

			if(audioSource.isPlaying == false)
			{
				audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length)]; 
				audioSource.Play();
			}
		}
		else if(!dead && inCover && !bypassCover)
		{
			//TODO: tell your cover object to play hit sound
		}

		if (health <= 0 && this.tag != "Player") 
		{
			EnemyDeath();
		}
		else if(health <= 0 && this.tag == "Player")
		{
			PlayerDeath();
		}
	}

	void YouAreNOTInCover()
	{
		inCover = false;
	}
	void YouAreInCover()
	{
		inCover = true;
	}

	void EnemyDeath()
	{		
		dead = true;
		Instantiate(deathAnimPrefab, transform.position, transform.rotation);
		DropLoot();
		Destroy (gameObject);
	}

	void PlayerDeath()
	{
		dead = true;
		transform.FindChild ("Gun").gameObject.SetActive (false);
		PlayerMovement playerMovement = gameObject.GetComponent<PlayerMovement> ();
		playerMovement.enabled = false;
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		gameObject.GetComponent<Collider2D> ().enabled = false;
		Instantiate(deathAnimPrefab, transform.position, transform.rotation);
		healthSlider.gameObject.SetActive(false);
		_MANAGER.instance.GameOver ();
	}

	void DropLoot()
	{
		foreach(GameObject loot in myLoot)
		{
			GameObject drop = GameObject.FindGameObjectWithTag("GameController").GetComponent<_MANAGER>().getLootPrefab(loot.name);
			Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle;
			Instantiate(drop, pos, Quaternion.identity);
		}
	}
}//Mono
