using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class Elevator : MonoBehaviour {

	AstarPath pathfinderScript;

	public float rateOfDescent = 0.05f;
	public bool startDescent = false;
	bool alreadyRun = false;
	AudioSource myAudioSource;

	[Tooltip("This should be the door, but can be a wall either as long as it has collider on Obstacle Layer")]
	public GameObject doorWall;

	public bool elevatorHasLeft = false;

	void Start()
	{
		//startDescent = true;
		myAudioSource = GetComponent<AudioSource> ();
		pathfinderScript = GameObject.Find ("A*").GetComponent<AstarPath> ();
	}

	void FixedUpdate () 
	{
		if(startDescent)
		{
			if(!alreadyRun)
			{
				alreadyRun = true;
				MakePassengersChildren();
				myAudioSource.Play();

				StartCoroutine(Descend());
				StartCoroutine(_MANAGER.instance.RPGstatsScreen());
			}

		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			if(RPGelements.instance.hasKey)
			{
				startDescent = true;
			}
			else
			{
				//TODO: set up a message about needing key
			}
		}
	}

	void MakePassengersChildren()
	{
		Vector2 pointA = (Vector2)transform.position + new Vector2 (-3.5f, 3.5f);
		Vector2 pointB = (Vector2)transform.position + new Vector2 (3.5f, -3.5f);
		Collider2D[] passengers = Physics2D.OverlapAreaAll (pointA, pointB);

		foreach(Collider2D passenger in passengers)
		{
			if(passenger.tag == "Player"){
				passenger.GetComponentInChildren<Shooting>().enabled = false;
				passenger.GetComponent<Animator>().SetBool ("Walking", false);
				passenger.GetComponent<PlayerMovement>().enabled = false;
				passenger.GetComponent<Collider2D>().enabled = false;
				passenger.transform.FindChild("Gun").FindChild("Laser").gameObject.SetActive(false);
				try{passenger.transform.FindChild("UI").FindChild("Canvas").FindChild("Health Panel").gameObject.SetActive(false);
				}catch{}
			}

			if(passenger.name != "floor" && passenger.gameObject.layer != LayerMask.NameToLayer("Obstacle")
			   && passenger.name != "Gun")
			{
				passenger.transform.SetParent(this.transform);
			}
			else if(passenger.name.StartsWith("crate"))
			{
				passenger.transform.SetParent(this.transform);
			}
		}
		elevatorHasLeft = true;
		doorWall.SetActive (true);
		pathfinderScript.Scan ();
	}

	IEnumerator Descend()
	{
		yield return new WaitForSeconds(1.75f);

		while(transform.localScale.x >= 0.28f)
		{
			float newX = transform.localScale.x * (1-rateOfDescent * Time.deltaTime);
			float newY = transform.localScale.y * (1-rateOfDescent * Time.deltaTime);

			transform.localScale = new Vector3 (newX, newY, 1);

			yield return new WaitForEndOfFrame();
		}
	}
}
