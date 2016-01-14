using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupZone : MonoBehaviour {

	float timer;
	
	public float hintTime = 3;
	public SpriteRenderer hintImage;
	public Sprite openDoor;
	public AudioClip doorOpenNoise;
	public List<GameObject> myLoot;
	[Tooltip("out of 100")]public float dropChance = 50;

	public GameObject greenOutline;


	void Start()
	{
		float rot = hintImage.transform.parent.rotation.z;
		hintImage.transform.rotation = Quaternion.Euler (0, 0, rot);
		hintImage.enabled = false;
	}

	void OnTriggerStay2D(Collider2D other)
	{
			
		if(other.tag == "Player")
		{
			if(hintImage.enabled == false)
				timer += Time.deltaTime;
			
			if(timer >= hintTime)
			{
				hintImage.enabled = true;
			}
			
			if(Input.GetKeyDown(KeyCode.E))
			{
				Open();
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
	
	void Open()
	{
		AudioSource.PlayClipAtPoint(doorOpenNoise, transform.position);
		hintImage.enabled = false;
		timer = 0;	
		GetComponentInParent<SpriteRenderer> ().sprite = openDoor;
		DropLoot ();
		Destroy (greenOutline);
		Destroy (gameObject);
	}

	void DropLoot()
	{
		foreach(GameObject loot in myLoot)
		{
			float roll = Random.Range(0, 100);
			if(roll > dropChance)
				continue;

			GameObject drop = GameObject.FindGameObjectWithTag("GameController").GetComponent<_MANAGER>().getLootPrefab(loot.name);
			Vector2 pos = (Vector2)transform.position + (Random.insideUnitCircle * 0.5f);
			Instantiate(drop, pos, Quaternion.identity);
		}
	}
}