using UnityEngine;
using System.Collections;

public class Medkit : MonoBehaviour {

	public AudioClip clip;
	public float healthAmount = 25;


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
			if(other.GetComponent<Health>().health >= other.GetComponent<Health>().maxHealth)
			{
				return;
			}
			AudioSource.PlayClipAtPoint(clip, transform.position, 0.5f);

			other.GetComponent<Health>().health += healthAmount * Random.Range(0.5f, 2);

			Destroy(gameObject);
		}
	}
}
