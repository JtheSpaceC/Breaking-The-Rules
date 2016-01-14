using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Door : MonoBehaviour {

	AudioSource audioSource;
	SpriteRenderer myRenderer;
	BoxCollider2D detectorCollider; //detector
	public BoxCollider2D physicalDoorCollider; //door physical collider

	public enum doorState {Closed, Open, Hacked};
	public doorState state;

	public bool canHack = true;
	public float autoCloseTime = 2f;
	public SpriteRenderer button;
	public Sprite[] buttonSprites;

	private LayerMask mask;

	float timer;

	public float hintTime = 3;
	public SpriteRenderer hintImage;

	void Awake()
	{
		audioSource = GetComponent<AudioSource> ();
		myRenderer = transform.parent.FindChild("door_asset").GetComponent<SpriteRenderer>();
		physicalDoorCollider = transform.parent.FindChild("door_asset").GetComponent<BoxCollider2D> ();
		detectorCollider = GetComponent<BoxCollider2D> ();
		mask = LayerMask.GetMask("Enemy", "Ally", "Corpses");
	}

	void Start()
	{
		float rot = hintImage.transform.parent.rotation.z;
		hintImage.transform.rotation = Quaternion.Euler (0, 0, rot);
	}


	void OnTriggerStay2D(Collider2D other)
	{
		if(state == doorState.Closed)
		{
			if(other.tag == "Soldier" && Vector2.Distance(transform.position, other.transform.position) <= 2)
			{
				OpenDoor();
			}

			else if(other.tag == "Player")
			{
				if(hintImage.enabled == false)
					timer += Time.deltaTime;

				if(timer >= hintTime)
				{
					hintImage.enabled = true;
				}

				if(Input.GetKeyDown(KeyCode.E))
				{
					OpenDoor();
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			hintImage.enabled = false;
			timer = 0;
		}
	}

	void OpenDoor()
	{
		audioSource.Play ();
		physicalDoorCollider.enabled = false;
		myRenderer.enabled = false;
		button.sprite = buttonSprites [0];
		state = doorState.Open;
		hintImage.enabled = false;
		timer = 0;

		Invoke ("CloseDoor", autoCloseTime);
	}

	void CloseDoor()
	{
		Collider2D[] others = Physics2D.OverlapCircleAll (transform.position, 2, mask);

		foreach(Collider2D other in others)
		{
			if(detectorCollider.IsTouching(other))
			{
				Invoke ("CloseDoor", autoCloseTime);
				return;
			}
		}
		audioSource.Play ();
		physicalDoorCollider.enabled = true;
		myRenderer.enabled = true;
		button.sprite = buttonSprites [1];
		state = doorState.Closed;
	}

	void HackDoor()
	{
		//play hack sound
		physicalDoorCollider.enabled = true;
		myRenderer.enabled = true;
		button.sprite = buttonSprites [2];
		state = doorState.Hacked;
	}
}
