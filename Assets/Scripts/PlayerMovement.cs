using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	
	Shooting playerShooting;

	Animator animator;

	float normalSpeed = 5f;
	float walkSpeed = 2.5f;
	float speed;
	public float rotationSpeed = 180.0f;

	public AudioClip[] footsteps;

	public bool stationary = true;
	public bool walking = false;
	public bool running = false;
	

	void Awake()
	{
		playerShooting = GetComponentInChildren<Shooting> ();
		animator = GetComponent<Animator> ();
	}

	void Start()
	{
		normalSpeed = RPGelements.rpgElements.runningSpeed;
		walkSpeed = RPGelements.rpgElements.walkingSpeed;

		speed = normalSpeed;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if(Input.GetKey(KeyCode.LeftShift))
		{
			speed = walkSpeed; //TODO: Adjust animation frames here
		}
		else
		{
			speed = normalSpeed;
		}

		//For movement
		Vector3 dir = (Vector3.up*Input.GetAxis("Vertical") + Vector3.right*Input.GetAxis("Horizontal")).normalized;

		transform.Translate (dir *speed*Time.deltaTime, Space.World);

		if (Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0)
		{
			animator.SetBool ("Walking", true);
			/*footstepTimer += Time.deltaTime;
			if(footstepTimer >= stepNoiseIntervalRunning)
			{
				footstepTimer = 0;
				AudioSource.PlayClipAtPoint(footsteps[Random.Range(0, footsteps.Length)], transform.position, 0.5f);
			}*/
		}
		else
		{
			animator.SetBool ("Walking", false);
			//footstepTimer = stepNoiseIntervalRunning;
		}


		Vector2 mousePos = (Vector2)(Camera.main.ScreenToWorldPoint (Input.mousePosition));
		LookAt (mousePos);

		//for bools
		if (Input.GetAxis ("Vertical") == 0 && Input.GetAxis ("Horizontal") == 0)
		{
			stationary = true;
			walking = false;
			running = false;
			playerShooting.speedModifier = 1;
		}
		else if ((Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0) && Input.GetKey (KeyCode.LeftShift))
		{
			stationary = false;
			walking = true;
			running = false;
			playerShooting.speedModifier = 1.5f;
		}
		else if(Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0 && !Input.GetKey (KeyCode.LeftShift))
		{
			stationary = false;
			walking = false;
			running = true;
			playerShooting.speedModifier = 2;
		}
	}// end of UPDATE


	void LookAt(Vector2 lookPos)
	{
		Vector3 dir = lookPos - (Vector2)transform.position; 
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg -90;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		
		if(!System.Single.IsNaN(angle))
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.deltaTime * rotationSpeed);
		}
	}

}//Mono
