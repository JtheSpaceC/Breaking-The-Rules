using UnityEngine;
using System.Collections;

public class Syringe : MonoBehaviour {

	public AudioClip clip;
	public float adrenalineAmount = 25;


	void Start()
	{
		Invoke ("ColliderOn", 1);
	}

	void ColliderOn()
	{
		this.GetComponent<BoxCollider2D> ().enabled = true;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			AudioSource.PlayClipAtPoint(clip, transform.position, 1f);

			RPGelements.rpgElements.adrenalineFound += (int)(adrenalineAmount * Random.Range(0.5f, 2));

			Destroy(gameObject);
		}
	}
}
