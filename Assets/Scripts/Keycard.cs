using UnityEngine;
using System.Collections;

public class Keycard : MonoBehaviour {

	public AudioClip clip;

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
			RPGelements.instance.hasKey = true;
			AudioSource.PlayClipAtPoint(clip, transform.position);
			Destroy(gameObject.transform.parent.gameObject);
		}
	}
}
